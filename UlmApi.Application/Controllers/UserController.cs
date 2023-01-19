using UlmApi.Application.Models;
using UlmApi.Domain.Entities;
using UlmApi.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using UlmApi.Domain.Models.Queries;
using UlmApi.Application.Extensions;
using UlmApi.Domain.Entities.Enums;
using UlmApi.Domain.Models;

namespace UlmApi.Application.Controllers
{
    [Authorize]
    [Route("api/v1/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService; 
        private readonly IBaseService<Solution, int> _baseSolutionService;

        public UserController(IUserService userService, IBaseService<Solution, int> baseSolutionService)
        {
            _userService = userService;
            _baseSolutionService = baseSolutionService;
        }

        [Authorize(Policy = Policies.ADMIN)]
        [HttpPut, Route("{id}/disable-user")]
        public async Task<IActionResult> DisableUser([FromRoute] string id)
        {
            var user = await _userService.GetUserKeycloak(id);
            if(user == null)
                return NotFound("User not found.");

            await _userService.DisableUser(user);
            return NoContent();
        }

        [Authorize(Policy = Policies.ADMIN)]
        [HttpPut, Route("{id}/enable-user")]
        public async Task<IActionResult> EnableUser([FromRoute] string id)
        {
            var user = await _userService.GetUserKeycloak(id);
            if(user == null)
                return NotFound("User not found.");

            await _userService.EnableUser(user);
            return NoContent();
        }
        
        [HttpPut, Route("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            await _userService.ChangePassword(model);
            return NoContent();
        }

        [HttpGet, Route("")]
        public async Task<IActionResult> Get([FromQuery] GetUsersQuery queryParams)
        {
            return Ok(await _userService.GetUsers(queryParams));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var user = await _userService.GetUserById(id);
            if (user == null)
                return NotFound("User not found");

            return Ok(user);
        }

        [HttpGet("solutions/{id}/owner")]
        public async Task<IActionResult> GetOwnerBySolutionId(int id)
        {
            var solution = await _baseSolutionService.GetById<SolutionModel>(id);
            if (solution == null)
                return NotFound("Solution not found.");

            var owner = await _userService.GetOwnerBySolutionId(id);
            return Ok(owner);
        }

        [HttpGet, Route("owners-without-solution")]
        public async Task<IActionResult> GetOwnersWithoutSolution()
        {
            return Ok(await _userService.GetOwnersWithoutSolution());
        }

        [Authorize(Policy = Policies.ADMIN)]
        [HttpPut, Route("{id}/update-role")]
        public async Task<IActionResult> UpdateUserRole([FromRoute] string id, [FromBody] UpdateUserRoleModel model)
        {
            var user = await _userService.GetUserKeycloak(id);
            if (user == null)
                return NotFound("User not found.");
            
            if (await _userService.UserAlreadyOwnsASolution(id))
                return BadRequest("It was not possible to update the user's Role because he already owns a solution.");
            
            await _userService.UpdateUserRole(id, Enum.GetName(typeof(Role), model.Role));
            return NoContent();
        }

        [Authorize(Policy = Policies.ADMIN)]
        [HttpGet, Route("available-roles")]
        public IActionResult AvailableRoles()
        {
            return Ok(_userService.GetAvailableRoles());        
        }

    }
}
