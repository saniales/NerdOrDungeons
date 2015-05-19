using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace NerdOrDungeons
{
   /**                                                              **
    ******************************************************************
    **                                                              **
    ** Personaggio --> Eredita da Agente :                          **
    ** Usata Per Distinguere da "Nemico" e Per Identificare il      **
    ** Personaggio Controllato dall'Utente                          **
    **                                                              **
    **                   [Roberto Mana Akbar]                       **
    **                                                              **
    ******************************************************************
    **                                                              **/

    public class Personaggio : Agente
    {

        #region Modelli Personaggi Precostruiti (devo assegnargli un MainGame dopo aver copiato)


        public static Dictionary<string, Personaggio> Istances;
        public static string DefaultPath;

        internal static void InizializzaClasse()
        {
            Proiettile.InizializzaClasse();
            DefaultPath = @".\Content\Players\"; 
            Istances = new Dictionary<string, Personaggio>();
            Istances.Add("LinkBlue"  , new Personaggio(MainGame.RiferimentoGlobaleAlGioco, Vector2.Zero, ParametriPersonaggio.LinkBlue.Copy()           , DefaultPath + "LinkBlue"  , Proiettile.Types["FlameShot"]      , false, false));
            Istances.Add("LinkRed"   , new Personaggio(MainGame.RiferimentoGlobaleAlGioco, Vector2.Zero, ParametriPersonaggio.LinkRed.Copy()            , DefaultPath + "LinkRed"   , Proiettile.Types["RedFlameShot"]   ,  true, false));
            Istances.Add("LinkPurple", new Personaggio(MainGame.RiferimentoGlobaleAlGioco, Vector2.Zero, ParametriPersonaggio.LinkPurple.Copy()         , DefaultPath + "LinkPurple", Proiettile.Types["PurpleFlameShot"],  true, false));
            Istances.Add("LinkBlack" , new Personaggio(MainGame.RiferimentoGlobaleAlGioco, Vector2.Zero, ParametriPersonaggio.LinkDark.Copy()           , DefaultPath + "LinkBlack" , Proiettile.Types["DarkFlameShot"]  ,  true, false));
            Istances.Add("PlayerShip", new Personaggio(MainGame.RiferimentoGlobaleAlGioco, Vector2.Zero, ParametriPersonaggio.PlayerShip.Copy()         , DefaultPath + "PlayerShip", Proiettile.Types["CannonBall"]     ,  true,  true));
            Istances.Add("Caius"     , new Personaggio(MainGame.RiferimentoGlobaleAlGioco, Vector2.Zero, ParametriPersonaggio.Caius.Copy()              , DefaultPath + "Caius"     , Proiettile.Types["Spear"]          ,  true, false));
            Istances.Add("Tomonobu"  , new Personaggio(MainGame.RiferimentoGlobaleAlGioco, Vector2.Zero, ParametriPersonaggio.TomonobuMultiplayer.Copy(), DefaultPath + "Tomonobu"  , Proiettile.Types["FlameShot"]      ,  true, false));
        }

        #endregion

        #region Dichiarazione Variabili

        public bool isVehicle;

        public Controller[] PlayerInput = new Controller[4];

        public  bool[] IndiceGiocatore = new bool[4] {false, false, false, false};

        private bool Multiplayer;
        public static bool loaded;

        #endregion

        #region Costruttori

        //COSTRUTTORE PER COPIA
        public Personaggio(Personaggio PersonaggioDaCopiare, Vector2 Posizione)
        : this(PersonaggioDaCopiare.Game, Posizione, PersonaggioDaCopiare.Parametri, PersonaggioDaCopiare.SpriteFolderPath, PersonaggioDaCopiare.ProiettileCheSparo,PersonaggioDaCopiare.PossoSparare ,PersonaggioDaCopiare.isVehicle) {}

        //COSTRUTTORE CHE CONSENTE DI IMPOSTARE SE POSSO SPARARE O NO (PER I LOAD GAME)
        public Personaggio(Game Game, Vector2 Posizione, ParametriPersonaggio Parametri, string SpriteFolderPath, Proiettile Projectile, bool CanShot, bool isVehicle)
        : base(Game, Posizione, Parametri, SpriteFolderPath, Projectile)
        {
            this.PossoSparare = CanShot;
            this.isVehicle    = isVehicle;
        }  

        //DI DEFAULT NON PUO' SPARARE (SERVE LEGGERE UNA PERGAMENA)
        public Personaggio(Game Game, Vector2 Posizione, ParametriPersonaggio Parametri, string SpriteFolderPath, Proiettile Projectile, bool isVehicle)
        : this(Game, Posizione, Parametri, SpriteFolderPath, Projectile, false, isVehicle) { }

        //Abilita Il Multiplayer Su Arena
        public void AbilitaMultiplayer()
        { this.PossoSparare = true; this.Multiplayer = true; }

        #endregion

        #region MultiPlayer

        internal void Muori()
        {
            //Se non Ho Gia Cancellato il Livello Precedentemente (A Causa Di Altri Eventi)
            if(this.Game.Level != null)
                this.Game.Level.LevelTrack.Stop();
            SoundEngine.Tracks["Death"].Play();
            Current = Sprites[DEAD];
            DisabilitaMovimenti = true;
        }

        internal void MuoriMultiplayer()
        {
            Current = Sprites[DEAD];
            this.ListaProiettili.Clear();
            DisabilitaMovimenti = true;
        }

        internal void MovimentoTastiera1(GameTime gameTime)
        { ImpostaMovimenti(DirezioneCorrente.Manuale, gameTime); }

        internal void MovimentoTastiera2(GameTime gameTime)
        {
            KeyboardState k = Keyboard.GetState();

            if (k.IsKeyDown(Keys.S))
                ImpostaMovimenti(DirezioneCorrente.Giù, gameTime);
            else if (k.IsKeyDown(Keys.W))
                ImpostaMovimenti(DirezioneCorrente.Su, gameTime);
            else if (k.IsKeyDown(Keys.A))
                ImpostaMovimenti(DirezioneCorrente.Sinistra, gameTime);
            else if (k.IsKeyDown(Keys.D))
                ImpostaMovimenti(DirezioneCorrente.Destra, gameTime);
            else if (k.IsKeyDown(Keys.E))
                ImpostaMovimenti(DirezioneCorrente.Spara, gameTime);
            else
                ImpostaMovimenti(DirezioneCorrente.Fermo, gameTime);
        }

        internal void MovimentoTastiera3(GameTime gameTime)
        {
            KeyboardState k = Keyboard.GetState();

            if (k.IsKeyDown(Keys.D5))
                ImpostaMovimenti(DirezioneCorrente.Giù, gameTime);
            else if (k.IsKeyDown(Keys.D8))
                ImpostaMovimenti(DirezioneCorrente.Su, gameTime);
            else if (k.IsKeyDown(Keys.D4))
                ImpostaMovimenti(DirezioneCorrente.Sinistra, gameTime);
            else if (k.IsKeyDown(Keys.D6))
                ImpostaMovimenti(DirezioneCorrente.Destra, gameTime);
            else if (k.IsKeyDown(Keys.D9))
                ImpostaMovimenti(DirezioneCorrente.Spara, gameTime);
            else
                ImpostaMovimenti(DirezioneCorrente.Fermo, gameTime);
        }

        internal void MovimentoTastiera4(GameTime gameTime)
        {
            KeyboardState k = Keyboard.GetState();

            if (k.IsKeyDown(Keys.K))
                ImpostaMovimenti(DirezioneCorrente.Giù, gameTime);
            else if (k.IsKeyDown(Keys.I))
                ImpostaMovimenti(DirezioneCorrente.Su, gameTime);
            else if (k.IsKeyDown(Keys.J))
                ImpostaMovimenti(DirezioneCorrente.Sinistra, gameTime);
            else if (k.IsKeyDown(Keys.L))
                ImpostaMovimenti(DirezioneCorrente.Destra, gameTime);
            else if (k.IsKeyDown(Keys.O))
                ImpostaMovimenti(DirezioneCorrente.Spara, gameTime);
            else
                ImpostaMovimenti(DirezioneCorrente.Fermo, gameTime);
        }

        internal void MovimentoPad(GameTime gameTime,PlayerIndex player)
        {
            DirezioneCorrente Dummy = DirezioneCorrente.Fermo;
            GamePadState gamepad = GamePad.GetState(player);
            if (gamepad.IsButtonDown(Buttons.LeftThumbstickDown))
                Dummy = DirezioneCorrente.Giù;
            else if (gamepad.IsButtonDown(Buttons.LeftThumbstickUp))
                Dummy = DirezioneCorrente.Su;
            else if (gamepad.IsButtonDown(Buttons.LeftThumbstickLeft))
                Dummy = DirezioneCorrente.Sinistra;
            else if (gamepad.IsButtonDown(Buttons.LeftThumbstickRight))
                Dummy = DirezioneCorrente.Destra;
            if (gamepad.IsButtonDown(Buttons.X))
                Dummy = DirezioneCorrente.Spara;
            else if (gamepad.IsButtonDown(Buttons.A))
                Dummy = DirezioneCorrente.Attacco;
            ImpostaMovimenti(Dummy,gameTime);
        }

        internal void VerificaMovimenti4Giocatori(GameTime gameTime)
        {
            int i;
            for(i = 0; i < PlayerInput.Length; i++)
            {
                if(this.IndiceGiocatore[i] != false)
                {
                    switch(this.PlayerInput[i])
                    {
                    case Controller.KEYB1:
                        MovimentoTastiera1(gameTime);
                        break;
                    case Controller.KEYB2:
                        MovimentoTastiera2(gameTime);
                        break;
                    case Controller.KEYB3:
                        MovimentoTastiera3(gameTime);
                        break;
                    case Controller.KEYB4:
                        MovimentoTastiera4(gameTime);
                        break;
                    case Controller.PAD1:
                        MovimentoPad(gameTime, PlayerIndex.One);
                        break;
                    case Controller.PAD2:
                        MovimentoPad(gameTime, PlayerIndex.Two);
                        break;
                    case Controller.PAD3:
                        MovimentoPad(gameTime, PlayerIndex.Three);
                        break;
                    case Controller.PAD4:
                        MovimentoPad(gameTime, PlayerIndex.Four);
                        break;
                    }
                }
            }
        }

        #endregion

        #region Membri Ereditati

        public override void Update(GameTime gameTime)
        {
            if (!DisabilitaMovimenti)
            {
                if (!this.Multiplayer)
                {
                    if ((this.isVehicle && !Keyboard.GetState().IsKeyDown(Keys.Space)) || !this.isVehicle)
                        if (!this.Game.Joystick)
                            ImpostaMovimenti(DirezioneCorrente.Manuale, gameTime);
                        else
                            MovimentoPad(gameTime, PlayerIndex.One);
                }
                else
                    VerificaMovimenti4Giocatori(gameTime);

                base.Update(gameTime);
            }
        }

        public Personaggio Copy(Vector2 Posizione)
        { return new Personaggio(this, Posizione); }

        #endregion

    }
}
