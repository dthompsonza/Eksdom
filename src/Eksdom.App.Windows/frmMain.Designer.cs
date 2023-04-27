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
            notifyIcon = new NotifyIcon(components);
            timerMain = new System.Windows.Forms.Timer(components);
            statusStripMain = new StatusStrip();
            lblApiAllowance = new ToolStripStatusLabel();
            lblServiceInfo = new ToolStripStatusLabel();
            menuStrip1 = new MenuStrip();
            menuToolStripMenuItem = new ToolStripMenuItem();
            aPIKeyToolStripMenuItem = new ToolStripMenuItem();
            label1 = new Label();
            lblNextLoadshedding = new Label();
            statusStripMain.SuspendLayout();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // notifyIcon
            // 
            notifyIcon.Icon = (Icon)resources.GetObject("notifyIcon.Icon");
            notifyIcon.Text = "Eksdom Utility";
            notifyIcon.Visible = true;
            // 
            // timerMain
            // 
            timerMain.Interval = 5000;
            timerMain.Tick += timerMain_Tick;
            // 
            // statusStripMain
            // 
            statusStripMain.Items.AddRange(new ToolStripItem[] { lblApiAllowance, lblServiceInfo });
            statusStripMain.Location = new Point(0, 286);
            statusStripMain.Name = "statusStripMain";
            statusStripMain.Size = new Size(473, 22);
            statusStripMain.TabIndex = 0;
            statusStripMain.Text = "statusStrip1";
            // 
            // lblApiAllowance
            // 
            lblApiAllowance.DisplayStyle = ToolStripItemDisplayStyle.Text;
            lblApiAllowance.Name = "lblApiAllowance";
            lblApiAllowance.Size = new Size(229, 17);
            lblApiAllowance.Spring = true;
            lblApiAllowance.Text = "-";
            // 
            // lblServiceInfo
            // 
            lblServiceInfo.Name = "lblServiceInfo";
            lblServiceInfo.Size = new Size(229, 17);
            lblServiceInfo.Spring = true;
            lblServiceInfo.Text = "...";
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { menuToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(473, 24);
            menuStrip1.TabIndex = 1;
            menuStrip1.Text = "menuStrip1";
            // 
            // menuToolStripMenuItem
            // 
            menuToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { aPIKeyToolStripMenuItem });
            menuToolStripMenuItem.Name = "menuToolStripMenuItem";
            menuToolStripMenuItem.Size = new Size(50, 20);
            menuToolStripMenuItem.Text = "Menu";
            // 
            // aPIKeyToolStripMenuItem
            // 
            aPIKeyToolStripMenuItem.Name = "aPIKeyToolStripMenuItem";
            aPIKeyToolStripMenuItem.Size = new Size(122, 22);
            aPIKeyToolStripMenuItem.Text = "Config ...";
            aPIKeyToolStripMenuItem.Click += aPIKeyToolStripMenuItem_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(29, 57);
            label1.Name = "label1";
            label1.Size = new Size(107, 15);
            label1.TabIndex = 2;
            label1.Text = "Next loadshedding";
            // 
            // lblNextLoadshedding
            // 
            lblNextLoadshedding.BorderStyle = BorderStyle.FixedSingle;
            lblNextLoadshedding.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            lblNextLoadshedding.Location = new Point(54, 90);
            lblNextLoadshedding.Name = "lblNextLoadshedding";
            lblNextLoadshedding.Size = new Size(359, 102);
            lblNextLoadshedding.TabIndex = 3;
            lblNextLoadshedding.Text = "--";
            // 
            // frmMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(473, 308);
            Controls.Add(lblNextLoadshedding);
            Controls.Add(label1);
            Controls.Add(statusStripMain);
            Controls.Add(menuStrip1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip1;
            MaximizeBox = false;
            Name = "frmMain";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Eksdom";
            Load += frmMain_Load;
            statusStripMain.ResumeLayout(false);
            statusStripMain.PerformLayout();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private NotifyIcon notifyIcon;
        private System.Windows.Forms.Timer timerMain;
        private StatusStrip statusStripMain;
        private ToolStripStatusLabel lblApiAllowance;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem menuToolStripMenuItem;
        private ToolStripMenuItem aPIKeyToolStripMenuItem;
        private ToolStripStatusLabel lblServiceInfo;
        private Label label1;
        private Label lblNextLoadshedding;
    }
}