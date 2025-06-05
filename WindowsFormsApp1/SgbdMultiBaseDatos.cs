using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ConexionesSGBD;

namespace WindowsFormsApp1
{
    public partial class SgbdMultiBaseDatos : Form
    {
        private Dictionary<string, IBaseDatos> conexiones;
        private IBaseDatos conexionActual;
        private string nombreConexionActual;

        public SgbdMultiBaseDatos(Dictionary<string, IBaseDatos> conexiones)
        {
            InitializeComponent();
            this.conexiones = conexiones ?? new Dictionary<string, IBaseDatos>();
            this.treeViewBD.MouseUp += new System.Windows.Forms.MouseEventHandler(this.treeViewBD_MouseUp);

        }

        private void SgbdMultiBaseDatos_Load(object sender, EventArgs e)
        {
            CargarListaConexiones();
            LlenarTreeView();
        }

        private void CargarListaConexiones()
        {
            treeViewBD.Nodes.Clear();
            foreach (var conexion in conexiones)
            {
                TreeNode conexionNode = new TreeNode(conexion.Key) { Tag = "conexion" };
                treeViewBD.Nodes.Add(conexionNode);
            }
            if (treeViewBD.Nodes.Count > 0)
            {
                treeViewBD.SelectedNode = treeViewBD.Nodes[0];
                CambiarConexion(treeViewBD.SelectedNode.Text);
            }
        }

        private void TreeViewBD_AfterSelect_1(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag?.ToString() == "conexion")
            {
                CambiarConexion(e.Node.Text);
            }
            else if (e.Node.Parent != null && e.Node.Parent.Tag?.ToString() == "conexion")
            {
                CambiarConexion(e.Node.Parent.Text);
                string bd = e.Node.Text.Replace("Base de Datos: ", "");
                if (comboBoxBD.Items.Contains(bd)) comboBoxBD.SelectedItem = bd;
            }
        }

