using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace NerdOrDungeons
{
    /**                                                              **
     ******************************************************************
     **                                                              **
     ** Interruttore -> Eredita da OggettoInanimato :                **
     ** Classe Che Rappresenta Fisicamente Un Interruttore Nello     **
     ** Scenario : Se Lo Premo Fa Sparire O Apparire Dei             **
     ** MuriRimovibili A cui E' Legato.                              **
     **                                                              **
     ******************************************************************
     **                                                              **/

    public sealed class Interruttore : OggettoInanimato
    {
        #region Variabili Preconfigurate

        private static string DefaultPath;

        public  static Interruttore Tipo1;
        public  static Interruttore Tipo2;
        public  static Interruttore Tipo3;

        static Interruttore()
        {
            DefaultPath = @".\Content\Switches\";

            Tipo1 = new Interruttore(MainGame.RiferimentoGlobaleAlGioco, Vector2.Zero, DefaultPath + @"Tipo1\", true);
            Tipo2 = new Interruttore(MainGame.RiferimentoGlobaleAlGioco, Vector2.Zero, DefaultPath + @"Tipo2\", true);
            Tipo3 = new Interruttore(MainGame.RiferimentoGlobaleAlGioco, Vector2.Zero, DefaultPath + @"Tipo3\", true); 
        }

        #endregion

        #region Dichiarazione Variabili

        internal List<MuroRimovibile> MuriLegati;    // ! , " dipende dall'interruttore
        
        private  Sprite  Sprite;
        public   bool    Touched; // dice se al frame prima toccavo gia l'interruttore
        public   string  SpritePath;
        public   bool    isActive;
        #region public   Vector2 Posizione (Get + Set)

        public Vector2 Posizione 
        {
            get
            {
                return this.Sprite.Posizione;
            }
            set
            {
                this.Sprite.Posizione = value;
                this.Bordo.X = (int)value.X;
                this.Bordo.Y = (int)value.Y;
            }
        }

        #endregion
        #endregion

        #region Costruttore

        public Interruttore(Game Game, Vector2 Posizione, string SpritePath, bool Active)
            : base(Game, Posizione)
        {
            this.MuriLegati = new List<MuroRimovibile>();
            this.SpritePath = SpritePath;
            this.Touched    = false;
            this.isActive   = Active;
            this.Sprite = new Sprite(this.Game, SpritePath + (isActive? 0 : 1) + ".png");
            this.Posizione = Posizione;
        }

        private Interruttore(Interruttore i)
        : this(i.Game, i.Posizione, i.SpritePath, i.isActive) { }

        public Interruttore Copy(Vector2 Posizione)
        { return new Interruttore(this); }

        public void Switch() // Fa Apparire E Sparire I Muri Legati
        {
                isActive = !isActive;
                this.Sprite.Dispose();
                //carico lo sprite (che è diverso se è attivato l'interruttore o no)
                this.Sprite = new Sprite(this.Game, SpritePath + (isActive ? 0 : 1) + ".png", Posizione);
                SoundEngine.Effects["SwitchPressed"].Play();

                this.Touched = true;
        }

        public void AddDependingWall(MuroRimovibile Item)
        { this.MuriLegati.Add(Item); }

        public void Reset() // non cancella il riferimento e mantiene lo sprite 
        {
            this.MuriLegati.Clear();
            this.MuriLegati = new List<MuroRimovibile>();
        }

        #endregion

        #region Metodi Ereditati

        public override void Draw(GameTime gameTime)
        {
            if (this != null)
            {
                this.Sprite.Draw(gameTime);
                if (this.isActive)
                    foreach (MuroRimovibile m in MuriLegati)
                        m.Draw(gameTime);
            }
        }

        // per evitare la chiamata a Switch() a ogni update
        public bool CheckCollision(Personaggio player)
        { return this.Bordo.Intersects(player.Bordo); }
        
        public bool CheckCollision(Proiettile p)
        { return p.CheckCollision(this); }

        public bool CheckSwitch(Personaggio Player)
        {
            if (CheckCollision(Player) && !Touched)
            {
                Switch();
                return true;
            }
            else if (!CheckCollision(Player))
            {
                Touched = false;
                return false;
            }
            else
                return false;
        }

        public bool CheckSwitch(Proiettile p)
        {
            if (CheckCollision(p))
            {
                Switch();
                return true;
            }
            else
                return false;
        }

        protected override void Dispose(bool disposing) // cancella il riferimento
        {
            this.isActive = false;

            MuriLegati.Clear();
            this.Sprite.Dispose();
            base.Dispose(disposing);
        }

        public override void Update(GameTime gameTime)
        {
            if (isActive)
            {
                foreach (MuroRimovibile m in this.MuriLegati)
                    m.Update(gameTime);
                base.Update(gameTime);
            }
        }

        #endregion

       
    }
}
