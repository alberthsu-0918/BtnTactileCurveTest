
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using APS168_W32;
using APS_Define_W32;
using System.Runtime.InteropServices;


namespace AIO_Simultaneous_UI
{
    /// <summary>
    /// Struct gptc_config
    /// </summary>
    public struct gptc_config
    {
        /// <summary>
        /// The GPTC port
        /// </summary>
        public ushort[] GCtr;
        /// <summary>
        /// The mode
        /// </summary>
        public ushort Mode;
        /// <summary>
        /// The GPTC control parameter identifier
        /// </summary>
        public ushort[] GPTC_Ctrl_ParmeID;
        /// <summary>
        /// The GPTC number
        /// </summary>
        public ushort GPTCNum;
        /// <summary>
        /// Flag to set data combined with AI
        /// </summary>
        public bool SetCombined;
    }

    /// <summary>
    /// Struct ai_config
    /// </summary>
    public struct ai_config
    {
        /// <summary>
        /// The ai channel count
        /// </summary>
        public ushort ai_chnl_cnt;
        /// <summary>
        /// The ai channel range
        /// </summary>
        public ushort ai_chnl_range;
        /// <summary>
        /// The ai channel configuration
        /// </summary>
        public ushort ai_chnl_config;
        /// <summary>
        /// The trigger control
        /// </summary>
        public ushort trig_control;
        /// <summary>
        /// The ai sample rate
        /// </summary>
        public double ai_sample_rate;
        /// <summary>
        /// The ai raw data buf
        /// </summary>
        public IntPtr[] ai_raw_data_buf;
        /// <summary>
        /// The ai buffer identifier
        /// </summary>
        public ushort[] ai_buffer_id;
        /// <summary>
        /// flag for ai buffer seted
        /// </summary>
        public bool is_ai_set_buf;
        /// <summary>
        /// The ai sample count per channel
        /// </summary>
        public uint ai_sample_count_per_channel;
        /// <summary>
        /// The ai all data count
        /// </summary>
        public uint ai_all_data_count;
        /// <summary>
        /// The ai select channel
        /// </summary>
        public ushort ai_select_channel;
        // AI Operation status variables
        /// <summary>
        /// The ai buf ready index
        /// </summary>
        public uint ai_buf_ready_idx;
    }

    /// <summary>
    /// Class P9524 for DAQ card control.
    /// </summary>
    public class P9524
    {
        //
        // Device configuration variables
        //
        /// <summary>
        /// The card number
        /// </summary>
        private ushort card_num;
        /// <summary>
        /// The card handle
        /// </summary>
        private short card_handle;
        /// <summary>
        /// The latest encoder value
        /// </summary>
        private double latestEncoderValue;
        /// <summary>
        /// The latest voltage value
        /// </summary>
        private double latestVoltageValue;
        /// <summary>
        /// The GPTC control
        /// </summary>
        private gptc_config gptcCtrl;
        /// <summary>
        /// The configuration para
        /// </summary>
        private ai_config config_para;

