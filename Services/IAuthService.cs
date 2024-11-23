using FinancialApi.Dtos;

namespace FinancialApi.Services
{
    public interface IAuthService
    {
        Task RegisterUserAsync(UserForRegistrationsDto userForRegistrationsDto);
        Task<bool> LoginUserAsync(UserForLoginDto userForLoginDto);
    }
}