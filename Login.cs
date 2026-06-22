using BE;
using BLL;
using Services.Modelos.Idioma;
using Services_55CA;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Servicios
{
    public partial class Login : Form, IIdiomaObserver
    {
        UsuarioService _userService = new UsuarioService();
        BLLIdioma55CA _idiomaService = new BLLIdioma55CA();
        

        public Login()
        {
            InitializeComponent();
            ServiceSessionManager55CA.getIntancia().Idioma.Suscribir(this);
            ServiceSessionManager55CA.getIntancia().Idioma.CargarIdioma("es");

        }

        

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            
            string username = txtUser.Text;
            string password = txtPassword.Text;
            try
            {
                bool usaPasswordDefault = _userService.login(username, password);


                int idiomaUsuario = ServiceSessionManager55CA.getIntancia().usuarioActivo.IdIdioma;
                string codIdiomaUsuario = idiomaUsuario == 1 ? "es" : "en";
                ServiceSessionManager55CA.getIntancia().Idioma.CargarIdioma(codIdiomaUsuario);
                
                txtUser.Text = "";
                txtPassword.Text = "";
                this.Hide();

                if (usaPasswordDefault)
                {
                    CambiarContraseña form = new CambiarContraseña();
                    form.FormClosed += (s, args) => RestaurarIdiomaLogin();
                    form.Show();
                }
                else
                {
                    MenuPrincipal menu = new MenuPrincipal();
                    menu.FormClosed += (s, args) => RestaurarIdiomaLogin();
                    menu.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void RestaurarIdiomaLogin()
        {
            ServiceSessionManager55CA.getIntancia().Idioma.CargarIdioma("es");
            this.Show();

        }

        public void actualizarIdioma()
        {
            var t = ServiceSessionManager55CA.getIntancia().Idioma;

            this.Text = t.Translate("Login.formTitle");
            label3.Text = t.Translate("Login.labelBienvenido");
            lblUsuario.Text = t.Translate("Login.lblUsuario");
            lblContrasena.Text = t.Translate("Login.lblPassword");
            btnLogin.Text = t.Translate("Login.btnLogin");
        }

        
    }
}
