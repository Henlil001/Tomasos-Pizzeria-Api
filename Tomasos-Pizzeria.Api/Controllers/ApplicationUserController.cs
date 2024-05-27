using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Tomasos_Pizzeria.Core;
using Tomasos_Pizzeria.Core.Interfaces;
using Tomasos_Pizzeria.Domain.DTO;

namespace Tomasos_Pizzeria.Api.Controllers
{

    [ApiController]
    public class ApplicationUserController : ControllerBase
    {
        private readonly IApplicationUserService _applicationUserService;
        private readonly ILogger<ApplicationUserController> _logger;

        public ApplicationUserController(IApplicationUserService applicationUserService, ILogger<ApplicationUserController> logger)
        {
            _applicationUserService = applicationUserService;
            _logger = logger;
        }

        [AllowAnonymous]
        [Route("api/[action]")]
        [HttpPost]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDTO login)
        {
            string token = await _applicationUserService.UserLoginAsync(login.Username, login.Password);

            return token.IsNullOrEmpty() ? Unauthorized() : Ok(new { Token = token });
        }

        [AllowAnonymous]
        [Route("api/[action]")]
        [HttpPost]
        public async Task<IActionResult> AddUserAsync(UserDTO user)
        {
            bool result = await _applicationUserService.AddUserAsync(user);

            return result is true ?
                Ok($"User: \"{user.Username}\" was created successfully") :
                BadRequest("UserName taken/Failed to create");
        }

        [Route("api/[action]")]
        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> UpgradeToPremiumAsync(string userNameToUpdate)
        {
            if (userNameToUpdate == SecretKey.AdminName)
                return BadRequest("You cant do that");

            bool result = await _applicationUserService.UpgradeToPremiumAsync(userNameToUpdate);

            return result is true ?
                Ok($"User \"{userNameToUpdate}\" upgraded to Premium") :
                BadRequest("User dosent exist/ Role was not changed.");
        }

        [Route("api/[action]")]
        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> DownGradeToRegularAsync(string userNameToDowngrade)
        {
            if (userNameToDowngrade == SecretKey.AdminName)
                return BadRequest("You cant do that");

            bool result = await _applicationUserService.DownGradeToRegularAsync(userNameToDowngrade);

            return result is true ?
                Ok($"User \"{userNameToDowngrade}\" downgraded to Regular") :
                BadRequest("User dosent exist/ Role was not changed.");
        }

        [Route("api/[action]")]
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            return Ok(await _applicationUserService.GetAllUsersWithRoles());
        }
    }
}
