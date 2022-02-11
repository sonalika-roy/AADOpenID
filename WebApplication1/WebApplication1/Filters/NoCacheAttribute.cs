using System;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Filters
{
	public class NoCacheAttribute : ActionFilterAttribute
	{
		public override void OnResultExecuted(ResultExecutedContext filterContext)
		{
			HttpCachePolicyBase cache = filterContext.HttpContext.Response.Cache;
			cache.SetCacheability(HttpCacheability.NoCache);
			cache.SetExpires(DateTime.Today.AddDays(-1));
			cache.SetMaxAge(new TimeSpan(0));
			cache.SetNoStore();
		}
	}
}