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
    ** Missile -> Eredita da Nemico                                 **
    ** Proiettile Con Intelligenza : Ti Insegue Poi Sparisce Dopo   **
    ** Aver Inflitto Il Danno                                       **
    ** Per Distruggerlo Basta Colpirlo.                             **
    **                                                              **
    ******************************************************************
    **                                                              **/

    public class Missile : Nemico
    {
        public static Dictionary<string, Missile> Types;

        static Missile()
        {
            Types = new Dictionary<string, Missile>();

            Types.Add("Rocket", new Missile(MainGame.RiferimentoGlobaleAlGioco, Vector2.Zero, ParametriPersonaggio.Rocket, @".\Content\Projectiles\Rocket"   , null));
        }

        #region Variabili

        private Personaggio Bersaglio;

        #endregion

        private Missile(Game Game, Vector2 Posizione, ParametriPersonaggio Parametri, string SpriteFolderPath, Personaggio Target)
        : base(Game, Posizione, Parametri, SpriteFolderPath, null, TipoIA.Insegui, 800) {  }

        private Missile(Missile Clone, Vector2 Posizione, Personaggio Target) 
        : this(Clone.Game, Posizione, Clone.Parametri, Clone.SpriteFolderPath, Target) { }

        public Missile Create(Vector2 Posizione, Personaggio MyTarget)
        { return new Missile(this, Posizione, MyTarget); }
    }
}
