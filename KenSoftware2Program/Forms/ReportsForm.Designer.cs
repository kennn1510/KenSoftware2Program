namespace KenSoftware2Program.Forms
{
    partial class ReportsForm
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
            this.Report1Label = new System.Windows.Forms.Label();
            this.Report1RichTextBox = new System.Windows.Forms.RichTextBox();
            this.Report2Label = new System.Windows.Forms.Label();
            this.Report2RichTextBox = new System.Windows.Forms.RichTextBox();
            this.Report3Label = new System.Windows.Forms.Label();
            this.Report3RichTextBox = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // Report1Label
            // 
            this.Report1Label.AutoSize = true;
            this.Report1Label.Location = new System.Drawing.Point(12, 18);
            this.Report1Label.Name = "Report1Label";
            this.Report1Label.Size = new System.Drawing.Size(242, 16);
            this.Report1Label.TabIndex = 0;
            this.Report1Label.Text = "Number of appointment types by month:\r\n";
            // 
            // Report1RichTextBox
            // 
            this.Report1RichTextBox.Location = new System.Drawing.Point(15, 51);
            this.Report1RichTextBox.Name = "Report1RichTextBox";
            this.Report1RichTextBox.ReadOnly = true;
            this.Report1RichTextBox.Size = new System.Drawing.Size(252, 387);
            this.Report1RichTextBox.TabIndex = 2;
            this.Report1RichTextBox.Text = "";
            // 
            // Report2Label
            // 
            this.Report2Label.AutoSize = true;
            this.Report2Label.Location = new System.Drawing.Point(344, 18);
            this.Report2Label.Name = "Report2Label";
            this.Report2Label.Size = new System.Drawing.Size(147, 16);
            this.Report2Label.TabIndex = 3;
            this.Report2Label.Text = "Schedule for each user:";
            // 
            // Report2RichTextBox
            // 
            this.Report2RichTextBox.Location = new System.Drawing.Point(300, 51);
            this.Report2RichTextBox.Name = "Report2RichTextBox";
            this.Report2RichTextBox.ReadOnly = true;
            this.Report2RichTextBox.Size = new System.Drawing.Size(252, 387);
            this.Report2RichTextBox.TabIndex = 4;
            this.Report2RichTextBox.Text = "";
            // 
            // Report3Label
            // 
            this.Report3Label.AutoSize = true;
            this.Report3Label.Location = new System.Drawing.Point(560, 18);
            this.Report3Label.Name = "Report3Label";
            this.Report3Label.Size = new System.Drawing.Size(312, 16);
            this.Report3Label.TabIndex = 5;
            this.Report3Label.Text = "Number of customers with and without appointments";
            // 
            // Report3RichTextBox
            // 
            this.Report3RichTextBox.Location = new System.Drawing.Point(593, 51);
            this.Report3RichTextBox.Name = "Report3RichTextBox";
            this.Report3RichTextBox.ReadOnly = true;
            this.Report3RichTextBox.Size = new System.Drawing.Size(258, 387);
            this.Report3RichTextBox.TabIndex = 6;
            this.Report3RichTextBox.Text = "";
            // 
            // ReportsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(915, 450);
            this.Controls.Add(this.Report3RichTextBox);
            this.Controls.Add(this.Report3Label);
            this.Controls.Add(this.Report2RichTextBox);
            this.Controls.Add(this.Report2Label);
            this.Controls.Add(this.Report1RichTextBox);
            this.Controls.Add(this.Report1Label);
            this.Name = "ReportsForm";
            this.Text = "Reports Form";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Report1Label;
        private System.Windows.Forms.RichTextBox Report1RichTextBox;
        private System.Windows.Forms.Label Report2Label;
        private System.Windows.Forms.RichTextBox Report2RichTextBox;
        private System.Windows.Forms.Label Report3Label;
        private System.Windows.Forms.RichTextBox Report3RichTextBox;
    }
}