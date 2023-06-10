using System;
using Microsoft.Xna.Framework;
using System.Timers;

namespace NerdOrDungeons
{
    public class AntiSganon : Nemico // 'Y'
    {
        #region Variabili Preconfigurate

        static AntiSganon()
        {
            //DefaultPath è Ereditata Da Nemico e Sovrascritta
            DefaultPath = @".\Content\Enemies\Bosses\AntiSganon";
        }

        #endregion

        #region Dichiarazione Variabili

        private Timer  TimerPassi;
        private Timer  TimerSpara;
        private Timer  TimerSuperAttacco;

        private Timer  IdlePulizia;
        private Random Rnd;

        private bool CamminoADestra;

        #endregion

        #region Costruttore

        private AntiSganon(Vector2 Posizione)
        : base(MainGame.RiferimentoGlobaleAlGioco, Posizione, ParametriPersonaggio.AntiSganon.Copy(),
               DefaultPath, Proiettile.Types["Spear"], TipoIA.Assente, 0) 
        {
            CamminoADestra             = true;
            Rnd                        = new Random();

            TimerPassi        = new Timer();
            TimerSpara        = new Timer();
            TimerSuperAttacco = new Timer();
            IdlePulizia       = new Timer();

            TimerPassi.Interval        = Rnd.Next(5000 , 6001);  // da 5   a 6  Secondi
            TimerSpara.Interval        = Rnd.Next(500  , 1001);  // da 0.5 a 1  Secondi
            TimerSuperAttacco.Interval = Rnd.Next(18000, 22001); //da 18 a 22 secondi

            IdlePulizia.Interval       = 120000;                 //Pulizia Memoria Ogni 2 Minuti

            TimerPassi.AutoReset        = true;
            TimerSpara.AutoReset        = true;
            TimerSuperAttacco.AutoReset = true;
            IdlePulizia.AutoReset       = true;

            TimerPassi.Elapsed        += new ElapsedEventHandler(CambiaDirezione);
            TimerSpara.Elapsed        += new ElapsedEventHandler(SparaProiettile);
            TimerSuperAttacco.Elapsed += new ElapsedEventHandler(AttaccoFinale);

            IdlePulizia.Elapsed += new ElapsedEventHandler(PuliziaMemoria);

            //Per AntiSganon Non Serve Il TimerIA
            TimerIA.Dispose();
            GestisciPassi.Dispose();
        }       

        #endregion

        #region Eventi Timers

        private void AttaccoFinale(object sender, ElapsedEventArgs e)
        {
            for(int i = 18; i <= 300; i+= 16)
                this.ListaProiettili.Add(ProiettileCheSparo.Copy(Proiettile.DirezioneGiù, new Vector2(i, 150)));
            ((Timer)sender).Interval = Rnd.Next(18000, 22001);
        }

        private void SparaProiettile(object sender, ElapsedEventArgs e) 
        {
            //Sparo Sempre In Giù
            this.Current = Sprites[WAIT_DOWN];
            Spara();
            ((Timer)sender).Interval = Rnd.Next(500, 1001);
        }

        private void CambiaDirezione(object sender, ElapsedEventArgs e) 
        {
            CamminoADestra = !CamminoADestra;
            if (CamminoADestra)
                this.DirezioneCorrente = DirezioneCorrente.Destra;
            else
                this.DirezioneCorrente = DirezioneCorrente.Sinistra;
            ((Timer)sender).Interval = Rnd.Next(5000, 6001);
        }

        private void PuliziaMemoria(object sender, ElapsedEventArgs e)
        { GC.Collect(); }

        #endregion

        #region Metodi Ereditati

        public override void Update(GameTime gameTime)  
        {
            ImpostaMovimenti(this.DirezioneCorrente, gameTime);
            this.ListaProiettili.Update(gameTime);
        }

        protected override void Dispose(bool disposing) 
        {
            TimerPassi.Dispose();
            TimerSpara.Dispose();
            TimerSuperAttacco.Dispose();
            IdlePulizia.Dispose();
            base.Dispose(disposing);
        }

        public static AntiSganon Create(Vector2 Posizione)
        {
            AntiSganon temp = new AntiSganon(Posizione);
            temp.TimerPassi.Start();
            temp.TimerSpara.Start();
            temp.TimerSuperAttacco.Start();
            temp.IdlePulizia.Start();
            return temp;
        }

        #endregion
    }
}
