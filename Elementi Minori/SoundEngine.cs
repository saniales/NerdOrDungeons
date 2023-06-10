using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;

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

        public static void initialize(ContentManager ContentManager) {
            /* Mediaplayer Settings */
            MediaPlayer.IsMuted = false;
            MediaPlayer.IsShuffled = false;
            MediaPlayer.Volume = 0.5f;
            /*  Static  Constructor */
            lastID = 0;
            TracksPath  = @"Sounds/Tracks/" ;
            EffectsPath = @"Sounds/Effects/";

            Tracks  = new Dictionary<string, SoundEngine>();
            Effects = new Dictionary<string, SoundEngine>();

            SoundEngine temp = new SoundEngine(ContentManager, TracksPath + @"Track1", SoundType.Music);
            //Ogni Boss Passato Cambio Le Tracks(Durante e Dopo Il Level Boss si Cambia Canzone)
            Tracks.Add("Level1", temp);
            Tracks.Add("Level2", temp);
            Tracks.Add("Level3", temp);
            Tracks.Add("Level4", temp);

            temp = new SoundEngine(ContentManager, TracksPath + @"Track2", SoundType.Music);
            Tracks.Add("Level5", temp);      //Boss1

            temp = new SoundEngine(ContentManager, TracksPath + @"Track3", SoundType.Music);
            Tracks.Add("Sequence6", temp);
            Tracks.Add("Level6", temp);
            Tracks.Add("Level7", temp);
            Tracks.Add("Level8", temp);
            Tracks.Add("Level9", temp);
            Tracks.Add("Level10", temp);

            temp = new SoundEngine(ContentManager, TracksPath + @"Track4", SoundType.Music);
            Tracks.Add("Sequence11", temp);
            Tracks.Add("Sequence12", temp);
            Tracks.Add("Level11", temp);     //Boss2

            temp = new SoundEngine(ContentManager, TracksPath + @"Track5", SoundType.Music);
            Tracks.Add("Level12", temp);
            Tracks.Add("Level13", temp);
            Tracks.Add("Level14", temp);
            Tracks.Add("Level15", temp);

            temp = new SoundEngine(ContentManager, TracksPath + @"Track6", SoundType.Music);
            Tracks.Add("Sequence16", temp);
            Tracks.Add("Level16", temp);     //Boss3

            temp = new SoundEngine(ContentManager, TracksPath + @"Track7", SoundType.Music);
            Tracks.Add("Sequence17", temp);
            Tracks.Add("Level17", temp); 
            Tracks.Add("Level18", temp);
            Tracks.Add("Level19", temp);
            Tracks.Add("Level20", temp);

            temp = new SoundEngine(ContentManager, TracksPath + @"Track8", SoundType.Music);
            Tracks.Add("Level21", temp);   //Boss4

            temp = new SoundEngine(ContentManager, TracksPath + @"Track9", SoundType.Music);
            Tracks.Add("Sequence22", temp);
            Tracks.Add("Level22", temp);
            Tracks.Add("Level23", temp);
            Tracks.Add("Level24", temp);
            Tracks.Add("Level25", temp);

            temp = new SoundEngine(ContentManager, TracksPath + @"Track10", SoundType.Music);
            Tracks.Add("Sequence26", temp);
            Tracks.Add("Level26", temp);  //Boss5

            temp = new SoundEngine(ContentManager, TracksPath + @"Track11", SoundType.Music);
            Tracks.Add("Sequence27", temp);
            Tracks.Add("Level27", temp);
            Tracks.Add("Level28", temp);
            Tracks.Add("Level29", temp);
            Tracks.Add("Level30", temp);

            temp = new SoundEngine(ContentManager, TracksPath + @"Track12", SoundType.Music);
            Tracks.Add("Sequence31", temp);
            Tracks.Add("Level31", temp);   //Final Boss

            temp = new SoundEngine(ContentManager, TracksPath + @"Track13", SoundType.Music);
            Tracks.Add("Sequence32", temp);
            Tracks.Add("Level32", temp);

            temp = new SoundEngine(ContentManager, TracksPath + @"Epilogue", SoundType.Music);
            Tracks.Add("Sequence33", temp);
            Tracks.Add("Level33", temp); //Final Dialogue & Epilogue
                              
            //Qui Carico Gli Effetti Sonori
            Effects.Add("PlayerSwordAttack", new SoundEngine(ContentManager, EffectsPath + "PlayerSwordAttack", SoundType.Effect));
            Effects.Add("ScrollTouched"    , new SoundEngine(ContentManager, EffectsPath + "ScrollTouched"    , SoundType.Effect));
            Effects.Add("PowerUpTouched"   , new SoundEngine(ContentManager, EffectsPath + "PowerUpTouched"   , SoundType.Effect));
            Effects.Add("PlayerShot"       , new SoundEngine(ContentManager, EffectsPath + "PlayerShot"       , SoundType.Effect));
            Effects.Add("EnemyKilled"      , new SoundEngine(ContentManager, EffectsPath + "EnemyKilled"      , SoundType.Effect));
            Effects.Add("SwitchPressed"    , new SoundEngine(ContentManager, EffectsPath + "SwitchPressed"    , SoundType.Effect));
            Effects.Add("CannonShot"       , new SoundEngine(ContentManager, EffectsPath + "CannonShot"       , SoundType.Effect));
            Effects.Add("LittleDevilLaugh" , new SoundEngine(ContentManager, EffectsPath + "LittleDevilLaugh" , SoundType.Effect));

            //Carico Sequences Tracks
            temp = new SoundEngine(ContentManager, TracksPath + @"Menu", SoundType.Music);
            Tracks.Add("Menu", temp);
            Tracks.Add("Death"     , temp);
            Tracks.Add("Sequence1" , temp);
            
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

        public SoundEngine(ContentManager ContentManager, string SoundPath, SoundType Type) {
            this.SoundPath = Directory.GetCurrentDirectory() + "\\" + SoundPath;
            this.SoundType = Type;
            this.ID = ++lastID;
            switch (this.SoundType)
            {
                case SoundType.Effect :
                    this.SoundInstance = ContentManager.Load<SoundEffect>(SoundPath).CreateInstance();
                    break;
                case SoundType.Music :
                    //inizializzo Musica
                    this.MusicInstance = ContentManager.Load<Song>(SoundPath);
                    break;
                default :
                    throw new ArgumentException("Sound Type ERROR : il Parametro \"SoundType\" non è stato assegnato correttamente");
            }            
        }

        #endregion

        #region Metodi Ereditati (Wrappers Di SoundEffectInstance)

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