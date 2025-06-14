using Google.Apis.Auth;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Project_B.Data;
using Project_B.DTOs;
using Project_B.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Project_B.Interface.Implement
{
    public class UserImpl : IUserRepository
    {
        private readonly AppDbContext _context;

        private readonly IConfiguration _configuration;

        public UserImpl(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<IEnumerable<UserDTO>> GetUsersAsync()
        {
            return await _context.Users
                .Include(u => u.RoleUsers)
                    .ThenInclude(ru => ru.Role)
                .Select(u => new UserDTO(u))
                .ToListAsync();
        }

        public async Task<UserDTO> GetUserAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                throw new InvalidOperationException($"User with ID {userId} not found.");
            return new UserDTO(user);
        }

        public async Task<bool> CreateUserAsync(UserDTO userDto)
        {
            if (!IsValidEmail(userDto.Email))
                throw new ArgumentException("Invalid email format.");

            if (await _context.Users.AnyAsync(u => u.Email == userDto.Email))
                return false;

            var user = new User
            {
                Name = userDto.Name,
                Email = userDto.Email,
                Password = userDto.Password,
                IsActive = userDto.IsActive,
                IsDeleted = userDto.IsDeleted,
                CreatedTime = DateTime.UtcNow,
                Status = 1
            };

            _context.Users.Add(user);
            var result = await _context.SaveChangesAsync() > 0;

            if (result)
            {
                var roleUser = new RoleUser
                {
                    UserId = user.UserId,
                    RoleId = 2 // 2 = normal user
                };
                _context.Set<RoleUser>().Add(roleUser);
                await _context.SaveChangesAsync();
            }

            return result;
        }

        public async Task<bool> UpdateUserProfileAsync(int userId, UserSelfUpdateDTO dto)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            user.Name = dto.Name;
            user.Address = dto.Address;
            user.Phone = dto.Phone;
            user.DOB = dto.DOB;
            user.Gender = dto.Gender;

            _context.Entry(user).State = EntityState.Modified;
            return await _context.SaveChangesAsync() > 0;
        }


        public async Task<bool> UpdateUserByAdminAsync(UserStatusUpdateDTO userStatusUpdateDTO)
        {
            var user = await _context.Users.FindAsync(userStatusUpdateDTO.UserId);
            if (user == null) return false;

            user.Status = userStatusUpdateDTO.Status;

            _context.Entry(user).State = EntityState.Modified;
            return await _context.SaveChangesAsync() > 0;
        }


        public async Task<bool> DeleteUserAsync(int userId)
        {

            var roleLinks = _context.RoleUsers.Where(ru => ru.UserId == userId);
            _context.RoleUsers.RemoveRange(roleLinks);

            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            _context.Users.Remove(user);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> RegisterUserAsync(EmailDTO registerDTO)
        {
            if (!IsValidEmail(registerDTO.Email))
                throw new ArgumentException("Invalid email format.");

            if (await _context.Users.AnyAsync(u => u.Email == registerDTO.Email))
                return false;

            var otpCode = GenerateOtpCode();
            var user = new User
            {
                Name = registerDTO.Email,
                Email = registerDTO.Email,
                CreatedTime = DateTime.UtcNow,
                Status = 0,
                IsActive = false,
                IsDeleted = false,
                OTPCode = otpCode
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            await SendActivationEmail(user.Email, otpCode);
            return true;
        }

        public async Task<bool> VerifyEmailAsync(OTPCodeVerifyDTO verifyEmailDTO)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.OTPCode == verifyEmailDTO.ActiveCode);
            if (user == null) return false;

            user.IsActive = true;

            // After user is activated
            var roleUser = new RoleUser
            {
                UserId = user.UserId,
                RoleId = 2 // 2 = normal user
            };
            _context.Set<RoleUser>().Add(roleUser);
            _context.RoleUsers.Add(roleUser);
            await _context.SaveChangesAsync();
            user.Status = 1;
            user.OTPCode = null;

            _context.Entry(user).State = EntityState.Modified;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ResendCodeAsync(EmailDTO resendCode)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == resendCode.Email);
            if (user == null) return false;

            var otpCode = GenerateOtpCode();
            user.OTPCode = otpCode;
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            await SendActivationEmail(user.Email, otpCode);
            return true;
        }

        public async Task<bool> SetPasswordAsync(AccountPasswordDTO setPasswordDTO)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == setPasswordDTO.Email);
            if (user == null) return false;

            user.Password = BCrypt.Net.BCrypt.HashPassword(setPasswordDTO.Password);
            _context.Entry(user).State = EntityState.Modified;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<UserDTO> LoginAsync(AccountDTO loginDTO)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginDTO.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDTO.Password, user.Password))
                return null;

            if (!user.IsActive)
                return null;

            var userDto = new UserDTO(user);
            userDto.Token = GenerateJwtToken(user);
            return userDto;
        }
        public async Task<UserDTO> GoogleLoginAsync(string idToken)
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(idToken);

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == payload.Email);

            if (user == null)
            {
                user = new User
                {
                    Email = payload.Email,
                    Name = payload.Name,
                    IsActive = true,
                    CreatedTime = DateTime.UtcNow,
                    Status = 1,
                    Password = null
                };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                var roleUser = new RoleUser
                {
                    UserId = user.UserId,
                    RoleId = 2 // normal user
                };
                _context.Set<RoleUser>().Add(roleUser);
                await _context.SaveChangesAsync();
            }
            else
            {
                if (!user.IsActive)
                    throw new Exception("Tài khoản đã bị khóa.");

            }

            user = await _context.Users.FirstOrDefaultAsync(u => u.Email == payload.Email);

            var userDto = new UserDTO(user);
            userDto.Token = GenerateJwtToken(user);
            return userDto;
        }


        private string GenerateJwtToken(User user)
        {
            var jwtKey = _configuration["Jwt:SecretKey"];
            var jwtIssuer = _configuration["Jwt:Issuer"];
            var jwtAudience = _configuration["Jwt:Audience"];

            // Lấy role từ DB
            var role = _context.RoleUsers
                .Where(r => r.UserId == user.UserId)
                .Select(r => r.Role.RoleName)
                .FirstOrDefault() ?? "User";

            var claims = new List<Claim>
                {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim("UserId", user.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, role)
                };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtAudience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }



        public async Task<bool> ResetPasswordAsync(EmailDTO resetPasswordDTO)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == resetPasswordDTO.Email);
            if (user == null || user.Status == 0) // Status 0 = banned/inactive
                return false;

            var otpCode = GenerateOtpCode();
            user.OTPCode = otpCode;
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            await SendActivationEmail(user.Email, otpCode);
            return true;
        }

        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            var trimmedEmail = email.Trim();
            var emailRegex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (!Regex.IsMatch(trimmedEmail, emailRegex)) return false;
            try
            {
                var addr = new MailAddress(trimmedEmail);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }

        private string GenerateOtpCode()
        {
            int number = RandomNumberGenerator.GetInt32(0, 1_000_000);
            return number.ToString("D6");
        }


        private async Task SendActivationEmail(string email, string otpCode)
        {
            var fromAddress = new MailAddress("dtaminh0310@gmail.com", "Livio");
            var toAddress = new MailAddress(email);
            const string fromPassword = "";
            const string subject = "Email Verification";
            string body = $"Your activation code is: {otpCode}";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };

            using var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            };
            await smtp.SendMailAsync(message);
        }
    }
}
