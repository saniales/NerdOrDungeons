using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Timers;
using System.IO;
using System.Windows.Forms;

namespace NerdOrDungeons
{
    public class GeneratoreNemici : GameComponent
    {
        #region Variabili Preconfigurate

        public const int MARGIN_STANDARD = 30;

        #endregion

        #region Variabili

        public    string   PunteggioGiocatore;
        public    int      NemiciPerGiro;
        protected bool     PossoGenerareNemici;
        protected bool     PossoAumentareDifficoltà;
        protected MainGame Game;
        protected System.Timers.Timer TimerAumentoDifficoltà;
        protected System.Timers.Timer TimerGenera;
        private bool isActivated;
        private InputBox Temp;            

        #endregion

        #region Costruttori

        private GeneratoreNemici(Game Game, int NumeroNemiciGeneratiPerGiro)
        : base(Game)
        {
            this.Game = (MainGame)Game;
            this.Temp = null;
            PunteggioGiocatore  = "0";
            PossoGenerareNemici = false;
            PossoAumentareDifficoltà = false;
            isActivated              = true;
            NemiciPerGiro = NumeroNemiciGeneratiPerGiro;

            TimerAumentoDifficoltà = new System.Timers.Timer(30000);
            TimerGenera            = new System.Timers.Timer(6000);

            TimerAumentoDifficoltà.AutoReset = true;
            TimerGenera.AutoReset            = true;

            TimerAumentoDifficoltà.Elapsed += new ElapsedEventHandler(PermettiAumentoDifficoltà);
            TimerGenera.Elapsed            += new ElapsedEventHandler(PermettiGenerazioneNemico);
        }

        public static GeneratoreNemici Istanzia(int Numero_Nemici_Generati_Ogni_Tick)
        {
            GeneratoreNemici ret = new GeneratoreNemici(MainGame.RiferimentoGlobaleAlGioco, Numero_Nemici_Generati_Ogni_Tick);
            //Faccio Partire I timer Solo Ora (Non Quando Inizializzo)
            ret.TimerAumentoDifficoltà.Start();
            ret.TimerGenera.Start();
            return ret;
        }

        #endregion

        #region Eventi Timers

        protected void PermettiGenerazioneNemico(object sender, ElapsedEventArgs e)
        { PossoGenerareNemici = true; }

        protected virtual void PermettiAumentoDifficoltà(object sender, ElapsedEventArgs e)
        { PossoAumentareDifficoltà = true;}

        #endregion

        #region GeneraNemici

