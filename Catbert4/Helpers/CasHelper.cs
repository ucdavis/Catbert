using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Web.Security;
using NHibernate.Hql.Ast.ANTLR;

namespace Catbert4.Helpers
{
    public static class CasHelper
    {
#if DEBUG
        private const string StrCasUrl = "https://ssodev.ucdavis.edu/cas/";
#else
        private const string StrCasUrl = "https://cas.ucdavis.edu/cas/";
#endif
        private const string StrTicket = "ticket";
        private const string StrReturnUrl = "ReturnURL";

        /// <summary>
        /// This logic removes any lingering "ticket=..." text from the return URL.
        /// </summary>
        /// <param name="ticket"></param>
        /// <returns>return URL less any instances of "ticket=..."</returns>
        public static string GetReturnUrl(string ticket)
        { 
           var strReturnUrl = HttpContext.Current.Request.QueryString[StrReturnUrl];
           string retval = strReturnUrl;

            // remove any lingering ticket portions from the URL:

            if (!string.IsNullOrEmpty(ticket))
            {
                var ticketKeyLength = "ticket=".Length;
                var startIndex = strReturnUrl.IndexOf("ticket=");

                if (startIndex > -1)
                {
                    var endIndex = ticket.Length;

                    retval = strReturnUrl.Remove(startIndex, ticketKeyLength + endIndex);

                    startIndex = retval.IndexOf("?");
                    if (startIndex == retval.Length - 1 //remove "?"
                       )
                    {
                        retval = retval.Remove(startIndex, 1);
                    }
                    else
                    {
                        // There may have been additional parameters on the end of the query string,
                        // so remove any trailing "&"
                        // 
                        endIndex = retval.LastIndexOf(("&"));
                        if (endIndex == retval.Length - 1)
                        {
                            retval = retval.Remove(endIndex, 1);
                        }
                    }
                }
            }

            return retval;
        }

        /// <summary>
        /// Performs the CAS Login and automatically redirects to the desired page, if possible.
        /// Will do nothing if the user is already authenticated
        /// </summary>
        public static void LoginAndRedirect()
        {
            string returnUrl = Login();

            if (returnUrl != null) HttpContext.Current.Response.Redirect(returnUrl);
        }


        /// <summary>
        /// Login to the campus DistAuth system using CAS        
        /// </summary>
        public static string Login()
        {
            // get the context from the source
            var context = HttpContext.Current;

            // This should always just get back the original host.

            // try to load a valid ticket
            HttpCookie validCookie = context.Request.Cookies[FormsAuthentication.FormsCookieName];
            FormsAuthenticationTicket validTicket = null;

            // check to make sure cookie is valid by trying to decrypt it
            if (validCookie != null)
            {
                try
                {
                    validTicket = FormsAuthentication.Decrypt(validCookie.Value);
                }
                catch
                {
                    validTicket = null;
                }
            }

            // if user is unauthorized and no validTicket is defined then authenticate with cas
            //if (context.Response.StatusCode == 0x191 && (validTicket == null || validTicket.Expired))
            if (validTicket == null || validTicket.Expired)
            {
                // get ticket & service
                string ticket = context.Request.QueryString[StrTicket];

                var query = "";

                string service;

                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["UseOption2ToBuildCasServiceString"]) &&
                    Convert.ToBoolean(ConfigurationManager.AppSettings["UseOption2ToBuildCasServiceString"].Equals("true")))
                {
                    // Remove ticket=xxx from the querystring and from returnUrl:
                    query = GetReturnUrl(ticket);
                    
                    // Although a bit clumsy, this is the best technique to build
                    // the actual CAS service string.
                    // The service should be something simple like the URL present in the original request, 
                    // "http(s)://host(:port)/<queryStringWithoutAnyReferenceToTicket>, i.e.,
                    // "https://catbert4.ucdavis.edu", "https://catbert4.ucdavis.edu/UserManagement/Manage/EligibilityList" or
                    // "https://catbert4.ucdavis.edu/Home/UserManagementPortal?application=EligibilityList&DisplayHeader=Off", etc.
                    // Using logic similar to
                    // context.Server.UrlEncode(context.Request.Url.GetLeftPart(UriPartial.Path) + query), resulted 
                    // in multiple service strings and multiple redirects to CAS when each view, page segment or
                    // include link was called when building the various sections of the page.
                    //
                    service = context.Server.UrlEncode(
                        context.Request.Url.Scheme
                        + "://"
                        + context.Request.Url.Authority
                        + query
                    );
                }
                else
                {
                    // build query string but strip out ticket if it is defined

                    foreach (string key in context.Request.QueryString.AllKeys)
                    {
                        if (string.Compare(key, StrTicket, true) != 0)
                        {
                            query += "&" + key + "=" + context.Request.QueryString[key];
                        }
                    }

                    // replace 1st character with ? if query is not empty
                    if (!string.IsNullOrEmpty(query))
                    {
                        query = "?" + query.Substring(1);
                    }
                    
                    service = context.Server.UrlEncode(context.Request.Url.GetLeftPart(UriPartial.Path) + query);
                }

                // if ticket is defined then we assume they are coming from CAS
                if (!string.IsNullOrEmpty(ticket))
                {
                    // validate ticket against cas

                    StreamReader sr = new StreamReader(new WebClient().OpenRead(StrCasUrl + "validate?ticket=" + ticket + "&service=" + service));

                    // parse text file
                    if (sr.ReadLine() == "yes")
                    {
                        // get kerberos id
                        string kerberos = sr.ReadLine();

                        // set forms authentication ticket
                        FormsAuthentication.SetAuthCookie(kerberos, false);

                        //strip out any lingering "ticket=..." in the returnUrl:
                        string returnUrl = GetReturnUrl(ticket);

                        return !string.IsNullOrEmpty(returnUrl) ? returnUrl : FormsAuthentication.DefaultUrl;
                    }
                }

                // ticket doesn't exist or is invalid so redirect user to CAS login
                context.Response.Redirect(StrCasUrl + "login?service=" + service);
            }

            return null;
        }
    }
}