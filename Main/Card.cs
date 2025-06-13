using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solitaire.Main
{
    internal struct Card(Suit suit, Rank rank)
    {
        public Suit Suit { get; private set; } = suit;
        public Rank Rank { get; private set; } = rank;
    }

    /// <summary>
    /// Enumeration of possible card ranks.
    /// </summary>
    public enum Rank
    {
        Ace = 1,
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5,
        Six = 6,
        Seven = 7,
        Eight = 8,
        Nine = 9,
        Ten = 10,
        Jack = 11,
        Queen = 12,
        King = 13
    }

    /// <summary>
    /// Enumeration of possible card suits.
    /// </summary>
    public enum Suit
    {
        Clubs, Spades, Hearts, Diamonds
    }
}
