using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terminal.Gui;

namespace Solitaire.Main
{
    /// <summary>
    /// Handles all card display formatting and color schemes
    /// Single Responsibility: Card presentation logic
    /// </summary>
    internal static class CardDisplayHelper
    {
        /// <summary>
        /// Converts a card to its display text representation (e.g., "A♠", "T♥")
        /// </summary>
        /// <param name="card">The card to display</param>
        /// <returns>Formatted card text</returns>
        public static string GetCardDisplayText(Card card)
        {
            string rankText = card.Rank switch
            {
                Rank.Two => "2",
                Rank.Three => "3",
                Rank.Four => "4",
                Rank.Five => "5",
                Rank.Six => "6",
                Rank.Seven => "7",
                Rank.Eight => "8",
                Rank.Nine => "9",
                Rank.Ten => "T", // Keep consistent width
                Rank.Jack => "J",
                Rank.Queen => "Q",
                Rank.King => "K",
                Rank.Ace => "A",
                _ => card.Rank.ToString()[0].ToString()
            };

            string suitText = card.Suit switch
            {
                Suit.Hearts => "♥",
                Suit.Diamonds => "♦",
                Suit.Clubs => "♣",
                Suit.Spades => "♠",
                _ => card.Suit.ToString()[0].ToString()
            };

            return $"{rankText}{suitText}";
        }

        /// <summary>
        /// Gets the appropriate color scheme for a card based on its suit
        /// </summary>
        /// <param name="card">The card to get colors for</param>
        /// <returns>ColorScheme with red or black text</returns>
        public static ColorScheme GetCardColorScheme(Card card)
        {
            bool isRed = card.Suit == Suit.Hearts || card.Suit == Suit.Diamonds;

            return new ColorScheme
            {
                Normal = new Terminal.Gui.Attribute(
                    isRed ? Color.Red : Color.Black,
                    Color.White
                ),
                Focus = new Terminal.Gui.Attribute(
                    isRed ? Color.Red : Color.Black,
                    Color.White
                )
            };
        }

        /// <summary>
        /// Standard hover color scheme for all interactive elements
        /// </summary>
        public static readonly ColorScheme HoverScheme = new()
        {
            Normal = new Terminal.Gui.Attribute(Color.Black, Color.Cyan),
            Focus = new Terminal.Gui.Attribute(Color.Black, Color.Cyan)
        };
    }
}
