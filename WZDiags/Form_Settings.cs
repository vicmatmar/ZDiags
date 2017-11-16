using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO.Ports;

namespace WZDiags
{
    public partial class Form_Settings : Form
    {
        public Form_Settings()
        {
            InitializeComponent();
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Operator = textBox_Operator.Text;

            Properties.Settings.Default.COM_DUT = comboBox_ComDUT.Text;
            Properties.Settings.Default.COM_BLE = comboBox_ComBT.Text;

            Properties.Settings.Default.HwVer = Convert.ToInt32(numericUpDown_HWVer.Value);
            Properties.Settings.Default.Costumer = domainUpDown_Customer.Text;

            // LEDs
            Properties.Settings.Default.LED_Red_Off_Val = (double)Red_min_numericUpDown.Value;
            Properties.Settings.Default.LED_Red_On_Val = (double)Red_max_numericUpDown.Value;
            Properties.Settings.Default.LED_Green_Off_Val = (double)Green_min_numericUpDown.Value;
            Properties.Settings.Default.LED_Green_On_Val = (double)Green_max_numericUpDown.Value;
            Properties.Settings.Default.LED_Yellow_Off_Val = (double)Yellow_min_numericUpDown.Value;
            Properties.Settings.Default.LED_Yellow_On_Val = (double)Yellow_max_numericUpDown.Value;

            Properties.Settings.Default.Save();

            Close();
        }

        private void Form_Settings_Load(object sender, EventArgs e)
        {
            textBox_Operator.Text = Properties.Settings.Default.Operator;

            comboBox_ComDUT.Text = Properties.Settings.Default.COM_DUT;
            comboBox_ComBT.Text = Properties.Settings.Default.COM_BLE;

            numericUpDown_HWVer.Value = Properties.Settings.Default.HwVer;
            domainUpDown_Customer.Text = Properties.Settings.Default.Costumer;

            string[] portnames = SerialPort.GetPortNames();
            comboBox_ComDUT.Items.AddRange(portnames);
            comboBox_ComBT.Items.AddRange(portnames);

            domainUpDown_Customer.Items.Clear();
            domainUpDown_Customer.Items.AddRange(Enum.GetNames(typeof(ZDiags.Diags.Customers)));

            // LEDs
            Red_min_numericUpDown.Value = (decimal)Properties.Settings.Default.LED_Red_Off_Val;
            Red_max_numericUpDown.Value = (decimal)Properties.Settings.Default.LED_Red_On_Val;
            Green_min_numericUpDown.Value = (decimal)Properties.Settings.Default.LED_Green_Off_Val;
            Green_max_numericUpDown.Value = (decimal)Properties.Settings.Default.LED_Green_On_Val;
            Yellow_min_numericUpDown.Value = (decimal)Properties.Settings.Default.LED_Yellow_Off_Val;
            Yellow_max_numericUpDown.Value = (decimal)Properties.Settings.Default.LED_Yellow_On_Val;


        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
