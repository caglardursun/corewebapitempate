
namespace API.Contracts
{
    public enum PasswordVerificationResult
    {
        Failed = 0,
        Success = 1        
    }

    public interface IPasswordHashService
    {
        string HashPassword(string password);
        PasswordVerificationResult VerifyHashedPassword(string hashedPassword, string password);
    }
}
