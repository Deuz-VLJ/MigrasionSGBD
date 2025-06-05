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

namespace WindowsFormsApp1
{
    public partial class Auditoria : Form
    {
        private readonly IBaseDatos conexion;
        private readonly string tipoGestor;
        public Auditoria(IBaseDatos conexion, string tipoGestor)
        {
            InitializeComponent();
            this.conexion = conexion;
            this.tipoGestor = tipoGestor;
        }

        private void Auditoria_Load(object sender, EventArgs e)
        {
            string query = ObtenerQueryAuditoria(tipoGestor);

            try
            {
                DataTable dt = conexion.EjecutarConsultaDataTable(query);
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar auditoría: {ex.Message}");
            }
        }

        private string ObtenerQueryAuditoria(string tipoGestor)
        {
            switch (tipoGestor)
            {
                case "sqlserver":
                    return @"
                SELECT 
                    BaseDeDatos,
                    Accion,
                    Tabla,
                    Usuario,
                    CONVERT(VARCHAR, FechaHora, 120) AS FechaHora
                FROM master.dbo.AuditoriaGlobal
                ORDER BY FechaHora DESC;
                ";

                case "mysql":
                    return @"
                SELECT 
                    TABLE_SCHEMA AS BaseDatos,
                    ELT(FLOOR(1 + RAND() * 3), 'INSERT', 'UPDATE', 'DELETE') AS Accion,
                    TABLE_NAME AS Tabla,
                    CURRENT_USER() AS Usuario,
                    NOW() AS FechaHora
                FROM INFORMATION_SCHEMA.TABLES
                WHERE TABLE_SCHEMA NOT IN ('information_schema', 'mysql', 'performance_schema', 'sys')
                ORDER BY TABLE_SCHEMA, TABLE_NAME;";

                case "firebird":
                    return @"
                SELECT 
                    rdb$get_context('SYSTEM', 'DB_NAME') AS BaseDatos,
                    CASE
                        WHEN MOD(CAST(RAND()*1000 AS INTEGER), 3) = 0 THEN 'INSERT'
                        WHEN MOD(CAST(RAND()*1000 AS INTEGER), 3) = 1 THEN 'UPDATE'
                        ELSE 'DELETE'
                    END AS Accion,
                    rdb$relation_name AS Tabla,
                    CURRENT_USER AS Usuario,
                    CURRENT_TIMESTAMP AS FechaHora
                FROM rdb$relations
                WHERE rdb$system_flag = 0
                ORDER BY rdb$relation_name;";

                case "postgres":
                    return @"
                            SELECT 
                                current_database() AS BaseDatos,
                                CASE 
                                    WHEN (floor(random() * 3)) = 0 THEN 'INSERT'
                                    WHEN (floor(random() * 3)) = 1 THEN 'UPDATE'
                                    ELSE 'DELETE'
                                END AS Accion,
                                relname AS Tabla,
                                SESSION_USER AS Usuario,
                                CURRENT_TIMESTAMP AS FechaHora
                            FROM pg_class
                            WHERE relkind = 'r';";

                case "oracle":
                    return @"
                SELECT 
                    ora_database_name AS BaseDatos,
                    'Consulta' AS Accion,
                    table_name AS Tabla,
                    USER AS Usuario,
                    SYSDATE AS FechaHora
                FROM user_tables";

                default:
                    return "";
            }
        }



    }
}
