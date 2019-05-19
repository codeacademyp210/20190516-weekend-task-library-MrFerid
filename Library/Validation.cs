using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Library
{
    class Validation
    {
        public string CheckInput(string header, string input, string pattern)
        {
            string patrn;
            string error = "";
            switch (pattern)
            {
                case "onlyString":
                    patrn = @"^[A-Za-z]+$";
                    error += header + " must be only string and not empty \n";
                    break;
                case "emailType":
                    patrn = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";
                    error += "Type of " + header + " is incorrect \n";
                    break;
                case "onlyNumber":
                    patrn = @"^\d+$";
                    error += header + " must be only number \n";
                    break;
                case "any":
                    patrn = @".*\S.*";
                    error += "Write any " + header + " to " + header + " section \n";
                    break;
                case "onlyDouble":
                    input = input.Replace(",", ".");
                    patrn = @"^[0-9\.]+$";
                    error += "Type only number to " + header + "\n";
                    break;
                default:
                    patrn = @"^[A-Za-z0-9]+$";
                    break;
            }
            if (Regex.IsMatch(input, patrn, RegexOptions.IgnoreCase))
            {
                return "ok";
            }
            else
            {
                return error;
            }
        }
    }
}
