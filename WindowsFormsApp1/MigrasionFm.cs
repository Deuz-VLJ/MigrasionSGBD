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
                MessageBox.Show("Debes seleccionar tanto una conexión de origen (SQL Server) como una de destino (PostgreSQL).");
                return;
            }

            string nombreConexionOrigen = comboBoxSqlServer.SelectedItem.ToString();
            string nombreConexionDestino = comboBoxOtros.SelectedItem.ToString();

            if (!conexiones.ContainsKey(nombreConexionOrigen) || !conexiones.ContainsKey(nombreConexionDestino))
            {
                MessageBox.Show("No se encontraron una o ambas conexiones seleccionadas.");
                return;
            }

            IBaseDatos conexionOrigen = conexiones[nombreConexionOrigen];
            IBaseDatos conexionDestino = conexiones[nombreConexionDestino];

            var ventanaMigrar = new MigrasionBasa(conexionOrigen, conexionDestino);
            ventanaMigrar.ShowDialog();
        }
    }
}
