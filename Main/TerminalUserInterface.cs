using Microsoft.Extensions.Logging;
using Solitaire.Main;

using Terminal.Gui;

/// <summary>
/// Main UI class that coordinates all game elements for the Solitaire game.
/// Handles the creation, layout, and event management of all UI components,
/// including the stock, waste, foundation, and tableau piles.
/// </summary>
internal class TerminalUserInterface : Window
{
    // Game logic managers

    /// <summary>
    /// Manages the stock and waste piles' game logic and state.
    /// </summary>
    private StockWasteManager stockWasteManager;

    /// <summary>
    /// Array of tableau columns, each representing a column in the Solitaire tableau.
    /// </summary>
    private TableauColumn[] tableauColumns;

    // UI elements

    /// <summary>
    /// Label displaying the current state of the stock pile.
    /// </summary>
    private Label stockPileLabel;

    /// <summary>
    /// Label displaying the current state of the waste pile.
    /// </summary>
    private Label wastePileLabel;

    /// <summary>
    /// Array of labels representing the tableau cards on the UI.
    /// </summary>
    private Label[] tableauLabels;

    /// <summary>
    /// Initializes the solitaire game UI, including window, game logic, UI elements, and event handlers.
    /// </summary>
    public TerminalUserInterface()
    {
        InitializeWindow();
        InitializeGameLogic();
        CreateUIElements();
        SetupEventHandlers();
    }

    /// <summary>
    /// Sets up the main window properties, such as position, size, and color scheme.
    /// </summary>
    private void InitializeWindow()
    {
        this.X = 0;
        this.Y = 1;
        this.Width = Dim.Fill();
        this.Height = Dim.Fill();
        this.ColorScheme = new ColorScheme
        {
            Normal = new Terminal.Gui.Attribute(Color.White, Color.Green),
            Focus = new Terminal.Gui.Attribute(Color.BrightYellow, Color.Green),
            HotNormal = new Terminal.Gui.Attribute(Color.BrightYellow, Color.Blue),
            HotFocus = new Terminal.Gui.Attribute(Color.BrightYellow, Color.DarkGray)
        };
    }

    /// <summary>
    /// Initializes game logic components, including the stock/waste manager and tableau columns.
    /// Deals cards to tableau columns according to Solitaire rules.
    /// </summary>
    private void InitializeGameLogic()
    {
        stockWasteManager = new StockWasteManager();

        // Create tableau columns with proper card distribution
        tableauColumns = new TableauColumn[7];
        var remainingCards = DeckManager.ShuffleList(DeckManager.CreateStandartDeck());

        // Deal cards to tableau (1 to first column, 2 to second, etc.)
        int cardIndex = 0;
        for (int col = 0; col < 7; col++)
        {
            var columnCards = remainingCards.GetRange(cardIndex, col + 1);
            cardIndex += col + 1;
            tableauColumns[col] = new TableauColumn(col, columnCards);
        }
    }

    /// <summary>
    /// Creates all UI elements for the game, including stock, waste, foundation, and tableau piles.
    /// </summary>
    private void CreateUIElements()
    {
        CreateStockAndWastePiles();
        CreateFoundationPiles();
        CreateTableauPiles();
    }

    /// <summary>
    /// Creates stock and waste pile UI elements and adds them to the window.
    /// </summary>
    private void CreateStockAndWastePiles()
    {
        stockPileLabel = new Label()
        {
            Text = stockWasteManager.GetStockDisplay(),
            VerticalTextAlignment = Alignment.Center,
            TextAlignment = Alignment.Center,
            X = 5,
            Y = 1
        };

        wastePileLabel = new Label()
        {
            Text = stockWasteManager.GetWasteDisplay(),
            VerticalTextAlignment = Alignment.Center,
            TextAlignment = Alignment.Center,
            X = Pos.Right(stockPileLabel) + 3,
            Y = Pos.Top(stockPileLabel)
        };

        Add(stockPileLabel, wastePileLabel);
    }

    /// <summary>
    /// Creates foundation pile UI elements and adds them to the window.
    /// </summary>
    private void CreateFoundationPiles()
    {
        for (int i = 0; i < 4; i++)
        {
            var foundation = new Label()
            {
                Text = "[  ]",
                VerticalTextAlignment = Alignment.Center,
                TextAlignment = Alignment.Center,
                X = Pos.Right(wastePileLabel) + 10 + (i * 10),
                Y = Pos.Top(stockPileLabel)
            };

            SetupHoverEffect(foundation);
            Add(foundation);
        }
    }

    /// <summary>
    /// Creates tableau pile UI elements and adds them to the window.
    /// Each label represents a card or placeholder in the tableau.
    /// </summary>
    private void CreateTableauPiles()
    {
        tableauLabels = new Label[27];

        for (int rows = 0; rows < 7; rows++)
        {
            for (int columns = 0; columns < 7; columns++)
            {

                if(columns > rows) continue;

                tableauLabels[rows + columns] = new Label()
                {
                    Text = rows == columns ? tableauColumns[rows].GetDisplay() : "[--]",
                    VerticalTextAlignment = Alignment.Center,
                    TextAlignment = Alignment.Center,
                    X = 5 + (rows * 12),
                    Y = 8 + (columns * 2)
                };


                if (rows == columns)
                {
                    SetupHoverEffect(tableauLabels[rows + columns]);
                    SetupClickEffect(tableauLabels[rows + columns]);
                }
                Add(tableauLabels[rows + columns]);
            }
            
        }
    }

