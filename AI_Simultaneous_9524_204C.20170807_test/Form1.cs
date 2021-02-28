using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using System.Threading;
using System.Diagnostics;
using ZedGraph;





namespace AIO_Simultaneous_UI
{


    /// <summary>
    /// Class frm_main.
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Form" />
    public partial class frm_main : Form
    {
        /// <summary>
        /// The configuration parameters for monitoring and controlling
        /// </summary>
        static program_config config_para;
        /// <summary>
        /// The motion card
        /// </summary>
        static MotionCard motionCard;
        /// <summary>
        /// The DAQ card
        /// </summary>
        static P9524 daqCard;
        /// <summary>
        /// The list handler for stored data during the measurement
        /// </summary>
        static StoredListHandler listHandler;

        /// <summary>
        /// Delegate InterruptForVelocityMove for motion card's velocity move functon
        /// </summary>
        /// <param name="dir">The direction.</param>
        delegate void InterruptForVelocityMove(int dir);
        /// <summary>
        /// Delegate InterruptForPtpMove for motion card's point to point move functon
        /// </summary>
        /// <param name="dir">The dirirection.</param>
        /// <param name="distance">The distance.</param>
        delegate void InterruptForPtpMove(int dir, int distance);
                     
        /// <summary>
        /// Initialization zed graph for polting data
        /// </summary>
        public void Initial_Zedgraph()
        {
            // Raw data graph
            GraphPane tmp_pane = this.zg_graph.GraphPane;
            tmp_pane.Title.Text = "Curve of Tactile Click";
            tmp_pane.XAxis.Title.Text = "Travel (encoder)";
            tmp_pane.YAxis.Title.Text = "Force (gf)";
            this.zg_graph.AxisChange();
            this.zg_graph.Refresh();

        }


        /// <summary>
        /// Reflashes the sensor related texts.
        /// </summary>
        public void ReflashSensorRelatedTexts()
        {
            //
            // Sensor settings
            //
            this.tbox_sensor_sensitivity.Text = config_para.sensor_sensitivity.ToString();
            this.tbox_encoderUnit_Tomm.Text = config_para.encoder_unit_to_mm.ToString();
            this.tbox_sensor_force_limit.Text = config_para.force_limit.ToString();
            this.tbox_sensor_return_position.Text = config_para.return_position.ToString();
        }

        // Resource release handler
        /// <summary>
        /// Releases the cards.
        /// </summary>
        public void ReleaseCards()
        {
            int err;
            err = daqCard.ReleaseCard();
            err = motionCard.Close();
        }



