using System;
using Microsoft.Xna.Framework;
using System.Timers;

namespace NerdOrDungeons
{
    public class Tomonobu : Nemico // 'T'
    {
        #region Variabili Preconfigurate

        static Tomonobu()
        {
            //DefaultPath è Ereditata Da Nemico e Sovrascritta
            DefaultPath = @".\Content\Enemies\Bosses\Tomonobu";
        }

        #endregion

        #region Dichiarazione Variabili

        private Timer TimerPassi;
        private Timer TimerSuperAttacco;
        private Timer TimerGeneraNemico;

        private bool  PossoNemicare;

        private Timer IdlePulizia;
        private Random Rnd;

        private bool CamminoADestra;
        private int CamminoSu; //0 -> y uguale, 1 -> su, 2 -> giù.
        private bool PossoFareAttaccoFinale;

        #endregion

        #region Costruttore

        private Tomonobu(Vector2 Posizione)
        : base(MainGame.RiferimentoGlobaleAlGioco, Posizione, ParametriPersonaggio.Tomonobu.Copy(),
               DefaultPath, Proiettile.Types["GreenLaser"], TipoIA.Assente, 0)
        {
            CamminoADestra = true;
            CamminoSu = 0;
            Rnd = new Random();

            TimerPassi = new Timer();
            TimerSuperAttacco = new Timer();
            TimerGeneraNemico = new Timer();
            IdlePulizia = new Timer();

            TimerPassi.Interval = Rnd.Next(5000, 6001);  // da 5   a 6  Secondi
            TimerSuperAttacco.Interval = Rnd.Next(18000, 22001); //da 18 a 22 secondi
            TimerGeneraNemico.Interval = Rnd.Next(5000, 10000);

            IdlePulizia.Interval = 60000;                 //Pulizia Memoria Ogni Minuto

            TimerPassi.AutoReset = true;
            TimerSuperAttacco.AutoReset = true;
            TimerGeneraNemico.AutoReset = true;
            IdlePulizia.AutoReset = true;

            TimerPassi.Elapsed += new ElapsedEventHandler(CambiaDirezione);
            TimerSuperAttacco.Elapsed += new ElapsedEventHandler(PermettiAttaccoFinale);
            TimerGeneraNemico.Elapsed += new ElapsedEventHandler(CreateEnemy);

            IdlePulizia.Elapsed += new ElapsedEventHandler(PuliziaMemoria);

            //Per AntiSganon Non Serve Il TimerIA
            TimerIA.Dispose();
            GestisciPassi.Dispose();
            this.PossoNemicare = false;
            this.PossoFareAttaccoFinale = false;
        }         

        #endregion

        #region Eventi Timers

        private void CreoIlNemico()
        {
            Random rndPosizione = new Random();
            float x = rndPosizione.Next(20, 600);
            float y = rndPosizione.Next(20, 500);
            Vector2 posizioneNemico = new Vector2(x, y);
            if (this.Game.Level != null) // non sto chiudendo
            {
                Nemico Enemy = Nemico.Snake.Copy(posizioneNemico);
                /* APPLICO DIFFICULTY */
                Enemy.Parametri.Salute = (int)(this.Game.DifficultyMultiplier * Enemy.Parametri.Salute);
                Enemy.Parametri.Attacco = (int)(this.Game.DifficultyMultiplier * Enemy.Parametri.Attacco);
                Enemy.Parametri.Velocità = this.Game.DifficultyMultiplier * Enemy.Parametri.Velocità;
                /* END */
                this.Game.Level.Nemici.Add(Enemy);
            }
        }

        private void CreoPrematurators()
        {
            Random rndPosizione = new Random();
            float x,y;
            Vector2 posizioneNemico;
            if (this.Game.Level != null) // non sto chiudendo
            {
                Nemico Enemy = Nemico.Prematurator.Copy();
                /* APPLICO DIFFICULTY */
                Enemy.Parametri.Salute = (int)(this.Game.DifficultyMultiplier * Enemy.Parametri.Salute);
                Enemy.Parametri.Attacco = (int)(this.Game.DifficultyMultiplier * Enemy.Parametri.Attacco);
                Enemy.Parametri.Velocità = this.Game.DifficultyMultiplier * Enemy.Parametri.Velocità;
                /* CREO 4 PREMATURATORS */
                for(int i = 0; i < 4; i++)   
                {                                     
                   x = rndPosizione.Next(20, 600);
                   y = rndPosizione.Next(20, 500);
                   posizioneNemico = new Vector2(x, y);
                   this.Game.Level.Nemici.Add(Enemy.Copy(posizioneNemico));
                }
            }
        }

        private void CreateEnemy(object sender, ElapsedEventArgs e)
        { PossoNemicare = true; }


        private void PermettiAttaccoFinale(object sender, ElapsedEventArgs e)
        { PossoFareAttaccoFinale = true; }

        private void AttaccoFinale()
        {
            CreoPrematurators();
            TimerSuperAttacco.Interval = Rnd.Next(18000, 22001);
            PossoFareAttaccoFinale = false;
        }

        private void CambiaDirezione(object sender, ElapsedEventArgs e)
        {
            Random rnd = new Random();
            CamminoSu = rnd.Next(0, 3);
            if (CamminoSu == 0)
            {
                CamminoADestra = !CamminoADestra;
                if (CamminoADestra)
                    this.DirezioneCorrente = DirezioneCorrente.Destra;
                else
                    this.DirezioneCorrente = DirezioneCorrente.Sinistra;
            }
            else if (CamminoSu == 1)
                this.DirezioneCorrente = DirezioneCorrente.Su;
            else
                this.DirezioneCorrente = DirezioneCorrente.Giù;
            ((Timer)sender).Interval = Rnd.Next(5000, 6001);
        }

        private void PuliziaMemoria(object sender, ElapsedEventArgs e)
        { GC.Collect(); }

        #endregion

        #region Metodi Ereditati

        public override void Update(GameTime gameTime)
        {
            ImpostaMovimenti(this.DirezioneCorrente, gameTime);
            if (PossoNemicare == true)
            {
                CreoIlNemico();
                PossoNemicare = false;
            }
            if (PossoFareAttaccoFinale)
                AttaccoFinale();
            this.ListaProiettili.Update(gameTime);
        }

        protected override void Dispose(bool disposing)
        {
            TimerPassi.Dispose();
            TimerSuperAttacco.Dispose();
            TimerGeneraNemico.Dispose();
            IdlePulizia.Dispose();
            if(this.Game.Level != null)
                this.Game.Level.Nemici.Clear();
            base.Dispose(disposing);
        }

        public static Tomonobu Create(Vector2 Posizione)
        {
            Tomonobu temp = new Tomonobu(Posizione);
            temp.TimerPassi.Start();
            temp.TimerSuperAttacco.Start();
            temp.IdlePulizia.Start();
            temp.TimerGeneraNemico.Start();
            return temp;
        }

        #endregion
    }
}
