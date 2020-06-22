using System;
using System.Collections.Generic;
using System.Linq;

using DiscussionWeb.Data.DbContexts;
using DiscussionWeb.Data.Models;
using DiscussionWeb.Models;
using DiscussionWeb.ResourceParameters;

namespace DiscussionWeb.Data.Services
{


	public class DiscussionWebRepository : IDiscussionWebRepository, IDisposable
	{
		private readonly DiscussionWebDbContext _context;

		public DiscussionWebRepository(DiscussionWebDbContext context)
		{
			_context = context ?? throw new ArgumentNullException(nameof(context));
		}

		public void AddPost(Guid authorId, Post post)
		{
			if (authorId == Guid.Empty)
			{
				throw new ArgumentException("AuthorID can't be empty when you adding a post.", nameof(authorId));
			}

			if (post == null)
			{
				throw new ArgumentNullException(nameof(post), "You can't add null as a post.");
			}

			// always set the AuthorId to the passed-in authorId
			post.AuthorId = authorId;
			_context.Posts.Add(post);
		}

		public void DeletePost(Post post)
		{
			_context.Posts.Remove(post);
		}


		public Post GetPost(Guid authorId, Guid postId)
		{
			if (authorId == Guid.Empty)
			{
				throw new ArgumentException("AuthorID can't be empty when you getting a post.", nameof(authorId));
			}

			if (postId == Guid.Empty)
			{
				throw new ArgumentNullException(nameof(postId), "Post ID can't be null when you are looking a specific post.");
			}

			return _context.Posts.FirstOrDefault(c => c.AuthorId == authorId && c.Id == postId);
		}

		public IEnumerable<Post> GetPosts(Guid authorId)
		{
			if (authorId == Guid.Empty)
			{
				throw new ArgumentException("AuthorID can't be empty when you getting posts for an author.", nameof(authorId));
			}
			return _context.Posts.Where(c => c.AuthorId == authorId).OrderBy(c => c.Posted).ToList();
		}

		public void UpdatePost(Post post)
		{
			// no code in this implementation
		}

		public void AddAuthor(Author author)
		{
			if (author == null)
			{
				throw new ArgumentNullException(nameof(author), "You can't add null as an author.");
			}

			// the repository fills the id (instead of using identity columns)
			author.Id = Guid.NewGuid();

			foreach (var post in author.Posts)
			{
				post.Id = Guid.NewGuid();
			}

			_context.Authors.Add(author);
		}

		public bool AuthorExists(Guid authorId)
		{
			if (authorId == Guid.Empty)
			{
				throw new ArgumentNullException(nameof(authorId), "Author ID can't be null when you are looking for an author.");
			}

			return _context.Authors.Any(a => a.Id == authorId);
		}

		public void DeleteAuthor(Author author)
		{
			if (author == null)
			{
				throw new ArgumentNullException(nameof(author), "You really can't delete null as an author.");
			}

			_context.Authors.Remove(author);
		}

		public Author GetAuthor(Guid authorId)
		{
			if (authorId == Guid.Empty)
			{
				throw new ArgumentNullException(nameof(authorId), "Author ID can't be null when you are looking for an author.");
			}

			return _context.Authors.FirstOrDefault(a => a.Id == authorId);
		}

		public IEnumerable<Author> GetAuthors()
		{
			return _context.Authors.ToList<Author>();
		}

		public IEnumerable<Author> GetAuthors(AuthorsResourceParameters authorsResourceParameters)
		{
			if (authorsResourceParameters == null)
			{
				throw new ArgumentNullException(nameof(authorsResourceParameters), "Can't map author's resource parameters.");
			}

			if (authorsResourceParameters.AccessLevel == AccessLevel.BasicUser
				 && string.IsNullOrWhiteSpace(authorsResourceParameters.SearchQuery))
			{
				return GetAuthors();
			}

			var collection = _context.Authors as IQueryable<Author>;

			if (authorsResourceParameters.AccessLevel != AccessLevel.BasicUser)
			{
				collection = collection.Where(a => a.Permission == authorsResourceParameters.AccessLevel);
			}

			if (!string.IsNullOrWhiteSpace(authorsResourceParameters.SearchQuery))
			{

				var searchQuery = authorsResourceParameters.SearchQuery.Trim();
				collection = collection.Where(a => a.NickName.Contains(searchQuery)
					|| a.FirstName.Contains(searchQuery)
					|| a.LastName.Contains(searchQuery)
					|| a.NickName.Contains(searchQuery));
			}

			return collection.ToList();
		}

		public IEnumerable<Author> GetAuthors(IEnumerable<Guid> authorIds)
		{
			if (authorIds == null)
			{
				throw new ArgumentNullException(nameof(authorIds));
			}

			return _context.Authors.Where(a => authorIds.Contains(a.Id))
				.OrderBy(a => a.LastName)
				.ThenBy(a => a.FirstName)
				.ToList();
		}

		public void UpdateAuthor(Author author)
		{
			// no code in this implementation
		}

		public bool Save()
		{
			return (_context.SaveChanges() >= 0);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				// dispose resources when needed
			}
		}
	}
}