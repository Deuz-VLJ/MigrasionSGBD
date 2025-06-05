using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConexionesSGBD
{
    public interface IBaseDatos
    {
        void AbrirConexion();  // 🔹 Método para abrir la conexión
        void CerrarConexion();
        bool ProbarConexion();
        List<string> ObtenerBasesDeDatos(); // 🔹 Método agregado
        List<string> ObtenerTablas(string baseDatos);
        Dictionary<string, string> ObtenerAtributos(string baseDatos, string tabla);
        
        List<string> ObtenerIndices();
       
        List<string> ObtenerSecuencias();
        List<string> ObtenerTriggers();
        List<string> ObtenerTiposDeDatos();

        //nuevo

        List<string> ObtenerVistas(string baseDatos);
        List<string> ObtenerLlavesPrimarias(string baseDatos);
        List<string> ObtenerLlavesForaneas(string baseDatos);
        List<string> ObtenerProcedimientos(string baseDatos);


        // 🔹 Nuevo método para ejecutar consultas
        List<string> EjecutarConsulta(string consulta);

        DataTable EjecutarConsultaDataTable(string consulta);

    }
}
