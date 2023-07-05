using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TallerMecanico.Models
{
    internal class Automovil : Vehiculo
    {
        public Tipo Tipo { get; set; }
        public int CantidadPuertas { get;}
    }

    public enum Tipo
    {
        compacto,
        sedan,
        monovolumen,
        utilitario,
        lujo
    }
}
