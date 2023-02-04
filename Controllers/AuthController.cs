using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SAMSUNG_4_YOU.Repository.IRepository;
using SAMSUNG_4_YOU.ViewModels;
using System.Security.Claims;

namespace SAMSUNG_4_YOU.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        
        private readonly IAuthRepository _auth;
        public AuthController(IAuthRepository auth)
        {
            _auth = auth;
        }

        [HttpPost("login")]
        public IActionResult Login(UserLogin user)
        {

            bool confirm = _auth.LoginUser(user);
            if (confirm)
            {
                string token = _auth.GenerateToken(user, _auth.userRole,_auth.userId);
                return Ok(new { login = true, token = token,role=_auth.userRole });
            }

            return Ok(new { login =false });
        }

        [HttpPost("register")]
        public IActionResult CustomerRegistration(CustomerRegistration customer)
        {
            bool confirm = _auth.CustomerRegistration(customer);
            return Ok(new { isRegistered = confirm });
        }

        [HttpGet("getAllUsers")]
        public IActionResult GetAllUsers()
        {
            try
            {
                return Ok(new { getAllUsers = _auth.getAllUsers()});
            }
            catch (Exception)
            {
                return Ok(new {exception="Something went wrong, please try again later" });
            }
        }

    }
}
