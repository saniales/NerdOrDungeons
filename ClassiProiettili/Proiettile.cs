using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;

namespace NerdOrDungeons
{
   /**                                                              **
    ******************************************************************
    **                                                              **
    ** Proiettile:                                                  **
    ** Agente particolare : Ha un Unico Parametro "Attacco", Se     **
    ** Collide Sparisce e se chi colpisce è un "Personaggio" Gli    **
    ** Toglie Health.                                               **
    ** Se Legato A Personaggio o Alleato Toglie vita Ai Nemici.     **
    ** Se Legato A Nemico Toglie Vita a Personaggio o Alleato.      **
    **                                                              **
    ******************************************************************
    **                                                              **/

    public sealed class Proiettile : DrawableGameComponent
    {  
        #region Costanti Precaricate

        private  static string  DefaultPath;

        internal static Vector2 DirezioneSu;
        internal static Vector2 DirezioneGiù;
        internal static Vector2 DirezioneSinistra;
        internal static Vector2 DirezioneDestra;

        //Contenitore Di Tutti i Tipi Di Proiettili Sparabili Da Tutti
        public static Dictionary <string,Proiettile> Types;

        internal static void InizializzaClasse()
        {
            DefaultPath       = @".\Content\Projectiles\";

            DirezioneSu       = new Vector2(0, -1);
            DirezioneGiù      = new Vector2(0, +1);
            DirezioneSinistra = new Vector2(-1, 0);
            DirezioneDestra   = new Vector2(+1, 0);

            Types = new Dictionary<string, Proiettile>();

            //Player
            Types.Add("BlueLaser"        , new Proiettile(MainGame.RiferimentoGlobaleAlGioco, DefaultPath + "BlueLaser"      , ParametriProiettile.BlueLaser.Copy() , Vector2.Zero, Vector2.Zero, null));
            Types.Add("RedLaser"         , new Proiettile(MainGame.RiferimentoGlobaleAlGioco, DefaultPath + "RedLaser"       , ParametriProiettile.RedLaser.Copy()  , Vector2.Zero, Vector2.Zero, null));
            Types.Add("GreenLaser"       , new Proiettile(MainGame.RiferimentoGlobaleAlGioco, DefaultPath + "GreenLaser"     , ParametriProiettile.GreenLaser.Copy(), Vector2.Zero, Vector2.Zero, null));
            Types.Add("CannonBall"       , new Proiettile(MainGame.RiferimentoGlobaleAlGioco, DefaultPath + "CannonBall"     , ParametriProiettile.CannonBall.Copy(), Vector2.Zero, Vector2.Zero, null));
                   
            Types.Add("FlameShot"        , new Proiettile(MainGame.RiferimentoGlobaleAlGioco, DefaultPath + "FlameShot"      , ParametriProiettile.FlameShot.Copy() , Vector2.Zero, Vector2.Zero, null));
            Types.Add("DarkFlameShot"    , new Proiettile(MainGame.RiferimentoGlobaleAlGioco, DefaultPath + "DarkFlameShot"  , ParametriProiettile.FlameShot.Copy() , Vector2.Zero, Vector2.Zero, null));
            Types.Add("PurpleFlameShot"  , new Proiettile(MainGame.RiferimentoGlobaleAlGioco, DefaultPath + "PurpleFlameShot", ParametriProiettile.FlameShot.Copy() , Vector2.Zero, Vector2.Zero, null));
            Types.Add("RedFlameShot"     , new Proiettile(MainGame.RiferimentoGlobaleAlGioco, DefaultPath + "RedFlameShot"   , ParametriProiettile.FlameShot.Copy() , Vector2.Zero, Vector2.Zero, null));

            //Enemies, Boss & Allies
            Types.Add("Rock"             , new Proiettile(MainGame.RiferimentoGlobaleAlGioco, DefaultPath + "Rock"           , ParametriProiettile.Rock.Copy()       , Vector2.Zero, Vector2.Zero, null));

            Types.Add("EnergyBall"       , new Proiettile(MainGame.RiferimentoGlobaleAlGioco, DefaultPath + "EnergyBall"     , ParametriProiettile.EnergyBall.Copy() , Vector2.Zero, Vector2.Zero, null));
            Types.Add("Spear"            , new Proiettile(MainGame.RiferimentoGlobaleAlGioco, DefaultPath + "Spear"          , ParametriProiettile.Spear.Copy()      , Vector2.Zero, Vector2.Zero, null));
        }

