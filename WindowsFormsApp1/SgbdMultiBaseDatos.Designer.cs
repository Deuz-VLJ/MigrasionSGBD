namespace WindowsFormsApp1
{
    partial class SgbdMultiBaseDatos
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btnAgregarConexion = new System.Windows.Forms.Button();
            this.btnEjecutar = new System.Windows.Forms.Button();
            this.txtQuery = new System.Windows.Forms.TextBox();
            this.treeViewBD = new System.Windows.Forms.TreeView();
            this.comboBoxBD = new System.Windows.Forms.ComboBox();
            this.BttDesconexion = new System.Windows.Forms.Button();
            this.BttActualizar = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.contextMenuServidor = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.monitoreoToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.auditoriaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuServidor.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnAgregarConexion
            // 
            this.btnAgregarConexion.Location = new System.Drawing.Point(16, 5);
            this.btnAgregarConexion.Name = "btnAgregarConexion";
            this.btnAgregarConexion.Size = new System.Drawing.Size(22, 23);
            this.btnAgregarConexion.TabIndex = 9;
            this.btnAgregarConexion.Text = "+";
            this.btnAgregarConexion.UseVisualStyleBackColor = true;
            this.btnAgregarConexion.Click += new System.EventHandler(this.btnAgregarConexion_Click_1);
            // 
            // btnEjecutar
            // 
            this.btnEjecutar.Location = new System.Drawing.Point(217, 409);
            this.btnEjecutar.Name = "btnEjecutar";
            this.btnEjecutar.Size = new System.Drawing.Size(75, 23);
            this.btnEjecutar.TabIndex = 7;
            this.btnEjecutar.Text = "Compilar";
            this.btnEjecutar.UseVisualStyleBackColor = true;
            this.btnEjecutar.Click += new System.EventHandler(this.btnEjecutar_Click);
            // 
            // txtQuery
            // 
            this.txtQuery.Location = new System.Drawing.Point(199, 31);
            this.txtQuery.Multiline = true;
            this.txtQuery.Name = "txtQuery";
            this.txtQuery.Size = new System.Drawing.Size(474, 372);
            this.txtQuery.TabIndex = 6;
            // 
            // treeViewBD
            // 
            this.treeViewBD.Location = new System.Drawing.Point(13, 31);
            this.treeViewBD.Name = "treeViewBD";
            this.treeViewBD.Size = new System.Drawing.Size(158, 402);
            this.treeViewBD.TabIndex = 5;
            this.treeViewBD.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeViewBD_AfterSelect_1);
            // 
            // comboBoxBD
            // 
            this.comboBoxBD.FormattingEnabled = true;
            this.comboBoxBD.Location = new System.Drawing.Point(199, 7);
            this.comboBoxBD.Name = "comboBoxBD";
            this.comboBoxBD.Size = new System.Drawing.Size(121, 21);
            this.comboBoxBD.TabIndex = 10;
            // 
            // BttDesconexion
            // 
            this.BttDesconexion.Location = new System.Drawing.Point(44, 4);
            this.BttDesconexion.Name = "BttDesconexion";
            this.BttDesconexion.Size = new System.Drawing.Size(21, 23);
            this.BttDesconexion.TabIndex = 11;
            this.BttDesconexion.Text = "-";
            this.BttDesconexion.UseVisualStyleBackColor = true;
            this.BttDesconexion.Click += new System.EventHandler(this.BttDesconexion_Click);
            // 
            // BttActualizar
            // 
            this.BttActualizar.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BttActualizar.Location = new System.Drawing.Point(71, 4);
            this.BttActualizar.Name = "BttActualizar";
            this.BttActualizar.Size = new System.Drawing.Size(23, 23);
            this.BttActualizar.TabIndex = 12;
            this.BttActualizar.Text = "↻";
            this.BttActualizar.UseVisualStyleBackColor = true;
            this.BttActualizar.Click += new System.EventHandler(this.BttActualizar_Click);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(100, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(23, 23);
            this.button1.TabIndex = 13;
            this.button1.Text = "M";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // contextMenuServidor
            // 
            this.contextMenuServidor.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.monitoreoToolStripMenuItem1,
            this.auditoriaToolStripMenuItem});
            this.contextMenuServidor.Name = "contextMenuServidor";
            this.contextMenuServidor.Size = new System.Drawing.Size(181, 70);
            // 
            // monitoreoToolStripMenuItem1
            // 
            this.monitoreoToolStripMenuItem1.Name = "monitoreoToolStripMenuItem1";
            this.monitoreoToolStripMenuItem1.Size = new System.Drawing.Size(180, 22);
            this.monitoreoToolStripMenuItem1.Text = "Monitoreo";
            this.monitoreoToolStripMenuItem1.Click += new System.EventHandler(this.monitoreoToolStripMenuItem1_Click);
            // 
            // auditoriaToolStripMenuItem
            // 
            this.auditoriaToolStripMenuItem.Name = "auditoriaToolStripMenuItem";
            this.auditoriaToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.auditoriaToolStripMenuItem.Text = "Auditoria";
            this.auditoriaToolStripMenuItem.Click += new System.EventHandler(this.auditoriaToolStripMenuItem_Click);
            // 
            // SgbdMultiBaseDatos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.BttActualizar);
            this.Controls.Add(this.BttDesconexion);
            this.Controls.Add(this.comboBoxBD);
            this.Controls.Add(this.btnAgregarConexion);
            this.Controls.Add(this.btnEjecutar);
            this.Controls.Add(this.txtQuery);
            this.Controls.Add(this.treeViewBD);
            this.Name = "SgbdMultiBaseDatos";
            this.Text = "SgbdMultiBaseDatos";
            this.Load += new System.EventHandler(this.SgbdMultiBaseDatos_Load);
            this.contextMenuServidor.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnAgregarConexion;
        private System.Windows.Forms.Button btnEjecutar;
        private System.Windows.Forms.TextBox txtQuery;
        private System.Windows.Forms.TreeView treeViewBD;
        private System.Windows.Forms.ComboBox comboBoxBD;
        private System.Windows.Forms.Button BttDesconexion;
        private System.Windows.Forms.Button BttActualizar;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ContextMenuStrip contextMenuServidor;
        private System.Windows.Forms.ToolStripMenuItem monitoreoToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem auditoriaToolStripMenuItem;
    }
}