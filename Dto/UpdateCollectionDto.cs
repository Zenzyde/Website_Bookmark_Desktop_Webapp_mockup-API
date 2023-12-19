using System.ComponentModel;
using Website_Bookmark_Desktop_App_API.Models;

namespace Website_Bookmark_Desktop_App_API.Dto
{
	public class UpdateCollectionDto
	{
		public Guid UserId { get; set; } = Guid.Empty;
		public int Id { get; set; }
		public string CollectionTitle { get; set; } = string.Empty;
		[DefaultValue(-1)] // Add for showing alternative default value in Swagger UI
		public int OwningCollectionID { get; set; } = -1;
	}
}
