using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using FirebirdSql.Data.FirebirdClient;

namespace ConexionesSGBD
{

    //J:\Documentos\TEC\ADMINISTRACION DE BASE DE DATOS\Aprendisaje firebird\Base de datos\BASE DE DATOS PRUEBA.GDB
    public class ConexionFirebird : IBaseDatos
    {

        private readonly FbConnection conexion;

        public ConexionFirebird(string servidor, string rutaBD, string usuario, string contraseña)
        {
            string cadenaConexion = $"User={usuario};Password={contraseña};Database={rutaBD};DataSource={servidor};Port=3050;Dialect=3;Charset=UTF8;";
            conexion = new FbConnection(cadenaConexion);
        }

        // Abrir conexión (si está cerrada)
        public void AbrirConexion()
        {
            if (conexion.State == ConnectionState.Closed)
            {
                conexion.Open();
            }
        }

        // Cerrar conexión (cuando la aplicación termine)
        public void CerrarConexion()
        {
            if (conexion.State == ConnectionState.Open)
            {
                conexion.Close();
            }
        }

        // Método para probar la conexión
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

        // Método genérico para ejecutar consultas usando la misma conexión
        //Aun no se usa
        public List<string> EjecutarConsulta(string consulta)
        {
            List<string> resultados = new List<string>();

            try
            {
                AbrirConexion();  // Asegurar que la conexión esté abierta
                using (FbCommand cmd = new FbCommand(consulta, conexion))
                using (FbDataReader reader = cmd.ExecuteReader())
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


        //cm
        public Dictionary<string, string> ObtenerAtributos(string baseDatos, string tabla)
        {
            Dictionary<string, string> atributos = new Dictionary<string, string>();
            string consulta = $@"
        SELECT rf.RDB$FIELD_NAME, f.RDB$FIELD_TYPE, co.RDB$CONSTRAINT_TYPE
        FROM RDB$RELATION_FIELDS rf
        JOIN RDB$FIELDS f ON rf.RDB$FIELD_SOURCE = f.RDB$FIELD_NAME
        LEFT JOIN RDB$INDEX_SEGMENTS ixs ON ixs.RDB$FIELD_NAME = rf.RDB$FIELD_NAME
        LEFT JOIN RDB$INDICES ix ON ix.RDB$INDEX_NAME = ixs.RDB$INDEX_NAME
        LEFT JOIN RDB$RELATION_CONSTRAINTS co ON co.RDB$INDEX_NAME = ix.RDB$INDEX_NAME
        WHERE rf.RDB$RELATION_NAME = '{tabla.ToUpper()}';";

            try
            {
                AbrirConexion();
                using (FbCommand cmd = new FbCommand(consulta, conexion))
                using (FbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string campo = reader.GetString(0).Trim();
                        string tipoDato = reader.GetInt16(1).ToString(); // Opcionalmente podrías mapear los tipos Firebird
                        string esPK = !reader.IsDBNull(2) && reader.GetString(2).Trim() == "PRIMARY KEY" ? "PK" : "";
                        atributos[campo] = string.IsNullOrEmpty(esPK) ? tipoDato : $"{tipoDato} {esPK}";
                    }
                }
            }
            catch (Exception ex)
            {
                atributos.Add("Error", ex.Message);
            }

            return atributos;
        }



        // Métodos para obtener la estructura de la base de datos
        public List<string> ObtenerBasesDeDatos()
        {
            List<string> basesDeDatos = new List<string>();

            try
            {
                if (conexion.State != ConnectionState.Open)
                {
                    conexion.Open();
                }

                // 🔹 Intentar obtener el nombre de la base de datos desde `MON$DATABASE`
                string consulta = "SELECT TRIM(MON$DATABASE_NAME) AS DATABASE_PATH FROM MON$DATABASE;";

                using (FbCommand comando = new FbCommand(consulta, conexion))
                using (FbDataReader lector = comando.ExecuteReader())
                {
                    if (lector.Read())
                    {
                        basesDeDatos.Add(lector["DATABASE_PATH"].ToString().Trim());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en ObtenerBasesDeDatos() Firebird: {ex.Message}");
            }

            return basesDeDatos;
        }



        //cm
        public List<string> ObtenerTablas(string baseDatos)
        {
            List<string> tablas = new List<string>();
            string consulta = "SELECT RDB$RELATION_NAME FROM RDB$RELATIONS WHERE RDB$SYSTEM_FLAG = 0;";

            try
            {
                AbrirConexion();
                using (FbCommand cmd = new FbCommand(consulta, conexion))
                using (FbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tablas.Add(reader.GetString(0).Trim());
                    }
                }
            }
            catch (Exception ex)
            {
                tablas.Add($"Error: {ex.Message}");
            }

            return tablas;
        }


        // 🔹 Método para obtener el nombre de la base de datos activa en Firebird
        private string ObtenerNombreBaseDeDatos()
        {
            string nombreBD = "FirebirdDB"; // 🔹 Valor por defecto si no se encuentra

            try
            {
                string consulta = "SELECT MON$DATABASE_NAME FROM MON$DATABASE;";

                using (FbCommand comando = new FbCommand(consulta, conexion))
                using (FbDataReader lector = comando.ExecuteReader())
                {
                    if (lector.Read())
                    {
                        nombreBD = lector["MON$DATABASE_NAME"].ToString().Trim();
                    }
                }
            }
            catch (Exception)
            {
                // 🔹 Si no se puede obtener, sigue con el nombre predeterminado
            }

            return nombreBD;
        }



        public List<string> ObtenerVistas(string baseDatos)
        {
            List<string> vistas = new List<string>();
            string consulta = "SELECT RDB$RELATION_NAME FROM RDB$RELATIONS WHERE RDB$VIEW_BLR IS NOT NULL AND RDB$SYSTEM_FLAG = 0;";

            AbrirConexion();
            using (FbCommand cmd = new FbCommand(consulta, conexion))
            using (FbDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    vistas.Add(reader.GetString(0).Trim());
                }
            }

            return vistas;
        }

        public List<string> ObtenerLlavesPrimarias(string baseDatos)
        {
            List<string> llaves = new List<string>();
            string consulta = @"
        SELECT
            sg.RDB$RELATION_NAME AS TABLA,
            si.RDB$FIELD_NAME AS COLUMNA
        FROM
            RDB$RELATION_CONSTRAINTS sg
            JOIN RDB$INDEX_SEGMENTS si ON sg.RDB$INDEX_NAME = si.RDB$INDEX_NAME
        WHERE
            sg.RDB$CONSTRAINT_TYPE = 'PRIMARY KEY';";

            AbrirConexion();
            using (FbCommand cmd = new FbCommand(consulta, conexion))
            using (FbDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    string tabla = reader.GetString(0).Trim();
                    string columna = reader.GetString(1).Trim();
                    llaves.Add($"{tabla}.{columna}");
                }
            }

            return llaves;
        }