        /// <summary>
        /// Initializes a new instance of the <see cref="P9524"/> class.
        /// </summary>
        public P9524()
        {
            card_num = 0;
            card_handle = -1;
            //GPTC
            gptcCtrl.GPTCNum = 3;
            gptcCtrl.GCtr = new ushort[gptcCtrl.GPTCNum];
            gptcCtrl.GCtr[0] = DASK.P9524_CTR_QD0;
            gptcCtrl.GCtr[1] = DASK.P9524_CTR_QD1;
            gptcCtrl.GCtr[2] = DASK.P9524_CTR_QD2;
            gptcCtrl.GPTC_Ctrl_ParmeID = new ushort[gptcCtrl.GPTCNum];
            gptcCtrl.GPTC_Ctrl_ParmeID[0] = DASK.P9524_CTR_Enable;
            gptcCtrl.GPTC_Ctrl_ParmeID[1] = DASK.P9524_CTR_Enable;
            gptcCtrl.GPTC_Ctrl_ParmeID[2] = DASK.P9524_CTR_Enable;
            gptcCtrl.Mode = DASK.P9524_x4_AB_Phase_Decoder;
            gptcCtrl.SetCombined = true;
            //AI
            config_para.ai_chnl_cnt = 1; //PCI-9524 has four AI. Set 1 channel now.
            config_para.ai_chnl_range = 0; //PCI-9524's range of Load Cell group can only be set to 0
            config_para.ai_chnl_config = DASK.P9524_VEX_Range_10V | DASK.P9524_AI_BufAutoReset | DASK.P9524_AI_AZMode;// | DASK.P9524_VEX_Sence_Remote;
            config_para.trig_control = 0;
            config_para.ai_sample_rate = DASK.P9524_ADC_2K_SPS;
            //Buffers
            // AI
            config_para.ai_sample_count_per_channel = 128; //4096
            config_para.ai_all_data_count = config_para.ai_sample_count_per_channel * config_para.ai_chnl_cnt;
            config_para.ai_buffer_id = new ushort[2] { 0, 0 };
            config_para.is_ai_set_buf = false;
            config_para.ai_select_channel = DASK.P9524_AI_LC_CH0;

            //
            // AI Operation status variables
            // 
            config_para.ai_buf_ready_idx = 0;
        }

        /// <summary>
        /// Registers the card.
        /// </summary>
        /// <param name="card_num">The card number.</param>
        /// <returns>System.Int16.</returns>
        public short RegisterCard(ushort card_num)
        {
            if (card_handle < 0 || this.card_num != card_num)
            {
                short handle = DASK.Register_Card(DASK.PCI_9524, card_num);
                if (handle < 0)
                {
                    return handle;//return error coed if error happened.
                }
                else
                {
                    ReleaseCard();
                    this.card_num = card_num;
                    this.card_handle = handle;
                }
            }
            return DASK.NoError;

        }

        /// <summary>
        /// Releases the card.
        /// </summary>
        /// <returns>System.Int16.</returns>
        public short ReleaseCard()
        {
            short err = DASK.NoError;
            if (card_handle >= 0)
            {
                if (config_para.is_ai_set_buf)
                {
                    // Reset buffer
                    Marshal.FreeHGlobal(config_para.ai_raw_data_buf[0]);
                    Marshal.FreeHGlobal(config_para.ai_raw_data_buf[1]);
                    config_para.is_ai_set_buf = false;
                }
                err = DASK.Release_Card((ushort)card_handle);
                card_handle = -1;
            }
            return err;
        }

        /// <summary>
        /// Gets the load cell offset.
        /// Use this function to get an average of offset when load cell is in steady status
        /// </summary>
        /// <returns>System.Double.</returns>
        /// <exception cref="InvalidOperationException">
        /// AI_AsyncDblBufferMode Fail, error:  " + err
        /// or
        /// AI_ContBufferSetup Fail, error:  " + err
        /// or
        /// GetLoadCellOffset SetDSP Fail, error:  " + err
        /// or
        /// AI_ContScanChannels Fail, error:  " + err
        /// </exception>
        public double GetLoadCellOffset()
        {
            double offset = 0.0;
            short err = -1;
            IntPtr OffsetCalBf;
            double[] OffsetVolbuf;
            uint calSampleNum = 512;
            ushort BufID;

            OffsetCalBf = Marshal.AllocHGlobal((int)(sizeof(uint) * calSampleNum));
            OffsetVolbuf = new double[((int)calSampleNum)];

            ///////Disable fun of AI data combined with enconder, otherwise the caculated offset is going to BOOM 
            
            err = DASK.GPTC_9524_SetCombineEcdData(GetHandle(), false);
            if (err < 0)
            {
                throw new InvalidOperationException("GPTC_9524_SetCombineEcdData Fail, error:  " + err);
            }
            
            
            for (int i = 0; i < gptcCtrl.GPTCNum; i++)
            {
                err = DASK.GPTC_Clear(GetHandle(), gptcCtrl.GCtr[i]);
                if (err < 0)
                {
                    throw new InvalidOperationException("GPTC_Clear Fail, error:  " + err);
                }
            }           
            ///////

            err = DASK.AI_AsyncDblBufferMode((ushort)GetHandle(), false);
            if (err < 0)
            {
                throw new InvalidOperationException("AI_AsyncDblBufferMode Fail, error:  " + err);
            }

            err = DASK.AI_ContBufferSetup((ushort)GetHandle(), OffsetCalBf, calSampleNum, out BufID);
            if (err < 0)
            {
                throw new InvalidOperationException("AI_ContBufferSetup Fail, error:  " + err);
            }

            err = SetDSP();
            if (err < 0)
            {
                throw new InvalidOperationException("GetLoadCellOffset SetDSP Fail, error:  " + err);
            }


            // Start Acquisition. The acquired raw data will be stored in the set buffer.
            //ushort ADC_SampRate = DASK.P9524_ADC_2K_SPS;
            err = DASK.AI_ContScanChannels((ushort)GetHandle(), config_para.ai_select_channel, config_para.ai_chnl_range, new ushort[] { BufID }, calSampleNum, config_para.ai_sample_rate, DASK.SYNCH_OP);
            if (err < 0)
            {
                throw new InvalidOperationException("AI_ContScanChannels Fail, error:  " + err);
            }

            DASK.AI_ContVScale(GetHandle(), config_para.ai_chnl_range, OffsetCalBf, OffsetVolbuf, (int)calSampleNum);

            for (int i = 0; i < calSampleNum; i++)
            {
                offset = offset + OffsetVolbuf[i];
            }
            offset = offset / calSampleNum;
            DASK.AI_ContBufferReset(GetHandle());

            uint ai_access_cnt;
            DASK.AI_AsyncClear(GetHandle(), out ai_access_cnt);
            return offset;
        }

