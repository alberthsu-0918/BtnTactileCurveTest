using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using APS168_W32;
using APS_Define_W32;


namespace AIO_Simultaneous_UI
{
    /// <summary>
    /// Class Amp204C for motion card control.
    /// </summary>
    /// <seealso cref="AIO_Simultaneous_UI.MotionCard" />
    public class Amp204C : MotionCard
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="Amp204C"/> class.
        /// </summary>
        public Amp204C()
        {
            mCardPara.mode = 0;
            mCardPara.AxisID = 0;

        }

        /// <summary>
        /// Initials the specified axis identifier.
        /// </summary>
        /// <param name="Axis_ID">The axis identifier.</param>
        /// <returns>System.Int32.</returns>
        public override int InitialAxis(Int32 Axis_ID)
        {
            int err = -1;
            err = APS168.APS_initial(ref mCardPara.boardID_InBits, mCardPara.mode);
            mCardPara.AxisID = Axis_ID;
            this.currentDirection = MotionDirection.Initial;
            return err;
        }

        /// <summary>
        /// Sets the servo on.
        /// </summary>
        /// <param name="On">The on.</param>
        /// <returns>System.Int32.</returns>
        public override int SetServoOn(int On)
        {
            //servo on
            int err = -1;
            err = APS168.APS_set_servo_on(mCardPara.AxisID, On);
            System.Threading.Thread.Sleep(500); // Wait stable.

            return err;
        }

        /// <summary>
        /// To the home position.
        /// </summary>
        /// <returns>System.Int32.</returns>
        public override int ToHome()
        {
            int err = -1;
            err = APS168.APS_set_axis_param(mCardPara.AxisID, (Int32)APS_Define.PRA_HOME_MODE, 0); //Set home mode
            if (err != (Int32)APS_Define.ERR_NoError) return err;
            err = APS168.APS_set_axis_param(mCardPara.AxisID, (Int32)APS_Define.PRA_HOME_DIR, 0); //Set home direction
            if (err != (Int32)APS_Define.ERR_NoError) return err;

            err = APS168.APS_set_axis_param(mCardPara.AxisID, (Int32)APS_Define.PRA_HOME_CURVE, 0); // Set acceleration pattern (T-curve)
            if (err != (Int32)APS_Define.ERR_NoError) return err;
            err = APS168.APS_set_axis_param(mCardPara.AxisID, (Int32)APS_Define.PRA_HOME_ACC, 2000000); //Set homing acceleration rate
            if (err != (Int32)APS_Define.ERR_NoError) return err;
            err = APS168.APS_set_axis_param(mCardPara.AxisID, (Int32)APS_Define.PRA_HOME_VM, 200000); //Set homing maximum velocity.
            if (err != (Int32)APS_Define.ERR_NoError) return err;
            err = APS168.APS_set_axis_param(mCardPara.AxisID, (Int32)APS_Define.PRA_HOME_VO, 200000); //Set homing leave home velocity
            if (err != (Int32)APS_Define.ERR_NoError) return err;
            /*
            err = APS168.APS_set_axis_param(axisID, (Int32)APS_Define.PRA_HOME_EZA, 0); // Set EZ signal alignment (yes or no)
            if (err != (Int32)APS_Define.ERR_NoError) return err;
            err = APS168.APS_set_axis_param(axisID, (Int32)APS_Define.PRA_HOME_SHIFT, 0); // Set home position shfit distance.
            if (err != (Int32)APS_Define.ERR_NoError) return err;
            err = APS168.APS_set_axis_param(axisID, (Int32)APS_Define.PRA_HOME_POS, 0); // Set final home position.
            if (err != (Int32)APS_Define.ERR_NoError) return err;
          */
            /*
            err = APS168.APS_set_axis_param_f(axisID, (Int32)APS_Define.PRA_HOME_CURVE, 0.5); //Set s-factor to 0.5
            if (err != (Int32)APS_Define.ERR_NoError) return err;
            err = APS168.APS_set_axis_param_f(axisID, (Int32)APS_Define.PRA_HOME_ACC, 10000.0); //Set homing acceleration rate
            if (err != (Int32)APS_Define.ERR_NoError) return err;
            err = APS168.APS_set_axis_param_f(axisID, (Int32)APS_Define.PRA_HOME_VM, 100000.0); //Set homing maximum velocity.
            if (err != (Int32)APS_Define.ERR_NoError) return err;
            err = APS168.APS_set_axis_param_f(axisID, (Int32)APS_Define.PRA_HOME_VO, 20000.0); //Set homing leave home velocity
            if (err != (Int32)APS_Define.ERR_NoError) return err;
             */


            err = APS168.APS_home_move(mCardPara.AxisID);
            if (err != (Int32)APS_Define.ERR_NoError) return err;
            
            Int32 check = -1;
            do{
                check = APS168.APS_motion_io_status(mCardPara.AxisID);
            }while(check!=0x88);

           
            
            
            return err;
        }