        private void CambiarConexion(string nombreConexion)
        {
            if (!conexiones.ContainsKey(nombreConexion))
                return;

            // 🔴 Cerrar la conexión anterior si existe
            conexionActual?.CerrarConexion();

            // 🔁 Cambiar conexión
            conexionActual = conexiones[nombreConexion];
            nombreConexionActual = nombreConexion;

            // ✅ Abrir conexión sin cambiar aún de base de datos
            conexionActual.AbrirConexion();

            // 🔄 Cargar bases de datos al ComboBox
            CargarListaBasesDatos();

            // 🔄 Si hay una base seleccionada en el ComboBox, cambiar a ella (especialmente útil para PostgreSQL)
            if (comboBoxBD.SelectedItem != null)
            {
                string baseSeleccionada = comboBoxBD.SelectedItem.ToString();

                // 🔁 Cambiar de base de datos si es PostgreSQL o MySQL
                if (conexionActual is ConexionPostgresSQL postgres)
                {
                    try
                    {
                        postgres.CambiarBaseDatos(baseSeleccionada);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al cambiar base de datos en PostgreSQL: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else if (conexionActual is ConexionMySQL mysql)
                {
                    try
                    {
                        mysql.CambiarBaseDatos(baseSeleccionada);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al cambiar base de datos en MySQL: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            // 🔄 Actualizar el TreeView con los datos actualizados de la conexión y base
            LlenarTreeView();
        }

        private void CargarListaBasesDatos()
        {
            comboBoxBD.Items.Clear();
            if (conexionActual == null) return;
            var bases = conexionActual.ObtenerBasesDeDatos();
            comboBoxBD.Items.AddRange(bases.ToArray());
            if (bases.Count > 0) comboBoxBD.SelectedIndex = 0;
        }

        //private void comboBoxBD_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (comboBoxBD.SelectedItem == null) return;
        //    LlenarTreeView();
        //}

        private void comboBoxBD_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxBD.SelectedItem == null || conexionActual == null)
                return;

            string baseDatos = comboBoxBD.SelectedItem.ToString();

            // 🔹 Solo para PostgreSQL: recrear la conexión con la nueva base
            if (conexionActual is ConexionPostgresSQL postgres)
            {
                try
                {
                    postgres.CambiarBaseDatos(baseDatos);
                    postgres.AbrirConexion(); // 🔄 Asegurarse de abrir la nueva conexión
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"No se pudo cambiar la base de datos en PostgreSQL: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            // 🔄 Refrescar vista con la nueva base seleccionada
            LlenarTreeView();
        }


        private void LlenarTreeView()
        {
            if (conexionActual == null || string.IsNullOrEmpty(nombreConexionActual)) return;

            TreeNode conexionNode = treeViewBD.Nodes.Cast<TreeNode>().FirstOrDefault(n => n.Text == nombreConexionActual);
            if (conexionNode == null)
            {
                conexionNode = new TreeNode(nombreConexionActual) { Tag = "conexion" };
                treeViewBD.Nodes.Add(conexionNode);
            }

            conexionNode.Nodes.Clear();

            foreach (string baseDatos in conexionActual.ObtenerBasesDeDatos())
            {
                TreeNode bdNode = new TreeNode("Base de Datos: " + baseDatos) { Tag = "baseDatos" };

                // Tablas
                TreeNode tablasNode = new TreeNode("Tablas");
                var tablas = conexionActual.ObtenerTablas(baseDatos);
                foreach (var tabla in tablas)
                {
                    TreeNode tablaNode = new TreeNode(tabla);

                    // Atributos
                    var atributos = conexionActual.ObtenerAtributos(baseDatos, tabla);
                    foreach (var atributo in atributos)
                    {
                        tablaNode.Nodes.Add(new TreeNode($"{atributo.Key} ({atributo.Value})"));
                    }

                    tablasNode.Nodes.Add(tablaNode);
                }
                if (tablasNode.Nodes.Count > 0)
                    bdNode.Nodes.Add(tablasNode);

                // Vistas
                TreeNode vistasNode = new TreeNode("Vistas");
                var vistas = conexionActual.ObtenerVistas(baseDatos);
                foreach (var vista in vistas)
                {
                    vistasNode.Nodes.Add(new TreeNode(vista));
                }
                if (vistasNode.Nodes.Count > 0)
                    bdNode.Nodes.Add(vistasNode);

                // Llaves (Primarias y Foráneas en un solo nodo)
                TreeNode llavesNode = new TreeNode("Llaves");

                TreeNode pkNode = new TreeNode("Llaves Primarias");
                var pks = conexionActual.ObtenerLlavesPrimarias(baseDatos);
                foreach (var pk in pks)
                {
                    pkNode.Nodes.Add(new TreeNode(pk));
                }

                TreeNode fkNode = new TreeNode("Llaves Foráneas");
                var fks = conexionActual.ObtenerLlavesForaneas(baseDatos);
                foreach (var fk in fks)
                {
                    fkNode.Nodes.Add(new TreeNode(fk));
                }

                if (pkNode.Nodes.Count > 0 || fkNode.Nodes.Count > 0)
                {
                    if (pkNode.Nodes.Count > 0)
                        llavesNode.Nodes.Add(pkNode);
                    if (fkNode.Nodes.Count > 0)
                        llavesNode.Nodes.Add(fkNode);

                    bdNode.Nodes.Add(llavesNode);
                }

                // Procedimientos Almacenados
                TreeNode procNode = new TreeNode("Procedimientos");
                var procs = conexionActual.ObtenerProcedimientos(baseDatos);
                foreach (var proc in procs)
                {
                    procNode.Nodes.Add(new TreeNode(proc));
                }
                if (procNode.Nodes.Count > 0)
                    bdNode.Nodes.Add(procNode);

                conexionNode.Nodes.Add(bdNode);
            }

            conexionNode.Expand();
        }





        private string AdaptarConsulta(IBaseDatos conexion, string baseDatos, string consulta)
        {
            if (conexion is ConexionSQLServer)
                return $"USE [{baseDatos}];\n{consulta}";
            return consulta;
        }

        private void btnEjecutar_Click(object sender, EventArgs e)
        {
            if (conexionActual == null || comboBoxBD.SelectedItem == null) return;

            string bd = comboBoxBD.SelectedItem.ToString();
            string consulta = txtQuery.Text.Trim();
            if (string.IsNullOrWhiteSpace(consulta)) return;

            // 🔹 Cambiar base de datos antes de ejecutar, si aplica
            if (conexionActual is ConexionMySQL mysql)
            {
                try
                {
                    mysql.CambiarBaseDatos(bd);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"MySQL error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else if (conexionActual is ConexionPostgresSQL postgres)
            {
                try
                {
                    postgres.CambiarBaseDatos(bd);
                    postgres.AbrirConexion(); // 🔑 Reabrir la conexión con la nueva base
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"PostgreSQL error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            // 🔹 Adaptar consulta si es SQL Server
            string consultaFinal = AdaptarConsulta(conexionActual, bd, consulta);

            try
            {
                List<string> resultados = conexionActual.EjecutarConsulta(consultaFinal);

                if (resultados.Count == 0)
                    MessageBox.Show("Consulta ejecutada con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else if (resultados[0].StartsWith("Error:"))
                    MessageBox.Show(resultados[0], "Error en la consulta", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                    MessageBox.Show("Consulta ejecutada con éxito y datos retornados.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al ejecutar la consulta: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BttDesconexion_Click(object sender, EventArgs e)
        {
            if (treeViewBD.SelectedNode == null) return;
            TreeNode nodo = treeViewBD.SelectedNode;
            if (nodo.Parent != null && nodo.Parent.Tag?.ToString() == "conexion") nodo = nodo.Parent;
            if (nodo.Tag?.ToString() != "conexion") return;

            string nombreConexion = nodo.Text;
            if (conexiones.ContainsKey(nombreConexion))
            {
                conexiones[nombreConexion].CerrarConexion();
                conexiones.Remove(nombreConexion);
            }
            treeViewBD.Nodes.Remove(nodo);
            if (nombreConexionActual == nombreConexion)
            {
                conexionActual = null;
                nombreConexionActual = null;
                comboBoxBD.Items.Clear();
                comboBoxBD.Text = "";
            }
        }


        private void btnAgregarConexion_Click_1(object sender, EventArgs e)
        {
            using (var loginForm = new Form1())
            {
                if (loginForm.ShowDialog() == DialogResult.OK)
                {
                    string nombreConexion = loginForm.NombreConexion;

                    if (string.IsNullOrEmpty(nombreConexion))
                    {
                        MessageBox.Show("No se recibió un nombre de conexión válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    IBaseDatos nuevaConexion = loginForm.Conexion;

                    if (nuevaConexion == null)
                    {
                        MessageBox.Show("La conexión no fue creada correctamente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (!conexiones.ContainsKey(nombreConexion))
                    {
                        conexiones[nombreConexion] = nuevaConexion;

                        TreeNode nodoConexion = new TreeNode(nombreConexion)
                        {
                            Tag = "conexion"
                        };
                        treeViewBD.Nodes.Add(nodoConexion);
                    }

                    // ✅ Cambiar a la nueva conexión
                    CambiarConexion(nombreConexion);

                    // ✅ Si hay bases disponibles, seleccionar la primera y cambiarla si aplica
                    if (comboBoxBD.Items.Count > 0)
                    {
                        comboBoxBD.SelectedIndex = 0;
                        string bdSeleccionada = comboBoxBD.SelectedItem.ToString();

                        if (nuevaConexion is ConexionPostgresSQL postgres)
                        {
                            try
                            {
                                postgres.CambiarBaseDatos(bdSeleccionada);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"PostgreSQL: No se pudo cambiar de base: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else if (nuevaConexion is ConexionMySQL mysql)
                        {
                            try
                            {
                                mysql.CambiarBaseDatos(bdSeleccionada);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"MySQL: No se pudo cambiar de base: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }

                        // 🔁 Refrescar el TreeView con la nueva base seleccionada
                        LlenarTreeView();
                    }
                }
            }
        }




        private void BttActualizar_Click(object sender, EventArgs e)
        {
            if (treeViewBD.SelectedNode == null) return;

            TreeNode nodo = treeViewBD.SelectedNode;
            if (nodo.Parent != null && nodo.Parent.Tag?.ToString() == "conexion")
                nodo = nodo.Parent;

            if (nodo.Tag?.ToString() != "conexion") return;

            string nombreConexion = nodo.Text;
            if (!conexiones.ContainsKey(nombreConexion)) return;

            try
            {
                var conexion = conexiones[nombreConexion];
                conexion.CerrarConexion();
                conexion.AbrirConexion();

                if (nombreConexionActual == nombreConexion)
                {
                    conexionActual = conexion;
                    CargarListaBasesDatos();
                }

                nodo.Nodes.Clear();

                // 🔄 Recorremos cada base de datos individual
                foreach (var bd in conexion.ObtenerBasesDeDatos())
                {
                    TreeNode bdNode = new TreeNode($"Base de Datos: {bd}") { Tag = "baseDatos" };
                    TreeNode tablasNode = new TreeNode("Tablas");

                    // 🔄 Obtener solo las tablas de esa base de datos
                    var tablas = conexion.ObtenerTablas(bd);
                    foreach (var tabla in tablas)
                    {
                        TreeNode tablaNode = new TreeNode(tabla);

                        // 🔄 Obtener solo los atributos de esa tabla y base
                        var atributos = conexion.ObtenerAtributos(bd, tabla);
                        foreach (var atr in atributos)
                        {
                            tablaNode.Nodes.Add(new TreeNode($"{atr.Key} ({atr.Value})"));
                        }

                        tablasNode.Nodes.Add(tablaNode);
                    }

                    if (tablasNode.Nodes.Count > 0)
                        bdNode.Nodes.Add(tablasNode);

                    nodo.Nodes.Add(bdNode);
                }

                nodo.Expand();
                MessageBox.Show("Conexión actualizada correctamente.", "Actualizado", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar: {ex.Message}");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var migracionForm = new MigrasionFm(conexiones);
            migracionForm.Show();
        }

        private TreeNode nodoClicDerecho; // almacena el nodo seleccionado con clic derecho

        private void treeViewBD_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                TreeNode nodo = treeViewBD.GetNodeAt(e.X, e.Y);
                if (nodo != null && nodo.Tag?.ToString() == "conexion")
                {
                    treeViewBD.SelectedNode = nodo;
                    nodoClicDerecho = nodo;
                    contextMenuServidor.Show(treeViewBD, e.Location);
                }
            }
        }

        private void monitoreoToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (nodoClicDerecho != null)
            {
                string nombreConexion = nodoClicDerecho.Text;

                if (conexiones.TryGetValue(nombreConexion, out IBaseDatos conexion))
                {
                    if (conexion is ConexionSQLServer sqlServerConexion)
                    {
                        Moniturizacion form = new Moniturizacion(sqlServerConexion);
                        form.Show();
                    }
                }
            }
        }



        private void auditoriaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (nodoClicDerecho != null)
            {
                string nombreConexion = nodoClicDerecho.Text;
                MessageBox.Show($"🔍 Auditoría de la conexión: {nombreConexion}");

                // Obtener la conexión desde el diccionario real de conexiones
                if (conexiones.TryGetValue(nombreConexion, out IBaseDatos conexion))
                {
                    // Detectar el tipo de conexión
                    string tipoConexion = conexion.GetType().Name.ToLower();

                    string tipoGestor = tipoConexion.Contains("sqlserver") ? "sqlserver" :
                                        tipoConexion.Contains("mysql") ? "mysql" :
                                        tipoConexion.Contains("firebird") ? "firebird" :
                                        tipoConexion.Contains("postgres") ? "postgres" :
                                        tipoConexion.Contains("oracle") ? "oracle" : "desconocido";

                    // Abrir el formulario de auditoría y pasar la conexión y tipo de gestor
                    Auditoria formAuditoria = new Auditoria(conexion, tipoGestor);
                    formAuditoria.Show();
                }
                else
                {
                    MessageBox.Show("No se pudo encontrar la conexión correspondiente en el diccionario.");
                }
            }
        }




    }
}
