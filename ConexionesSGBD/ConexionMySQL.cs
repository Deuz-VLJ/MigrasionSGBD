using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MySql.Data.MySqlClient;

namespace ConexionesSGBD
{
    public class ConexionMySQL : IBaseDatos
    {
        private readonly MySqlConnection conexion;

        public ConexionMySQL(string servidor, string usuario, string contraseña)
        {
            string cadenaConexion = $"Server={servidor};Database=mysql;User={usuario};Password={contraseña};";
            conexion = new MySqlConnection(cadenaConexion);
        }

        public void AbrirConexion()
        {
            if (conexion.State == ConnectionState.Closed)
            {
                conexion.Open();
            }
        }

        public void CerrarConexion()
        {
            if (conexion.State == ConnectionState.Open)
            {
                conexion.Close();
            }
        }

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

        public List<string> EjecutarConsulta(string consulta)
        {
            List<string> resultados = new List<string>();

            try
            {
                AbrirConexion();
                using (MySqlCommand cmd = new MySqlCommand(consulta, conexion))
                using (MySqlDataReader reader = cmd.ExecuteReader())
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

        public Dictionary<string, string> ObtenerAtributos(string baseDatos, string tabla)
        {
            Dictionary<string, string> atributos = new Dictionary<string, string>();
            string consulta = $@"
        SELECT COLUMN_NAME, DATA_TYPE,
        CASE WHEN COLUMN_KEY = 'PRI' THEN 'PK' ELSE '' END AS KEY_TYPE
        FROM INFORMATION_SCHEMA.COLUMNS
        WHERE TABLE_SCHEMA = '{baseDatos}' AND TABLE_NAME = '{tabla}';";

            try
            {
                AbrirConexion();
                using (MySqlCommand comando = new MySqlCommand(consulta, conexion))
                using (MySqlDataReader lector = comando.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        string columna = lector["COLUMN_NAME"].ToString();
                        string tipo = lector["DATA_TYPE"].ToString();
                        string key = lector["KEY_TYPE"].ToString();
                        atributos[columna] = string.IsNullOrEmpty(key) ? tipo : $"{tipo} {key}";
                    }
                }
            }
            catch (Exception ex)
            {
                atributos.Clear();
                atributos["Error"] = ex.Message;
            }

            return atributos;
        }


        public List<string> ObtenerTablas(string baseDatos)
        {
            List<string> tablas = new List<string>();
            string consulta = $"SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = '{baseDatos}' AND TABLE_TYPE = 'BASE TABLE';";

            try
            {
                AbrirConexion();
                using (MySqlCommand comando = new MySqlCommand(consulta, conexion))
                using (MySqlDataReader lector = comando.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        tablas.Add(lector.GetString(0));
                    }
                }
            }
            catch (Exception ex)
            {
                tablas.Add($"Error: {ex.Message}");
            }

            return tablas;
        }

        public List<string> ObtenerVistas(string baseDatos)
        {
            List<string> vistas = new List<string>();
            string consulta = $"SELECT table_name FROM information_schema.views WHERE table_schema = '{baseDatos}';";

            AbrirConexion();
            using (MySqlCommand cmd = new MySqlCommand(consulta, conexion))
            using (MySqlDataReader reader = cmd.ExecuteReader())
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
        SELECT TABLE_NAME, COLUMN_NAME 
        FROM information_schema.KEY_COLUMN_USAGE 
        WHERE CONSTRAINT_NAME = 'PRIMARY' 
        AND table_schema = '{baseDatos}';";

            AbrirConexion();
            using (MySqlCommand cmd = new MySqlCommand(consulta, conexion))
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    string tabla = reader.GetString("TABLE_NAME");
                    string columna = reader.GetString("COLUMN_NAME");
                    llaves.Add($"{tabla}.{columna}");
                }
            }

