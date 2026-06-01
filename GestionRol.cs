using Services.Modelos.Idioma;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Servicios
{
    public partial class GestionRol : Form, IIdiomaObserver
    {
        public GestionRol()
        {
            InitializeComponent();
        }

        public void actualizarIdioma()
        {
            throw new NotImplementedException();
        }
    }
}
