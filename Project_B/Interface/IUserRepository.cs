using Project_B.DTOs;

namespace Project_B.Interface
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserDTO>> GetUsersAsync();
        Task<UserDTO> GetUserAsync(int userId);
        Task<bool> CreateUserAsync(UserDTO user);
        Task<bool> UpdateUserAsync(UserDTO user);
        Task<bool> DeleteUserAsync(int userId);
        Task<bool> RegisterUserAsync(EmailDTO registerDTO);
        Task<bool> VerifyEmailAsync(OTPCodeVerifyDTO verifyEmailDTO);
        Task<bool> ResendCodeAsync(EmailDTO resendCodeDTO);
        Task<bool> SetPasswordAsync(AccountPasswordDTO setPasswordDTO);
        Task<UserDTO> LoginAsync(AccountDTO loginDTO);
        Task<bool> ResetPasswordAsync(EmailDTO resetPasswordDTO);
        Task<UserDTO> GoogleLoginAsync(string idToken);
    }
}
