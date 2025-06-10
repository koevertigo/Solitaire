using System;
using System.Collections.Generic;

using Solitaire.Enums;
using Solitaire.Structs;

namespace Solitaire.Main
{
    // New Class to create and shuffle the Cards
    internal class CardDeck
    {
        public static List<Card> CreateStandartDeck()
        {
            // List of the Cards => Card Deck
            List<Card> CardDeck = new List<Card>();

            // Looping through the Type and Value Enums to create a Card for each type and value
            for (int i = 0; i < Enum.GetNames(typeof(CardType)).Length; i++)
            {
                // Second loop for the Values
                for (int j = 0; j < Enum.GetNames(typeof(CardValue)).Length; j++)
                {
                    CardType cardType = (CardType)Enum.GetValues(typeof(CardType)).GetValue(i);
                    CardValue cardValue = (CardValue)Enum.GetValues(typeof(CardValue)).GetValue(j);

                    CardDeck.Add(new Card(cardType, cardValue));

                }
            }

            return CardDeck;

        }

        public static List<Card> ShuffleList(List<Card> CardDeck)
        {
            // Using the Fisher-Yates Shuffle to randomize the Cards (Read More: https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle)
            Random random = new Random();

            int n = CardDeck.Count;

            while (n > 1)
            {
                n--;

                int k = random.Next(n + 1);
                Card card = CardDeck[k];
                CardDeck[k] = CardDeck[n];
                CardDeck[n] = card;
            }

            return CardDeck;
        }

    }
}


