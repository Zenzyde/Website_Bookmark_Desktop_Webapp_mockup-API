using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Website_Bookmark_Desktop_App_API.Dto;
using Website_Bookmark_Desktop_App_API.Models;

namespace Website_Bookmark_Desktop_App_API.Service
{
    public class UserAuthService : IUserAuthService
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration configuration;

        private readonly IMapper mapper;
        private readonly ILogger<UserAuthService> logger;

        public UserAuthService(IMapper mapper, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, ILogger<UserAuthService> logger)
        {
            this.mapper = mapper;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.configuration = configuration;
            this.logger = logger;
        }

        public async Task<ServiceResponse<GetUserDto>> DeleteUser(UpdateUserDto updateUser)
        {
            ServiceResponse<GetUserDto> response = new ServiceResponse<GetUserDto>();
            try
            {
                User userExists = await userManager.FindByNameAsync(updateUser.UserName);

                if (userExists == null)
                {
                    response.Success = false;
                    response.Message = "User not found";
                    return response;
                }

                IList<string> currentRole = await userManager.GetRolesAsync(userExists);

                IdentityResult roleUnassignResult = await userManager.RemoveFromRolesAsync(userExists, currentRole);
                IdentityResult deleteResult = await userManager.DeleteAsync(userExists);

                if (!deleteResult.Succeeded)
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.Append("Failed to delte user because");

                    foreach (IdentityError error in deleteResult.Errors)
                    {
                        stringBuilder.Append(" ");
                        stringBuilder.Append(error.Description);
                    }

                    response.Success = false;
                    response.Message = stringBuilder.ToString();
                    return response;
                }

                response.Success = true;
                response.Message = "User deleted successfully";
                return response;
            }
            catch (Exception e)
            {
                logger.LogError("Error when deleting user: {0}", e);
                throw;
            }
        }

        public async Task<ServiceResponse<GetUserDto>> Login(LoginDto login)
        {
            ServiceResponse<GetUserDto> response = new ServiceResponse<GetUserDto>();

            try
            {
                User user = await userManager.FindByNameAsync(login.UserName);

                if (user == null)
                {
                    response.Success = false;
                    response.Message = "Invalid credentials";
                    return response;
                }

                bool isPasswordCorrect = await userManager.CheckPasswordAsync(user, login.Password);
                if (!isPasswordCorrect)
                {
                    response.Success = false;
                    response.Message = "Invalid credentials";
                    return response;
                }

                IList<string> roles = await userManager.GetRolesAsync(user);

                List<Claim> claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim("JWTID", Guid.NewGuid().ToString())
                };

                foreach (string role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                string token = GenerateJWTJSONWebToken(claims);

                response.Success = true;
                response.Message = $"Token: {token}";
                return response;
            }
            catch (Exception e)
            {
                logger.LogError("Error when logging in user: {0}", e);
                throw;
            }
        }

        public async Task<ServiceResponse<GetUserDto>> RegisterUser(AddUserDto addUser)
        {
            ServiceResponse<GetUserDto> response = new ServiceResponse<GetUserDto>();

            try
            {
                User userExists = await userManager.FindByNameAsync(addUser.UserName);

                if (userExists != null)
                {
                    response.Success = false;
                    response.Message = "Username already exists";
                    return response;
                }

                User newUser = mapper.Map<User>(addUser);
                newUser.SecurityStamp = Guid.NewGuid().ToString();

                IdentityResult creationResult = await userManager.CreateAsync(newUser, addUser.Password);

                if (!creationResult.Succeeded)
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.Append("User creation failed because");

                    foreach (IdentityError error in creationResult.Errors)
                    {
                        stringBuilder.Append(" ");
                        stringBuilder.Append(error.Description);
                    }

                    response.Success = false;
                    response.Message = stringBuilder.ToString();
                    return response;
                }

                IdentityResult roleAssignResult = await userManager.AddToRoleAsync(newUser, UserRoles.USER);
                response.Success = true;
                response.Message = "User created successfully";
                response.Data = mapper.Map<GetUserDto>(newUser);
                return response;
            }
            catch (Exception e)
            {
                logger.LogError("Error when registering user: {0}", e);
                throw;
            }
        }

        public async Task<ServiceResponse<GetUserDto>> UpdateUser(UpdateUserDto updateUser)
        {
            ServiceResponse<GetUserDto> response = new ServiceResponse<GetUserDto>();
            try
            {
                User user = await userManager.FindByNameAsync(updateUser.UserName);

                if (user == null)
                {
                    response.Success = false;
                    response.Message = "User not found";
                    return response;
                }

                user.UserName = updateUser.UserName;
                user.FirstName = updateUser.FirstName;
                user.LastName = updateUser.LastName;
                user.Email = updateUser.Email;
                user.Password = updateUser.Password;

                await userManager.UpdateAsync(user);

                response.Success = true;
                response.Message = "User successfully updated";
                response.Data = mapper.Map<GetUserDto>(user);
                return response;
            }
            catch (Exception e)
            {
                logger.LogError("Error when updating user: {0}", e);
                throw;
            }
        }

        // For this mock-up, i'm assuming a user can only have one role at a given time
        public async Task<ServiceResponse<GetUserRoleDto>> UpdateUserRole(UpdateUserRoleDto updateUserRole)
        {
            ServiceResponse<GetUserRoleDto> response = new ServiceResponse<GetUserRoleDto>();

            try
            {
                User userExists = await userManager.FindByNameAsync(updateUserRole.UserName);

                if (userExists == null)
                {
                    response.Success = false;
                    response.Message = "User not found";
                    return response;
                }

                IList<string> currentRole = await userManager.GetRolesAsync(userExists);

                foreach (string role in currentRole)
                {
                    if (role == updateUserRole.NewUserRole)
                    {
                        response.Success = false;
                        response.Message = $"User already has the {role} role";
                        return response;
                    }
                }

                await userManager.RemoveFromRolesAsync(userExists, currentRole);

                IdentityResult roleAssignResult = await userManager.AddToRoleAsync(userExists, updateUserRole.NewUserRole);
                response.Success = true;
                response.Message = $"Role {updateUserRole.NewUserRole} added to user";
                response.Data = mapper.Map<GetUserRoleDto>(userExists);
                return response;
            }
            catch (Exception e)
            {
                logger.LogError("Error when updating user role: {0}", e);
                throw;
            }
        }

        public async Task<ServiceResponse<List<GetUserRoleDto>>> SeedRoles()
        {
            bool usersExist = await roleManager.RoleExistsAsync(UserRoles.USER);
            bool adminsExist = await roleManager.RoleExistsAsync(UserRoles.ADMIN);
            bool ownersExist = await roleManager.RoleExistsAsync(UserRoles.OWNER);

            if (usersExist && adminsExist && ownersExist)
                return new ServiceResponse<List<GetUserRoleDto>>
                {
                    Message = "All roles already created",
                    Success = true
                };

            await roleManager.CreateAsync(new IdentityRole(UserRoles.USER));
            await roleManager.CreateAsync(new IdentityRole(UserRoles.ADMIN));
            await roleManager.CreateAsync(new IdentityRole(UserRoles.OWNER));

            return new ServiceResponse<List<GetUserRoleDto>>
            {
                Message = "All roles created successfully",
                Success = true
            };
        }

        public async Task<ServiceResponse<GetUserDto>> UpdateUserPassword(UpdateUserDto updateUser)
        {
            ServiceResponse<GetUserDto> response = new ServiceResponse<GetUserDto>();
            try
            {
                User user = await userManager.FindByNameAsync(updateUser.UserName);

                if (user == null)
                {
                    response.Success = false;
                    response.Message = "User not found";
                    return response;
                }

                await userManager.ChangePasswordAsync(user, user.Password, updateUser.Password);

                response.Success = true;
                response.Message = "User password successfully updated";
                response.Data = mapper.Map<GetUserDto>(user);
                return response;
            }
            catch (Exception e)
            {
                logger.LogError("Error when updating user password: {0}", e);
                throw;
            }
        }

        // This token is sent to a user when the user logs in, and is used to authenticate using for example: "Bearer [TOKEN]"
        private string GenerateJWTJSONWebToken(List<Claim> claims)
        {
            SymmetricSecurityKey authSecret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));

            JwtSecurityToken tokenObject = new JwtSecurityToken(
                issuer: configuration["JWT:ValidIssuer"],
                audience: configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(1),
                claims: claims,
                signingCredentials: new SigningCredentials(authSecret, SecurityAlgorithms.HmacSha256)
                );

            string token = new JwtSecurityTokenHandler().WriteToken(tokenObject);

            return token;
        }
    }
}
