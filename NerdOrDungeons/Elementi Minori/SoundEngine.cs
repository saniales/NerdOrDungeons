using System;
using System.Windows;
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
            /* Mediaplayer Settings */
            MediaPlayer.IsMuted = false;
            MediaPlayer.IsShuffled = false;
            MediaPlayer.IsVisualizationEnabled = false;
            MediaPlayer.Volume = 0.5f;
            /*  Static  Constructor */
            lastID = 0;
            TracksPath  = @"Content\Sounds\Tracks\" ;
            EffectsPath = @"Content\Sounds\Effects\";

            Tracks  = new Dictionary<string, SoundEngine>();
            Effects = new Dictionary<string, SoundEngine>();

            //Ogni Boss Passato Cambio Le Tracks(Durante e Dopo Il Level Boss si Cambia Canzone)
            Tracks.Add("Level1", new SoundEngine(TracksPath + @"Track1.mp3", SoundType.Music));
            Tracks.Add("Level2", Tracks["Level1"]);
            Tracks.Add("Level3", Tracks["Level1"]);
            Tracks.Add("Level4", Tracks["Level1"]);

            Tracks.Add("Level5", new SoundEngine(TracksPath + @"Track2.mp3", SoundType.Music));      //Boss1

            Tracks.Add("Level6", new SoundEngine(TracksPath + @"Track3.mp3", SoundType.Music));
            Tracks.Add("Level7", Tracks["Level6"]);
            Tracks.Add("Level8", Tracks["Level6"]);
            Tracks.Add("Level9", Tracks["Level6"]);
            Tracks.Add("Level10", Tracks["Level6"]);

            Tracks.Add("Level11", new SoundEngine(TracksPath + @"Track4.mp3", SoundType.Music));     //Boss2

            Tracks.Add("Level12", new SoundEngine(TracksPath + @"Track5.mp3", SoundType.Music));
            Tracks.Add("Level13", Tracks["Level12"]);
            Tracks.Add("Level14", Tracks["Level12"]);
            Tracks.Add("Level15", Tracks["Level12"]);

            Tracks.Add("Level16", new SoundEngine(TracksPath + @"Track6.mp3", SoundType.Music));     //Boss3

            Tracks.Add("Level17", new SoundEngine(TracksPath + @"Track7.mp3", SoundType.Music)); 
            Tracks.Add("Level18", Tracks["Level17"]);
            Tracks.Add("Level19", Tracks["Level17"]);
            Tracks.Add("Level20", Tracks["Level17"]);

            Tracks.Add("Level21", new SoundEngine(TracksPath + @"Track8.mp3", SoundType.Music));   //Boss4

            Tracks.Add("Level22", new SoundEngine(TracksPath + @"Track9.mp3", SoundType.Music));
            Tracks.Add("Level23", Tracks["Level22"]);
            Tracks.Add("Level24", Tracks["Level22"]);
            Tracks.Add("Level25", Tracks["Level22"]);

            Tracks.Add("Level26", new SoundEngine(TracksPath + @"Track10.mp3", SoundType.Music));  //Boss5

            Tracks.Add("Level27", new SoundEngine(TracksPath + @"Track11.mp3", SoundType.Music));
            Tracks.Add("Level28", Tracks["Level27"]);
            Tracks.Add("Level29", Tracks["Level27"]);
            Tracks.Add("Level30", Tracks["Level27"]);

            Tracks.Add("Level31", new SoundEngine(TracksPath + @"Track12.mp3", SoundType.Music));   //Final Boss

            Tracks.Add("Level32", new SoundEngine(TracksPath + @"Track13.mp3", SoundType.Music));
            Tracks.Add("Level33", new SoundEngine(TracksPath + @"Epilogue.mp3", SoundType.Music)); //Final Dialogue & Epilogue
                              
            //Qui Carico Gli Effetti Sonori
            Effects.Add("PlayerSwordAttack", new SoundEngine(EffectsPath + "PlayerSwordAttack.wav", SoundType.Effect));
            Effects.Add("ScrollTouched"    , new SoundEngine(EffectsPath + "ScrollTouched.wav"    , SoundType.Effect));
            Effects.Add("PowerUpTouched"   , new SoundEngine(EffectsPath + "PowerUpTouched.wav"   , SoundType.Effect));
            Effects.Add("PlayerShot"       , new SoundEngine(EffectsPath + "PlayerShot.wav"       , SoundType.Effect));
            Effects.Add("EnemyKilled"      , new SoundEngine(EffectsPath + "EnemyKilled.wav"      , SoundType.Effect));
            Effects.Add("SwitchPressed"    , new SoundEngine(EffectsPath + "SwitchPressed.wav"    , SoundType.Effect));
            Effects.Add("CannonShot"       , new SoundEngine(EffectsPath + "CannonShot.wav"       , SoundType.Effect));
            Effects.Add("LittleDevilLaugh" , new SoundEngine(EffectsPath + "LittleDevilLaugh.wav" , SoundType.Effect));

            //Carico Sequences Tracks
            Tracks.Add("Menu", new SoundEngine(TracksPath + @"Menu.mp3", SoundType.Music));
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
        private Song                MusicInstance;
        public  int       ID;
        public  SoundType SoundType;

        #endregion

        #region Costruttori

        public SoundEngine(string SoundPath, SoundType Type) {
            this.SoundPath = Directory.GetCurrentDirectory() + "\\" + SoundPath;
            this.SoundType = Type;
            this.ID = ++lastID;
            switch (this.SoundType)
            {
                case SoundType.Effect :
                    this.SoundInstance = SoundEffect.FromStream(File.OpenRead(this.SoundPath)).CreateInstance();
                    break;
                case SoundType.Music : 
                    //inizializzo Musica
                    this.MusicInstance = Song.FromUri(this.ID.ToString(), new Uri(this.SoundPath.Replace(" ", "%20")));
                    break;
                default :
                    throw new ArgumentException("Sound Type ERROR : il Parametro \"SoundType\" non è stato assegnato correttamente");
            }            
        }

        #endregion

        #region Metodi Ereditati (Wrappers Dis SoundEffectInstance)

        public void Play()
        { this.Play(false); }

        public void Play(bool Loop)
        {
            if (this.SoundType == SoundType.Effect)
            {
                try { this.SoundInstance.IsLooped = Loop; }
                catch (Exception) { }
                if (this.SoundInstance.State != SoundState.Playing)
                    this.SoundInstance.Play();
            }
            else /* if(this.SoundType == SoundType.Music) */
            {
                MediaPlayer.IsRepeating = Loop;
                if(MediaPlayer.State != MediaState.Playing)
                    MediaPlayer.Play(this.MusicInstance);
            }
        }

        public void Stop()
        {
            if (this.SoundType == SoundType.Effect)
                this.SoundInstance.Stop();
            else /* if(this.SoundType == SoundType.Music) */
                MediaPlayer.Stop();
        }

        public void Pause()
        {
            if (this.SoundType == SoundType.Effect)
                this.SoundInstance.Pause();
            else /* if(this.SoundType == SoundType.Music) */
                MediaPlayer.Pause();
        }

        public void Dispose()
        {
            if (this.SoundType == SoundType.Effect)
                this.SoundInstance.Dispose();
            else /* if(this.SoundType == SoundType.Music) */
                this.MusicInstance.Dispose();
        }

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