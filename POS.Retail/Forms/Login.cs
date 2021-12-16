using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS.Retail.Forms
{
    public partial class Login : Form
    {
        bool mouseDown;
        private Point offset;

        public Login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void shapedPanel1_MouseDown(object sender, MouseEventArgs e)
        {
            offset.X = e.X;
            offset.Y = e.Y;
            mouseDown = true;
        }

        private void shapedPanel1_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void shapedPanel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown == true)
            {
                Point currentScreenPos = PointToScreen(e.Location);
                Location = new Point(currentScreenPos.X - offset.X, currentScreenPos.Y - offset.Y);
            }
        }

        private void label1_MouseDown(object sender, MouseEventArgs e)
        {
            shapedPanel1_MouseDown(sender, e);
        }

        private void label1_MouseUp(object sender, MouseEventArgs e)
        {
            shapedPanel1_MouseUp(sender, e);
        }

        private void label1_MouseMove(object sender, MouseEventArgs e)
        {
            shapedPanel1_MouseMove(sender, e);
        }
    }
}
