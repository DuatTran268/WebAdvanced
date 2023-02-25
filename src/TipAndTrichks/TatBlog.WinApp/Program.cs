// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");


using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;
using TatBlog.Data.Seeders;
// csdl va trang thai cua cac doi tuong
var context = new BlogDbContext();

//// tao doi tuong khoi tao du lieu mau
//var seeder = new DataSeeder(context);

//// goi ham initialize de nhap du lieu mau
//seeder.Initialize();


//var authors = context.Authors.ToList();

//// xuat danh sach cac tac gia ra man hinh
//Console.WriteLine("{0, -4} {1, -30} {2, -30} {3, 12}", "ID", "Full Name", "Email", "Joined Date");

//foreach (var author in authors)
//{
//    Console.WriteLine("{0, -4} {1, -30} {2, -30} {3, 12:MM/dd/yyyy}",
//        author.Id, author.FullNames, author.Email, author.JoinedDate);
//}

// doc danh sach bai viet tu co so du lieu
// lay kem ten tac gia va chuyen muc
var posts = context.Posts
    .Where(p => p.Published)
    .OrderBy(p => p.Title)
    .Select(p => new
    {
        Id = p.Id,
        Title = p.Title,
        ViewCount = p.ViewCount,
        PostedDate = p.PostedDate,
        Author = p.Author.FullNames,
        Category = p.Category.Name,
    }).ToList();


foreach (var post in posts)
{
    Console.WriteLine("ID       : {0}", post.Id);
    Console.WriteLine("Title    : {0}", post.Title);
    Console.WriteLine("View     : {0}", post.ViewCount);
    Console.WriteLine("Date     : {0:MM/dd/yyyy}", post.PostedDate);
    Console.WriteLine("Author   : {0}", post.Author);
    Console.WriteLine("Category : {0}", post.Category);
    Console.WriteLine("".PadRight(80, '-'));
}

