using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Website_Bookmark_Desktop_App_API.Models
{
    public class DesktopBookmark
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid UserId { get; set; }
        public string BookmarkURL { get; set; } = string.Empty;
        public string URLTitle {  get; set; } = string.Empty;
        public int CollectionID { get; set; } = -1;
    }
}
