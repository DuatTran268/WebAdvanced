﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;

namespace TatBlog.Data.Seeders;

public class DataSeeder : IDataSeeder
{
    private readonly BlogDbContext _dbContext;

    public DataSeeder(BlogDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public void Initialize()
    {
        _dbContext.Database.EnsureCreated();

        // kiểm tra xem nếu có database trong sql rồi thì ko thêm nữa, return ra khỏi hàm luôn
        if (_dbContext.Posts.Any()) return;

        var authors = AddAuthors();
        var categories = AddCategories();
        var tags = AddTags();
        var posts = AddPosts(authors, categories, tags);
    }

    
    private IList<Author> AddAuthors()
    {
        var authors = new List<Author>() {
            new()
            {
                FullNames = "Jason Mouth",
                UrlSlug = "jason-mouth",
                Email = "json@gmail.com",
                JoinedDate = new DateTime(2022, 10, 21)
            },
            new()
            {
                FullNames = "Jessica Wonder",
                UrlSlug = "jessica-wonder",
                Email = "jessica665@gmail.com",
                JoinedDate = new DateTime(2020, 4, 19)
            },
            new()
            {
                FullNames = "Leo Messi",
                UrlSlug = "messi",
                Email = "messipsg10@gmail.com",
                JoinedDate = new DateTime(2022, 8, 19)
            },
            new()
            {
                FullNames = "Cristiano Ronadldo",
                UrlSlug = "cristiano ronaldo",
                Email = "ronaldo7@gmail.com",
                JoinedDate = new DateTime(2022, 8, 7)
            },
            new()
            {
                FullNames = "Neymar Jr",
                UrlSlug = "neymar jr",
                Email = "neymarjs11@gmail.com",
                JoinedDate = new DateTime(2023, 8, 6)
            }
        };
        _dbContext.Authors.AddRange(authors);
        _dbContext.SaveChanges();

        return authors;
    }

    private IList<Category> AddCategories()
    {
        var categories = new List<Category>() {
            new()
            {
                Name = ".NET Core",
                Description = ".NET Core",
                UrlSlug = "net-corer"
            },
            new()
            {
                Name = "Architecture",
                Description = "Architecture",
                UrlSlug = "architecture"
            },
            new()
            {
                Name = "Messaging",
                Description = "Messaging",
                UrlSlug = "messaging"
            },
            new()
            {
                Name = "OOP",
                Description = "OOP",
                UrlSlug = "oop"
            },
            new()
            {
                Name = "Design Patterns",
                Description = "Design Patterns",
                UrlSlug = "design patterns"
            },
            new()
            {
                Name = "Javascript",
                Description = "Javascript Language",
                UrlSlug = "javascript language"
            },
            new()
            {
                Name = "C++",
                Description = "C++ Language",
                UrlSlug = "c pluss pluss language"
            },
            new()
            {
                Name = "Python",
                Description = "python is traslate con tran",
                UrlSlug = "python python"
            }
            ,
            new()
            {
                Name = "React",
                Description = "Reactjs is a framework",
                UrlSlug = "reactjs reactjs"
            },
            new()
            {
                Name = "ABC",
                Description = "ABC is a ....",
                UrlSlug = "abc abc abc"
            }
        };

        _dbContext.AddRange(categories);
        _dbContext.SaveChanges();

        return categories;
    }


    private IList<Tag> AddTags()
    {
        var tags = new List<Tag>()
        {
            new()
            {
                Name = "Google",
                Description = "Google application a",
                UrlSlug = "google"
            },
            new()
            {
                Name = "ASP .NET MVC",
                Description = "ASP .NET MVC",
                UrlSlug = "asp .net mvc"
            },
            new()
            {
                Name = "Razor Page",
                Description = "Razor Page",
                UrlSlug = "razor page"
            },
            new()
            {
                Name = "Blazor",
                Description = "Blazor",
                UrlSlug = "blazor"
            },
            new()
            {
                Name = "Deep Learning",
                Description = "Deep Learning",
                UrlSlug = "deep learning"
            },
            new()
            {
                Name = "Neural Learning",
                Description = "Neural Learning",
                UrlSlug = "neural learing"
            },
            new()
            {
                Name = "C",
                Description = "C language",
                UrlSlug = "c language"
            },
            new()
            {
                Name = "Cpp",
                Description = "C plus plus is a",
                UrlSlug = "c plus plus"
            },
            new()
            {
                Name = "abc",
                Description = "abc is a ...",
                UrlSlug = "abcacb"
            },
            new()
            {
                Name = "HTML CSS",
                Description = "HTML/CSS use web design",
                UrlSlug = "webdesign"
            },
        };

        // function update database to sql
        //foreach (var item in tags)
        //{
        //    if (!_dbContext.Tags.Any(t => t.UrlSlug == item.UrlSlug))
        //    {
        //        _dbContext.Tags.Add(item);
        //    }
        //}

        _dbContext.AddRange(tags);        // update database 
        _dbContext.SaveChanges();
        return tags;
    }


    private IList<Post> AddPosts(IList<Author> authors,
        IList<Category> categories, IList<Tag> tags)
    {
        var posts = new List<Post>()
        {
            new()
            {
                Title = "ASP .NET Core Diagnostic Scenarios",
                ShortDescrption = "David and friend has a great repository",
                Description = "Here 's a few great DON'T and DO example",
                Meta = "David and friend has a great reponsitory ",
                UrlSlug = "aspnet-core-diagnostic-scenarios",
                Published= true,
                PostedDate = new DateTime(2021, 9, 30, 10, 20 , 0),
                ModifiedDate = null,
                ViewCount = 10,
                Author = authors[0],
                Category = categories[0],
                Tags = new List<Tag>()
                {
                    tags[0]
                }
            }
            ,
            new()
            {
                Title = "Javascript",
                ShortDescrption = "Duat and friend has a great repository",
                Description = "Here 's a few great DON'T and DO example",
                Meta = "Duat and friend has a great reponsitory ",
                UrlSlug = "javascript ....",
                Published= true,
                PostedDate = new DateTime(2022, 12, 10, 2, 3 , 0),
                ModifiedDate = null,
                ViewCount = 2,
                Author = authors[0],
                Category = categories[0],
                Tags = new List<Tag>()
                {
                    tags[0]
                }
            },
            new()
            {
                Title = "HTML CSS",
                ShortDescrption = "ABC and friend has a great repository",
                Description = "Here 's a few great DON'T and DO example",
                Meta = "abc and friend has a great reponsitory ",
                UrlSlug = "html css",
                Published= true,
                PostedDate = new DateTime(2022, 2, 10, 20, 3 , 0),
                ModifiedDate = null,
                ViewCount = 1,
                Author = authors[0],
                Category = categories[0],
                Tags = new List<Tag>()
                {
                    tags[0]
                }
            },

        };

        _dbContext.AddRange(posts);
        _dbContext.SaveChanges();

        return posts;
    }
}