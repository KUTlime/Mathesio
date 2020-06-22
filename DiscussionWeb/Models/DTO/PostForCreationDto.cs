using System.ComponentModel.DataAnnotations;

namespace DiscussionWeb.Models.DTO
{
	public class PostForCreationDto
	{
		[Required(ErrorMessage = "You should fill out a description.")]
		[MaxLength(2000)]
		public string Message { get; set; }
	}
}