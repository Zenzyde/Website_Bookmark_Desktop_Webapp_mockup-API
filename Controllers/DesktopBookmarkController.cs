using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Website_Bookmark_Desktop_App_API.Dto;
using Website_Bookmark_Desktop_App_API.Models;
using Website_Bookmark_Desktop_App_API.Service;

namespace Website_Bookmark_Desktop_App_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = UserRoles.USER + "," + UserRoles.ADMIN + "," + UserRoles.OWNER)]
    public class DesktopBookmarkController : ControllerBase
    {
        private readonly IDesktopBookmarkService service;
        public DesktopBookmarkController(IDesktopBookmarkService service)
        {
            this.service = service;
        }

        [HttpGet("bookmarks")]
        public async Task<ActionResult<ServiceResponse<List<GetBookmarkDto>>>> GetAllDesktopBookmarks(GetBookmarkRequestDto getBookmarkRequest)
        {
            return Ok(await service.GetAllDesktopBookmarks(getBookmarkRequest));
        }

        [HttpGet("collections")]
        public async Task<ActionResult<ServiceResponse<List<GetCollectionDto>>>> GetAllCollections(GetCollectionRequestDto getCollectionRequest)
        {
            return Ok(await service.GetAllCollections(getCollectionRequest));
        }

        [HttpGet("bookmarks/{id}")]
        public async Task<ActionResult<ServiceResponse<GetBookmarkDto>>> GetSpecificDesktopBookmark(GetBookmarkRequestDto getBookmarkRequest)
        {
            if (getBookmarkRequest.Id < 0)
            {
                return NotFound();
            }
            return Ok(await service.GetSpecificDesktopBookmark(getBookmarkRequest));
        }

        [HttpGet("bookmarks/collection/{id}")]
        public async Task<ActionResult<ServiceResponse<GetBookmarkDto>>> GetAllBookmarksInCollection(GetCollectionRequestDto getCollectionRequest)
        {
			if (getCollectionRequest.Id < 0)
			{
				return NotFound();
			}
			return Ok(await service.GetAllBookmarksInCollection(getCollectionRequest));
        }

        [HttpGet("collections/{id}")]
        public async Task<ActionResult<ServiceResponse<GetCollectionDto>>> GetSpecificCollection(GetCollectionRequestDto getCollectionRequest)
        {
			if (getCollectionRequest.Id < 0)
			{
				return NotFound();
			}
			return Ok(await service.GetSpecificCollection(getCollectionRequest));
        }

        [HttpGet("collections/collection/{id}")]
        public async Task<ActionResult<ServiceResponse<GetCollectionDto>>> GetAllCollectionsInCollection(GetCollectionRequestDto getCollectionRequest)
        {
			if (getCollectionRequest.Id < 0)
			{
				return NotFound();
			}
			return Ok(await service.GetAllCollectionsInCollection(getCollectionRequest));
        }

        [HttpPost("bookmarks")]
        public async Task<ActionResult<ServiceResponse<List<GetBookmarkDto>>>> AddBookmark(AddBookmarkDto bookmark)
        {
            return Ok(await service.AddBookmark(bookmark));
        }

        [HttpPost("collections")]
        public async Task<ActionResult<ServiceResponse<List<GetCollectionDto>>>> AddCollection(AddCollectionDto collection)
        {
            return Ok(await service.AddCollection(collection));
        }

		[HttpPut("bookmarks")]
		public async Task<ActionResult<ServiceResponse<List<GetBookmarkDto>>>> UpdateBookmark(UpdateBookmarkDto bookmark)
		{
            if (bookmark.Id < 0)
            {
                return NotFound();
            }
            ServiceResponse<GetBookmarkDto> response = await service.UpdateDesktopBookmark(bookmark);
			if (response.Data == null)
            {
                return NotFound(response);
            }
            return Ok(response);
		}

        [HttpPut("collections")]
        public async Task<ActionResult<ServiceResponse<List<GetCollectionDto>>>> UpdateCollection(UpdateCollectionDto collection)
        {
			if (collection.Id < 0)
			{
				return NotFound();
			}
			ServiceResponse<GetCollectionDto> response = await service.UpdateCollection(collection);
			if (response.Data == null)
			{
				return NotFound(response);
			}
			return Ok(response);
		}

		[HttpDelete("bookmarks/{id}")]
		public async Task<ActionResult<ServiceResponse<List<GetBookmarkDto>>>> DeleteBookmark(DeleteBookmarkRequestDto deleteBookmarkRequest)
		{
			if (deleteBookmarkRequest.Id < 0)
			{
				return NotFound();
			}
			ServiceResponse<List<GetBookmarkDto>> response = await service.DeleteBookmark(deleteBookmarkRequest);
			if (response.Data == null)
			{
				return NotFound(response);
			}
			return Ok(response);
		}

        [HttpDelete("collections/{id}")]
        public async Task<ActionResult<ServiceResponse<List<GetCollectionDto>>>> DeleteCollection(DeleteCollectionRequestDto deleteCollectionRequest)
        {
			if (deleteCollectionRequest.Id < 0)
			{
				return NotFound();
			}
			ServiceResponse<List<GetCollectionDto>> response = await service.DeleteCollection(deleteCollectionRequest);
			if (response.Data == null)
			{
				return NotFound(response);
			}
			return Ok(response);
		}
	}
}
