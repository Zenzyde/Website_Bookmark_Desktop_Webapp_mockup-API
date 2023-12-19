using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Website_Bookmark_Desktop_App_API.Data;
using Website_Bookmark_Desktop_App_API.Dto;
using Website_Bookmark_Desktop_App_API.Models;

namespace Website_Bookmark_Desktop_App_API.Service
{
    public class DesktopBookmarkService : IDesktopBookmarkService
    {
		private readonly IMapper mapper;
		private readonly DataContext context;
		private readonly ILogger<DesktopBookmarkService> logger;

        public DesktopBookmarkService(IMapper mapper, DataContext context, ILogger<DesktopBookmarkService> logger)
        {
            this.mapper = mapper;
            this.context = context;
            this.logger = logger;
        }

        public async Task<ServiceResponse<List<GetBookmarkDto>>> AddBookmark(AddBookmarkDto newBookmark)
        {
            ServiceResponse<List<GetBookmarkDto>> response = new ServiceResponse<List<GetBookmarkDto>>();
            DesktopBookmark bookmark = mapper.Map<DesktopBookmark>(newBookmark);
			// Database seems to be automatically incrementing the ID of bookmarks in the bookmarks database, i don't need to do it manually but might need to look at this later..
			// Update: supposedly turned off auto-increment of ID by adding [DatabaseGenerated(DatabaseGeneratedOption.None)] to the ID properties of the classes
			// -- also needed to delete the existing dbo-s in SQL server, delete the Migrations folder in Visual Studio, create a new migration using dotnet ef
			// -- and then update the database using dotnet ef

			bool dbContextNotEmpty = await context.Bookmarks.AnyAsync();
			if (dbContextNotEmpty)
			{
				bookmark.Id = await context.Bookmarks.MaxAsync(x => x.Id) + 1;
			}
			else
			{
				bookmark.Id = 0;
			}

			EntityEntry<DesktopBookmark> dbBookmark = await context.Bookmarks.AddAsync(bookmark);
			await context.SaveChangesAsync();
            response.Data = mapper.Map<List<GetBookmarkDto>>(await context.Bookmarks.ToListAsync());
            response.Success = true;
            response.Message = "OK";
            return response;
        }

		public async Task<ServiceResponse<List<GetCollectionDto>>> AddCollection(AddCollectionDto newCollection)
		{
			ServiceResponse<List<GetCollectionDto>> response = new ServiceResponse<List<GetCollectionDto>>();
			BookmarkCollection collection = mapper.Map<BookmarkCollection>(newCollection);

			bool dbContextNotEmpty = await context.Collections.AnyAsync();
			if (dbContextNotEmpty)
			{
				collection.Id = await context.Collections.MaxAsync(x => x.Id) + 1;
			}
			else
			{
				collection.Id = 0;
			}

			EntityEntry<BookmarkCollection> dbCollection = await context.Collections.AddAsync(collection);
			await context.SaveChangesAsync();
			response.Data = mapper.Map<List<GetCollectionDto>>(await context.Collections.ToListAsync());
			response.Success = true;
			response.Message = "OK";
			return response;
		}

		public async Task<ServiceResponse<List<GetBookmarkDto>>> DeleteBookmark(DeleteBookmarkRequestDto deleteBookmarkRequest)
		{
			ServiceResponse<List<GetBookmarkDto>> response = new ServiceResponse<List<GetBookmarkDto>>();
			try
			{
				DesktopBookmark bookmark = await context.Bookmarks.FirstAsync(x => x.Id == deleteBookmarkRequest.Id && x.UserId == deleteBookmarkRequest.UserId);

				if (bookmark == null)
				{
					throw new Exception($"Bookmark with id '{deleteBookmarkRequest.Id}' not found.");
				}

                context.Bookmarks.Remove(bookmark);

				await context.SaveChangesAsync();

                response.Data = context.Bookmarks.Select(x => mapper.Map<GetBookmarkDto>(x)).ToList();
			}
			catch (Exception ex)
			{
				logger.LogError("Error when deleting bookmark: {0}", ex);
				throw;
			}
			return response;
		}

		public async Task<ServiceResponse<List<GetCollectionDto>>> DeleteCollection(DeleteCollectionRequestDto deleteCollectionRequest)
		{
			ServiceResponse<List<GetCollectionDto>> response = new ServiceResponse<List<GetCollectionDto>>();
			try
			{
				BookmarkCollection collection = await context.Collections.FirstAsync(x => x.Id == deleteCollectionRequest.Id && x.UserId == deleteCollectionRequest.UserId);

				if (collection == null)
				{
					throw new Exception($"Bookmark collection with id '{deleteCollectionRequest.Id}' not found.");
				}

				context.Collections.Remove(collection);

				await context.SaveChangesAsync();

				response.Data = context.Collections.Select(x => mapper.Map<GetCollectionDto>(x)).ToList();
			}
			catch (Exception ex)
			{
                logger.LogError("Error when deleting collection: {0}", ex);
                throw;
            }
			return response;
		}

		public async Task<ServiceResponse<List<GetBookmarkDto>>> GetAllBookmarksInCollection(GetCollectionRequestDto getCollectionRequest)
		{
			ServiceResponse<List<GetBookmarkDto>> response = new ServiceResponse<List<GetBookmarkDto>>();
			try
			{
				BookmarkCollection collection = await context.Collections.FirstAsync(x => x.Id == getCollectionRequest.Id && x.UserId == getCollectionRequest.UserId);

				if (collection == null)
				{
					throw new Exception($"Bookmark collection with id '{getCollectionRequest.Id}' not found.");
				}

				List<DesktopBookmark> bookmarks = await context.Bookmarks.Where(x => x.CollectionID == collection.Id && x.UserId == collection.UserId).ToListAsync();

				response.Data = bookmarks.Select(x => mapper.Map<GetBookmarkDto>(x)).ToList();
			}
			catch (Exception ex)
			{
                logger.LogError("Error when getting all bookmarks in collection: {0}", ex);
                throw;
            }
			return response;
		}

