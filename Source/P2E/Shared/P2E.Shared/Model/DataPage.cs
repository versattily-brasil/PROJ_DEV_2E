using DapperExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace P2E.Shared.Model
{
    public class DataPage<T>
    {
        public DataPage()
        {
            Items = new List<T>();
            Pages = new List<Page>();

            if (CurrentPage < 1)
            {
                CurrentPage = 1;
            }
            else
            if (CurrentPage > TotalPages)
            {
                CurrentPage = TotalPages;
            }
        }

        

        public int MaxPages { get; set; }
        public int PageSize { get; set; } = 10;
        public int FirstPage { get { return (1); } }
        public int PriorPage { get { return (CurrentPage > 1 ? CurrentPage - 1 : 1); } }
        public int CurrentPage { get; set; } = 1;
        public int NextPage { get { return (CurrentPage < LastPage ? CurrentPage + 1 : LastPage); }}
        public int LastPage { get { return TotalPages; }}

        public string OrderBy { get; set; }
        public bool Descending { get; set; }
        public IList<T> Items { get; set; }

        public int TotalItems { get; set; }

        public int TotalPages { get { return GetTotalPages(); }}
        public IList<Page> Pages { get; set; }

        public string Message { get; set; }
        public string UrlSearch { get; set; }

        private int GetTotalPages()
        {
            Pages = new List<Page>();

            int totalPage = TotalItems > 0 ? ((int)Math.Ceiling((decimal)TotalItems / (decimal)PageSize)) : 1;
            for (int i = 1; i <= totalPage; i++)
            {
                if(!Pages.Any(p=> p.PageNumber == i))
                    Pages.Add(new Page() { PageNumber = i, Url = ""});
            }
            return totalPage;
        }

        public string GetLinkPage(string filters, int pg)
        {
            return $"{UrlSearch}{filters}&dataPage.currentPage={pg}&dataPage.pagesize={PageSize}&dataPage.orderby={OrderBy}&dataPage.Descending={Descending}";
        }
    }

    public class Page
    {
        public int PageNumber { get; set; }
        public string Url { get; set; }
    }
}
