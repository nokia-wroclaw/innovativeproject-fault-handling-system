using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Fault_handling_system.Views.Account
{
    public static class ManageAdminNavPages
    {
        public static string ActivePageKey => "ActivePage";

        public static string Register = "Register";

        public static string ManageUsers = "ManageUsers";

        public static string AdminGuide = "IAdminGuide";

        public static string RegisterNavClass(ViewContext viewContext) => PageNavClass(viewContext, Register);

        public static string ManageUsersNavClass(ViewContext viewContext) => PageNavClass(viewContext, ManageUsers);

        public static string InstructionNavClass(ViewContext viewContext) => PageNavClass(viewContext, AdminGuide);

        public static string PageNavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData["ActivePage"] as string;
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }

        public static void AddActivePage(this ViewDataDictionary viewData, string activePage) => viewData[ActivePageKey] = activePage;
    }
}
