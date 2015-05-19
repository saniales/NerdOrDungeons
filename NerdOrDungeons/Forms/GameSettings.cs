using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace NerdOrDungeons
{
    public partial class GameSettings : Form
    {
        MainGame Parent;

        public GameSettings(MainGame Parent)
        { InitializeComponent(); this.Parent = Parent; }

        private void BtnOK_Click(object sender, EventArgs e)
        { 
            //1. Salvo Le Impostazioni
            /*   TODO: Impostazioni!!!!! */
            //2. Uscire con Status "OK"
            SaveSettings();
            this.DialogResult = DialogResult.OK; 
        }    

        private void button1_Click(object sender, EventArgs e)
        { this.DialogResult = DialogResult.Cancel; }

        private void GameSettings_Load(object sender, EventArgs e)
        {
            //Carico Le Impostazioni Attuali
            /* TODO: Carica Impostazioni */
            LoadSettings();
        }

        private void SaveSettings()
        {
            StreamWriter w = new StreamWriter(@".\Content\GameSettings.cfg", false, Encoding.ASCII);
            string WriteLine = String.Empty;
            if (rdbGamepad.Checked)
            {
                WriteLine += "True";
                this.Parent.Joystick = true;
            }
            else
            {
                WriteLine += "False";
                this.Parent.Joystick = false;
            }
            WriteLine += '/';
            if (RdbEasy.Checked)
            {
                WriteLine += "0,5";
                this.Parent.DifficultyMultiplier = 0.5f;
            }
            else if (RdbMedium.Checked)
            {
                WriteLine += "1,0";
                this.Parent.DifficultyMultiplier = 1.0f;
            }
            else if (RdbHard.Checked)
            {
                WriteLine += "1,5";
                this.Parent.DifficultyMultiplier = 1.5f;
            }
            else if (RdbVeryHard.Checked)
            {
                WriteLine += "2,0";
                this.Parent.DifficultyMultiplier = 2.0f;
            }
            else if (rdbMasterNinja.Checked)
            {
                WriteLine += "3,5";
                this.Parent.DifficultyMultiplier = 3.5f;
            }
            w.WriteLine(WriteLine);
            w.Close();             
        }

        private void LoadSettings()
        {
            bool Joystick;
            float DifficultyMultiplier;
            StreamReader r = new StreamReader(@".\Content\GameSettings.cfg");
            string[] temp = r.ReadLine().Split('/');
            Joystick = Convert.ToBoolean(temp[0]);
            DifficultyMultiplier = Convert.ToSingle(temp[1]);
            r.Close();

            if (Joystick)
                rdbGamepad.Checked = true;
            else
                rdbKeyboard.Checked = true;

            if (DifficultyMultiplier == 0.5f)
                RdbEasy.Checked = true;
            else if (DifficultyMultiplier == 1.0f)
                RdbMedium.Checked = true;
            else if (DifficultyMultiplier == 1.5f)
                RdbHard.Checked = true;
            else if (DifficultyMultiplier == 2.0f)
                RdbVeryHard.Checked = true;
            else /* if(DifficultyMultiplier == 3.5f) */
                rdbMasterNinja.Checked = true;
        }
    }
}
