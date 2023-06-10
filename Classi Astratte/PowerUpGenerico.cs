using Microsoft.Xna.Framework;

namespace NerdOrDungeons
{
    /**                                                              **
     ******************************************************************
     **                                                              **
     ** PowerUpGenerico -> Eredita da Oggettoinanimato :             **
     ** Gestisce Tutti i PowerUps e Il Loro Interfacciamento Con il  **
     ** Game e il Player.                                            **
     **                                                              **
     ******************************************************************
     **                                                              **/

    public abstract class PowerUpGenerico : OggettoInanimato
    {
        #region Variabili Precaricate

        protected static string DefaultPath;

        static PowerUpGenerico()
        { DefaultPath = @".\Content\PowerUps\"; }

        #endregion

        #region Variabili

        public    Sprite               Sprite;
        public    Bonus                BonusMalus;
        public    bool                 IsValid;

        protected string               SpritePath;

        #endregion

        #region Costruttore

        public PowerUpGenerico(Game Game, string SpritePath, Vector2 Posizione, Bonus BonusMalus) : base(Game, Posizione)
        {
            this.SpritePath = SpritePath;
            this.BonusMalus = BonusMalus;
            this.IsValid    = true;
            this.Sprite = new Sprite(this.Game, this.SpritePath, this.Posizione);
        }

        #endregion

        #region Metodi Ereditati

        public override void Draw(GameTime gameTime)
        {
            if (this.IsValid)
            {
                this.Sprite.Draw(gameTime);
                base.Draw(gameTime);
            }
        }

        /** Metodo TouchedBy : nelle Classi Ereditate Scrivere Qui  **
         ** Il Codice di Override Delle Azioni Che Vanno Eseguite   **
         ** Quando il PowerUp Collide Con Personaggio               **
         ** (Il PowerUp Generico Semplicemente Sparisce)            **/

        public virtual void TouchedBy(Personaggio Player) { this.IsValid = false; }

        public bool CheckCollision(Personaggio Player)
        {
            if (this.IsValid)
                if (Rectangle.Intersect(this.Bordo, Player.Bordo) != Rectangle.Empty)
                    return true;
                else
                    return false;
            else
                return false;
        }

        #endregion
    }
}
