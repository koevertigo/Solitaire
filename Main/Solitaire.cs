using System;
using System.Drawing;
using System.Reflection.Emit;
using Terminal.Gui;
using Windows.UI.ViewManagement;
using Windows.Web.Syndication;

namespace Solitaire.Main
{
    /// <summary>
    /// Entry point for the Solitaire application.
    /// Initializes and runs the main game loop and UI.
    /// </summary>
    internal static class Solitaire
    {
        /// <summary>
        /// Main method. Starts the Solitaire game.
        /// </summary>
        public static void Main(string[] args)
        {
            Application.Init();
            var userInterface = new TerminalUserInterface();

            Application.Run(userInterface);

            userInterface.Dispose();
            Application.Shutdown();
        }
    }
}
