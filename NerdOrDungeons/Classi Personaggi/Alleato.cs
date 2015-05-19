using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Timers;

namespace NerdOrDungeons
{
    /**                                                              **
     ******************************************************************
     **                                                              **
     ** Alleato --> Eredita Da Nemico :                              **
     ** Nemico Particolare : non Attacca il giocatore, ma gli altri  **
     ** nemici, Appare solo All'ultimo Livello                       **
     **                                                              **
     **                   [Roberto Mana Docet]                       **
     **                                                              **
     ******************************************************************
     **                                                              **/

    public class Alleato : Nemico
    {
        #region Variabili Precaricate

        private static string  DefaultPath;

        public  static Alleato Prematurator; //'O'

        static Alleato()
        {
            DefaultPath = @".\Content\Allies\";

            Prematurator = new Alleato(MainGame.RiferimentoGlobaleAlGioco, Vector2.Zero, ParametriPersonaggio.AllyPrematurator.Copy(), DefaultPath + "Prematurator", Proiettile.Types["EnergyBall"], 400, null);
        }

        #endregion

        #region Dichiarazione Variabili

        Personaggio Master;
        Timer TimerSpara;

        #endregion

        #region Costruttori

        public Alleato(Game Game, Vector2 Posizione, ParametriPersonaggio Parametri, string SpriteFolderPath, Proiettile Projectile, int CampoVisivo, Personaggio Master)
        : base(Game, Posizione, Parametri, SpriteFolderPath, Projectile, TipoIA.Alleato, CampoVisivo)
        {
            this.Master = Master;
            TimerSpara = new Timer(500);
        }

        private Alleato(Alleato Clone, Vector2 Posizione, Personaggio Master) 
        : this(Clone.Game, Posizione, Clone.Parametri, Clone.SpriteFolderPath, Clone.ProiettileCheSparo, Clone.CampoVisivo.Height, Master) { }

        public Alleato Copy(Vector2 Posizione, Personaggio Master)
        {
            return new Alleato(this, Posizione, Master);
        }

        private void ApplicaAIAlleato(GameTime g)
        {
            List<Nemico> NemiciInVista;
 
            NemiciInVista = this.NemiciInVista(this.CampoVisivo);
            //non lo vedo più quindi se non ci sono nemici intorno mi teletrasporto a proteggerlo
            if (NonVedoNemici(this.CampoVisivo) && !CampoVisivo.Intersects(Master.Bordo))
                this.Posizione = Master.Posizione;
            //inseguo un Nemico A Caso Fra Quelli Che Vedo
            if (NonVedoNemici(this.CampoVisivo))
                this.IA = TipoIA.Casuale;
            else
            //spara al nemico quando lo vedi ma tieniti a distanza
            { 
                PrendiDiMiraunNemico(NemiciInVista);

                this.DirezioneCorrente = DirezioneCorrente.Fermo;
            }
        }

        public void Muori()      
        {
            this.Current = Sprites[DEAD];
            this.DisabilitaMovimenti = true;
            Master.DisabilitaMovimenti = true;
            if (this.Game.Level != null)
                this.Game.Level.LevelTrack.Stop();
            SoundEngine.Tracks["Death"].Play(true);
        }

        #endregion

        #region Metodi Ereditati

        public override void Update(GameTime gameTime)
        {
            ApplicaAIAlleato(gameTime);
            base.Update(gameTime);
        }

        #endregion


        #region Funzioni Appoggio Al CampoVisivo

        public void PrendiDiMiraunNemico(List<Nemico> NemiciInVista)
        {
            foreach (Nemico n in NemiciInVista)
            {
                /* Se è Nel Mio Raggio D'Azione Sparo               *
                 *                                                  *
                 *             Enemy                                *
                 *               |                                  *
                 *               |                                  *
                 *  Enemy ----- I o ----- Enemy                     *
                 *               |                                  *
                 *               |                                  *
                 *             Enemy                                *
                 * ( o almeno questa sarebbe l'idea, ma non funza ) */
                if (this.Posizione.X - (this.Current.Width / 2) < n.Posizione.X &&
                    n.Posizione.X < this.Posizione.X + (this.Current.Width / 2))
                {
                    if (this.Posizione.Y > n.Posizione.Y)
                        this.DirezioneCorrente = DirezioneCorrente.Su;
                    else
                        this.DirezioneCorrente = DirezioneCorrente.Giù;
                    break;
                }

                else if (this.Posizione.Y - -(this.Current.Height / 2) < n.Posizione.Y &&
                        n.Posizione.Y < this.Posizione.X + (this.Current.Height / 2))
                {
                    if (this.Posizione.X > n.Posizione.X)
                        this.DirezioneCorrente = DirezioneCorrente.Destra;
                    else
                        this.DirezioneCorrente = DirezioneCorrente.Sinistra;
                    break;
                }           
            }
            Spara();    
        }

        public List<Nemico> NemiciInVista(Rectangle CampoVisivo)
        {
            List<Nemico> ret = new List<Nemico>();
            if (this.Game.Level != null)
            {
                foreach (Nemico Enemy in this.Game.Level.Nemici)
                    if (Enemy != null && Enemy.Bordo.Intersects(CampoVisivo))
                        ret.Add(Enemy);
            }
            return ret;
        }

        public bool NonVedoNemici(Rectangle CampoVisivo)
        {
            if (NemiciInVista(CampoVisivo).Count == 0)
                return true;
            else
                return false;
        }

        #endregion
    }
}
