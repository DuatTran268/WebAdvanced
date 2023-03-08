namespace TatBlog.WebApp.Extensions
{
    public static class RouteExtensions
    {
        public static IEndpointRouteBuilder UseBlogRouters(this IEndpointRouteBuilder endpoint) {

            // định nghĩa route template , constraint cho các endpoint kết hợp với các action trong các controller
            endpoint.MapControllerRoute(
            name: "posts-by-category",
            pattern: "blog/category/{slug}",
            defaults: new { controller = "Blog", action = "Category" });

            endpoint.MapControllerRoute(
              name: "posts-by-tag",
              pattern: "blog/tag/{slug}",
              defaults: new { controller = "Blog", action = "Tag" });

            endpoint.MapControllerRoute(
              name: "single-post",
              pattern: "blog/post/{year:int}/{month:int}/{day:int}/{slug}",
              defaults: new { controller = "Blog", action = "Post" });

            endpoint.MapControllerRoute(
              name: "default",
              pattern: "{controller=Blog}/{action=Index}/{id?}");

            return endpoint;
        }
    }
}
