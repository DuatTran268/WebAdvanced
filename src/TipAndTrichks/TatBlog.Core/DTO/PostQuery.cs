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
    
    // ma chuyen muc
    public int CategoriesId { get; set; }

    // ten ki hieu chuyen muc
    public int CategoriesName { get; set; }

    // theo tu khoa keyword
    public string KeyWord { get; set; }

    // thang dang
    public int  PostedMonths { get; set; }

    // nam dang bai
    public int PostedYear { get; set; }

    public string SelectedTags { get; set; }

    public List<String> GetSelectedTags()
    {
        return (SelectedTags ?? "").Split(new[] { ',', ';', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList();
    }
}