        // Genero "N" Nemici In Questo Giro in Un Rettagolo Rimpicciolito di "Panning"
        // Dal Rettangolo Del Livello.
        protected void GeneraNemici(int N, int Margin)
        {
            int i;
            int XCasuale, YCasuale;
            Random Rnd;
            Nemico NemicoDaAggiungere;
            Vector2 PosizioneNemico;

            Rnd = new Random();

            //Genero Un Nemico Diverso A Caso Fra Questi
            for (i = 0; i < N; i++)
            {
                //Imposto La Posizione Random Nello Schermo (Height MIN = 0 + 16 + Panning; MAX = 480 - 16 - Margin;)
                //                                          (Width  MIN = 0 + 16 + Panning; MAX = 800 - 16 - Margin;)

                XCasuale = Rnd.Next(16 + Margin, 784 - Margin);
                YCasuale = Rnd.Next(16 + Margin, 464 - Margin);

                PosizioneNemico = new Vector2(XCasuale, YCasuale);
                switch (Rnd.Next(9))
                {
                    case 0: NemicoDaAggiungere = Nemico.OctorockRed.Copy(PosizioneNemico);
                        break;
                    case 1: NemicoDaAggiungere = Nemico.Goomba.Copy(PosizioneNemico);
                        break;
                    case 2: NemicoDaAggiungere = Nemico.Snake.Copy(PosizioneNemico);
                        break;
                    case 3: NemicoDaAggiungere = Nemico.MegaOctorockRed.Copy(PosizioneNemico);
                        break;
                    case 4: NemicoDaAggiungere = Nemico.Snake.Copy(PosizioneNemico);
                        break;
                    case 5: NemicoDaAggiungere = Nemico.EarthTank.Copy(PosizioneNemico);
                        break;
                    case 6: NemicoDaAggiungere = Nemico.WaterTank.Copy(PosizioneNemico);
                        break;
                    case 7: NemicoDaAggiungere = Nemico.Myself.Copy(PosizioneNemico);
                        break;
                    case 8: NemicoDaAggiungere = Nemico.LittleDevil.Copy(PosizioneNemico);
                        break;
                    default:
                        throw new ArgumentException("Valore Passato Allo Switch Non Valido.\nRicontrolla In GeneratoreNemici :\nFunzione GeneraNemici(N, Margin)");
                }
                NemicoDaAggiungere.Parametri.Salute = (int) (this.Game.DifficultyMultiplier * NemicoDaAggiungere.Parametri.Salute);
                NemicoDaAggiungere.Parametri.Attacco = (int)(this.Game.DifficultyMultiplier * NemicoDaAggiungere.Parametri.Attacco);
                NemicoDaAggiungere.Parametri.Velocità = (this.Game.DifficultyMultiplier * NemicoDaAggiungere.Parametri.Velocità);
                this.Game.Level.Nemici.Add(NemicoDaAggiungere);
                PossoGenerareNemici = false;
            }
        }

        #endregion

        #region Metodi Ereditati

        public override void Update(GameTime gameTime)
        {
            if (this.isActivated)
            {
                PunteggioGiocatore = ContaNemiciUccisi();
                if (PossoGenerareNemici && this.Game.Level.Nemici.Count < 100)
                    GeneraNemici(NemiciPerGiro, MARGIN_STANDARD);
                if (PossoAumentareDifficoltà)
                    AumentaDifficoltà();
            }
            if (this.Game.Level.Player.Parametri.Salute <= 0 && isActivated)
            {
                StreamReader r;
                StreamWriter w;
                string[] temp;
                string PlayerName;
                int BestScore;
                int MyScore;

                r = new StreamReader(@".\Content\Scores.cfg");
                temp = r.ReadLine().Split('-');
                BestScore = Convert.ToInt32(temp[0]);
                MyScore = Convert.ToInt32(PunteggioGiocatore);
                r.Close();

                if (MyScore > BestScore && Temp == null) // NUOVO RECORD
                {
                    Temp = new InputBox("NEW BEST SCORE!!!!!!", "Enter Your Name : ");
                    Temp.ShowDialog();
                    PlayerName = Temp.ReturnValue;
                    Temp.Dispose();
                    Temp = null;
                    if (PlayerName != String.Empty)
                    {
                        w = new StreamWriter(@".\Content\Scores.cfg", false, Encoding.ASCII);
                        w.WriteLine(PunteggioGiocatore + '-' + PlayerName);
                        w.Close();
                        MessageBox.Show("NEW Best Score = " + PunteggioGiocatore + " BY " + PlayerName);
                    }
                }
                else if (MyScore <= BestScore)
                        MessageBox.Show("Best Score = " + temp[0] + " BY " + temp[1]);
                this.isActivated = false;
            }
        }  

        #endregion

        #region Calcola Punteggio

        // 50 Punti A Nemico Ucciso
        public string ContaNemiciUccisi()
        {
            return (this.Game.Level.NemiciUccisi * 50 * Game.DifficultyMultiplier).ToString();
        }

        private void AumentaDifficoltà()
        {
            this.TimerGenera.Stop();
            this.TimerGenera.Interval /= 3;
            this.TimerGenera.Interval *= 2;
            this.TimerGenera.Start();
            this.PossoAumentareDifficoltà = false;
        }

        #endregion

    }
}
