using Website_Bookmark_Desktop_App_API.Dto;
using Website_Bookmark_Desktop_App_API.Models;

namespace Website_Bookmark_Desktop_App_API.Service
{
    public interface IUserAuthService
    {
        Task<ServiceResponse<GetUserDto>> RegisterUser(AddUserDto addUser);
        Task<ServiceResponse<GetUserDto>> UpdateUser(UpdateUserDto updateUser);
        Task<ServiceResponse<GetUserDto>> UpdateUserPassword(UpdateUserDto updateUser);
        Task<ServiceResponse<GetUserDto>> DeleteUser(UpdateUserDto updateUser);
        Task<ServiceResponse<GetUserDto>> Login(LoginDto login);
        Task<ServiceResponse<List<GetUserRoleDto>>> SeedRoles();
        Task<ServiceResponse<GetUserRoleDto>> UpdateUserRole(UpdateUserRoleDto updateUserRole);
    }
}
