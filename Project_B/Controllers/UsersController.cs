using Microsoft.AspNetCore.Mvc;
using Project_B.DTOs;
using Project_B.Interface;
using Project_B.Models;

namespace Project_B.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userRepository.GetUsersAsync();
            return Ok(users);
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _userRepository.GetUserAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserDTO userDto)
        {
            var result = await _userRepository.CreateUserAsync(userDto);
            if (!result) return BadRequest("Failed to create user.");

            return Ok("User created successfully.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserDTO userDto)
        {
            if (id != userDto.UserId) return BadRequest();

            var result = await _userRepository.UpdateUserAsync(userDto);
            if (!result) return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userRepository.DeleteUserAsync(id);
            if (!result) return NotFound();

            return NoContent();
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] EmailDTO registerDTO)
        {
            var result = await _userRepository.RegisterUserAsync(registerDTO);
            if (!result) return BadRequest("Email is already registered.");

            return Ok("User registered successfully. Please check your email to verify your account.");
        }

        // check activecode
        [HttpPost("VerifyEmail")]
        public async Task<IActionResult> VerifyEmail([FromBody] OTPCodeVerifyDTO verifyEmailDTO)
        {
            var result = await _userRepository.VerifyEmailAsync(verifyEmailDTO);
            if (!result) return BadRequest("Invalid activation code.");

            return Ok("Email verified successfully. Your account is now active.");
        }

        // resend activation code
        [HttpPost("ResendActivationCode")]
        public async Task<IActionResult> ResendActivationCode([FromBody] EmailDTO resendCodeDTO)
        {
            var result = await _userRepository.ResendCodeAsync(resendCodeDTO);
            if (!result) return BadRequest("Failed to resend activation code.");
            return Ok("Activation code resent successfully.");
        }


        // set password
        [HttpPost("SetPassword")]
        public async Task<IActionResult> SetPassword([FromBody] AccountPasswordDTO setPasswordDTO)
        {
            var result = await _userRepository.SetPasswordAsync(setPasswordDTO);
            if (!result) return BadRequest("Failed to set password.");

            return Ok("Password set successfully.");
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] AccountDTO loginDTO)
        {
            var user = await _userRepository.LoginAsync(loginDTO);
            if (user == null) return Unauthorized("Invalid email or password.");

            return Ok(user);
        }

        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin([FromBody] string idToken)
        {
            var userDto = await _userRepository.GoogleLoginAsync(idToken);
            if (userDto == null)
                return Unauthorized("Google login failed.");

            return Ok(userDto);
        }


        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] EmailDTO resetPasswordDTO)
        {
            var result = await _userRepository.ResetPasswordAsync(resetPasswordDTO);
            if (!result) return BadRequest("The email does not exist");
            return Ok("Password reset successfully. Please check your email for the OTP.");
        }



    }
}
