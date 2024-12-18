namespace FinancialApi.Dtos
{
    public partial class UserForRegistrationsDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public UserForRegistrationsDto()
        {
            Email ??= "";
            Password ??= "";
            ConfirmPassword ??= "";
            FirstName ??= "";
            LastName ??= "";
        }
    }
}