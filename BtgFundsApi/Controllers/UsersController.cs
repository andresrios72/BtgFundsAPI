using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BtgFundsApi.Services;
using BtgFundsApi.Models;

namespace BtgFundsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "admin")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> GetAllUsers()
        {
            var users = await _userService.GetAsync();
            return Ok(users);
        }
    }
}
