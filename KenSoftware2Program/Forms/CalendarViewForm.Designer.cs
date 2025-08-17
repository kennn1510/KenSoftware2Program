namespace KenSoftware2Program.Forms
{
    partial class CalendarViewForm
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
            this.CalendarMonthCalendar = new System.Windows.Forms.MonthCalendar();
            this.CalendarDataGridView = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.CalendarDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // CalendarMonthCalendar
            // 
            this.CalendarMonthCalendar.Location = new System.Drawing.Point(18, 108);
            this.CalendarMonthCalendar.MaxSelectionCount = 1;
            this.CalendarMonthCalendar.Name = "CalendarMonthCalendar";
            this.CalendarMonthCalendar.TabIndex = 0;
            this.CalendarMonthCalendar.DateSelected += new System.Windows.Forms.DateRangeEventHandler(this.CalendarMonthCalendar_DateSelected);
            // 
            // CalendarDataGridView
            // 
            this.CalendarDataGridView.AllowUserToAddRows = false;
            this.CalendarDataGridView.AllowUserToDeleteRows = false;
            this.CalendarDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.CalendarDataGridView.Location = new System.Drawing.Point(337, 12);
            this.CalendarDataGridView.Name = "CalendarDataGridView";
            this.CalendarDataGridView.ReadOnly = true;
            this.CalendarDataGridView.RowHeadersWidth = 51;
            this.CalendarDataGridView.RowTemplate.Height = 24;
            this.CalendarDataGridView.Size = new System.Drawing.Size(774, 426);
            this.CalendarDataGridView.TabIndex = 1;
            // 
            // CalendarViewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1123, 450);
            this.Controls.Add(this.CalendarDataGridView);
            this.Controls.Add(this.CalendarMonthCalendar);
            this.Name = "CalendarViewForm";
            this.Text = "Calendar View";
            ((System.ComponentModel.ISupportInitialize)(this.CalendarDataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MonthCalendar CalendarMonthCalendar;
        private System.Windows.Forms.DataGridView CalendarDataGridView;
    }
}