namespace WindowsFormsApp1
{
    partial class Moniturizacion
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea4 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend4 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea5 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend5 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series5 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea6 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend6 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series6 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.chartCPU = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chartRAM = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chartNetwork = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.labelCPU = new System.Windows.Forms.Label();
            this.labelRAM = new System.Windows.Forms.Label();
            this.labelConexiones = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.chartCPU)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartRAM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartNetwork)).BeginInit();
            this.SuspendLayout();
            // 
            // chartCPU
            // 
            chartArea4.Name = "ChartArea1";
            this.chartCPU.ChartAreas.Add(chartArea4);
            legend4.Name = "Legend1";
            this.chartCPU.Legends.Add(legend4);
            this.chartCPU.Location = new System.Drawing.Point(142, 12);
            this.chartCPU.Name = "chartCPU";
            series4.ChartArea = "ChartArea1";
            series4.Legend = "Legend1";
            series4.Name = "Series1";
            this.chartCPU.Series.Add(series4);
            this.chartCPU.Size = new System.Drawing.Size(422, 189);
            this.chartCPU.TabIndex = 0;
            this.chartCPU.Text = "chart1";
            // 
            // chartRAM
            // 
            chartArea5.Name = "ChartArea1";
            this.chartRAM.ChartAreas.Add(chartArea5);
            legend5.Name = "Legend1";
            this.chartRAM.Legends.Add(legend5);
            this.chartRAM.Location = new System.Drawing.Point(235, 207);
            this.chartRAM.Name = "chartRAM";
            series5.ChartArea = "ChartArea1";
            series5.Legend = "Legend1";
            series5.Name = "Series1";
            this.chartRAM.Series.Add(series5);
            this.chartRAM.Size = new System.Drawing.Size(420, 113);
            this.chartRAM.TabIndex = 1;
            this.chartRAM.Text = "chart2";
            // 
            // chartNetwork
            // 
            chartArea6.Name = "ChartArea1";
            this.chartNetwork.ChartAreas.Add(chartArea6);
            legend6.Name = "Legend1";
            this.chartNetwork.Legends.Add(legend6);
            this.chartNetwork.Location = new System.Drawing.Point(235, 326);
            this.chartNetwork.Name = "chartNetwork";
            series6.ChartArea = "ChartArea1";
            series6.Legend = "Legend1";
            series6.Name = "Series1";
            this.chartNetwork.Series.Add(series6);
            this.chartNetwork.Size = new System.Drawing.Size(420, 113);
            this.chartNetwork.TabIndex = 2;
            this.chartNetwork.Text = "chart3";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(29, 70);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 42);
            this.label1.TabIndex = 3;
            this.label1.Text = "CPU:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(142, 261);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 33);
            this.label2.TabIndex = 4;
            this.label2.Text = "RAM:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(145, 356);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 33);
            this.label3.TabIndex = 5;
            this.label3.Text = "RED:";
            // 
            // labelCPU
            // 
            this.labelCPU.AutoSize = true;
            this.labelCPU.Location = new System.Drawing.Point(61, 113);
            this.labelCPU.Name = "labelCPU";
            this.labelCPU.Size = new System.Drawing.Size(10, 13);
            this.labelCPU.TabIndex = 6;
            this.labelCPU.Text = "-";
            // 
            // labelRAM
            // 
            this.labelRAM.AutoSize = true;
            this.labelRAM.Location = new System.Drawing.Point(158, 294);
            this.labelRAM.Name = "labelRAM";
            this.labelRAM.Size = new System.Drawing.Size(10, 13);
            this.labelRAM.TabIndex = 7;
            this.labelRAM.Text = "-";
            // 
            // labelConexiones
            // 
            this.labelConexiones.AutoSize = true;
            this.labelConexiones.Location = new System.Drawing.Point(158, 389);
            this.labelConexiones.Name = "labelConexiones";
            this.labelConexiones.Size = new System.Drawing.Size(10, 13);
            this.labelConexiones.TabIndex = 8;
            this.labelConexiones.Text = "-";
            // 
            // Moniturizacion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(805, 485);
            this.Controls.Add(this.labelConexiones);
            this.Controls.Add(this.labelRAM);
            this.Controls.Add(this.labelCPU);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chartNetwork);
            this.Controls.Add(this.chartRAM);
            this.Controls.Add(this.chartCPU);
            this.Name = "Moniturizacion";
            this.Text = "Moniturizacion";
            this.Load += new System.EventHandler(this.Moniturizacion_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chartCPU)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartRAM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartNetwork)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chartCPU;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartRAM;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartNetwork;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label labelCPU;
        private System.Windows.Forms.Label labelRAM;
        private System.Windows.Forms.Label labelConexiones;
    }
}