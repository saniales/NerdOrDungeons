using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NerdOrDungeons
{
    /**                                                              **
     ******************************************************************
     **                                                              **
     ** Sprite :                                                     **
     ** Classe Di Gestione Di Tutte le Immagini Di Personaggi,       **
     ** Nemici, OggettiInanimati, Proiettili, Cursori.               **
     ** Classe Di Base.                                              **
     **                                                              **
     **               [Roberto Mana Is Our Supreme Lord]             **
     **                                                              **
     ******************************************************************
     **                                                              **/ 

    public class Sprite : DrawableGameComponent 
    {
        #region Variabili

        protected string       path;
        protected new MainGame Game;

        public    Vector2 Posizione;
        public    Texture2D texture;

        public    int Height { get { if (this.texture != null) return this.texture.Height; else return 0; } }
        public    int Width  { get { if (this.texture != null) return this.texture.Width;  else return 0; } }
         
        #endregion

        #region Costruttore

        public Sprite(Game _game, string _path, Vector2 Posizione) : base(_game)
        {
            this.Game = (MainGame)_game;
            this.Posizione = Posizione;
            this.path = _path;

            if(this.Game.GraphicsDevice != null) // Non Sto Chiudendo Il Gioco
                this.texture = Texture2D.FromStream(this.Game.GraphicsDevice, File.OpenRead(this.path));
        }

        public Sprite(Game Game, string _path) : this(Game, _path, Vector2.Zero) { }

        #endregion

        #region Metodi Ereditati

        protected override void Dispose(bool disposing)
        {
            this.texture.Dispose();
            base.Dispose(disposing);
        }

        public override void Draw(GameTime gameTime)
        {
            this.Game.SpriteBatch.Draw(this.texture, this.Posizione, Color.White);
            base.Draw(gameTime);
        }

        #endregion

    }
}
