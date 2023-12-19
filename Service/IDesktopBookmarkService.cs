using Website_Bookmark_Desktop_App_API.Dto;
using Website_Bookmark_Desktop_App_API.Models;

namespace Website_Bookmark_Desktop_App_API.Service
{
    public interface IDesktopBookmarkService
    {
        Task<ServiceResponse<List<GetBookmarkDto>>> GetAllDesktopBookmarks(GetBookmarkRequestDto getBookmarkRequest);
        Task<ServiceResponse<List<GetCollectionDto>>> GetAllCollections(GetCollectionRequestDto getCollectionRequest);
        Task<ServiceResponse<List<GetBookmarkDto>>> GetAllBookmarksInCollection(GetCollectionRequestDto collectionRequest);
        Task<ServiceResponse<List<GetCollectionDto>>> GetAllCollectionsInCollection(GetCollectionRequestDto collectionRequest);
        Task<ServiceResponse<GetBookmarkDto>> GetSpecificDesktopBookmark(GetBookmarkRequestDto bookmarkRequest);
        Task<ServiceResponse<GetCollectionDto>> GetSpecificCollection(GetCollectionRequestDto collectionRequest);
        Task<ServiceResponse<List<GetBookmarkDto>>> AddBookmark(AddBookmarkDto bookmark); 
        Task<ServiceResponse<List<GetCollectionDto>>> AddCollection(AddCollectionDto bookmark); 
        Task<ServiceResponse<GetBookmarkDto>> UpdateDesktopBookmark(UpdateBookmarkDto bookmark);
        Task<ServiceResponse<GetCollectionDto>> UpdateCollection(UpdateCollectionDto bookmark);
        Task<ServiceResponse<List<GetBookmarkDto>>> DeleteBookmark(DeleteBookmarkRequestDto deleteBookmarkRequest);
        Task<ServiceResponse<List<GetCollectionDto>>> DeleteCollection(DeleteCollectionRequestDto deleteCollectionRequest);
	}
}
