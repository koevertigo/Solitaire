using Solitaire.Main;

/// <summary>
/// Manages a single tableau column with its cards and interactions
/// Single Responsibility: Tableau column operations
/// </summary>
internal class TableauColumn
{
    /// <summary>
    /// All cards in this tableau column (face-down and face-up).
    /// </summary>
    private List<Card> cards;

    /// <summary>
    /// Cards currently face-up in this column.
    /// </summary>
    private List<Card> faceUpCards;

    /// <summary>
    /// Zero-based index of this tableau column (0-6).
    /// </summary>
    private int columnIndex;

    /// <summary>
    /// Event fired when this column's state changes.
    /// </summary>
    public event Action<int> ColumnChanged;

    /// <summary>
    /// Number of face-down cards in this column.
    /// </summary>
    public int FaceDownCount => cards.Count - faceUpCards.Count;

    /// <summary>
    /// All face-up cards in this column.
    /// </summary>
    public IReadOnlyList<Card> FaceUpCards => faceUpCards.AsReadOnly();

    /// <summary>
    /// Top visible card, or null if column is empty.
    /// </summary>
    public Card TopCard => faceUpCards.Count > 0 ? faceUpCards.Last() : new Card();

    /// <summary>
    /// Initializes a tableau column with starting cards.
    /// </summary>
    /// <param name="columnIndex">Zero-based column index (0-6).</param>
    /// <param name="initialCards">Cards to place in this column.</param>
    public TableauColumn(int columnIndex, List<Card> initialCards)
    {
        this.columnIndex = columnIndex;
        this.cards = new List<Card>(initialCards);
        this.faceUpCards = new List<Card>();

        // In solitaire, only the top card starts face-up
        if (cards.Count > 0)
        {
            Card topCard = cards.Last();
            cards.RemoveAt(cards.Count - 1);
            faceUpCards.Add(topCard);
        }
    }

    /// <summary>
    /// Attempts to place a card on this column according to Solitaire rules.
    /// </summary>
    /// <param name="card">Card to place.</param>
    /// <returns>True if placement is valid and successful; otherwise, false.</returns>
    public bool TryPlaceCard(Card card)
    {
        if (CanPlaceCard(card))
        {
            faceUpCards.Add(card);
            ColumnChanged?.Invoke(columnIndex);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Checks if a card can be legally placed on this column.
    /// </summary>
    /// <param name="card">Card to check.</param>
    /// <returns>True if placement is valid; otherwise, false.</returns>
    public bool CanPlaceCard(Card card)
    {
        // Empty column accepts only Kings
        if (faceUpCards.Count == 0)
        {
            return card.Rank == Rank.King;
        }

        Card topCard = faceUpCards.Last();

        // Must be opposite color and one rank lower
        bool isOppositeColor = IsOppositeColor(card, topCard);
        bool isOneLower = IsOneLowerRank(card, topCard);

        return isOppositeColor && isOneLower;
    }

    /// <summary>
    /// Attempts to remove cards from this column starting from a specific card.
    /// </summary>
    /// <param name="startingCard">The card to start removing from.</param>
    /// <returns>List of removed cards, or empty list if invalid.</returns>
    public List<Card> TryRemoveCards(Card startingCard)
    {
        int startIndex = faceUpCards.IndexOf(startingCard);
        if (startIndex == -1) return new List<Card>();

        // Validate that the sequence from startingCard to end is valid
        if (!IsValidSequence(startIndex))
        {
            return new List<Card>();
        }

        // Remove and return the cards
        List<Card> removedCards = faceUpCards.GetRange(startIndex, faceUpCards.Count - startIndex);
        faceUpCards.RemoveRange(startIndex, faceUpCards.Count - startIndex);

        // If we removed all face-up cards and there are face-down cards, flip one
        if (faceUpCards.Count == 0 && cards.Count > 0)
        {
            Card newTopCard = cards.Last();
            cards.RemoveAt(cards.Count - 1);
            faceUpCards.Add(newTopCard);
        }

        ColumnChanged?.Invoke(columnIndex);
        return removedCards;
    }

    /// <summary>
    /// Gets the display representation of this column for the UI.
    /// </summary>
    /// <returns>Display text for the column.</returns>
    public string GetDisplay()
    {
        if (faceUpCards.Count > 0)
        {
            return $"[{CardDisplayHelper.GetCardDisplayText(faceUpCards.Last())}]";
        }
        else if (cards.Count > 0)
        {
            return "[##]"; // Face-down cards
        }
        else
        {
            return $"[{columnIndex + 1}]"; // Empty column showing number
        }
    }

    /// <summary>
    /// Checks if two cards have opposite colors (red/black).
    /// </summary>
    /// <param name="card1">First card.</param>
    /// <param name="card2">Second card.</param>
    /// <returns>True if cards have opposite colors; otherwise, false.</returns>
    private bool IsOppositeColor(Card card1, Card card2)
    {
        bool card1IsRed = card1.Suit == Suit.Hearts || card1.Suit == Suit.Diamonds;
        bool card2IsRed = card2.Suit == Suit.Hearts || card2.Suit == Suit.Diamonds;
        return card1IsRed != card2IsRed;
    }

    /// <summary>
    /// Checks if card1 is one rank lower than card2.
    /// </summary>
    /// <param name="card1">First card.</param>
    /// <param name="card2">Second card.</param>
    /// <returns>True if card1 is one rank lower than card2; otherwise, false.</returns>
    private bool IsOneLowerRank(Card card1, Card card2)
    {
        return (int)card1.Rank == (int)card2.Rank - 1;
    }

    /// <summary>
    /// Validates that a sequence of face-up cards forms a valid descending sequence.
    /// </summary>
    /// <param name="startIndex">Index to start validation from.</param>
    /// <returns>True if the sequence is valid; otherwise, false.</returns>
    private bool IsValidSequence(int startIndex)
    {
        for (int i = startIndex; i < faceUpCards.Count - 1; i++)
        {
            if (!IsOppositeColor(faceUpCards[i + 1], faceUpCards[i]) ||
                !IsOneLowerRank(faceUpCards[i + 1], faceUpCards[i]))
            {
                return false;
            }
        }
        return true;
    }
}
