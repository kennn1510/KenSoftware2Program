namespace KenSoftware2Program.Forms
{
    partial class AppointmentForm
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
            this.TitleLabel = new System.Windows.Forms.Label();
            this.DescriptionLabel = new System.Windows.Forms.Label();
            this.LocationLabel = new System.Windows.Forms.Label();
            this.ContactLabel = new System.Windows.Forms.Label();
            this.TypeLabel = new System.Windows.Forms.Label();
            this.UrlLabel = new System.Windows.Forms.Label();
            this.StartLabel = new System.Windows.Forms.Label();
            this.EndLabel = new System.Windows.Forms.Label();
            this.TitleTextBox = new System.Windows.Forms.TextBox();
            this.DescriptionTextBox = new System.Windows.Forms.TextBox();
            this.LocationTextBox = new System.Windows.Forms.TextBox();
            this.ContactTextBox = new System.Windows.Forms.TextBox();
            this.UrlTextBox = new System.Windows.Forms.TextBox();
            this.AddAppointmentButton = new System.Windows.Forms.Button();
            this.TypeComboBox = new System.Windows.Forms.ComboBox();
            this.AppointmentDataGridView = new System.Windows.Forms.DataGridView();
            this.CalendarViewButton = new System.Windows.Forms.Button();
            this.EditAppointmentButton = new System.Windows.Forms.Button();
            this.DeleteAppointmentButton = new System.Windows.Forms.Button();
            this.EndDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.StartDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.StartErrorLabel = new System.Windows.Forms.Label();
            this.EndErrorLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.AppointmentDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // TitleLabel
            // 
            this.TitleLabel.AutoSize = true;
            this.TitleLabel.Location = new System.Drawing.Point(12, 27);
            this.TitleLabel.Name = "TitleLabel";
            this.TitleLabel.Size = new System.Drawing.Size(36, 16);
            this.TitleLabel.TabIndex = 0;
            this.TitleLabel.Text = "Title:";
            // 
            // DescriptionLabel
            // 
            this.DescriptionLabel.AutoSize = true;
            this.DescriptionLabel.Location = new System.Drawing.Point(12, 64);
            this.DescriptionLabel.Name = "DescriptionLabel";
            this.DescriptionLabel.Size = new System.Drawing.Size(78, 16);
            this.DescriptionLabel.TabIndex = 1;
            this.DescriptionLabel.Text = "Description:";
            // 
            // LocationLabel
            // 
            this.LocationLabel.AutoSize = true;
            this.LocationLabel.Location = new System.Drawing.Point(12, 99);
            this.LocationLabel.Name = "LocationLabel";
            this.LocationLabel.Size = new System.Drawing.Size(61, 16);
            this.LocationLabel.TabIndex = 2;
            this.LocationLabel.Text = "Location:";
            // 
            // ContactLabel
            // 
            this.ContactLabel.AutoSize = true;
            this.ContactLabel.Location = new System.Drawing.Point(12, 140);
            this.ContactLabel.Name = "ContactLabel";
            this.ContactLabel.Size = new System.Drawing.Size(55, 16);
            this.ContactLabel.TabIndex = 3;
            this.ContactLabel.Text = "Contact:";
            // 
            // TypeLabel
            // 
            this.TypeLabel.AutoSize = true;
            this.TypeLabel.Location = new System.Drawing.Point(15, 180);
            this.TypeLabel.Name = "TypeLabel";
            this.TypeLabel.Size = new System.Drawing.Size(42, 16);
            this.TypeLabel.TabIndex = 4;
            this.TypeLabel.Text = "Type:";
            // 
            // UrlLabel
            // 
            this.UrlLabel.AutoSize = true;
            this.UrlLabel.Location = new System.Drawing.Point(12, 219);
            this.UrlLabel.Name = "UrlLabel";
            this.UrlLabel.Size = new System.Drawing.Size(37, 16);
            this.UrlLabel.TabIndex = 5;
            this.UrlLabel.Text = "URL:";
            // 
            // StartLabel
            // 
            this.StartLabel.AutoSize = true;
            this.StartLabel.Location = new System.Drawing.Point(15, 277);
            this.StartLabel.Name = "StartLabel";
            this.StartLabel.Size = new System.Drawing.Size(37, 16);
            this.StartLabel.TabIndex = 7;
            this.StartLabel.Text = "Start:";
            // 
            // EndLabel
            // 
            this.EndLabel.AutoSize = true;
            this.EndLabel.Location = new System.Drawing.Point(15, 336);
            this.EndLabel.Name = "EndLabel";
            this.EndLabel.Size = new System.Drawing.Size(34, 16);
            this.EndLabel.TabIndex = 8;
            this.EndLabel.Text = "End:";
            // 
            // TitleTextBox
            // 
            this.TitleTextBox.Location = new System.Drawing.Point(93, 21);
            this.TitleTextBox.Name = "TitleTextBox";
            this.TitleTextBox.Size = new System.Drawing.Size(191, 22);
            this.TitleTextBox.TabIndex = 12;
            // 
            // DescriptionTextBox
            // 
            this.DescriptionTextBox.Location = new System.Drawing.Point(93, 58);
            this.DescriptionTextBox.Name = "DescriptionTextBox";
            this.DescriptionTextBox.Size = new System.Drawing.Size(191, 22);
            this.DescriptionTextBox.TabIndex = 13;
            // 
            // LocationTextBox
            // 
            this.LocationTextBox.Location = new System.Drawing.Point(93, 99);
            this.LocationTextBox.Name = "LocationTextBox";
            this.LocationTextBox.Size = new System.Drawing.Size(191, 22);
            this.LocationTextBox.TabIndex = 14;
            // 
            // ContactTextBox
            // 
            this.ContactTextBox.Location = new System.Drawing.Point(93, 140);
            this.ContactTextBox.Name = "ContactTextBox";
            this.ContactTextBox.Size = new System.Drawing.Size(191, 22);
            this.ContactTextBox.TabIndex = 15;
            // 
            // UrlTextBox
            // 
            this.UrlTextBox.Location = new System.Drawing.Point(93, 219);
            this.UrlTextBox.Name = "UrlTextBox";
            this.UrlTextBox.Size = new System.Drawing.Size(191, 22);
            this.UrlTextBox.TabIndex = 17;
            // 
            // AddAppointmentButton
            // 
            this.AddAppointmentButton.Enabled = false;
            this.AddAppointmentButton.Location = new System.Drawing.Point(18, 376);
            this.AddAppointmentButton.Name = "AddAppointmentButton";
            this.AddAppointmentButton.Size = new System.Drawing.Size(254, 36);
            this.AddAppointmentButton.TabIndex = 18;
            this.AddAppointmentButton.Text = "Add Appointment";
            this.AddAppointmentButton.UseVisualStyleBackColor = true;
            this.AddAppointmentButton.Click += new System.EventHandler(this.AddAppointmentButton_Click);
            // 
            // TypeComboBox
            // 
            this.TypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.TypeComboBox.FormattingEnabled = true;
            this.TypeComboBox.Items.AddRange(new object[] {
            "Presentation",
            "Scrum",
            "Remote",
            "Interview",
            "Assessment"});
            this.TypeComboBox.Location = new System.Drawing.Point(93, 180);
            this.TypeComboBox.Name = "TypeComboBox";
            this.TypeComboBox.Size = new System.Drawing.Size(191, 24);
            this.TypeComboBox.TabIndex = 19;
            // 
            // AppointmentDataGridView
            // 
            this.AppointmentDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.AppointmentDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.AppointmentDataGridView.Location = new System.Drawing.Point(290, 68);
            this.AppointmentDataGridView.MultiSelect = false;
            this.AppointmentDataGridView.Name = "AppointmentDataGridView";
            this.AppointmentDataGridView.ReadOnly = true;
            this.AppointmentDataGridView.RowHeadersWidth = 51;
            this.AppointmentDataGridView.RowTemplate.Height = 24;
            this.AppointmentDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.AppointmentDataGridView.Size = new System.Drawing.Size(794, 344);
            this.AppointmentDataGridView.TabIndex = 20;
            // 
            // CalendarViewButton
            // 
            this.CalendarViewButton.Location = new System.Drawing.Point(290, 14);
            this.CalendarViewButton.Name = "CalendarViewButton";
            this.CalendarViewButton.Size = new System.Drawing.Size(794, 36);
            this.CalendarViewButton.TabIndex = 21;
            this.CalendarViewButton.Text = "Calendar View";
            this.CalendarViewButton.UseVisualStyleBackColor = true;
            // 
            // EditAppointmentButton
            // 
            this.EditAppointmentButton.Location = new System.Drawing.Point(290, 418);
            this.EditAppointmentButton.Name = "EditAppointmentButton";
            this.EditAppointmentButton.Size = new System.Drawing.Size(262, 35);
            this.EditAppointmentButton.TabIndex = 23;
            this.EditAppointmentButton.Text = "Edit Appointment";
            this.EditAppointmentButton.UseVisualStyleBackColor = true;
            // 
            // DeleteAppointmentButton
            // 
            this.DeleteAppointmentButton.Location = new System.Drawing.Point(854, 418);
            this.DeleteAppointmentButton.Name = "DeleteAppointmentButton";
            this.DeleteAppointmentButton.Size = new System.Drawing.Size(230, 35);
            this.DeleteAppointmentButton.TabIndex = 24;
            this.DeleteAppointmentButton.Text = "Delete Appointment";
            this.DeleteAppointmentButton.UseVisualStyleBackColor = true;
            // 
            // EndDateTimePicker
            // 
            this.EndDateTimePicker.CustomFormat = "MM/dd/yyyy hh:mm tt EST";
            this.EndDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.EndDateTimePicker.Location = new System.Drawing.Point(93, 336);
            this.EndDateTimePicker.Name = "EndDateTimePicker";
            this.EndDateTimePicker.ShowUpDown = true;
            this.EndDateTimePicker.Size = new System.Drawing.Size(191, 22);
            this.EndDateTimePicker.TabIndex = 11;
            this.EndDateTimePicker.ValueChanged += new System.EventHandler(this.EndDateTimePicker_ValueChanged);
            // 
            // StartDateTimePicker
            // 
            this.StartDateTimePicker.CustomFormat = "MM/dd/yyyy hh:mm tt EST";
            this.StartDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.StartDateTimePicker.Location = new System.Drawing.Point(93, 277);
            this.StartDateTimePicker.Name = "StartDateTimePicker";
            this.StartDateTimePicker.ShowUpDown = true;
            this.StartDateTimePicker.Size = new System.Drawing.Size(191, 22);
            this.StartDateTimePicker.TabIndex = 10;
            this.StartDateTimePicker.ValueChanged += new System.EventHandler(this.StartDateTimePicker_ValueChanged);
            // 
            // StartErrorLabel
            // 
            this.StartErrorLabel.AutoSize = true;
            this.StartErrorLabel.Location = new System.Drawing.Point(12, 258);
            this.StartErrorLabel.Name = "StartErrorLabel";
            this.StartErrorLabel.Size = new System.Drawing.Size(97, 16);
            this.StartErrorLabel.TabIndex = 25;
            this.StartErrorLabel.Text = "StartErrorLabel";
            this.StartErrorLabel.Visible = false;
            // 
            // EndErrorLabel
            // 
            this.EndErrorLabel.AutoSize = true;
            this.EndErrorLabel.Location = new System.Drawing.Point(13, 317);
            this.EndErrorLabel.Name = "EndErrorLabel";
            this.EndErrorLabel.Size = new System.Drawing.Size(94, 16);
            this.EndErrorLabel.TabIndex = 26;
            this.EndErrorLabel.Text = "EndErrorLabel";
            this.EndErrorLabel.Visible = false;
            // 
            // AppointmentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1096, 465);
            this.Controls.Add(this.EndErrorLabel);
            this.Controls.Add(this.StartErrorLabel);
            this.Controls.Add(this.DeleteAppointmentButton);
            this.Controls.Add(this.EditAppointmentButton);
            this.Controls.Add(this.CalendarViewButton);
            this.Controls.Add(this.AppointmentDataGridView);
            this.Controls.Add(this.TypeComboBox);
            this.Controls.Add(this.AddAppointmentButton);
            this.Controls.Add(this.UrlTextBox);
            this.Controls.Add(this.ContactTextBox);
            this.Controls.Add(this.LocationTextBox);
            this.Controls.Add(this.DescriptionTextBox);
            this.Controls.Add(this.TitleTextBox);
            this.Controls.Add(this.EndDateTimePicker);
            this.Controls.Add(this.StartDateTimePicker);
            this.Controls.Add(this.EndLabel);
            this.Controls.Add(this.StartLabel);
            this.Controls.Add(this.UrlLabel);
            this.Controls.Add(this.TypeLabel);
            this.Controls.Add(this.ContactLabel);
            this.Controls.Add(this.LocationLabel);
            this.Controls.Add(this.DescriptionLabel);
            this.Controls.Add(this.TitleLabel);
            this.Name = "AppointmentForm";
            this.Text = "Appointment Form";
            ((System.ComponentModel.ISupportInitialize)(this.AppointmentDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label TitleLabel;
        private System.Windows.Forms.Label DescriptionLabel;
        private System.Windows.Forms.Label LocationLabel;
        private System.Windows.Forms.Label ContactLabel;
        private System.Windows.Forms.Label TypeLabel;
        private System.Windows.Forms.Label UrlLabel;
        private System.Windows.Forms.Label StartLabel;
        private System.Windows.Forms.Label EndLabel;
        private System.Windows.Forms.TextBox TitleTextBox;
        private System.Windows.Forms.TextBox DescriptionTextBox;
        private System.Windows.Forms.TextBox LocationTextBox;
        private System.Windows.Forms.TextBox ContactTextBox;
        private System.Windows.Forms.TextBox UrlTextBox;
        private System.Windows.Forms.Button AddAppointmentButton;
        private System.Windows.Forms.ComboBox TypeComboBox;
        private System.Windows.Forms.DataGridView AppointmentDataGridView;
        private System.Windows.Forms.Button CalendarViewButton;
        private System.Windows.Forms.Button EditAppointmentButton;
        private System.Windows.Forms.Button DeleteAppointmentButton;
        private System.Windows.Forms.DateTimePicker EndDateTimePicker;
        private System.Windows.Forms.DateTimePicker StartDateTimePicker;
        private System.Windows.Forms.Label StartErrorLabel;
        private System.Windows.Forms.Label EndErrorLabel;
    }
}