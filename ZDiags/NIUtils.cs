using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NationalInstruments.DAQmx;

namespace ZDiags
{
    class NIUtils
    {
        static public double Read_SingelAi(uint linenum)
        {
            string ai_port_desc = get_PhysicalAIChannel(0);

            using (Task analogReaderTask = new Task())
            {
                //  Create channel and name it.
                string linestr = string.Format("{0}", ai_port_desc);
                string name = string.Format("ai{0}", linenum);
                analogReaderTask.AIChannels.CreateVoltageChannel(linestr, name, AITerminalConfiguration.Differential, 0, 5, AIVoltageUnits.Volts);

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

        // Define other methods and classes here
        static string get_PhysicalAIChannel(int index)
        {
            string[] data = DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.AI, PhysicalChannelAccess.External);
            return data[index];
        }

        static string get_PhysicalDOPortChannel(int index)
        {
            string[] data = DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.DOPort, PhysicalChannelAccess.External);
            return data[index];
        }
    }

}

