using Microsoft.EntityFrameworkCore;
using NLog.Web;
using TatBlog.Data.Contexts;
using TatBlog.Data.Seeders;
using TatBlog.Services.Blogs;
using TatBlog.Services.Media;
using TatBlog.WebApp.Middlewares;

namespace TatBlog.WebApp.Extensions;

public static class WebApplicationExtensions
{

    // them cac dinh vu duoc yeu cau boi MVC Framework
    public static WebApplicationBuilder ConfigureMvc(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllersWithViews();
        builder.Services.AddResponseCompression();
        builder.Services.AddRazorPages().AddRazorRuntimeCompilation();


        return builder;
    }

    // đăng ký các dịch vụ với DI container
    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<BlogDbContext>(
            options => options.UseSqlServer(
                builder.Configuration.GetConnectionString("DefaultConnection")));

        // media
        builder.Services.AddScoped<IMediaManager, LocalFileSystemMediaManager>();
        builder.Services.AddScoped<IBlogRepository, BlogRepository>();
        builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
        builder.Services.AddScoped<IDataSeeder, DataSeeder>();


        return builder;

    }
    // cấu hình việc sử dụng NLog
    public static WebApplicationBuilder ConfigrueNlog(this WebApplicationBuilder builder)
    {
        builder.Logging.ClearProviders();
        builder.Host.UseNLog();
        return builder;
    }



    // cau hinh HTTP Request pipeline
    public static WebApplication UseRequestPipeline(this WebApplication app)
    {
        // middleware de hien thi thong bao loi
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Blog/Error");

            // them middleware cho viec ap dung HSTS
            app.UseHsts();
        }

        // middleware de tu dong nen http response
        app.UseResponseCompression();

        // middleware chuyen huong HTTP sang HTTPS
        app.UseHttpsRedirection();

        // middleware thuc hien cac yeu cau lien quan toi tap tin noi dung tinh nhu hinh anh css
        app.UseStaticFiles();

        // middleware lua chon endpoint phu hop xu ly mot http request
        app.UseRouting();

        app.UseMiddleware<UserActivityMiddleware>();

        return app;
    }

    // them du lieu mau vao csdl
    public static IApplicationBuilder UseDataSeeder(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        try
        {
            scope.ServiceProvider.GetRequiredService<IDataSeeder>().Initialize();
        }
        catch (Exception ex)
        {
            scope.ServiceProvider.GetRequiredService<ILogger<Program>>()
                .LogError(ex, "Could not insert data into database"); // ko the chen du lieu vao database
        }

        return app;
    }




}

