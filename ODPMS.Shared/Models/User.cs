﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace ODPMS.Models
{
    public class User
    {
        public int? Id { get; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserType { get; set; }
        public string Status { get; set; }
        public DateTime? LastLogin { get; set; }

        public User(int? id, string username, string password, string salt, string firstName, string lastName, string userType, string status, DateTime? lastLogin)
        {
            Id = id;
            Username = username;
            Password = password;
            Salt = salt;
            FirstName = firstName;
            LastName = lastName;
            UserType = userType;
            Status = status;
            LastLogin = lastLogin;
        }

        public User(string username, string password, string salt, string firstName, string lastName, string userType, string status)
        {
            Username = username;
            Password = password;
            Salt = salt;
            FirstName = firstName;
            LastName = lastName;
            UserType = userType;
            Status = status;
        }

        public override string ToString()
        {
            return Status;
        }
    }

    public class UserViewModel
    {
        public int? Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserType { get; set; }
        public string Status { get; set; }
        public DateTime? LastLogin { get; set; }

        public UserViewModel(int? id, string username, string firstName, string lastName, string userType, string status, DateTime? lastLogin)
        {
            Id = id;
            Username = username;
            FirstName = firstName;
            LastName = lastName;
            UserType = userType;
            Status = status;
            LastLogin = lastLogin;
        }
    }
}
