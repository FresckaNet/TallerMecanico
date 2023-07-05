using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TallerMecanico.Repository
{
    internal class ConnectionString
    {
        public string Property { get; set; }
        public string Server { get; set; }
        public string Database { get; set; }
        public string Uid { get; set; }
        public string Pwd { get; set; }
        public bool Avaliable { get; set; }
    }
}
