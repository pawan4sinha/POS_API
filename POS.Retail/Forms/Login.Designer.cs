
namespace POS.Retail.Forms
{
    partial class Login
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
            this.lblUsername = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtUsername = new POS.Retail.Utilities.RoundedCornerTextbox();
            this.btnLogin = new POS.Retail.Utilities.CustomButton();
            this.shapedPanel1 = new POS.Retail.Utilities.ShapedPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.btnExit = new POS.Retail.Utilities.CustomButton();
            this.txtPassword = new POS.Retail.Utilities.RoundedCornerTextbox();
            this.shapedPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblUsername.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblUsername.Location = new System.Drawing.Point(61, 93);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(75, 19);
            this.lblUsername.TabIndex = 0;
            this.lblUsername.Text = "Username";
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblPassword.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblPassword.Location = new System.Drawing.Point(61, 164);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(71, 19);
            this.lblPassword.TabIndex = 1;
            this.lblPassword.Text = "Password";
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(61, 115);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.NewText = "";
            this.txtUsername.OriText = "";
            this.txtUsername.Size = new System.Drawing.Size(225, 27);
            this.txtUsername.TabIndex = 1;
            this.txtUsername.Texthaschanged = false;
            // 
            // btnLogin
            // 
            this.btnLogin.ButtonBoardColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(201)))), ((int)(((byte)(39)))));
            this.btnLogin.ButtonBoardColorOnClick = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(201)))), ((int)(((byte)(39)))));
            this.btnLogin.CornerRadius = 10;
            this.btnLogin.DisableButtonBoardColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(227)))), ((int)(((byte)(232)))));
            this.btnLogin.DisableFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(233)))), ((int)(((byte)(239)))));
            this.btnLogin.DisableShadeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(175)))), ((int)(((byte)(19)))));
            this.btnLogin.EnableFocus = true;
            this.btnLogin.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(211)))), ((int)(((byte)(51)))));
            this.btnLogin.FillColorOnClick = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(211)))), ((int)(((byte)(51)))));
            this.btnLogin.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnLogin.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnLogin.IsChangeColorOnClick = false;
            this.btnLogin.Location = new System.Drawing.Point(61, 244);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.ShadeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(175)))), ((int)(((byte)(19)))));
            this.btnLogin.ShadeColorOnClick = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(175)))), ((int)(((byte)(19)))));
            this.btnLogin.Size = new System.Drawing.Size(109, 28);
            this.btnLogin.TabIndex = 3;
            this.btnLogin.Text = "Login";
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // shapedPanel1
            // 
            this.shapedPanel1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(175)))), ((int)(((byte)(19)))));
            this.shapedPanel1.Controls.Add(this.label1);
            this.shapedPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.shapedPanel1.Edge = 50;
            this.shapedPanel1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(175)))), ((int)(((byte)(19)))));
            this.shapedPanel1.Location = new System.Drawing.Point(0, 0);
            this.shapedPanel1.Name = "shapedPanel1";
            this.shapedPanel1.Radius = 8;
            this.shapedPanel1.ShadeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(175)))), ((int)(((byte)(19)))));
            this.shapedPanel1.Size = new System.Drawing.Size(338, 78);
            this.shapedPanel1.TabIndex = 12;
            this.shapedPanel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.shapedPanel1_MouseDown);
            this.shapedPanel1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.shapedPanel1_MouseMove);
            this.shapedPanel1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.shapedPanel1_MouseUp);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Calibri", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(114, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 36);
            this.label1.TabIndex = 0;
            this.label1.Text = "SIGN IN";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label1_MouseDown);
            this.label1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.label1_MouseMove);
            this.label1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.label1_MouseUp);
            // 
            // btnExit
            // 
            this.btnExit.ButtonBoardColor = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(201)))), ((int)(((byte)(204)))));
            this.btnExit.ButtonBoardColorOnClick = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(201)))), ((int)(((byte)(39)))));
            this.btnExit.CornerRadius = 10;
            this.btnExit.DisableButtonBoardColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(227)))), ((int)(((byte)(232)))));
            this.btnExit.DisableFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(233)))), ((int)(((byte)(239)))));
            this.btnExit.DisableShadeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(175)))), ((int)(((byte)(19)))));
            this.btnExit.EnableFocus = true;
            this.btnExit.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.btnExit.FillColorOnClick = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(211)))), ((int)(((byte)(51)))));
            this.btnExit.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnExit.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnExit.IsChangeColorOnClick = false;
            this.btnExit.Location = new System.Drawing.Point(177, 244);
            this.btnExit.Name = "btnExit";
            this.btnExit.ShadeColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(228)))), ((int)(((byte)(229)))));
            this.btnExit.ShadeColorOnClick = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(175)))), ((int)(((byte)(19)))));
            this.btnExit.Size = new System.Drawing.Size(109, 28);
            this.btnExit.TabIndex = 4;
            this.btnExit.Text = "Exit";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(61, 186);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.NewText = "";
            this.txtPassword.OriText = "";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(225, 27);
            this.txtPassword.TabIndex = 2;
            this.txtPassword.Texthaschanged = false;
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(338, 311);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.shapedPanel1);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.lblUsername);
            this.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Login";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Login";
            this.shapedPanel1.ResumeLayout(false);
            this.shapedPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.Label lblPassword;
        private Utilities.RoundedCornerTextbox txtUsername;
        private Utilities.CustomButton btnLogin;
        private Utilities.ShapedPanel shapedPanel1;
        private System.Windows.Forms.Label label1;
        private Utilities.CustomButton btnExit;
        private Utilities.RoundedCornerTextbox txtPassword;
    }
}