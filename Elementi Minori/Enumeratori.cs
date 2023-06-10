namespace NerdOrDungeons
{
    /**                                                              **
     ******************************************************************
     **                                                              **
     ** Enumeratori : Spazio Dedicato Alla Dichiarazione Degli Enum  **
     ** Vari.                                                        **
     **                                                              **
     ******************************************************************
     **                                                              **/

    public enum TipoIA
    {
        Assente = 0,
        Casuale = 1,
        Insegui = 2,
        Scappa  = 3,
        Alleato = 4
    }

    public enum DirezioneCorrente
    {
        Fermo = 0,
        Su = 1,
        Giù = 2,
        Sinistra = 3,
        Destra = 4,
        Spara = 5,
        Attacco = 6,
        Manuale = 7
    }

    public enum Controller
    {
        NONE = -1,
        PAD1 = 0,
        PAD2 = 1,
        PAD3 = 2,
        PAD4 = 3,
        KEYB1 = 4,
        KEYB2 = 5,
        KEYB3 = 6,
        KEYB4 = 7
    };

    public enum SoundType
    {
        Music = 0,
        Effect = 1
    };
}
