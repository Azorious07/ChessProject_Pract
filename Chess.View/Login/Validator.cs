using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Chess.View.Login
{
    public static class Validator
    {
        public static bool RegisterSymbolsValid(string login)
        {
            string loginPattern = "^[a-zA-Z][a-zA-Z0-9]{0,29}$";
            bool loginValidated = Regex.IsMatch(login, loginPattern) && login != "Username";

            return loginValidated;
        }
    }
}
