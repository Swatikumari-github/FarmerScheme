using Microsoft.AspNetCore.Mvc;
using BusinessLayer.Interface;
using ModelLayer.DTO;

namespace FarmScheme.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserBL _userBL;

        public AuthController(IUserBL userBL)
        {
            _userBL = userBL;
        }

          [HttpPost("CreateUser")]
        public IActionResult Create()
       {
        return Ok("success usercreated");
       }

        /// <summary>
        /// User Registration Endpoint
        /// </summary>
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterDTO registerDTO)
        {
            if (registerDTO == null)
                return BadRequest("Invalid user data");

            bool result = _userBL.RegisterUser(registerDTO);

            if (result)
            {
                return Ok(new
                {
                    Success = true,
                    Message = "User registered successfully"
                });
            }

            return BadRequest(new
            {
                Success = false,
                Message = "User registration failed"
            });
        }

        /// <summary>
        /// User Login Endpoint
        /// </summary>
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDTO loginDTO)
        {
            if (loginDTO == null)
                return BadRequest("Invalid login data");

            string result = _userBL.LoginUser(loginDTO);

            if (result == "Invalid credentials" || result == "User not found")
            {
                return Unauthorized(new
                {
                    Success = false,
                    Message = result
                });
            }

            return Ok(new
            {
                Success = true,
                Message = "Login successful",
                Token = result
            });
        }
    }
}
