using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Net;

namespace CAESDO
{
    public partial class login : System.Web.UI.Page
    {
        private const string STR_ReturnURL = "ReturnURL";
        private const string STR_CAS_URL = "https://cas.ucdavis.edu:8443/cas/";
        private const string STR_KERBEROS_URL = "https://secureweb.ucdavis.edu/form-auth/sendback?";
        private const string STR_Ticket = "ticket";

        private static bool GetUseKerberos()
        {
            bool useKerberos = false;
            return useKerberos;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string CASticket = Request.QueryString[STR_Ticket] ?? string.Empty;

            if (!string.IsNullOrEmpty(CASticket))
                CASLogin();
            else
                DistAuthLogin();
        }

        private void DistAuthLogin()
        {
            bool useKerberos = GetUseKerberos();

            if (useKerberos)
            {
                KerberosLogin();
            }
            else
            {
                CASLogin();
            }
        }

        #region CAS
        /// <summary>
        /// Login to the campus DistAuth system using CAS        
        /// </summary>
        private void CASLogin()
        {
            string loginUrl = STR_CAS_URL;

            // get the context from the source
            HttpContext context = HttpContext.Current;

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
                // build query string but strip out ticket if it is defined
                string query = "";
                
                foreach (string key in context.Request.QueryString.AllKeys)
                {
                    if (String.Compare(key, STR_Ticket, true) != 0)
                    {
                        if ( query.Contains(key) == false )
                            query += "&" + key + "=" + context.Request.QueryString[key];
                    }
                }

                // replace 1st character with ? if query is not empty
                if (!String.IsNullOrEmpty(query))
                {
                    query = "?" + query.Substring(1);
                }
                
                // get ticket & service
                string ticket = context.Request.QueryString[STR_Ticket];
                string service = context.Server.UrlEncode(context.Request.Url.GetLeftPart(UriPartial.Path) + query);

                // if ticket is defined then we assume they are coming from CAS
                if (!String.IsNullOrEmpty(ticket))
                {
                    // validate ticket against cas
                    StreamReader sr = new StreamReader(new WebClient().OpenRead(loginUrl + "validate?ticket=" + ticket + "&service=" + service));

                    // parse text file
                    if (sr.ReadLine() == "yes")
                    {
                        // get kerberos id
                        string kerberos = sr.ReadLine();

                        // set forms authentication ticket
                        FormsAuthentication.SetAuthCookie(kerberos, false);
                        
                        string returnURL = string.Empty;

                        if (!string.IsNullOrEmpty(query))
                        {
                            //need to grab the return url from the query string: ?ReturnURL=/...                        
                            returnURL = query.Substring(query.IndexOf("=") + 1); //So get everything after the first equals sign
                        }

                        if (returnURL == null)
                            returnURL = FormsAuthentication.DefaultUrl;

                        context.Response.Redirect(returnURL);

                        return;
                    }
                }

                // ticket doesn't exist or is invalid so redirect user to CAS login
                context.Response.Redirect(loginUrl + "login?service=" + service);
            }
        } 
        #endregion

        #region Kerberos
        private void KerberosLogin()
        {
            HttpCookie authCookie = System.Web.HttpContext.Current.Request.Cookies["AuthUser"];

            if (authCookie == null)
                Response.Redirect(STR_KERBEROS_URL + Request.Url, true);

            //Check to see if the user is already authenticated.  
            //If so, then if there is a return url they are not authorized
            if (User.Identity.IsAuthenticated)
                if (Request.QueryString[STR_ReturnURL] != null)
                {
                    Response.Write("Not Authorized!");
                    return;
                }

            string userID;
            string afsHash;
            string distAuthHash;

            ParseDistAuthCookie(authCookie, out userID, out afsHash, out distAuthHash);

            if (VerifyDistAuthCookie(distAuthHash))
            {
                //then we are ok
                //CAESDOMembershipProvider prov = (CAESDOMembershipProvider)Membership.Provider;
                //if (prov.ValidateUser(userID))
                if (true)
                {
                    FormsAuthentication.Initialize();
                    //Create a new ticket for authentication

                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                        1, //version
                        userID, //Username
                        DateTime.Now, //time issued
                        DateTime.Now.AddMinutes(10), //10 minutes to expire in
                        false, //non persistent ticket
                        String.Empty,
                        FormsAuthentication.FormsCookiePath);

                    //Hash the cookie for transport
                    string hash = FormsAuthentication.Encrypt(ticket);
                    HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, hash); //hashed ticket in cookie

                    //Now add the auth cookie
                    Response.Cookies.Add(cookie);

                    string returnURL = Request.QueryString[STR_ReturnURL];

                    if (returnURL == null)
                        returnURL = FormsAuthentication.DefaultUrl;

                    Response.Redirect(returnURL);

                }
            }

            Response.Redirect("Error.aspx"); //no authorized
            //Response.Redirect(Recruitment.Web.RecruitmentConfiguration.ErrorPage(CAESDO.Recruitment.Web.RecruitmentConfiguration.ErrorType.AUTH));
        }

        /// <summary>
        /// Parses the DistAuth cookie into user, AFS Hash, and IP Hash
        /// </summary>
        /// <param name="suppliedDistAuthCookie">The cookie obtained from the DistAuth server</param>
        /// <param name="suppliedUserName">An out reference to the User name variable</param>
        /// <param name="suppliedAfsHash">An out reference to the AFS hash variable</param>
        /// <param name="suppliedDistAuthHash">An out reference to the DistAuth hash variable</param>
        private void ParseDistAuthCookie(HttpCookie suppliedDistAuthCookie,
            out string suppliedUserName,
            out string suppliedAfsHash,
            out string suppliedDistAuthHash)
        {
            string authUserValue = suppliedDistAuthCookie.Value;
            string[] authUserValueTokens = authUserValue.Split(new char[1] { '-' });
            suppliedUserName = authUserValueTokens[0];
            suppliedAfsHash = authUserValueTokens[1];
            suppliedDistAuthHash = authUserValueTokens[2];
        }

        /// <summary>
        /// Verifies the clients IP address matches the hashed IP address in DistAuth
        /// Prevents copying the cookie and using from another machine 
        /// </summary>
        /// <param name="ipHash">The DistAuth cookie's IP Address hash</param>
        /// <returns>True if verified, false otherwise</returns>
        private bool VerifyDistAuthCookie(string ipHash)
        {
            if (IpHash(System.Web.HttpContext.Current.Request.UserHostAddress) == ipHash)
                return true;
            return false;
        }

        /// <summary>
        /// Hashes an IP address using DistAuth's algoritm
        /// </summary>
        /// <param name="suppliedIPAddress">The clien'ts IP address</param>
        /// <returns>The hexadecimal string hash of the client's IP address</returns>
        private string IpHash(string suppliedIPAddress)
        {
            char[] ipArray = suppliedIPAddress.ToCharArray();
            ulong checksum;
            ulong ltmp;
            int i;

            // Hash base
            for (i = 0, checksum = 1; i < ipArray.Length / 2; i++)
            {
                checksum = (checksum & 0x00ffffff) * ipArray[i];
            }

            // Hash power
            for (ltmp = 1; i < ipArray.Length; i++)
            {
                ltmp = (ltmp & 0x00ffffff) * ipArray[i];
            }

            checksum = checksum ^ ltmp;

            // Convert checksum to uppercase hexadecimal string
            return checksum.ToString("X");
        } 
        #endregion
    }
}