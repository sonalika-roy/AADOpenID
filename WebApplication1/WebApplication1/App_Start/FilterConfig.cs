﻿using System.Web;
using System.Web.Mvc;
using WebApplication1.Filters;

namespace WebApplication1
{
    public static class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new NoCacheAttribute());
        }
    }
}

