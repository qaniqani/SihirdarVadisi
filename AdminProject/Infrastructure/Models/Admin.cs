﻿using System;
using AdminProject.Models;

namespace AdminProject.Infrastructure.Models
{
    public class Admin
    {
        public int Id { get; set; }
        public AdminTypes AdminType { get; set; } = AdminTypes.Admin;
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Authorization { get; set; }
        public string Picture { get; set; }
        public string Motto { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public DateTime LastLoginDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public StatusTypes Status { get; set; }
    }
}