        #endregion

        #region Dichiarazione Variabili

        public       ParametriProiettile Parametri;
        public       string              Name;

        internal     Vector2 Position;
        internal     Vector2 Direction;
        internal     List<Sprite> Sprites;
        internal     Sprite Current;
        internal     Agente Owner;
       
        private  new MainGame            Game;
        private      string              SpriteFolderPath;
        private      Rectangle           Bordo;

        #endregion

        #region Costruttore

        public Proiettile(Proiettile Projectile)
        : this(Projectile.Game, Projectile.SpriteFolderPath, Projectile.Parametri, Projectile.Direction, Projectile.Direction, Projectile.Owner) { }

        public Proiettile(Proiettile Projectile, Vector2 Direzione, Vector2 Posizione) : this(Projectile) 
        { this.Direction = Direzione; this.Position = Posizione; }

        public Proiettile(Game Game, string SpriteFolderPath, ParametriProiettile Parametri, Vector2 Direzione, Vector2 Posizione, Agente Owner) : base(Game)
        {
            this.Game             = (MainGame)Game;
            this.Name             = SpriteFolderPath.Substring(SpriteFolderPath.LastIndexOf(@"\") + 1);
            this.SpriteFolderPath = SpriteFolderPath;
            this.Parametri        = Parametri;
            this.Position         = Posizione;
            this.Direction        = Direzione;
            this.Owner            = Owner;
           
            this.Sprites = new List<Sprite>();
            this.LoadSprites();
        }

        public Proiettile Copy()
        { return new Proiettile(this.Game, this.SpriteFolderPath, this.Parametri, this.Direction, this.Position, this.Owner); }

        public Proiettile Copy(Vector2 Direzione, Vector2 Posizione)
        { return new Proiettile(this, Direzione, Posizione); }

        #endregion

        #region Caricamento Sprite

        private void LoadSprites()
        {
            string[] Files = Directory.GetFiles(this.SpriteFolderPath, "*.png");

            for (int i = 0; i < Files.Length; i++)
                Sprites.Add(new Sprite(this.Game, Files[i]));              

            if (Direction == DirezioneSu)
                Current = Sprites[0];
            else if (Direction == DirezioneGiù)
                Current = Sprites[1];
            else if (Direction == DirezioneSinistra)
                Current = Sprites[2];
            else if (Direction == DirezioneDestra)
                Current = Sprites[3];
            if(Current != null)
                this.Bordo = new Rectangle((int)this.Position.X, (int)this.Position.Y, this.Current.Width, this.Current.Height);
        }

        #endregion

        #region Metodi Ereditati

        public override void Draw(GameTime gameTime)
        {
            if(Current != null)
                this.Game.SpriteBatch.Draw(Current.texture, this.Position, Color.White);
        }

        public override void Update(GameTime gameTime)
        { Muovi(); }


        #endregion

        #region Funzioni minori (Movimento e CheckCollision)

        public void Muovi() //Cancella e Ridisegna lo Sprite Corrente nella Nuova Posizione 
        {
            this.Position.X += Direction.X * Parametri.Velocità;
            this.Position.Y += Direction.Y * Parametri.Velocità;

            this.Bordo.X = (int)this.Position.X;
            this.Bordo.Y = (int)this.Position.Y;
        }

        public bool CheckCollision(Agente Agent)
        {
            if (this != null && Agent != null)
                return this.Bordo.Intersects(Agent.Bordo);
            else
                return false;
            
        }

        public bool CheckCollision(OggettoInanimato Obj)
        {
            if (this != null && Obj != null)
                return this.Bordo.Intersects(Obj.Bordo);
            else
                return false;
        }

        #endregion

    }
}


