using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace Catbert.Services
{
    public class ServiceUser
    {
        public ServiceUser()
        {
        }
        public ServiceUser(string eid, string firstName, string lastName, string login, string email, string department)
        {
            this._employeeID = eid;
            this._firstName = firstName;
            this._lastName = lastName;
            this._login = login;
            this._email = email;
            this._department = department;
        }

        protected string _employeeID;
        protected string _firstName;
        protected string _lastName;
        protected string _login;
        protected string _email;
        protected string _department;

        public string EmployeeID
        {
            get { return _employeeID; }
            set { _employeeID = value; }
        }
        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; }
        }
        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }
        public string Login
        {
            get { return _login; }
            set { _login = value; }
        }
        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }
        public string Department
        {
            get { return _department; }
            set { _department = value; }
        }
    }
    public class CatbertUser : ServiceUser
    {
        public CatbertUser()
        {
        }
        public CatbertUser(int userID, string eid, string firstName, string lastName, string login, string email, string sid, string role, int roleID)
        {
            this._userID = userID;
            this._employeeID = eid;
            this._firstName = firstName;
            this._lastName = lastName;
            this._sid = sid;
            this._role = role;

            this._roleID = roleID;

            this._login = login;
            this._email = email;
        }

        private int _userID;
        private string _sid;
        private string _role;
        private int _roleID;

        public int RoleID
        {
            get { return _roleID; }
            set { _roleID = value; }
        }

        public int UserID
        {
            get { return _userID; }
            set { _userID = value; }
        }
        public string SID
        {
            get { return _sid; }
            set { _sid = value; }
        }
        public string Role
        {
            get { return _role; }
            set { _role = value; }
        }
    }

    public class ServiceUnit
    {
        public ServiceUnit()
        {
        }
        public ServiceUnit(int unitID, string unit, string unitFIS)
        {
            this.ID = unitID;
            this.Name = unit;
            this.UnitFIS = unitFIS;
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public string UnitFIS { get; set; }
    }
    public class ServiceRole
    {
        public ServiceRole()
        {
        }
        public ServiceRole(int roleID, string role)
        {
            this.ID = roleID;
            this.Name = role;
        }

        public int ID { get; set; }
        public string Name { get; set; }
    }

}