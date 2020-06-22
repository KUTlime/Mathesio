using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiscussionWeb;

using AutoMapper;

namespace DiscussionWeb.Profiles
{
	public class AuthorsProfile : Profile
	{
		public AuthorsProfile()
		{
			CreateMap<Data.Models.Author, Models.DTO.AuthorDto>()
				.ForMember(dest => dest.Name, opt => opt.MapFrom(src => $"{src.LastName} {src.FirstName}"));
		}
	}
}