        /// <summary>
        /// Configurations the specified half buffer read and ai end callback address.
        /// </summary>
        /// <param name="halfReadCallbackAddr">The half buffer read callback address.</param>
        /// <param name="AiEndCallbackAddrr">The ai end callback address.</param>
        /// <returns>System.Int16.</returns>
        public short Config(MulticastDelegate halfReadCallbackAddr, MulticastDelegate AiEndCallbackAddrr)
        {
            short err = DASK.NoError;
            RegisterCard(this.card_num);
            err = GonfigGPTC();
            err = SetupAcqCallbacks(halfReadCallbackAddr, AiEndCallbackAddrr);
            err = SetDSP();
            return err;
        }

        /// <summary>
        /// Starts the acquisition.
        /// </summary>
        /// <returns>System.Int16.</returns>
        /// <exception cref="InvalidOperationException">AI_ContScanChannels Fail, error:  " + err</exception>
        public short StartAcq()
        {
            config_para.ai_buf_ready_idx = 0;
            latestEncoderValue = Double.NaN;
            latestVoltageValue = Double.NaN;
            short err = DASK.AI_ContScanChannels(GetHandle(), config_para.ai_select_channel, config_para.ai_chnl_range, new ushort[] { config_para.ai_buffer_id[0] }, config_para.ai_all_data_count, config_para.ai_sample_rate, DASK.ASYNCH_OP);
            if (err < 0)
            {
                throw new InvalidOperationException("AI_ContScanChannels Fail, error:  " + err);
            }
            return err;
        }

        /// <summary>
        /// Polling a voltage data.
        /// </summary>
        /// <returns>System.Double.</returns>
        /// <exception cref="InvalidOperationException">
        /// AI_ReadChannel32 Fail, error:  " + err
        /// or
        /// AI_VoltScale32 Fail, error:  " + err
        /// </exception>
        public double VoltagePolling()
        {
            double volt;
            uint value;
            short err = DASK.AI_ReadChannel32(GetHandle(), config_para.ai_select_channel, config_para.ai_chnl_range, out value);
            if (err < 0)
            {
                throw new InvalidOperationException("AI_ReadChannel32 Fail, error:  " + err);
            }

            err = DASK.AI_VoltScale32(GetHandle(), config_para.ai_chnl_range, (int)value, out volt);
            if (err < 0)
            {
                throw new InvalidOperationException("AI_VoltScale32 Fail, error:  " + err);
            }
            return volt;
        }