        /// <summary>
        /// Plots the data.
        /// </summary>
        unsafe public void PlotData()
        {
            if (InvokeRequired && IsHandleCreated)
            {
                try
                {
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            else
            {
                //
                // Update AI UI control HERE
                //
                if (IsHandleCreated)
                {
                    // Show last data of update buffer
                    // Raw data graph
                    GraphPane[] tmp_ai_wave_raw_pane = new GraphPane[1] { this.zg_graph.GraphPane };
                    double[] ai_update_buffer_position = new double[daqCard.GetSampleCountsPerChannel() / 2];
                    double[] ai_update_buffer_force = new double[daqCard.GetSampleCountsPerChannel() / 2];
                    

                    // Convert Raw Data to Voltage
                    IntPtr ptr;
                    fixed (int* p = listHandler.ai_dataList[(int)listHandler.updatedCount])
                    {
                        ptr = (IntPtr)p;
                    }
                    daqCard.ConvertRawData(ptr, config_para.load_cell_offset, config_para.encoder_unit_to_mm, config_para.sensor_sensitivity, ai_update_buffer_position, ai_update_buffer_force);

                    // Add curve to UI objects (ZedGraph)
                    if (tmp_ai_wave_raw_pane[0].CurveList.Count > 0 && config_para.previous_curve_index == config_para.curve_index)
                    {
                        LineItem tmp_ai_wave_raw_curveTest = tmp_ai_wave_raw_pane[0].CurveList[tmp_ai_wave_raw_pane[0].CurveList.Count-1] as LineItem;
                        IPointListEdit pList = tmp_ai_wave_raw_curveTest.Points as IPointListEdit;
                        for (ushort vj = 0; vj < daqCard.GetSampleCountsPerChannel() / 2; ++vj)
                            pList.Add(ai_update_buffer_position[vj], ai_update_buffer_force[vj]);
                    }
                    else
                    {
                        LineItem tmp_ai_wave_raw_curve = tmp_ai_wave_raw_pane[0].AddCurve(null, ai_update_buffer_position, ai_update_buffer_force, config_para.curve_color[config_para.curve_index % 10], SymbolType.None);
                        tmp_ai_wave_raw_curve.Line.IsSmooth = false;
                        config_para.previous_curve_index = config_para.curve_index;
                    }
                    

                    int motionStatus = motionCard.GetStatus();
                    //Graph update
                    this.zg_graph.AxisChange();
                    this.zg_graph.Refresh();
                    this.textBox_motion_status.Text = motionStatus.ToString();
                    //this.textBox_motion_status.Text = daqCard.Polling().ToString();
                    //add ai_buf_update_cnt to mark data ia updated to UI
                    listHandler.updated();
                    
                }
            }
        }


        /// <summary>
        /// Gets the force limit in millivolt.
        /// </summary>
        /// <returns>System.Double.</returns>
        static public double GetForceLimitInMillivolt()
        {
            if (config_para.force_limit > 0 && config_para.sensor_sensitivity >0)
                return config_para.force_limit / 1000.0 / config_para.sensor_sensitivity + config_para.load_cell_offset;
            else
                return 0.0;
        }

        /// <summary>
        /// Records the return posistion.
        /// </summary>
        /// <param name="encoderValue">The encoder value.</param>
        static public void RecordReturnPosistion(double encoderValue)
        {
            if (encoderValue > 0 && config_para.encoder_unit_to_mm > 0)
                config_para.return_position = config_para.encoder_unit_to_mm * encoderValue;
            else
                config_para.return_position = 0.0;
        }

        /// <summary>
        /// Gets the return position.
        /// </summary>
        /// <returns>Int32.</returns>
        public Int32 GetReturnPosition()
        {
            if (config_para.return_position > 0 && config_para.encoder_unit_to_mm > 0)
            {
                double encoderValue = config_para.return_position / config_para.encoder_unit_to_mm;
                return Convert.ToInt32(encoderValue);
            }
            else
                return 0;
        }

        /// <summary>
        /// Callback function for AI buffer ready event during find return point operation
        /// </summary>
        static CallbackDelegate ai_buf_ready_callback_for_find_rtn_pt = new CallbackDelegate(ai_buf_ready_func_for_find_rtn_pt);
        /// <summary>
        /// Function for AI buffer ready event during find return point operation.
        /// </summary>
        unsafe static void ai_buf_ready_func_for_find_rtn_pt()
        {
            // Transfer data from kernel to user
            Int32[] Data_array = new Int32[daqCard.GetSampleCountsPerChannel()];
            double ReturnningStopPoint = 0.1;//set stop point to 0.1mm when returning

            try
            {
                daqCard.TransferBuffer(Data_array);
                if (motionCard.currentDirection != MotionDirection.Stop)
                {
                    // Copy data to update buffer(List)
                    listHandler.ai_dataList.Add(Data_array);

                    // Pressing: Check AI raw data. See if load cell's pressure is over limit
                    if (motionCard.currentDirection == MotionDirection.Pressing && daqCard.GetLatestVoltageValue() > GetForceLimitInMillivolt())
                    {
                        // Stop Servo and Return
                        motionCard.Stop(); // the stop function will trigger "Move Done" Event
                        RecordReturnPosistion(daqCard.GetlatestEncoderValue());
                    }

                    // Returning: Check Encoder raw data. See if it reutrns to ReturnningStopPoint 
                    if (motionCard.currentDirection == MotionDirection.Returning && daqCard.GetlatestEncoderValue() * config_para.encoder_unit_to_mm < ReturnningStopPoint)
                    {
                        // Done. Stop Servo.
                        motionCard.Stop();
                        Returned_event.Set(); // Signal Returned Event to call AsyncClear, which will call back ai_done_cbfunc.
                    }
                }


            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        ///Callback function for AI buffer ready event for general operation
        /// </summary>
        static CallbackDelegate ai_buf_ready_callback_for_general_op = new CallbackDelegate(ai_buf_ready_func_for_general_op);
        /// <summary>
        /// Fnction for AI buffer ready event for general operation.
        /// </summary>
        unsafe static void ai_buf_ready_func_for_general_op()
        {
            // Process AI data of ready buffer HERE
            // Transfer data from kernel to user
            Int32[] Data_array = new Int32[daqCard.GetSampleCountsPerChannel()];
            try
            {
                daqCard.TransferBuffer(Data_array);
                if (motionCard.currentDirection != MotionDirection.Stop)
                {

                    // Copy data to update buffer(List)
                    listHandler.ai_dataList.Add(Data_array);
                }
                else
                {
                    Console.WriteLine("In ai_buf_ready_func_for_general_op, return event is going to be set");
                    Returned_event.Set(); // Signal Returned Event to call AsyncClear, which will call back ai_done_cbfunc.
                }
            
                
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e.Message);
            }
        }
       
        /// <summary>
        /// The returned event.The Event is signaled by Servo's returning to start point.
        /// </summary>
        static AutoResetEvent Returned_event = new AutoResetEvent(false);
        /// <summary>
        /// Checks servo returned during find return point operation.
        /// </summary>
        public void CheckServoReturnedForFindRtnPtOperation()
        {
            Returned_event.WaitOne();
            this.btn_find_return_pt.Invoke((MethodInvoker)delegate { this.btn_find_return_pt.PerformClick(); });
            this.tbox_sensor_return_position.Invoke((MethodInvoker)delegate { this.tbox_sensor_return_position.Text = config_para.return_position.ToString(); });
        }

        /// <summary>
        /// Checks servo returned during general operation.
        /// </summary>
        public void CheckServoReturnedForGeneralOperation()
        {
            Returned_event.WaitOne();
            this.btn_general_op.Invoke((MethodInvoker)delegate { this.btn_general_op.PerformClick(); });

        }

        /// <summary>
        /// Terminates the operation.
        /// </summary>
        public void TerminateOperation()
        {
            daqCard.StopAcq();
            // Wait for that ai_done_cbfunc() is complete
            Console.WriteLine("AI_Done event is set");
            // motionCard to Start Point
            motionCard.ToStartPositoin();

            do
            {
                System.Threading.Thread.Sleep(1);

            } while (listHandler.updatedCount != listHandler.ai_dataList.Count);
            UiUpdateTimer.Stop();
            
            //FindPeak(1.95, 2.05, 1.90, 2.00);
            config_para.is_operation_started = false;
            this.btn_find_return_pt.Invoke((MethodInvoker)delegate { this.btn_find_return_pt.Text = "Find Return Point"; this.btn_find_return_pt.Enabled = true; });
            this.btn_general_op.Invoke((MethodInvoker)delegate { this.btn_general_op.Text = "Start Operation"; this.btn_general_op.Enabled = true; });
            this.btn_device_open.Invoke((MethodInvoker)delegate { this.btn_device_open.Enabled = true; });
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="frm_main"/> class.
        /// </summary>
        public frm_main()
        {
            InitializeComponent();

            //
            // Motion Card Parameters
            //
            motionCard = new Amp204C();
            //
            // DAQ(PCI-9524) Parameters
            //
            daqCard = new P9524();
            //
            // Monitoing Parameters
            //
            config_para = new program_config();
            //
            // list handler to store AI data 
            //
            listHandler = new StoredListHandler(); 
            // Initialize global variables
           

            
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="frm_main"/> class.
        /// </summary>
        ~frm_main()
        {
            Console.WriteLine("Destructor of frm_main is called");
        }

        /// <summary>
        /// Handles the Load event of the frm_main control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void frm_main_Load(object sender, EventArgs e)
        {
            ReflashSensorRelatedTexts();
            // Initial graph
            Initial_Zedgraph();
        }

        /// <summary>
        /// Handles the FormClosing event of the frm_main control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FormClosingEventArgs"/> instance containing the event data.</param>
        private void frm_main_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("Exit " + this.Text + "?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// Handles the FormClosed event of the frm_main control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FormClosedEventArgs"/> instance containing the event data.</param>
        private void frm_main_FormClosed(object sender, FormClosedEventArgs e)
        {
            //mcard.Close();
            this.Cursor = Cursors.WaitCursor;

            // Push Stop button
            if (config_para.is_operation_started)
            {
                btn_general_operation_Click(this.btn_general_op, null);
            }

            // Free all related reources and close the open device (if necessary)
            ReleaseCards();

            this.Cursor = Cursors.Default;
        }

        /// <summary>
        /// Handles the MouseCaptureChanged event of the frm_xxxxx control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void frm_xxxxx_MouseCaptureChanged(object sender, EventArgs e)
        {
            if (this.ActiveControl is TextBox)
            {
                this.ActiveControl = this.ActiveControl.Parent;
            }
        }


        /// <summary>
        /// Handles the Click event of the btn_device_open control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btn_device_open_Click(object sender, EventArgs e)
        {
            if (sender != null)
            {
                int err = -1;
                Button tmp_btn_device_open = (Button)sender;
                this.Cursor = Cursors.WaitCursor;
                if (tmp_btn_device_open.Text == "Open Device")
                {
                    // Register a specified device, it sets and initializes all related variables and necessary resources.
                    // This function must be called before calling any other functions to control the device.
                    // Remember to call Release_Card() to release all allocated resources.
                    short result = daqCard.RegisterCard(0);
                    if (result < 0)
                    {
                        this.Cursor = Cursors.Default;
                        MessageBox.Show("Falied to perform DASK.Register_Card(), error: " + result, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                   
                    this.gbox_ai_operation.Enabled = true;
                    this.zg_graph.Enabled = true;
                    this.btn_general_op.Enabled = true;
                    this.btn_find_return_pt.Enabled = true;

                    tmp_btn_device_open.Text = "Close Device";


                    // Intial Motion Card. Register motion Card
                    // Setup single axis, connect to servo. 
                    // Turn on Servo.
                    err = motionCard.InitialAxis(0);
                    if (err != 0)
                    {
                        MessageBox.Show("Fail to intial Motion Card");
                    }
                    
                    motionCard.SetServoOn(1);
                    if (err != 0)
                    {
                        MessageBox.Show("Fail to Turn on Servo");
                    }

                    motionCard.ToHome();
                    if (err != 0)
                    {
                        MessageBox.Show("Fail to Turn on Servo");
                    }

                    System.Threading.Thread.Sleep(1000);
                    motionCard.ToStartPositoin();
                    if (err != 0)
                    {
                        MessageBox.Show("Fail to Turn on Servo");
                    }
                }
                else
                {
                    ReleaseCards();
                    this.gbox_ai_operation.Enabled = false;
                    this.zg_graph.Enabled = false;
                    this.btn_general_op.Enabled = false;
                    this.btn_find_return_pt.Enabled = false;
                    tmp_btn_device_open.Text = "Open Device";
                }
                this.Cursor = Cursors.Default;
            }
        }


        /// <summary>
        /// Handles the Click event of the btn_general_operation control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btn_general_operation_Click(object sender, EventArgs e)
        {
            if (sender != null)
            {
                Button tmp_btn_device_start = (Button)sender;
                this.Cursor = Cursors.WaitCursor;

                if (config_para.is_operation_started == false)
                {
                    //
                    // Start operation - configure all necessary settings
                    //
                    config_para.curve_index++;
                    if (config_para.curve_index%10 == 0)
                        this.btn_clear_chart.PerformClick();

                                      
                                       
                    //For load cell's offset calibration
                    config_para.load_cell_offset = daqCard.GetLoadCellOffset();


                    // Reset status variables
                    UiUpdateTimer.Tick += new EventHandler(UiUpdateTimerTick);
                    UiUpdateTimer.Enabled = true;
                    UiUpdateTimer.Start();

                    //reset list for storing data
                    listHandler.resetStoredList();

                    // GPTC Setup
                    daqCard.Config(ai_buf_ready_callback_for_general_op, null);

                    
                    //  motion card's config
                    //Setup Interrupt 0's parameters. After reach pressure limit, we want it returning (using interrupt)
                    int factor = 12; //Motion Done Interrupt 12. Command stop 8.
                    Int32 distance = GetReturnPosition();
                    InterruptForPtpMove interrFun = motionCard.PtpMove;
                    object[] arg = {0, distance }; //Function parameter for interrFun(mcard.PtpMove)

                    motionCard.SetupInterrupt(factor, interrFun, arg, ref motionCard.interruptsetup[1]);
                    Thread thread1 = new Thread(new ParameterizedThreadStart(motionCard.CreateInterrupt));
                    thread1.Start(motionCard.interruptsetup[1]);
                    Returned_event.Reset();

                    // Setup threadforCheckervoReturned. This thread is checking if servo returning to start point (by checking encoder)
                    // threadforCheckervoReturned wait for an event.
                    // This event is set when Motion Card's end of returning, determined by 9524 encoder's position. 
                    // After event is set, AsyncClear is going to be called to end 9524's double bf acquisition and trigger DASK.AIEnd.
                    // When DASK.AIEnd is signaled, function ai_done_cbdel will be called back. 

                    Thread threadforCheckervoReturned = new Thread(CheckServoReturnedForGeneralOperation);
                    threadforCheckervoReturned.Start();

                    //Start pressing movement
                    motionCard.PtpMove(1, distance);

                    daqCard.StartAcq();

                    config_para.is_operation_started = true;
                    this.btn_device_open.Enabled = false;
                    this.btn_find_return_pt.Enabled = false;
                    tmp_btn_device_start.Text = "Stop Operation";

                }
                else
                {
                    Thread threadforTerminateOperation = new Thread(TerminateOperation);
                    threadforTerminateOperation.Start();
                }
                this.Cursor = Cursors.Default;
            }
            
        }


        /// <summary>
        /// Handles the Click event of the btn_find_return_pt control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btn_find_return_pt_Click(object sender, EventArgs e)
        {

            if (sender != null)
            {
                Button tmp_btn_find_rtnPt = (Button)sender;
                this.Cursor = Cursors.WaitCursor;

                if (config_para.is_operation_started == false)
                {
                    //
                    // Start operation - configure all necessary settings
                    //
                    config_para.curve_index++;
                    if (config_para.curve_index % 10 == 0)
                        this.btn_clear_chart.PerformClick();




                    //For load cell's offset calibration
                    config_para.load_cell_offset = daqCard.GetLoadCellOffset();

                    UiUpdateTimer.Tick += new EventHandler(UiUpdateTimerTick);
                    UiUpdateTimer.Enabled = true;
                    UiUpdateTimer.Start();

                    //reset list for storing data
                    listHandler.resetStoredList();

                    daqCard.Config(ai_buf_ready_callback_for_find_rtn_pt, null);

                    //  motion card's config
                    //Setup Interrupt 0's parameters. After reach pressure limit, we want it returning (using interrupt)
                    int factor = 8; //Motion Done Interrupt 12. Command stop 8.
                    InterruptForVelocityMove interrFun = motionCard.VelocityMove;
                    object[] arg = { 0 }; //Function parameter for interrFun (mcard.VelocityMove)

                    motionCard.SetupInterrupt(factor, interrFun, arg, ref motionCard.interruptsetup[0]);
                    Thread thread1 = new Thread(new ParameterizedThreadStart(motionCard.CreateInterrupt));
                    thread1.Start(motionCard.interruptsetup[0]);
                    Returned_event.Reset();

                    // Setup threadforCheckervoReturned. This thread is checking if servo returning to start point (by checking encoder)
                    // threadforCheckervoReturned wait for an event.
                    // This event is set when Motion Card's end of returning, determined by 9524 encoder's position. 
                    // After event is set, AsyncClear is going to be called to end 9524's double bf acquisition and trigger DASK.AIEnd.
                    // When DASK.AIEnd is signaled, function ai_done_cbdel will be called back. 

                    Thread threadforCheckervoReturned = new Thread(CheckServoReturnedForFindRtnPtOperation);
                    threadforCheckervoReturned.Start();

                    //Start pressing movement
                    motionCard.VelocityMove(1);

                    daqCard.StartAcq();

                    config_para.is_operation_started = true;
                    this.btn_device_open.Enabled = false;
                    this.btn_general_op.Enabled = false;
                    tmp_btn_find_rtnPt.Text = "Stop Operation";
                }
                else
                {
                    Thread threadforTerminateOperation = new Thread(TerminateOperation);
                    threadforTerminateOperation.Start();
                }
                this.Cursor = Cursors.Default;
            }
        }



        /// <summary>
        /// Handles the KeyDown event of the tbox_sensor_ch_sensitivity control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="KeyEventArgs"/> instance containing the event data.</param>
        private void tbox_sensor_ch_sensitivity_KeyDown(object sender, KeyEventArgs e)
        {
            if (sender != null)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    this.SelectNextControl((Control)sender, true, true, true, true);
                }
            }
        }

        /// <summary>
        /// Handles the Validating event of the tbox_sensor_ch0_sensitivity control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="CancelEventArgs"/> instance containing the event data.</param>
        private void tbox_sensor_ch0_sensitivity_Validating(object sender, CancelEventArgs e)
        {
            if (sender != null)
            {
                TextBox tmp_sensor_ch0_sensitivity = (TextBox)sender;
                if (tmp_sensor_ch0_sensitivity.Text == "")
                {
                    return;
                }

                double sensor_ch0_sensitivity = Convert.ToDouble(tmp_sensor_ch0_sensitivity.Text);
                config_para.sensor_sensitivity = sensor_ch0_sensitivity;
                this.tbox_sensor_sensitivity.Text = config_para.sensor_sensitivity.ToString();
            }
        }


        /// <summary>
        /// Handles the Click event of the button1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        /// <summary>
        /// Handles the Click event of the btn_clear_chart control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btn_clear_chart_Click(object sender, EventArgs e)
        {
            GraphPane tmp_pane = this.zg_graph.GraphPane;
            tmp_pane.CurveList.Clear();
            this.zg_graph.Refresh();
        }

        /// <summary>
        /// Handles the KeyDown event of the tbox_encoderUnit_ToMillimeter control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="KeyEventArgs"/> instance containing the event data.</param>
        private void tbox_encoderUnit_ToMillimeter_KeyDown(object sender, KeyEventArgs e)
        {
            if (sender != null)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    this.SelectNextControl((Control)sender, true, true, true, true);
                }
            }

        }

        /// <summary>
        /// Handles the validating event of the tbox_encoderoUnit_ToMillimeter control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="CancelEventArgs"/> instance containing the event data.</param>
        private void tbox_encoderoUnit_ToMillimeter_validating(object sender, CancelEventArgs e)
        {
            if (sender != null)
            {
                TextBox tbox_encoder0Unit_Tomm = (TextBox)sender;
                if (tbox_encoder0Unit_Tomm.Text == "")
                {
                    return;
                }

                double encoder0_unit_mm = Convert.ToDouble(tbox_encoder0Unit_Tomm.Text);
                config_para.encoder_unit_to_mm = encoder0_unit_mm;
                this.tbox_encoderUnit_Tomm.Text = config_para.encoder_unit_to_mm.ToString();
            }

        }



        /// <summary>
        /// Handles the Validating event of the tbox_sensor_force_limit control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="CancelEventArgs"/> instance containing the event data.</param>
        private void tbox_sensor_force_limit_Validating(object sender, CancelEventArgs e)
        {
            if (sender != null)
            {
                TextBox tmp_sensor_force_limit = (TextBox)sender;
                if (tbox_sensor_force_limit.Text == "")
                {
                    return;
                }

                double limit = Convert.ToDouble(tmp_sensor_force_limit.Text);
                config_para.force_limit = limit;
                this.tbox_sensor_force_limit.Text = config_para.force_limit.ToString();
            }
        }

        /// <summary>
        /// Handles the Validating event of the tbox_sensor_return_position control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="CancelEventArgs"/> instance containing the event data.</param>
        private void tbox_sensor_return_position_Validating(object sender, CancelEventArgs e)
        {
            if (sender != null)
            {
                TextBox tmp_sensor_return_position = (TextBox)sender;
                if (tbox_sensor_return_position.Text == "")
                {
                    return;
                }

                double pos = Convert.ToDouble(tmp_sensor_return_position.Text);
                config_para.return_position = pos;
                this.tbox_sensor_return_position.Text = config_para.return_position.ToString();
            }
           
        }

        /// <summary>
        /// Handles the Click event of the button_demo control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void button_demo_Click(object sender, EventArgs e)
        {
            if (sender != null)
            {
                if (this.button_demo.Text == "Demo Mode")
                {
                    this.button_demo.Text = "Stop Demo";
                    DemoTimer.Interval = 100;
                    DemoTimer.Start();
                }
                else
                {
                    DemoTimer.Stop();
                    this.button_demo.Text = "Demo Mode";
                }
            }
        }

        /// <summary>
        /// Handles the Tick event of the demoTimer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void demoTimer_Tick(object sender, EventArgs e)
        {
            if (this.btn_general_op.Text == "Start Operation")
            {
                this.btn_general_op.PerformClick();
            }
           
        }


        /// <summary>
        /// timer tick function for UI updating.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void UiUpdateTimerTick(object sender, EventArgs e)
        {
            
            if (listHandler.ai_dataList == null)
                return;
            
            try
            {
                if (Monitor.TryEnter(listHandler.ai_dataList))
                {
                    try
                    {
                        if (config_para.is_operation_started == true && listHandler.ai_dataList.Count > listHandler.updatedCount)
                        {
                            PlotData();
                            Console.WriteLine("UI locked and update new");
                        }

                    }
                    finally
                    {
                        Monitor.Exit(listHandler.ai_dataList);
                        Console.WriteLine("UI released");
                    }
                }
            }
            catch (ArgumentNullException e1)
            {
                Console.WriteLine("ObjectLcok is dead");
            }
        }

        /// <summary>
        /// Finds the peaks.
        /// </summary>
        /// <param name="forwardMin">The forward minimum.</param>
        /// <param name="forwardMax">The forward maximum.</param>
        /// <param name="backwardMin">The backward minimum.</param>
        /// <param name="backwardMax">The backward maximum.</param>
        unsafe public void FindPeaks(double forwardMin, double forwardMax, double backwardMin, double backwardMax)
        {

            if (listHandler.ai_dataList != null)
            {
                double pPoint = Double.MinValue;
                double cPoint = Double.MaxValue;
                double mPoint = Double.MinValue;
                double rPoint = Double.MaxValue;
                bool isReturning = false;
                double ePoint = Convert.ToDouble(tbox_sensor_return_position.Text);
                double[] ai_update_buffer_position = new double[daqCard.GetSampleCountsPerChannel() / 2];
                double[] ai_update_buffer_force = new double[daqCard.GetSampleCountsPerChannel() / 2];
                listHandler.updatedCount = 0;
                for (int j = 0; j < listHandler.ai_dataList.Count; j++)
                {
                    IntPtr ptr;
                    fixed (int* p = listHandler.ai_dataList[j])
                    {
                        ptr = (IntPtr)p;
                    }
                    daqCard.ConvertRawData(ptr, config_para.load_cell_offset, config_para.encoder_unit_to_mm, config_para.sensor_sensitivity, ai_update_buffer_position, ai_update_buffer_force);

                    for (int i = 0; i < daqCard.GetTotalSampleCounts() / 2; i++)
                    {
                        if (Math.Abs(ai_update_buffer_position[i] - ePoint) < 0.1)
                            isReturning = true;
                        if (isReturning == false && ai_update_buffer_position[i] >= forwardMin && ai_update_buffer_position[i] <= forwardMax)
                        {
                            if (ai_update_buffer_force[i] >= pPoint)
                                pPoint = ai_update_buffer_force[i];
                            if (ai_update_buffer_force[i] <= cPoint)
                                cPoint = ai_update_buffer_force[i];
                        }
                        if (isReturning == true && ai_update_buffer_position[i] >= backwardMin && ai_update_buffer_position[i] <= backwardMax)
                        {
                            if (ai_update_buffer_force[i] >= mPoint)
                                mPoint = ai_update_buffer_force[i];
                            if (ai_update_buffer_force[i] <= rPoint)
                                rPoint = ai_update_buffer_force[i];
                        }
                    }
                    listHandler.updated();
                }

                if (pPoint == Double.MinValue)
                    pPoint = Double.NaN;
                if (cPoint == Double.MaxValue)
                    cPoint = Double.NaN;
                if (mPoint == Double.MinValue)
                    mPoint = Double.NaN;
                if (rPoint == Double.MaxValue)
                    rPoint = Double.NaN;

                tbox_peak_point.Text = pPoint.ToString("F2");
                tbox_contact_point.Text = cPoint.ToString("F2");
                tbox_max_return_point.Text = mPoint.ToString("F2");
                tbox_return_point.Text = rPoint.ToString("F2");
                tbox_ClickRatio.Text = ((pPoint - cPoint) / pPoint * 100.0).ToString("F2") + " %"; ;

            }
            
        }

        /// <summary>
        /// Handles the Click event of the button_save_data control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        unsafe private void button_save_data_Click(object sender, EventArgs e)
        {
            if (sender != null)
            {
                if (listHandler.ai_dataList != null)
                {
                    string fileName = "data.csv";
                    FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default);
                    double[] ai_update_buffer_position = new double[daqCard.GetSampleCountsPerChannel() / 2];
                    double[] ai_update_buffer_force = new double[daqCard.GetSampleCountsPerChannel() / 2];

                    listHandler.updatedCount = 0;
                    for (int j = 0; j < listHandler.ai_dataList.Count; j++)
                    {
                        IntPtr ptr;
                        fixed (int* p = listHandler.ai_dataList[j])
                        {
                            ptr = (IntPtr)p;
                        }
                        daqCard.ConvertRawData(ptr, config_para.load_cell_offset, config_para.encoder_unit_to_mm, config_para.sensor_sensitivity, ai_update_buffer_position, ai_update_buffer_force);
                        for (int i = 0; i < daqCard.GetTotalSampleCounts() / 2; i++)
                        {
                            sw.Write(ai_update_buffer_position[i].ToString() + "," + ai_update_buffer_force[i].ToString() + "\n");
                        }
                        listHandler.updated();
                    }
                    sw.Close();

                }


            }
        }

       
       

    }


    /// <summary>
    /// class program_config for program status monitoring and controlling
    /// </summary>
    class program_config
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="program_config"/> class.
        /// </summary>
        public program_config()
        {
            //
            // Program status
            //
            is_operation_started = false;
            //
            // Sensor settings
            //
            sensor_sensitivity = 50.2664;
            force_limit = 100.0;
            return_position = 1.852235;
            load_cell_offset = 0.0;
            //
            // encoder unit setting
            //
            encoder_unit_to_mm = 0.000385;
            // ZG Color
            curve_color = new Color[10] { Color.Red, Color.Blue, Color.Green, Color.Black, Color.Purple, Color.Yellow, Color.YellowGreen, Color.Gray, Color.LightBlue, Color.Orange };
            previous_curve_index = -1;
            curve_index = -1;
            //set first curve index to zero
            curve_index = 0;
        }
        // Program status
        /// <summary>
        /// The flag for checking if measuring operation started
        /// </summary>
        public bool is_operation_started;
        // Sensor settings
        /// <summary>
        /// The sensor sensitivity
        /// </summary>
        public double sensor_sensitivity;
        /// <summary>
        /// The force limit in grams for find reurn point operation 
        /// </summary>
        public double force_limit;
        /// <summary>
        /// The return position in in millimeters finded from find reurn point operation.
        /// This parameter also used in gerneal operation as the returning point.
        /// </summary>
        public double return_position;
        /// <summary>
        /// The load cell offset befor the measuring operation
        /// </summary>
        public double load_cell_offset;
        /// <summary>
        /// The convertion used for change encoder value to mm
        /// </summary>
        public double encoder_unit_to_mm;
        /// <summary>
        /// The curve color for the graph
        /// </summary>
        public Color[] curve_color;
        /// <summary>
        /// The curve index
        /// </summary>
        public int curve_index;
        /// <summary>
        /// The previous curve index
        /// </summary>
        public int previous_curve_index;
    }

    /// <summary>
    /// Class StoredListHandler.
    /// </summary>
    public class StoredListHandler
    {
        /// <summary>
        /// The ai data list
        /// </summary>
        public List<Int32[]> ai_dataList;
        /// <summary>
        /// The updated count
        /// </summary>
        public long updatedCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="StoredListHandler"/> class.
        /// </summary>
        public StoredListHandler()
        {
            ai_dataList = null;
            updatedCount = 0;
        }

        /// <summary>
        /// Resets the stored list.
        /// </summary>
        public void resetStoredList()
        {
            if (ai_dataList != null)
            {
                ai_dataList.Clear();
            }
            ai_dataList = new List<Int32[]>(5000);
            updatedCount = 0;
        }

        /// <summary>
        /// Updateds this instance.
        /// </summary>
        public void updated()
        {
            updatedCount++;
        }
    }
    

}

