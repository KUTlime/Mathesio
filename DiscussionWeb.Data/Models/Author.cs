using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;


namespace DiscussionWeb.Data.Models
{
	public class Author
	{
		[Key]
		public Guid Id { get; set; }

		[Display(Name = "First Name")]
		[Required]
		[MaxLength(50)]
		public string FirstName { get; set; }

		[Display(Name = "Last Name")]
		[Required]
		[MaxLength(100)]
		public string LastName { get; set; }

		[Display(Name = "Nick Name")]
		[Required]
		[MaxLength(50)]
		public string NickName { get; set; }

		[Display(Name = "Registration email")]
		[Required]
		[MaxLength(100)]
		public string RegistrationEmail { get; set; }

		[Required]
		[DataType(DataType.DateTime)]
		public DateTime Registered { get; set; }

		public ICollection<Post> Posts { get; set; }

		public ICollection<Thread> Threads { get; set; }

		[Required]
		public byte Permission { get; set; }
	}
}
