using System.ComponentModel.DataAnnotations;

using DiscussionWeb.Models.DTO;

namespace DiscussionWeb.ValidationAttributes
{
	public class ThreadTitleMustBeDifferentFromDescription : ValidationAttribute
	{
		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			var post = (ThreadForManipulationDto)validationContext.ObjectInstance;

			return post.Title == post.Description ? new ValidationResult(ErrorMessage, new[] { nameof(ThreadForManipulationDto) }) : ValidationResult.Success;
		}
	}
}