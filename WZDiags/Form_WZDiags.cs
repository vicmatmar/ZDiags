using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ZDiags;

namespace WZDiags
{
    public partial class Form_WZDiags : Form
    {
        string _com_dut, _com_bt;

        Diags.Customer _custumer;
        int _hw_version;
        string _tester;

        Diags _diags;

        Task _run_task;

    

        public Form_WZDiags()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox_Serial.Text = "";
            textBox_RunStatus.Text = "Ready";
            textBox_OutputStatus.Text = "";

            button_Run.Enabled = true;

            _com_dut = "com12";
            _com_bt = "com11";
            _hw_version = 3;
            _tester = "Victor Martin";
        }

        private void button_Run_Click(object sender, EventArgs e)
        {
            _diags = new Diags(
                dut_port_name: _com_dut, ble_port_name: _com_bt,
                smt_serial: textBox_Serial.Text,
                customer: _custumer,
                hw_version: _hw_version,
                tester: _tester,
                hub_ip_addr: null
            );

            _diags.Status_Event += _diags_Status_Event;

            _run_task = new Task(() => _diags.Run());
            _run_task.ContinueWith(runDone, TaskContinuationOptions.OnlyOnRanToCompletion);
            _run_task.ContinueWith(runError, TaskContinuationOptions.OnlyOnFaulted);
            

            setRunning(true);
            this.timer_UpdateRunning.Start();
            _run_task.Start();


        }

        private void _diags_Status_Event(object sender, string status_txt)
        {
            syncControlAppendText(textBox_OutputStatus, status_txt + "\r\n");
        }

        void runDone(Task task)
        {
            if (_diags != null)
                _diags.Dispose();

            setRunning(false);

            syncControlSetTextAndColor(textBox_RunStatus, "PASS", Color.White, Color.Green);

            this.timer_UpdateRunning.Stop();

        }

        void runError(Task task)
        {
            if (_diags != null)
                _diags.Dispose();

            setRunning(false);

            syncControlAppendText(textBox_OutputStatus, task.Exception.InnerException.Message);
            syncControlAppendText(textBox_OutputStatus, task.Exception.InnerException.StackTrace);

            syncControlSetTextAndColor(textBox_RunStatus, "FAIL", Color.White, Color.Red);

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

    }
}
