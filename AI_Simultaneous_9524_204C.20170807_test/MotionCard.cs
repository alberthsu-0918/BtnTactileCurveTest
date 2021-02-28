using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace AIO_Simultaneous_UI
{
    public enum MotionDirection { Stop = 0, Pressing, Returning, Initial };

    /// <summary>
    /// Base class for motion card control.
    /// </summary>
    public abstract class MotionCard 
    {
        protected struct CardPmeter
        {
            public Int32 boardID_InBits;
            public Int32 mode;
            public Int32 card_name;
            public Int32 AxisID;
            public Int32 TotalAxisNum;
        }
        protected CardPmeter mCardPara;

        public struct InterruputSetup
        {
            public Int32 board_id;
            public Int32 item;
            public Int32 factor;
            public Int32 int_no;
            public Delegate EventAddr;
            public object[] EventArgs;
        }
        public InterruputSetup[] interruptsetup = new InterruputSetup[12];
        public MotionDirection currentDirection;

        public MotionCard()
        {
            this.currentDirection = MotionDirection.Initial;
        }


        public abstract int InitialAxis(Int32 Axis_ID);
        public abstract int SetServoOn(int On);
        public abstract int ToHome();
        public abstract int ToStartPositoin();
        public abstract void PtpMove(Int32 dir, Int32 distance);
        public abstract void SetupInterrupt(Int32 factor, MulticastDelegate EventAddr, object[] EventArg, ref InterruputSetup someinterr);
        public abstract void CreateInterrupt(Object mtcrl/*Int32 Board_ID, Int32 Axis_num, Int32 factor, int timeout*/);
        public abstract void VelocityMove(Int32 dir);
        public abstract void Stop();
        public abstract int Close();
        public abstract int GetStatus();
    }
}
