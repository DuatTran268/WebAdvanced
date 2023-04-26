﻿using Microsoft.EntityFrameworkCore;
using NLog.Web;
using TatBlog.Data.Contexts;
using TatBlog.Data.Seeders;
using TatBlog.Services.Blogs;
using TatBlog.Services.Media;
using TatBlog.Services.Timing;

namespace TatBlog.WebApi.Extensions
{
	public static class WebApplicationExtensions
	{
		public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
		{
			builder.Services.AddMemoryCache();

			builder.Services.AddDbContext<BlogDbContext>(options => options.UseSqlServer(
				builder.Configuration.GetConnectionString("DefaultConnection"
				)));

			builder.Services.AddScoped<ITimeProvider, LocalTimeProvider>();
			builder.Services.AddScoped<IMediaManager, LocalFileSystemMediaManager>();
			builder.Services.AddScoped<IBlogRepository, BlogRepository>();
			builder.Services.AddScoped<IAuthorRepository , AuthorRepository>();
			builder.Services.AddScoped<IDataSeeder , DataSeeder>();

			return builder;
		}

		public static WebApplicationBuilder ConfigureCors(this WebApplicationBuilder builder)
		{
			builder.Services.AddCors(options =>
			{
				options.AddPolicy("TatBlogApp",
					policyBuilder => policyBuilder
					.AllowAnyOrigin()
					.AllowAnyHeader()
					.AllowAnyMethod()
					);
			});

			return builder;
		}

		// cau hinh viec su dung Nlog
		public static WebApplicationBuilder ConfigureNLog(
			this WebApplicationBuilder builder)
		{
			builder.Logging.ClearProviders();
			builder.Host.UseNLog();

			return builder;
		}

		public static WebApplicationBuilder ConfigureSwaggerOpenApi(this WebApplicationBuilder builder)
		{
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();
			return builder;
		}

		public static WebApplication SetUpRequestPipeline(this WebApplication app)
		{
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();

			}

			app.UseStaticFiles();
			app.UseHttpsRedirection();
			app.UseCors("TatBlogApp");

			return app;

		}

		public static IApplicationBuilder UseDataSeeder(
		 this IApplicationBuilder app)
		{
			using var scope = app.ApplicationServices.CreateScope();
			try
			{
				scope.ServiceProvider
				  .GetRequiredService<IDataSeeder>()
				  .Initialize();
			}
			catch (Exception ex)
			{
				scope.ServiceProvider
					.GetRequiredService<ILogger<Program>>()
					.LogError(ex, "Could not insert data into database");
			}
			return app;
		}
	}
}
