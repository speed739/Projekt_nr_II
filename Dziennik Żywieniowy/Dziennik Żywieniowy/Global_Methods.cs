using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace Dziennik_Żywieniowy
{
    static class Global_Methods
    {
        private static Regex _regex = new Regex("[^0-9,]+");
        public static string username;
        public static decimal chartvalue = 0;
      
        public static bool IsTextAllowed(string text)
        {
            return _regex.IsMatch(text);
        }
        public static void Exit_method()
        {
            List<Window> windows = new List<Window>();
            windows = Application.Current.Windows.OfType<Window>().ToList();
            windows.ForEach(x => x.Close());
        }
    }
}
