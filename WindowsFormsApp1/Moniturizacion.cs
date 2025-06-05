using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using ConexionesSGBD;

namespace WindowsFormsApp1
{
    public partial class Moniturizacion : Form
    {
        private readonly ConexionSQLServer conexion;

        public Moniturizacion(ConexionSQLServer conexionSQL)
        {
            InitializeComponent();
            conexion = conexionSQL;
            ConfigurarGraficas();
        }

        private void Moniturizacion_Load(object sender, EventArgs e)
        {
            timer1.Interval = 1000; // 1 segundo
            timer1.Tick += timer1_Tick;
            timer1.Start();
        }

        private void ConfigurarGraficas()
        {
            ConfigurarGrafica(chartCPU, "CPU (%)", "Uso de CPU");
            ConfigurarGrafica(chartRAM, "RAM (MB)", "RAM en uso");
            ConfigurarGrafica(chartNetwork, "Conexiones", "Conexiones activas");
        }

        private void ConfigurarGrafica(Chart chart, string nombreSerie, string titulo)
        {
            chart.Series.Clear();
            chart.ChartAreas.Clear();
            chart.Titles.Clear();

            chart.ChartAreas.Add("Area");
            chart.Series.Add(nombreSerie);
            chart.Series[nombreSerie].ChartType = SeriesChartType.Line;
            chart.Series[nombreSerie].BorderWidth = 2;
            chart.Series[nombreSerie].IsValueShownAsLabel = true;

            chart.Titles.Add(titulo);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                var datos = ObtenerDatosSQLServer();
                float cpu = datos.Item1;
                float ram = datos.Item2;
                float conexiones = datos.Item3;

                ActualizarGrafico(chartCPU, "CPU (%)", cpu);
                ActualizarGrafico(chartRAM, "RAM (MB)", ram);
                ActualizarGrafico(chartNetwork, "Conexiones", conexiones);

                labelCPU.Text = $"CPU: {cpu:F2}%";
                labelRAM.Text = $"RAM: {ram:F2} MB";
                labelConexiones.Text = $"Conexiones: {conexiones}";
            }
            catch (Exception ex)
            {
                timer1.Stop();
                MessageBox.Show("❌ Error al consultar SQL Server:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ActualizarGrafico(Chart chart, string serie, float valor)
        {
            var puntos = chart.Series[serie].Points;
            puntos.AddY(valor);

            if (puntos.Count > 30)
                puntos.RemoveAt(0);

            chart.ResetAutoValues();
        }



        private Tuple<float, float, float> ObtenerDatosSQLServer()
        {
            float cpuEstimado = 0, ramUso = 0, conexionesActivas = 0;

            SqlConnection conn = conexion.GetConnection(); // ya está abierta

            string consulta = @"
        SELECT 
            (SELECT COUNT(*) FROM sys.dm_exec_requests) AS CpuEstimado,
            (SELECT (total_physical_memory_kb - available_physical_memory_kb) / 1024.0 
             FROM sys.dm_os_sys_memory) AS MemoriaEnUso,
            (SELECT COUNT(*) FROM sys.dm_exec_connections) AS Conexiones";

            using (var cmd = new SqlCommand(consulta, conn))
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    cpuEstimado = reader.IsDBNull(0) ? 0 : Convert.ToSingle(reader.GetValue(0));
                    ramUso = reader.IsDBNull(1) ? 0 : Convert.ToSingle(reader.GetValue(1));
                    conexionesActivas = reader.IsDBNull(2) ? 0 : Convert.ToSingle(reader.GetValue(2));
                }
            }

            return Tuple.Create(cpuEstimado, ramUso, conexionesActivas);
        }







    }
}
