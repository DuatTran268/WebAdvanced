using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatBlog.Core.Contracts 
{
    public interface IPagedList
    {
        // tong so trang
        int PageCount { get; set; }


        // tong so phan tu tra ve tu truy van
        int TotalItemCount { get; } // get lay ve

        // choi so trang hien tai bat dau tu 0
        int PageIndex { get; }
        // vi tri trang hien tai bat dau tu 1
        int PageNumber { get; }
        // so luong phan tu toi da tren mot trang
        int PageSize { get; }

        // kiem tra xem co trang truoc hay ko
        bool HasPreviousPage { get; }

        // kiem tra co trang tiep theo hay khong
        bool HasNextPage { get; }

        // trang hien tai co phai la trang dau tien
        bool IsFirstPage { get; }

        // trang hien tai co phai la trang cuoi cung
        bool IsLastPage { get; }

        // thu tu cua phan tu dau trang trong truy van bat dau tu 1
        int FirstItemIndex { get; }

        // thu tu cua phan tu cuoi trang trong truy van bat dau tu 1
        int LastItemIndex { get; }





    }

    public interface IPagedList<out T> : IPagedList, IEnumerable<T>
    {
        public T this[int index] { get; }
        public int Count { get; }
    }
}


