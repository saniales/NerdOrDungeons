using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NerdOrDungeons
{
    /**                                                              **
     ******************************************************************
     **                                                              **
     ** OggettoInanimato --> Eredita Da Sprite :                     **
     ** Particolare Tipo Di Sprite : Su di esso devo gestire le      **
     ** Collisioni di Utente, Nemici, Alleati.                       **
     **                                                              **
     ******************************************************************
     **                                                              **/ 

    public class OggettoInanimato : DrawableGameComponent
    {
        #region Variabili

        protected new MainGame   Game;
        protected     Vector2    Posizione;   //Posizione dell'oggetto Inanimato
        internal      Rectangle  Bordo;

        #endregion

        #region Costruttore + Metodi Appoggio

        public OggettoInanimato(Game Game, Vector2 Posizione) : base(Game)
        {
            this.Game = (MainGame)Game;
            this.Posizione = Posizione;
            this.Bordo = new Rectangle((int)this.Posizione.X, (int)this.Posizione.Y, 16, 16);
        }  
      

        #endregion

    }
}
