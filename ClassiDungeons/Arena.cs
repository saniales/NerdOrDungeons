using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace NerdOrDungeons
{
    public sealed class Arena : DrawableGameComponent
    {
        /* Premetto:
         * si trovano parecchi if(PersonaggioIstanza/NemicoIstanza != null) in quanto gli elementi di una lista non li rimuovo
         * ma li metto a null poi faccio lavorare il GC alla fine (mi dava problemi Visual Studio con i cicli for
         * sennò)
         */

        #region Dichiarazione Variabili

        public string CurrentLevelName;

        private Personaggio[] Players;
        private bool[] Dead;
        private bool[] PresenzaGiocatore;
        private string[] PersonaggioGiocatore;
        private Controller[] Controllers;


        private SoundEngine LevelTrack;
        private Sprite IstanzaUnicaPerTerreno;
        private List<OggettoInanimato> Fosse; //Solo I Proiettili Le Oltrepassano
        private List<OggettoInanimato> Muri;
        private bool CaricaMessaggioFinePartita;
        private bool DevoCaricareLevelTrack;
        private string DirectoryPath;
        private new MainGame Game;

        #endregion

        #region Costruttore

        public Arena(Game Game, string DirectoryPath, string PersonaggioGiocatore1, string PersonaggioGiocatore2, string PersonaggioGiocatore3, string PersonaggioGiocatore4, Controller ControllerGiocatore1, Controller ControllerGiocatore2, Controller ControllerGiocatore3, Controller ControllerGiocatore4)
            : base(Game)
        {
            this.Game = (MainGame)Game;

            this.DirectoryPath    = DirectoryPath;
            this.CurrentLevelName = "Arena";

            this.Dead               = new bool[] { false, false, false, false };
            this.PresenzaGiocatore  = new bool[] { false, false, false, false };
            this.PersonaggioGiocatore = new string[]     { PersonaggioGiocatore1, PersonaggioGiocatore2,
                                                           PersonaggioGiocatore3, PersonaggioGiocatore4 };
            this.Controllers          = new Controller[] { ControllerGiocatore1 , ControllerGiocatore2 , 
                                                           ControllerGiocatore3 , ControllerGiocatore4  };
            this.Players = new Personaggio[4];

            this.CaricaMessaggioFinePartita = false;
            this.DevoCaricareLevelTrack = true;

            Muri = new List<OggettoInanimato>();
            Fosse = new List<OggettoInanimato>();

            CaricaScenarioDaLevelMap();
        }

        #endregion

        #region Funzioni Per Caricare lo Scenario

        private void CaricaScenarioDaLevelMap() //A partire da un Level.map(.txt fittizio) carico il livello 
        {
            StreamReader sr;
            OggettoInanimato tempObj;
            Vector2 tempPosizione;
            string tempRiga;
            char CarattereJolly;        // Lo uso per caricare l'immagine a seconda di quella
            // Che mi serve in un determinato ciclo.

            sr = new StreamReader(DirectoryPath + @"\Level.map");
            this.Game.SpriteBatch = new SpriteBatch(this.Game.GraphicsDevice);
            IstanzaUnicaPerTerreno = new Sprite(this.Game, DirectoryPath + @"\terreno.png", Vector2.Zero);

            tempPosizione = Vector2.Zero;

            if (this.Game.MenuPrincipale != null)
                this.Game.MenuPrincipale = null;

            this.LevelTrack = SoundEngine.Tracks["Level6"];

            while (!sr.EndOfStream)
            {
                tempRiga = sr.ReadLine();
                for (int i = 0; i < tempRiga.Length; i++)
                {
                    CarattereJolly = tempRiga[i];
                    tempObj = new OggettoInanimato(this.Game, tempPosizione);

                    switch (CarattereJolly) //a seconda del carattere faccio un terreno/muro/personaggio/nemico/antani
                    {
                        //Muri
                        case '#':
                            Muri.Add(tempObj);
                            break;
                        case '^':
                            Fosse.Add(tempObj);
                            break;
                        //Players
                        case '1':
                            if (PersonaggioGiocatore[0] != "")
                            {
                                this.Players[0] = Personaggio.Istances[PersonaggioGiocatore[0]].Copy(tempPosizione);
                                this.Players[0].IndiceGiocatore[0] = true;
                                this.Players[0].PlayerInput[0] = Controllers[0]; // Player1Input = Player Principale
                                this.Players[0].AbilitaMultiplayer();
                                this.PresenzaGiocatore[0] = true;
                            }
                            break;
                        case '2':
                            if (PersonaggioGiocatore[1] != "")
                            {
                                this.Players[1] = Personaggio.Istances[PersonaggioGiocatore[1]].Copy(tempPosizione);
                                this.Players[1].IndiceGiocatore[1] = true;
                                this.Players[1].PlayerInput[1] = Controllers[1]; //Player2Input = Player Secondario
                                this.Players[1].AbilitaMultiplayer();
                                this.PresenzaGiocatore[1] = true;
                            }
                            break;
                        case '3':
                            if (PersonaggioGiocatore[2] != "")
                            {
                                this.Players[2] = Personaggio.Istances[PersonaggioGiocatore[2]].Copy(tempPosizione);
                                this.Players[2].IndiceGiocatore[2] = true;
                                this.Players[2].PlayerInput[2] = Controllers[2];
                                this.Players[2].AbilitaMultiplayer();
                                this.PresenzaGiocatore[2]    = true;
                            }
                            break;
                        case '4':
                            if (PersonaggioGiocatore[3] != "")
                            {
                                this.Players[3] = Personaggio.Istances[PersonaggioGiocatore[1]].Copy(tempPosizione);
                                this.Players[3].IndiceGiocatore[3] = true;
                                this.Players[3].PlayerInput[3] = Controllers[3];
                                this.Players[3].AbilitaMultiplayer();
                                this.PresenzaGiocatore[3] = true;
                            }
                            break;
                        default: /* SALTA AL SUCCESSIVO */ break;
                    }
                    tempPosizione.X += 16;      /* 16x16 pixel = dimensione dello sprite del terreno */
                }
                tempPosizione.X = 0;
                tempPosizione.Y += 16;
            }

            //Verifico i Player Inesistenti
            for (int i = 0; i < PresenzaGiocatore.Length; i++)
                if (!PresenzaGiocatore[i]) /* se il player non c'è è uguale a null */
                    Dead[i] = true;

            sr.Close();
            GC.Collect();
        }

        #endregion

        #region Membri Ereditati

        public override void Draw(GameTime gameTime)
        {
            IstanzaUnicaPerTerreno.Draw(gameTime);
            foreach (Personaggio Player in Players)
                if(Player != null)
                    Player.Draw(gameTime);
            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            if (DevoCaricareLevelTrack)
            {
                this.LevelTrack.Play(true);
                this.DevoCaricareLevelTrack = false;
            }
            KeyboardState k = Keyboard.GetState();

            if (k.IsKeyDown(Keys.Escape))
            {
                this.LevelTrack.Stop();
                this.Game.MenuPrincipale = new Menu(this.Game, @".\Content\Menu");
                this.Game.ArenaMultiplayer = null;
            }

            //prima controllo le collisioni del frame precedente, poi mi muovo (update)
            for (int i = 0; i < PresenzaGiocatore.Length; i++)
                if (PresenzaGiocatore[i])
                {
                    ControllaCollisioni(Players[i]);
                    ControllaCollisioni(Players[i].ListaProiettili);
                }
            //mi muovo
            for (int i = 0; i < PresenzaGiocatore.Length; i++)
                if (PresenzaGiocatore[i])
                    Players[i].Update(gameTime);

             //Aggiorno La Barra Del Titolo (Multiplayer)
                string[] p = new string[] { "", "", "", "" };
                for (int i = 0; i < PresenzaGiocatore.Length; i++)
                {
                    if (this.PresenzaGiocatore[i])
                        if (this.Players[i].Parametri.Salute > 0)
                            p[i] = this.Players[i].Parametri.Salute.ToString();
                        else
                            p[i] = "-";
                    else
                        p[i] = "-";
                }                   
                base.Update(gameTime);
                this.Game.Window.Title = "PLAYER 1 = " + p[0] + "     PLAYER 2 = " + p[1] + "     PLAYER 3 = " + p[2] + "     PLAYER 4 = " + p[3]; 
        }

        private new void Dispose()
        {
            LevelTrack.Dispose();
            Reset();
            for (int i = 0; i < PresenzaGiocatore.Length; i++)
                if (PresenzaGiocatore[i])
                    Players[i].Dispose();
            base.Dispose();
        }
        #endregion

        #region Funzioni Di appoggio e Funzioni Per Aiutare Il Disposing Degli Oggetti

        #region ControllaCollisioni Overloads

        private void ControllaCollisioni(GestoreProiettili p)
        {
            //Controllo Che I Proiettili Miei Colpiscano un avversario.
            if (p.Istanza != null && p.Owner != null)
            {
                for (int j = 0; j < p.Count; j++)
                {
                    for (int k = 0; k < Players.Length; k++)
                        if (Players[k] != null && (Personaggio)p.Owner != Players[k])
                        {
                            if (p.Istanza[j].CheckCollision(Players[k]) == true)
                            {
                                Players[k].Parametri.Salute -= p.Istanza[j].Parametri.Attacco;
                                p.Istanza.RemoveAt(j);
                                if (Players[k].Parametri.Salute <= 0)
                                {
                                    Dead[k] = true;
                                    if (UnicoVivo())
                                        CaricaMessaggioFinePartita = true;
                                    Players[k].MuoriMultiplayer();
                                }
                            }
                        }
                }
            }
            if (CaricaMessaggioFinePartita)
            {
                IstanzaUnicaPerTerreno = new Sprite(this.Game, DirectoryPath + @"\winner.png", Vector2.Zero);
                CaricaMessaggioFinePartita = false;
            }
            //Se Colpisce un Muro/MuroRimovibile Lo Elimino Semplicemente
            foreach (OggettoInanimato Obj in this.Muri)
                for (int i = 0; i < p.Count; i++)
                        if (p.Istanza[i].CheckCollision(Obj) == true)
                            p.Istanza.RemoveAt(i);

        }
        private bool UnicoVivo()
        {
            int tot = 0;
            foreach (bool b in Dead)
                if (!b)
                    tot++;
            if (tot != 1)
                return false;
            else return true;
            
        }

        private void ControllaCollisioni(Agente Agent)
        {
            foreach (OggettoInanimato Muro in Muri)
                if (Agent.CheckCollision(Muro) == true)
                    Agent.Posizione = Agent.OldPosizione;
            foreach (OggettoInanimato Fossa in Fosse)
                if (Agent.CheckCollision(Fossa) == true)
                    Agent.Posizione = Agent.OldPosizione;
        }
        
     #endregion

        public void Reset()
        {
            for (int i = 0; i < PresenzaGiocatore.Length; i++)
                if (PresenzaGiocatore[i])
                    Players[i].ListaProiettili.Clear();
            Muri.Clear();
            Fosse.Clear();
        }

        #endregion

    }
}