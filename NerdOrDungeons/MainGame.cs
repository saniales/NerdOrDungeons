using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using System;

namespace NerdOrDungeons
{
    public class MainGame : Microsoft.Xna.Framework.Game
    {
        public static MainGame RiferimentoGlobaleAlGioco;

        public SpriteBatch SpriteBatch;
        public Scenario Level;
        public Arena ArenaMultiplayer;
        public Menu MenuPrincipale;
        public float DifficultyMultiplier;
        public bool  Joystick;

        public bool PossoUscire;

        private GraphicsDeviceManager graphics;
        
        public MainGame() {
            //Posso Uscire Solo Dal Portale EXIT (Non Cliccando Sulla X)
            ((Form)Form.FromHandle(this.Window.Handle)).FormClosing += new FormClosingEventHandler(ImpedisciChiusura);
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = @"Content";
            MainGame.RiferimentoGlobaleAlGioco = this;
            PossoUscire = false;
            SettingsLoad();
        }

        public void SettingsLoad()
        {
            StreamReader r = new StreamReader(@".\Content\GameSettings.cfg");
            string[] temp = r.ReadLine().Split('/');
            r.Close();
            r.Dispose();
            this.Joystick = Convert.ToBoolean(temp[0]);
            this.DifficultyMultiplier = Convert.ToSingle(temp[1]);
        }

        private void ImpedisciChiusura(object sender, FormClosingEventArgs e)
        { e.Cancel = !PossoUscire; }

        protected override void LoadContent() 
        { this.MenuPrincipale = new Menu(this, @".\Content\Menu"); }

        protected override void Update(GameTime gameTime)
        {
            if (MenuPrincipale != null)
                MenuPrincipale.Update(gameTime);
            else if (Level != null)
                Level.Update(gameTime);
            else if (ArenaMultiplayer != null)
                ArenaMultiplayer.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            this.SpriteBatch.Begin();

            if      (MenuPrincipale != null)
                MenuPrincipale.Draw(gameTime);
            else if (Level != null) // Se sono nel menu non ho il livello istanziato
                Level.Draw(gameTime);
            else if (ArenaMultiplayer != null)
                ArenaMultiplayer.Draw(gameTime);

            this.SpriteBatch.End();
        }
    }
}
