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
    public sealed partial class InputBox : Form
    {
        private string Title;
        private string Caption;
        public string ReturnValue;

        public InputBox(string Title, string Caption)
        {
            InitializeComponent();
            this.Title = Title;
            this.Caption = Caption;
        }

        public static string Show(string Title, string Caption)
        {
            string ret;
            InputBox temp = new InputBox(Title, Caption);
            temp.ShowDialog();
            ret = temp.ReturnValue;
            temp.Dispose();
            return ret;
        }

        private void InputBox_Load(object sender, EventArgs e)
        {
            this.Text = Title;
            label1.Text = Caption;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != String.Empty)
            {
                ReturnValue = textBox1.Text;
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            else
                MessageBox.Show("Not A Valid Player Name");
        }

        private void button2_Click(object sender, EventArgs e)
        { ReturnValue = String.Empty; this.DialogResult = System.Windows.Forms.DialogResult.Cancel; }
    }
}
