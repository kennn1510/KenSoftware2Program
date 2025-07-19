namespace KenSoftware2Program
{
    partial class Form1
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
            this.ConnectB = new System.Windows.Forms.Button();
            this.LoginLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ConnectB
            // 
            this.ConnectB.Location = new System.Drawing.Point(44, 33);
            this.ConnectB.Name = "ConnectB";
            this.ConnectB.Size = new System.Drawing.Size(75, 23);
            this.ConnectB.TabIndex = 0;
            this.ConnectB.Text = "Connect";
            this.ConnectB.UseVisualStyleBackColor = true;
            this.ConnectB.Click += new System.EventHandler(this.button1_Click);
            // 
            // LoginLabel
            // 
            this.LoginLabel.AutoSize = true;
            this.LoginLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LoginLabel.Location = new System.Drawing.Point(323, 9);
            this.LoginLabel.Name = "LoginLabel";
            this.LoginLabel.Size = new System.Drawing.Size(85, 32);
            this.LoginLabel.TabIndex = 1;
            this.LoginLabel.Text = "Login";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.LoginLabel);
            this.Controls.Add(this.ConnectB);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ConnectB;
        private System.Windows.Forms.Label LoginLabel;
    }
}

