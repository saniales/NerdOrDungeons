using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace NerdOrDungeons
{
    /**                                                              **
     ******************************************************************
     **                                                              **
     ** Menu -> Eredita da Scenario :                                **
     ** Classe Di Gestione Del Menù Principale, è Un Particolare     **
     ** Di Scenario Senza Nemici Dove Ci Sono Solo Player e 3        **
     ** Portali (PortaleCarica, PortaleNuovaPartita e PortaleEsci)   **
     ** e Alcune ShortCut a Varie Form di Aiuto e Diverse Modalità   **
     **                                                              **
     ******************************************************************
     **                                                              **/

    public sealed class Menu : Scenario
    {
        #region Dichiarazione Variabili

        OggettoInanimato PortaleCarica, PortaleNuovaPartita, PortaleEsci;
        System.Windows.Forms.Form FormRichiamata;    

        #endregion

        #region Costruttore

        public Menu(Game Game, string DirectoryPath) : base(Game, DirectoryPath, "", false) { FormRichiamata = null; }
                   
        #endregion
       
        #region Metodi Per Lo Scenario del Menu

        protected sealed override void CaricaScenarioDaLevelMap()
        {
            StreamReader sr;
            Vector2 tempPosizione;
            string tempRiga;
            char CarattereJolly;

            sr                     = new StreamReader(DirectoryPath + @"\Level.map");
            this.Game.SpriteBatch  = new SpriteBatch(this.Game.GraphicsDevice);
            IstanzaUnicaPerTerreno = new Sprite(this.Game, DirectoryPath + @"\terreno.png", Vector2.Zero);
            LevelTrack             = SoundEngine.Tracks["Menu"];
            tempPosizione          = Vector2.Zero;

            GC.Collect();

            this.DialoghiPreLivello = new Sequenza(this.Game,"", null);

            if (this.Game.Level != null)
            {
                this.Game.Level.LevelTrack.Stop();
                this.Game.Level = null;
            }

            if (this.LevelTrack != null)
                this.LevelTrack.Stop();    

            while (!sr.EndOfStream)
            {
                tempRiga = sr.ReadLine();
                for (int i = 0; i < tempRiga.Length; i++)
                {
                    CarattereJolly = tempRiga[i];
                    switch (CarattereJolly) //a seconda del carattere faccio un terreno/muro/personaggio/nemico
                    {
                        case 'P':
                            Player.Posizione = tempPosizione;
                            break;
                        case 'L':
                            PortaleCarica = new OggettoInanimato(this.Game, tempPosizione);
                            break;
                        case 'E':
                            PortaleEsci = new OggettoInanimato(this.Game, tempPosizione);
                            break;
                        case '@':
                            PortaleNuovaPartita = new OggettoInanimato(this.Game, tempPosizione);
                            break;
                        case '#':
                            Muri.Add(new OggettoInanimato(this.Game, tempPosizione));
                            break;
                    }
                    tempPosizione.X += 16;      //16x16 pixel = dimensione dello sprite del terreno
                }
                tempPosizione.X = 0;
                tempPosizione.Y += 16;
            }
            sr.Close();
        }

        private void ControllaCollisioni(Personaggio agent)
        {
            if (agent.CheckCollision(PortaleCarica) == true)
            {
                string temp;
                StreamReader sr = new StreamReader(Directory.GetCurrentDirectory() + @"/Content/Saves.cfg");
                temp = sr.ReadLine();
                sr.Close();
                this.CaricaLivello(temp, true);
            }
            if (agent.CheckCollision(PortaleNuovaPartita) == true)
                this.CaricaLivello("Level1", false);
            if (agent.CheckCollision(PortaleEsci) == true)
            {
                this.LevelTrack.Stop();
                this.Game.PossoUscire = true;
                this.Game.Exit();
            }
        }

        private void ControllaPressioneTasti()
        {
            KeyboardState k = Keyboard.GetState();
            if (k.IsKeyDown(Keys.F1) && FormRichiamata == null)
            {
                this.Player.DisabilitaMovimenti = true;
                (FormRichiamata = new TutorialForm()).ShowDialog();
                this.Player.DisabilitaMovimenti = false;
                FormRichiamata = null;
            }
            else if (k.IsKeyDown(Keys.F2) && FormRichiamata == null)
            {
                this.Player.DisabilitaMovimenti = true;
                if ((FormRichiamata = new MultiPlayerSettings(this.Game)).ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    this.LevelTrack.Stop();
                    this.Game.MenuPrincipale = null;
                }
                FormRichiamata = null;
                this.Player.DisabilitaMovimenti = false;
            }
            else if (k.IsKeyDown(Keys.F3) && FormRichiamata == null)
            {
                this.Player.DisabilitaMovimenti = true;
                if ((FormRichiamata = new SelectLevelForm()).ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    this.LevelTrack.Stop();
                    this.Game.MenuPrincipale = null;
                }
                this.FormRichiamata = null;
                this.Player.DisabilitaMovimenti = false;
            }
            else if (k.IsKeyDown(Keys.F4) && FormRichiamata == null)
            {
                this.Player.DisabilitaMovimenti = true;
                if ((FormRichiamata = new GameSettings(this.Game)).ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    this.Game.SettingsLoad();
                this.FormRichiamata = null;
                this.Player.DisabilitaMovimenti = false;
            }
        }

        private void CaricaLivello(string Livello, bool IsSavedgame)
        {
            this.LevelTrack.Stop();
            this.Game.Level = new Scenario(this.Game, @".\Content\Dungeons", Livello, IsSavedgame);
        }

        #endregion

        #region Metodi Ereditati

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);  
            this.Game.Window.Title = "NERD OR DUNGEONS : THE GAME (Press F1 For Tutorial)";
            ControllaCollisioni(Player);
            ControllaPressioneTasti();
        }

        #endregion
    }
}
