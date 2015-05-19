using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace NerdOrDungeons
{
    /**                                                               **
     *******************************************************************
     **                                                               **
     ** SoundEngine :                                                 **
     ** Libreria Di Gestione Di Ogni istanza Di Suono Del Game.       **
     ** Va Intesa Come Una Libreria Di Suoni, infatti assegno un      **
     ** Suono usando SoundEngine.Tracks["NomeTraccia"];               **
     ** Oppure un Effetto Sonoro (SoundEngine.Effects["NomeEffetto"]; **
     **                                                               **
     *******************************************************************
     **                                                               **/

    public sealed class SoundEngine
    {
        #region Dichiarazione Costanti Tracks Precaricate

        public    static Dictionary<string, SoundEngine> Tracks;
        public    static Dictionary<string, SoundEngine> Effects;
        public    static string                          TracksPath;
        public    static string                          EffectsPath;
                  
        #region Identificatore Ultima Track (Serve a Tenere Traccia e Ad Assegnare ID Diversi)

        private static int lastID;

        #endregion

        static SoundEngine() {
            lastID = 0;
            TracksPath  = @".\Content\Sounds\Tracks\" ;
            EffectsPath = @".\Content\Sounds\Effects\";

            Tracks  = new Dictionary<string, SoundEngine>();
            Effects = new Dictionary<string, SoundEngine>();

            //Ogni Boss Passato Cambio Le Tracks(Durante e Dopo Il Level Boss si Cambia Canzone)
            Tracks.Add("Level1", new SoundEngine(TracksPath + @"Track1.wav"));
            Tracks.Add("Level2", Tracks["Level1"]);
            Tracks.Add("Level3", Tracks["Level1"]);
            Tracks.Add("Level4", Tracks["Level1"]);

            Tracks.Add("Level5", new SoundEngine(TracksPath + @"Track2.wav"));      //Boss1

            Tracks.Add("Level6" , new SoundEngine(TracksPath + @"Track3.wav"));
            Tracks.Add("Level7" , Tracks["Level6"]);
            Tracks.Add("Level8" , Tracks["Level6"]);
            Tracks.Add("Level9" , Tracks["Level6"]);
            Tracks.Add("Level10", Tracks["Level6"]);

            Tracks.Add("Level11", new SoundEngine(TracksPath + @"Track4.wav"));     //Boss2


            Tracks.Add("Level12", new SoundEngine(TracksPath + @"Track5.wav"));
            Tracks.Add("Level13", Tracks["Level12"]);
            Tracks.Add("Level14", Tracks["Level12"]);
            Tracks.Add("Level15", Tracks["Level12"]);

            Tracks.Add("Level16", new SoundEngine(TracksPath + @"Track6.wav"));     //Boss3

            Tracks.Add("Level17", new SoundEngine(TracksPath + @"Track7.wav")); 
            Tracks.Add("Level18", Tracks["Level17"]);
            Tracks.Add("Level19", Tracks["Level17"]);
            Tracks.Add("Level20", Tracks["Level17"]);

            Tracks.Add("Level21", new SoundEngine(TracksPath + @"Track8.wav"));   //Boss4

            Tracks.Add("Level22", new SoundEngine(TracksPath + @"Track9.wav"));
            Tracks.Add("Level23", Tracks["Level22"]);
            Tracks.Add("Level24", Tracks["Level22"]);
            Tracks.Add("Level25", Tracks["Level22"]);

            Tracks.Add("Level26", new SoundEngine(TracksPath + @"Track10.wav"));  //Boss5

            Tracks.Add("Level27", new SoundEngine(TracksPath + @"Track11.wav"));
            Tracks.Add("Level28", Tracks["Level27"]);
            Tracks.Add("Level29", Tracks["Level27"]);
            Tracks.Add("Level30", Tracks["Level27"]);

            Tracks.Add("Level31", new SoundEngine(TracksPath + @"Track12.wav"));   //Final Boss

            Tracks.Add("Level32", new SoundEngine(TracksPath + @"Track13.wav"));
            Tracks.Add("Level33", new SoundEngine(TracksPath + @"Epilogue.wav")); //Final Dialogue & Epilogue
                              
            //Qui Carico Gli Effetti Sonori
            Effects.Add("PlayerSwordAttack", new SoundEngine(EffectsPath + "PlayerSwordAttack.wav"));
            Effects.Add("ScrollTouched"    , new SoundEngine(EffectsPath + "ScrollTouched.wav"    ));
            Effects.Add("PowerUpTouched"   , new SoundEngine(EffectsPath + "PowerUpTouched.wav"   ));
            Effects.Add("PlayerShot"       , new SoundEngine(EffectsPath + "PlayerShot.wav"       ));
            Effects.Add("EnemyKilled"      , new SoundEngine(EffectsPath + "EnemyKilled.wav"      ));
            Effects.Add("SwitchPressed"    , new SoundEngine(EffectsPath + "SwitchPressed.wav"    ));
            Effects.Add("CannonShot"       , new SoundEngine(EffectsPath + "CannonShot.wav"       ));
            Effects.Add("LittleDevilLaugh" , new SoundEngine(EffectsPath + "LittleDevilLaugh.wav" ));

            //Carico Sequences Tracks
            Tracks.Add("Menu"      , new SoundEngine(TracksPath + @"Menu.wav"));
            Tracks.Add("Death"     , Tracks["Menu"]   );
            Tracks.Add("Sequence1" , Tracks["Menu"]   );
            Tracks.Add("Sequence6" , Tracks["Level6"] );
            Tracks.Add("Sequence11", Tracks["Level11"]);
            Tracks.Add("Sequence12", Tracks["Level11"]);
            Tracks.Add("Sequence16", Tracks["Level16"]);
            Tracks.Add("Sequence17", Tracks["Level17"]);
            Tracks.Add("Sequence22", Tracks["Level22"]);
            Tracks.Add("Sequence26", Tracks["Level26"]);
            Tracks.Add("Sequence27", Tracks["Level27"]);
            Tracks.Add("Sequence31", Tracks["Level31"]);
            Tracks.Add("Sequence32", Tracks["Level32"]);
            Tracks.Add("Sequence33", Tracks["Level33"]);//Epilogo
        }

        #endregion
        
        #region Variabili

        private string              SoundPath;
        private SoundEffectInstance SoundInstance;
        public int ID;

        #endregion

        #region Costruttori

        public SoundEngine(string SoundPath) {
            this.SoundPath = SoundPath;
            this.SoundInstance = SoundEffect.FromStream(File.OpenRead(SoundPath)).CreateInstance();
            this.ID = ++lastID;
        }

        #endregion

        #region Metodi Ereditati (Wrappers Di SoundEffectInstance)

        public void Play()
        {
            if (this.SoundInstance.State == SoundState.Paused)
                this.SoundInstance.Resume();
            if (this.SoundInstance.State == SoundState.Stopped)
                this.SoundInstance.Play();
            //se è gia in play non faccio un picchio
        }

        public void Play(bool Loop)
        {
            try { this.SoundInstance.IsLooped = Loop; }
            catch (Exception) { }
            if (this.SoundInstance.State == SoundState.Paused)
                this.Resume();
            if (this.SoundInstance.State == SoundState.Stopped)
                this.Play();              
        }

        public void Stop()
        {
            this.SoundInstance.Stop();
        }

        public void Pause()
        {
            if (this.SoundInstance.State == SoundState.Playing)
                this.SoundInstance.Pause();
        }

        public void Resume()
        {
            if (this.SoundInstance.State == SoundState.Paused)
                this.Resume();
            if (this.SoundInstance.State == SoundState.Stopped)
                this.Play();
        }

        public void Dispose()
        { this.SoundInstance.Dispose(); }

        public override bool Equals(object obj)
        {
            try {
                if (this.ID == ((SoundEngine)obj).ID)
                    return true;
                else
                    return false;
            }
            catch(Exception)
            { return false; }
        }

        #endregion
    }
}