using System;
using System.ComponentModel.DataAnnotations;

namespace DiscussionWeb.Models.DTO
{
	public class PostDto
	{
		public Guid Id { get; set; }

		public string Message { get; set; }

		public Guid AuthorId { get; set; }

		public DateTime Posted { get; set; }

		public DateTime LastEdited { get; set; }

		public UInt32 NumberOfEdits { get; set; }
	}
}