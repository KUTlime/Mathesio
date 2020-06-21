using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

using DiscussionWeb.Models;

namespace DiscussionWeb.Data.Models
{
	public class Author
	{
		[Key]
		public Guid Id { get; set; }

		[DisplayName("First Name")]
		[Required]
		[MaxLength(50)]
		public string FirstName { get; set; }

		[DisplayName("Last Name")]
		[Required]
		[MaxLength(100)]
		public string LastName { get; set; }

		[DisplayName("Nick Name")]
		[Required]
		[MaxLength(50)]
		public string NickName { get; set; }

		[DisplayName("Registration email")]
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
