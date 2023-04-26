namespace Eksdom.App.Windows
{
    partial class frmMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            btnPing = new Button();
            lblPing = new Label();
            btnArea = new Button();
            txtArea = new TextBox();
            notifyIcon = new NotifyIcon(components);
            button1 = new Button();
            SuspendLayout();
            // 
            // btnPing
            // 
            btnPing.Location = new Point(12, 12);
            btnPing.Name = "btnPing";
            btnPing.Size = new Size(98, 50);
            btnPing.TabIndex = 0;
            btnPing.Text = "Ping";
            btnPing.UseVisualStyleBackColor = true;
            btnPing.Click += btnPing_Click;
            // 
            // lblPing
            // 
            lblPing.AutoSize = true;
            lblPing.Location = new Point(116, 12);
            lblPing.Name = "lblPing";
            lblPing.Size = new Size(12, 15);
            lblPing.TabIndex = 1;
            lblPing.Text = "-";
            // 
            // btnArea
            // 
            btnArea.Location = new Point(12, 68);
            btnArea.Name = "btnArea";
            btnArea.Size = new Size(98, 48);
            btnArea.TabIndex = 2;
            btnArea.Text = "Area";
            btnArea.UseVisualStyleBackColor = true;
            btnArea.Click += btnArea_Click;
            // 
            // txtArea
            // 
            txtArea.Location = new Point(12, 122);
            txtArea.Multiline = true;
            txtArea.Name = "txtArea";
            txtArea.ScrollBars = ScrollBars.Both;
            txtArea.Size = new Size(449, 174);
            txtArea.TabIndex = 3;
            // 
            // notifyIcon
            // 
            notifyIcon.Icon = (Icon)resources.GetObject("notifyIcon.Icon");
            notifyIcon.Text = "Eksdom Utility";
            notifyIcon.Visible = true;
            // 
            // button1
            // 
            button1.Location = new Point(376, 47);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 4;
            button1.Text = "button1";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // frmMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(473, 308);
            Controls.Add(button1);
            Controls.Add(txtArea);
            Controls.Add(btnArea);
            Controls.Add(lblPing);
            Controls.Add(btnPing);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "frmMain";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Eksdom - Anti-Loadshedding Tool";
            Load += frmMain_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnPing;
        private Label lblPing;
        private Button btnArea;
        private TextBox txtArea;
        private NotifyIcon notifyIcon;
        private Button button1;
    }
}