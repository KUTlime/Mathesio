using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiscussionWeb.Data.Models
{
	public class Thread
	{
		[Key]
		public Guid Id { get; set; }

		[Required]
		[ForeignKey("AuthorId")]
		public Author Author { get; set; }

		public Guid? AuthorId { get; set; }

		[Required]
		[MaxLength(100)]
		public string Title { get; set; }

		[Required]
		[MaxLength(2000)]
		public string Description { get; set; }

		[Required]
		[DataType(DataType.DateTime)]
		public DateTime Posted { get; set; }

		[Required]
		[DataType(DataType.DateTime)]
		public DateTime LastEdited { get; set; }

		[Required]
		public UInt32 NumberOfEdits { get; set; }

		public ICollection<Post> Posts { get; set; }

		[NotMapped]
		public UInt32 NumberOfPosts => (uint)(Posts?.Count ?? 0);
	}
}