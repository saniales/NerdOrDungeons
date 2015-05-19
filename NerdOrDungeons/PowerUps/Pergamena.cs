using Microsoft.Xna.Framework;

namespace NerdOrDungeons
{       
    /**                                                              **
     ******************************************************************
     **                                                              **
     ** ModificatoreParametri -> Eredita da PowerUpGenerico :        **
     ** Particolare PowerUp : Permette Di Imparare Tecniche Nuove    **
     ** Per Sparare Nuovi Tipi di Proiettile                         **
     **                                                              **
     ******************************************************************
     **                                                              **/

    public sealed class Pergamena : PowerUpGenerico
    {  
        #region Pergamene Preconfigurate      

        public static Pergamena BlueLaserScroll;          //  'q'
        public static Pergamena RedLaserScroll;           //  'w'
        public static Pergamena GreenLaserScroll;         //  'e'
        public static Pergamena FlameShotScroll;          //  'r'
        public static Pergamena UltimateFlameShotScroll;  //  't'

        static Pergamena()
        {                            
            BlueLaserScroll  = new Pergamena(MainGame.RiferimentoGlobaleAlGioco, DefaultPath + "BlueLaserScroll.png" , Vector2.Zero, Proiettile.Types["BlueLaser" ]);
            RedLaserScroll   = new Pergamena(MainGame.RiferimentoGlobaleAlGioco, DefaultPath + "RedLaserScroll.png"  , Vector2.Zero, Proiettile.Types["RedLaser"  ]);
            GreenLaserScroll = new Pergamena(MainGame.RiferimentoGlobaleAlGioco, DefaultPath + "GreenLaserScroll.png", Vector2.Zero, Proiettile.Types["GreenLaser"]);

            FlameShotScroll  = new Pergamena(MainGame.RiferimentoGlobaleAlGioco, DefaultPath + "FlameShotScroll.png" , Vector2.Zero, Proiettile.Types["FlameShot"]);
        }

        #endregion

        #region Variabili

        Proiettile Tecnica;

        #endregion

        #region Costruttori

        public Pergamena(Game Game, string SpritePath, Vector2 Posizione, Proiettile Technique) : base(Game, SpritePath, Posizione, Bonus.Empty) 
        { this.Tecnica = Technique; }

        public Pergamena Copy(Vector2 Posizione)
        { return new Pergamena(this.Game, this.SpritePath, Posizione, this.Tecnica); }

        #endregion

        #region Metodi Ereditati

        public override void TouchedBy(Personaggio Player)
        {
            Player.PossoSparare = true;
            Player.ProiettileCheSparo = this.Tecnica;
            SoundEngine.Effects["ScrollTouched"].Play();
            base.TouchedBy(Player);
        }

        #endregion
    }
}
