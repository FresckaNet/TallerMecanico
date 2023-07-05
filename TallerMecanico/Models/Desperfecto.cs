using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TallerMecanico.Models
{
    internal class Desperfecto
    {
        public int Id { get; set; }
        public int IdPressupuesto { get; set; }
        public string Descripcion { get; set;}
        public float ManoObra { get; set; }
        public int Tiempo { get; set; }
    }
}
