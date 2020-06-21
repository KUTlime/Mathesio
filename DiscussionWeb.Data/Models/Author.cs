using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

using DiscussionWeb.Models;

namespace DiscussionWeb.Data.Models
{
	public class Author
	{
		[Key]
		public Guid Id { get; set; }

		[Required]
		[MaxLength(50)]
		public string FirstName { get; set; }


		[Required]
		[MaxLength(100)]
		public string LastName { get; set; }


		[Required]
		[MaxLength(50)]
		public string NickName { get; set; }


		[Required]
		[MaxLength(100)]
		public string RegistrationEmail { get; set; }

		[Required]
		[DataType(DataType.DateTime)]
		public DateTime Registered { get; set; }

		public ICollection<Post> Posts { get; set; }

		public ICollection<Thread> Threads { get; set; }

		[Required]
		public AccessLevel Permission { get; set; }
	}
}
