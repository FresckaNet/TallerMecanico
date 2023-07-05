using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TallerMecanico.Models
{
    internal class DesperfectoRepuesto
    {
        public int Id { get; set; }
        public int IdDesperfecto { get; set; }    
        public int IdRepuesto { get; set;}
    }
}
