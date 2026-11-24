namespace Library_Management_System.Services.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken(ApplicationUser user);
    }
}
