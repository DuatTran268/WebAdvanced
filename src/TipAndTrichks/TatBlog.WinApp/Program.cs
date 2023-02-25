// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");


using TatBlog.Data.Contexts;
using TatBlog.Data.Seeders;
// csdl va trang thai cua cac doi tuong
var context = new BlogDbContext();

// tao doi tuong khoi tao du lieu mau
var seeder = new DataSeeder(context);

// goi ham initialize de nhap du lieu mau
seeder.Initialize();

    
var authors = context.Authors.ToList();

// xuat danh sach cac tac gia ra man hinh
Console.WriteLine("{0, -4} {1, -30} {2, -30} {3, 12}", "ID", "Full Name", "Email", "Joined Date");

foreach (var author in authors)
{
    Console.WriteLine("{0, -4} {1, -30} {2, -30} {3, 12:MM/dd/yyyy}",
        author.Id, author.FullNames, author.Email, author.JoinedDate);
}