        /// <summary>
        /// Polling an encorder data.
        /// </summary>
        /// <returns>System.Double.</returns>
        /// <exception cref="InvalidOperationException">GPTC_Read Fail, error:  " + err</exception>
        public double EncoderPolling()
        {
            uint value;
            short err = DASK.GPTC_Read(GetHandle(), gptcCtrl.GCtr[0], out value);
            if (err < 0)
            {
                throw new InvalidOperationException("GPTC_Read Fail, error:  " + err);
            }
            return Math.Abs((value * 256) / 256); ;
        }

        /// <summary>
        /// Gets the handle.
        /// </summary>
        /// <returns>System.UInt16.</returns>
        /// <exception cref="System.InvalidOperationException">No card_handle available</exception>
        protected ushort GetHandle()
        {
            if (card_handle < 0)
                throw new InvalidOperationException("No card_handle available");

            return (ushort)card_handle;
        }

        /// <summary>
        /// Gets the actual rate per channel.
        /// </summary>
        /// <returns>System.Double.</returns>
        /// <exception cref="InvalidOperationException">GetActualRate_9524 Fail, error:  " + err</exception>
        public double GetActualRatePerChannel()
        {
            double ActualRate;
            short err = DASK.GetActualRate_9524(GetHandle(), DASK.P9524_AI_LC_Group, config_para.ai_sample_rate, out ActualRate);
            if (err < 0)
            {
                throw new InvalidOperationException("GetActualRate_9524 Fail, error:  " + err);
            }
            return ActualRate / config_para.ai_chnl_cnt;
        }

        /// <summary>
        /// Gets the channel counts.
        /// </summary>
        /// <returns>System.UInt32.</returns>
        public uint GetChannelCounts()
        {
            return config_para.ai_chnl_cnt;
        }

        /// <summary>
        /// Gets the sample counts per channel.
        /// </summary>
        /// <returns>System.UInt32.</returns>
        public uint GetSampleCountsPerChannel()
        {
            return config_para.ai_sample_count_per_channel;
        }

        /// <summary>
        /// Gets the total sample counts.
        /// </summary>
        /// <returns>System.UInt32.</returns>
        public uint GetTotalSampleCounts()
        {
            return config_para.ai_all_data_count;
        }

        /// <summary>
        /// Converts the raw data to position and force arrays.
        /// </summary>
        /// <param name="readingArray">The reading array.</param>
        /// <param name="loadCellOffset">The load cell offset.</param>
        /// <param name="unitTomm">The scalling factor to convert encoder unit to millimeter.</param>
        /// <param name="sensitivity">The sensitivity of loadcell.</param>
        /// <param name="position">The position array.</param>
        /// <param name="force">The force array.</param>
        /// <returns>System.Int16.</returns>
        public short ConvertRawData(IntPtr readingArray, double loadCellOffset, double unitTomm, double sensitivity, double[] position, double[] force)
        {
            short err = DASK.NoError;
            double[] ai_scale_data_buf = new double[config_para.ai_all_data_count];
            DASK.AI_ContVScale(GetHandle(), config_para.ai_chnl_range, readingArray, ai_scale_data_buf, (int)config_para.ai_all_data_count);
            //seperate data
            unsafe
            {
                int* offsetPtr = (int*)readingArray.ToPointer();
                for (int i = 0; i < config_para.ai_all_data_count / 2; i++)
                {
                    //Use two list, list of AI voltage (forece) and list of Encoder (position), to replace
                    position[i] = unitTomm * Math.Abs((double)(*(offsetPtr+i * 2 + 1) * 256) / 256);
                    force[i] = 1000.0 * sensitivity * (ai_scale_data_buf[i * 2] - loadCellOffset);
                }
            }
            return err;
        }

