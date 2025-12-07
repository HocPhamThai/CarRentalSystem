using System;
using System.Windows.Forms;

namespace CarRentalSystem
{
    public partial class Splash : Form
    {
        public Splash()
        {
            InitializeComponent();
        }

        int startPoint = 0;

        private void timer1_Tick(object sender, EventArgs e)
        {
            startPoint += 5;
            progressBar.Value = startPoint;
            lbPercentage.Text = startPoint.ToString();

            if (progressBar.Value == 100)
            {
                progressBar.Value = 0;
                timer1.Stop();
                LoginFr login = new LoginFr();
                login.Show();
                this.Hide();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Splash_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }
    }
}
