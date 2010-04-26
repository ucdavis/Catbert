using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.DirectoryServices.Protocols;

namespace CAESDO.Catbert.BLL
{
    public class DirectoryServices
    {
        static readonly string LDAPUser = System.Web.Configuration.WebConfigurationManager.AppSettings["LDAPUser"];
        static readonly string LDAPPassword = System.Web.Configuration.WebConfigurationManager.AppSettings["LDAPPassword"];

        public static List<DirectoryUser> LDAPSearchUsers(string employeeID, string firstName, string lastName, string loginID)
        {
            if (employeeID == null && firstName == null && lastName == null && loginID == null) return new List<DirectoryUser>();

            List<DirectoryUser> users = new List<DirectoryUser>();

            //Establishing a Connection to the LDAP Server
            LdapDirectoryIdentifier ldapident = new LdapDirectoryIdentifier("ldap.ucdavis.edu", 636);
            //LdapConnection lc = new LdapConnection(ldapident, null, AuthType.Basic);
            LdapConnection lc = new LdapConnection(ldapident, new System.Net.NetworkCredential(LDAPUser, LDAPPassword), AuthType.Basic);
            lc.Bind();
            lc.SessionOptions.ProtocolVersion = 3;
            lc.SessionOptions.SecureSocketLayer = true;

            //Configure the Search Request to Query the UCD OpenLDAP Server's People Search Base for a Specific User ID or Mail ID and Return the Requested Attributes 
            string[] attributesToReturn = new string[] { "uid", "employeeNumber", "mail", "displayName", "cn", "sn", "givenName" };

            StringBuilder searchFilter = new StringBuilder("(&");

            //loginID = "postit";

            if (employeeID != null)
            {
                searchFilter.AppendFormat("(employeeNumber={0})", employeeID);
            }

            if (firstName != null)
            {
                searchFilter.AppendFormat("(givenName={0})", firstName);
            }

            if (lastName != null)
            {
                searchFilter.AppendFormat("(sn={0})", lastName);
            }

            if (loginID != null)
            {
                searchFilter.AppendFormat("(uid={0})", loginID);
            }

            searchFilter.Append(")");

            string strSearchFilter = searchFilter.ToString(); //"(&(uid=" + (loginID ?? string.Empty) + ")(sn=" + (lastName ?? "Kirkland") + "))";
            string strSearchBase = "ou=People,dc=ucdavis,dc=edu";

            SearchRequest sRequest = new SearchRequest(strSearchBase, strSearchFilter, SearchScope.Subtree, attributesToReturn);
            //Send the Request and Load the Response
            SearchResponse sResponse = (SearchResponse)lc.SendRequest(sRequest);

            foreach (SearchResultEntry result in sResponse.Entries)
            {
                DirectoryUser user = new DirectoryUser();

                //Grab out the first response entry

                foreach (DirectoryAttribute attr in result.Attributes.Values)
                {
                    switch (attr.Name)
                    {
                        case "uid":
                            user.LoginID = attr[0].ToString();
                            break;
                        case "givenName":
                            user.FirstName = attr[0].ToString();
                            break;
                        case "sn":
                            user.LastName = attr[0].ToString();
                            break;
                        case "mail":
                            user.EmailAddress = attr[0].ToString();
                            break;
                        case "employeeNumber":
                            user.EmployeeID = attr[0].ToString();
                            break;
                        case "cn":
                            user.FullName = attr[0].ToString();
                            break;
                        default:
                            break;
                    }
                }

                users.Add(user);
            }

            return users;
        }
    }

    public class DirectoryUser
    {
        public string EmployeeID { get; set; }
        public string LoginID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string EmailAddress { get; set; }

        public DirectoryUser()
        {

        }

    }
}
