﻿using Catbert4.Services;
using AutoMapper;

namespace Catbert4.Models
{
    public class ServiceUser
    {
        public string Login { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public int RoleId { get; set; }
        public int UnitId { get; set; }

        public ServiceUser() { }

        public ServiceUser(DirectoryUser directoryUser)
        {
            //Map from the directory user to this
            Mapper.Map(directoryUser, this);
        }
    }
}