        /// <summary>
        /// To the start positoin(-116000).
        /// </summary>
        /// <returns>System.Int32.</returns>
        public override int ToStartPositoin()
        {
            int err = -1;
            int pos = 0;

            if (this.currentDirection != MotionDirection.Stop || this.currentDirection != MotionDirection.Initial)
            {
                err = APS168.APS_stop_move(mCardPara.AxisID);
                System.Threading.Thread.Sleep(200);
            }
            int startPos = -116000;//-116000 for smart phone demo; -50000 for Fred's customer
            err = APS168.APS_get_position(mCardPara.AxisID, ref pos);
            if (pos == startPos)
                return err;

            APS168.APS_set_axis_param_f(mCardPara.AxisID, (Int32)APS_Define.PRA_STP_DEC, 1000000.0);
            APS168.APS_set_axis_param_f(mCardPara.AxisID, (Int32)APS_Define.PRA_CURVE, 0.5); //Set acceleration rate
            APS168.APS_set_axis_param_f(mCardPara.AxisID, (Int32)APS_Define.PRA_ACC, 2000000.0); //Set acceleration rate
            APS168.APS_set_axis_param_f(mCardPara.AxisID, (Int32)APS_Define.PRA_DEC, 2000000.0); //Set deceleration rate

            err = APS168.APS_absolute_move(mCardPara.AxisID, startPos, 200000);
            do
            {
                APS168.APS_get_position(mCardPara.AxisID, ref pos);
            } while (pos != startPos);
            this.currentDirection = MotionDirection.Stop;
            return err;
            
        }

        /// <summary>
        /// Point to point move.
        /// </summary>
        /// <param name="dir">The direction.</param>
        /// <param name="distance">The distance.</param>
        public override void PtpMove(Int32 dir, Int32 distance)
        {
            int err = -1;
            Int32 Option = dir;         //0: Positive direction   1:negative direction
            ASYNCALL p = new ASYNCALL();

            //err = APS168.APS_set_axis_param_f(Axis_ID, (Int32)APS_Define.PRA_CURVE, 0.5);
            //if (err != (Int32)APS_Define.ERR_NoError) return;
            //err = APS168.APS_set_axis_param_f(Axis_ID, (Int32)APS_Define.PRA_ACC, 1000000.0);
            //if (err != (Int32)APS_Define.ERR_NoError) return;
            //err = APS168.APS_set_axis_param_f(Axis_ID, (Int32)APS_Define.PRA_DEC, 1000000.0);
            //if (err != (Int32)APS_Define.ERR_NoError) return;
            //err = APS168.APS_set_axis_param_f(Axis_ID, (Int32)APS_Define.PRA_VM, 100000.0);
            //if (err != (Int32)APS_Define.ERR_NoError) return err;

            int pos = 0;
            int prePos = 0;
            int refPos = 0;
            if (Option == 0)
                distance *= -1;
            //System.Threading.Thread.Sleep(10);

            //if (dir == 0)
            //{
            //    int motionStatus = 0;
            //    do{
            //        motionStatus = APS168.APS_motion_io_status(Axis_ID);
            //        if (motionStatus!=128)
            //             motionStatus =0;
            //        motionStatus &= 0x40;
            //        motionStatus = motionStatus >> 6;
            //        } while (motionStatus != 1);
            //}

            //err = APS168.APS_get_position(Axis_ID, ref refPos);
            err = APS168.APS_get_position(mCardPara.AxisID, ref prePos);
            do
            {
                if (prePos != refPos)
                    prePos = refPos;
                APS168.APS_get_position(mCardPara.AxisID, ref refPos);
                System.Threading.Thread.Sleep(4);
            } while (prePos != refPos);


            if (err != (Int32)APS_Define.ERR_NoError) return;

            if (dir == 0)
            {
                this.currentDirection = MotionDirection.Returning;
            }
            else
            {
                this.currentDirection = MotionDirection.Pressing;
            }

            err = APS168.APS_ptp_all(mCardPara.AxisID, 0, refPos - distance, 0.0, 3000.0, 0.0, 10000000.0, 10000000.0, 0.5, ref p);
            //err = APS168.APS_absolute_move(Axis_ID, refPos - distance, 600);
            if (err != (Int32)APS_Define.ERR_NoError) return;

            if (this.currentDirection == MotionDirection.Returning)
            {
                do
                {
                    APS168.APS_get_position(mCardPara.AxisID, ref pos);
                    System.Threading.Thread.Sleep(1);
                } while (pos != refPos - distance && this.currentDirection != MotionDirection.Stop);
                this.currentDirection = MotionDirection.Stop;
            }

            return;
        }


