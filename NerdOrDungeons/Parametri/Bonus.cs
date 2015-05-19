using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NerdOrDungeons
{
    /**                                                              **
     ******************************************************************
     **                                                              **
     ** Bonus --> Eredita da ParametriPersonaggio:                   **
     ** La Classe Bonus è una Semplice Ridefinizione Di              **
     ** ParametriPersonaggio Specifica Per I Power Up                **
     **                                                              **
     ******************************************************************
     **                                                              **/ 

    public sealed class Bonus : ParametriPersonaggio
    {
        #region Variabili Preconfigurate

        public static new Bonus Empty; //Bonus Vuoto (0,0,0)

        public static     Bonus SmallMedikit;
        public static     Bonus MediumMedikit;
        public static     Bonus BigMedikit;

        public static     Bonus AttackSmallBonus;
        public static     Bonus AttackBigBonus;
        public static     Bonus AttackMalus;

        public static     Bonus SpeedBonus;
        public static     Bonus SpeedMalus;

        static Bonus()
        {
            Empty = new Bonus(+0, +0, +0);

            SmallMedikit     = new Bonus(+200 , +0 , +0.0f );
            MediumMedikit    = new Bonus(+400 , +0 , +0.0f );
            BigMedikit       = new Bonus(+800 , +0 , +0.0f );

            AttackSmallBonus = new Bonus(+0   , +10, +0.0f );
            AttackBigBonus   = new Bonus(+0   , +30, +0.0f );
            AttackMalus      = new Bonus(+0   , -30, +0.0f );
                                             
            SpeedBonus       = new Bonus(+0   , +0 , +0.2f);
            SpeedMalus       = new Bonus(+0   , +0 , -0.1f);
        }

        #endregion

        #region Costruttori

        private Bonus(int Salute, int Attacco, float Velocità) : base(Salute, Attacco, Velocità) { }
        private Bonus(Bonus b) : base(b) { }

        public new Bonus Copy() { return new Bonus(this); }

        #endregion
    }
}
