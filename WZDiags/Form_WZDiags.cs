using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ZDiags;

namespace WZDiags
{
    public partial class Form_WZDiags : Form
    {

        Diags _diags;
        Task _run_task;

        public Form_WZDiags()
        {
            InitializeComponent();
        }

        private void Form_Load(object sender, EventArgs e)
        {
            Version version = System.Reflection.Assembly.GetEntryAssembly().GetName().Version;
            this.Text = this.Text + " " + version.ToString();

            textBox_Serial.Text = "";
            textBox_RunStatus.Text = "Ready";
            textBox_OutputStatus.Text = "";

            button_Run.Enabled = true;

        }

        private void button_Run_Click(object sender, EventArgs e)
        {
            Diags.Customers costumer = (Diags.Customers)Enum.Parse(typeof(Diags.Customers), Properties.Settings.Default.Costumer);

            _diags = new Diags(
                dut_port_name: Properties.Settings.Default.COM_DUT, 
                ble_port_name: Properties.Settings.Default.COM_BLE,
                smt_serial: textBox_Serial.Text.ToUpper(),
                customer: costumer,
                hw_version: Properties.Settings.Default.HwVer,
                tester: Properties.Settings.Default.Operator,
                hub_ip_addr: null
            );
            _diags.Status_Event += _diags_Status_Event;

            _diags.LogFolder = Properties.Settings.Default.Log_Folder;
            Directory.CreateDirectory(_diags.LogFolder);

            _diags.LED_Red_Low_Value = Properties.Settings.Default.LED_Red_Off_Val;
            _diags.LED_Red_High_Value = Properties.Settings.Default.LED_Red_On_Val;
            _diags.LED_Green_Low_Value = Properties.Settings.Default.LED_Green_Off_Val;
            _diags.LED_Green_High_Value = Properties.Settings.Default.LED_Green_On_Val;
            _diags.LED_Yellow_Low_Value = Properties.Settings.Default.LED_Yellow_Off_Val;
            _diags.LED_Yellow_High_Value = Properties.Settings.Default.LED_Yellow_On_Val;

            _run_task = new Task(() => _diags.Run());
            _run_task.ContinueWith(runDone, TaskContinuationOptions.OnlyOnRanToCompletion);
            _run_task.ContinueWith(runError, TaskContinuationOptions.OnlyOnFaulted);
            

            setRunning(true);

            textBox_OutputStatus.Clear();
            textBox_OutputStatus.AppendText("Parameters:\r\n");
            textBox_OutputStatus.AppendText(string.Format("Operator: {0}\r\n", _diags.Tester));
            textBox_OutputStatus.AppendText(string.Format("DUT COM: {0}\r\n", _diags.COM_DUT_Name));
            textBox_OutputStatus.AppendText(string.Format("BT COM: {0}\r\n", _diags.COM_BT_Name));
            textBox_OutputStatus.AppendText(string.Format("Customer: {0}\r\n", _diags.Customer.ToString()));
            textBox_OutputStatus.AppendText(string.Format("HW Ver: {0}\r\n", _diags.HW_Ver));
            textBox_OutputStatus.AppendText("\r\n");

            this.timer_UpdateRunning.Start();
            _run_task.Start();


        }

        private void _diags_Status_Event(object sender, string status_txt)
        {
            syncControlAppendText(textBox_OutputStatus, status_txt + "\r\n");
        }

        void runDone(Task task)
        {
            syncControlSetTextAndColor(textBox_RunStatus, "PASS", Color.White, Color.Green);
            syncControlAppendText(textBox_OutputStatus, "All Tests Passed for: " + textBox_Serial.Text + "\r\n");

            runTearDown();
        }

        void runError(Task task)
        {
            syncControlAppendText(textBox_OutputStatus, "\r\n" + task.Exception.InnerException.Message + "\r\n");
            syncControlAppendText(textBox_OutputStatus, task.Exception.InnerException.StackTrace + "\r\n");

            syncControlSetTextAndColor(textBox_RunStatus, "FAIL", Color.White, Color.Red);
            syncControlAppendText(textBox_OutputStatus, textBox_Serial.Text + " FAILED\r\n");

            runTearDown();
        }

        void runTearDown()
        {
            if (_diags != null)
                _diags.Dispose();

            setRunning(false);

            syncControlSetTextAndColor(textBox_Serial, "", Color.Black, Color.White);

            this.timer_UpdateRunning.Stop();
        }


        void setRunning(bool isRunning)
        {
            syncControlSetEnable(button_Run, !isRunning);
            syncControlSetEnable(textBox_Serial, !isRunning);

            if (isRunning)
                syncControlSetTextAndColor(textBox_RunStatus, "Running...", Color.Black, Color.White);
        }

        void syncControlSetEnable(Control control, bool enable)
        {
            synchronizedInvoke(control,
                delegate ()
                {
                    control.Enabled = enable;
                });
        }

        void syncControlAppendText(TextBox control, string text)
        {
            synchronizedInvoke(control,
                delegate ()
                {
                    control.AppendText(text);
                    control.Update();
                });
        }


        void syncControlSetTextAndColor(Control control, string text, Color forcolor, Color backcolor)
        {
            synchronizedInvoke(control,
                delegate ()
                {
                    control.Text = text;
                    control.ForeColor = forcolor;
                    control.BackColor = backcolor;
                    control.Update();
                });
        }

        private void timer_UpdateRunning_Tick(object sender, EventArgs e)
        {
            if(_run_task != null && !_run_task.IsCompleted)
            {
                textBox_RunStatus.Text += ".";
                if (textBox_RunStatus.Text.Length > 10)
                    textBox_RunStatus.Text = "Running";
            }
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_Settings dlg = new Form_Settings();
            dlg.Show();
        }

        void synchronizedInvoke(ISynchronizeInvoke sync, Action action)
        {
            // If the invoke is not required, then invoke here and get out.
            if (!sync.InvokeRequired)
            {
                // Execute action.
                action();

                // Get out.
                return;
            }

            try
            {
                // Marshal to the required context.
                sync.Invoke(action, new object[] { });
                //sync.BeginInvoke(action, new object[] { });
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
        }

        private void textBox_Serial_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
