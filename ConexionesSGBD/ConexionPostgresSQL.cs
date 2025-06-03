using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace ConexionesSGBD
{
    public class ConexionPostgresSQL : IBaseDatos
    {


        //public ConexionPostgresSQL(string servidor, string usuario, string contraseña)
        //{
        //    string cadenaConexion = $"host={servidor};Database=postgres;Username={usuario};Password={contraseña};Port=5432;";
        //    conexion = new NpgsqlConnection(cadenaConexion);
        //}


        private NpgsqlConnection conexion;

        private string host;
        private string usuario;
        private string contrasena;
        private string baseDatosActual;

        public ConexionPostgresSQL(string servidor, string usuario, string contraseña)
        {
            this.host = servidor;
            this.usuario = usuario;
            this.contrasena = contraseña;
            this.baseDatosActual = "postgres"; // default
            RecrearConexion();
        }

        private void RecrearConexion()
        {
            string cadena = $"Host={host};Database={baseDatosActual};Username={usuario};Password={contrasena};Port=5432;";
            conexion = new NpgsqlConnection(cadena);
        }

        public void CambiarBaseDatos(string nuevaBase)
        {
            // Verificar si la base existe en el servidor
            List<string> bases = ObtenerBasesDeDatos();

            if (!bases.Contains(nuevaBase))
            {
                // Si no existe, crearla usando la conexión a la base por defecto (postgres)
                string cadenaTemporal = $"Host={host};Database=postgres;Username={usuario};Password={contrasena};Port=5432;";
                using (var conexionTemporal = new NpgsqlConnection(cadenaTemporal))
                {
                    conexionTemporal.Open();
                    using (var cmd = new NpgsqlCommand($"CREATE DATABASE \"{nuevaBase}\"", conexionTemporal))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }

            // Cambiar la base de datos actual
            baseDatosActual = nuevaBase;

            if (conexion.State == ConnectionState.Open)
                conexion.Close();

            RecrearConexion(); // Recrear la conexión con la nueva base
        }

        public void AbrirConexion()
        {
            if (conexion.State != ConnectionState.Open)
                conexion.Open();
        }

        public void CerrarConexion()
        {
            if (conexion.State == ConnectionState.Open)
                conexion.Close();
        }

        public bool ProbarConexion()
        {
            try
            {
                AbrirConexion();
                return true;
            }
            catch
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
                using (var cmd = new NpgsqlCommand(consulta, conexion))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        resultados.Add(reader.GetString(0).Trim());
                }
            }
            catch (Exception ex)
            {
                resultados.Add("Error: " + ex.Message);
            }
            return resultados;
        }

        public List<string> ObtenerBasesDeDatos()
        {
            return EjecutarConsulta("SELECT datname FROM pg_database WHERE datistemplate = false;");
        }

        public List<string> ObtenerTablas(string baseDatos)
        {
            CambiarBaseDatos(baseDatos);
            List<string> tablas = new List<string>();

            AbrirConexion();
            NpgsqlCommand cmd = new NpgsqlCommand("SELECT table_name FROM information_schema.tables WHERE table_schema = 'public' AND table_type = 'BASE TABLE';", conexion);
            NpgsqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                tablas.Add(reader.GetString(0));
            }
            reader.Close();

            return tablas;
        }

        public Dictionary<string, string> ObtenerAtributos(string baseDatos, string tabla)
        {
            CambiarBaseDatos(baseDatos);
            Dictionary<string, string> atributos = new Dictionary<string, string>();

            string consulta = $@"
                SELECT 
                    cols.column_name, 
                    cols.data_type,
                    CASE 
                        WHEN pk.column_name IS NOT NULL THEN 'PK'
                        ELSE ''
                    END AS key_type
                FROM information_schema.columns cols
                LEFT JOIN (
                    SELECT kcu.column_name
                    FROM information_schema.table_constraints tc
                    JOIN information_schema.key_column_usage kcu 
                        ON tc.constraint_name = kcu.constraint_name
                    WHERE tc.constraint_type = 'PRIMARY KEY' AND kcu.table_name = '{tabla}'
                ) pk ON cols.column_name = pk.column_name
                WHERE cols.table_name = '{tabla}' AND cols.table_schema = 'public';";

            AbrirConexion();
            NpgsqlCommand cmd = new NpgsqlCommand(consulta, conexion);
            NpgsqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string nombre = reader["column_name"].ToString();
                string tipo = reader["data_type"].ToString();
                string key = reader["key_type"].ToString();
                atributos[nombre] = string.IsNullOrEmpty(key) ? tipo : $"{tipo} {key}";
            }
            reader.Close();

            return atributos;
        }

        public List<string> ObtenerVistas(string baseDatos)
        {
            CambiarBaseDatos(baseDatos);
            return EjecutarConsulta("SELECT table_name FROM information_schema.views WHERE table_schema = 'public';");
        }

        public List<string> ObtenerLlavesPrimarias(string baseDatos)
        {
            CambiarBaseDatos(baseDatos);
            return EjecutarConsulta(@"
                SELECT kcu.table_name || '.' || kcu.column_name
                FROM information_schema.table_constraints tc
                JOIN information_schema.key_column_usage kcu ON tc.constraint_name = kcu.constraint_name
                WHERE tc.constraint_type = 'PRIMARY KEY' AND tc.table_schema = 'public';");
        }

        public List<string> ObtenerLlavesForaneas(string baseDatos)
        {
            CambiarBaseDatos(baseDatos);
            List<string> fks = new List<string>();

            string consulta = @"
                SELECT 
                    tc.table_name, kcu.column_name, 
                    ccu.table_name AS ref_table, 
                    ccu.column_name AS ref_column
                FROM information_schema.table_constraints tc
                JOIN information_schema.key_column_usage kcu ON tc.constraint_name = kcu.constraint_name
                JOIN information_schema.constraint_column_usage ccu ON ccu.constraint_name = tc.constraint_name
                WHERE tc.constraint_type = 'FOREIGN KEY' AND tc.table_schema = 'public';";

            AbrirConexion();
            NpgsqlCommand cmd = new NpgsqlCommand(consulta, conexion);
            NpgsqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string origen = reader.GetString(0);
                string colOrigen = reader.GetString(1);
                string destino = reader.GetString(2);
                string colDestino = reader.GetString(3);
                fks.Add($"FK: {origen}.{colOrigen} → {destino}.{colDestino}");
            }
            reader.Close();

            return fks;
        }

        public List<string> ObtenerProcedimientos(string baseDatos)
        {
            CambiarBaseDatos(baseDatos);
            return EjecutarConsulta(@"
                SELECT routine_name
                FROM information_schema.routines
                WHERE routine_type = 'PROCEDURE' AND specific_schema = 'public';");
        }



        public List<string> ObtenerFunciones()
        {
            return EjecutarConsulta("SELECT routine_name FROM information_schema.routines WHERE routine_schema = 'public';");
        }

        public List<string> ObtenerTriggers()
        {
            return EjecutarConsulta("SELECT trigger_name FROM information_schema.triggers WHERE trigger_schema = 'public';");
        }

        public List<string> ObtenerTiposDeDatos()
        {
            return EjecutarConsulta("SELECT DISTINCT data_type FROM information_schema.columns WHERE table_schema = 'public';");
        }

        public List<string> ObtenerIndices()
        {
            return EjecutarConsulta("SELECT indexname FROM pg_indexes WHERE schemaname = 'public';");
        }

        public List<string> ObtenerSecuencias()
        {
            return EjecutarConsulta("SELECT sequence_name FROM information_schema.sequences WHERE sequence_schema = 'public';");
        }


        public void MigrarDesdeSQLServer(ConexionSQLServer origen, string baseOrigen, string baseDestino, List<string> tablas)
        {
            // Cambiar a base destino y abrir conexión en PostgreSQL
            CambiarBaseDatos(baseDestino);
            AbrirConexion();

            foreach (var tabla in tablas)
            {
                string nombreTablaPostgres = tabla.Replace('.', '_'); // PostgreSQL no acepta puntos en nombres

                // Obtener columnas y datos desde SQL Server
                var columnas = origen.ObtenerAtributos(baseOrigen, tabla); // Dictionary<string, string>
                var datos = origen.ObtenerDatosTabla(baseOrigen, tabla);   // List<List<object>>

                // Crear tabla en PostgreSQL (eliminar si ya existe)
                string scriptDrop = $"DROP TABLE IF EXISTS \"{nombreTablaPostgres}\";";
                using (var cmdDrop = new NpgsqlCommand(scriptDrop, conexion))
                {
                    cmdDrop.ExecuteNonQuery();
                }

                var columnasPostgres = columnas.Select(c =>
                {
                    string tipoSQL = c.Value.ToLower().Split('(')[0]; // remover longitudes como varchar(255)

                    string tipoPostgres =
                        tipoSQL.Contains("int") ? "INTEGER" :
                        tipoSQL.Contains("decimal") || tipoSQL.Contains("numeric") ? "DECIMAL" :
                        tipoSQL.Contains("float") || tipoSQL.Contains("real") ? "DOUBLE PRECISION" :
                        tipoSQL.Contains("char") || tipoSQL.Contains("text") || tipoSQL.Contains("varchar") ? "TEXT" :
                        "TEXT"; // tipo por defecto

                    string nombreColumna = c.Key.Replace('.', '_');
                    return $"\"{nombreColumna}\" {tipoPostgres}";
                });

                string scriptCrear = $"CREATE TABLE \"{nombreTablaPostgres}\" ({string.Join(", ", columnasPostgres)});";
                using (var cmdCrear = new NpgsqlCommand(scriptCrear, conexion))
                {
                    cmdCrear.ExecuteNonQuery();
                }

                // Insertar datos en PostgreSQL
                foreach (var fila in datos)
                {
                    string columnasInsert = string.Join(", ", columnas.Keys.Select(k => $"\"{k.Replace('.', '_')}\""));

                    string valoresInsert = string.Join(", ", fila.Select((v, i) =>
                    {
                        string tipoCol = columnas.ElementAt(i).Value.ToLower();

                        if (v == null || string.IsNullOrWhiteSpace(v.ToString()))
                            return "NULL";

                        if (tipoCol.Contains("decimal") || tipoCol.Contains("numeric") || tipoCol.Contains("float") || tipoCol.Contains("real"))
                            return v.ToString().Replace(',', '.'); // PostgreSQL usa punto decimal

                        if (tipoCol.Contains("int"))
                            return v.ToString();

                        // Escapar comillas para texto
                        return $"'{v.ToString().Replace("'", "''")}'";
                    }));

                    string insert = $"INSERT INTO \"{nombreTablaPostgres}\" ({columnasInsert}) VALUES ({valoresInsert});";

                    using (var cmdInsert = new NpgsqlCommand(insert, conexion))
                    {
                        cmdInsert.ExecuteNonQuery();
                    }
                }
            }
        }




        NpgsqlConnection GetConexion()
        {
            return conexion;
        }


    }
}
