using AutoMapper;
using E_Commerce_Backend.Entity;
using E_Commerce_Backend.Model;
using E_Commerce_Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;

namespace E_Commerce_Backend.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;

        public CartController(
            ICartRepository cartRepository, 
            IMapper mapper)
        {
            _cartRepository = cartRepository ?? throw new ArgumentNullException(nameof(cartRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpPost("add-item")]
        [Authorize]
        public async Task<ActionResult> Cart(CartItemDto cartItem)
        {
            try
            {
                var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

                // Use userId to check if user has a cart
                var activeCart = await _cartRepository.UserActiveCartAsync(userId);

                if (activeCart == null)
                {
                    // Create a cart if there's no cart for the user and if cart.IsActive is false
                    var newCart = new Cart
                    {
                        UserId = userId,
                        IsActive = true,
                        CartItems = new List<CartItem>()
                    };

                    await _cartRepository.AddCartAsync(newCart);

                    activeCart = newCart; // Assign the new cart to activeCart
                }

                // Check if product already exists in the cart
                var existingCartItem = activeCart.CartItems.FirstOrDefault(ci => ci.AllProductId == cartItem.AllProductId);

                if (existingCartItem != null)
                {
                    // Update the quantity and amount if the product already exists
                    existingCartItem.Quantity += cartItem.Quantity;
                    existingCartItem.Amount = existingCartItem.Price * existingCartItem.Quantity;

                    // Update the CartItem
                    await _cartRepository.UpdateCartItemAsync(existingCartItem);

                    // Save the changes
                    await _cartRepository.SaveChangesAsync();

                    return Ok();
                }

                // If the product doesn't exist, create a new CartItem
                var cartItemEntity = new CartItem
                {
                    CartId = activeCart.Id,
                    AllProductId = cartItem.AllProductId,
                    Price = cartItem.Price,
                    Quantity = cartItem.Quantity,
                    Amount = cartItem.Price * cartItem.Quantity
                };

                activeCart.CartItems.Add(cartItemEntity);

                await _cartRepository.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                // Log the error for debugging purposes
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("get-cartproductdetails")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<CartItemWithProductDetailsDto>>> GetCartWithProductDetails()
        {
            var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

            if (!await _cartRepository.CartExistAsync(userId))
            {
                return NotFound("Cart does not exist");
            }

            var isActiveCart = await _cartRepository.GetCartWithoutCartItemAsync(userId);

            if (isActiveCart == null)
            {
                return Ok(new List<CartItemWithProductDetailsDto>());
            }

            //if (!await _cartRepository.CartIsActiveAsync(userId))
            //{
            //    return NotFound("Cart is not active");
            //}

            //var cartIsActive = await _cartRepository.CartIsActiveAsync(userId);

            //if (!cart.IsActive)
            //{
            //    return Ok(new List<CartItemWithProductDetailsDto>());
            //}

            var cartItemWithProductDetails = await _cartRepository.GetCartItemWithProductDetailsAsync(isActiveCart.Id);

            //if (cartItemWithProductDetails == null || !cartItemWithProductDetails.Any())
            //{
            //    return NotFound("Cart is empty");  // Custom message for empty cart
            //}

            return Ok(cartItemWithProductDetails);
        }

        [HttpDelete("remove-product/{allProductId}")]
        [Authorize]
        public async Task<ActionResult> DeleteCartItem(int allProductId)
        {
            var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

            var cart = await _cartRepository.UserActiveCartAsync(userId);

            if (cart == null)
            {
                return NotFound();
            }

            var cartItemEntity = cart.CartItems.FirstOrDefault(ci => ci.AllProductId == allProductId);

            if (cartItemEntity == null)
            {
                return NotFound();
            }

            _cartRepository.RemoveCartItemForCartAsync(cartItemEntity);

            await _cartRepository.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("remove-all-products")]
        [Authorize]
        public async Task<ActionResult> DeleteAllCartItems()
        {
            var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            
            var cart = await _cartRepository.UserActiveCartAsync(userId);

            if (cart == null)
            {
                return NotFound();
            }

            _cartRepository.RemoveAllCartItemForCartAsync(cart.Id);

            await _cartRepository.SaveChangesAsync();

            return Ok();

        }

    }
}
