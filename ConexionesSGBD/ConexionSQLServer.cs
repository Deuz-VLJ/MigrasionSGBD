using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace ConexionesSGBD
{
    public class ConexionSQLServer : IBaseDatos
    {

        private readonly SqlConnection conexion;

        public ConexionSQLServer(string servidor, string usuario, string contraseña)
        {
            string cadenaConexion = $"Server={servidor};Database=master;User Id={usuario};Password={contraseña};MultipleActiveResultSets=True;";
            conexion = new SqlConnection(cadenaConexion);
        }

        // Abrir conexión si está cerrada
        public void AbrirConexion()
        {
            if (conexion.State == ConnectionState.Closed)
            {
                conexion.Open();
            }
        }

        // Cerrar conexión cuando termine la aplicación
        public void CerrarConexion()
        {
            if (conexion.State == ConnectionState.Open)
            {
                conexion.Close();
            }
        }

        // Probar si la conexión funciona
        public bool ProbarConexion()
        {
            try
            {
                AbrirConexion();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // Método genérico para ejecutar consultas SQL
        public List<string> EjecutarConsulta(string consulta)
        {
            List<string> resultados = new List<string>();

            try
            {
                AbrirConexion();
                using (SqlCommand cmd = new SqlCommand(consulta, conexion))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        resultados.Add(reader.GetString(0).Trim());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en consulta: {ex.Message}");
            }

            return resultados;
        }


        public List<string> ObtenerTablas(string baseDatos)
        {
            List<string> tablas = new List<string>();

            CambiarBaseDatos(baseDatos);

            string consulta = @"SELECT TABLE_SCHEMA + '.' + TABLE_NAME 
                        FROM INFORMATION_SCHEMA.TABLES 
                        WHERE TABLE_TYPE = 'BASE TABLE'";

            using (SqlCommand cmd = new SqlCommand(consulta, conexion))
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    tablas.Add(reader.GetString(0));
                }
            }

            return tablas;
        }


        public Dictionary<string, string> ObtenerAtributos(string baseDatos, string tabla)
        {
            Dictionary<string, string> atributos = new Dictionary<string, string>();

            // Dividir esquema y tabla si vienen juntos
            string esquema = "dbo"; // por defecto
            string nombreTabla = tabla;

            if (tabla.Contains('.'))
            {
                var partes = tabla.Split('.');
                esquema = partes[0];
                nombreTabla = partes[1];
            }

            // Consulta para columnas y tipos con longitud
            string consulta = $@"
    USE [{baseDatos}];
    SELECT COLUMN_NAME, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH 
    FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_NAME = '{nombreTabla}' AND TABLE_SCHEMA = '{esquema}';";

            using (SqlCommand cmd = new SqlCommand(consulta, conexion))
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    string nombre = reader["COLUMN_NAME"].ToString();
                    string tipo = reader["DATA_TYPE"].ToString().ToLower();
                    string longitud = reader["CHARACTER_MAXIMUM_LENGTH"]?.ToString();

                    // Incluir longitud si es varchar o similar
                    if (!string.IsNullOrEmpty(longitud) && int.TryParse(longitud, out int len) && tipo.Contains("char"))
                        atributos[nombre] = $"{tipo}({len})";
                    else
                        atributos[nombre] = tipo;
                }
            }

            // Consulta para claves primarias
            string pkQuery = $@"
        USE [{baseDatos}];
        SELECT COLUMN_NAME 
        FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE 
        WHERE OBJECTPROPERTY(OBJECT_ID(CONSTRAINT_SCHEMA + '.' + CONSTRAINT_NAME), 'IsPrimaryKey') = 1 
        AND TABLE_NAME = '{nombreTabla}' AND TABLE_SCHEMA = '{esquema}';";

            List<string> clavesPrimarias = new List<string>();
            using (SqlCommand cmd = new SqlCommand(pkQuery, conexion))
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    clavesPrimarias.Add(reader["COLUMN_NAME"].ToString());
                }
            }

            // Marcar columnas primarias
            foreach (var pk in clavesPrimarias)
            {
                if (atributos.ContainsKey(pk))
                {
                    atributos[pk] += " (PK)";
                }
            }

            return atributos;
        }




        public List<string> ObtenerVistas(string baseDatos)
        {
            List<string> vistas = new List<string>();
            string consulta = $"USE [{baseDatos}]; SELECT name FROM sys.views;";

            AbrirConexion();
            using (SqlCommand cmd = new SqlCommand(consulta, conexion))
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    vistas.Add(reader.GetString(0));
                }
            }

            return vistas;
        }

        public List<string> ObtenerLlavesPrimarias(string baseDatos)
        {
            List<string> llaves = new List<string>();
            string consulta = $@"
        USE [{baseDatos}];
        SELECT t.name AS Tabla, c.name AS Columna
        FROM sys.indexes i
        INNER JOIN sys.index_columns ic ON i.object_id = ic.object_id AND i.index_id = ic.index_id
        INNER JOIN sys.columns c ON ic.object_id = c.object_id AND c.column_id = ic.column_id
        INNER JOIN sys.tables t ON i.object_id = t.object_id
        WHERE i.is_primary_key = 1;";

            AbrirConexion();
            using (SqlCommand cmd = new SqlCommand(consulta, conexion))
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    string tabla = reader.GetString(0);
                    string columna = reader.GetString(1);
                    llaves.Add($"{tabla}.{columna}");
                }
            }

            return llaves;
        }

        public List<string> ObtenerLlavesForaneas(string baseDatos)
        {
            List<string> foraneas = new List<string>();
            string consulta = $@"
        USE [{baseDatos}];
        SELECT 
            fk.name AS FK_Name,
            tp.name AS TablaPadre,
            cp.name AS ColumnaPadre,
            tr.name AS TablaReferencia,
            cr.name AS ColumnaReferencia
        FROM sys.foreign_keys fk
        INNER JOIN sys.foreign_key_columns fkc ON fkc.constraint_object_id = fk.object_id
        INNER JOIN sys.tables tp ON tp.object_id = fk.parent_object_id
        INNER JOIN sys.columns cp ON cp.object_id = tp.object_id AND cp.column_id = fkc.parent_column_id
        INNER JOIN sys.tables tr ON tr.object_id = fk.referenced_object_id
        INNER JOIN sys.columns cr ON cr.object_id = tr.object_id AND cr.column_id = fkc.referenced_column_id;";

            AbrirConexion();
            using (SqlCommand cmd = new SqlCommand(consulta, conexion))
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    string origen = reader.GetString(1);
                    string colOrigen = reader.GetString(2);
                    string destino = reader.GetString(3);
                    string colDestino = reader.GetString(4);
                    foraneas.Add($"FK: {origen}.{colOrigen} → {destino}.{colDestino}");
                }
            }

            return foraneas;
        }

        public List<string> ObtenerProcedimientos(string baseDatos)
        {
            List<string> procedimientos = new List<string>();
            string consulta = $"USE [{baseDatos}]; SELECT name FROM sys.procedures;";

            AbrirConexion();
            using (SqlCommand cmd = new SqlCommand(consulta, conexion))
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    procedimientos.Add(reader.GetString(0));
                }
            }

            return procedimientos;
        }


        public List<string> ObtenerFunciones()
        {
            return EjecutarConsulta("SELECT name FROM sys.objects WHERE type IN ('FN', 'TF');");
        }

        public List<string> ObtenerTriggers()
        {
            return EjecutarConsulta("SELECT name FROM sys.triggers;");
        }

        public List<string> ObtenerTiposDeDatos()
        {
            return EjecutarConsulta("SELECT name FROM sys.types;");
        }

        public List<string> ObtenerIndices()
        {
            return EjecutarConsulta("SELECT name FROM sys.indexes WHERE is_primary_key = 0 AND is_unique = 0;");
        }

        public List<string> ObtenerSecuencias()
        {
            return EjecutarConsulta("SELECT name FROM sys.sequences;");
        }

        // Obtener bases de datos en el servidor
        public List<string> ObtenerBasesDeDatos()
        {
            return EjecutarConsulta("SELECT name FROM sys.databases;");
        }

        public List<List<object>> ObtenerDatosTabla(string baseDatos, string tablaOriginal)
        {
            List<List<object>> filas = new List<List<object>>();

            CambiarBaseDatos(baseDatos);
            AbrirConexion();

            // tablaOriginal debería venir como "Sales.SalesTaxRate"
            string consulta = $"SELECT * FROM [{tablaOriginal.Replace(".", "].[")}]";

            using (SqlCommand cmd = new SqlCommand(consulta, conexion))
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    List<object> fila = new List<object>();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        fila.Add(reader.GetValue(i));
                    }
                    filas.Add(fila);
                }
            }

            return filas;
        }



        public void CambiarBaseDatos(string baseDatos)
        {
            if (conexion.State == ConnectionState.Open)
                conexion.ChangeDatabase(baseDatos);
            else
            {
                // Modificar la cadena de conexión y volver a abrir
                var builder = new SqlConnectionStringBuilder(conexion.ConnectionString)
                {
                    InitialCatalog = baseDatos
                };

                conexion.ConnectionString = builder.ToString();
                conexion.Open();
            }
        }



    }
}
