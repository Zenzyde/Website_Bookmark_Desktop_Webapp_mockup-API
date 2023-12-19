using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Website_Bookmark_Desktop_App_API.Models;

namespace Website_Bookmark_Desktop_App_API.Data
{
    public class UserDataContext : IdentityDbContext<User>
    {
        public UserDataContext(DbContextOptions<UserDataContext> options) : base(options)
        {

        }

        public DbSet<User> Users => Set<User>();
    }
}
