using AutoMapper;
using E_Commerce_Backend.Entity;
using E_Commerce_Backend.Model;
using E_Commerce_Backend.Services;
using EmailService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace E_Commerce_Backend.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IRegisterRepository _registerRepository;
        private readonly ILoginRepository _loginRepository;
        private readonly IConfiguration _configuration;
        private readonly IEmailSender _emailSender;
        private readonly IForgotPasswordRepository _forgotPasswordRepository;
        private readonly IValidateResetGuidRepository _validateResetGuidRepository;
        private readonly IResetPasswordRepository _resetPasswordRepository;
        private readonly IChangePasswordRepository _changePasswordRepository;
        public AccountController(
            IMapper mapper, 
            IRegisterRepository registerRepository,
            ILoginRepository loginRepository,
            IConfiguration configuration,
            IEmailSender emailSender,
            IForgotPasswordRepository forgotPasswordRepository,
            IValidateResetGuidRepository validateResetGuidRepository,
            IResetPasswordRepository resetPasswordRepository,
            IChangePasswordRepository changePasswordRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _registerRepository = registerRepository ?? throw new ArgumentNullException(nameof(registerRepository));
            _loginRepository = loginRepository ?? throw new ArgumentNullException(nameof(loginRepository));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
            _forgotPasswordRepository = forgotPasswordRepository ?? throw new ArgumentNullException(nameof(forgotPasswordRepository));
            _validateResetGuidRepository = validateResetGuidRepository ?? throw new ArgumentNullException(nameof(validateResetGuidRepository));
            _resetPasswordRepository = resetPasswordRepository ?? throw new ArgumentNullException(nameof(_resetPasswordRepository));
            _changePasswordRepository = changePasswordRepository ?? throw new ArgumentNullException(nameof(changePasswordRepository));
        }

        [HttpPost("register")]
        public async Task<ActionResult<string>> Register(RegisterDto register)
        {
            if (register == null)
            {
                return BadRequest("Please provide the necessary details.");
            }

            register.password = BCrypt.Net.BCrypt.HashPassword(register.password);

            var user = _mapper.Map<Entity.User>(register);

            var userExist = await _registerRepository.UserExistAsync(user);

            if (userExist.AnyExists)
            {
                var errors = new List<string>();
                if (userExist.EmailExists) errors.Add("Email already exists.");
                if (userExist.PhonenumberExists) errors.Add("Phonenumber already exists.");

                return Conflict(string.Join(" ", errors));
            }

            await _registerRepository.RegisterUserAsync(user);

            var message = new Message(new string[] { register.email }, "Registration Successful", "You have been registered successfully.");

            await _emailSender.SendEmailAsync(message);

            return Ok(new { message = "User registered Successfully" });
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(LoginDto loginDto)
        {
            if (loginDto == null)
            {
                return BadRequest("Invalid login request");
            }

            var user = await _loginRepository.GetUserByEmail(loginDto.Email);

            if (user == null)
            {
                return BadRequest("Invalid login details");
            }

            if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
            {
                return BadRequest("Invalid username or password");
            }

            var userModel = _mapper.Map<Model.UserModelDto>(user);

            // Create a token
            var securityKey = new SymmetricSecurityKey(
                Convert.FromBase64String(_configuration["Authentication:SecretForKey"]));

            // Implementing signing credentials
            var signingCredentials = new SigningCredentials(
                securityKey, SecurityAlgorithms.HmacSha256);

            // Creating a list of claims
            var claimsForToken = new List<Claim>();
            claimsForToken.Add(new Claim(ClaimTypes.NameIdentifier, userModel.Id.ToString()));
            claimsForToken.Add(new Claim("given_name", userModel.Firstname));
            claimsForToken.Add(new Claim("family_name", userModel.Lastname));
            claimsForToken.Add(new Claim(ClaimTypes.Email, userModel.Email));

            // Creating the actual token
            var jwtSecurityToken = new JwtSecurityToken(
                _configuration["Authentication:Issuer"],
                _configuration["Authentication:Audience"],
                claimsForToken,
                DateTime.UtcNow,
                DateTime.UtcNow.AddHours(1),
                signingCredentials);

            // Newing up a handler to effectively write the token
            var tokenToReturn = new JwtSecurityTokenHandler()
                .WriteToken(jwtSecurityToken);

            return Ok(tokenToReturn);
        }

        [HttpPost("forgotpassword")]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordDto forgotPassword)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid");
            }

            var userDetails = await _forgotPasswordRepository.UserEmailExistAsync(forgotPassword.email);

            if (userDetails == null)
            {
                return BadRequest("Invalid Request");
            }

            //Generate reset token
            var resetGuid = Guid.NewGuid().ToString();

            // Save the resetGuid in the database with a timestamp
            userDetails.PasswordResetGuid = resetGuid;
            userDetails.PasswordResetGuidCreate = DateTime.UtcNow;
            userDetails.PasswordResetGuidExpiry = DateTime.UtcNow.AddMinutes(30);
            userDetails.IsUsed = false;

            // Update database 
            await _forgotPasswordRepository.UpdateUserAsync(userDetails);

            //var resetLink = Url.Action("ResetPassword", "Account", new { guid = resetGuid }, Request.Scheme);

            var resetLink = $"https://e-commerce-345fe.web.app/resetpassword?guid={resetGuid}";

            var message = new Message(new string[] { userDetails.Email }, "Forgot Password Reset Link", resetLink!);

            await _emailSender.SendEmailAsync(message);

            return Ok("A password reset link has been sent to the provided email address");

        }

        [HttpGet("validate-reset-token")] 
        public async Task<ActionResult> ValidateResetGuid([FromQuery] string guid)
        {
            if (string.IsNullOrEmpty(guid))
            {
                return BadRequest("GUID is required.");
            }

            var isValid = await _validateResetGuidRepository.ValidateResetGuidAsync(guid);

            if (isValid)
            {
                return Ok(); // GUID is valid
            }
            else
            {
                return BadRequest();
            }
            
        }

        [HttpPost("resetpassword")]
        public async Task<ActionResult<string>> ResetPassword(ResetPasswordDto resetPassword)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var resetRequest = await _resetPasswordRepository.IsGuidValidAsync(resetPassword.guid);

            if (resetRequest == null)
            {
                return BadRequest("Invalid Guid");
            }

            if (resetRequest.PasswordResetGuidExpiry < DateTime.UtcNow)
            {
                return BadRequest("Expired link");
            }

            if (resetRequest.IsUsed == true)
            {
                return BadRequest("Guid has been used already");
            }

            resetRequest.Password = BCrypt.Net.BCrypt.HashPassword(resetPassword.newPassword);

            resetRequest.IsUsed = true;

            await _forgotPasswordRepository.UpdateUserAsync(resetRequest);

            var message = new Message(new string[] { resetRequest.Email }, "Password Change Success", "Password successfully changed");

            await _emailSender.SendEmailAsync(message);

            return Ok("Password successfully changed");

        }

        [HttpPost("changepassword")]
        [Authorize]
        public async Task<ActionResult> ChangePassword(ChangePasswordDto changePassword)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid request.");
            }

            var getUserIdFromClaim = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

            var getUserFirstNameFromClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value;

            var getUserEmailFromClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            if (getUserEmailFromClaim == null)
            {
                return NotFound("User's Email is missing.");
            }

            var userDetails = await _changePasswordRepository.GetUserByIdAndEmailAsync(getUserIdFromClaim, getUserEmailFromClaim);

            if (userDetails == null)
            {
                return NotFound("User not Found");
            }

            if (!BCrypt.Net.BCrypt.Verify(changePassword.OldPassword, userDetails.Password))
            {
                return BadRequest("Current Password is incorrect.");
            }

            userDetails.Password = BCrypt.Net.BCrypt.HashPassword(changePassword.NewPassword);

            await _changePasswordRepository.UpdateUserAsync(userDetails);

            var message = new Message(new string[] { userDetails.Email }, "Password Change Successful", "Your password has been changed successfully.");
            
            await _emailSender.SendEmailAsync(message);

            return Ok("Password Successfully changed.");
        }
    }
}

