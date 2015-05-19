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
     ** ParametriProiettile:                                         **
     ** Classe Di Gestione Dei Parametri Di Proiettili Sparati       **
     ** dall'Utente, Nemici, Alleati                                 **
     **                                                              **
     ******************************************************************
     **                                                              **/ 

    public struct ParametriProiettile
    {
        #region Parametri Preconfigurati

        public static ParametriProiettile BlueLaser;
        public static ParametriProiettile RedLaser;
        public static ParametriProiettile GreenLaser;
        public static ParametriProiettile CannonBall;        

        public static ParametriProiettile FlameShot;

        public static ParametriProiettile EnergyBall;
        public static ParametriProiettile Rock;
        
        public static ParametriProiettile Spear;
        public static ParametriProiettile FireWall;

        static ParametriProiettile()
        {             
            GreenLaser        = new ParametriProiettile(200 , 1.5f);
            RedLaser          = new ParametriProiettile(200 , 2.0f);
            BlueLaser         = new ParametriProiettile(400 , 4.0f);
            CannonBall        = new ParametriProiettile(500 , 2.5f);

            FlameShot         = new ParametriProiettile(1000, 4.0f);

            Rock              = new ParametriProiettile(100 , 1.5f);
            EnergyBall        = new ParametriProiettile(500 , 2.0f);
            Spear             = new ParametriProiettile(500 , 2.5f);
            FireWall          = new ParametriProiettile(5000, 0.1f); 
        }

        #endregion

        #region Dichiarazione Variabili

        public int   Attacco;
        public float Velocità;

        #endregion

        #region Costruttore

        private ParametriProiettile(int Attacco, float Velocità)
        {
            this.Attacco = Attacco;
            this.Velocità = Velocità;
        }

        public ParametriProiettile(ParametriProiettile Params) 
        : this(Params.Attacco, Params.Velocità) { }

        public ParametriProiettile Copy() 
        { return new ParametriProiettile(this); }

        #endregion

    }
}
