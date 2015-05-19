using System.Collections.Generic;
using System.IO;
using System.Timers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace NerdOrDungeons
{
    /**                                                              **
     ******************************************************************
     **                                                              **
     ** Agente:                                                      **
     ** SuperClasse di Personaggio, Nemico e Alleato                 **
     ** Qui Gestisco come carico gli sprite, come ridisegno e come   **
     ** eseguo le animazioni : insomma le cose comuni a tutti gli    **
     ** oggetti agenti nel Game.                                     **
     **                                                              **
     ** Classe Di Base.                                              **
     **                                                              **
     **      [Roberto Mana Dixit Illvd Ac Sequor Doctrinæ Suæ]       **
     **                                                              **
     ******************************************************************
     **                                                              **/

    public abstract class Agente : DrawableGameComponent
    {
        #region Dichiarazione Costanti Movimento

        static public int WAIT_DOWN = 0, WAIT_UP = 1, WAIT_LEFT = 2, WAIT_RIGHT = 3, UP_1 = 4, UP_2 = 5, DOWN_1 = 6;
        static public int DOWN_2 = 7, LEFT_1 = 8, LEFT_2 = 9, RIGHT_1 = 10, RIGHT_2 = 11, ATTACK_UP = 12, ATTACK_DOWN = 13;
        static public int ATTACK_LEFT = 14, ATTACK_RIGHT = 15, DEAD = 16, OTHER = 17;

        /** WAIT_DOWN   -> Fermo Girato Verso Il Basso
         ** DOWN_1      -> Sprite 1 : Camminare Verso Il Basso (Piede Dx)
         ** DOWN_2      -> Sprite 2 : Camminare Verso Il Basso (Piede Sx)
         ** ATTACK_DOWN -> Sprite Attacco : Girato Verso Il Basso (Spada)
         ** ecc...
         ***/

        #endregion

        #region Dichiarazione Variabili
        
        public        Vector2              OldPosizione; //per gestire le collisioni
        public        ParametriPersonaggio Parametri;
        #region public        Vector2              Posizione;
        // Variabile Wrapper Di this.posizione e this.Bordo (per Gli Spostamenti)
        public Vector2 Posizione
        {
            get { return this.posizione; }
            set
            {
                this.posizione = value;
                this.Bordo.X = (int)value.X;
                this.Bordo.Y = (int)value.Y;
                this.Bordo.Height = this.Current.Height;
                this.Bordo.Width = this.Current.Width;
            }
        } 
        #endregion 
        public        string               Name;

        internal      GestoreProiettili    ListaProiettili;
        internal      Rectangle            Bordo;
        internal      List<Sprite>         Sprites;
        internal      Sprite               Current;
        internal      Proiettile           ProiettileCheSparo;
        internal      bool                 DisabilitaMovimenti;
        internal      bool                 PossoSparare;

        //usata Solo In Ambito Privato (Per Ambito Pubblico Uso Solo La Variabile Wrapper "Posizione") 
        protected     Vector2              posizione;
        protected new MainGame             Game;
        protected     Timer                t;
        protected     Timer                t2;
        protected     string               SpriteFolderPath;
        

        private       bool                 SwordTimeExceeded;
        private       bool                 StoSparando;
        private       bool                 StoRiattivandoIlTimer;
        private       bool                 SwordAttacking;
        private       bool                 PassoADestra;

        #endregion

        #region Costruttore + Metodi Di Appoggio Del Costruttore

        public Agente(Game Game, Vector2 Posizione, ParametriPersonaggio Parameters, string SpriteFolderPath, Proiettile Projectile)
            : base(Game)
        {
            this.Game = (MainGame)Game;
            //Al Primo Giro Non Ci Sono Cambiamenti, La Variabile Wrapper non Serve...
            this.posizione = Posizione; 
            this.OldPosizione = Posizione;
            this.Parametri = Parameters.Copy();
            this.SpriteFolderPath = SpriteFolderPath;
            this.ProiettileCheSparo = Projectile;

            //Impostazione Flags
            this.SwordTimeExceeded     = false;
            this.SwordAttacking        = false;
            this.PossoSparare          = false;
            this.StoSparando           = false;
            this.DisabilitaMovimenti   = false;
            this.StoRiattivandoIlTimer = false;
            this.PassoADestra          = false;
            this.Name                  = SpriteFolderPath.Substring(SpriteFolderPath.LastIndexOf(@"\") + 1);

            Inizializza();
        }

        //COSTRUTTORE PER COPIA (Serve per Farmi i Modelli Precostruiti)
        protected Agente(Agente Agente, Vector2 Posizione)
            : this(Agente.Game, Posizione, Agente.Parametri, Agente.SpriteFolderPath, null) { }

        private void LoadSprites(string path) // carica tutti gli sprite del personaggio / nemico a partire dalla cartella dove si trovano le immagini 
        {
            /** scandisce la cartella del personaggio e carica gli sprite relativi 
             ** es. [gioco]Content/[NOME_PERSONAGGIO]/000_WAIT.png, ecc...
             ** /

            /** Pattern:
             ** 000_WAITDOWN.png, 001_WAITUP.png, 002_WAITLEFT.png, 003_WAITRIGHT.png, 004_UP1.png, 005_UP2.png,
             ** 006_DOWN1.png, 007_DOWN2.png, 008_LEFT1.png, 009_LEFT2.png, 
             ** 010_RIGHT1.png, 011_RIGHT2.png, 012_ATTACK_UP.png, 013_ATTACK_DOWN.png, 
             ** 014_ATTACK_LEFT.png, 015_ATTACK_RIGHT.png
             **/

            string[] Image_paths = Directory.GetFiles(path, "*.png");
            foreach (string s in Image_paths)
                Sprites.Add(new Sprite(this.Game, s));
            Current = Sprites[WAIT_DOWN];
            this.Bordo = new Rectangle((int)this.Posizione.X, (int)this.Posizione.Y, this.Current.Width, this.Current.Height);
        }

        private void Inizializza() //a differenza di Initialize() questo Metodo è Caricato A Ogni Istanza
        {
            this.Sprites = new List<Sprite>();
            this.ListaProiettili = new GestoreProiettili(this);

            this.t = new Timer(150);
            this.t2 = new Timer(500);
            this.t.AutoReset = true;
            this.t.Elapsed += new ElapsedEventHandler(RiaggiornaMovimento);
            this.t2.AutoReset = false;
            this.t2.Elapsed += new ElapsedEventHandler(ReimpostaSwordAttack);

            this.LoadSprites(this.SpriteFolderPath);
        }

        #endregion

        #region Funzioni Inerenti La Coordinazione Tasti/Movimenti Sprite

        private void ImpostaMovimento(GameTime g, KeyboardState k) //Muovo e cambio gli sprite di conseguenza 
        {
            /** NELLA FUNZIONE MI CHIEDO SE t = Enabled PER VEDERE SE MI STO GIA MUOVENDO (INDI PER CUI
             ** GLI EVENTI DI 'AggiornaMovimento' Non Vanno Disturbati
             ****/
            Vector2 Direzione = Vector2.Zero;

            if (k.IsKeyDown(Keys.C))
                //se posso sparare e sono fermo
                if (this.Posizione == this.OldPosizione)
                    Spara();

            if (k.IsKeyDown(Keys.Right))
            {
                Direzione.X += Parametri.Velocità;
                if (t.Enabled == false)
                {
                    Current = Sprites[RIGHT_1];
                    t.Start();
                }
            }
            if (k.IsKeyDown(Keys.Left))
            {
                Direzione.X -= Parametri.Velocità;
                if (t.Enabled == false)
                {
                    Current = Sprites[LEFT_1];
                    t.Start();
                }
            }
            if (k.IsKeyDown(Keys.Up))
            {
                Direzione.Y -= Parametri.Velocità;
                if (t.Enabled == false)
                {
                    Current = Sprites[UP_1];
                    t.Start();
                }
            }
            if (k.IsKeyDown(Keys.Down))
            {
                Direzione.Y += Parametri.Velocità;
                if (t.Enabled == false)
                {
                    Current = Sprites[DOWN_1];
                    t.Start();
                }
            }

            if (this is Personaggio)
                if (k.IsKeyDown(Keys.Space) && !SwordTimeExceeded)  //attacco (dipende da come sono girato)
                {
                    t.Stop();

                    if (!SwordAttacking)
                        SoundEngine.Effects["PlayerSwordAttack"].Play();
                    SwordAttacking = true;
                    if (Current == Sprites[WAIT_DOWN] || Current == Sprites[DOWN_1] || Current == Sprites[DOWN_2])
                        Current = Sprites[ATTACK_DOWN];
                    if (Current == Sprites[WAIT_UP] || Current == Sprites[UP_1] || Current == Sprites[UP_2])
                        Current = Sprites[ATTACK_UP];
                    if (Current == Sprites[WAIT_LEFT] || Current == Sprites[LEFT_1] || Current == Sprites[LEFT_2])
                        Current = Sprites[ATTACK_LEFT];
                    if (Current == Sprites[WAIT_RIGHT] || Current == Sprites[RIGHT_1] || Current == Sprites[RIGHT_2])
                        Current = Sprites[ATTACK_RIGHT];

                    if (!StoRiattivandoIlTimer) // se riattivo il timer non riusciro mai a attivare l'evento ReimpostaSwordAttack
                    {
                        t2.Start();
                        StoRiattivandoIlTimer = true; // per il prossimo giro...
                    }
                }
            Muovi(Direzione);
        }

        private void ReimpostaDopoMovimento(KeyboardState k) //Reimposta gli Sprites su "WAIT" dopo aver rilasciato il tasto / essermi mosso / attaccato 
        {
            if (k.IsKeyUp(Keys.C))
                ReimpostaDopoAverSparato();

            if (k.IsKeyUp(Keys.Right))
                if (Current == Sprites[RIGHT_1] || Current == Sprites[RIGHT_2])
                {
                    t.Stop();
                    Current = Sprites[WAIT_RIGHT];
                }
            if (k.IsKeyUp(Keys.Left))
                if (Current == Sprites[LEFT_1] || Current == Sprites[LEFT_2])
                {
                    t.Stop();
                    Current = Sprites[WAIT_LEFT];
                }
            if (k.IsKeyUp(Keys.Up))
                if (Current == Sprites[UP_1] || Current == Sprites[UP_2])
                {
                    t.Stop();
                    Current = Sprites[WAIT_UP];
                }
            if (k.IsKeyUp(Keys.Down))
                if (Current == Sprites[DOWN_1] || Current == Sprites[DOWN_2])
                {
                    t.Stop();
                    Current = Sprites[WAIT_DOWN];
                }

            if (this is Personaggio)
                if ((k.IsKeyUp(Keys.Space) && !StoSparando) || (SwordTimeExceeded == true))
                {
                    if (k.IsKeyUp(Keys.Space))
                        SwordTimeExceeded = false;
                    if (Current == Sprites[ATTACK_DOWN])
                        Current = Sprites[WAIT_DOWN];
                    if (Current == Sprites[ATTACK_UP])
                        Current = Sprites[WAIT_UP];
                    if (Current == Sprites[ATTACK_LEFT])
                        Current = Sprites[WAIT_LEFT];
                    if (Current == Sprites[ATTACK_RIGHT])
                        Current = Sprites[WAIT_RIGHT];
                    StoRiattivandoIlTimer = false;
                    SwordAttacking = false;
                    t2.Stop();
                }
        }

        protected void ImpostaMovimenti(DirezioneCorrente SimulataOReale, GameTime g)
        {
            KeyboardState k;
            switch (SimulataOReale)
            {
                default:
                    throw new ArgumentException("Comando assegnato a Nemico.ImpostaMovimenti() Non Corretto");
                case DirezioneCorrente.Manuale:
                    k = Keyboard.GetState();
                    break;
                case DirezioneCorrente.Sinistra:
                    k = new KeyboardState(Keys.Left);
                    break;
                case DirezioneCorrente.Destra:
                    k = new KeyboardState(Keys.Right);
                    break;
                case DirezioneCorrente.Su:
                    k = new KeyboardState(Keys.Up);
                    break;
                case DirezioneCorrente.Giù:
                    k = new KeyboardState(Keys.Down);
                    break;
                case DirezioneCorrente.Attacco:
                    k = new KeyboardState(Keys.Space);
                    break;
                case DirezioneCorrente.Spara:
                    k = new KeyboardState(Keys.C); // TASTO PER SPARARE
                    break;
                case DirezioneCorrente.Fermo:
                    k = new KeyboardState(Keys.None);
                    break;
            }
            ReimpostaDopoMovimento(k);
            ImpostaMovimento(g, k);
        }

        #endregion

        #region Eventi

        private void ReimpostaSwordAttack(object sender, ElapsedEventArgs e) 
        { this.SwordTimeExceeded = true; }

        //Evento che permette di camminare (SpriteChange + Timer)
        protected void RiaggiornaMovimento(object sender, ElapsedEventArgs e) 
        {
            KeyboardState k = Keyboard.GetState();

            if (!k.IsKeyDown(Keys.Space))
            {
                //Riaggiorno i frame (esempio : quando cammino alterno piede dx e sx)
                if (Current == Sprites[DOWN_1] || Current == Sprites[DOWN_2])
                {
                    Current = Sprites[WAIT_DOWN];
                    return;
                }
                if (Current == Sprites[UP_1] || Current == Sprites[UP_2])
                {
                    Current = Sprites[WAIT_UP];
                    return;
                }
                if (Current == Sprites[RIGHT_1] || Current == Sprites[RIGHT_2])
                {
                    Current = Sprites[WAIT_RIGHT];
                    return;
                }
                if (Current == Sprites[LEFT_1] || Current == Sprites[LEFT_2])
                {
                    Current = Sprites[WAIT_LEFT];
                    return;
                }
                if (Current == Sprites[WAIT_DOWN])
                {
                    if (PassoADestra)
                        Current = Sprites[DOWN_1];
                    else
                        Current = Sprites[DOWN_2];
                    PassoADestra = !PassoADestra;
                    return;
                }
                if (Current == Sprites[WAIT_UP])
                {
                    if (PassoADestra)
                        Current = Sprites[UP_1];
                    else
                        Current = Sprites[UP_2];
                    PassoADestra = !PassoADestra;
                    return;
                }
                if (Current == Sprites[WAIT_LEFT])
                {
                    if (PassoADestra)
                        Current = Sprites[LEFT_1];
                    else
                        Current = Sprites[LEFT_2];
                    PassoADestra = !PassoADestra;
                    return;
                }
                if (Current == Sprites[WAIT_RIGHT])
                {
                    if (PassoADestra)
                        Current = Sprites[RIGHT_1];
                    else
                        Current = Sprites[RIGHT_2];
                    PassoADestra = !PassoADestra;
                    return;
                }
            }
        }

        #endregion

        #region Funzioni Collisioni

        internal bool CheckCollision(OggettoInanimato obj) 
        { return this.Bordo.Intersects(obj.Bordo); }

        internal bool CheckCollision(Agente Nemico) 
        { return this.Bordo.Intersects(Nemico.Bordo); }

        #endregion

        #region Membri Ereditati

        public override void Draw(GameTime gameTime)
        {
            this.Game.SpriteBatch.Draw(this.Current.texture, this.Posizione, Color.White);
            this.ListaProiettili.Draw(gameTime);
            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            this.ListaProiettili.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Dispose(bool disposing) //Rilascia le Risorse Non Utilizzate 
        {
            t.Dispose();  //cancello i timer
            t2.Dispose();

            foreach (Sprite s in Sprites) //prima cancello i singoli sprite
                s.Dispose();
            Sprites.Clear(); //Dopo il riferimento alla lista

            ListaProiettili.Dispose();
            base.Dispose(disposing);
        }

        #endregion

        #region Funzioncine Di Appoggio Minori (Scambio Sprite e Disegno Sprite)

        //Cancella e Ridisegna lo Sprite Corrente nella Nuova Posizione (Corregge Anche I Bordi)
        protected void Muovi(Vector2 _direzione) 
        {
            this.OldPosizione = this.Posizione;
            this.Posizione += _direzione;
        }

        protected void Spara() 
        {
            if (PossoSparare && !StoSparando)
                if (this.Current != Sprites[DEAD] && this.ProiettileCheSparo != null)
                {

                    if (this.Current == Sprites[WAIT_DOWN])
                        this.Current = Sprites[ATTACK_DOWN];
                    if (this.Current == Sprites[WAIT_UP])
                        this.Current = Sprites[ATTACK_UP];
                    if (this.Current == Sprites[WAIT_LEFT])
                        this.Current = Sprites[ATTACK_LEFT];
                    if (this.Current == Sprites[WAIT_RIGHT])
                        this.Current = Sprites[ATTACK_RIGHT];
                    if (this is Personaggio)
                    {
                        Personaggio p = (Personaggio)this;
                        if (p.isVehicle)
                            SoundEngine.Effects["CannonShot"].Play();
                        else
                        {
                            SoundEngine.Effects["PlayerSwordAttack"].Play();
                            SoundEngine.Effects["PlayerShot"].Play();
                        }
                    }

                    this.StoSparando = true;
                    this.ListaProiettili.Add(ProiettileCheSparo);
                }
        }

        protected void ReimpostaDopoAverSparato() 
        {
            if (StoSparando)
            {
                if (this.Current == Sprites[ATTACK_RIGHT])
                    this.Current =  Sprites[WAIT_RIGHT];
                if (this.Current == Sprites[ATTACK_LEFT])
                    this.Current =  Sprites[WAIT_LEFT];
                if (this.Current == Sprites[ATTACK_UP])
                    this.Current =  Sprites[WAIT_UP];
                if (this.Current == Sprites[ATTACK_DOWN])
                    this.Current =  Sprites[WAIT_DOWN];
                StoSparando = false;
            }
        }

        #endregion
    }
}