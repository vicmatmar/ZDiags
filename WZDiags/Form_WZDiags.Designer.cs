﻿namespace WZDiags
{
    partial class Form_WZDiags
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
            this.components = new System.ComponentModel.Container();
            this.button_Run = new System.Windows.Forms.Button();
            this.textBox_RunStatus = new System.Windows.Forms.TextBox();
            this.textBox_OutputStatus = new System.Windows.Forms.TextBox();
            this.textBox_Serial = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.timer_UpdateRunning = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // button_Run
            // 
            this.button_Run.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Run.Location = new System.Drawing.Point(473, 12);
            this.button_Run.Name = "button_Run";
            this.button_Run.Size = new System.Drawing.Size(75, 23);
            this.button_Run.TabIndex = 16;
            this.button_Run.Text = "&Run";
            this.button_Run.UseVisualStyleBackColor = true;
            this.button_Run.Click += new System.EventHandler(this.button_Run_Click);
            // 
            // textBox_RunStatus
            // 
            this.textBox_RunStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_RunStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_RunStatus.Location = new System.Drawing.Point(13, 41);
            this.textBox_RunStatus.Name = "textBox_RunStatus";
            this.textBox_RunStatus.ReadOnly = true;
            this.textBox_RunStatus.Size = new System.Drawing.Size(536, 49);
            this.textBox_RunStatus.TabIndex = 15;
            this.textBox_RunStatus.Text = "test";
            this.textBox_RunStatus.WordWrap = false;
            // 
            // textBox_OutputStatus
            // 
            this.textBox_OutputStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_OutputStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_OutputStatus.ForeColor = System.Drawing.SystemColors.WindowText;
            this.textBox_OutputStatus.Location = new System.Drawing.Point(12, 96);
            this.textBox_OutputStatus.Multiline = true;
            this.textBox_OutputStatus.Name = "textBox_OutputStatus";
            this.textBox_OutputStatus.ReadOnly = true;
            this.textBox_OutputStatus.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_OutputStatus.Size = new System.Drawing.Size(536, 396);
            this.textBox_OutputStatus.TabIndex = 14;
            this.textBox_OutputStatus.WordWrap = false;
            // 
            // textBox_Serial
            // 
            this.textBox_Serial.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_Serial.Location = new System.Drawing.Point(83, 13);
            this.textBox_Serial.Name = "textBox_Serial";
            this.textBox_Serial.Size = new System.Drawing.Size(384, 20);
            this.textBox_Serial.TabIndex = 17;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 18;
            this.label1.Text = "Enter Serial:";
            // 
            // timer_UpdateRunning
            // 
            this.timer_UpdateRunning.Interval = 500;
            this.timer_UpdateRunning.Tick += new System.EventHandler(this.timer_UpdateRunning_Tick);
            // 
            // Form_WZDiags
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(560, 504);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox_Serial);
            this.Controls.Add(this.button_Run);
            this.Controls.Add(this.textBox_RunStatus);
            this.Controls.Add(this.textBox_OutputStatus);
            this.Name = "Form_WZDiags";
            this.Text = "WZDiags";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_Run;
        private System.Windows.Forms.TextBox textBox_RunStatus;
        private System.Windows.Forms.TextBox textBox_OutputStatus;
        private System.Windows.Forms.TextBox textBox_Serial;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer timer_UpdateRunning;
    }
}

