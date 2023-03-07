var builder = WebApplication.CreateBuilder(args);
{
    // add services requested by MVC Framework
    builder.Services.AddControllersWithViews();

}
var app = builder.Build();
{
    // config HTTP request pipelie

    // add middleware showed error notification 
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }
    else
    {
        app.UseExceptionHandler("/Blog/Error");
        app.UseHsts();
    }

    // thêm middleware để chuyển hướng HTTP sang HTTPS
    app.UseHttpsRedirection(); 

    // thêm middleware phục vụ các yêu cầu liên quan tới các tập tin nội dung tĩnh như hình ảnh, css, ..
    app.UseStaticFiles();

    // thêm middleware lựa chọn endpoint phù hợp nhất
    // để xử lý một http request
    app.UseRouting();


    // định nghĩa route teamplate, route constraint cho các enpoints kết hợp với các action trong các controler

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Blog}/{action=Index}/{id?}"
        );

}

//app.MapGet("/", () => "Hello World!");

app.Run();
