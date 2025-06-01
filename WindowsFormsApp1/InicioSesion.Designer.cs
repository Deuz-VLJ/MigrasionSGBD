namespace WindowsFormsApp1
{
    public partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtServidor;
        private System.Windows.Forms.TextBox txtRutaBD;
        private System.Windows.Forms.TextBox txtUsuario;
        private System.Windows.Forms.TextBox txtContrasena;
        private System.Windows.Forms.CheckBox chkFirebird;
        private System.Windows.Forms.CheckBox chkSqlServer;
        private System.Windows.Forms.CheckBox chkMySQL;
        private System.Windows.Forms.CheckBox chkPostgreSQL;
        private System.Windows.Forms.CheckBox chkOracle;
        private System.Windows.Forms.Button btnConectar;
        private System.Windows.Forms.Button btnCancelar;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtServidor = new System.Windows.Forms.TextBox();
            this.txtRutaBD = new System.Windows.Forms.TextBox();
            this.txtUsuario = new System.Windows.Forms.TextBox();
            this.txtContrasena = new System.Windows.Forms.TextBox();
            this.chkFirebird = new System.Windows.Forms.CheckBox();
            this.chkSqlServer = new System.Windows.Forms.CheckBox();
            this.chkMySQL = new System.Windows.Forms.CheckBox();
            this.chkPostgreSQL = new System.Windows.Forms.CheckBox();
            this.chkOracle = new System.Windows.Forms.CheckBox();
            this.btnConectar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(20, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "Servidor (IP o Nombre):";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(20, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 23);
            this.label2.TabIndex = 2;
            this.label2.Text = "Ruta BD:";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(20, 100);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 23);
            this.label3.TabIndex = 4;
            this.label3.Text = "Usuario:";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(20, 140);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 23);
            this.label4.TabIndex = 6;
            this.label4.Text = "Contraseña:";
            // 
            // txtServidor
            // 
            this.txtServidor.Location = new System.Drawing.Point(160, 20);
            this.txtServidor.Name = "txtServidor";
            this.txtServidor.Size = new System.Drawing.Size(100, 20);
            this.txtServidor.TabIndex = 1;
            // 
            // txtRutaBD
            // 
            this.txtRutaBD.Location = new System.Drawing.Point(160, 60);
            this.txtRutaBD.Name = "txtRutaBD";
            this.txtRutaBD.Size = new System.Drawing.Size(100, 20);
            this.txtRutaBD.TabIndex = 3;
            // 
            // txtUsuario
            // 
            this.txtUsuario.Location = new System.Drawing.Point(160, 100);
            this.txtUsuario.Name = "txtUsuario";
            this.txtUsuario.Size = new System.Drawing.Size(100, 20);
            this.txtUsuario.TabIndex = 5;
            // 
            // txtContrasena
            // 
            this.txtContrasena.Location = new System.Drawing.Point(160, 140);
            this.txtContrasena.Name = "txtContrasena";
            this.txtContrasena.Size = new System.Drawing.Size(100, 20);
            this.txtContrasena.TabIndex = 7;
            // 
            // chkFirebird
            // 
            this.chkFirebird.Location = new System.Drawing.Point(400, 20);
            this.chkFirebird.Name = "chkFirebird";
            this.chkFirebird.Size = new System.Drawing.Size(104, 24);
            this.chkFirebird.TabIndex = 8;
            this.chkFirebird.Text = "Firebird";
            // 
            // chkSqlServer
            // 
            this.chkSqlServer.Location = new System.Drawing.Point(400, 50);
            this.chkSqlServer.Name = "chkSqlServer";
            this.chkSqlServer.Size = new System.Drawing.Size(104, 24);
            this.chkSqlServer.TabIndex = 9;
            this.chkSqlServer.Text = "SQL Server";
            // 
            // chkMySQL
            // 
            this.chkMySQL.Location = new System.Drawing.Point(400, 80);
            this.chkMySQL.Name = "chkMySQL";
            this.chkMySQL.Size = new System.Drawing.Size(104, 24);
            this.chkMySQL.TabIndex = 10;
            this.chkMySQL.Text = "MySQL";
            // 
            // chkPostgreSQL
            // 
            this.chkPostgreSQL.Location = new System.Drawing.Point(400, 110);
            this.chkPostgreSQL.Name = "chkPostgreSQL";
            this.chkPostgreSQL.Size = new System.Drawing.Size(104, 24);
            this.chkPostgreSQL.TabIndex = 11;
            this.chkPostgreSQL.Text = "PostgreSQL";
            // 
            // chkOracle
            // 
            this.chkOracle.Location = new System.Drawing.Point(400, 140);
            this.chkOracle.Name = "chkOracle";
            this.chkOracle.Size = new System.Drawing.Size(104, 24);
            this.chkOracle.TabIndex = 12;
            this.chkOracle.Text = "Oracle";
            // 
            // btnConectar
            // 
            this.btnConectar.Location = new System.Drawing.Point(100, 180);
            this.btnConectar.Name = "btnConectar";
            this.btnConectar.Size = new System.Drawing.Size(75, 23);
            this.btnConectar.TabIndex = 13;
            this.btnConectar.Text = "Iniciar Conexión";
            this.btnConectar.Click += new System.EventHandler(this.btnConectar_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.Location = new System.Drawing.Point(250, 180);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(75, 23);
            this.btnCancelar.TabIndex = 14;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(500, 250);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtServidor);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtRutaBD);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtUsuario);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtContrasena);
            this.Controls.Add(this.chkFirebird);
            this.Controls.Add(this.chkSqlServer);
            this.Controls.Add(this.chkMySQL);
            this.Controls.Add(this.chkPostgreSQL);
            this.Controls.Add(this.chkOracle);
            this.Controls.Add(this.btnConectar);
            this.Controls.Add(this.btnCancelar);
            this.Name = "Form1";
            this.Text = "Inicio de Sesión";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}

