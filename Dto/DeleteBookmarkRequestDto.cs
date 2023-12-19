namespace Website_Bookmark_Desktop_App_API.Dto
{
    public class DeleteBookmarkRequestDto
    {
        public Guid UserId { get; set; } = Guid.Empty;
        public int Id { get; set; } = -1;
    }
}
