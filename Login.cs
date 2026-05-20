using Services;
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
    public partial class Login : Form
    {
        UsuarioService _userService = new UsuarioService();
        public Login()
        {
            InitializeComponent();
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

                txtUser.Text = "";
                txtPassword.Text = "";

                this.Hide();

                if (usaPasswordDefault)
                {
                    CambiarContraseña form = new CambiarContraseña();

                    form.FormClosed += (s, args) =>
                    {
                        this.Show();
                    };

                    form.Show();
                }
                else
                {
                    MenuPrincipal menu = new MenuPrincipal();

                    menu.FormClosed += (s, args) =>
                    {
                        this.Show();
                    };

                    menu.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
