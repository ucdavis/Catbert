﻿using System;
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
    public class Users
    {
        public Users()
        {
        }
        public Users(string eid, string firstName, string lastName, string login, string email, string department)
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
    public class CatbertUsers : Users
    {
        public CatbertUsers()
        {
        }
        public CatbertUsers(int userID, string eid, string firstName, string lastName, string login, string email, string sid, string role, int roleID)
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
    public class EmailTypes
    {
        public EmailTypes()
        {
        }
        public EmailTypes(int emailTypeID, string type)
        {
            this.emailTypeID = emailTypeID;
            this._type = type;
        }

        private int emailTypeID;
        private string _type;

        public int EmailTypeID
        {
            get { return emailTypeID; }
            set { emailTypeID = value; }
        }
        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }
    }
    public class PhoneTypes
    {
        public PhoneTypes()
        {
        }
        public PhoneTypes(int phoneTypeID, string type)
        {
            this._phoneTypeID = phoneTypeID;
            this._phoneType = type;
        }

        private int _phoneTypeID;
        private string _phoneType;

        public int PhoneTypeID
        {
            get { return _phoneTypeID; }
            set { _phoneTypeID = value; }
        }
        public string Type
        {
            get { return _phoneType; }
            set { _phoneType = value; }
        }
    }
    public class Units
    {
        public Units()
        {
        }
        public Units(int unitID, string unit)
        {
            this._unitID = unitID;
            this._unit = unit;
        }

        private int _unitID;
        private string _unit;

        public int UnitID
        {
            get { return _unitID; }
            set { _unitID = value; }
        }
        public string Unit
        {
            get { return _unit; }
            set { _unit = value; }
        }

    }
    public class Roles
    {
        public Roles()
        {
        }
        public Roles(int roleID, string role)
        {
            this._roleID = roleID;
            this._role = role;
        }

        private int _roleID;
        private string _role;

        public int RoleID
        {
            get { return _roleID; }
            set { _roleID = value; }
        }
        public string Role
        {
            get { return _role; }
            set { _role = value; }
        }

    }

}