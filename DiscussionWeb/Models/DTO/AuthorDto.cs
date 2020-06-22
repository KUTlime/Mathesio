using System;

namespace DiscussionWeb.Models.DTO
{
	public class AuthorDto
	{
		public Guid Id { get; set; }

		public string Name { get; set; }

		public string NickName { get; set; }

		public DateTime Registered { get; set; }

		public string Rank { get; set; }

	}
}