using Solitaire.Main;

/// <summary>
/// Manages the stock pile and waste pile game logic for Solitaire.
/// Handles drawing cards, resetting the deck, and providing display information.
/// </summary>
internal class StockWasteManager
{
    /// <summary>
    /// List of cards currently in the stock pile (face-down).
    /// </summary>
    private List<Card> stockPile;

    /// <summary>
    /// List of cards currently in the waste pile (face-up).
    /// </summary>
    private List<Card> wastePile;

    /// <summary>
    /// Event fired when pile states change, requiring UI update.
    /// </summary>
    public event Action PilesChanged;

    /// <summary>
    /// Current number of cards in stock pile.
    /// </summary>
    public int StockCount => stockPile.Count;

    /// <summary>
    /// Current number of cards in waste pile.
    /// </summary>
    public int WasteCount => wastePile.Count;

    /// <summary>
    /// Top card of waste pile, or null if empty.
    /// </summary>
    public Card TopWasteCard => wastePile.Count > 0 ? wastePile.Last() : new Card();

    /// <summary>
    /// Initializes the stock and waste piles with a shuffled deck.
    /// </summary>
    public StockWasteManager()
    {
        stockPile = [.. DeckManager.ShuffleList(DeckManager.CreateStandartDeck()).Skip(27)];
        wastePile = [];
    }

    /// <summary>
    /// Handles clicking the stock pile - either draws a card or resets the deck if empty.
    /// </summary>
    public void HandleStockClick()
    {
        if (stockPile.Count > 0)
        {
            DrawCardFromStock();
        }
        else if (wastePile.Count > 0)
        {
            ResetDeck();
        }

        PilesChanged?.Invoke();
    }

    /// <summary>
    /// Draws one card from stock to waste pile.
    /// </summary>
    private void DrawCardFromStock()
    {
        if (stockPile.Count > 0)
        {
            Card card = stockPile[0];
            stockPile.RemoveAt(0);
            wastePile.Add(card);
        }
    }

    /// <summary>
    /// Resets the deck by moving all waste cards back to stock in reverse order.
    /// </summary>
    private void ResetDeck()
    {
        stockPile.AddRange(wastePile.AsEnumerable().Reverse());
        wastePile.Clear();
    }

    /// <summary>
    /// Gets display text for stock pile for the UI.
    /// </summary>
    /// <returns>Formatted stock pile display.</returns>
    public string GetStockDisplay()
    {
        return stockPile.Count > 0 ? $"[#{stockPile.Count}]" : "[↻]";
    }

    /// <summary>
    /// Gets display text for waste pile for the UI.
    /// </summary>
    /// <returns>Formatted waste pile display.</returns>
    public string GetWasteDisplay()
    {
        return wastePile.Count > 0
            ? $"[{CardDisplayHelper.GetCardDisplayText(wastePile.Last())}]"
            : "[  ]";
    }
}