using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pet_Shop_HCI.classes
{
    class Employee
    {
        public string jmb { get; set; }
        public string name { get; set; }
        public string lastName { get; set; }
        public int admin { get; set; }
        public int active { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
        public string theme { get; set; }

        public Employee(string jmb, string name, string lastname, int ad, int activeStatus, string username, string pass, string theme)
        {
            this.jmb = jmb;
            this.name = name;
            this.lastName = lastname;
            this.admin = ad;
            this.active = activeStatus;
            this.userName = username;
            this.password = pass;
            this.theme = theme;
        }
    }
}
