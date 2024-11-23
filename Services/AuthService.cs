using System.Data;
using Dapper;
using FinancialApi.Dtos;
using FinancialApi.Helpers;
using FinancialApi.Repositories;

namespace FinancialApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly AuthHelper _authHelper;

        public AuthService(IAuthRepository authRepository, AuthHelper authHelper)
        {
            _authRepository = authRepository;
            _authHelper = authHelper;
        }

        public async Task RegisterUserAsync(UserForRegistrationsDto userForRegistrationsDto)
        {
            var password = userForRegistrationsDto.Password;
            var passwordSalt = _authHelper.GetPasswordSalt();
            var passwordHash = _authHelper.GetPasswordHash(password, passwordSalt);

            var authParams = new DynamicParameters();
            authParams.Add("@Email", userForRegistrationsDto.Email, DbType.String);
            authParams.Add("@PasswordSalt", passwordSalt, DbType.Binary);
            authParams.Add("@PasswordHash", passwordHash, DbType.Binary);

            var userParams = new DynamicParameters();
            userParams.Add("@FirstName", userForRegistrationsDto.FirstName, DbType.String);
            userParams.Add("@LastName", userForRegistrationsDto.LastName, DbType.String);
            userParams.Add("@Email", userForRegistrationsDto.Email, DbType.String);

            await _authRepository.RegisterUser(authParams, userParams);
        }

        public async Task<bool> LoginUserAsync(UserForLoginDto userForLoginDto)
        {
            var userAuth = await _authRepository.GetAuthDetailsByEmail(userForLoginDto.Email);

            if (userAuth == null)
            {
                throw new Exception("User not found.");
            }

            var newHash = _authHelper.GetPasswordHash(userForLoginDto.Password, userAuth.PasswordSalt);

            for (int index = 0; index < newHash.Length; index++)
            {
                if (newHash[index] != userAuth.PasswordHash[index])
                {
                    throw new Exception("Incorrect password!");
                }
            }

            return newHash.SequenceEqual(userAuth.PasswordHash);
        }
    }
}