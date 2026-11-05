using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pet_Shop_HCI.classes
{
    public static class Sesija
    {
        public static string JMB { get; set; }
        public static string KorisnickoIme { get; set; }
        public static bool Admin { get; set; }
        public static string Tema { get; set; }

        public static void ClearSesija()
        {
            JMB = null;
            KorisnickoIme = null;
            Admin = false;
            Tema = null;
        }
    }
}
