namespace NerdOrDungeons
{
    /**                                                              **
     ******************************************************************
     **                                                              **
     ** ParametriPersonaggio:                                        **
     ** Classe Di Gestione Dei Parametri Di Persoonaggi Controllati  **
     ** dall'Utente, Nemici, Alleati.                                **
     **                                                              **
     ******************************************************************
     **                                                              **/ 

    public class ParametriPersonaggio
    {
        #region Variabili Precaricate (Nemici e Personaggi)

        public static ParametriPersonaggio Empty;

        //Nemici
        public static ParametriPersonaggio  OctorockRed;
        public static ParametriPersonaggio  Goomba;
        public static ParametriPersonaggio  Snake;
        public static ParametriPersonaggio  EnemyShip;
        public static ParametriPersonaggio  WaterTank;
        public static ParametriPersonaggio  EarthTank;
        public static ParametriPersonaggio  LittleDevil;

        //Bosses
        public static ParametriPersonaggio  MegaOctorockRed;
        public static ParametriPersonaggio  Prematurator;
        public static ParametriPersonaggio  AntiSganon; 
        public static ParametriPersonaggio  MegaWaterTank;
        public static ParametriPersonaggio  FiendDragon;
        public static ParametriPersonaggio  Tomonobu;

        //Players
        public static ParametriPersonaggio LinkBlue;
        public static ParametriPersonaggio LinkRed;
        public static ParametriPersonaggio LinkPurple;
        public static ParametriPersonaggio LinkDark;
        public static ParametriPersonaggio LinkPotenziato;
        public static ParametriPersonaggio PlayerShip;
        public static ParametriPersonaggio Caius;
        public static ParametriPersonaggio TomonobuMultiplayer;

        //Allies
        public static ParametriPersonaggio AllyPrematurator;

        //Missiles
        public static ParametriPersonaggio Rocket;

        static ParametriPersonaggio()
        {
            Empty               = new ParametriPersonaggio(0    , 0   , 0.0f);
                   
            OctorockRed         = new ParametriPersonaggio(200  ,     5, 1.0f);
            Goomba              = new ParametriPersonaggio(400  ,    10, 1.0f);
            Snake               = new ParametriPersonaggio(1000 ,    50, 2.0f);
            EnemyShip           = new ParametriPersonaggio(1000 ,    50, 1.0f);
            WaterTank           = new ParametriPersonaggio(1000 ,   100, 1.0f);
            EarthTank           = new ParametriPersonaggio(1000 ,     0, 1.0f);
            LittleDevil         = new ParametriPersonaggio(2000 ,    10, 2.0f);
            
                                     
            MegaOctorockRed     = new ParametriPersonaggio(2000 ,    20, 0.5f);
            Prematurator        = new ParametriPersonaggio(10000,  1000, 0.2f);
            AntiSganon          = new ParametriPersonaggio(20000,     0, 1.0f);
            MegaWaterTank       = new ParametriPersonaggio(20000,  1000, 1.5f);
            FiendDragon         = new ParametriPersonaggio(22500,    50, 0.0f);
            Tomonobu            = new ParametriPersonaggio(25000,     0, 5.0f);
                                       
            LinkBlue            = new ParametriPersonaggio( 1000,    20, 1.5f);
            LinkRed             = new ParametriPersonaggio( 1500,     0, 2.0f);
            LinkPurple          = new ParametriPersonaggio( 3500,     0, 1.0f);
            LinkDark            = new ParametriPersonaggio(  500,     0, 3.0f);
            Caius               = new ParametriPersonaggio(30000,     0, 0.5f);
            TomonobuMultiplayer = new ParametriPersonaggio( 3000,     0, 2.5f);
            LinkPotenziato      = new ParametriPersonaggio(30000,   200, 2.5f);

            PlayerShip          = new ParametriPersonaggio( 1000,     0, 1.5f);   
            Rocket              = new ParametriPersonaggio(    0,   500, 2.0f);
        }

        #endregion

        #region Variabili

        public int   Salute;
        public int   Attacco;
        public float Velocità;

        #endregion

        #region Costruttori

        internal ParametriPersonaggio(int Salute, int Attacco, float Velocità) 
        {
            this.Salute = Salute;
            this.Attacco = Attacco;
            this.Velocità = Velocità;
        }
         
        //1.COSTRUTTORE EFFETTIVO PER COPIA:
        protected ParametriPersonaggio(ParametriPersonaggio p)
        : this(p.Salute, p.Attacco, p.Velocità) { }
        //2.FUNZIONE CHE RICHIAMA IL COSTRUTTORE PER RIFERIMENTI AUTOMATICI
        public ParametriPersonaggio Copy() 
        { return new ParametriPersonaggio(this); }

        #endregion

        #region Funzione Add(Bonus b) 

        /* Permette Di Usare I Power Up ModificatoriParametri Per *
         * Potenziare / Depotenziare Il Personaggio               */
        public void Add(Bonus b)
        {
            //Se I Parametri Sono Troppo Bassi non Ha senso Continuare Mettere Malus
            //(Imposto Un Minimo)
            if (this.Attacco + b.Attacco > 10)
                this.Attacco += b.Attacco;
            else
                this.Attacco = 10;
            if (this.Salute  + b.Salute  > 0) // Salute = 0 --> Death
                this.Salute  += b.Salute;
            else
                this.Salute = 1;
            if (this.Velocità + b.Velocità > 0.5f)
                this.Velocità += b.Velocità;
            else
                this.Velocità = 0.5f;
        }

        #endregion
    }
}