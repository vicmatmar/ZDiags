namespace WZDiags
{
    partial class Form_Settings
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.comboBox_ComBT = new System.Windows.Forms.ComboBox();
            this.comboBox_ComDUT = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDown_HWVer = new System.Windows.Forms.NumericUpDown();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.domainUpDown_Customer = new System.Windows.Forms.DomainUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.button_OK = new System.Windows.Forms.Button();
            this.button_Cancel = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox_Operator = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_HWVer)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.comboBox_ComBT);
            this.groupBox1.Controls.Add(this.comboBox_ComDUT);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 46);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(148, 100);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Serial Ports";
            // 
            // comboBox_ComBT
            // 
            this.comboBox_ComBT.FormattingEnabled = true;
            this.comboBox_ComBT.Location = new System.Drawing.Point(60, 58);
            this.comboBox_ComBT.Name = "comboBox_ComBT";
            this.comboBox_ComBT.Size = new System.Drawing.Size(70, 21);
            this.comboBox_ComBT.TabIndex = 3;
            // 
            // comboBox_ComDUT
            // 
            this.comboBox_ComDUT.FormattingEnabled = true;
            this.comboBox_ComDUT.Location = new System.Drawing.Point(60, 23);
            this.comboBox_ComDUT.Name = "comboBox_ComDUT";
            this.comboBox_ComDUT.Size = new System.Drawing.Size(70, 21);
            this.comboBox_ComDUT.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(24, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "BT:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "DUT:";
            // 
            // numericUpDown_HWVer
            // 
            this.numericUpDown_HWVer.Location = new System.Drawing.Point(60, 62);
            this.numericUpDown_HWVer.Name = "numericUpDown_HWVer";
            this.numericUpDown_HWVer.Size = new System.Drawing.Size(37, 20);
            this.numericUpDown_HWVer.TabIndex = 1;
            this.numericUpDown_HWVer.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDown_HWVer.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.domainUpDown_Customer);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.numericUpDown_HWVer);
            this.groupBox2.Location = new System.Drawing.Point(166, 46);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(169, 100);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Unit Info";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 26);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(54, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Customer:";
            // 
            // domainUpDown_Customer
            // 
            this.domainUpDown_Customer.Items.Add("IRIS");
            this.domainUpDown_Customer.Items.Add("Amazone");
            this.domainUpDown_Customer.Location = new System.Drawing.Point(66, 24);
            this.domainUpDown_Customer.Name = "domainUpDown_Customer";
            this.domainUpDown_Customer.Size = new System.Drawing.Size(79, 20);
            this.domainUpDown_Customer.TabIndex = 4;
            this.domainUpDown_Customer.Text = "Amazone";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 62);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "HW Ver:";
            // 
            // button_OK
            // 
            this.button_OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button_OK.Location = new System.Drawing.Point(85, 166);
            this.button_OK.Name = "button_OK";
            this.button_OK.Size = new System.Drawing.Size(75, 23);
            this.button_OK.TabIndex = 3;
            this.button_OK.Text = "&OK";
            this.button_OK.UseVisualStyleBackColor = true;
            this.button_OK.Click += new System.EventHandler(this.button_OK_Click);
            // 
            // button_Cancel
            // 
            this.button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_Cancel.Location = new System.Drawing.Point(173, 166);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(75, 23);
            this.button_Cancel.TabIndex = 4;
            this.button_Cancel.Text = "&Cancel";
            this.button_Cancel.UseVisualStyleBackColor = true;
            this.button_Cancel.Click += new System.EventHandler(this.button_Cancel_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 12);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(51, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Operator:";
            // 
            // textBox_Operator
            // 
            this.textBox_Operator.Location = new System.Drawing.Point(72, 9);
            this.textBox_Operator.Name = "textBox_Operator";
            this.textBox_Operator.Size = new System.Drawing.Size(256, 20);
            this.textBox_Operator.TabIndex = 6;
            // 
            // Form_Settings
            // 
            this.AcceptButton = this.button_OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button_Cancel;
            this.ClientSize = new System.Drawing.Size(340, 201);
            this.Controls.Add(this.textBox_Operator);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.button_Cancel);
            this.Controls.Add(this.button_OK);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "Form_Settings";
            this.Text = "WZdiags Settings";
            this.Load += new System.EventHandler(this.Form_Settings_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_HWVer)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button_OK;
        private System.Windows.Forms.Button button_Cancel;
        private System.Windows.Forms.Label label5;
        public System.Windows.Forms.ComboBox comboBox_ComBT;
        public System.Windows.Forms.ComboBox comboBox_ComDUT;
        public System.Windows.Forms.NumericUpDown numericUpDown_HWVer;
        public System.Windows.Forms.DomainUpDown domainUpDown_Customer;
        public System.Windows.Forms.TextBox textBox_Operator;
    }
}