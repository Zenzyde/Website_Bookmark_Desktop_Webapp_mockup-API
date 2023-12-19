using AutoMapper;
using Website_Bookmark_Desktop_App_API.Dto;
using Website_Bookmark_Desktop_App_API.Models;

namespace Website_Bookmark_Desktop_App_API
{
	// Profile needed for Microsoft auto-mapper to know how to map classes to eachother
	public class AutoMapperProfile : Profile
	{
		public AutoMapperProfile()
		{
			CreateMap<DesktopBookmark, GetBookmarkDto>();
			CreateMap<AddBookmarkDto, DesktopBookmark>();
			CreateMap<BookmarkCollection, GetCollectionDto>();
			CreateMap<AddCollectionDto, BookmarkCollection>();
			CreateMap<User, GetUserDto>();
			CreateMap<AddUserDto, User>();
			CreateMap<User, GetUserRoleDto>();
		}
	}
}
