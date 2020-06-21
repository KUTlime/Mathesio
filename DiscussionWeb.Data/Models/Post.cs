using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiscussionWeb.Data.Models
{
	public class Post
	{
		[Key]
		public Guid Id { get; set; }

		[ForeignKey("AuthorId")]
		public Author Author { get; set; }

		[ForeignKey("ThreadId")]
		public Thread Thread { get; set; }

		public Guid? AuthorId { get; set; }

		public Guid? ThreadId { get; set; }

		[Required]
		[MaxLength(2000)]
		public string Message { get; set; }

		[Required]
		[DataType(DataType.DateTime)]
		public DateTime Posted { get; set; }

		[DisplayName("Last Edited")]
		[Required]
		[DataType(DataType.DateTime)]
		public DateTime LastEdited { get; set; }

		[DisplayName("Number of edits")]
		[Required]
		public UInt32 NumberOfEdits { get; set; }
	}
}