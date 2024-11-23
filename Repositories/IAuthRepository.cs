using Dapper;
using FinancialApi.Dtos;

namespace FinancialApi.Repositories
{
    public interface IAuthRepository
    {
        Task RegisterUser(DynamicParameters authParams, DynamicParameters userParams);
        Task<UserForLoginConfirmationDto?> GetAuthDetailsByEmail(string email);
    }
}