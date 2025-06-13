using System;
using System.Collections.Generic;

namespace Solitaire.Main
{
    /// <summary>
    /// Provides methods for creating and shuffling decks of cards for Solitaire.
    /// </summary>
    internal static class DeckManager
    {
        /// <summary>
        /// Creates a standard 52-card deck.
        /// </summary>
        /// <returns>List of <see cref="Card"/> representing a standard deck.</returns>
        public static List<Card> CreateStandartDeck()
        {
            // List of the Cards => Card Deck
            List<Card> CardDeck = [];


            
            // Looping through the Type and Value Enums to create a Card for each type and value
            for (int i = 0; i < Enum.GetNames(typeof(Suit)).Length; i++)
            {
                // Second loop for the Values
                for (int j = 0; j < Enum.GetNames(typeof(Rank)).Length; j++)
                {
                    Suit suit = (Suit)Enum.GetValues(typeof(Suit)).GetValue(i);
                    Rank rank = (Rank)Enum.GetValues(typeof(Rank)).GetValue(j);

                    CardDeck.Add(new Card(suit, rank));

                }
            }
            return CardDeck;

        }

        /// <summary>
        /// Shuffles a list of cards using a random algorithm.
        /// </summary>
        /// <param name="deck">The deck to shuffle.</param>
        /// <returns>A new shuffled list of cards.</returns>
        public static List<Card> ShuffleList(List<Card> deck)
        {
            // Using the Fisher-Yates Shuffle to randomize the Cards (Read More: https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle)
            Random random = new();

            int n = deck.Count;

            while (n > 1)
            {
                n--;

                int k = random.Next(n + 1);
                (deck[n], deck[k]) = (deck[k], deck[n]);
            }

            return deck;
        }
    }
}


