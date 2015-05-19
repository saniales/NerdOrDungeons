using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NerdOrDungeons
{
    public partial class SelectLevelForm : Form
    {
        public SelectLevelForm()
        { InitializeComponent(); }

        private void button1_Click(object sender, EventArgs e)
        { this.DialogResult = DialogResult.Cancel; }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            MainGame.RiferimentoGlobaleAlGioco.Level = new Scenario(MainGame.RiferimentoGlobaleAlGioco, @".\Content\Dungeons\", "Level" + Convert.ToInt16(maskedTextBox1.Text).ToString(), false);
            this.DialogResult = DialogResult.OK;
        }
    }
}
