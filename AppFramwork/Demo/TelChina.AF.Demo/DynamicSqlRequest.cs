using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TelChina.AF.Demo
{
    public class DynamicSqlRequest
    {
        public string Name { set; get; }
        public int Size { set; get; }
        public List<int> SizeArray { get; set; }
    }
}
