using System;
using System.Windows.Forms;


/**                                                               **
 *******************************************************************
 **                                                               **
 ** Program.cs -> File Di Main Sotto Il Quale gira Tutto il Gioco **
 **                                                               **
 *******************************************************************
 **                                                               **/

namespace NerdOrDungeons
{
    #if WINDOWS || XBOX
    static class Program
    { static void Main(string[] args) { using (MainGame Game = new MainGame()) { Game.Run(); } } }//MultiPlayerSettings form = new MultiPlayerSettings(new MainGame())) { form.ShowDialog(); } } }
    #endif
}

