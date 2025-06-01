using ConexionesSGBD;
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

namespace WindowsFormsApp1
{
    public partial class MigrasionBasa : Form
    {
        private readonly IBaseDatos conexionInicio;
        private readonly IBaseDatos conexionDestino;

        public MigrasionBasa(IBaseDatos conexionInicio, IBaseDatos conexionDestino)
        {
            InitializeComponent();
            this.conexionInicio = conexionInicio;
            this.conexionDestino = conexionDestino;
        }

        private void MigrasionBasa_Load(object sender, EventArgs e)
        {
            comboBoxBases.Items.Clear();
            var bases = conexionInicio.ObtenerBasesDeDatos();
            comboBoxBases.Items.AddRange(bases.ToArray());

            if (comboBoxBases.Items.Count > 0)
                comboBoxBases.SelectedIndex = 0;
        }

        private void comboBoxBases_SelectedIndexChanged(object sender, EventArgs e)
        {
            checkedListBoxTablas.Items.Clear();

            if (comboBoxBases.SelectedItem == null) return;

            string baseSeleccionada = comboBoxBases.SelectedItem.ToString();
            var tablas = conexionInicio.ObtenerTablas(baseSeleccionada);
            checkedListBoxTablas.Items.AddRange(tablas.ToArray());
        }


        private void button1_Click(object sender, EventArgs e)
        {
            string baseOrigen = comboBoxBases.SelectedItem?.ToString();
            string baseDestino = baseOrigen; // o usa otro ComboBox si tienes uno para destino

            var tablas = new List<string>();
            foreach (var item in checkedListBoxTablas.CheckedItems)
            {
                tablas.Add(item.ToString());
            }

            if (!string.IsNullOrEmpty(baseOrigen) && !string.IsNullOrEmpty(baseDestino) && tablas.Count > 0)
            {
                if (conexionInicio is ConexionSQLServer sqlServer)
                {
                    if (conexionDestino is ConexionPostgresSQL postgres)
                    {
                        postgres.MigrarDesdeSQLServer(sqlServer, baseOrigen, baseDestino, tablas);
                        MessageBox.Show("Migración completada con éxito a PostgreSQL.");
                    }
                    else if (conexionDestino is ConexionMySQL mysql)
                    {
                        mysql.MigrarDesdeSQLServer(sqlServer, baseOrigen, baseDestino, tablas);
                        MessageBox.Show("Migración completada con éxito a MySQL.");
                    }
                    else
                    {
                        MessageBox.Show("Destino no compatible para migración desde SQL Server.");
                    }
                }
                else
                {
                    MessageBox.Show("Origen no compatible. Solo se admite SQL Server como origen.");
                }
            }
            else
            {
                MessageBox.Show("Debe seleccionar una base de datos y al menos una tabla.");
            }
        }



    }
}
