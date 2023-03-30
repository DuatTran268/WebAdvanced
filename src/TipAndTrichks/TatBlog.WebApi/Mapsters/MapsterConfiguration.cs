using Mapster;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.WebApi.Models.Author;
using TatBlog.WebApi.Models.Category;
using TatBlog.WebApi.Models.Post;
using TatBlog.WebApi.Models.Tag;

namespace TatBlog.WebApi.Mapsters;

public class MapsterConfiguration : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.NewConfig<Author, AuthorDto>();
		config.NewConfig<Author, AuthorItem>()
			.Map(dest => dest.Id, src => src.Id)
			.Map(dest => dest.PostCount, src => src.Posts == null ? 0
			: src.Posts.Count);

		config.NewConfig<AuthorEditModel, Author>();

		config.NewConfig<Category, CategoryDto>();
		config.NewConfig<Category, CategoryItem>()
			.Map(dest => dest.PostCount, src => src.Posts == null ? 0
			: src.Posts.Count);
		config.NewConfig<CategoryEditModel, Category>();

		config.NewConfig<Post, PostDto>();
		config.NewConfig<Post, PostDetail>();
		config.NewConfig<PostEditModel, Post>()
			.Ignore(dest => dest.Id)
			.Ignore(dest => dest.ImageUrl)
			.Ignore(dest => dest.Tags);
		config.NewConfig<Post, PostEditModel>();

		config.NewConfig<Tag, TagDto>();
		config.NewConfig<Tag, TagItem>().Map(dest => dest.Id, src => src.Id)
			.Map(dest => dest.PostCount, src => src.Posts == null ? 0 : src.Posts.Count);
		config.NewConfig<TagEditModel, Tag>();

	}

}
