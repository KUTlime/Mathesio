using System;
using System.Collections.Generic;

using DiscussionWeb.Data.Models;
using DiscussionWeb.ResourceParameters;

namespace DiscussionWeb.Data.Services
{
	public interface IDiscussionWebRepository
	{
		void AddPost(Guid authorId, Post post);

		void DeletePost(Post post);

		Post GetPost(Guid authorId, Guid postId);

		IEnumerable<Post> GetPosts(Guid authorId);

		void UpdatePost(Post post);

		void AddAuthor(Author author);

		bool AuthorExists(Guid authorId);

		void DeleteAuthor(Author author);

		Author GetAuthor(Guid authorId);

		IEnumerable<Author> GetAuthors();

		IEnumerable<Author> GetAuthors(AuthorsResourceParameters authorsResourceParameters);

		IEnumerable<Author> GetAuthors(IEnumerable<Guid> authorIds);

		void UpdateAuthor(Author author);

		bool Save();
	}
}