using System;
using System.Drawing;
using System.Reflection.Emit;
using Terminal.Gui;
using Windows.UI.ViewManagement;
using Windows.Web.Syndication;


namespace Solitaire.Main
{
    internal class Solitare
    {
        static void Main()
        {
            Application.Init();
            Application.Run(new TUI());
            
        }

    }

}
