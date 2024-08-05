using Microsoft.AspNetCore.Mvc;
using UserGroupApi.Services;
using UserGroupApi.Models;


namespace UserGroupApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly DbService _dbService;

        public UserController(DbService dbService)
        {
            _dbService = dbService;
        }

        [HttpPost("add-user")]
        public async Task<IActionResult> AddUser([FromBody] UserModel userModel)
        {
            await _dbService.AddUserAsync(userModel);
            return Ok();
        }

        [HttpPost("add-group")]
        public async Task<IActionResult> AddGroup([FromBody] GroupModel groupModel)
        {
            await _dbService.AddGroupAsync(groupModel);
            return Ok();
        }

        [HttpGet("available-groups/{userId}")]
        public async Task<ActionResult<List<GroupModel>>> GetAvailableGroupsForUser(int userId)
        {
            var groups = await _dbService.GetAvailableGroupsForUserAsync(userId);
            return Ok(groups);
        }

        [HttpPost("add-user-to-group")]
        public async Task<IActionResult> AddUserToGroup([FromBody] UserGroupModel model)
        {
            await _dbService.AddUserToGroupAsync(model.UserId, model.GroupId);
            return Ok();
        }

        [HttpGet("users-with-groups")]
        public async Task<ActionResult<List<UserGroupsModel>>> GetUsersWithGroups()
        {
            var usersWithGroups = await _dbService.GetUsersWithGroupsAsync();
            return Ok(usersWithGroups);
        }
    }
}
