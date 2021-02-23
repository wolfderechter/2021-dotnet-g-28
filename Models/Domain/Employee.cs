﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2021_dotnet_g_28.Models.Domain
{
    public class Employee 
    {
        #region properties
        public String Adress { get; set; }
        public DateTime DateInService { get; set; }
        public String Email { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public int EmployeeNr { get; set; }
        public String Role { get; set; }
       // public ICollection<String> TelNr { get; set; }
        #endregion
    }
}
