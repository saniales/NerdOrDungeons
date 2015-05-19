using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace NerdOrDungeons
{
    /**                                                              **
     ******************************************************************
     **                                                              **
     ** MuroRimovibile -> Eredita da OggettoInanimato :              **
     ** E' Legato A Un Interruttore. Sparisce o Riappare Quando      **
     ** L'Interruttore è Premuto.                                    **
     **                                                              **
     ******************************************************************
     **                                                              **/

    public sealed class MuroRimovibile : OggettoInanimato
    {
        #region Muri Rimovibili Preconfigurati

        public static MuroRimovibile Tipo1;
        public static MuroRimovibile Tipo2;
        public static MuroRimovibile Tipo3;

        static MuroRimovibile()
        {
            Tipo1 = new MuroRimovibile(MainGame.RiferimentoGlobaleAlGioco, Vector2.Zero, 1);
            Tipo2 = new MuroRimovibile(MainGame.RiferimentoGlobaleAlGioco, Vector2.Zero, 2);
            Tipo3 = new MuroRimovibile(MainGame.RiferimentoGlobaleAlGioco, Vector2.Zero, 3);
        }
  
        #endregion

        #region Dichiarazione variabili

        Sprite Sprite;
        int Tipo;

        #endregion

        #region Costruttore

        public MuroRimovibile(Game Game, Vector2 Posizione, int Tipo) : base(Game, Posizione)
        { 
            this.Tipo = Tipo;
            this.Sprite = new Sprite(this.Game, @".\Content\Switches\SwitchableWalls\" + Tipo + ".png");
            this.Sprite.Posizione = Posizione;
        }

        private MuroRimovibile(MuroRimovibile m, Vector2 Posizione )
        : this(m.Game, Posizione, m.Tipo) { }

        public MuroRimovibile Copy(Vector2 Posizione)
        { return new MuroRimovibile(this, Posizione); }

        #endregion

        #region Metodi Ereditati

        public override void Draw(GameTime gameTime)
        {
                Sprite.Draw(gameTime);
                base.Draw(gameTime);
        }

        protected override void Dispose(bool disposing)
        {
            Sprite.Dispose();
            base.Dispose(disposing);
        }

        #endregion
    }
}
