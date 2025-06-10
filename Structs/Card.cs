using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Solitaire.Enums;

namespace Solitaire.Structs
{
    // Card structure for easier and cleaner creation of the deck
    internal struct Card
    {
        public Card(CardType cardType, CardValue cardValue)
        {
            this.cardType = cardType;
            this.cardValue = cardValue;
        }

        public CardType cardType { get; private set; }
        public CardValue cardValue { get; private set; }
    }
}
