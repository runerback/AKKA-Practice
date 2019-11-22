namespace ChartApp
{
    sealed partial class Main
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.sysChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.diskSwitchButton = new System.Windows.Forms.Button();
            this.memorySwitchButton = new System.Windows.Forms.Button();
            this.cpuSwitchButton = new System.Windows.Forms.Button();
            this.controlButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.sysChart)).BeginInit();
            this.SuspendLayout();
            // 
            // sysChart
            // 
            chartArea1.Name = "ChartArea1";
            this.sysChart.ChartAreas.Add(chartArea1);
            this.sysChart.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            this.sysChart.Legends.Add(legend1);
            this.sysChart.Location = new System.Drawing.Point(0, 0);
            this.sysChart.Margin = new System.Windows.Forms.Padding(4);
            this.sysChart.Name = "sysChart";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.sysChart.Series.Add(series1);
            this.sysChart.Size = new System.Drawing.Size(912, 549);
            this.sysChart.TabIndex = 0;
            this.sysChart.Text = "sysChart";
            // 
            // diskSwitchButton
            // 
            this.diskSwitchButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.diskSwitchButton.Location = new System.Drawing.Point(774, 464);
            this.diskSwitchButton.Name = "diskSwitchButton";
            this.diskSwitchButton.Size = new System.Drawing.Size(107, 45);
            this.diskSwitchButton.TabIndex = 1;
            this.diskSwitchButton.Text = "DISK (OFF)";
            this.diskSwitchButton.UseVisualStyleBackColor = true;
            this.diskSwitchButton.Click += new System.EventHandler(this.diskSwitchButton_Click);
            // 
            // memorySwitchButton
            // 
            this.memorySwitchButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.memorySwitchButton.Location = new System.Drawing.Point(774, 413);
            this.memorySwitchButton.Name = "memorySwitchButton";
            this.memorySwitchButton.Size = new System.Drawing.Size(107, 45);
            this.memorySwitchButton.TabIndex = 2;
            this.memorySwitchButton.Text = "MEMORY (OFF)";
            this.memorySwitchButton.UseVisualStyleBackColor = true;
            this.memorySwitchButton.Click += new System.EventHandler(this.memorySwitchButton_Click);
            // 
            // cpuSwitchButton
            // 
            this.cpuSwitchButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cpuSwitchButton.Location = new System.Drawing.Point(774, 362);
            this.cpuSwitchButton.Name = "cpuSwitchButton";
            this.cpuSwitchButton.Size = new System.Drawing.Size(107, 45);
            this.cpuSwitchButton.TabIndex = 3;
            this.cpuSwitchButton.Text = "CPU (ON)";
            this.cpuSwitchButton.UseVisualStyleBackColor = true;
            this.cpuSwitchButton.Click += new System.EventHandler(this.cpuSwitchButton_Click);
            // 
            // controlButton
            // 
            this.controlButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.controlButton.Location = new System.Drawing.Point(774, 249);
            this.controlButton.Name = "controlButton";
            this.controlButton.Size = new System.Drawing.Size(107, 45);
            this.controlButton.TabIndex = 4;
            this.controlButton.Text = "PAUSE ||";
            this.controlButton.UseVisualStyleBackColor = true;
            this.controlButton.Click += new System.EventHandler(this.controlButton_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(912, 549);
            this.Controls.Add(this.controlButton);
            this.Controls.Add(this.cpuSwitchButton);
            this.Controls.Add(this.memorySwitchButton);
            this.Controls.Add(this.diskSwitchButton);
            this.Controls.Add(this.sysChart);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Main";
            this.Text = "System Metrics";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.Load += new System.EventHandler(this.Main_Load);
            ((System.ComponentModel.ISupportInitialize)(this.sysChart)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart sysChart;
        private System.Windows.Forms.Button diskSwitchButton;
        private System.Windows.Forms.Button memorySwitchButton;
        private System.Windows.Forms.Button cpuSwitchButton;
        private System.Windows.Forms.Button controlButton;
    }
}

