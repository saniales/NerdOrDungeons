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
    public partial class MultiPlayerSettings : Form
    {
        public List<Panel> PanelPlayer;
        protected MainGame Game;
        string[] SelectablePlayers;
        int numberOfPlayers;
        int[] CharacterIndex;

        public MultiPlayerSettings(MainGame Parent) // chiedo il Game così gli passo i parametri
        {
            InitializeComponent();
            Game = Parent;
            PanelPlayer = new List<Panel>();
            SelectablePlayers = new string[] { "LinkBlue", "LinkRed", "LinkPurple", "LinkBlack", "Caius", "Tomonobu" };
            CharacterIndex = new int[4];
        }

        private void MultiPlayerSettings_Load(object sender, EventArgs e)
        {
            numberOfPlayers = 1;

            PanelPlayer.Add(pnlPlayer1);
            PanelPlayer.Add(pnlPlayer2);
            PanelPlayer.Add(pnlPlayer3);
            PanelPlayer.Add(pnlPlayer4);
            
            ((ComboBox)pnlPlayer1.Controls["combo1"]).SelectedIndex = 0;
            onPlayerNumberChanged();
        }

        private void onPlayerNumberChanged()
        {
            //Resetto i Player Visibili e Invisibili da Settare
            int i;
            for (i = 0; i < numberOfPlayers; i++)
            {
                PanelPlayer[i].Show();
                foreach (Control c in PanelPlayer[i].Controls)
                    if (c is TextBox) c.Text = "LinkBlue";
                    else if (c is ComboBox) ((ComboBox)c).SelectedIndex = 0;
            }
            for (i = numberOfPlayers; i < PanelPlayer.Count; i++)
            {
                PanelPlayer[i].Hide();
                foreach (Control c in PanelPlayer[i].Controls)
                    if (c is TextBox) c.Text = "";
                    else if (c is ComboBox) ((ComboBox)c).SelectedIndex = -1;
            }

        }

        private void AddPlayerNumber(object sender, EventArgs e)
        {
            if (numberOfPlayers != 4)
            {
                txtPlayerNumber.Text = (++numberOfPlayers).ToString();
                onPlayerNumberChanged();
            }
        }

        private void SubstractPlayerNumber(object sender, EventArgs e)
        {
            if (numberOfPlayers != 1)
            {
                txtPlayerNumber.Text = (--numberOfPlayers).ToString();
                onPlayerNumberChanged();
            }
        }

        private void ScorriListaCharactersSinistra(object sender, EventArgs e)
        {
            int CurrentIndex = -1, i = 0;
            TextBox t =(TextBox)((Button)sender).Parent.Controls[4];
            foreach (string s in SelectablePlayers)
                if (s == t.Text)
                    CurrentIndex = i;
                else i++;
            if (CurrentIndex > 0) // = 0 -> il primo; oppure = -1 -> errore; non va bene
                t.Text = SelectablePlayers[--CurrentIndex];
        }

        private void ScorriListaCharactersDestra(object sender, EventArgs e)
        {
            int CurrentIndex = SelectablePlayers.Length, i = 0;
            TextBox t = (TextBox)((Button)sender).Parent.Controls[4];
            foreach (string s in SelectablePlayers)
                if (s == t.Text)
                    CurrentIndex = i;
                else i++;
            if (CurrentIndex < (SelectablePlayers.Length - 1))
                t.Text = SelectablePlayers[++CurrentIndex];
        }

        private void button1_Click(object sender, EventArgs e)
        {               
            this.Game.ArenaMultiplayer = new Arena(this.Game, @".\Content\Arena",
                                      txt1.Text, txt2.Text, txt3.Text, txt4.Text,
                                      (Controller)combo1.SelectedIndex,
                                      (Controller)combo2.SelectedIndex, 
                                      (Controller)combo3.SelectedIndex, 
                                      (Controller)combo4.SelectedIndex);
            this.DialogResult = DialogResult.OK;
        }
    }
}
