namespace E_Commerce_Backend.Services
{
    public interface IValidateResetGuidRepository
    {
        Task<bool> ValidateResetGuidAsync(string guid);
    }
}
