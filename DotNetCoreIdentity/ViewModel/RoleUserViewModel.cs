using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCoreIdentity.ViewModel
{
    public class RoleUserViewModel
    {
        public string UserID { get; set; }
        public string RoleID { get; set; }
        public string UserFullName { get; set; }
        public bool IsSelected { get; set; }
    }
}
