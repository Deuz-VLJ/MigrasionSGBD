﻿namespace WindowsFormsApp1
{
    partial class MigrasionBasa
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
            this.comboBoxBases = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.checkedListBoxTablas = new System.Windows.Forms.CheckedListBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // comboBoxBases
            // 
            this.comboBoxBases.FormattingEnabled = true;
            this.comboBoxBases.Location = new System.Drawing.Point(12, 22);
            this.comboBoxBases.Name = "comboBoxBases";
            this.comboBoxBases.Size = new System.Drawing.Size(199, 21);
            this.comboBoxBases.TabIndex = 0;
            this.comboBoxBases.SelectedIndexChanged += new System.EventHandler(this.comboBoxBases_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Base de datos";
            // 
            // checkedListBoxTablas
            // 
            this.checkedListBoxTablas.FormattingEnabled = true;
            this.checkedListBoxTablas.Location = new System.Drawing.Point(15, 73);
            this.checkedListBoxTablas.Name = "checkedListBoxTablas";
            this.checkedListBoxTablas.Size = new System.Drawing.Size(338, 184);
            this.checkedListBoxTablas.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(254, 277);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(99, 36);
            this.button1.TabIndex = 3;
            this.button1.Text = "Migrar";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(152, 277);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(81, 36);
            this.button2.TabIndex = 4;
            this.button2.Text = "select all";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // MigrasionBasa
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(387, 450);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.checkedListBoxTablas);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxBases);
            this.Name = "MigrasionBasa";
            this.Text = "MigrasionBasa";
            this.Load += new System.EventHandler(this.MigrasionBasa_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxBases;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckedListBox checkedListBoxTablas;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}