        public List<string> ObtenerLlavesForaneas(string baseDatos)
        {
            List<string> foraneas = new List<string>();
            string consulta = @"
        SELECT
            c.RDB$RELATION_NAME AS TABLA_ORIGEN,
            i.RDB$FIELD_NAME AS COLUMNA_ORIGEN,
            rc.RDB$CONST_NAME_UQ AS FK_NAME
        FROM
            RDB$RELATION_CONSTRAINTS c
            JOIN RDB$INDEX_SEGMENTS i ON c.RDB$INDEX_NAME = i.RDB$INDEX_NAME
            JOIN RDB$REF_CONSTRAINTS rc ON c.RDB$CONSTRAINT_NAME = rc.RDB$CONSTRAINT_NAME
        WHERE
            c.RDB$CONSTRAINT_TYPE = 'FOREIGN KEY';";

            AbrirConexion();
            using (FbCommand cmd = new FbCommand(consulta, conexion))
            using (FbDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    string tabla = reader.GetString(0).Trim();
                    string columna = reader.GetString(1).Trim();
                    string fk = reader.GetString(2).Trim();
                    foraneas.Add($"FK {fk}: {tabla}.{columna}");
                }
            }

            return foraneas;
        }


        public List<string> ObtenerProcedimientos(string baseDatos)
        {
            List<string> procedimientos = new List<string>();
            string consulta = "SELECT RDB$PROCEDURE_NAME FROM RDB$PROCEDURES;";

            AbrirConexion();
            using (FbCommand cmd = new FbCommand(consulta, conexion))
            using (FbDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    procedimientos.Add(reader.GetString(0).Trim());
                }
            }

            return procedimientos;
        }


        public List<string> ObtenerIndices()
        {
            return EjecutarConsulta("SELECT RDB$INDEX_NAME FROM RDB$INDICES;");
        }

        public List<string> ObtenerSecuencias()
        {
            return EjecutarConsulta("SELECT RDB$GENERATOR_NAME FROM RDB$GENERATORS;");
        }

        public List<string> ObtenerTriggers()
        {
            return EjecutarConsulta("SELECT RDB$TRIGGER_NAME FROM RDB$TRIGGERS;");
        }

        public List<string> ObtenerTiposDeDatos()
        {
            return EjecutarConsulta("SELECT DISTINCT RDB$FIELD_NAME FROM RDB$FIELDS;");
        }

        public DataTable EjecutarConsultaDataTable(string consulta)
        {
            DataTable dt = new DataTable();
            try
            {
                AbrirConexion();
                using (FbCommand cmd = new FbCommand(consulta, conexion))
                using (FbDataAdapter adapter = new FbDataAdapter(cmd))
                {
                    adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al ejecutar consulta DataTable en Firebird: {ex.Message}");
            }
            return dt;
        }



    }
}
