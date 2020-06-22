using System;
using System.Collections.Generic;

using AutoMapper;

using DiscussionWeb.Data.Models;
using DiscussionWeb.Data.Services;
using DiscussionWeb.Models.DTO;

using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace DiscussionWeb.API.Controllers
{
	[ApiController]
	[Route("api/authors/{authorId}/posts")]
	public class PostController : ControllerBase
	{
		private readonly IDiscussionWebRepository _discussionWebRepository;
		private readonly IMapper _mapper;

		public PostController(IDiscussionWebRepository discussionWebRepository, IMapper mapper)
		{
			_discussionWebRepository = discussionWebRepository ?? throw new ArgumentNullException(nameof(discussionWebRepository), "A repository can't be null!");
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper), "A mapper object can't be null. Parsing object to and from controller wouldn't be possible.");
		}

		[HttpGet]
		public ActionResult<IEnumerable<PostDto>> GetPostsForAuthor(Guid authorId)
		{
			if (!_discussionWebRepository.AuthorExists(authorId))
			{
				return NotFound();
			}

			return Ok(_mapper.Map<IEnumerable<PostDto>>(_discussionWebRepository.GetPosts(authorId)));
		}

		[HttpGet("{postId}", Name = "GetPostForAuthor")]
		public ActionResult<PostDto> GetPostForAuthor(Guid authorId, Guid postId)
		{
			if (!_discussionWebRepository.AuthorExists(authorId))
			{
				return NotFound();
			}

			var postFromAuthor = _discussionWebRepository.GetPost(authorId, postId);

			if (postFromAuthor == null)
			{
				return NotFound();
			}

			return Ok(_mapper.Map<PostDto>(postFromAuthor));
		}

		[HttpPost]
		public ActionResult<PostDto> CreatePostForAuthor(Guid authorId, PostForCreationDto post)
		{
			if (!_discussionWebRepository.AuthorExists(authorId))
			{
				return NotFound();
			}

			var postModel = _mapper.Map<Data.Models.Post>(post);
			return AddPostToRepository(ref authorId, ref postModel);
		}

		private ActionResult<PostDto> AddPostToRepository(ref Guid authorId, ref Post postModel)
		{
			postModel.Posted = DateTime.UtcNow;
			postModel.LastEdited = postModel.Posted;
			postModel.NumberOfEdits = 0;
			_discussionWebRepository.AddPost(authorId, postModel);
			_discussionWebRepository.Save();

			var postToReturn = _mapper.Map<PostDto>(postModel);
			return CreatedAtRoute("GetPostForAuthor", new { authorId, postId = postToReturn.Id }, postToReturn);
		}

		[HttpPut("{postId}")]
		public ActionResult<PostDto> UpdatePostForAuthor(Guid authorId, Guid postId, PostForUpdateDto post)
		{
			if (!_discussionWebRepository.AuthorExists(authorId))
			{
				return NotFound();
			}

			var postFromAuthorFromRepo = _discussionWebRepository.GetPost(authorId, postId);

			if (postFromAuthorFromRepo == null)
			{
				var postToAdd = _mapper.Map<Data.Models.Post>(post);
				postToAdd.Id = postId;

				return AddPostToRepository(ref authorId, ref postToAdd);
			}

			// map the entity to a PostForUpdateDto
			// apply the updated field values to that dto
			// map the PostForUpdateDto back to an entity
			return UpdatePostInRepository(ref post, ref postFromAuthorFromRepo);
		}

		[HttpPatch("{postId}")]
		public ActionResult<PostDto> PartiallyUpdatePostForAuthor(Guid authorId, Guid postId, JsonPatchDocument<PostForUpdateDto> patchDocument)
		{
			if (!_discussionWebRepository.AuthorExists(authorId))
			{
				return NotFound();
			}

			var postForAuthorFromRepo = _discussionWebRepository.GetPost(authorId, postId);

			if (postForAuthorFromRepo == null)
			{
				var postDto = new PostForUpdateDto();
				patchDocument.ApplyTo(postDto, ModelState);

				if (!TryValidateModel(postDto))
				{
					return ValidationProblem(ModelState);
				}

				var postToAdd = _mapper.Map<Data.Models.Post>(postDto);
				postToAdd.Id = postId;

				return AddPostToRepository(ref authorId, ref postToAdd);
			}

			var postToPatch = _mapper.Map<PostForUpdateDto>(postForAuthorFromRepo);
			// add validation
			patchDocument.ApplyTo(postToPatch, ModelState);

			if (!TryValidateModel(postToPatch))
			{
				return ValidationProblem(ModelState);
			}

			return UpdatePostInRepository(ref postToPatch, ref postForAuthorFromRepo);
		}

		[HttpDelete("{postId}")]
		public ActionResult DeletePostForAuthor(Guid authorId, Guid postId)
		{
			if (!_discussionWebRepository.AuthorExists(authorId))
			{
				return NotFound();
			}

			var postForAuthorFromRepo = _discussionWebRepository.GetPost(authorId, postId);

			if (postForAuthorFromRepo == null)
			{
				return NotFound();
			}

			_discussionWebRepository.DeletePost(postForAuthorFromRepo);
			_discussionWebRepository.Save();

			return NoContent();
		}

		private ActionResult<PostDto> UpdatePostInRepository(ref PostForUpdateDto post, ref Post postFromAuthorFromRepo)
		{
			_mapper.Map(post, postFromAuthorFromRepo);
			postFromAuthorFromRepo.LastEdited = DateTime.UtcNow;
			postFromAuthorFromRepo.NumberOfEdits++;
			_discussionWebRepository.UpdatePost(postFromAuthorFromRepo);
			_discussionWebRepository.Save();
			return NoContent();
		}

		public override ActionResult ValidationProblem(/*[ActionResultObjectValue]*/ ModelStateDictionary modelStateDictionary)
		{
			var options = HttpContext.RequestServices.GetRequiredService<IOptions<ApiBehaviorOptions>>();
			return (ActionResult)options.Value.InvalidModelStateResponseFactory(ControllerContext);
		}
	}
}