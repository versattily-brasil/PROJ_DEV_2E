using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P2E.SSO.API.DTO
{
    public class Menu
    {
        public string title { get; set; }
        public bool root { get; set; }
        public string bullet { get; set; }
        public string icon { get; set; }
        public List<SubMenu> submenu { get; set; }
    }
}