        // void SetupInterrupt( Int32 item, Int32 factor, MulticastDelegate EventAddr, ref InterruputSetup someinterr)
        // Motion Interrupt setup function
        //Parameters:
        // item:       the selected axis.  
        // Factor:     type of interrupt(when to interrupt)
        // EventAddr:  Delegate function. It is action after interrupt.  
        // EventArg :  Argument for interrupt function: EventAddr
        // someinterr: record setup
        /// <summary>
        /// Setups the interrupt.
        /// </summary>
        /// <param name="factor">Type of interrupt(when to interrupt).</param>
        /// <param name="EventAddr">Delegate function. It is action after interrupt.</param>
        /// <param name="EventArg">Argument for interrupt function: EventAddr.</param>
        /// <param name="someinterr">record setup.</param>
        public override void SetupInterrupt(Int32 factor, MulticastDelegate EventAddr, object[] EventArg, ref InterruputSetup someinterr)
        {
            someinterr.board_id = this.mCardPara.boardID_InBits;
            someinterr.item = mCardPara.AxisID;
            someinterr.factor = factor;
            someinterr.int_no = APS168.APS_set_int_factor(/*someinterr.board_id*/0, someinterr.item, someinterr.factor, 1);
            someinterr.EventAddr = EventAddr;
            int argNum = EventArg.Length;
            someinterr.EventArgs = new object[argNum];

            for (int i = 0; i < argNum; i++)
            {
                someinterr.EventArgs[i] = EventArg[i];
            }
            

            APS168.APS_int_enable(0, 1);
        }


        /// <summary>
        /// Creates the interrupt.
        /// </summary>
        /// <param name="mtcrl">The MTCRL.</param>
        public override void CreateInterrupt(Object mtcrl)
        {
            Int32 err = APS168.APS_wait_single_int(((Amp204C.InterruputSetup)mtcrl).int_no, -1); //Wait interrupt
           
            if (err == (Int32)APS_Define.ERR_NoError)
            { //Interrupt occurred	
                
                //Disable Interrupt
                APS168.APS_reset_int(((Amp204C.InterruputSetup)mtcrl).int_no);
                APS168.APS_set_int_factor(/*someinterr.board_id*/0, ((Amp204C.InterruputSetup)mtcrl).item, ((Amp204C.InterruputSetup)mtcrl).factor, 1);
                APS168.APS_int_enable(0, 0);

                //Return
                ((Amp204C.InterruputSetup)mtcrl).EventAddr.DynamicInvoke(((Amp204C.InterruputSetup)mtcrl).EventArgs);
                //err = APS168.APS_vel(0, 0, 4000, ref p);
                //this.currentDirection = MotionDirection.Returning;
                //if(this.currentDirection == MotionDirection.Returning)
                //    this.currentDirection = MotionDirection.Stop;
                
            }
        }


        /// <summary>
        /// Velocity move(speed_1 = 200.0).
        /// </summary>
        /// <param name="dir">The direction.</param>
        public override void VelocityMove(Int32 dir)
        {
            Int32 Option = dir;         //0: Positive direction   1:negative direction
            ASYNCALL p = new ASYNCALL();
            double speed = 200.0; //1000-2000 may be the final selection speed

            APS168.APS_set_axis_param_f(mCardPara.AxisID, (Int32)APS_Define.PRA_STP_DEC, 1000000.0);
            APS168.APS_set_axis_param_f(mCardPara.AxisID, (Int32)APS_Define.PRA_CURVE, 0.5); //Set acceleration rate
            APS168.APS_set_axis_param_f(mCardPara.AxisID, (Int32)APS_Define.PRA_ACC, 1000000.0); //Set acceleration rate
            APS168.APS_set_axis_param_f(mCardPara.AxisID, (Int32)APS_Define.PRA_DEC, 1000000.0); //Set deceleration rate

            //go
            APS168.APS_vel(mCardPara.AxisID, Option, speed, ref p);
            if (dir == 0)
            {
                this.currentDirection = MotionDirection.Returning;
            }
            else
            {
                this.currentDirection = MotionDirection.Pressing;
            }
        }


        /// <summary>
        /// Stops the movement.
        /// </summary>
        public override void Stop()
        {
            APS168.APS_set_axis_param_f(mCardPara.AxisID, (Int32)APS_Define.PRA_STP_DEC, 1000000.0);
            APS168.APS_stop_move(mCardPara.AxisID);
            this.currentDirection = MotionDirection.Stop;
        }


        /// <summary>
        /// Closes this instance.
        /// </summary>
        /// <returns>System.Int32.</returns>
        public override int Close()
        {
            Int32 Servo_On = 0; // 0 = OFF
            this.currentDirection = MotionDirection.Stop;
            APS168.APS_int_enable(0, 0);
            APS168.APS_set_servo_on(mCardPara.AxisID, Servo_On);
            return APS168.APS_close();
        }

        /// <summary>
        /// Gets the status.
        /// </summary>
        /// <returns>System.Int32.</returns>
        public override int GetStatus()
        {
            return APS168.APS_motion_io_status(mCardPara.AxisID);
        }
    }
}
