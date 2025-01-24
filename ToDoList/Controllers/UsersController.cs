using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ToDoList.Models;
using Microsoft.AspNetCore.Identity;

namespace ToDoList.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IConfiguration conf;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenServices _tokenService;
        public readonly UserManager<AppUser> _userManager;

        public UsersController(UserManager<AppUser> userManager, IConfiguration configration, SignInManager<AppUser> signInManager, ITokenServices tokenService)
        {
            _userManager = userManager;
            conf = configration;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }



        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto register)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var user = new AppUser
                {
                    UserName = register.UserName,
                    Email = register.Email,
                };

                var createUser = await _userManager.CreateAsync(user, register.Password);

                if (createUser.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(user, "User");
                    if (roleResult.Succeeded)
                    {
                        return Ok(_tokenService.CreatToken(user));
                    }
                    else
                    {
                        return StatusCode(500, roleResult.Errors);
                    }
                }
                else
                {
                    return StatusCode(500, createUser.Errors);
                }


            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException?.Message);
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto Login)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    ModelState.AddModelError(string.Empty, error.ErrorMessage);
                }
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByNameAsync(Login.UserName);

            if (user != null)
            {


                var resualt = await _signInManager.CheckPasswordSignInAsync(user, Login.Password, false);
                if (resualt.Succeeded)
                {
                    return Ok(_tokenService.CreatToken(user));
                }
            }
            return Unauthorized(new { Message = "Invalid username or password" });
        }

    }
}
