using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Timers;
using System;

namespace NerdOrDungeons
{
    /**                                                              **
     ******************************************************************
     **                                                              **
     ** Sequenza :                                                   **
     ** Classe Di Gestione Delle Storie / Monologhi Fra Un Livello   **
     ** L'altro. Stampano A Video Delle Stringhe.                    **
     **                                                              **
     ******************************************************************
     **                                                              **/

    public sealed class Sequenza : DrawableGameComponent
    {
        #region Variabili Preconfigurate

        public static Vector2 PuntoDaCuiScrivo;

        //Scrivo In Centro Allo Schermo
        static Sequenza()
        {
            float CenterX, CenterY;

            CenterX = MainGame.RiferimentoGlobaleAlGioco.GraphicsDevice.Viewport.Width  / 2f;
            CenterY = MainGame.RiferimentoGlobaleAlGioco.GraphicsDevice.Viewport.Height / 2f;
                   
            PuntoDaCuiScrivo = new Vector2(CenterX, CenterY); 
        }

        #endregion

        #region Dichiarazione Variabili

        private new MainGame Game;
        private     string SeqFilePath;
        //private Sprite Image;
        private     List<string> Phrases;       //Vengono Caricate Da Un File.seq E Scandite Con Un foreach
        private     int CurrentIndex;           //Indice Della Frase Attualmente Sullo Schermo
        private     SpriteFont SpriteCharacter; //Font Delle Frasi Disegnate
        private     bool Temporizzatore;        //Flag Che Mi Dice Se Tengo Premuto Il Tasto Invio (Blocca a Una Sola pressione)

        public      SoundEngine SequenceSound;
        public      bool isOver;                 //Dice Se Ho Finito Le Frasi (Quindi Se Devo Passare Il Controllo a Scenario)
        #endregion

        #region Costruttore

        public Sequenza(Game Game, string SeqFilePath, SoundEngine SequenceSound)
            : base(Game)
        {
            this.Game = (MainGame)Game;
            this.SeqFilePath = SeqFilePath;
            this.SequenceSound = SequenceSound;
            //this.Image = new Sprite(this.Game, SeqFilePath.Replace(".seq", ".png"));
            this.isOver = false;
            this.Temporizzatore = false;
            Inizializza();
        }

        #endregion

        #region Metodi Ereditati

        private void Inizializza() //se non ce il livello non carico sequenze
        {
            CurrentIndex = 0;
            Phrases = new List<string>();
            try
            {
                StreamReader sr = new StreamReader(this.SeqFilePath);

                while (!sr.EndOfStream)
                    Phrases.Add(sr.ReadLine());
                sr.Close();
            }
            //Non Tutti I Livelli Hanno Una Sequenza Quindi non C'è Sempre Il File
            catch { this.isOver = true; }
            Initialize();
        }


        protected override void LoadContent()
        {
            SpriteCharacter = this.Game.Content.Load<SpriteFont>(@"SpriteTesto");

            if (!this.isOver)
                this.SequenceSound.Play(true);
            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            if (!this.isOver)
            {
                //this.Image.Draw(gameTime);
                this.Game.SpriteBatch.DrawString(SpriteCharacter, Phrases[CurrentIndex], Sequenza.PuntoDaCuiScrivo - (SpriteCharacter.MeasureString(Phrases[CurrentIndex]) / 2f), Color.White);
            }
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState k = Keyboard.GetState();
            if (Temporizzatore == false)
            {
                if (this.Game.Joystick && GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.A) ||
                   (!this.Game.Joystick && k.IsKeyDown(Keys.Enter)))
                    if (CurrentIndex < Phrases.Count - 1)
                    {
                        CurrentIndex++;
                        Temporizzatore = true;
                    }
                    else
                        this.isOver = true;
            }
            else if ((!this.Game.Joystick && k.IsKeyUp(Keys.Enter)) || (this.Game.Joystick && GamePad.GetState(PlayerIndex.One).IsButtonUp(Buttons.A))) Temporizzatore = false;

            if (this.isOver)
                this.SequenceSound.Stop();
        }

        protected override void Dispose(bool disposing)
        {

            //this.Image.Dispose();
            if (this.Phrases != null)
                this.Phrases.Clear();
            base.Dispose(disposing);
        }

        #endregion


    }
}
