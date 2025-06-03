using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ConexionesSGBD;

namespace WindowsFormsApp1
{
    public partial class MigrasionFm : Form
    {
        private Dictionary<string, IBaseDatos> conexiones;
        public MigrasionFm(Dictionary<string, IBaseDatos> conexionesExistentes)
        {
            InitializeComponent();
            conexiones = conexionesExistentes;
        }

        private void MigrasionFm_Load(object sender, EventArgs e)
        {
            comboBoxSqlServer.Items.Clear();
            comboBoxOtros.Items.Clear();

            foreach (var conexion in conexiones)
            {
                if (conexion.Value is ConexionSQLServer)
                    comboBoxSqlServer.Items.Add(conexion.Key);
                else
                    comboBoxOtros.Items.Add(conexion.Key);
            }

            if (comboBoxSqlServer.Items.Count > 0)
                comboBoxSqlServer.SelectedIndex = 0;
            if (comboBoxOtros.Items.Count > 0)
                comboBoxOtros.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBoxSqlServer.SelectedItem == null || comboBoxOtros.SelectedItem == null)
            {
                MessageBox.Show("⚠️ Debes seleccionar una conexión de origen (SQL Server) y una de destino (PostgreSQL o MySQL).");
                return;
            }

            string nombreConexionOrigen = comboBoxSqlServer.SelectedItem.ToString();
            string nombreConexionDestino = comboBoxOtros.SelectedItem.ToString();

            if (!conexiones.TryGetValue(nombreConexionOrigen, out IBaseDatos conexionOrigen) ||
                !conexiones.TryGetValue(nombreConexionDestino, out IBaseDatos conexionDestino))
            {
                MessageBox.Show("❌ No se encontraron una o ambas conexiones seleccionadas.");
                return;
            }

            try
            {
                // Verificación opcional de conectividad antes de abrir el formulario
                if (!conexionOrigen.ProbarConexion())
                {
                    MessageBox.Show("❌ No se pudo conectar con la base de datos de origen.");
                    return;
                }

                if (!conexionDestino.ProbarConexion())
                {
                    MessageBox.Show("❌ No se pudo conectar con la base de datos de destino.");
                    return;
                }

                // Abrir el formulario de migración
                var ventanaMigrar = new MigrasionBasa(conexionOrigen, conexionDestino);
                ventanaMigrar.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Error al intentar abrir la ventana de migración:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