        /// <summary>
        /// Transfers the buffer.
        /// This function mark the buffer is transfered during double buffer operation.
        /// Check the double buffer overrun to make sure data is not lost.
        /// </summary>
        /// <param name="dataArray">The data array.</param>
        /// <returns>System.Int16.</returns>
        /// <exception cref="InvalidOperationException">Double buffer overrun</exception>
        public short TransferBuffer(Int32[] dataArray)
        {
            short err = DASK.NoError;
            ushort overrunFlag = 0;
            IntPtr latestIndex = config_para.ai_raw_data_buf[(config_para.ai_buf_ready_idx + 1) % 2];
            Marshal.Copy(config_para.ai_raw_data_buf[config_para.ai_buf_ready_idx], dataArray, 0, (int)GetSampleCountsPerChannel());
            DASK.AI_VoltScale32(GetHandle(), config_para.ai_chnl_range, dataArray[GetSampleCountsPerChannel()-2], out latestVoltageValue);
            latestEncoderValue = Math.Abs((dataArray[GetSampleCountsPerChannel() - 1] * 256) / 256);
            config_para.ai_buf_ready_idx += 1;
            config_para.ai_buf_ready_idx %= 2;

            err = DASK.AI_AsyncDblBufferHandled(GetHandle());
            err = DASK.AI_AsyncDblBufferOverrun(GetHandle(), 0, out overrunFlag);
            if (overrunFlag != 0)
            {
                throw new InvalidOperationException("Double buffer overrun");
            }
            return err;
        }

        /// <summary>
        /// Gets the latest voltage value updated after TransferBuffer.
        /// </summary>
        /// <returns>System.Double.</returns>
        public double GetLatestVoltageValue()
        {
            return latestVoltageValue;
        }

        /// <summary>
        /// Getlatests the encoder value updated after TransferBuffer.
        /// </summary>
        /// <returns>System.Double.</returns>
        public double GetlatestEncoderValue()
        {
            return latestEncoderValue;
        }

        /// <summary>
        /// Stops the acquisition.
        /// </summary>
        /// <returns>System.Int16.</returns>
        /// <exception cref="InvalidOperationException">AI_9524_Config Fail, error:  " + err</exception>
        public short StopAcq()
        {
            short err = DASK.NoError;
            uint ai_access_cnt;
            err = DASK.AI_AsyncClear(GetHandle(), out ai_access_cnt);
            if (err < 0)
            {
                throw new InvalidOperationException("AI_9524_Config Fail, error:  " + err);
            }

            return err;
        }

        /// <summary>
        /// Gonfigs the GPTC setting.
        /// </summary>
        /// <returns>System.Int16.</returns>
        /// <exception cref="InvalidOperationException">
        /// GPTC_Clear Fail, error:  " + err
        /// or
        /// GPTC_Setup Fail, error:  " + err
        /// or
        /// GPTC_Control Fail, error:  " + err
        /// or
        /// GPTC_9524_SetCombineEcdData Fail, error:  " + err
        /// </exception>
        private short GonfigGPTC()
        {
            short err = DASK.NoError;
            // GPTC Setup
            for (int i = 0; i < gptcCtrl.GPTCNum; i++)
            {

               
                err = DASK.GPTC_Clear(GetHandle(), gptcCtrl.GCtr[i]);
                if (err < 0)
                {
                    throw new InvalidOperationException("GPTC_Clear Fail, error:  " + err);
                }

               
                err = DASK.GPTC_Setup(GetHandle(), gptcCtrl.GCtr[i], gptcCtrl.Mode, 0, 0, 0, 0);
                if (err < 0)
                {
                    throw new InvalidOperationException("GPTC_Setup Fail, error:  " + err);
                }


                err = DASK.GPTC_Control(GetHandle(), gptcCtrl.GCtr[i], gptcCtrl.GPTC_Ctrl_ParmeID[i], 1);
                if (err < 0)
                {
                    throw new InvalidOperationException("GPTC_Control Fail, error:  " + err);
                }
            }

            if (gptcCtrl.SetCombined)
            {
                err = DASK.GPTC_9524_SetCombineEcdData(GetHandle(), true);
                if (err < 0)
                {
                    throw new InvalidOperationException("GPTC_9524_SetCombineEcdData Fail, error:  " + err);
                }
            }
            return err;
        }

