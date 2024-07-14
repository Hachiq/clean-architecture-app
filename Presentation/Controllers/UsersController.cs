using Application.Interfaces.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetUserById([FromRoute]Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            return Ok(user);
        }

        [HttpPut("{id}/update-contacts")]
        public async Task<ActionResult> UpdateUserContactInfo([FromRoute] Guid id, [FromBody] UserContactsRequest request)
        {
            await _userService.UpdateUserContactsAsync(id, request);
            return Ok();
        }

        [HttpPut("{id}/update-email")]
        public async Task<ActionResult> UpdateUserEmail([FromRoute] Guid id, [FromBody] UserEmailRequest request)
        {
            await _userService.UpdateUserEmailAsync(id, request);
            return Ok();
        }
    }
}