    /// <summary>
    /// Sets up all event handlers for UI elements and game logic events.
    /// </summary>
    private void SetupEventHandlers()
    {
        // Stock/Waste pile events
        stockWasteManager.PilesChanged += UpdateStockWasteDisplay;
        stockPileLabel.MouseClick += (s, e) => stockWasteManager.HandleStockClick();

        // Tableau events
        for (int i = 0; i < 7; i++)
        {
            int columnIndex = i; // Capture for closure
            tableauColumns[i].ColumnChanged += (col) => UpdateTableauDisplay(col);
        }

        SetupHoverEffect(stockPileLabel);
    }

    /// <summary>
    /// Updates the stock and waste pile displays to reflect the current game state.
    /// Also updates the color scheme of the waste pile label based on the top card.
    /// </summary>
    private void UpdateStockWasteDisplay()
    {
        stockPileLabel.Text = stockWasteManager.GetStockDisplay();
        wastePileLabel.Text = stockWasteManager.GetWasteDisplay();

        // Update waste pile color based on top card
        if (!stockWasteManager.TopWasteCard.Equals(null))
        {
            wastePileLabel.ColorScheme = CardDisplayHelper.GetCardColorScheme(stockWasteManager.TopWasteCard);
        }
        else
        {
            wastePileLabel.ColorScheme = this.ColorScheme;
        }

        stockPileLabel.SetNeedsDraw();
        wastePileLabel.SetNeedsDraw();
    }

    /// <summary>
    /// Updates a specific tableau column display to reflect the current cards.
    /// Also updates the color scheme of the label based on the top card.
    /// </summary>
    /// <param name="columnIndex">Index of column to update (0-based).</param>
    private void UpdateTableauDisplay(int columnIndex)
    {
        tableauLabels[columnIndex].Text = tableauColumns[columnIndex].GetDisplay();

        // Update color scheme based on top card
        if (!tableauColumns[columnIndex].TopCard.Equals(null))
        {
            tableauLabels[columnIndex].ColorScheme =
                //CardDisplayHelper.HoverScheme;
                CardDisplayHelper.GetCardColorScheme(tableauColumns[columnIndex].TopCard);
                
        }
        else
        {
            tableauLabels[columnIndex].ColorScheme = CardDisplayHelper.HoverScheme;
        }

        tableauLabels[columnIndex].SetNeedsDraw();
    }

    /// <summary>
    /// Sets up hover effects for interactive label elements.
    /// Changes the color scheme on mouse enter/leave events.
    /// </summary>
    /// <param name="label">Label to add hover effects to.</param>
    private void SetupHoverEffect(Label label)
    {
        var normalScheme = label.ColorScheme ?? this.ColorScheme;
        

        label.MouseEnter += (s, e) =>
        {
            //if (!isClicked)
            {
                label.ColorScheme = CardDisplayHelper.HoverScheme;
                label.SetNeedsDraw();
            }
        };

        

        label.MouseLeave += (s, e) =>
        {
            //if (!isClicked)
            {
                // Restoring previous color
                if (label == wastePileLabel && stockWasteManager.TopWasteCard.Equals(null))
                {
                    label.ColorScheme = CardDisplayHelper.GetCardColorScheme(stockWasteManager.TopWasteCard);
                }
                else if (Array.IndexOf(tableauLabels, label) >= 0 && Array.IndexOf(tableauLabels, label) < tableauColumns.Length)
                {
                    int index = Array.IndexOf(tableauLabels, label);

                    if (tableauColumns[index].TopCard.Equals(null))
                    {
                        label.ColorScheme = CardDisplayHelper.GetCardColorScheme(tableauColumns[index].TopCard);
                    }
                    else
                    {
                        label.ColorScheme = this.ColorScheme;
                    }
                }
                else
                {
                    label.ColorScheme = normalScheme;
                }

                label.SetNeedsDraw();
            }
        };
    }

    /// <summary>
    /// Sets up click effects for interactive label elements.
    /// Changes the color scheme on mouse click events.
    /// </summary>
    /// <param name="label">Label to add click effects to.</param>
    private void SetupClickEffect(Label label)
    {
        bool isClicked = false;
        var normalScheme = label.ColorScheme ?? this.ColorScheme;

        label.MouseClick += (s, e) =>
        {
            isClicked = !isClicked;

            if (isClicked)
            {
                label.ColorScheme = CardDisplayHelper.HoverScheme;
            }
            else
            {
                label.ColorScheme = normalScheme;
            }

            label.SetNeedsDraw();
        };
    }
}
