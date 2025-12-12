namespace CarRentalSystem
{
    partial class MainFr
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
            this.label1 = new System.Windows.Forms.Label();
            this.btnCar = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lbExit = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnCustomer = new System.Windows.Forms.Button();
            this.btnBookings = new System.Windows.Forms.Button();
            this.btnSchedules = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnUser = new System.Windows.Forms.Button();
            this.btnDashBoard = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(303, 5);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(288, 45);
            this.label1.TabIndex = 12;
            this.label1.Text = "Car Rental System";
            // 
            // btnCar
            // 
            this.btnCar.BackColor = System.Drawing.Color.Red;
            this.btnCar.Font = new System.Drawing.Font("Segoe UI Semibold", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCar.ForeColor = System.Drawing.Color.White;
            this.btnCar.Location = new System.Drawing.Point(154, 134);
            this.btnCar.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnCar.Name = "btnCar";
            this.btnCar.Size = new System.Drawing.Size(118, 39);
            this.btnCar.TabIndex = 14;
            this.btnCar.Text = "CARS";
            this.btnCar.UseVisualStyleBackColor = false;
            this.btnCar.Click += new System.EventHandler(this.btnCar_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(345, 56);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(171, 37);
            this.label5.TabIndex = 13;
            this.label5.Text = "Main Form";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Red;
            this.panel1.Controls.Add(this.lbExit);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(902, 112);
            this.panel1.TabIndex = 15;
            // 
            // lbExit
            // 
            this.lbExit.AutoSize = true;
            this.lbExit.BackColor = System.Drawing.Color.Red;
            this.lbExit.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbExit.ForeColor = System.Drawing.Color.White;
            this.lbExit.Location = new System.Drawing.Point(866, 5);
            this.lbExit.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbExit.Name = "lbExit";
            this.lbExit.Size = new System.Drawing.Size(28, 32);
            this.lbExit.TabIndex = 15;
            this.lbExit.Text = "X";
            this.lbExit.Click += new System.EventHandler(this.lbExit_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Red;
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 510);
            this.panel2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(902, 37);
            this.panel2.TabIndex = 16;
            // 
            // btnCustomer
            // 
            this.btnCustomer.BackColor = System.Drawing.Color.Red;
            this.btnCustomer.Font = new System.Drawing.Font("Segoe UI Semibold", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCustomer.ForeColor = System.Drawing.Color.White;
            this.btnCustomer.Location = new System.Drawing.Point(277, 134);
            this.btnCustomer.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnCustomer.Name = "btnCustomer";
            this.btnCustomer.Size = new System.Drawing.Size(132, 39);
            this.btnCustomer.TabIndex = 17;
            this.btnCustomer.Text = "CUSTOMERS";
            this.btnCustomer.UseVisualStyleBackColor = false;
            this.btnCustomer.Click += new System.EventHandler(this.btnCustomer_Click);
            // 
            // btnBookings
            // 
            this.btnBookings.BackColor = System.Drawing.Color.Red;
            this.btnBookings.Font = new System.Drawing.Font("Segoe UI Semibold", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBookings.ForeColor = System.Drawing.Color.White;
            this.btnBookings.Location = new System.Drawing.Point(413, 134);
            this.btnBookings.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnBookings.Name = "btnBookings";
            this.btnBookings.Size = new System.Drawing.Size(150, 39);
            this.btnBookings.TabIndex = 18;
            this.btnBookings.Text = "BOOKINGS";
            this.btnBookings.UseVisualStyleBackColor = false;
            this.btnBookings.Click += new System.EventHandler(this.btnBookings_Click);
            // 
            // btnSchedules
            // 
            this.btnSchedules.BackColor = System.Drawing.Color.Red;
            this.btnSchedules.Font = new System.Drawing.Font("Segoe UI Semibold", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSchedules.ForeColor = System.Drawing.Color.White;
            this.btnSchedules.Location = new System.Drawing.Point(567, 134);
            this.btnSchedules.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnSchedules.Name = "btnSchedules";
            this.btnSchedules.Size = new System.Drawing.Size(130, 39);
            this.btnSchedules.TabIndex = 19;
            this.btnSchedules.Text = "SCHEDULES";
            this.btnSchedules.UseVisualStyleBackColor = false;
            this.btnSchedules.Click += new System.EventHandler(this.btnSchedules_Click);
            // 
            // btnLogout
            // 
            this.btnLogout.BackColor = System.Drawing.Color.Red;
            this.btnLogout.Font = new System.Drawing.Font("Segoe UI Semibold", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLogout.ForeColor = System.Drawing.Color.White;
            this.btnLogout.Location = new System.Drawing.Point(392, 466);
            this.btnLogout.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(118, 39);
            this.btnLogout.TabIndex = 21;
            this.btnLogout.Text = "Logout";
            this.btnLogout.UseVisualStyleBackColor = false;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::CarRentalSystem.Properties.Resources.AUDI_2cho;
            this.pictureBox1.Location = new System.Drawing.Point(272, 197);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(358, 272);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 11;
            this.pictureBox1.TabStop = false;
            // 
            // btnUser
            // 
            this.btnUser.BackColor = System.Drawing.Color.Red;
            this.btnUser.Font = new System.Drawing.Font("Segoe UI Semibold", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUser.ForeColor = System.Drawing.Color.White;
            this.btnUser.Location = new System.Drawing.Point(32, 134);
            this.btnUser.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnUser.Name = "btnUser";
            this.btnUser.Size = new System.Drawing.Size(118, 39);
            this.btnUser.TabIndex = 22;
            this.btnUser.Text = "USERS";
            this.btnUser.UseVisualStyleBackColor = false;
            this.btnUser.Click += new System.EventHandler(this.btnUser_Click);
            // 
            // btnDashBoard
            // 
            this.btnDashBoard.BackColor = System.Drawing.Color.Red;
            this.btnDashBoard.Font = new System.Drawing.Font("Segoe UI Semibold", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDashBoard.ForeColor = System.Drawing.Color.White;
            this.btnDashBoard.Location = new System.Drawing.Point(702, 134);
            this.btnDashBoard.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnDashBoard.Name = "btnDashBoard";
            this.btnDashBoard.Size = new System.Drawing.Size(172, 39);
            this.btnDashBoard.TabIndex = 23;
            this.btnDashBoard.Text = "DASHBOARD";
            this.btnDashBoard.UseVisualStyleBackColor = false;
            this.btnDashBoard.Click += new System.EventHandler(this.btnDashBoard_Click);
            // 
            // MainFr
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(902, 547);
            this.Controls.Add(this.btnDashBoard);
            this.Controls.Add(this.btnUser);
            this.Controls.Add(this.btnLogout);
            this.Controls.Add(this.btnSchedules);
            this.Controls.Add(this.btnBookings);
            this.Controls.Add(this.btnCustomer);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btnCar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "MainFr";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MainForm";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnCar;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnCustomer;
        private System.Windows.Forms.Button btnBookings;
        private System.Windows.Forms.Button btnSchedules;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Label lbExit;
        private System.Windows.Forms.Button btnUser;
        private System.Windows.Forms.Button btnDashBoard;
    }
}