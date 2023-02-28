// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");


using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;
using TatBlog.Data.Seeders;
using TatBlog.Services.Blogs;
using TatBlog.WinApp;
// csdl va trang thai cua cac doi tuong
var context = new BlogDbContext();

//// tao doi tuong khoi tao du lieu mau
var seeder = new DataSeeder(context);

//// goi ham initialize de nhap du lieu mau
seeder.Initialize();




// ================== PART 5 ======================
//var authors = context.Authors.ToList();

//// xuat danh sach cac tac gia ra man hinh
//Console.WriteLine("{0, -4} {1, -30} {2, -30} {3, 12}", "ID", "Full Name", "Email", "Joined Date");

//foreach (var author in authors)
//{
//    Console.WriteLine("{0, -4} {1, -30} {2, -30} {3, 12:MM/dd/yyyy}",
//        author.Id, author.FullNames, author.Email, author.JoinedDate);
//}









// ===== Hien thi danh sach bai viet kem theo chuyen muc tac gia ============
// doc danh sach bai viet tu co so du lieu
// lay kem ten tac gia va chuyen muc
//var posts = context.Posts
//    .Where(p => p.Published)
//    .OrderBy(p => p.Title)
//    .Select(p => new
//    {
//        Id = p.Id,
//        Title = p.Title,
//        ViewCount = p.ViewCount,
//        PostedDate = p.PostedDate,
//        Author = p.Author.FullNames,
//        Category = p.Category.Name,
//    }).ToList();


//foreach (var post in posts)
//{
//    Console.WriteLine("ID       : {0}", post.Id);
//    Console.WriteLine("Title    : {0}", post.Title);
//    Console.WriteLine("View     : {0}", post.ViewCount);
//    Console.WriteLine("Date     : {0:MM/dd/yyyy}", post.PostedDate);
//    Console.WriteLine("Author   : {0}", post.Author);
//    Console.WriteLine("Category : {0}", post.Category);
//    Console.WriteLine("".PadRight(80, '-'));
//}





//====================== PART 6 ===============================
////tao doi tuong BlogRepository
//IBlogRepository blogRepository = new BlogRepository(context);

//// tim 3 bai viet duoc xem doc nhieu nhat
//var posts = await blogRepository.GetPopularArticlesAsync(3);

//////xuat danh sach ra man hinh
//foreach (var post in posts)
//{
//    Console.WriteLine("ID       : {0}", post.Id);
//    Console.WriteLine("Title    : {0}", post.Title);
//    Console.WriteLine("View     : {0}", post.ViewCount);
//    Console.WriteLine("Date     : {0}", post.PostedDate);
//    Console.WriteLine("Author   : {0}", post.Author.FullNames);
//    Console.WriteLine("Category : {0}", post.Category.Name);
//    Console.WriteLine("".PadRight(80, '-'));
//}






// ====================== PART 7=========================
//// tao doi tuong BlogRepository
//IBlogRepository blogRepo = new BlogRepository(context);
//// danh sach
//var categories = await blogRepo.GetCategoryAsync();

//// xuat ra man hinh
//Console.WriteLine("{0, -5}{1, -50}{2, 10}", "ID", "Name", "Count");
//foreach (var item in categories)
//{
//    Console.WriteLine("{0, -5}{1, -50}{2, 10}", item.Id, item.Name, item.PostCount);
//}








// ======================= PART 8 ========================
// tao doi tuong Blogrepository
IBlogRepository blogRepository = new BlogRepository(context);

// tao doi tuong chua tham so phan trang
var pagingParams = new PagingParams
{
    PageNumber = 1, // lay kq trang 1
    PageSize = 5,   // lay 5 mau tin
    SortColumn = "Name",        // sap xep tang dan theo ten
    SortOrder = "DESC"      // theo chieu giam dan
};


// lay danh sach tu khoa
var tagList = await blogRepository.GetPagedTageAsync(pagingParams);

// hien thi 
Console.WriteLine("{0,-5}{1,-50}{2,10}", "ID", "Name", "Count");

foreach (var item in tagList)
{
    Console.WriteLine("{0,-5}{1,-50}{2,10}", item.Id, item.Name, item.PostCount);
}

// done part all B