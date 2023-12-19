using Microsoft.EntityFrameworkCore;
using Website_Bookmark_Desktop_App_API.Models;

namespace Website_Bookmark_Desktop_App_API.Data
{
	public class DataContext : DbContext
	{
		public DataContext(DbContextOptions<DataContext> options) : base(options)
		{

		}

		// Representation of model in the database -- needs to be a DbSet<(type)>
		public DbSet<DesktopBookmark> Bookmarks => Set<DesktopBookmark>();
		public DbSet<BookmarkCollection> Collections => Set<BookmarkCollection>();

		//protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		//{
		//	base.OnConfiguring(optionsBuilder);
		//}

		//protected override void OnModelCreating(ModelBuilder modelBuilder)
		//{
		//	base.OnModelCreating(modelBuilder);
		//}
	}
}
