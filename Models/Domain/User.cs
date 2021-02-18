using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2021_dotnet_g_28.Models.Domain
{
    public class User
    {
        #region fields
        private IList<IList<String>> _numberOfLoginAttempts;
        private int _maxNumberOfLoginAttempts;
        #endregion

        #region properties
        public String Username { get; set; }
        public bool IsBlocked { get; set; }
        public String Password { get; set; }
        #endregion

        #region methods
        public int GetNumberOfLoginAttempts()
        {
            throw new NotImplementedException();
        }

        public void Login(String password)
        {
            throw new NotImplementedException();
        }

        public void RegisterLoginAttempts()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
