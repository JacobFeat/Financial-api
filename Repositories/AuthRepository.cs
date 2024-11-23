using System.Data;
using Dapper;
using FinancialApi.Dtos;
using Microsoft.Data.SqlClient;

namespace FinancialApi.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly string _connectionString;

        public AuthRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task RegisterUser(DynamicParameters authParams, DynamicParameters userParams)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        string sqlAddAuth = @"
                            INSERT INTO FinancialAppSchema.Auth(
                                [Email],
                                [PasswordSalt], 
                                [PasswordHash]) 
                            VALUES (@Email, @PasswordSalt, @PasswordHash)
                        ";


                        await connection.ExecuteAsync(sqlAddAuth, authParams, transaction);

                        var sqlAddUser = @"
                            INSERT INTO FinancialAppSchema.Users(
                                [FirstName],
                                [LastName],
                                [Email],
                                [CreatedAt]
                            )
                            VALUES(@FirstName, @LastName, @Email, GETDATE())
                        ";

                        await connection.ExecuteAsync(sqlAddUser, userParams, transaction);

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Registration failed. Transaction rolled back.", ex);
                    }
                }
            }
        }

        public async Task<UserForLoginConfirmationDto?> GetAuthDetailsByEmail(string email)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    string sqlGetPasswordSaltAndHash = @"
                        SELECT a.PasswordSalt, a.PasswordHash 
                        FROM FinancialAppSchema.Auth a
                        WHERE a.Email = @Email
                    ";

                    var emailParam = new DynamicParameters();
                    emailParam.Add("@Email", email, DbType.String);

                    return await connection.QueryFirstOrDefaultAsync<UserForLoginConfirmationDto>(
                        sqlGetPasswordSaltAndHash, emailParam);
                }
                catch (SqlException ex)
                {
                    throw new Exception("Database error occurred while retrieving user passwords.", ex);
                }
                catch (Exception ex)
                {
                    throw new Exception("An unexpected error occurred.", ex);
                }
            }
        }
    }
}