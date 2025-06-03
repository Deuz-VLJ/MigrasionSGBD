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


        private async void button1_Click(object sender, EventArgs e)
        {
            string baseOrigen = comboBoxBases.SelectedItem?.ToString();
            string baseDestino = baseOrigen;

            var tablas = new List<string>();
            foreach (var item in checkedListBoxTablas.CheckedItems)
            {
                tablas.Add(item.ToString());
            }

            if (!string.IsNullOrEmpty(baseOrigen) && !string.IsNullOrEmpty(baseDestino) && tablas.Count > 0)
            {
                if (conexionInicio is ConexionSQLServer sqlServer)
                {
                    if (conexionDestino is ConexionPostgresSQL postgres || conexionDestino is ConexionMySQL mysql)
                    {
                        Form2 mensaje = new Form2();

                        Task migrar = Task.Run(() =>
                        {
                            if (conexionDestino is ConexionPostgresSQL pg)
                                pg.MigrarDesdeSQLServer(sqlServer, baseOrigen, baseDestino, tablas);
                            else if (conexionDestino is ConexionMySQL my)
                                my.MigrarDesdeSQLServer(sqlServer, baseOrigen, baseDestino, tablas);
                        });

                        mensaje.Show();
                        await migrar;
                        mensaje.Close();

                        MessageBox.Show("✅ Migración completada con éxito.");
                    }
                    else
                    {
                        MessageBox.Show("❌ El destino seleccionado no es compatible.");
                    }
                }
                else
                {
                    MessageBox.Show("❌ Solo se admite SQL Server como origen.");
                }
            }
            else
            {
                MessageBox.Show("⚠️ Debe seleccionar una base y al menos una tabla.");
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBoxTablas.Items.Count; i++)
            {
                checkedListBoxTablas.SetItemChecked(i, true);
            }
        }
    }
}
