using System;
using System.Collections.Generic;
using System.Text;

namespace P2E.Shared.Model
{
    public interface IPageControl
    {
       int PageSize { get; set; }
       int CurrentPage { get; set; }
       int TotalRows { get; set; }
    }
}
