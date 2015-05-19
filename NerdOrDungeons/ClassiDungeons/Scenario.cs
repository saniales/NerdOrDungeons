using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace NerdOrDungeons
{
    /**                                                          **
     **************************************************************
     **                                                          **
     ** Scenario:                                                **
     ** Classe Di Gestione Dei Vari Livelli(Dungeons) e dei vari **
     ** Agenti e Oggetti presenti al suo interno e del loro      **
     ** Interfacciamento con il Main Game                        **
     **                                                          **
     **              [Mana Roberto Laudatum Est]                 **
     **                                                          **
     **************************************************************
     **                                                          **/ 

    public class Scenario : DrawableGameComponent
    {     
        /* Premetto:
         * si trovano parecchi if(PersonaggioIstanza/NemicoIstanza != null) in quanto gli elementi di una lista non li rimuovo
         * ma li metto a null poi faccio lavorare il GC alla fine (mi dava problemi Visual Studio con i cicli for
         * sennò)
         */

        #region Dichiarazione Variabili

        public        string                 CurrentLevelName;
                          
        internal      Personaggio            Player;
        internal      SoundEngine            LevelTrack;
        internal      Sprite                 IstanzaUnicaPerTerreno;
        internal      Sequenza               DialoghiPreLivello;
        internal      List<Nemico>           Nemici;
        internal      Tomonobu               BossFinale;
        internal      FiendDragon            Dragone;
        internal      List<OggettoInanimato> Fosse; //Solo I Proiettili Le Oltrepassano
        internal      List<OggettoInanimato> Muri;
        internal      List<OggettoInanimato> MuriRimovibili;
        internal      List<OggettoInanimato> ScalaPerIlLivelloSuccessivo;
        internal      List<PowerUpGenerico>  PowerUps;
        internal      List<Missile>          Missili;
        internal      uint                   NemiciUccisi;
         
        protected     bool                   DevoCaricareLevelTrack;
        protected     bool                   IsSavedgame;
        protected     bool                   Dead;
        protected     string                 DirectoryPath;
        protected new MainGame               Game;
        protected     Interruttore[]         Interruttori;


        private       int                    currentLevelIndex;
        private       SpriteFont             SpriteTesto;
        //Quando uso un mezzo quando scendo servono a riprendere i parametri del non-vehicle
        private       Proiettile             tempProiettile;
        private       ParametriPersonaggio   tempParams;
        private GeneratoreNemici God;
        private bool Pausa;
        private bool Temporizzatore;
        

        #endregion

        #region Costruttore

        public Scenario(Game Game, string DirectoryPath, string LeveltoLoad, bool PartitaSalvata) : base(Game) 
        {
            Personaggio.InizializzaClasse();
            this.Game = (MainGame)Game;

            this.NemiciUccisi     = 0;
            this.DirectoryPath    = DirectoryPath + @"\" + LeveltoLoad;
            this.CurrentLevelName = LeveltoLoad;
            this.Dead             = false;
            this.IsSavedgame      = PartitaSalvata;

            ScalaPerIlLivelloSuccessivo = new List<OggettoInanimato>();
            Muri           = new List<OggettoInanimato>();
            MuriRimovibili = new List<OggettoInanimato>();
            Fosse          = new List<OggettoInanimato>();
            Nemici         = new List<Nemico>();
            PowerUps       = new List<PowerUpGenerico>();
            Missili        = new List<Missile>();
            Interruttori   = new Interruttore[3];
            
            if (this.IsSavedgame)
                LoadGamePlayerSettings();
            else
                Player = Personaggio.Istances["LinkBlue"].Copy(Vector2.Zero);

            SpriteTesto = this.Game.Content.Load<SpriteFont>("SpriteTesto");
            CaricaScenarioDaLevelMap();       
        }

        #endregion

        #region Funzioni Per Caricare lo Scenario

        protected virtual void CaricaScenarioDaLevelMap() //A partire da un Level.map(.txt fittizio) carico il livello 
        {
            StreamReader     sr;
            OggettoInanimato tempObj;
            Vector2          tempPosizione;
            string           tempRiga;
            char             CarattereJolly;        // Lo uso per caricare l'immagine a seconda di quella
                                                    // Che mi serve in un determinato ciclo.

            sr                     = new StreamReader(DirectoryPath + @"\Level.map");
            this.Game.SpriteBatch  = new SpriteBatch(this.Game.GraphicsDevice);
            IstanzaUnicaPerTerreno = new Sprite(this.Game, DirectoryPath + @"\terreno.png", Vector2.Zero);
            currentLevelIndex      = Convert.ToInt32(CurrentLevelName.Substring(CurrentLevelName.LastIndexOf("l") + 1), 10);

            tempPosizione = Vector2.Zero;

            if (this.Game.MenuPrincipale != null)
                this.Game.MenuPrincipale = null;

            currentLevelIndex = Convert.ToInt32(CurrentLevelName.Substring(CurrentLevelName.LastIndexOf('l') + 1), 10);

            this.LevelTrack = SoundEngine.Tracks[CurrentLevelName];
            /* Se carico lo Scenario la Prima Volta è null Per Forza
               Se nel livello prima avevo la stessa track garantisco la continuità
               Se sono nel primo livello non faccio questo test : va in errore perché cerca Level0 */
            if (currentLevelIndex != 1 && !this.LevelTrack.Equals(SoundEngine.Tracks["Level" + (currentLevelIndex - 1)]))
                SoundEngine.Tracks["Level" + (currentLevelIndex - 1)].Stop();

            Interruttori[0] = Interruttore.Tipo1.Copy(Vector2.Zero);
            Interruttori[1] = Interruttore.Tipo2.Copy(Vector2.Zero);
            Interruttori[2] = Interruttore.Tipo3.Copy(Vector2.Zero);

               
            //NON TUTTI I LIVELLI HANNO DELLE SEQUENZE DI MONOLOGO/DIALOGO
            try 
            { DialoghiPreLivello = new Sequenza(this.Game, @".\Content\Sequences\" + CurrentLevelName + ".seq", SoundEngine.Tracks["Sequence" + CurrentLevelName.Substring(CurrentLevelName.LastIndexOf('l') + 1)]); }
            catch (Exception) { DialoghiPreLivello = null; this.LevelTrack.Play(true); }
                               
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
                        case '@':
                            ScalaPerIlLivelloSuccessivo.Add(tempObj);
                            break;
                        case '.':
                            MuriRimovibili.Add(tempObj);
                            break;
                        case '#':
                            Muri.Add(tempObj);
                            break;
                        case '^':
                            Fosse.Add(tempObj);
                            break;
                            //Interruttori
                        case 'Q':
                            Interruttori[0].AddDependingWall(MuroRimovibile.Tipo1.Copy(tempPosizione));
                            break;
                        case 'W':
                            Interruttori[1].AddDependingWall(MuroRimovibile.Tipo2.Copy(tempPosizione));
                            break;
                        case 'E':
                            Interruttori[2].AddDependingWall(MuroRimovibile.Tipo3.Copy(tempPosizione));
                            break;
                        case '1':
                            Interruttori[0].Posizione = tempPosizione;
                            break;
                        case '2':
                            Interruttori[1].Posizione = tempPosizione;
                            break;
                        case '3':
                            Interruttori[2].Posizione = tempPosizione;
                            break;
                            //Player
                        case 'P':
                            if (this.Player.isVehicle)
                            {
                                //Scendo dal veicolo e riprendo i panni del personaggio normale
                                this.Player = Personaggio.Istances["LinkBlue"].Copy(tempPosizione);
                                this.Player.ProiettileCheSparo = tempProiettile;
                                this.Player.PossoSparare = true;
                                this.Player.Parametri = tempParams;
                            }
                            else
                            {
                                Player.Posizione = tempPosizione;
                                Player.OldPosizione = tempPosizione;
                            }
                            break;
                        case 'O':
                            if (!this.Player.isVehicle)
                            {
                                //salgo sul veicolo
                                this.tempProiettile = this.Player.ProiettileCheSparo;
                                this.tempParams = this.Player.Parametri;
                                this.Player = Personaggio.Istances["PlayerShip"].Copy(tempPosizione);
                            }
                            else
                            {
                                Player.Posizione = tempPosizione;
                                Player.OldPosizione = tempPosizione;
                            }
                            break;
                        case '%':
                            Player = Personaggio.Istances["LinkBlack"].Copy(tempPosizione);
                            Player.Parametri = ParametriPersonaggio.LinkPotenziato;
                            break;
                            //Enemies
                        case 'S':
                            Nemici.Add(Nemico.Snake.Copy(tempPosizione));
                            break;
                        case 'R':
                            Nemici.Add(Nemico.OctorockRed.Copy(tempPosizione));
                            break;
                        case 'G':
                            Nemici.Add(Nemico.Goomba.Copy(tempPosizione));
                            break;
                        case 'F':
                            Nemici.Add(Nemico.FightingPuppet.Copy(tempPosizione));
                            break;
                        case '/': Nemici.Add(Nemico.Myself.Copy(tempPosizione));
                            break;
                        case 'H':
                            Nemici.Add(Nemico.EnemyShip.Copy(tempPosizione));
                            break;
                        case 'I':
                            Nemici.Add(Nemico.WaterTank.Copy(tempPosizione));
                            break;
                        case 'J':
                            Nemici.Add(Nemico.EarthTank.Copy(tempPosizione));
                            break;
                        case 'V':
                            Nemici.Add(Nemico.LittleDevil.Copy(tempPosizione));
                            break;
                            //Bosses
                        case 'M':
                            Nemici.Add(Nemico.MegaOctorockRed.Copy(tempPosizione));
                            break;
                        case 'L':
                            Nemici.Add(Nemico.Prematurator.Copy(tempPosizione));
                            break;
                        case 'Y':
                            Nemici.Add(AntiSganon.Create(tempPosizione));
                            break;
                        case 'U':
                            Nemici.Add(MegaWaterTank.Create(tempPosizione));
                            break;
                        case 'T':
                            BossFinale = Tomonobu.Create(tempPosizione);
                            break;
                        case 'D':
                            Dragone    = FiendDragon.Create(tempPosizione);
                            break;
                            //powerUps
                        case 'a':
                            PowerUps.Add(ModificatoreParametri.SmallMedikit.Copy(tempPosizione));
                            break;
                        case 's':
                            PowerUps.Add(ModificatoreParametri.MediumMedikit.Copy(tempPosizione));
                            break;
                        case 'd':
                            PowerUps.Add(ModificatoreParametri.BigMedikit.Copy(tempPosizione));
                            break;
                        case 'f':
                            PowerUps.Add(ModificatoreParametri.AttackSmallBonus.Copy(tempPosizione));
                            break;
                        case 'g':
                            PowerUps.Add(ModificatoreParametri.AttackBigBonus.Copy(tempPosizione));
                            break;
                        case 'h':
                            PowerUps.Add(ModificatoreParametri.AttackMalus.Copy(tempPosizione));
                            break;
                        case 'j':
                            PowerUps.Add(ModificatoreParametri.SpeedBonus.Copy(tempPosizione));
                            break;
                        case 'k':
                            PowerUps.Add(ModificatoreParametri.SpeedMalus.Copy(tempPosizione));
                            break;
                            //Pergamene
                        case 'q':
                            PowerUps.Add(Pergamena.BlueLaserScroll.Copy(tempPosizione));
                            break;
                        case 'w':
                            PowerUps.Add(Pergamena.RedLaserScroll.Copy(tempPosizione));
                            break;
                        case 'e':
                            PowerUps.Add(Pergamena.GreenLaserScroll.Copy(tempPosizione));
                            break;
                        case 'r':
                            PowerUps.Add(Pergamena.FlameShotScroll.Copy(tempPosizione));
                            break;
                        case 't':
                            PowerUps.Add(Pergamena.UltimateFlameShotScroll.Copy(tempPosizione));
                            break;
                            //GeneratoreNemici + Me Stesso oscuro Potenziato
                        case '*': 
                            Player = Personaggio.Istances["LinkBlack"].Copy(tempPosizione);
                            Player.Parametri = ParametriPersonaggio.LinkPotenziato.Copy();
                            Player.PossoSparare = true;
                            God = GeneratoreNemici.Istanzia(2);
                            break;
                        // Lo Considero Terreno Quindi Non Faccio Nulla
                        default: break;
                    }
                    tempPosizione.X += 16;      //16x16 pixel = dimensione dello sprite del terreno
                }
                tempPosizione.X = 0;
                tempPosizione.Y += 16;
            }
            sr.Close();
            //rilascio gli interruttori Inutilizzati
            for (int i= 0; i < Interruttori.Length;i++)
                if (Interruttori[i].Posizione == Vector2.Zero)
                    Interruttori[i] = null;

            if (DialoghiPreLivello != null)
                SaveGame();

            GC.Collect();
        }

        #endregion

        #region Membri Ereditati
         
        public override void Draw(GameTime gameTime) 
        {
            //this.Game.SpriteBatch.Begin();
            
            if (DialoghiPreLivello == null)
            {
                IstanzaUnicaPerTerreno.Draw(gameTime);

                foreach (Interruttore s in Interruttori)
                    if(s != null)
                        s.Draw(gameTime);
                foreach (PowerUpGenerico p in this.PowerUps)
                    p.Draw(gameTime);
                foreach (Nemico Enemy in Nemici)
                        Enemy.Draw(gameTime);
                if (BossFinale != null)
                    BossFinale.Draw(gameTime);
                else if (Dragone != null)
                    Dragone.Draw(gameTime);
                foreach (Missile m in Missili)
                    if (m != null)
                        m.Draw(gameTime);
                Player.Draw(gameTime);

                base.Draw(gameTime);
            }
            else
                DialoghiPreLivello.Draw(gameTime);

            if (Dead && DialoghiPreLivello == null)
            {
                string DeathPhrase = "I Did Not Manage To Attempt My Quest...\n                         GAME OVER";
                this.Game.SpriteBatch.DrawString(SpriteTesto, DeathPhrase, Sequenza.PuntoDaCuiScrivo - (SpriteTesto.MeasureString(DeathPhrase) / 2), Color.White);
            }
            if (Pausa && DialoghiPreLivello == null)
            {
                string DeathPhrase = "PAUSE";
                this.Game.SpriteBatch.DrawString(SpriteTesto, DeathPhrase, Sequenza.PuntoDaCuiScrivo - (SpriteTesto.MeasureString(DeathPhrase) / 2), Color.White);
            }

            //this.Game.SpriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState k = Keyboard.GetState();
            GamePadState g = GamePad.GetState(PlayerIndex.One);

            if (this.Player.Parametri.Salute < 0)
                this.Player.Parametri.Salute = 0;
            if (!(this is Menu))
                this.Game.Window.Title = "PLAYER : LIFE = " + this.Player.Parametri.Salute +
                                         " - ATTACK = " + this.Player.Parametri.Attacco +
                                         " - SPEED = " + this.Player.Parametri.Velocità + "          ";

            if (DialoghiPreLivello != null && DialoghiPreLivello.isOver)
            {
                DialoghiPreLivello = null;
                DevoCaricareLevelTrack = true;
            }

            if (DevoCaricareLevelTrack)
            {
                this.LevelTrack.Play(true);
                this.DevoCaricareLevelTrack = false;
            }

            if (((g.IsButtonDown(Buttons.Start) && this.Game.Joystick) ||
               (k.IsKeyDown(Keys.Enter) && !this.Game.Joystick)) &&
                !(this is Menu) && !Temporizzatore)
            {
                Pausa = !Pausa;
                Temporizzatore = true;
            }

            if (((g.IsButtonUp(Buttons.Start) && this.Game.Joystick) ||
               (k.IsKeyUp(Keys.Enter) && !this.Game.Joystick)) &&
                !(this is Menu))
                Temporizzatore = false;
            if (DialoghiPreLivello == null && !Pausa)
            {

                //questa if serve per i Boss Levels (dove per continuare devo uccidere i boss)
                //i muri rimovibili bloccano il player fino a quel punto
                if (HoUccisoTuttiINemici())
                    if (this.currentLevelIndex == 32)
                        this.ScalaPerIlLivelloSuccessivo.Add(new OggettoInanimato(this.Game, this.Player.Posizione));
                    else
                        MuriRimovibili.Clear();
                foreach (PowerUpGenerico p in this.PowerUps)
                    if (p.CheckCollision(Player) == true)
                        p.TouchedBy(Player);

                if (((g.IsButtonDown(Buttons.Back) &&  this.Game.Joystick) ||
                    (k.IsKeyDown(Keys.Escape)     && !this.Game.Joystick)) &&
                     !(this is Menu))
                {
                    this.LevelTrack.Stop();
                    this.Game.MenuPrincipale = new Menu(this.Game, @".\Content\Menu");
                    if (this.BossFinale != null)
                        this.BossFinale.Dispose();
                    else if (this.Dragone != null)
                        Dragone.Dispose();
                    else if (this.God != null)
                        God.Dispose();
                    this.God = null;
                    this.Dragone = null;
                    this.BossFinale = null;
                }
                if (God != null)
                {
                    God.Update(gameTime);
                    this.Game.Window.Title += "SCORE : " + God.PunteggioGiocatore + " - ENEMIES COUNT : " + this.Nemici.Count;
                }
                ControllaCollisioni(Player.ListaProiettili);
                foreach (Interruttore s in Interruttori)
                    if(s != null)
                        s.Update(gameTime);
                foreach (Nemico Enemy in Nemici)
                    ControllaCollisioni(Enemy.ListaProiettili);

                Player.Update(gameTime);
                foreach (Nemico Enemy in Nemici)
                {
                    ControllaCollisioni(Enemy);
                    if (Enemy is AntiSganon)
                        ((AntiSganon)Enemy).Update(gameTime);
                    else if (Enemy is MegaWaterTank)
                        ((MegaWaterTank)Enemy).Update(gameTime);
                    else
                        Enemy.Update(gameTime);
                }
                if (BossFinale != null)
                {
                    ControllaCollisioni(BossFinale);
                    BossFinale.Update(gameTime);
                    this.Game.Window.Title += "TOMONOBU LIFE = " + this.BossFinale.Parametri.Salute;
                }
                if (Dragone != null)
                {
                    ControllaCollisioni(Dragone);
                    Dragone.Update(gameTime);
                    this.Game.Window.Title += "FIEND DRAGON LIFE = " + this.Dragone.Parametri.Salute;
                }
                foreach (Missile m in Missili) 
                {
                    ControllaCollisioni(m);
                    m.Update(gameTime);
                }
                ControllaCollisioni(Player);
            }
            else if (DialoghiPreLivello != null)
                DialoghiPreLivello.Update(gameTime);
        }

        protected new void Dispose() {
            LevelTrack.Dispose();
            DialoghiPreLivello.Dispose();
            Reset();
            Player.Dispose(); // il personaggio lo dealloco solo se esco dal gioco, se cambio livello no
            base.Dispose();
        }

        #endregion

        #region Funzioni Di appoggio e Funzioni Per Aiutare Il Disposing Degli Oggetti

        #region ControllaCollisioni Overloads

        protected void ControllaCollisioni(GestoreProiettili p)
        {
            if (p.Owner != null)
            {
                if (p.Owner is Nemico)
                    //Controllo Che I Proiettili Nemici Colpiscano Personaggio
                    for (int i = 0; i < p.Count; i++)
                            if (p.Istanza[i].CheckCollision(Player) == true)
                            {
                                Player.Parametri.Salute -= p.Istanza[i].Parametri.Attacco;
                                p.Istanza.RemoveAt(i);
                                if (Player.Parametri.Salute <= 0)
                                {
                                    Player.Muori();
                                    Dead = true;
                                }
                            }

                if (p.Owner is Personaggio)
                {
                    //Controllo Che I Proiettili Colpiscano un Nemico
                    for (int i = 0; i < Nemici.Count; i++)
                        for (int j = 0; j < p.Count; j++)
                                    if (Nemici.Count != 0 && p.Istanza[j].CheckCollision(Nemici[i]) == true)
                                    {
                                        Nemici[i].Parametri.Salute -= p.Istanza[j].Parametri.Attacco;
                                        p.Istanza.RemoveAt(j);
                                        if (Nemici[i].Parametri.Salute <= 0)
                                        {
                                            Nemici.RemoveAt(i);
                                            NemiciUccisi++;
                                            SoundEngine.Effects["EnemyKilled"].Play();
                                        }
                                    }
                    //Controllo Che I Proiettili Colpiscano il boss finale Tomonobu.
                        for (int j = 0; j < p.Count; j++)
                            if (BossFinale != null && p.Istanza[j] != null)
                                if (p.Istanza[j].CheckCollision(BossFinale) == true)
                                {
                                    BossFinale.Parametri.Salute -= p.Istanza[j].Parametri.Attacco;
                                    p.Istanza.RemoveAt(j);
                                    if (BossFinale.Parametri.Salute <= 0)
                                    {
                                        Nemici.Clear();
                                        BossFinale.Dispose();
                                        BossFinale = null;
                                        SoundEngine.Effects["EnemyKilled"].Play();
                                        this.ScalaPerIlLivelloSuccessivo.Add(new OggettoInanimato(this.Game, this.Player.Posizione));
                                    }
                                }
                        //Controllo Che I Proiettili Miei Colpiscano il boss FiendDragon.
                        for (int j = 0; j < p.Count; j++)
                            if (Dragone != null)
                                if (p.Istanza[j].CheckCollision(Dragone) == true)
                                {
                                    Dragone.Parametri.Salute -= p.Istanza[j].Parametri.Attacco;
                                    p.Istanza.RemoveAt(j);
                                    if (Dragone.Parametri.Salute <= 0)
                                    {
                                        Dragone.Dispose();
                                        Dragone = null;
                                        SoundEngine.Effects["EnemyKilled"].Play();
                                    }
                                }
                    for(int i = 0; i < Missili.Count; i++)
                        for(int j = 0; j < p.Count; j++)
                                if (p.Istanza[j].CheckCollision(Missili[i]) == true)
                                {
                                    Missili.RemoveAt(i);
                                    p.Istanza.RemoveAt(j);
                                }
                }
                //Se Colpisce un Muro/MuroRimovibile Lo Elimino Semplicemente
                foreach (OggettoInanimato Obj in this.Muri)
                    for (int i = 0; i < p.Count; i++)
                            if (p.Istanza[i].CheckCollision(Obj) == true)
                                p.Istanza.RemoveAt(i);
                foreach (OggettoInanimato Obj in this.MuriRimovibili)
                    for (int i = 0; i < p.Count; i++)
                            if (p.Istanza[i].CheckCollision(Obj) == true)
                                p.Istanza.RemoveAt(i);
                //I Muri Legati Collidono Solo Se C'è L'Interruttore Attivato
                foreach (Interruttore s in Interruttori)
                    if (s != null && s.isActive)
                        foreach (MuroRimovibile m in s.MuriLegati)
                            for (int i = 0; i < p.Count; i++)
                                    if (p.Istanza[i].CheckCollision(m) == true)
                                        p.Istanza.RemoveAt(i);
                    
                //Se Un proiettile di Personaggio Colpisce Uno Switch ne Cambia lo Stato, poi scompare
                //Utile Per Attivare Muri A Distanza
                foreach (Interruttore s in Interruttori)
                    for (int i = 0; i < p.Count; i++)
                        if (s != null && p.Istanza[i] != null && (p.Owner is Personaggio))
                            if (s.CheckSwitch(p.Istanza[i]))
                                p.Istanza.RemoveAt(i);
                if (Dead)
                {
                    foreach (Nemico Enemy in this.Nemici)
                            Enemy.IA = TipoIA.Assente;
                }
            }
        }

        protected void ControllaCollisioni(Agente Agent)
        {
            //prima i muri
            foreach (OggettoInanimato Muro in Muri)
                if (Agent.CheckCollision(Muro) == true)
                    Agent.Posizione = Agent.OldPosizione;
            foreach (OggettoInanimato Muro in MuriRimovibili)
                if (Agent.CheckCollision(Muro) == true)
                    Agent.Posizione = Agent.OldPosizione;
            foreach (Interruttore s in Interruttori)
                if (s != null && s.isActive)
                    foreach (MuroRimovibile m in s.MuriLegati)
                        if (Agent.CheckCollision(m) == true)
                            Agent.Posizione = Agent.OldPosizione;
            foreach (OggettoInanimato Fossa in Fosse)
                if (Agent.CheckCollision(Fossa) == true)
                    Agent.Posizione = Agent.OldPosizione;
            //poi gli interruttori (per Eventuali Switch())
            foreach (Interruttore s in Interruttori)
                if (s != null)
                    s.CheckSwitch(Player);
               


            //poi i passaggi al livello successivo
            if (ScalaPerIlLivelloSuccessivo != null)
                for (int i = 0; i < ScalaPerIlLivelloSuccessivo.Count; i++)
                    if (Agent.CheckCollision(ScalaPerIlLivelloSuccessivo[i]) == true)
                        if (Agent is Personaggio)
                        {
                            Switch("Level" + (++currentLevelIndex));
                            NemiciUccisi = 0;
                            return;
                        }
                        else
                            Agent.Posizione = Agent.OldPosizione;

            //Controllo Collisioni Con I Nemici
            for (int i = 0; i < Nemici.Count; i++)
            {
                if (Agent != Nemici[i]) //Non collide con se stesso!
                    if (Agent.CheckCollision(Nemici[i]) == true)
                    {
                        Agent.Posizione = Agent.OldPosizione;
                        if (Agent is Personaggio)
                        {
                            if (isNotAttacking())
                            {
                                Agent.Parametri.Salute -= Nemici[i].Parametri.Attacco;
                                Nemici[i].Posizione = Nemici[i].OldPosizione;
                                if (Agent.Parametri.Salute < 0)
                                {
                                    Player.Muori();
                                    Dead = true;
                                }
                            }
                            else //Se attacco io o alleato è l'avversario a indietreggiare, se non riesce sta fermo li e si prende le botte
                            {
                                Nemici[i].Parametri.Salute -= Agent.Parametri.Attacco;
                                Nemici[i].Posizione = Nemici[i].OldPosizione;
                                if (Nemici[i].Parametri.Salute < 0)
                                {
                                    Nemici.RemoveAt(i);
                                    NemiciUccisi++;
                                    SoundEngine.Effects["EnemyKilled"].Play();
                                }
                                break;
                            }
                        }
                        /* SE NON E' UN PERSONAGGIO, MA UN NEMICO, POSSO FARE LA COLLISIONE NORMALE (NON SI FERISCONO A VICENDA) */
                        else Nemici[i].Posizione = Nemici[i].OldPosizione;
                    }
            }
            //Collisioni con il boss finale Tomonobu.
            if(Agent is Personaggio && BossFinale != null)
            {
                if (Agent.CheckCollision(BossFinale) == true)
                    {
                        Agent.Posizione = Agent.OldPosizione;
                            if (isNotAttacking())
                            {
                                Agent.Parametri.Salute -= BossFinale.Parametri.Attacco;
                                BossFinale.Posizione = BossFinale.OldPosizione;
                                if (Agent.Parametri.Salute < 0)
                                {
                                    Player.Muori();
                                    Dead = true;
                                }
                            }
                            else //Se attacco io o alleato è l'avversario a indietreggiare, se non riesce sta fermo li e si prende le botte
                            {
                                BossFinale.Parametri.Salute -= Agent.Parametri.Attacco;
                                BossFinale.Posizione = BossFinale.OldPosizione;
                                if (BossFinale.Parametri.Salute < 0)
                                {
                                    BossFinale.Dispose();
                                    BossFinale = null;
                                    SoundEngine.Effects["EnemyKilled"].Play();
                                    this.ScalaPerIlLivelloSuccessivo.Add(new OggettoInanimato(this.Game, this.Player.Posizione));
                                }
                            }
                        }
            }
            else if (Agent is Personaggio && Dragone != null)
            {
                if (Agent.CheckCollision(Dragone) == true)
                {
                    Agent.Posizione = Agent.OldPosizione;
                    if (isNotAttacking())
                    {
                        Agent.Parametri.Salute -= Dragone.Parametri.Attacco;
                        Dragone.Posizione = Dragone.OldPosizione;
                        if (Agent.Parametri.Salute < 0)
                        {
                            Player.Muori();
                            Dead = true;
                        }
                    }
                    else 
                    {
                        Dragone.Parametri.Salute -= Agent.Parametri.Attacco;
                        Dragone.Posizione = Dragone.OldPosizione;
                        if (Dragone.Parametri.Salute < 0)
                        {
                            Dragone.Dispose();
                            Dragone = null;
                            SoundEngine.Effects["EnemyKilled"].Play();
                            this.Player.ProiettileCheSparo = Proiettile.Types["FlameShot"].Copy();
                        }
                    }
                }
            }
            
            //Se Player colpisce un missile, esso sparisce ma toglie vita
            if(Agent is Personaggio)
                for(int i = 0; i < Missili.Count; i++)
                    if (Missili[i] != null && Missili[i].CheckCollision(Agent) == true)
                    {
                        Agent.Parametri.Salute -= Missili[i].Parametri.Attacco;
                        Missili.RemoveAt(i);
                        if (Agent.Parametri.Salute < 0)
                        {
                            Player.Muori();
                            Dead = true;
                        }    
                    }
            if (Dead)
            {
                // azzero la IA dei Nemici : Il Gioco Si Ferma
                foreach (Nemico Enemy in Nemici)
                        Enemy.IA = TipoIA.Assente;
                Player.ListaProiettili.Clear();
            }
        }

        public bool HoUccisoTuttiINemici() 
        { return Nemici.Count == 0; }

        #endregion

        //Se Player Sta Attaccando E' Lui A Togliere Vita A Enemy
        protected bool isNotAttacking() { 
            if (Player.Current == Player.Sprites[Personaggio.ATTACK_DOWN])
                return false;
            if (Player.Current == Player.Sprites[Personaggio.ATTACK_UP])
                return false;
            if (Player.Current == Player.Sprites[Personaggio.ATTACK_LEFT])
                return false;
            if (Player.Current == Player.Sprites[Personaggio.ATTACK_RIGHT])
                return false;
            /* ELSE */
            return true;
        }

        /** La Funzione Reset() Dealloca La Memoria Non Utilizzata                                                  **
         ** Può Essere Usata In Combinazione l Caricamento Di Un Nuovo Livello (Switch) Che Chiama Il GC.Collect(); **/
        public void Reset()
        {
            ScalaPerIlLivelloSuccessivo.Clear();
            PowerUps.Clear();
            /* RESETTO LA LISTA DEI PROIETTILI SPARATI DA UN GIOCATORE (non lo Fa Automaticamente Il Player e *
             * Non Devo Ridisegnare I Proiettili Sparati Nel Livello Prima)                                   */
            Player.ListaProiettili.Clear();
            Muri.Clear();
            MuriRimovibili.Clear();
            Fosse.Clear();
            Nemici.Clear();
            Missili.Clear();
            //resetto la posizione del player perché se no potrebbe collidere con qualcosa del livello dopo (powerup)
            Player.Posizione = Vector2.Zero;
        }

        //Cambia Livello
        public void Switch(string LevelSubPath) { //Es.: Level1, Level2, ecc...
            Reset();
            DirectoryPath = DirectoryPath.Replace(CurrentLevelName, LevelSubPath);
            CurrentLevelName = LevelSubPath;
            CaricaScenarioDaLevelMap();
        }

        //Se Ho Caricato La Partita Assegno i Parametri Salvati nel file 'Saves.cfg' Seconda Riga
        private void LoadGamePlayerSettings()
        {
            string[] temp;
            string NomeTipoPersonaggio;
            StreamReader sr;
            int tempSalute;
            int tempAttacco;
            float tempVelocità;
            bool tempCanShot;
            string ProjectileName;

            sr = new StreamReader(Directory.GetCurrentDirectory() + @"/Content/Saves.cfg");
            // Non Mi Serve Sapere Il Livello Da Caricare (Lo Ho Gia Caricato)
            sr.ReadLine();
            NomeTipoPersonaggio = sr.ReadLine();
            temp = sr.ReadLine().Split('-');

            tempSalute   = Convert.ToInt16(temp[0]);
            tempAttacco  = Convert.ToInt16(temp[1]);
            tempVelocità = Convert.ToSingle(temp[2]);
            tempCanShot  = Convert.ToBoolean(temp[3]);
            ProjectileName = temp[4];

            Player = Personaggio.Istances[NomeTipoPersonaggio].Copy(Vector2.Zero);

            Player.Parametri          = new ParametriPersonaggio(tempSalute, tempAttacco, tempVelocità);
            Player.PossoSparare       = tempCanShot;
            Player.ProiettileCheSparo = Proiettile.Types[ProjectileName];
            sr.Close();
        }

        public void SaveGame()
        {
            StreamWriter sw = new StreamWriter(@".\Content\Saves.cfg", false);
            sw.WriteLine(CurrentLevelName);
            sw.WriteLine(this.Player.Name);
            sw.WriteLine(Player.Parametri.Salute + "-" + Player.Parametri.Attacco + "-" + 
                  Player.Parametri.Velocità + "-" + Player.PossoSparare.ToString() + "-" + Player.ProiettileCheSparo.Name);
            sw.Close();
        }

        #endregion
    }    
}