using System;
using System.Collections.Generic;

namespace DiscussionWeb.Models.DTO
{
	public class AuthorForCreationDto
	{
		public string FirstName { get; set; }

		public string LastName { get; set; }

		public string NickName { get; set; }

		public string RegistrationEmail { get; set; }

		public ICollection<PostForCreationDto> Posts { get; set; } = new List<PostForCreationDto>();

	}
}