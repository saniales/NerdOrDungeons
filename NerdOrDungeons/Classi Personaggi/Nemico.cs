using System;
using System.Timers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace NerdOrDungeons
{
    /**                                                              **
     ******************************************************************
     **                                                              **
     ** Nemico --> Eredita da Agente :                               **
     ** Qui Gestisco L'intelligenza Artificiale                      **
     **                                                              **
     **                   [Roberto Mana Docet]                       **
     **                                                              **
     ******************************************************************
     **                                                              **/

    public class Nemico : Agente
    {               
        #region Modelli Nemici Precostruiti

        protected static string DefaultPath;

        public static Nemico FightingPuppet;  // 'F'

        //Standard Enemies
        public static Nemico OctorockRed;     // 'R'
        public static Nemico Snake;           // 'S'
        public static Nemico Goomba;          // 'G'
        public static Nemico EnemyShip;       // 'H'
        public static Nemico WaterTank;       // 'I'
        public static Nemico EarthTank;       // 'J'

        //Bosses
        public static Nemico MegaOctorockRed; // 'M'
        public static Nemico Prematurator;    // 'L' 

        static Nemico()
        {
            DefaultPath = @".\Content\Enemies\";

            FightingPuppet  = new Nemico(MainGame.RiferimentoGlobaleAlGioco, Vector2.Zero, ParametriPersonaggio.Empty                 , DefaultPath + "FightingPuppet" , null                     , TipoIA.Assente, 0);

            OctorockRed     = new Nemico(MainGame.RiferimentoGlobaleAlGioco, Vector2.Zero, ParametriPersonaggio.OctorockRed.Copy()    , DefaultPath + "OctorockRed"    , Proiettile.Types["Rock"]      , TipoIA.Casuale, 0 ); 
            Snake           = new Nemico(MainGame.RiferimentoGlobaleAlGioco, Vector2.Zero, ParametriPersonaggio.Snake.Copy()          , DefaultPath + "Snake"          , null                          , TipoIA.Insegui, 500 );
            Goomba          = new Nemico(MainGame.RiferimentoGlobaleAlGioco, Vector2.Zero, ParametriPersonaggio.Goomba.Copy()         , DefaultPath + "Goomba"         , null                          , TipoIA.Insegui, 300 );
            EnemyShip      = new Nemico(MainGame.RiferimentoGlobaleAlGioco, Vector2.Zero, ParametriPersonaggio.EnemyShip1.Copy()     , DefaultPath + "EnemyShip1"     , Proiettile.Types["CannonBall"], TipoIA.Casuale, 0);

            MegaOctorockRed = new Nemico(MainGame.RiferimentoGlobaleAlGioco, Vector2.Zero, ParametriPersonaggio.MegaOctorockRed.Copy(), DefaultPath + "MegaOctorockRed", Proiettile.Types["Rock"]      , TipoIA.Insegui, 800 );
            Prematurator    = new Nemico(MainGame.RiferimentoGlobaleAlGioco, Vector2.Zero, ParametriPersonaggio.Prematurator.Copy()   , DefaultPath + "Prematurator"   , Proiettile.Types["EnergyBall"], TipoIA.Insegui, 1000);
            WaterTank       = new Nemico(MainGame.RiferimentoGlobaleAlGioco, Vector2.Zero, ParametriPersonaggio.WaterTank.Copy()      , DefaultPath + "WaterTank"      , Proiettile.Types["CannonBall"], TipoIA.Casuale, 0);
            EarthTank       = new Nemico(MainGame.RiferimentoGlobaleAlGioco, Vector2.Zero, ParametriPersonaggio.WaterTank.Copy()      , DefaultPath + "EarthTank"      , Proiettile.Types["CannonBall"], TipoIA.Casuale, 0);
        }    

        #endregion

        #region Variabili

        protected KeyboardState DirezioneSceltaIA;
        protected Timer GestisciPassi;
        protected Timer TimerIA;
        protected Random GeneratoreCasualeDirezioni;
        protected DirezioneCorrente DirezioneCorrente;
        internal  TipoIA IA;
        internal  Rectangle CampoVisivo;

        public Agente chiInseguo;

        #endregion

        #region Costruttori + Metodi Appoggio

        public Nemico(Game Game, Vector2 Posizione, ParametriPersonaggio Parametri, string SpriteFolderPath, Proiettile Projectile, TipoIA IA, int CampoVisivo)
            : base(Game, Posizione, Parametri, SpriteFolderPath, Projectile)
        {
            this.IA = IA;
            this.PossoSparare = true;
            this.CampoVisivo = ImpostaCampoVisivo(CampoVisivo);
            this.DirezioneCorrente = DirezioneCorrente.Fermo;
            Inizializza();
        }

        protected Nemico(Nemico n, Vector2 Posizione) 
            : this(n.Game, Posizione, n.Parametri, n.SpriteFolderPath, n.ProiettileCheSparo, n.IA, n.CampoVisivo.Height) { }

        //Funzioni Per Copia (Utile Per Modelli preconfigurati)
        public Nemico Copy(Vector2 Posizione)
        { return new Nemico(this, Posizione); }

        public Nemico Copy()
        { return new Nemico(this, this.Posizione); }

        private void Inizializza()
        {
            GeneratoreCasualeDirezioni = new Random();

            this.TimerIA = new Timer();
            this.GestisciPassi = new Timer(600); // gestisce il cambio sprite dei passi
            // Non Faccio Muovere In Modo Sincronizzato I Nemici, Non Ha Senso
            this.TimerIA.Interval = GeneratoreCasualeDirezioni.Next(8, 12) * 100;

            this.GestisciPassi.AutoReset = true;
            this.GestisciPassi.Elapsed += new ElapsedEventHandler(RiaggiornaMovimento);
            this.TimerIA.AutoReset = true;
            this.TimerIA.Elapsed += new ElapsedEventHandler(CambiaDirezioneCasualmente);

            GestisciPassi.Start();
            TimerIA.Start();

        }

        #endregion

        #region Metodi Movimento, IA e affini

        //ogni Tot Mi Sposto in una Direzione
        protected virtual void GestisciIACasuale(GameTime g)
        { ImpostaMovimenti(this.DirezioneCorrente, g); }

        //insegue Personaggio o Alleato
        //Sarà Reimplementato per Alleato che eredita da Nemico
        protected void GestisciIAInsegui(Agente Agent, GameTime g) 
        {
            if (Rectangle.Intersect(this.CampoVisivo, Agent.Bordo) != Rectangle.Empty)
            {
                if ((this.Posizione.Y + this.Current.Height > Agent.Posizione.Y) &&
                    (this.Posizione.Y - this.Current.Height < Agent.Posizione.Y)   )
                {
                    //se sono a destra mi muovo a sinistra (o Sparo) 
                    if (this.Posizione.X > Agent.Posizione.X)
                        if (GeneratoreCasualeDirezioni.Next(0, 60) < 1)
                            this.DirezioneCorrente = DirezioneCorrente.Spara;
                        else
                            this.DirezioneCorrente = DirezioneCorrente.Sinistra;
                    //se sono a sinistra mi muovo a destra (o Sparo)
                    if (this.Posizione.X < Agent.Posizione.X)
                        if (GeneratoreCasualeDirezioni.Next(0, 60) < 1)
                            this.DirezioneCorrente = DirezioneCorrente.Spara;
                        else                             
                            this.DirezioneCorrente = DirezioneCorrente.Destra;
                }
                if ((this.Posizione.X + this.Current.Width > Agent.Posizione.X) &&
                    (this.Posizione.X - this.Current.Width < Agent.Posizione.X))
                {
                    //se sono sotto mi muovo verso l'alto (o Sparo)
                    if (this.Posizione.Y > Agent.Posizione.Y)
                        if (GeneratoreCasualeDirezioni.Next(0, 60) < 1)
                            this.DirezioneCorrente = DirezioneCorrente.Spara;
                        else                                
                            this.DirezioneCorrente = DirezioneCorrente.Su;
                    //se sono sopra mi muovo verso il basso (o Sparo)
                    if (this.Posizione.Y < Agent.Posizione.Y)
                        if (GeneratoreCasualeDirezioni.Next(0, 60) < 1)
                            this.DirezioneCorrente = DirezioneCorrente.Spara;
                        else                              
                            this.DirezioneCorrente = DirezioneCorrente.Giù;
                }
            }
            ImpostaMovimenti(this.DirezioneCorrente, g);
        }

        protected void GestisciIAScappa(Agente Agent, GameTime g)  
        {
            if (Rectangle.Intersect(this.CampoVisivo, Agent.Bordo) != Rectangle.Empty)
            {
                if ((this.Posizione.Y + this.Current.Height > Agent.Posizione.Y) &&
                    (this.Posizione.Y - this.Current.Height < Agent.Posizione.Y))
                {
                    //se sono a destra mi muovo a destra
                    if (this.Posizione.X > Agent.Posizione.X)        
                            this.DirezioneCorrente = DirezioneCorrente.Destra;  
                    //se sono a sinistra mi muovo a sinistra
                    if (this.Posizione.X < Agent.Posizione.X)  
                        this.DirezioneCorrente = DirezioneCorrente.Sinistra; 
                }
                if ((this.Posizione.X + this.Current.Width > Agent.Posizione.X) &&
                    (this.Posizione.X - this.Current.Width < Agent.Posizione.X))
                {
                    //se sono sotto mi muovo verso il basso 
                    if (this.Posizione.Y > Agent.Posizione.Y) 
                        this.DirezioneCorrente = DirezioneCorrente.Giù;   
                    //se sono sotto mi muovo verso l'alto
                    if (this.Posizione.Y < Agent.Posizione.Y)        
                            this.DirezioneCorrente = DirezioneCorrente.Su;   
                }
            }
            ImpostaMovimenti(this.DirezioneCorrente, g);
        }

        public void ApplicaIA(GameTime g) 
        {
            switch (IA)
            {
                case TipoIA.Assente:
                case TipoIA.Alleato:
                    break;
                case TipoIA.Casuale:
                    GestisciIACasuale(g);
                    break;
                case TipoIA.Insegui:
                    GestisciIAInsegui(chiInseguo, g);
                    break;
                case TipoIA.Scappa:
                    GestisciIAScappa(chiInseguo, g);
                    break;
            }
        }

        #endregion

        #region Metodi ereditati
                                                                                  
        public override void Update(GameTime gameTime)  
        {
            if (this.Game.Level != null && this.IA == TipoIA.Insegui) //ho finito di inizializzare il livello (fine costruttore)
                this.chiInseguo = this.Game.Level.Player;
            if (this != null)
            {
                ApplicaIA(gameTime);
                this.CampoVisivo = ImpostaCampoVisivo(this.CampoVisivo.Width);
                base.Update(gameTime);
            }
        }

        protected override void Dispose(bool disposing) 
        {
            TimerIA.Dispose();
            GestisciPassi.Dispose();
            base.Dispose(disposing);
        }

        #endregion

        #region Procedure Di Evento

        protected void CambiaDirezioneCasualmente(object sender, ElapsedEventArgs e)
        {
            //Genera Casualmente Numeri, Così Da Cambiare Direzione In Base Al Numero
            // 0 -> Fermo
            // 1 -> Su
            // 2 -> Giù
            // 3 -> Sinistra
            // 4 -> Destra
            // 5 -> Spara, poi Fermo
            // Rnd.Next() < 0 OR Rnd.Next() > 5 -> throw InvalidOperationException("Valore Non Valido");
            int x = GeneratoreCasualeDirezioni.Next(0, 6);
            this.DirezioneCorrente = (DirezioneCorrente)x;
            this.CampoVisivo = ImpostaCampoVisivo(this.CampoVisivo.Height);
        }

        #endregion

        #region Metodi Appoggio Minori

        /* Costruisce Un Rettangolo Con Centro Uguale   *
         * Al Centro Dello Sprite Del Nemico E Permette *
         * Di Usarlo Come Campo Visivo (Intersects)     */
        protected Rectangle ImpostaCampoVisivo(int Ampiezza)
        {
            int X, Y;
                                 
            //XCampoVisivoRectangle + campovisivo/2 = XBordo + this.current.width / 2 -> xCampoVisivo = XBordo - campoVisivo/2 + this.current.Width/2
            X = (int)(this.Bordo.X - Ampiezza / 2 + this.Current.Width / 2);
            //yCampoVisivoRectangle + campovisivo/2 = yBordo + this.current.Height / 2 -> yCampoVisivo = YBordo - campoVisivo/2 + this.current.Height/2
            Y = (int)(this.Bordo.Y - Ampiezza / 2 + this.Current.Height / 2);

            return new Rectangle(X, Y, Ampiezza, Ampiezza);
        }

        #endregion
    }
}