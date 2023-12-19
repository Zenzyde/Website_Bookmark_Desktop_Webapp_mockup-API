using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Website_Bookmark_Desktop_App_API.Models
{
	public class BookmarkCollection
	{
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int Id { get; set; }
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public Guid UserId { get; set; }
		public string CollectionTitle { get; set; } = string.Empty;
		public int OwningCollectionID {  get; set; } = -1;
	}
}
