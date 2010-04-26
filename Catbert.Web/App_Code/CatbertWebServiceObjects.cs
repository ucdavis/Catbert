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
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Catbert.Services
{
    public class AutoCompleteData
    {
        public string result { get; set; } //like key
        public string value { get; set; }
        public object data { get; set; }

        public AutoCompleteData(string result, string value, object data)
        {
            this.result = result;
            this.value = value;
            this.data = data;
        }
    }

    public class GridData
    {
        public int page { get; set; }
        public int total { get; set; }

        public List<GridDataRow> rows { get; set; }

        public GridData()
        {
            rows = new List<GridDataRow>();
        }
    }

    public class jqGridData
    {
        public int page { get; set; }
        public int total { get; set; }
        public int records { get; set; }

        public List<object> rows { get; set; }

        public jqGridData()
        {
            rows = new List<object>();
        }
    }

    public class GridDataRow
    {
        public int id { get; set; }
        public List<string> cell { get; set; }
    }

    public class ServiceUser
    {
        public ServiceUser()
        {
        }
        public ServiceUser(string eid, string firstName, string lastName, string login, string email, string department)
        {
            EmployeeID = eid;
            FirstName = firstName;
            LastName = lastName;
            Login = login;
            Email = email;
            Department = department;
        }

        public string EmployeeID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public string Department { get; set; }
    }
    public class CatbertUser : ServiceUser
    {
        public CatbertUser()
        {

        }

        public CatbertUser(int userID, string eid, string firstName, string lastName, string login, string email, string sid, string role, int roleID)
            : base(eid, firstName, lastName, login, email, null)
        {
            Roles = new List<ServiceRole>();
            Units = new List<ServiceUnit>();

            UserID = userID;
            Roles.Add(new ServiceRole() { ID = roleID, Name = role });
        }

        public int UserID { get; set; }
        //public string SID { get; set; }
        public List<ServiceRole> Roles { get; set; }
        public List<ServiceUnit> Units { get; set; }
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