            return llaves;
        }

        public List<string> ObtenerLlavesForaneas(string baseDatos)
        {
            List<string> foraneas = new List<string>();
            string consulta = $@"
        SELECT 
            table_name AS tabla_origen,
            column_name AS columna_origen,
            referenced_table_name AS tabla_referenciada,
            referenced_column_name AS columna_referenciada
        FROM information_schema.KEY_COLUMN_USAGE 
        WHERE referenced_table_name IS NOT NULL 
        AND table_schema = '{baseDatos}';";

            AbrirConexion();
            using (MySqlCommand cmd = new MySqlCommand(consulta, conexion))
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    string origen = reader.GetString("tabla_origen");
                    string columnaOrigen = reader.GetString("columna_origen");
                    string destino = reader.GetString("tabla_referenciada");
                    string columnaDestino = reader.GetString("columna_referenciada");
                    foraneas.Add($"FK: {origen}.{columnaOrigen} → {destino}.{columnaDestino}");
                }
            }

            return foraneas;
        }

        public List<string> ObtenerProcedimientos(string baseDatos)
        {
            List<string> procedimientos = new List<string>();
            string consulta = $@"
        SELECT routine_name 
        FROM information_schema.routines 
        WHERE routine_type = 'PROCEDURE' 
        AND routine_schema = '{baseDatos}';";

            AbrirConexion();
            using (MySqlCommand cmd = new MySqlCommand(consulta, conexion))
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    procedimientos.Add(reader.GetString(0));
                }
            }

            return procedimientos;
        }



        public List<string> ObtenerBasesDeDatos()
        {
            return EjecutarConsulta("SHOW DATABASES;");
        }


        public List<string> ObtenerFunciones()
        {
            return EjecutarConsulta("SELECT ROUTINE_NAME FROM information_schema.ROUTINES WHERE ROUTINE_TYPE='FUNCTION' AND ROUTINE_SCHEMA = DATABASE();");
        }

        public List<string> ObtenerTriggers()
        {
            return EjecutarConsulta("SELECT TRIGGER_NAME FROM information_schema.TRIGGERS WHERE TRIGGER_SCHEMA = DATABASE();");
        }

        public List<string> ObtenerTiposDeDatos()
        {
            return EjecutarConsulta("SELECT DISTINCT DATA_TYPE FROM information_schema.COLUMNS WHERE TABLE_SCHEMA = DATABASE();");
        }
        public List<string> ObtenerIndices()
        {
            return EjecutarConsulta("SELECT DISTINCT INDEX_NAME FROM information_schema.statistics WHERE TABLE_SCHEMA = DATABASE();");
        }

        public List<string> ObtenerSecuencias()
        {
            return new List<string>(); // 🔹 MySQL no usa secuencias, utiliza AUTO_INCREMENT en su lugar.
        }




        public void CambiarBaseDatos(string nuevaBase)
        {
            try
            {
                // 🔎 Verificar si la base existe (ignorando mayúsculas/minúsculas)
                List<string> bases = ObtenerBasesDeDatos();
                bool existe = bases.Any(b => string.Equals(b, nuevaBase, StringComparison.OrdinalIgnoreCase));

                if (!existe)
                {
                    // Crear la base si no existe
                    using (var cmdCrear = new MySqlCommand($"CREATE DATABASE `{nuevaBase}`;", conexion))
                    {
                        AbrirConexion();
                        cmdCrear.ExecuteNonQuery();
                    }
                }

                // Cambiar a la nueva base
                if (conexion.State != ConnectionState.Open)
                    conexion.Open();

                conexion.ChangeDatabase(nuevaBase);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al cambiar o crear base de datos '{nuevaBase}' en MySQL: {ex.Message}");
            }
        }


        public void MigrarDesdeSQLServer(ConexionSQLServer origen, string baseOrigen, string baseDestino, List<string> tablas)
        {
            // Verificar si la base existe, si no, crearla
            List<string> bases = ObtenerBasesDeDatos();
            if (!bases.Any(b => string.Equals(b, baseDestino, StringComparison.OrdinalIgnoreCase)))
            {
                AbrirConexion();
                using (var cmdCrear = new MySqlCommand($"CREATE DATABASE `{baseDestino}`;", conexion))
                {
                    cmdCrear.ExecuteNonQuery();
                }
            }

            // Cambiar a la base destino
            CambiarBaseDatos(baseDestino);
            AbrirConexion();

            foreach (var tabla in tablas)
            {
                // Obtener columnas y registros
                var columnas = origen.ObtenerAtributos(baseOrigen, tabla);
                var datos = origen.ObtenerDatosTabla(baseOrigen, tabla); // List<List<object>>

                // Construir estructura de tabla
                var columnasMysql = columnas.Select(c =>
                {
                    string tipoOriginal = c.Value.ToLower();
                    string tipo;

                    if (tipoOriginal.StartsWith("varchar") && tipoOriginal.Contains("(") && tipoOriginal.Contains(")"))
                    {
                        tipo = "LONGTEXT"; // forzamos a LONGTEXT para evitar overflow
                    }
                    else if (tipoOriginal.Contains("int"))
                    {
                        tipo = "BIGINT";
                    }
                    else if (tipoOriginal.Contains("decimal"))
                    {
                        tipo = "DECIMAL(10,2)";
                    }
                    else
                    {
                        tipo = "LONGTEXT";
                    }

                    return $"`{c.Key}` {tipo}";
                });

                // Eliminar la tabla si ya existe
                string scriptDrop = $"DROP TABLE IF EXISTS `{tabla}`;";
                using (var cmdDrop = new MySqlCommand(scriptDrop, conexion))
                {
                    cmdDrop.ExecuteNonQuery();
                }

                // Crear la tabla desde cero
                string scriptCrear = $"CREATE TABLE `{tabla}` ({string.Join(", ", columnasMysql)});";
                using (var cmdCrear = new MySqlCommand(scriptCrear, conexion))
                {
                    cmdCrear.ExecuteNonQuery();
                }

                // Insertar datos
                foreach (var fila in datos)
                {
                    string columnasInsert = string.Join(", ", columnas.Keys.Select(k => $"`{k}`"));

                    var valoresInsert = new List<string>();
                    for (int i = 0; i < fila.Count; i++)
                    {
                        var valor = fila[i];
                        var tipoColumna = columnas.ElementAt(i).Value.ToLower();

                        if (valor == null || string.IsNullOrWhiteSpace(valor.ToString()))
                        {
                            valoresInsert.Add("NULL");
                        }
                        else if (tipoColumna.Contains("decimal") || tipoColumna.Contains("numeric"))
                        {
                            valoresInsert.Add(valor.ToString().Replace(',', '.'));
                        }
                        else if (tipoColumna.Contains("int"))
                        {
                            valoresInsert.Add(valor.ToString());
                        }
                        else
                        {
                            string valorStr = valor.ToString();
                            valoresInsert.Add($"'{valorStr.Replace("'", "''")}'");
                        }
                    }

                    string insert = $"INSERT INTO `{tabla}` ({columnasInsert}) VALUES ({string.Join(", ", valoresInsert)});";
                    using (var cmdInsert = new MySqlCommand(insert, conexion))
                    {
                        cmdInsert.ExecuteNonQuery();
                    }
                }
            }
        }



        private string ExtraerLongitud(string tipo)
        {
            int inicio = tipo.IndexOf("(") + 1;
            int fin = tipo.IndexOf(")");
            if (inicio > 0 && fin > inicio)
            {
                return tipo.Substring(inicio, fin - inicio);
            }
            return "255"; // valor por defecto si no se encuentra
        }



    }
}



