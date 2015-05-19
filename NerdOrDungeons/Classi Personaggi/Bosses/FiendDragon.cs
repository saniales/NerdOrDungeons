using Microsoft.Xna.Framework;
using System.Timers;
using System;

namespace NerdOrDungeons
{
    public sealed class FiendDragon : Nemico
    {
        #region Variabili Precaricate

        static FiendDragon()
        { DefaultPath = @".\Content\Enemies\Bosses\FiendDragon"; }

        #endregion

        #region Variabili

        private Timer TimerGenera;
        private Timer TimerBarriera;
        private Timer TimerFineBarriera;

        private bool PossoGenerareNemici;
        private bool PossoAttivareLaBarriera;
        public  bool BarrieraAttivata { get { return !PossoAttivareLaBarriera; } set { PossoAttivareLaBarriera = !value; } } 

        #endregion

        private FiendDragon(Vector2 Posizione) 
        : base(MainGame.RiferimentoGlobaleAlGioco, Posizione, ParametriPersonaggio.FiendDragon, DefaultPath, Proiettile.Types["FlameShot"], TipoIA.Insegui, 2000) 
        {
            PossoGenerareNemici     = false;
            PossoAttivareLaBarriera =  true;

            TimerGenera       = new Timer( 5000);
            TimerBarriera     = new Timer(20000);
            TimerFineBarriera = new Timer(10000);

            TimerGenera      .Elapsed += new ElapsedEventHandler(PermettiGenerazioneNemico);
            TimerBarriera    .Elapsed += new ElapsedEventHandler(PermettiBarriera);
            TimerFineBarriera.Elapsed += new ElapsedEventHandler(PermettiDisattivazioneBarriera);
        }

        public static FiendDragon Create(Vector2 Posizione)
        {
            FiendDragon ret = new FiendDragon(Posizione);
            ret.TimerGenera.Start();
            ret.TimerBarriera.Start();
            return ret;
        }

        #region EventiTimers

        private void PermettiGenerazioneNemico(object sender, ElapsedEventArgs e)
        { PossoGenerareNemici = true; }

        private void PermettiBarriera(object sender, ElapsedEventArgs e)
        {
            PossoAttivareLaBarriera = false;
            TimerFineBarriera.Start();
            TimerBarriera.Stop();
        }

        private void PermettiDisattivazioneBarriera(object sender, ElapsedEventArgs e)
        {
            PossoAttivareLaBarriera = true;
            this.TimerBarriera.Start();
            this.TimerFineBarriera.Stop();
        }


        #endregion

        #region Metodi Ereditati

        public override void Update(GameTime gameTime)
        {
            if (BarrieraAttivata)
            {
                //this.Current = this.Sprites[OTHER];
                this.Parametri.Salute += 1;
            }
            if (PossoGenerareNemici)
            {
                GeneraNemico(Nemico.LittleDevil);
                SoundEngine.Effects["LittleDevilLaugh"].Play();
                PossoGenerareNemici = false;
            }      
            base.Update(gameTime);
        }

        protected override void Dispose(bool disposing)
        {
            TimerGenera.Dispose();
            TimerBarriera.Dispose();
            TimerFineBarriera.Dispose();
            if(this.Game.Level != null)
                this.Game.Level.Nemici.Clear();
            base.Dispose(disposing);
        }

        private void GeneraNemico(Nemico Nemico)
        {
            Random NumeroCasuale;
            Vector2 PosizioneNemico;
            float x,y;
            NumeroCasuale = new Random();
            x = NumeroCasuale.Next(30, 770);
            y = NumeroCasuale.Next(30, 370);
            PosizioneNemico = new Vector2(x,y);
            if (this.Game.Level != null)
            {
                /* APPLICO LA DIFFICULTY */
                Nemico.Parametri.Salute = (int)(this.Game.DifficultyMultiplier * Nemico.Parametri.Salute);
                Nemico.Parametri.Attacco = (int)(this.Game.DifficultyMultiplier * Nemico.Parametri.Attacco);
                Nemico.Parametri.Velocità = (this.Game.DifficultyMultiplier * Nemico.Parametri.Velocità);
                /* END */
                this.Game.Level.Nemici.Add(Nemico.Copy(PosizioneNemico));
            }
        }

        #endregion
    }
}
