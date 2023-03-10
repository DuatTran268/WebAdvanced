using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatBlog.Core.DTO;

public class PostQuery
{
    // ma tac gia
    public int AuthorsId { get; set; }

    // slug tac gia
    public string AuthorSlug { get; set; }
    
    // ma chuyen muc
    public int CategoriesId { get; set; }

    // ten ki hieu chuyen muc
    public string CategoryName { get; set; }

    public string CategorySlug { get; set; }

    // theo tu khoa keyword
    public string Keyword { get; set; }

    // thang dang
    public int  PostedMonths { get; set; }

    // nam dang bai
    public int PostedYear { get; set; }

    public string SelectedTags { get; set; }
    public string TagSlug { get; set; }
    public bool PublishedOnly { get; set; }
    public bool NotPublished { get; set; }

    public List<String> GetSelectedTags()
    {
        return (SelectedTags ?? "").Split(new[] { ',', ';', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList();
    }
}
