using System.ComponentModel;

namespace Website_Bookmark_Desktop_App_API.Dto
{
    public class AddBookmarkDto
    {
        public Guid UserId { get; set; } = Guid.Empty;
        public string BookmarkURL { get; set; } = string.Empty;
        public string URLTitle { get; set; } = string.Empty;
        [DefaultValue(-1)] // Add for showing alternative default value in Swagger UI
        public int CollectionID { get; set; } = -1;
	}
}
