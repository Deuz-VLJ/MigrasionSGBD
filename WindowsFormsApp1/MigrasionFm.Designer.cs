﻿namespace WindowsFormsApp1
{
    partial class MigrasionFm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxSqlServer = new System.Windows.Forms.ComboBox();
            this.comboBoxOtros = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(57, 67);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Migrar desde";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(57, 130);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Destino Migrasion";
            // 
            // comboBoxSqlServer
            // 
            this.comboBoxSqlServer.FormattingEnabled = true;
            this.comboBoxSqlServer.Location = new System.Drawing.Point(165, 64);
            this.comboBoxSqlServer.Name = "comboBoxSqlServer";
            this.comboBoxSqlServer.Size = new System.Drawing.Size(121, 21);
            this.comboBoxSqlServer.TabIndex = 2;
            // 
            // comboBoxOtros
            // 
            this.comboBoxOtros.FormattingEnabled = true;
            this.comboBoxOtros.Location = new System.Drawing.Point(165, 127);
            this.comboBoxOtros.Name = "comboBoxOtros";
            this.comboBoxOtros.Size = new System.Drawing.Size(121, 21);
            this.comboBoxOtros.TabIndex = 3;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(119, 227);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(95, 36);
            this.button1.TabIndex = 4;
            this.button1.Text = "Empezar";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // MigrasionFm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(356, 365);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.comboBoxOtros);
            this.Controls.Add(this.comboBoxSqlServer);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "MigrasionFm";
            this.Text = "MigrasionFm";
            this.Load += new System.EventHandler(this.MigrasionFm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxSqlServer;
        private System.Windows.Forms.ComboBox comboBoxOtros;
        private System.Windows.Forms.Button button1;
    }
}