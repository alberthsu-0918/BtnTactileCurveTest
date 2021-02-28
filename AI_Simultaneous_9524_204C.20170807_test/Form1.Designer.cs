namespace AIO_Simultaneous_UI
{
    partial class frm_main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_main));
            this.gbox_device_operation = new System.Windows.Forms.GroupBox();
            this.tbox_sensor_return_position = new System.Windows.Forms.TextBox();
            this.btn_device_open = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.tbox_sensor_force_limit = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lb_encoderUnitTomm = new System.Windows.Forms.Label();
            this.tbox_encoderUnit_Tomm = new System.Windows.Forms.TextBox();
            this.tbox_sensor_sensitivity = new System.Windows.Forms.TextBox();
            this.lb_sensor_ch0_sensitivity = new System.Windows.Forms.Label();
            this.button_save_data = new System.Windows.Forms.Button();
            this.button_demo = new System.Windows.Forms.Button();
            this.btn_find_return_pt = new System.Windows.Forms.Button();
            this.btn_clear_chart = new System.Windows.Forms.Button();
            this.btn_general_op = new System.Windows.Forms.Button();
            this.zg_graph = new ZedGraph.ZedGraphControl();
            this.gbox_ai_status = new System.Windows.Forms.GroupBox();
            this.textBox_motion_status = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tbox_max_return_point = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbox_return_point = new System.Windows.Forms.TextBox();
            this.lb_ai_ch1_amplitude = new System.Windows.Forms.Label();
            this.tbox_ClickRatio = new System.Windows.Forms.TextBox();
            this.lb_ai_ch0_amplitude = new System.Windows.Forms.Label();
            this.tbox_contact_point = new System.Windows.Forms.TextBox();
            this.lb_ai_ch0_freqency = new System.Windows.Forms.Label();
            this.tbox_peak_point = new System.Windows.Forms.TextBox();
            this.gbox_ai_operation = new System.Windows.Forms.GroupBox();
            this.DemoTimer = new System.Windows.Forms.Timer(this.components);
            this.UiUpdateTimer = new System.Windows.Forms.Timer(this.components);
            this.gbox_device_operation.SuspendLayout();
            this.gbox_ai_status.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.gbox_ai_operation.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbox_device_operation
            // 
            this.gbox_device_operation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbox_device_operation.Controls.Add(this.tbox_sensor_return_position);
            this.gbox_device_operation.Controls.Add(this.btn_device_open);
            this.gbox_device_operation.Controls.Add(this.label6);
            this.gbox_device_operation.Controls.Add(this.tbox_sensor_force_limit);
            this.gbox_device_operation.Controls.Add(this.label1);
            this.gbox_device_operation.Controls.Add(this.lb_encoderUnitTomm);
            this.gbox_device_operation.Controls.Add(this.tbox_encoderUnit_Tomm);
            this.gbox_device_operation.Controls.Add(this.tbox_sensor_sensitivity);
            this.gbox_device_operation.Controls.Add(this.lb_sensor_ch0_sensitivity);
            this.gbox_device_operation.Location = new System.Drawing.Point(16, 16);
            this.gbox_device_operation.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gbox_device_operation.Name = "gbox_device_operation";
            this.gbox_device_operation.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gbox_device_operation.Size = new System.Drawing.Size(1230, 102);
            this.gbox_device_operation.TabIndex = 0;
            this.gbox_device_operation.TabStop = false;
            this.gbox_device_operation.Text = "Device Opeation";
            this.gbox_device_operation.MouseCaptureChanged += new System.EventHandler(this.frm_xxxxx_MouseCaptureChanged);
            // 
            // tbox_sensor_return_position
            // 
            this.tbox_sensor_return_position.Location = new System.Drawing.Point(1111, 33);
            this.tbox_sensor_return_position.Name = "tbox_sensor_return_position";
            this.tbox_sensor_return_position.Size = new System.Drawing.Size(86, 23);
            this.tbox_sensor_return_position.TabIndex = 23;
            this.tbox_sensor_return_position.Validating += new System.ComponentModel.CancelEventHandler(this.tbox_sensor_return_position_Validating);
            // 
            // btn_device_open
            // 
            this.btn_device_open.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_device_open.Location = new System.Drawing.Point(1111, 63);
            this.btn_device_open.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btn_device_open.Name = "btn_device_open";
            this.btn_device_open.Size = new System.Drawing.Size(86, 32);
            this.btn_device_open.TabIndex = 5;
            this.btn_device_open.Text = "Open Device";
            this.btn_device_open.UseVisualStyleBackColor = true;
            this.btn_device_open.Click += new System.EventHandler(this.btn_device_open_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(986, 36);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(119, 15);
            this.label6.TabIndex = 22;
            this.label6.Text = "Return Position(mm)";
            // 
            // tbox_sensor_force_limit
            // 
            this.tbox_sensor_force_limit.Location = new System.Drawing.Point(892, 33);
            this.tbox_sensor_force_limit.Name = "tbox_sensor_force_limit";
            this.tbox_sensor_force_limit.Size = new System.Drawing.Size(86, 23);
            this.tbox_sensor_force_limit.TabIndex = 21;
            this.tbox_sensor_force_limit.Validating += new System.ComponentModel.CancelEventHandler(this.tbox_sensor_force_limit_Validating);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(802, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 15);
            this.label1.TabIndex = 20;
            this.label1.Text = "Force Limit (g)";
            // 
            // lb_encoderUnitTomm
            // 
            this.lb_encoderUnitTomm.AutoSize = true;
            this.lb_encoderUnitTomm.Location = new System.Drawing.Point(574, 36);
            this.lb_encoderUnitTomm.Name = "lb_encoderUnitTomm";
            this.lb_encoderUnitTomm.Size = new System.Drawing.Size(128, 15);
            this.lb_encoderUnitTomm.TabIndex = 17;
            this.lb_encoderUnitTomm.Text = "Encoder0 Unit (mm/r) ";
            // 
            // tbox_encoderUnit_Tomm
            // 
            this.tbox_encoderUnit_Tomm.Location = new System.Drawing.Point(699, 33);
            this.tbox_encoderUnit_Tomm.Name = "tbox_encoderUnit_Tomm";
            this.tbox_encoderUnit_Tomm.Size = new System.Drawing.Size(86, 23);
            this.tbox_encoderUnit_Tomm.TabIndex = 16;
            this.tbox_encoderUnit_Tomm.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbox_encoderUnit_ToMillimeter_KeyDown);
            this.tbox_encoderUnit_Tomm.Validating += new System.ComponentModel.CancelEventHandler(this.tbox_encoderoUnit_ToMillimeter_validating);
            // 
            // tbox_sensor_sensitivity
            // 
            this.tbox_sensor_sensitivity.Location = new System.Drawing.Point(480, 33);
            this.tbox_sensor_sensitivity.Name = "tbox_sensor_sensitivity";
            this.tbox_sensor_sensitivity.Size = new System.Drawing.Size(86, 23);
            this.tbox_sensor_sensitivity.TabIndex = 6;
            this.tbox_sensor_sensitivity.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbox_sensor_ch_sensitivity_KeyDown);
            this.tbox_sensor_sensitivity.Validating += new System.ComponentModel.CancelEventHandler(this.tbox_sensor_ch0_sensitivity_Validating);
            // 
            // lb_sensor_ch0_sensitivity
            // 
            this.lb_sensor_ch0_sensitivity.AutoSize = true;
            this.lb_sensor_ch0_sensitivity.Location = new System.Drawing.Point(347, 36);
            this.lb_sensor_ch0_sensitivity.Name = "lb_sensor_ch0_sensitivity";
            this.lb_sensor_ch0_sensitivity.Size = new System.Drawing.Size(127, 15);
            this.lb_sensor_ch0_sensitivity.TabIndex = 1;
            this.lb_sensor_ch0_sensitivity.Text = "CH0 Sensitivity (g/mV)";
            this.lb_sensor_ch0_sensitivity.MouseCaptureChanged += new System.EventHandler(this.frm_xxxxx_MouseCaptureChanged);
            // 
            // button_save_data
            // 
            this.button_save_data.Location = new System.Drawing.Point(917, 662);
            this.button_save_data.Name = "button_save_data";
            this.button_save_data.Size = new System.Drawing.Size(120, 29);
            this.button_save_data.TabIndex = 25;
            this.button_save_data.Text = "Save Data";
            this.button_save_data.UseVisualStyleBackColor = true;
            this.button_save_data.Click += new System.EventHandler(this.button_save_data_Click);
            // 
            // button_demo
            // 
            this.button_demo.Location = new System.Drawing.Point(1044, 542);
            this.button_demo.Name = "button_demo";
            this.button_demo.Size = new System.Drawing.Size(120, 149);
            this.button_demo.TabIndex = 24;
            this.button_demo.Text = "Demo Mode";
            this.button_demo.UseVisualStyleBackColor = true;
            this.button_demo.Click += new System.EventHandler(this.button_demo_Click);
            // 
            // btn_find_return_pt
            // 
            this.btn_find_return_pt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_find_return_pt.Enabled = false;
            this.btn_find_return_pt.Location = new System.Drawing.Point(917, 582);
            this.btn_find_return_pt.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btn_find_return_pt.Name = "btn_find_return_pt";
            this.btn_find_return_pt.Size = new System.Drawing.Size(120, 29);
            this.btn_find_return_pt.TabIndex = 18;
            this.btn_find_return_pt.Text = "Find Return Point";
            this.btn_find_return_pt.UseVisualStyleBackColor = true;
            this.btn_find_return_pt.Click += new System.EventHandler(this.btn_find_return_pt_Click);
            // 
            // button1
            // 
            this.btn_clear_chart.Location = new System.Drawing.Point(917, 622);
            this.btn_clear_chart.Name = "btn_clear_chart";
            this.btn_clear_chart.Size = new System.Drawing.Size(120, 29);
            this.btn_clear_chart.TabIndex = 15;
            this.btn_clear_chart.Text = "Clear Chart";
            this.btn_clear_chart.UseVisualStyleBackColor = true;
            this.btn_clear_chart.Click += new System.EventHandler(this.btn_clear_chart_Click);
            // 
            // btn_general_op
            // 
            this.btn_general_op.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_general_op.Enabled = false;
            this.btn_general_op.Location = new System.Drawing.Point(917, 542);
            this.btn_general_op.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btn_general_op.Name = "btn_general_op";
            this.btn_general_op.Size = new System.Drawing.Size(120, 29);
            this.btn_general_op.TabIndex = 6;
            this.btn_general_op.Text = "General Operation";
            this.btn_general_op.UseVisualStyleBackColor = true;
            this.btn_general_op.Click += new System.EventHandler(this.btn_general_operation_Click);
            // 
            // zg_graph
            // 
            this.zg_graph.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.zg_graph.Enabled = false;
            this.zg_graph.IsEnableWheelZoom = false;
            this.zg_graph.IsPrintFillPage = false;
            this.zg_graph.IsPrintKeepAspectRatio = false;
            this.zg_graph.IsPrintScaleAll = false;
            this.zg_graph.IsShowCopyMessage = false;
            this.zg_graph.Location = new System.Drawing.Point(23, 39);
            this.zg_graph.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.zg_graph.Name = "zg_graph";
            this.zg_graph.ScrollGrace = 0D;
            this.zg_graph.ScrollMaxX = 0D;
            this.zg_graph.ScrollMaxY = 0D;
            this.zg_graph.ScrollMaxY2 = 0D;
            this.zg_graph.ScrollMinX = 0D;
            this.zg_graph.ScrollMinY = 0D;
            this.zg_graph.ScrollMinY2 = 0D;
            this.zg_graph.Size = new System.Drawing.Size(549, 708);
            this.zg_graph.TabIndex = 2;
            // 
            // gbox_ai_status
            // 
            this.gbox_ai_status.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.gbox_ai_status.Controls.Add(this.textBox_motion_status);
            this.gbox_ai_status.Controls.Add(this.label8);
            this.gbox_ai_status.Controls.Add(this.button_save_data);
            this.gbox_ai_status.Controls.Add(this.pictureBox1);
            this.gbox_ai_status.Controls.Add(this.button_demo);
            this.gbox_ai_status.Controls.Add(this.label7);
            this.gbox_ai_status.Controls.Add(this.tbox_max_return_point);
            this.gbox_ai_status.Controls.Add(this.zg_graph);
            this.gbox_ai_status.Controls.Add(this.btn_find_return_pt);
            this.gbox_ai_status.Controls.Add(this.btn_clear_chart);
            this.gbox_ai_status.Controls.Add(this.label2);
            this.gbox_ai_status.Controls.Add(this.tbox_return_point);
            this.gbox_ai_status.Controls.Add(this.lb_ai_ch1_amplitude);
            this.gbox_ai_status.Controls.Add(this.btn_general_op);
            this.gbox_ai_status.Controls.Add(this.tbox_ClickRatio);
            this.gbox_ai_status.Controls.Add(this.lb_ai_ch0_amplitude);
            this.gbox_ai_status.Controls.Add(this.tbox_contact_point);
            this.gbox_ai_status.Controls.Add(this.lb_ai_ch0_freqency);
            this.gbox_ai_status.Controls.Add(this.tbox_peak_point);
            this.gbox_ai_status.Location = new System.Drawing.Point(14, 22);
            this.gbox_ai_status.Name = "gbox_ai_status";
            this.gbox_ai_status.Size = new System.Drawing.Size(1200, 769);
            this.gbox_ai_status.TabIndex = 2;
            this.gbox_ai_status.TabStop = false;
            this.gbox_ai_status.Text = "AI status";
            this.gbox_ai_status.MouseCaptureChanged += new System.EventHandler(this.frm_xxxxx_MouseCaptureChanged);
            // 
            // textBox_motion_status
            // 
            this.textBox_motion_status.Location = new System.Drawing.Point(732, 711);
            this.textBox_motion_status.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBox_motion_status.Name = "textBox_motion_status";
            this.textBox_motion_status.ReadOnly = true;
            this.textBox_motion_status.Size = new System.Drawing.Size(88, 23);
            this.textBox_motion_status.TabIndex = 44;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(623, 714);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(83, 15);
            this.label8.TabIndex = 43;
            this.label8.Text = "Motion Status";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.InitialImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.InitialImage")));
            this.pictureBox1.Location = new System.Drawing.Point(609, 50);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(548, 402);
            this.pictureBox1.TabIndex = 42;
            this.pictureBox1.TabStop = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(623, 645);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(102, 15);
            this.label7.TabIndex = 40;
            this.label7.Text = "Max Return Point";
            // 
            // tbox_max_return_point
            // 
            this.tbox_max_return_point.Location = new System.Drawing.Point(732, 645);
            this.tbox_max_return_point.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbox_max_return_point.Name = "tbox_max_return_point";
            this.tbox_max_return_point.ReadOnly = true;
            this.tbox_max_return_point.Size = new System.Drawing.Size(88, 23);
            this.tbox_max_return_point.TabIndex = 41;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(623, 612);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 15);
            this.label2.TabIndex = 32;
            this.label2.Text = "Return Point";
            // 
            // tbox_return_point
            // 
            this.tbox_return_point.Location = new System.Drawing.Point(732, 612);
            this.tbox_return_point.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbox_return_point.Name = "tbox_return_point";
            this.tbox_return_point.ReadOnly = true;
            this.tbox_return_point.Size = new System.Drawing.Size(88, 23);
            this.tbox_return_point.TabIndex = 33;
            // 
            // lb_ai_ch1_amplitude
            // 
            this.lb_ai_ch1_amplitude.AutoSize = true;
            this.lb_ai_ch1_amplitude.Location = new System.Drawing.Point(623, 678);
            this.lb_ai_ch1_amplitude.Name = "lb_ai_ch1_amplitude";
            this.lb_ai_ch1_amplitude.Size = new System.Drawing.Size(65, 15);
            this.lb_ai_ch1_amplitude.TabIndex = 27;
            this.lb_ai_ch1_amplitude.Text = "Click Ratio";
            // 
            // tbox_ClickRatio
            // 
            this.tbox_ClickRatio.Location = new System.Drawing.Point(732, 678);
            this.tbox_ClickRatio.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbox_ClickRatio.Name = "tbox_ClickRatio";
            this.tbox_ClickRatio.ReadOnly = true;
            this.tbox_ClickRatio.Size = new System.Drawing.Size(88, 23);
            this.tbox_ClickRatio.TabIndex = 31;
            // 
            // lb_ai_ch0_amplitude
            // 
            this.lb_ai_ch0_amplitude.AutoSize = true;
            this.lb_ai_ch0_amplitude.Location = new System.Drawing.Point(623, 579);
            this.lb_ai_ch0_amplitude.Name = "lb_ai_ch0_amplitude";
            this.lb_ai_ch0_amplitude.Size = new System.Drawing.Size(81, 15);
            this.lb_ai_ch0_amplitude.TabIndex = 26;
            this.lb_ai_ch0_amplitude.Text = "Contact Point";
            // 
            // tbox_contact_point
            // 
            this.tbox_contact_point.Location = new System.Drawing.Point(732, 579);
            this.tbox_contact_point.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbox_contact_point.Name = "tbox_contact_point";
            this.tbox_contact_point.ReadOnly = true;
            this.tbox_contact_point.Size = new System.Drawing.Size(88, 23);
            this.tbox_contact_point.TabIndex = 30;
            // 
            // lb_ai_ch0_freqency
            // 
            this.lb_ai_ch0_freqency.AutoSize = true;
            this.lb_ai_ch0_freqency.Location = new System.Drawing.Point(623, 546);
            this.lb_ai_ch0_freqency.Name = "lb_ai_ch0_freqency";
            this.lb_ai_ch0_freqency.Size = new System.Drawing.Size(65, 15);
            this.lb_ai_ch0_freqency.TabIndex = 1;
            this.lb_ai_ch0_freqency.Text = "Peak Point";
            this.lb_ai_ch0_freqency.MouseCaptureChanged += new System.EventHandler(this.frm_xxxxx_MouseCaptureChanged);
            // 
            // tbox_peak_point
            // 
            this.tbox_peak_point.Location = new System.Drawing.Point(732, 546);
            this.tbox_peak_point.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbox_peak_point.Name = "tbox_peak_point";
            this.tbox_peak_point.ReadOnly = true;
            this.tbox_peak_point.Size = new System.Drawing.Size(88, 23);
            this.tbox_peak_point.TabIndex = 10;
            // 
            // gbox_ai_operation
            // 
            this.gbox_ai_operation.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.gbox_ai_operation.Controls.Add(this.gbox_ai_status);
            this.gbox_ai_operation.Enabled = false;
            this.gbox_ai_operation.Location = new System.Drawing.Point(16, 113);
            this.gbox_ai_operation.Name = "gbox_ai_operation";
            this.gbox_ai_operation.Size = new System.Drawing.Size(1230, 832);
            this.gbox_ai_operation.TabIndex = 1;
            this.gbox_ai_operation.TabStop = false;
            this.gbox_ai_operation.Text = "AI Operation";
            this.gbox_ai_operation.MouseCaptureChanged += new System.EventHandler(this.frm_xxxxx_MouseCaptureChanged);
            // 
            // DemoTimer
            // 
            this.DemoTimer.Tick += new System.EventHandler(this.demoTimer_Tick);
            // 
            // frm_main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1272, 1004);
            this.Controls.Add(this.gbox_ai_operation);
            this.Controls.Add(this.gbox_device_operation);
            this.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "frm_main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "9524 for Tactile Curve Test";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frm_main_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frm_main_FormClosed);
            this.Load += new System.EventHandler(this.frm_main_Load);
            this.MouseCaptureChanged += new System.EventHandler(this.frm_xxxxx_MouseCaptureChanged);
            this.gbox_device_operation.ResumeLayout(false);
            this.gbox_device_operation.PerformLayout();
            this.gbox_ai_status.ResumeLayout(false);
            this.gbox_ai_status.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.gbox_ai_operation.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbox_device_operation;
        private System.Windows.Forms.Button btn_device_open;
        private System.Windows.Forms.Button btn_general_op;
        private ZedGraph.ZedGraphControl zg_graph;
        private System.Windows.Forms.GroupBox gbox_ai_status;
        private System.Windows.Forms.Label lb_ai_ch1_amplitude;
        private System.Windows.Forms.TextBox tbox_sensor_sensitivity;
        private System.Windows.Forms.Label lb_sensor_ch0_sensitivity;
        private System.Windows.Forms.TextBox tbox_ClickRatio;
        private System.Windows.Forms.Label lb_ai_ch0_amplitude;
        private System.Windows.Forms.TextBox tbox_contact_point;
        private System.Windows.Forms.Label lb_ai_ch0_freqency;
        private System.Windows.Forms.TextBox tbox_peak_point;
        private System.Windows.Forms.GroupBox gbox_ai_operation;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbox_return_point;
        private System.Windows.Forms.Button btn_clear_chart;
        private System.Windows.Forms.TextBox tbox_encoderUnit_Tomm;
        private System.Windows.Forms.Label lb_encoderUnitTomm;
        private System.Windows.Forms.Button btn_find_return_pt;
        private System.Windows.Forms.TextBox tbox_sensor_force_limit;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbox_sensor_return_position;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button_demo;
        private System.Windows.Forms.Timer DemoTimer;
        private System.Windows.Forms.Timer UiUpdateTimer;
        private System.Windows.Forms.Button button_save_data;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tbox_max_return_point;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox textBox_motion_status;
        private System.Windows.Forms.Label label8;
        
    }
}

