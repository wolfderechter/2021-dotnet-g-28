﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2021_dotnet_g_28.Models.Domain
{
    public class SupportManager : Employee
    {

        public IdentityUser user { get; set; }
        #region methods
        public void ManageContractType()
        {
            throw new NotImplementedException();
        }

        public void CreateNewTicket()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
