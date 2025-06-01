using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using ConexionesSGBD;

namespace WindowsFormsApp1
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Iniciar con el formulario de inicio de sesión
            using (Form1 loginForm = new Form1())
            {
                if (loginForm.ShowDialog() == DialogResult.OK)
                {
                    // Obtener la conexión seleccionada en InicioSesion
                    IBaseDatos conexion = loginForm.Conexion;
                    string gestorSeleccionado = loginForm.GestorSeleccionado;

                    if (conexion == null)
                    {
                        MessageBox.Show("Error al establecer la conexión.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Diccionario con la conexión seleccionada
                    Dictionary<string, IBaseDatos> conexiones = new Dictionary<string, IBaseDatos>
                    {
                        { gestorSeleccionado, conexion }
                    };

                    // Iniciar el formulario principal con la conexión
                    Application.Run(new SgbdMultiBaseDatos(conexiones));
                }
            }
        }
    }
}
