namespace Eksdom.App.Windows
{
    partial class frmConfig
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmConfig));
            txtLicenceKey = new TextBox();
            label1 = new Label();
            label2 = new Label();
            linkSubscribe = new LinkLabel();
            btnSave = new Button();
            btnCancel = new Button();
            btnTestApiKey = new Button();
            groupBox1 = new GroupBox();
            radioCapeTown = new RadioButton();
            radioEskom = new RadioButton();
            txtSelectedArea = new TextBox();
            label3 = new Label();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // txtLicenceKey
            // 
            txtLicenceKey.Location = new Point(130, 97);
            txtLicenceKey.MaxLength = 100;
            txtLicenceKey.Name = "txtLicenceKey";
            txtLicenceKey.PlaceholderText = "Paste Licence Key from Eskom Se Push (Ctrl+V)";
            txtLicenceKey.Size = new Size(286, 23);
            txtLicenceKey.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(45, 100);
            label1.Name = "label1";
            label1.Size = new Size(72, 15);
            label1.TabIndex = 1;
            label1.Text = "Licence Key ";
            // 
            // label2
            // 
            label2.Location = new Point(42, 31);
            label2.Name = "label2";
            label2.Size = new Size(439, 47);
            label2.TabIndex = 2;
            label2.Text = "NOTE: In order to use this software you first need to obtain your API key from EskomSePush. You may claim your free Licence Key ";
            // 
            // linkSubscribe
            // 
            linkSubscribe.AutoSize = true;
            linkSubscribe.Location = new Point(318, 46);
            linkSubscribe.Name = "linkSubscribe";
            linkSubscribe.Size = new Size(124, 15);
            linkSubscribe.TabIndex = 3;
            linkSubscribe.TabStop = true;
            linkSubscribe.Text = "with this Subscription.";
            // 
            // btnSave
            // 
            btnSave.Location = new Point(389, 251);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(92, 43);
            btnSave.TabIndex = 4;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(314, 251);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(69, 43);
            btnCancel.TabIndex = 5;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnTestApiKey
            // 
            btnTestApiKey.Location = new Point(422, 83);
            btnTestApiKey.Name = "btnTestApiKey";
            btnTestApiKey.Size = new Size(59, 51);
            btnTestApiKey.TabIndex = 6;
            btnTestApiKey.Text = "Test Key";
            btnTestApiKey.UseVisualStyleBackColor = true;
            btnTestApiKey.Click += btnTestApiKey_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(radioCapeTown);
            groupBox1.Controls.Add(radioEskom);
            groupBox1.Controls.Add(txtSelectedArea);
            groupBox1.Controls.Add(label3);
            groupBox1.Location = new Point(45, 135);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(436, 97);
            groupBox1.TabIndex = 8;
            groupBox1.TabStop = false;
            groupBox1.Text = "Municipality";
            // 
            // radioCapeTown
            // 
            radioCapeTown.AutoSize = true;
            radioCapeTown.Location = new Point(221, 66);
            radioCapeTown.Name = "radioCapeTown";
            radioCapeTown.Size = new Size(83, 19);
            radioCapeTown.TabIndex = 3;
            radioCapeTown.TabStop = true;
            radioCapeTown.Text = "Cape Town";
            radioCapeTown.UseVisualStyleBackColor = true;
            // 
            // radioEskom
            // 
            radioEskom.AutoSize = true;
            radioEskom.Location = new Point(39, 66);
            radioEskom.Name = "radioEskom";
            radioEskom.Size = new Size(116, 19);
            radioEskom.TabIndex = 2;
            radioEskom.TabStop = true;
            radioEskom.Text = "National (Eskom)";
            radioEskom.UseVisualStyleBackColor = true;
            // 
            // txtSelectedArea
            // 
            txtSelectedArea.BackColor = SystemColors.ButtonFace;
            txtSelectedArea.Enabled = false;
            txtSelectedArea.Location = new Point(69, 26);
            txtSelectedArea.MaxLength = 500;
            txtSelectedArea.Name = "txtSelectedArea";
            txtSelectedArea.Size = new Size(361, 23);
            txtSelectedArea.TabIndex = 1;
            // 
            // label3
            // 
            label3.Location = new Point(15, 29);
            label3.Name = "label3";
            label3.Size = new Size(57, 20);
            label3.TabIndex = 0;
            label3.Text = "Area";
            // 
            // frmConfig
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(536, 315);
            Controls.Add(groupBox1);
            Controls.Add(btnTestApiKey);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Controls.Add(linkSubscribe);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(txtLicenceKey);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmConfig";
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Eksdom Configuration";
            Load += frmConfig_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtLicenceKey;
        private Label label1;
        private Label label2;
        private LinkLabel linkSubscribe;
        private Button btnSave;
        private Button btnCancel;
        private Button btnTestApiKey;
        private GroupBox groupBox1;
        private RadioButton radioCapeTown;
        private RadioButton radioEskom;
        private TextBox txtSelectedArea;
        private Label label3;
    }
}