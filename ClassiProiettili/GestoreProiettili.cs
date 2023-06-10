using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace NerdOrDungeons
{
    /**                                                              **
     ******************************************************************
     **                                                              **
     ** Proiettili :                                                 **
     ** Classe Assegnata A un Agente Per Gestire Tutti i Proiettili  **
     ** Sparati.                                                     **
     **                                                              **
     ******************************************************************
     **                                                              **/

    public sealed class GestoreProiettili : DrawableGameComponent
    {
        #region Dichiarazione Variabili
        
        public    int              Count { get { return Istanza.Count; } }

        internal  Agente           Owner;
        internal  List<Proiettile> Istanza;
        
        #endregion

        #region Costruttore + metodi Appoggio

        public GestoreProiettili(Agente Owner) : base(Owner.Game)
        {
            this.Istanza = new List<Proiettile>();
            this.Owner = Owner;
        }

        #endregion

        #region Wrapper Lista Add(Item)

        public void Add(Proiettile Item)
        {
            Proiettile temp = Item.Copy();
            temp.Owner = this.Owner;

            //prima setto direzione
            if (Owner.Current == Owner.Sprites[Agente.ATTACK_DOWN])
                    temp.Direction = Proiettile.DirezioneGiù;
            
            if (Owner.Current == Owner.Sprites[Agente.ATTACK_UP])
                    temp.Direction = Proiettile.DirezioneSu;
            
            if (Owner.Current == Owner.Sprites[Agente.ATTACK_LEFT])
                    temp.Direction = Proiettile.DirezioneSinistra;

            if (Owner.Current == Owner.Sprites[Agente.ATTACK_RIGHT])
                    temp.Direction = Proiettile.DirezioneDestra;
            
            //poi la posizione di partenza del proiettile
            temp.Position = Owner.Posizione;

            if (temp.Direction == Proiettile.DirezioneSu)
            {
                temp.Position.Y += 2;
                temp.Position.X += 2f; /* Owner.Posizione.X + (Owner.Current.Width / 2);*/
            }

            if (temp.Direction == Proiettile.DirezioneGiù)
            {
                temp.Position.Y += 30;
                temp.Position.X = Owner.Posizione.X + (Owner.Current.Width / 2);
            }

            if (temp.Direction == Proiettile.DirezioneDestra)
            {
                temp.Position.X += 25;
                temp.Position.Y = Owner.Posizione.Y + (Owner.Current.Height / 2);
            }

            if (temp.Direction == Proiettile.DirezioneSinistra)
            {
                temp.Position.X += 2f;
                temp.Position.Y = Owner.Posizione.Y + (Owner.Current.Height / 2);
            }

            Istanza.Add(temp.Copy());
            temp.Dispose();
        }

        public void Clear()
        { this.Istanza.Clear(); }

        #endregion

        #region Metodi Ereditati

        public override void Update(GameTime gameTime)
        {
            foreach (Proiettile p in Istanza)
                p.Update(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (Proiettile p in Istanza)
                p.Draw(gameTime);
            base.Draw(gameTime);
        }

        protected override void Dispose(bool disposing)
        {
            this.Istanza.Clear();
            //Non Devo Rilasciare L'Agente, Deve Essere Ancora Usato Senz'Altro Nello Scenario
            base.Dispose(disposing);
        }

        #endregion
    }
}
