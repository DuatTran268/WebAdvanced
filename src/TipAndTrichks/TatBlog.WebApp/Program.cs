#region Chưa tinh gọn
//using Microsoft.EntityFrameworkCore;
//using TatBlog.Data.Contexts;
//using TatBlog.Data.Seeders;
//using TatBlog.Services.Blogs;

//var builder = WebApplication.CreateBuilder(args);
//{
//    // add services requested by MVC Framework
//    builder.Services.AddControllersWithViews();
//    builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

//    // đăng ký các dịch vụ với DI containers

//    builder.Services.AddDbContext<BlogDbContext>(
//        options => options.UseSqlServer(
//        builder.Configuration.GetConnectionString("DefaultConnection")));
//    builder.Services.AddScoped<IBlogRepository, BlogRepository>();
//    builder.Services.AddScoped<IDataSeeder, DataSeeder>();

//}
//var app = builder.Build();
//{
//    // config HTTP request pipelie

//    // add middleware showed error notification 
//    if (app.Environment.IsDevelopment())
//    {
//        app.UseDeveloperExceptionPage();
//    }
//    else
//    {
//        app.UseExceptionHandler("/Blog/Error");
//        app.UseHsts();
//    }

//    // thêm middleware để chuyển hướng HTTP sang HTTPS
//    app.UseHttpsRedirection(); 

//    // thêm middleware phục vụ các yêu cầu liên quan tới các tập tin nội dung tĩnh như hình ảnh, css, ..
//    app.UseStaticFiles();

//    // thêm middleware lựa chọn endpoint phù hợp nhất
//    // để xử lý một http request
//    app.UseRouting();


//    // định nghĩa route teamplate, route constraint cho các enpoints kết hợp với các action trong các controler

//    app.MapControllerRoute(
//        name: "default",
//        pattern: "{controller=Blog}/{action=Index}/{id?}"
//        );

//	// định nghĩa route template , constraint cho các endpoint kết hợp với các action trong các controller
//	app.MapControllerRoute(
//	name: "posts-by-category",
//	pattern: "blog/category/{slug}",
//	defaults: new { controller = "Blog", action = "Category" });

//	app.MapControllerRoute(
//	  name: "posts-by-tag",
//	  pattern: "blog/tag/{slug}",
//	  defaults: new { controller = "Blog", action = "Tag" });

//	app.MapControllerRoute(
//	  name: "single-post",
//	  pattern: "blog/post/{year:int}/{month:int}/{day:int}/{slug}",
//	  defaults: new { controller = "Blog", action = "Post" });

//	app.MapControllerRoute(
//	  name: "default",
//	  pattern: "{controller=Blog}/{action=Index}/{id?}");

//}

////app.MapGet("/", () => "Hello World!");

//// them du lieu mau vao sql
//using (var scope = app.Services.CreateScope())
//{
//	var seeder = scope.ServiceProvider.GetRequiredService<IDataSeeder>();
//	seeder.Initialize();
//}

#endregion


#region Sau tinh gọn

using TatBlog.WebApp.Extensions;
using TatBlog.WebApp.Mapsters;
using TatBlog.WebApp.Validations;

var builder = WebApplication.CreateBuilder(args);
{
    builder.ConfigureMvc()
            .ConfigureServices()
            .ConfigureMapster()
            .ConfigureFluentValidation();
}

var app = builder.Build();
{
    app.UseRequestPipeline();
    app.UseBlogRouters();
    app.UseDataSeeder();
}


app.Run();


#endregion


// test push code prepare section C lab02 practice 

