using System;
using System.Collections.Generic;

using AutoMapper;

using DiscussionWeb.Data.ResourceParameters;
using DiscussionWeb.Data.Services;
using DiscussionWeb.Models.DTO;
using DiscussionWeb.ResourceParameters;

using Microsoft.AspNetCore.Mvc;

namespace DiscussionWeb.API.Controllers
{
	[ApiController]
	[Route("api/authors")]
	public class AuthorsController : ControllerBase
	{
		private readonly IDiscussionWebRepository _discussionWebRepository;
		private readonly IMapper _mapper;

		public AuthorsController(IDiscussionWebRepository discussionWebRepository, IMapper mapper)
		{
			_discussionWebRepository = discussionWebRepository ??
				throw new ArgumentNullException(nameof(discussionWebRepository));
			_mapper = mapper ??
				throw new ArgumentNullException(nameof(mapper));
		}

		[HttpGet()]
		[HttpHead]
		public ActionResult<IEnumerable<AuthorDto>> GetAuthors([FromQuery] AuthorsResourceParameters authorsResourceParameters)
		{
			var authorsFromRepo = _discussionWebRepository.GetAuthors(authorsResourceParameters);
			return Ok(_mapper.Map<IEnumerable<AuthorDto>>(authorsFromRepo));
		}

		[HttpGet("{authorId}", Name = "GetAuthor")]
		public IActionResult GetAuthor(Guid authorId)
		{
			var authorFromRepo = _discussionWebRepository.GetAuthor(authorId);

			if (authorFromRepo == null)
			{
				return NotFound();
			}

			return Ok(_mapper.Map<AuthorDto>(authorFromRepo));
		}

		[HttpPost]
		public ActionResult<AuthorDto> CreateAuthor(AuthorForCreationDto author)
		{
			var authorEntity = _mapper.Map<Data.Models.Author>(author);
			authorEntity.Registered = DateTime.UtcNow;
			_discussionWebRepository.AddAuthor(authorEntity);
			_discussionWebRepository.Save();

			var authorToReturn = _mapper.Map<AuthorDto>(authorEntity);
			return CreatedAtRoute("GetAuthor", new { authorId = authorToReturn.Id }, authorToReturn);
		}

		[HttpOptions]
		public IActionResult GetAuthorsOptions()
		{
			Response.Headers.Add("Allow", "GET,OPTIONS,POST");
			return Ok();
		}

		[HttpDelete("{authorId}")]
		public ActionResult DeleteAuthor(Guid authorId)
		{
			var authorFromRepo = _discussionWebRepository.GetAuthor(authorId);

			if (authorFromRepo == null)
			{
				return NotFound();
			}

			_discussionWebRepository.DeleteAuthor(authorFromRepo);

			_discussionWebRepository.Save();

			return NoContent();
		}
	}
}