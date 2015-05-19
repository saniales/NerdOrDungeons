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
     ** ModificatoreParametri -> Eredita da PowerUpGenerico :        **
     ** Particolare PowerUp : Aggiunge Dei Bonus/Malus Al Player che **
     ** Lo Tocca.                                                    **
     **                                                              **
     ******************************************************************
     **                                                              **/

    public sealed class ModificatoreParametri : PowerUpGenerico
    {
        #region Variabili Preconfigurate PowerUps ModificatoriParametri

        public static ModificatoreParametri SmallMedikit;          // 'a'
        public static ModificatoreParametri MediumMedikit;         // 's'
        public static ModificatoreParametri BigMedikit;            // 'd'

        public static ModificatoreParametri AttackSmallBonus;      // 'f'
        public static ModificatoreParametri AttackBigBonus;        // 'g'
        public static ModificatoreParametri AttackMalus;           // 'h'

        public static ModificatoreParametri SpeedBonus;            // 'j'
        public static ModificatoreParametri SpeedMalus;            // 'k'  

        static ModificatoreParametri()
        {
            SmallMedikit     = new ModificatoreParametri(MainGame.RiferimentoGlobaleAlGioco, DefaultPath + "SmallMediKit.png"    , Vector2.Zero, Bonus.SmallMedikit);
            MediumMedikit    = new ModificatoreParametri(MainGame.RiferimentoGlobaleAlGioco, DefaultPath + "MediumMediKit.png"   , Vector2.Zero, Bonus.MediumMedikit);
            BigMedikit       = new ModificatoreParametri(MainGame.RiferimentoGlobaleAlGioco, DefaultPath + "BigMediKit.png"      , Vector2.Zero, Bonus.BigMedikit);

            AttackSmallBonus = new ModificatoreParametri(MainGame.RiferimentoGlobaleAlGioco, DefaultPath + "AttackSmallBonus.png", Vector2.Zero, Bonus.AttackSmallBonus);
            AttackBigBonus   = new ModificatoreParametri(MainGame.RiferimentoGlobaleAlGioco, DefaultPath + "AttackBigBonus.png"  , Vector2.Zero, Bonus.AttackBigBonus);
            AttackMalus      = new ModificatoreParametri(MainGame.RiferimentoGlobaleAlGioco, DefaultPath + "AttackMalus.png"     , Vector2.Zero, Bonus.AttackMalus);
                                                                                                                                                           
            SpeedBonus       = new ModificatoreParametri(MainGame.RiferimentoGlobaleAlGioco, DefaultPath + "SpeedBonus.png"      , Vector2.Zero, Bonus.SpeedBonus);
            SpeedMalus       = new ModificatoreParametri(MainGame.RiferimentoGlobaleAlGioco, DefaultPath + "SpeedMalus.png"      , Vector2.Zero, Bonus.SpeedMalus);
        }

        #endregion

        #region Costruttori

        public ModificatoreParametri(Game Game, string SpritePath, Vector2 Posizione, Bonus BonusMalus) :base(Game, SpritePath, Posizione, BonusMalus) { }

        public ModificatoreParametri Copy(Vector2 Posizione)
        {
            return new ModificatoreParametri(this.Game, this.SpritePath, Posizione, this.BonusMalus);
        }

        #endregion

        #region Metodi Ereditati

        public override void TouchedBy(Personaggio Player)
        {
            Player.Parametri.Add(this.BonusMalus);
            SoundEngine.Effects["PowerUpTouched"].Play();
            base.TouchedBy(Player);
        }

        #endregion
    }
}
