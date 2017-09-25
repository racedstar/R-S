using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace RioManager.App_Code
{
    public class JS
    {
        public static string Alert(string str)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<script>");
            sb.AppendLine("alert(" + str + ")");
            sb.AppendLine("</script");

            return sb.ToString();
        }
    }
}