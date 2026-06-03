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
        private bool _cargando = false;
        private static int _ultimoIdIdioma = 1;

        public Login()
        {
            InitializeComponent();
            System.Diagnostics.Debug.WriteLine($"Constructor Login - _ultimoIdIdioma: {_ultimoIdIdioma}");
            CargarIdiomas();
            ServiceSessionManager55CA.getIntancia().Idioma.Suscribir(this);

            string codIdioma = _ultimoIdIdioma == 1 ? "es" : "en";
            System.Diagnostics.Debug.WriteLine($"Cargando idioma en constructor: {codIdioma}");
            ServiceSessionManager55CA.getIntancia().Idioma.CargarIdioma(codIdioma);

            _cargando = true;
            cmbIdioma.SelectedValue = _ultimoIdIdioma;
            _cargando = false;
        }

        private void CargarIdiomas()
        {
            _cargando = true;
            var idiomas = _idiomaService.obtenerTodos();
            cmbIdioma.DataSource = idiomas;
            cmbIdioma.DisplayMember = "Nombre";
            cmbIdioma.ValueMember = "Id";
            _cargando = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"--- INICIO LOGIN ---");
            System.Diagnostics.Debug.WriteLine($"combo seleccionado: {((Idioma55CA)cmbIdioma.SelectedItem).Id}");
            string username = txtUser.Text;
            string password = txtPassword.Text;
            try
            {
                bool usaPasswordDefault = _userService.login(username, password);

                // primero guardar el idioma elegido en el combo
                Idioma55CA idiomaSeleccionado = (Idioma55CA)cmbIdioma.SelectedItem;
                _ultimoIdIdioma = idiomaSeleccionado.Id;
                System.Diagnostics.Debug.WriteLine($"Guardando idioma: {idiomaSeleccionado.Id}");


                _userService.GuardarIdioma(idiomaSeleccionado.Id);

                System.Diagnostics.Debug.WriteLine($"IdIdioma en sesion: {ServiceSessionManager55CA.getIntancia().usuarioActivo.IdIdioma}");

                // despues cargar el idioma guardado
                string codIdioma = idiomaSeleccionado.Id == 1 ? "es" : "en";
                ServiceSessionManager55CA.getIntancia().Idioma.CargarIdioma(codIdioma);

                txtUser.Text = "";
                txtPassword.Text = "";
                this.Hide();

                if (usaPasswordDefault)
                {
                    CambiarContraseña form = new CambiarContraseña();
                    form.FormClosed += (s, args) =>
                    {
                        string cod = Login._ultimoIdIdioma == 1 ? "es" : "en";
                        ServiceSessionManager55CA.getIntancia().Idioma.CargarIdioma(cod);
                        this.Show();
                    };
                    form.Show();
                }
                else
                {
                    MenuPrincipal menu = new MenuPrincipal();
                    menu.FormClosed += (s, args) =>
                    {
                        string cod = Login._ultimoIdIdioma == 1 ? "es" : "en";
                        ServiceSessionManager55CA.getIntancia().Idioma.CargarIdioma(cod);
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

        public void actualizarIdioma()
        {
            var t = ServiceSessionManager55CA.getIntancia().Idioma;

            this.Text = t.Translate("Login.formTitle");
            label3.Text = t.Translate("Login.labelBienvenido");
            lblUsuario.Text = t.Translate("Login.lblUsuario");
            lblContrasena.Text = t.Translate("Login.lblPassword");
            btnLogin.Text = t.Translate("Login.btnLogin");
        }

        private void cmbIdioma_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_cargando || cmbIdioma.SelectedItem == null) 
            {
                return; 
            }

            Idioma55CA idiomaSeleccionado = (Idioma55CA)cmbIdioma.SelectedItem;
            string codIdioma = idiomaSeleccionado.Id == 1 ? "es" : "en";
            ServiceSessionManager55CA.getIntancia().Idioma.CargarIdioma(codIdioma);
        }
    }
}
