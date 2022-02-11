using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Net;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            try
            {   

                var userClaims = User.Identity as System.Security.Claims.ClaimsIdentity;

                //You get the user's first and last name below:
                ViewBag.Name = userClaims?.FindFirst("name")?.Value;

                // The 'preferred_username' claim can be used for showing the username
                ViewBag.Username = userClaims?.FindFirst("preferred_username")?.Value;

                // The subject/ NameIdentifier claim can be used to uniquely identify the user across the web
                ViewBag.Subject = userClaims?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

                // TenantId is the unique Tenant Id - which represents an organization in Azure AD
                ViewBag.TenantId = userClaims?.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid")?.Value;
            }
            catch (Exception ex)
            {
                //TODO
            }

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        /// <summary>
		/// Send an OpenID Connect sign-out request.
		/// </summary>
		public void SignOut()
        {

            try
            {
                /*the code below displays a prompt - pick an account to signout*/

                //HttpContext.GetOwinContext().Authentication.SignOut(
                //	OpenIdConnectAuthenticationDefaults.AuthenticationType,
                //	CookieAuthenticationDefaults.AuthenticationType);

                
                /*the code below automatically signs-out the user*/
                String endSessionEndPoint = ConfigurationManager.AppSettings["endSessionEndPoint"]; //"https://login.microsoftonline.com/common/oauth2/logout";
                String postLogoutRedirectUri = ConfigurationManager.AppSettings["postredirectUri"];
                String aadLogoutUrl = $"{endSessionEndPoint}";

                /*set to true, to redirect after sign-out*/
                if (ConfigurationManager.AppSettings["usePostredirectUri"].ToLower() == "true")
                {
                    aadLogoutUrl = $"{endSessionEndPoint}?post_logout_redirect_uri ={postLogoutRedirectUri}";
                }

                Response.Clear();
                Response.ContentType = "text/html";
                Response.StatusCode = 302;
                Response.RedirectLocation = aadLogoutUrl;
                Response.AppendCookie(new HttpCookie(".AspNet.Cookies", ""));
                Response.Expires = -1;
                Response.Flush();


                /*clear the session and cookie, if redirecting then add the code on the redirected page e.g. logout/index - similar to ORRS*/
                //Session.Clear();
                //Session.Abandon();
                //Response.Cookies.Clear();
            }
            catch
            {
                //HttpContext.Response.Redirect("Logout/Index", true);
            }
        }


        /// <summary>
        /// for a single sign-out request
        /// https://docs.microsoft.com/en-us/azure/active-directory/develop/v2-protocols-oidc#send-a-sign-out-request
        /// implementation for the logout url, the application clears the session, returning a 200 response.
        /// </summary>
        [HttpGet]
        public ActionResult SingleSignOut()
        {

            Session.Clear();
            Session.Abandon();
            Response.Cookies.Clear();


            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

    }
}