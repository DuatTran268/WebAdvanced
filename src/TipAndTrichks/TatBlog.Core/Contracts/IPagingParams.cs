using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatBlog.Core.Contracts;

public interface IPagingParams
{
    // so mau tin tren mot trang
    int PageSize { get; set; }

    // so trang tinh bat dau tu so 1
    int PageNumber { get; set; }
    // ten cot can sap xep
    string SortColumn { get; set; }

    // thu tu sap xep tang hay giam
    string SortOrder { get; set; }


}
