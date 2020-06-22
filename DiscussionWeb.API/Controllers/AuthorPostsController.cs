using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiscussionWeb.Data.Services;
using DiscussionWeb.Helpers;
using DiscussionWeb.Models.DTO;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace DiscussionWeb.API.Controllers
{
	[ApiController]
	[Route("api/authorposts")]
	public class AuthorPostsController : ControllerBase
	{
		private readonly IDiscussionWebRepository _discussionWebRepository;
		private readonly IMapper _mapper;

		public AuthorPostsController(IDiscussionWebRepository discussionWebRepository,
			IMapper mapper)
		{
			_discussionWebRepository = discussionWebRepository ?? throw new ArgumentNullException(nameof(discussionWebRepository));
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
		}

		[HttpGet("({ids})", Name = "GetAuthorCollection")]
		public IActionResult GetAuthorCollection([FromRoute][ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
		{
			if (ids == null)
			{
				return BadRequest();
			}

			var authorEntities = _discussionWebRepository.GetAuthors(ids);

			if (ids.Count() != authorEntities.Count())
			{
				return NotFound();
			}

			var authorsToReturn = _mapper.Map<IEnumerable<AuthorDto>>(authorEntities);

			return Ok(authorsToReturn);
		}


		[HttpPost]
		public ActionResult<IEnumerable<AuthorDto>> CreateAuthorCollection(IEnumerable<AuthorForCreationDto> authorCollection)
		{
			var authorEntities = _mapper.Map<IEnumerable<Data.Models.Author>>(authorCollection);
			foreach (var author in authorEntities)
			{
				_discussionWebRepository.AddAuthor(author);
			}

			_discussionWebRepository.Save();

			var authorCollectionToReturn = _mapper.Map<IEnumerable<AuthorDto>>(authorEntities);
			var idsAsString = string.Join(",", authorCollectionToReturn.Select(a => a.Id));
			return CreatedAtRoute("GetAuthorCollection", new { ids = idsAsString }, authorCollectionToReturn);
		}
	}
}