        /// <summary>
        /// Setups the acquisition callbacks.
        /// </summary>
        /// <param name="halfReadCallbackAddr">The half read callback addr.</param>
        /// <param name="AiEndCallbackAddrr">The ai end callback addrr.</param>
        /// <returns>System.Int16.</returns>
        /// <exception cref="System.InvalidOperationException">
        /// AI_AsyncDblBufferMode Fail, error:  " + err
        /// or
        /// AI_ContBufferSetup Fail, error:  " + err
        /// or
        /// AI_ContBufferSetup Fail, error:  " + err
        /// or
        /// AI_EventCallBack Fail, error:  " + err
        /// or
        /// AI_EventCallBack Fail, error:  " + err
        /// </exception>
        private short SetupAcqCallbacks(MulticastDelegate halfReadCallbackAddr, MulticastDelegate AiEndCallbackAddrr)
        {
            short err = DASK.NoError;
            // Double Buffer Setup
            config_para.ai_raw_data_buf = new IntPtr[2];
            config_para.ai_raw_data_buf[0] = Marshal.AllocHGlobal((int)(sizeof(uint) * config_para.ai_all_data_count));
            config_para.ai_raw_data_buf[1] = Marshal.AllocHGlobal((int)(sizeof(uint) * config_para.ai_all_data_count));
            config_para.is_ai_set_buf = true;

            err = DASK.AI_AsyncDblBufferMode(GetHandle(), true);
            if (err < 0)
            {
                throw new InvalidOperationException("AI_AsyncDblBufferMode Fail, error:  " + err);
            }

            err = DASK.AI_ContBufferSetup(GetHandle(), config_para.ai_raw_data_buf[0], config_para.ai_all_data_count, out config_para.ai_buffer_id[0]);
            if (err < 0)
            {
                throw new InvalidOperationException("AI_ContBufferSetup Fail, error:  " + err);
            }

            err = DASK.AI_ContBufferSetup(GetHandle(), config_para.ai_raw_data_buf[1], config_para.ai_all_data_count, out config_para.ai_buffer_id[1]);
            if (err < 0)
            {
                throw new InvalidOperationException("AI_ContBufferSetup Fail, error:  " + err);
            }

            // Set AI buffer Ready event
            if (halfReadCallbackAddr != null)
            {
                err = DASK.AI_EventCallBack(GetHandle(), 1/*add*/, DASK.DBEvent/*EventType*/, halfReadCallbackAddr);
                if (err != DASK.NoError)
                {
                    throw new InvalidOperationException("AI_EventCallBack Fail, error:  " + err);

                }
            }


            // Set AI done event
            if (AiEndCallbackAddrr != null)
            {
                err = DASK.AI_EventCallBack(GetHandle(), 1/*add*/, DASK.AIEnd/*EventType*/, AiEndCallbackAddrr);
                if (err != DASK.NoError)
                {
                    throw new InvalidOperationException("AI_EventCallBack Fail, error:  " + err);
                }
            }
            
            return err;
        }

        /// <summary>
        /// Sets the DSP settings.
        /// </summary>
        /// <returns>System.Int16.</returns>
        /// <exception cref="InvalidOperationException">
        /// AI_9524_SetDSP Fail, error:  " + err
        /// or
        /// AI_9524_Config Fail, error:  " + err
        /// </exception>
        private short SetDSP()
        {
            short err = DASK.NoError;
            ushort DFStage = 0;
            uint SPKRejThreshold = 100;
            uint dwTrigValue = 0;


            for (ushort i = 0; i < config_para.ai_chnl_cnt; i++)
                err = DASK.AI_9524_SetDSP(GetHandle(), i, DASK.P9524_SPIKE_REJ_DISABLE, DFStage, SPKRejThreshold);

            if (err < 0)
            {
                throw new InvalidOperationException("AI_9524_SetDSP Fail, error:  " + err);
            }


            err = DASK.AI_9524_Config(GetHandle(), DASK.P9524_AI_LC_Group, DASK.P9524_AI_XFER_DMA, config_para.ai_chnl_config, config_para.trig_control, (ushort)dwTrigValue);
            if (err < 0)
            {
                throw new InvalidOperationException("AI_9524_Config Fail, error:  " + err);
            }

            return err;
        }

    }

    


    
}