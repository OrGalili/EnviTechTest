using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnviTechTest
{
    class OperatorModel
    {
        private List<string> name = new List<string>();
        public List<string> Name { get {return name; } set { name = value; } }
    }
}