		public async Task<ServiceResponse<List<GetCollectionDto>>> GetAllCollectionsInCollection(GetCollectionRequestDto getCollectionRequest)
		{
			ServiceResponse<List<GetCollectionDto>> response = new ServiceResponse<List<GetCollectionDto>>();
			try
			{
				BookmarkCollection collection = await context.Collections.FirstAsync(x => x.Id == getCollectionRequest.Id && x.UserId == getCollectionRequest.UserId);

				if (collection == null)
				{
					throw new Exception($"Bookmark collection with id '{getCollectionRequest.Id}' not found.");
				}

				List<BookmarkCollection> childCollections = await context.Collections.Where(x => x.OwningCollectionID == collection.Id && x.UserId == collection.UserId).ToListAsync();

				response.Data = childCollections.Select(x => mapper.Map<GetCollectionDto>(x)).ToList();
			}
			catch (Exception ex)
			{
                logger.LogError("Error when getting all sub-collections in collection: {0}", ex);
                throw;
            }
			return response;
		}

		public async Task<ServiceResponse<List<GetCollectionDto>>> GetAllCollections(GetCollectionRequestDto getCollectionRequest)
		{
			ServiceResponse<List<GetCollectionDto>> response = new ServiceResponse<List<GetCollectionDto>>();
			List<BookmarkCollection> dbCollections = await context.Collections.Where(x => x.UserId == getCollectionRequest.UserId).ToListAsync();
			response.Data = dbCollections.Select(x => mapper.Map<GetCollectionDto>(x)).ToList();
			return response;
		}

		public async Task<ServiceResponse<List<GetBookmarkDto>>> GetAllDesktopBookmarks(GetBookmarkRequestDto getBookmarkRequest)
        {
            ServiceResponse<List<GetBookmarkDto>> response = new ServiceResponse<List<GetBookmarkDto>>();
            List<DesktopBookmark> dbBookmarks = await context.Bookmarks.Where(x => x.UserId == getBookmarkRequest.UserId).ToListAsync();
            response.Data = dbBookmarks.Select(x => mapper.Map<GetBookmarkDto>(x)).ToList();
            return response;
        }

		public async Task<ServiceResponse<GetCollectionDto>> GetSpecificCollection(GetCollectionRequestDto getCollectionRequest)
		{
			ServiceResponse<GetCollectionDto> response = new ServiceResponse<GetCollectionDto>();
			BookmarkCollection dbCollection = await context.Collections.FirstOrDefaultAsync(x => x.Id == getCollectionRequest.Id && x.UserId == getCollectionRequest.UserId);
			response.Data = mapper.Map<GetCollectionDto>(dbCollection);
			response.Success = true;
			response.Message = "OK";
			return response;
		}

		public async Task<ServiceResponse<GetBookmarkDto>> GetSpecificDesktopBookmark(GetBookmarkRequestDto getBookmarkRequest)
        {
            ServiceResponse<GetBookmarkDto> response = new ServiceResponse<GetBookmarkDto>();
            DesktopBookmark dbBookmark = await context.Bookmarks.FirstOrDefaultAsync(x => x.Id == getBookmarkRequest.Id && x.UserId == getBookmarkRequest.UserId);
            response.Data = mapper.Map<GetBookmarkDto>(dbBookmark);
            response.Success = true;
            response.Message = "OK";
            return response;
        }

		public async Task<ServiceResponse<GetCollectionDto>> UpdateCollection(UpdateCollectionDto updatedCollection)
		{
			ServiceResponse<GetCollectionDto> response = new ServiceResponse<GetCollectionDto>();
			try
			{
				BookmarkCollection collection = await context.Collections.FirstOrDefaultAsync(x => x.Id == updatedCollection.Id && x.UserId == updatedCollection.UserId);

				if (collection == null)
				{
					throw new Exception($"Bookmark collection with id '{updatedCollection.Id}' not found.");
				}

				collection.CollectionTitle = updatedCollection.CollectionTitle;
				collection.OwningCollectionID = updatedCollection.OwningCollectionID;

				await context.SaveChangesAsync();

				response.Data = mapper.Map<GetCollectionDto>(collection);

			}
			catch (Exception ex)
			{
                logger.LogError("Error when updating collection: {0}", ex);
                throw;
            }
			return response;
		}

		public async Task<ServiceResponse<GetBookmarkDto>> UpdateDesktopBookmark(UpdateBookmarkDto updatedBookmark)
		{
			ServiceResponse<GetBookmarkDto> response = new ServiceResponse<GetBookmarkDto>();
            try
            {
				DesktopBookmark bookmark = await context.Bookmarks.FirstOrDefaultAsync(x => x.Id == updatedBookmark.Id && x.UserId == updatedBookmark.UserId);

                if (bookmark == null)
                {
                    throw new Exception($"Bookmark with id '{updatedBookmark.Id}' not found.");
                }

				bookmark.BookmarkURL = updatedBookmark.BookmarkURL;
				bookmark.URLTitle = updatedBookmark.URLTitle;
				bookmark.CollectionID = updatedBookmark.CollectionID;

				await context.SaveChangesAsync();

				response.Data = mapper.Map<GetBookmarkDto>(bookmark);

			}
            catch (Exception ex)
            {
                logger.LogError("Error when updating bookmark: {0}", ex);
                throw;
            }
			return response;
		}
	}
}
