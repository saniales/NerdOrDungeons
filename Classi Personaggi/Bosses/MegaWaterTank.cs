using System;
using Microsoft.Xna.Framework;
using System.Timers;

namespace NerdOrDungeons
{
    public class MegaWaterTank : Nemico
    {
        #region Variabili

        private Random Rnd;
        private Timer TimerMissili;
        private Timer SparaNVolte;
        private int ContaSpari;
        private bool PuoSparareMissili;
        private bool PuoSparareUnMissile;
        private bool Damaged1, Damaged2, Damaged3;


        #endregion

        private MegaWaterTank(Vector2 Posizione)
            : base(MainGame.RiferimentoGlobaleAlGioco, Posizione, ParametriPersonaggio.MegaWaterTank, @".\Content\enemies\Bosses\MegaWaterTank", null, TipoIA.Casuale, 0)
        {
            Rnd = new Random();
            ContaSpari = 0;
            PuoSparareUnMissile = false;
            PuoSparareMissili = false;
            TimerMissili = new Timer(10000);
            SparaNVolte = new Timer(5000);

            TimerMissili.AutoReset = true;
            SparaNVolte.AutoReset = true;

            TimerMissili.Elapsed += new ElapsedEventHandler(PreparaSparo);
            SparaNVolte.Elapsed += new ElapsedEventHandler(SparaUnMissile);
        }

        #region Eventi Timers

        private void PreparaSparo(object sender, ElapsedEventArgs e)
        { PuoSparareMissili = true; SparaNVolte.Start(); }

        private void SparaUnMissile(object sender, ElapsedEventArgs e)
        { PuoSparareUnMissile = true; }

        #endregion

        private void SparaUnMissile()
        {
            if (ContaSpari++ == 4)
            {
                ContaSpari = 0;
                //azzero la situazione
                PuoSparareUnMissile = false;
                PuoSparareMissili = false;
                SparaNVolte.Stop();
            }
            else
            {
                PuoSparareUnMissile = true;
                //sparo da come sono girato
                /* N.B. : Posso Usare MainGame.RiferimentoGlobaleAlGioco Perché Player Sarà Sicuramente Inizializzato Quando Sparo */
                if (ContaSpari % 2 == 0)
                    this.Game.Level.Missili.Add(Missile.Types["Rocket"].Create(new Vector2( 50, 100), MainGame.RiferimentoGlobaleAlGioco.Level.Player));
                else
                    this.Game.Level.Missili.Add(Missile.Types["Rocket"].Create(new Vector2(550, 100), MainGame.RiferimentoGlobaleAlGioco.Level.Player));
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (PuoSparareMissili && PuoSparareUnMissile)
            {
                SparaUnMissile();
                PuoSparareUnMissile = false;
            }
        }

        public static MegaWaterTank Create(Vector2 Posizione)
        {
            MegaWaterTank ret = new MegaWaterTank(Posizione);
            ret.TimerMissili.Start();
            return ret;
        }
    }
}
