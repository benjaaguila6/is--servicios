using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Modelos
{
    public class RolModelo55CA : Componente55CA
    {
        
        public int Id { get; set; }
        public string Nombre { get; set; }
        public List<Componente55CA> Permisos { get; set; } = new List<Componente55CA>();
    }
}
