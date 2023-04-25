using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;

namespace TatBlog.Data.Seeders
{
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

            //if (_dbContext.Posts.Any()) return;

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
                    FullName = "Jason Mouth",
                    UrlSlug = "jason-mouth",
                    Email = "json@gmail.com",
                    JoinedDate = new DateTime(2022, 10, 21)
                },
                new()
                {
                    FullName = "Jessica Wonder",
                    UrlSlug = "jessica-wonder",
                    Email = "jessica665@gmail.com",
                    JoinedDate = new DateTime(2020, 4, 19)
                },
                new()
                {
                    FullName = "Leo Messi",
                    UrlSlug = "leo-messi",
                    Email = "messipsg10@gmail.com",
                    JoinedDate = new DateTime(2022, 3, 19)
                },
                new()
                {
                    FullName = "Cristiano Ronadldo",
                    UrlSlug = "cristiano-ronaldo",
                    Email = "ronaldo7@gmail.com",
                    JoinedDate = new DateTime(2022, 6, 7)
                },
                new()
                {
                    FullName = "Neymar Jr",
                    UrlSlug = "neymar-jr",
                    Email = "neymarjs11@gmail.com",
                    JoinedDate = new DateTime(2023, 8, 6)
                }
                ,
                new()
                {
                    FullName = "Kevin Bruyner",
                    UrlSlug = "kevin-bruyner",
                    Email = "kevinbruyner17@gmail.com",
                    JoinedDate = new DateTime(2023, 11, 16)
                }
				,
				new()
				{
					FullName = "Bill Gates",
					UrlSlug = "bill-gates",
					Email = "bill-gates26@gmail.com",
					JoinedDate = new DateTime(2023, 2, 6)
				}
				,
				new()
				{
					FullName = "Duat Tran",
					UrlSlug = "duat-tran",
					Email = "duattran36@gmail.com",
					JoinedDate = new DateTime(2023, 8, 7)
				}
			};


            // update database
            foreach (var author in authors)
            {
                if (!_dbContext.Authors.Any(a => a.UrlSlug == author.UrlSlug))
                {
                    _dbContext.Authors.Add(author);
                }
            }
            //_dbContext.Authors.AddRange(authors);
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
                UrlSlug = "design-patterns"
            },
            new()
            {
                Name = "Javascript",
                Description = "Javascript Language",
                UrlSlug = "javascript-language"
            },
            new()
            {
                Name = "C++",
                Description = "C++ Language",
                UrlSlug = "c-pluss-pluss-language"
            },
            new()
            {
                Name = "Python",
                Description = "python is traslate con tran",
                UrlSlug = "python-python"
            }
            ,
            new()
            {
                Name = "React",
                Description = "Reactjs is a framework",
                UrlSlug = "reactjs-reactjs"
            },
            new()
            {
                Name = "Java",
                Description = "Java is a language",
                UrlSlug = "java"
            }
            ,
            new()
            {
                Name = "Testing Software",
                Description = "Testing Software is a test case",
                UrlSlug = "testing-software"
            }
        };

            foreach (var catego in categories)
            {
                if (!_dbContext.Categories.Any(c => c.UrlSlug == catego.UrlSlug))
                {
                    _dbContext.Categories.Add(catego);
                }
            }
            //_dbContext.AddRange(categories);


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
                UrlSlug = "asp-net-mvc"
            },
            new()
            {
                Name = "Razor Page",
                Description = "Razor Page",
                UrlSlug = "razor-page"
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
                UrlSlug = "deep-learning"
            },
            new()
            {
                Name = "Neural Learning",
                Description = "Neural Learning",
                UrlSlug = "neural-learing"
            },
            new()
            {
                Name = "C",
                Description = "C language",
                UrlSlug = "c-language"
            },
            new()
            {
                Name = "Cpp",
                Description = "C plus plus is a",
                UrlSlug = "c-plus-plus"
            },
            new()
            {
                Name = "C#",
                Description = "C# is a",
                UrlSlug = "c-shap"
            },
            new()
            {
                Name = "HTML CSS",
                Description = "HTML/CSS use web design",
                UrlSlug = "html-css"
            },
			new()
			{
				Name = "Desgin",
				Description = "Desgin using is a",
				UrlSlug = "design"
			},
			new()
			{
				Name = "Analysis",
				Description = "analysis is a analysis data",
				UrlSlug = "analysis"
			},
			new()
			{
				Name = "Analysis Data",
				Description = "Analysis Data using is a",
				UrlSlug = "analysis-data"
			},
			new()
			{
				Name = "Desgin",
				Description = "Desgin using is a",
				UrlSlug = "design"
			},
			new()
			{
				Name = "Big Data",
				Description = "Big data is a",
				UrlSlug = "big-data"
			},
			new()
			{
				Name = "AI",
				Description = "AI is a intelligent",
				UrlSlug = "ai"
			},
			new()
			{
				Name = "DLU",
				Description = "Da Lat university",
				UrlSlug = "dlu"
			},
			new()
			{
				Name = "IT DLU",
				Description = "IT DLU department information technology",
				UrlSlug = "it-dlu"
			},
			new()
			{
				Name = "Web Advanced",
				Description = "Web Advanced hard :))",
				UrlSlug = "web-advance"
			},
			new()
			{
				Name = "Python",
				Description = "Python is a language ",
				UrlSlug = "python"
			},
			new()
			{
				Name = "Toeic",
				Description = "I learning english",
				UrlSlug = "toeic"
			},
		};

            foreach (var tag in tags)
            {
                if (!_dbContext.Tags.Any(t => t.UrlSlug == tag.UrlSlug))
                {
                    _dbContext.Tags.Add(tag);
                }
            }

           
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
                ShortDescription = "David and friend has a great repository",
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
                    tags[0], tags[1], tags[2]
                }
            }
            ,
            new()
            {
                Title = "Javascript",
                ShortDescription = "Duat and friend has a great repository",
                Description = "Here 's a few great DON'T and DO example",
                Meta = "Duat and friend has a great reponsitory ",
                UrlSlug = "javascript-language",
                Published= true,
                PostedDate = new DateTime(2022, 12, 10, 2, 3 , 0),
                ModifiedDate = null,
                ViewCount = 200,
                Author = authors[2],
                Category = categories[2],
                Tags = new List<Tag>()
                {
                   tags[0], tags[1], tags[2], tags[3]
                }
            },
            new()
            {
                Title = "HTML CSS",
                ShortDescription = "HTML CSS and friend has a great repository",
                Description = "Here 's a few great DON'T and DO example",
                Meta = "HTML CSS and friend has a great reponsitory ",
                UrlSlug = "html-css-language-mark",
                Published= true,
                PostedDate = new DateTime(2022, 2, 10, 20, 3 , 0),
                ModifiedDate = null,
                ViewCount = 100,
                Author = authors[4],
                Category = categories[3],
                Tags = new List<Tag>()
                {
                    tags[0], tags[1]
                }
            }
            ,new()
            {
                Title = "Reactjs",
                ShortDescription = "Reactjs and friend has a great repository",
                Description = "Here 's a few great reactjs framework of javascript",
                Meta = "reactjs and friend has a great reponsitory ",
                UrlSlug = "react-js-javascript",
                Published= true,
                PostedDate = new DateTime(2023, 5, 1, 20, 3 , 0),
                ModifiedDate = null,
                ViewCount = 100,
                Author = authors[6],
                Category = categories[3],
                Tags = new List<Tag>()
                {
                    tags[2], tags[8]
                }
            }

			// sửa từ đây 
			,new()
			{
				Title = "Python",
				ShortDescription = "Python and friend has a great repository",
				Description = "Python is a language program",
				Meta = "Python has a great reponsitory ",
				UrlSlug = "python",
				Published= true,
				PostedDate = new DateTime(2023, 2, 1, 12, 6 , 0),
				ModifiedDate = null,
				ViewCount = 100,
				Author = authors[8],
				Category = categories[5],
				Tags = new List<Tag>()
				{
					tags[17], tags[18]
				}
			}
			,new()
			{
				Title = "Assembly",
				ShortDescription = "Assembly has a great repository",
				Description = "Assembly example is a ",
				Meta = "Assembly great reponsitory ",
				UrlSlug = "assembly",
				Published= false,
				PostedDate = new DateTime(2021, 12, 6, 12, 03 , 0),
				ModifiedDate = null,
				ViewCount = 10,
				Author = authors[8],
				Category = categories[2],
				Tags = new List<Tag>()
				{
					tags[1], tags[2],tags[15], tags[12]
				}
			}
			,new()
			{
				Title = "PHP",
				ShortDescription = "PHP and friend has a great repository",
				Description = "PHP example is a abc ",
				Meta = "PHP has a great reponsitory ",
				UrlSlug = "php",
				Published= true,
				PostedDate = new DateTime(2023, 6, 12, 21, 13 , 0),
				ModifiedDate = null,
				ViewCount = 60,
				Author = authors[6],
				Category = categories[5],
				Tags = new List<Tag>()
				{
					tags[10], tags[11]
				}
			}
			,new()
			{
				Title = "C# C-Sharp",
				ShortDescription = "C-Sharp is ",
				Description = "C-Sharp is a language popular",
				Meta = "C-Sharp is a language popular today",
				UrlSlug = "c-sharp",
				Published= true,
				PostedDate = new DateTime(2021, 12, 11, 26, 12 , 0),
				ModifiedDate = null,
				ViewCount = 68,
				Author = authors[8],
				Category = categories[8],
				Tags = new List<Tag>()
				{
					tags[8], tags[9], tags[19]
				}
			}
			,new()
			{
				Title = "Ruby",
				ShortDescription = "Ruby is a",
				Description = "Ruby is a language program basic",
				Meta = "Ruby is a language program basic and developer",
				UrlSlug = "ruby-language",
				Published= false,
				PostedDate = new DateTime(2020, 8, 9, 28, 13 , 0),
				ModifiedDate = null,
				ViewCount = 86,
				Author = authors[6],
				Category = categories[6],
				Tags = new List<Tag>()
				{
					tags[16], tags[13], tags[20]
				}
			}
			,new()
			{
				Title = "Go",
				ShortDescription = "Go is a ",
				Description = "Go is a program language ",
				Meta = "Go is a program language development by Google",
				UrlSlug = "do",
				Published= false,
				PostedDate = new DateTime(2023, 3, 18, 4, 8 , 0),
				ModifiedDate = null,
				ViewCount = 23,
				Author = authors[7],
				Category = categories[3],
				Tags = new List<Tag>()
				{
					tags[3]
				}
			}
			,new()
			{
				Title = "C",
				ShortDescription = "C is a ",
				Description = "C is a program language",
				Meta = "C is a program language longest life span",
				UrlSlug = "c",
				Published= true,
				PostedDate = new DateTime(2021, 11, 21, 20, 3 , 0),
				ModifiedDate = null,
				ViewCount = 26,
				Author = authors[6],
				Category = categories[8],
				Tags = new List<Tag>()
				{
					tags[6], tags[8]
				}
			}
			,new()
			{
				Title = "C++",
				ShortDescription = "C plus plus",
				Description = "C plus plus is a",
				Meta = "C plus plus is a program language",
				UrlSlug = "cpp",
				Published= true,
				PostedDate = new DateTime(2022, 12, 8, 2, 11 , 0),
				ModifiedDate = null,
				ViewCount = 136,
				Author = authors[7],
				Category = categories[8],
				Tags = new List<Tag>()
				{
					tags[2], tags[3]
				}
			}
			,new()
			{
				Title = "Kotlin",
				ShortDescription = "Kotlin is a debut",
				Description = "Kotlin is a debut 2011",
				Meta = "Kotlin is a debut 2011 and using ",
				UrlSlug = "kotlin",
				Published= false,
				PostedDate = new DateTime(2023, 12, 11, 20, 3 , 0),
				ModifiedDate = null,
				ViewCount = 12,
				Author = authors[7],
				Category = categories[7],
				Tags = new List<Tag>()
				{
					tags[9]
				}
			}
			,new()
			{
				Title = "Swift",
				ShortDescription = "Swift is a ",
				Description = "Swift is a program language ",
				Meta = "Swift is a program language new",
				UrlSlug = "Swift",
				Published= true,
				PostedDate = new DateTime(2021, 9, 15, 2, 12 , 0),
				ModifiedDate = null,
				ViewCount = 23,
				Author = authors[2],
				Category = categories[1],
				Tags = new List<Tag>()
				{
					tags[4]
				}
			}
			,new()
			{
				Title = "Flutter",
				ShortDescription = "Flutter is a",
				Description = "Flutter is a program language",
				Meta = "Flutter is a program language development by Google",
				UrlSlug = "flutter",
				Published= true,
				PostedDate = new DateTime(2022, 2, 10, 20, 3 , 0),
				ModifiedDate = null,
				ViewCount = 230,
				Author = authors[8],
				Category = categories[8],
				Tags = new List<Tag>()
				{
					tags[6], tags[8]
				}
			}
			,new()
			{
				Title = "TypeScript",
				ShortDescription = "TypeScript and friend has a great repository",
				Description = "TypeScript and DO example",
				Meta = "TypeScript is a ....",
				UrlSlug = "typescript",
				Published= true,
				PostedDate = new DateTime(2023, 6, 8, 21, 3 , 0),
				ModifiedDate = null,
				ViewCount = 10,
				Author = authors[7],
				Category = categories[2],
				Tags = new List<Tag>()
				{
					tags[12]
				}
			}
			,new()
			{
				Title = "Toeic",
				ShortDescription = "Toeic is a",
				Description = "Toeic is a language",
				Meta = "I Learning Toeic",
				UrlSlug = "toeic",
				Published= true,
				PostedDate = new DateTime(2023, 6, 8, 16, 18 , 0),
				ModifiedDate = null,
				ViewCount = 260,
				Author = authors[8],
				Category = categories[6],
				Tags = new List<Tag>()
				{
					tags[19]
				}
			}
			,new()
			{
				Title = "Bootstrap",
				ShortDescription = "Bootstrap is a css framework",
				Description = "Bootstrap use desgin",
				Meta = "Bootstrap use desgin web development",
				UrlSlug = "bootstrap",
				Published= true,
				PostedDate = new DateTime(2021, 6, 9, 21, 3 , 0),
				ModifiedDate = null,
				ViewCount = 120,
				Author = authors[5],
				Category = categories[2],
				Tags = new List<Tag>()
				{
					tags[8], tags[9]
				}
			}
			,new()
			{
				Title = "Angular",
				ShortDescription = "Angular is a",
				Description = "Angular is a framework ",
				Meta = "Angular is a framework use web",
				UrlSlug = "angular",
				Published= false,
				PostedDate = new DateTime(2021, 3, 12, 20, 3 , 0),
				ModifiedDate = null,
				ViewCount = 50,
				Author = authors[2],
				Category = categories[6],
				Tags = new List<Tag>()
				{
					tags[1], tags[2], tags[4], tags[9],
				}
			}
			,new()
			{
				Title = "React Native",
				ShortDescription = "React Native alow programer ",
				Description = "React Native alow programer application",
				Meta = "React Native alow programer application mobile ",
				UrlSlug = "react-native",
				Published= false,
				PostedDate = new DateTime(2020, 2, 11, 20, 3 , 0),
				ModifiedDate = null,
				ViewCount = 80,
				Author = authors[7],
				Category = categories[5],
				Tags = new List<Tag>()
				{
					tags[0], tags[5]
				}
			}
			,new()
			{
				Title = "API",
				ShortDescription = "API is a ",
				Description = "API is a ....",
				Meta = "API is a .... use for ",
				UrlSlug = "api",
				Published= true,
				PostedDate = new DateTime(2023, 6, 3, 20, 3 , 0),
				ModifiedDate = null,
				ViewCount = 36,
				Author = authors[5],
				Category = categories[8],
				Tags = new List<Tag>()
				{
					tags[1], tags[9]
				}
			}
			,new()
			{
				Title = "API REST",
				ShortDescription = "API REST is a",
				Description = "API REST is a ...",
				Meta = "API REST is a ",
				UrlSlug = "api-rest",
				Published= false,
				PostedDate = new DateTime(2022, 3, 22, 20, 3 , 0),
				ModifiedDate = null,
				ViewCount = 56,
				Author = authors[2],
				Category = categories[10],
				Tags = new List<Tag>()
				{
					tags[8], tags[19]
				}
			}
			,new()
			{
				Title = "Programer",
				ShortDescription = "Programer Trĩ ",
				Description = "Programer is a developer application",
				Meta = "Programer is a developer application",
				UrlSlug = "programer",
				Published= false,
				PostedDate = new DateTime(2022, 2, 10, 20, 3 , 0),
				ModifiedDate = null,
				ViewCount = 38,
				Author = authors[8],
				Category = categories[10],
				Tags = new List<Tag>()
				{
					tags[3], tags[8]
				}
			}
			,new()
			{
				Title = "Infomation Technology",
				ShortDescription = "Infomation Technology is a ",
				Description = "Infomation Technology is a ... ",
				Meta = "Infomation Technology is a ...",
				UrlSlug = "it",
				Published= true,
				PostedDate = new DateTime(2023, 8, 10, 20, 3 , 0),
				ModifiedDate = null,
				ViewCount = 96,
				Author = authors[6],
				Category = categories[10],
				Tags = new List<Tag>()
				{
					tags[1], tags[6]
				}
			}
			,new()
			{
				Title = "TatBlog WebApi",
				ShortDescription = "TatBlog.WebApi is ",
				Description = "TatBlog.WebApi code very hard",
				Meta = "TatBlog.WebApi code very hard ....",
				UrlSlug = "tat-blog",
				Published= true,
				PostedDate = new DateTime(2023, 3, 6, 12, 3 , 0),
				ModifiedDate = null,
				ViewCount = 69,
				Author = authors[5],
				Category = categories[8],
				Tags = new List<Tag>()
				{
					tags[2], tags[18], 
				}
			}
			,new()
			{
				Title = "Facebook",
				ShortDescription = "Facebook and friend has a great repository",
				Description = "Facebook and DO example",
				Meta = "Facebook and friend has a great reponsitory ",
				UrlSlug = "facebook",
				Published= false,
				PostedDate = new DateTime(2021, 2, 8, 20, 3 , 0),
				ModifiedDate = null,
				ViewCount = 10,
				Author = authors[2],
				Category = categories[11],
				Tags = new List<Tag>()
				{
					tags[4], tags[9]
				}
			}
			,new()
			{
				Title = "Reactjs",
				ShortDescription = "Reactjs is a framework",
				Description = "Reactjs is a framework development web application",
				Meta = "Reactjs is a framework development web application",
				UrlSlug = "react-js",
				Published= true,
				PostedDate = new DateTime(2023, 7, 8, 20, 3 , 0),
				ModifiedDate = null,
				ViewCount = 600,
				Author = authors[8],
				Category = categories[9],
				Tags = new List<Tag>()
				{
					tags[20], tags[12], tags[18],
				}
			}
			,new()
			{
				Title = "Java",
				ShortDescription = "Java is a ",
				Description = "Java is a program language ",
				Meta = "Java is a program language development",
				UrlSlug = "java",
				Published= true,
				PostedDate = new DateTime(2023, 12, 10, 20, 3 , 0),
				ModifiedDate = null,
				ViewCount =200,
				Author = authors[2],
				Category = categories[8],
				Tags = new List<Tag>()
				{
					tags[10], tags[11]
				}
			}
			,new()
			{
				Title = "Spring boot",
				ShortDescription = "Spring boot is a ",
				Description = "Spring boot is a framework",
				Meta = "Spring boot is a framework development applications",
				UrlSlug = "spring-boot",
				Published= true,
				PostedDate = new DateTime(2022, 2, 10, 20, 3 , 0),
				ModifiedDate = null,
				ViewCount = 100,
				Author = authors[8],
				Category = categories[9],
				Tags = new List<Tag>()
				{
					tags[9], tags[12]
				}
			}
			,new()
			{
				Title = "Project",
				ShortDescription = "Specialized Project ",
				Description = "Specialized Project is a",
				Meta = "Specialized Project using .net and reactjs",
				UrlSlug = "project",
				Published= true,
				PostedDate = new DateTime(2022, 2, 10, 20, 3 , 0),
				ModifiedDate = null,
				ViewCount = 280,
				Author = authors[5],
				Category = categories[6],
				Tags = new List<Tag>()
				{
					tags[8], tags[9]
				}
			}

		};

            // update database
            foreach (var post in posts)
            {
                if (!_dbContext.Posts.Any(a => a.UrlSlug == post.UrlSlug))
                {
                    _dbContext.Posts.Add(post);
                }
            }
            //_dbContext.Authors.AddRange(authors);
            _dbContext.SaveChanges();

            return posts;

        }
    }
}


