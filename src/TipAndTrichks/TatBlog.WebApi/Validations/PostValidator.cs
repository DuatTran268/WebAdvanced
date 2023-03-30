using FluentValidation;
using TatBlog.WebApi.Models.Post;

namespace TatBlog.WebApi.Validations
{
	public class PostValidator : AbstractValidator<PostEditModel>
	{
		public PostValidator() 
		{ 
			
		
		}
	}
}
