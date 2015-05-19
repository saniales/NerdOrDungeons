namespace NerdOrDungeons
{
    partial class GameSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GameSettings));
            this.BtnOK = new System.Windows.Forms.Button();
            this.rdbKeyboard = new System.Windows.Forms.RadioButton();
            this.rdbGamepad = new System.Windows.Forms.RadioButton();
            this.button1 = new System.Windows.Forms.Button();
            this.RdbEasy = new System.Windows.Forms.RadioButton();
            this.RdbMedium = new System.Windows.Forms.RadioButton();
            this.RdbHard = new System.Windows.Forms.RadioButton();
            this.RdbVeryHard = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.rdbMasterNinja = new System.Windows.Forms.RadioButton();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // BtnOK
            // 
            this.BtnOK.Location = new System.Drawing.Point(12, 105);
            this.BtnOK.Name = "BtnOK";
            this.BtnOK.Size = new System.Drawing.Size(204, 23);
            this.BtnOK.TabIndex = 0;
            this.BtnOK.Text = "SAVE";
            this.BtnOK.UseVisualStyleBackColor = true;
            this.BtnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // rdbKeyboard
            // 
            this.rdbKeyboard.AutoSize = true;
            this.rdbKeyboard.Location = new System.Drawing.Point(35, 0);
            this.rdbKeyboard.Name = "rdbKeyboard";
            this.rdbKeyboard.Size = new System.Drawing.Size(92, 17);
            this.rdbKeyboard.TabIndex = 1;
            this.rdbKeyboard.TabStop = true;
            this.rdbKeyboard.Text = "Use Keyboard";
            this.rdbKeyboard.UseVisualStyleBackColor = true;
            // 
            // rdbGamepad
            // 
            this.rdbGamepad.AutoSize = true;
            this.rdbGamepad.Location = new System.Drawing.Point(170, 0);
            this.rdbGamepad.Name = "rdbGamepad";
            this.rdbGamepad.Size = new System.Drawing.Size(93, 17);
            this.rdbGamepad.TabIndex = 2;
            this.rdbGamepad.TabStop = true;
            this.rdbGamepad.Text = "Use Gamepad";
            this.rdbGamepad.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(271, 105);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(193, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "CANCEL";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // RdbEasy
            // 
            this.RdbEasy.AutoSize = true;
            this.RdbEasy.Location = new System.Drawing.Point(0, 0);
            this.RdbEasy.Name = "RdbEasy";
            this.RdbEasy.Size = new System.Drawing.Size(48, 17);
            this.RdbEasy.TabIndex = 4;
            this.RdbEasy.TabStop = true;
            this.RdbEasy.Text = "Easy";
            this.RdbEasy.UseVisualStyleBackColor = true;
            // 
            // RdbMedium
            // 
            this.RdbMedium.AutoSize = true;
            this.RdbMedium.Location = new System.Drawing.Point(77, 0);
            this.RdbMedium.Name = "RdbMedium";
            this.RdbMedium.Size = new System.Drawing.Size(62, 17);
            this.RdbMedium.TabIndex = 5;
            this.RdbMedium.TabStop = true;
            this.RdbMedium.Text = "Medium";
            this.RdbMedium.UseVisualStyleBackColor = true;
            // 
            // RdbHard
            // 
            this.RdbHard.AutoSize = true;
            this.RdbHard.Location = new System.Drawing.Point(172, 0);
            this.RdbHard.Name = "RdbHard";
            this.RdbHard.Size = new System.Drawing.Size(48, 17);
            this.RdbHard.TabIndex = 6;
            this.RdbHard.TabStop = true;
            this.RdbHard.Text = "Hard";
            this.RdbHard.UseVisualStyleBackColor = true;
            // 
            // RdbVeryHard
            // 
            this.RdbVeryHard.AutoSize = true;
            this.RdbVeryHard.Location = new System.Drawing.Point(256, 0);
            this.RdbVeryHard.Name = "RdbVeryHard";
            this.RdbVeryHard.Size = new System.Drawing.Size(72, 17);
            this.RdbVeryHard.TabIndex = 7;
            this.RdbVeryHard.TabStop = true;
            this.RdbVeryHard.Text = "Very Hard";
            this.RdbVeryHard.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(223, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Difficulty :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(225, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Controls :";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rdbGamepad);
            this.panel1.Controls.Add(this.rdbKeyboard);
            this.panel1.Location = new System.Drawing.Point(101, 25);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(282, 17);
            this.panel1.TabIndex = 10;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.rdbMasterNinja);
            this.panel2.Controls.Add(this.RdbEasy);
            this.panel2.Controls.Add(this.RdbMedium);
            this.panel2.Controls.Add(this.RdbHard);
            this.panel2.Controls.Add(this.RdbVeryHard);
            this.panel2.Location = new System.Drawing.Point(13, 72);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(451, 17);
            this.panel2.TabIndex = 11;
            // 
            // rdbMasterNinja
            // 
            this.rdbMasterNinja.AutoSize = true;
            this.rdbMasterNinja.Location = new System.Drawing.Point(347, 0);
            this.rdbMasterNinja.Name = "rdbMasterNinja";
            this.rdbMasterNinja.Size = new System.Drawing.Size(104, 17);
            this.rdbMasterNinja.TabIndex = 8;
            this.rdbMasterNinja.TabStop = true;
            this.rdbMasterNinja.Text = "MASTER NINJA";
            this.rdbMasterNinja.UseVisualStyleBackColor = true;
            // 
            // GameSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(476, 140);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.BtnOK);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "GameSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "NERD OR DUNGEONS : THE GAME SETTINGS";
            this.Load += new System.EventHandler(this.GameSettings_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnOK;
        private System.Windows.Forms.RadioButton rdbKeyboard;
        private System.Windows.Forms.RadioButton rdbGamepad;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RadioButton RdbEasy;
        private System.Windows.Forms.RadioButton RdbMedium;
        private System.Windows.Forms.RadioButton RdbHard;
        private System.Windows.Forms.RadioButton RdbVeryHard;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RadioButton rdbMasterNinja;
    }
}