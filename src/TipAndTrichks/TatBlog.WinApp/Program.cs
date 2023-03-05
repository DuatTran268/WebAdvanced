﻿// See https://aka.ms/new-console-template for more information
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
//IBlogRepository blogRepository = new BlogRepository(context);

//// tao doi tuong chua tham so phan trang
//var pagingParams = new PagingParams
//{
//    PageNumber = 1, // lay kq trang 1
//    PageSize = 5,   // lay 5 mau tin
//    SortColumn = "Name",        // sap xep tang dan theo ten
//    SortOrder = "DESC"      // theo chieu giam dan
//};


//// lay danh sach tu khoa
//var tagList = await blogRepository.GetPagedTageAsync(pagingParams);

//// hien thi 
//Console.WriteLine("{0,-5}{1,-50}{2,10}", "ID", "Name", "Count");

//foreach (var item in tagList)
//{
//    Console.WriteLine("{0,-5}{1,-50}{2,10}", item.Id, item.Name, item.PostCount);
//}

// done part all B

//============================== PRATICE ======================
// test branch lab01-practice

// tạo đối tượng IblogRepository
IBlogRepository blogRepository = new BlogRepository(context);

//// a) tìm một thẻ tag theo tên định danh slug
//Console.WriteLine("a) Tim mot the tag theo dinh danh slug");
//var tagSlug = await blogRepository.GetTagSlugAsync("deep learning");
//Console.WriteLine("{0,-5}{1,-30}{2,-10}{3,30}", tagSlug.Id, tagSlug.Name, tagSlug.UrlSlug, tagSlug.Description);






// c) 
//Console.WriteLine("\n");
//var tagAttachPost = await blogRepository.GetAllTagsAttachPost();
//Console.WriteLine("{0, -5}{1, -20}{2, -30}{3,-50}", "ID", "Name", "Description","Post Count");
//foreach (var tagPost in tagAttachPost)
//{
//    Console.WriteLine("{0, -5}{1, -20}{2, -30}{3,-50}", 
//        tagPost.Id, tagPost.Name, tagPost.Description, tagPost.PostCount);
//}






//// d) xoá một thẻ theo mã cho trước
//Console.WriteLine("\nd) xoa mot the theo ma cho truoc)");
//IBlogRepository tagRepo = new BlogRepository(context);
//var tableTag = await tagRepo.GetAllTagsAttachPost();
//// đổi id để thực hiện xoá
////await tagRepo.RemoveTagById(9); 
//Console.WriteLine("\nBang sau khi xoa");
//Console.WriteLine("{0, -5}{1, -20}{2, -30}{3,-50}", "ID", "Name", "UrlSlug", "Description");
//foreach (var tags in tableTag)
//{
//    Console.WriteLine("{0, -5}{1, -20}{2, -30}{3,-50}", tags.Id, tags.Name, tags.UrlSlug, tags.Description);
//}




//// e) tìm một chuyên mục category theo tên định dang slug
//Console.WriteLine("\n e) Tim mot chuyen muc category theo dinh danh slug");
//var categorySlug = await blogRepository.GetCategoryBySlugAsync("oop");
//Console.WriteLine("{0,-5}{1,-30}{2,-10}{3,30}", categorySlug.Id, categorySlug.Name, categorySlug.UrlSlug, categorySlug.Description);





//// f) tìm một chuyên mục theo mã số cho trước
////Console.Write("Nhap vao ma so ID can tim ");      // test input find number 
////int a = Convert.ToInt32(Console.ReadLine());
//Console.WriteLine("\n e) Tim mot chuyen muc theo ma so cho truoc");
//var findCateById = await blogRepository.FindCategoryById(6);   // tìm category chuyen muc co id = 6
//Console.WriteLine("{0,-5}{1,-30}{2,-10}{3,30}", "ID", "Name", "UrlSlug", "Description");
//Console.WriteLine("{0,-5}{1,-30}{2,-10}{3,30}", 
//    findCateById.Id, 
//    findCateById.Name, 
//    findCateById.UrlSlug, 
//    findCateById.Description);






////g) thêm hoặc cập nhập một chuyên mục chủ đề
// add categories
//Category categoryAdd = new Category()
//{
//    Name = "SQL server 2019",
//    Description = "SQL server database 2019 ... test",
//    UrlSlug = "sql-server2019-test"
//};

//await blogRepository.AddOrUpdateCategory(categoryAdd);

//Console.WriteLine("\n Bang du lieu sau khi them vao ");
//var categoryTable = await blogRepository.GetCategoryAsync();
//Console.WriteLine("{0, -5}{1, -20}{2, -30}{3,-50}", "ID", "Name", "UrlSlug", "Description");
//foreach (var cateTable in categoryTable)
//{
//    Console.WriteLine("{0, -5}{1, -20}{2, -30}{3,-50}",
//        cateTable.Id,
//        cateTable.Name,
//        cateTable.UrlSlug,
//        cateTable.Description);
//}








//// h) xoá một chuyên mục theo mã số cho trước
////Console.WriteLine(await blogRepository.DeleteCategoryByID(2));
//IBlogRepository delCategory = new BlogRepository(context);
//var category = await delCategory.GetCategoryAsync();
//// đổi id để thực hiện xoá
////await delCategory.DeleteCategoryByID(2);      // thực hiện xoá ID 2
//Console.WriteLine("\nBang sau khi xoa");
//Console.WriteLine("{0, -5}{1, -20}{2, -30}{3,-50}", "ID", "Name", "UrlSlug", "Description");
//foreach (var tags in category)
//{
//    Console.WriteLine("{0, -5}{1, -20}{2, -30}{3,-50}", tags.Id, tags.Name, tags.UrlSlug, tags.Description);
//}






//// i) kiểm tra slug đã tồn tại hay chưa
Console.WriteLine("\ni) Kiem tra dinh danh slug cua mot chuyen muc da ton tai hay chua");

var check = await blogRepository.CheckIDSlugOfCategoryExist("net-corer");
if (check == true)
{
    Console.WriteLine("\n Da ton tai dinh danh slug trong chuyen muc ");
}
else
{
    Console.WriteLine("\n Chua ton tai dinh danh slug trong chuyen muc");
}