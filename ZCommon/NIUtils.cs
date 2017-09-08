using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NationalInstruments.DAQmx;

namespace ZCommon
{
    public class NIUtils
    {
        static public double Read_SingelAi(uint linenum)
        {
            string ai_port_desc = get_PhysicalAIChannel((int)linenum);

            using (Task analogReaderTask = new Task())
            {
                //  Create channel and name it.
                string linestr = string.Format("{0}", ai_port_desc);
                string name = string.Format("ai{0}", linenum);
                analogReaderTask.AIChannels.CreateVoltageChannel(linestr, name, AITerminalConfiguration.Rse, 0, 5, AIVoltageUnits.Volts);

                AnalogSingleChannelReader reader = new AnalogSingleChannelReader(analogReaderTask.Stream);
                double value = reader.ReadSingleSample();
                return value;
            }

        }

        static public void Write_SingleDIO(uint linenum, bool value)
        {
            string ni_port_desc = get_PhysicalDOPortChannel(0);

            using (NationalInstruments.DAQmx.Task digitalWriteTask = new Task())
            {
                //  Create an Digital Output channel and name it.
                string linestr = string.Format("{0}/line{1}", ni_port_desc, linenum);
                string name = string.Format("line{0}", linenum);
                digitalWriteTask.DOChannels.CreateChannel(linestr, name, ChannelLineGrouping.OneChannelForEachLine);

                //  Write digital port data. WriteDigitalSingChanSingSampPort writes a single sample
                //  of digital data on demand, so no timeout is necessary.
                DigitalSingleChannelWriter writer = new DigitalSingleChannelWriter(digitalWriteTask.Stream);
                writer.WriteSingleSampleSingleLine(true, value);
            }

        }
                
        static string get_PhysicalAIChannel(int index)
        {
            string[] data = DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.AI, PhysicalChannelAccess.External);
            return data[index];
        }

        static string get_PhysicalDOPortChannel(int index)
        {
            string[] data = DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.DOPort, PhysicalChannelAccess.External);
            if (data.Length == 0)
                throw new Exception("NI box not detected");
            return data[index];
        }
    }

}

