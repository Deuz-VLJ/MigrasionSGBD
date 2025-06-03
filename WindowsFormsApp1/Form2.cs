using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form2 : Form
    {
        public Form2()
        {
           
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.ControlBox = false;
            this.Text = "Procesando...";
            this.Width = 320;
            this.Height = 100;

            Label mensaje = new Label()
            {
                Text = "⏳ Migrando datos, por favor espere...",
                AutoSize = false,
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10),
                TextAlign = ContentAlignment.MiddleCenter
            };

            this.Controls.Add(mensaje);
        }
    }
}
