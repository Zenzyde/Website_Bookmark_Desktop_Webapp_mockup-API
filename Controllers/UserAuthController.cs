using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Website_Bookmark_Desktop_App_API.Dto;
using Website_Bookmark_Desktop_App_API.Models;
using Website_Bookmark_Desktop_App_API.Service;

namespace Website_Bookmark_Desktop_App_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAuthController : ControllerBase
    {
        private readonly IUserAuthService userAuthService;

        public UserAuthController(IUserAuthService userAuthService)
        {
            this.userAuthService = userAuthService;
        }

        [HttpPost("seedRoles"), AllowAnonymous]
        public async Task<ActionResult<ServiceResponse<List<GetUserRoleDto>>>> SeedRoles()
        {
            return Ok(await userAuthService.SeedRoles());
        }

        [HttpPut("updateRole"), Authorize(Roles = UserRoles.ADMIN)]
        public async Task<ActionResult<ServiceResponse<GetUserRoleDto>>> UpdateRole(UpdateUserRoleDto updateUserRole)
        {
            ServiceResponse<GetUserRoleDto> response = await userAuthService.UpdateUserRole(updateUserRole);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPost("register"), AllowAnonymous]
        public async Task<ActionResult<ServiceResponse<GetUserDto>>> RegisterUser(AddUserDto addUser)
        {
            ServiceResponse<GetUserDto> response = await userAuthService.RegisterUser(addUser);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPut("updateUser"), Authorize(Roles = UserRoles.USER + "," + UserRoles.ADMIN)]
        public async Task<ActionResult<ServiceResponse<GetUserDto>>> UpdateUser(UpdateUserDto updateUser)
        {
            ServiceResponse<GetUserDto> response = await userAuthService.UpdateUser(updateUser);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpDelete("deleteUser"), Authorize(Roles = UserRoles.USER + "," + UserRoles.ADMIN)]
        public async Task<ActionResult<ServiceResponse<GetUserDto>>> DeleteUser(UpdateUserDto updateUser)
        {
            ServiceResponse<GetUserDto> response = await userAuthService.DeleteUser(updateUser);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPut("login"), AllowAnonymous]
        public async Task<ActionResult<ServiceResponse<GetUserDto>>> Login(LoginDto login)
        {
            ServiceResponse<GetUserDto> response = await userAuthService.Login(login);

            if (!response.Success)
            {
                return Unauthorized(response);
            }

            return Ok(response);
        }

        [HttpPut("updatePassword"), Authorize(Roles = UserRoles.ADMIN + "," + UserRoles.USER)]
        public async Task<ActionResult<ServiceResponse<GetUserDto>>> UpdateUserPassword(UpdateUserDto updateUser)
        {
            ServiceResponse<GetUserDto> response = await userAuthService.UpdateUserPassword(updateUser);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}
