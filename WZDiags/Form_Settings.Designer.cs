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
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.Yellow_max_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.Yellow_min_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.Green_max_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.Green_min_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.Red_max_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.Red_label = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.Red_min_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_HWVer)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Yellow_max_numericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Yellow_min_numericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Green_max_numericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Green_min_numericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Red_max_numericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Red_min_numericUpDown)).BeginInit();
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
            this.button_OK.Location = new System.Drawing.Point(100, 277);
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
            this.button_Cancel.Location = new System.Drawing.Point(181, 277);
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
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.tableLayoutPanel1);
            this.groupBox3.Location = new System.Drawing.Point(12, 152);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(323, 114);
            this.groupBox3.TabIndex = 8;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "LEDs";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.Yellow_max_numericUpDown, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.Yellow_min_numericUpDown, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.Green_max_numericUpDown, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.Green_min_numericUpDown, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.Red_max_numericUpDown, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.label7, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.label6, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.Red_label, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label8, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.Red_min_numericUpDown, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label9, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label10, 0, 3);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(24, 19);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(267, 89);
            this.tableLayoutPanel1.TabIndex = 8;
            // 
            // Yellow_max_numericUpDown
            // 
            this.Yellow_max_numericUpDown.DecimalPlaces = 3;
            this.Yellow_max_numericUpDown.Location = new System.Drawing.Point(161, 69);
            this.Yellow_max_numericUpDown.Maximum = new decimal(new int[] {
            6,
            0,
            0,
            0});
            this.Yellow_max_numericUpDown.Name = "Yellow_max_numericUpDown";
            this.Yellow_max_numericUpDown.Size = new System.Drawing.Size(102, 20);
            this.Yellow_max_numericUpDown.TabIndex = 13;
            // 
            // Yellow_min_numericUpDown
            // 
            this.Yellow_min_numericUpDown.DecimalPlaces = 3;
            this.Yellow_min_numericUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.Yellow_min_numericUpDown.Location = new System.Drawing.Point(53, 69);
            this.Yellow_min_numericUpDown.Maximum = new decimal(new int[] {
            6,
            0,
            0,
            0});
            this.Yellow_min_numericUpDown.Name = "Yellow_min_numericUpDown";
            this.Yellow_min_numericUpDown.Size = new System.Drawing.Size(102, 20);
            this.Yellow_min_numericUpDown.TabIndex = 12;
            // 
            // Green_max_numericUpDown
            // 
            this.Green_max_numericUpDown.DecimalPlaces = 3;
            this.Green_max_numericUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.Green_max_numericUpDown.Location = new System.Drawing.Point(161, 47);
            this.Green_max_numericUpDown.Maximum = new decimal(new int[] {
            6,
            0,
            0,
            0});
            this.Green_max_numericUpDown.Name = "Green_max_numericUpDown";
            this.Green_max_numericUpDown.Size = new System.Drawing.Size(102, 20);
            this.Green_max_numericUpDown.TabIndex = 10;
            // 
            // Green_min_numericUpDown
            // 
            this.Green_min_numericUpDown.DecimalPlaces = 3;
            this.Green_min_numericUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.Green_min_numericUpDown.Location = new System.Drawing.Point(53, 47);
            this.Green_min_numericUpDown.Maximum = new decimal(new int[] {
            6,
            0,
            0,
            0});
            this.Green_min_numericUpDown.Name = "Green_min_numericUpDown";
            this.Green_min_numericUpDown.Size = new System.Drawing.Size(102, 20);
            this.Green_min_numericUpDown.TabIndex = 9;
            // 
            // Red_max_numericUpDown
            // 
            this.Red_max_numericUpDown.DecimalPlaces = 3;
            this.Red_max_numericUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.Red_max_numericUpDown.Location = new System.Drawing.Point(161, 25);
            this.Red_max_numericUpDown.Maximum = new decimal(new int[] {
            6,
            0,
            0,
            0});
            this.Red_max_numericUpDown.Name = "Red_max_numericUpDown";
            this.Red_max_numericUpDown.Size = new System.Drawing.Size(102, 20);
            this.Red_max_numericUpDown.TabIndex = 7;
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(197, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(30, 13);
            this.label7.TabIndex = 4;
            this.label7.Text = "Max";
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(90, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(27, 13);
            this.label6.TabIndex = 3;
            this.label6.Text = "Min";
            // 
            // Red_label
            // 
            this.Red_label.AutoSize = true;
            this.Red_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Red_label.Location = new System.Drawing.Point(3, 0);
            this.Red_label.Name = "Red_label";
            this.Red_label.Size = new System.Drawing.Size(36, 13);
            this.Red_label.TabIndex = 2;
            this.Red_label.Text = "Color";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 22);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(27, 13);
            this.label8.TabIndex = 5;
            this.label8.Text = "Red";
            // 
            // Red_min_numericUpDown
            // 
            this.Red_min_numericUpDown.DecimalPlaces = 3;
            this.Red_min_numericUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.Red_min_numericUpDown.Location = new System.Drawing.Point(53, 25);
            this.Red_min_numericUpDown.Maximum = new decimal(new int[] {
            6,
            0,
            0,
            0});
            this.Red_min_numericUpDown.Name = "Red_min_numericUpDown";
            this.Red_min_numericUpDown.Size = new System.Drawing.Size(102, 20);
            this.Red_min_numericUpDown.TabIndex = 6;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(3, 44);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(36, 13);
            this.label9.TabIndex = 8;
            this.label9.Text = "Green";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(3, 66);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(38, 13);
            this.label10.TabIndex = 11;
            this.label10.Text = "Yellow";
            // 
            // Form_Settings
            // 
            this.AcceptButton = this.button_OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button_Cancel;
            this.ClientSize = new System.Drawing.Size(357, 312);
            this.Controls.Add(this.groupBox3);
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
            this.groupBox3.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Yellow_max_numericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Yellow_min_numericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Green_max_numericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Green_min_numericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Red_max_numericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Red_min_numericUpDown)).EndInit();
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
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label Red_label;
        private System.Windows.Forms.NumericUpDown Yellow_max_numericUpDown;
        private System.Windows.Forms.NumericUpDown Yellow_min_numericUpDown;
        private System.Windows.Forms.NumericUpDown Green_max_numericUpDown;
        private System.Windows.Forms.NumericUpDown Green_min_numericUpDown;
        private System.Windows.Forms.NumericUpDown Red_max_numericUpDown;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown Red_min_numericUpDown;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
    }
}