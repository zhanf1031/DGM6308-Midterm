using System;

class Program
{
    static Random rng = new Random();

    static void Main(string[] args)
    {
        Console.WriteLine("Welcome to the Multiplayer game mode!Each player will randomly draw 4 cards from a set of 52 cards, excluding the Joker card, and then compare the total.After drawing cards in each round, players can choose to exchange their cards from the deck. Each player has 3 chances to switch. If two or more players want to switch cards, the order is determined by rolling the dice. The winning player in each round will score one point, and when the entire set of cards is drawn, the player with the highest score wins.");

        int playerCount = GetPlayerCount();
        List<Player> players = GetPlayerNames(playerCount);

        bool playing = true;

        while (playing)
        {
            List<string> deck = CreateDeck();
            Shuffle(deck);

            while (deck.Count >= playerCount * 4)
            {
                Console.WriteLine("Starting a new round...");
                DealCards(players, deck);

                // Show players' hands
                foreach (var player in players)
                {
                    Console.WriteLine($"{player.Name}'s Cards:");
                    DisplayHand(player.Hand);
                }

                // Determine the order of changing cards by rolling dice
                DetermineChangeOrder(players);

                // Allow players to change cards up to 3 times
                foreach (var player in players)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        if (deck.Count == 0)
                        {
                            Console.WriteLine("No more cards left in the deck to exchange.");
                            break;
                        }

                        Console.WriteLine($"{player.Name}'s turn to change cards.");
                        DisplayHand(player.Hand);
                        Console.WriteLine("Would you like to switch one of your cards? (y/n):");
                        char switchChoice = GetValidInput(new char[] { 'y', 'Y', 'n', 'N' });
                        if (switchChoice == 'y' || switchChoice == 'Y')
                        {
                            Console.WriteLine("Choose a card to replace (1-4):");
                            int cardIndex = int.Parse(Console.ReadLine()) - 1;
                            string oldCard = player.Hand[cardIndex];
                            string newCard = DrawCard(deck);
                            player.Hand[cardIndex] = newCard;
                            deck.Add(oldCard); // Put the old card back into the deck
                            Shuffle(deck); // Shuffle the deck after putting the card back
                            Console.WriteLine($"You drew a new card: {newCard}");
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                // Show final hands and determine round winner
                foreach (var player in players)
                {
                    Console.WriteLine($"{player.Name}'s Final Cards:");
                    DisplayHand(player.Hand);
                    player.RoundScore = CalculateSum(player.Hand);
                    Console.WriteLine($"{player.Name}'s Total: {player.RoundScore}");
                }

                var roundWinner = players.OrderByDescending(p => p.RoundScore).First();
                roundWinner.Wins++;
                Console.WriteLine($"{roundWinner.Name} wins this round!");

                Console.WriteLine("Winning Status:");
                foreach (var player in players)
                {
                    Console.WriteLine($"{player.Name} wins {player.Wins} times");
                }

                Console.WriteLine($"Remaining cards in deck: {deck.Count}");

                if (deck.Count < playerCount * 4)
                {
                    break;
                }
            }

            Console.WriteLine("Deck is exhausted. Calculating final scores...");

            // Determine overall winner
            var overallWinner = players.OrderByDescending(p => p.Wins).First();
            Console.WriteLine($"{overallWinner.Name} is the overall winner!");

            Console.WriteLine("Final Winning Status:");
            foreach (var player in players)
            {
                Console.WriteLine($"{player.Name} wins {player.Wins} times");
            }

            Console.WriteLine("Play again? (y/n):");
            char playAgain = GetValidInput(new char[] { 'y', 'Y', 'n', 'N' });
            playing = playAgain == 'y' || playAgain == 'Y';

            if (playing)
            {
                foreach (var player in players)
                {
                    player.Wins = 0;
                    player.RoundScore = 0;
                }
            }
        }

        Console.WriteLine("Game over!");
    }

    static int GetPlayerCount()
    {
        Console.WriteLine("Enter the number of players (2, 3, or 4):");
        int playerCount;
        while (!int.TryParse(Console.ReadLine(), out playerCount) || playerCount < 2 || playerCount > 4)
        {
            Console.WriteLine("Invalid input. Please enter a number between 2 and 4.");
        }
        return playerCount;
    }

    static List<Player> GetPlayerNames(int playerCount)
    {
        List<Player> players = new List<Player>();
        for (int i = 1; i <= playerCount; i++)
        {
            Console.WriteLine($"Enter name for Player {i}:");
            string playerName = Console.ReadLine();
            players.Add(new Player(playerName));
        }
        return players;
    }

    static void DetermineChangeOrder(List<Player> players)
    {
        Console.WriteLine("Rolling dice to determine the order of changing cards...");
        foreach (var player in players)
        {
            player.DiceRoll = rng.Next(1, 7);
            Console.WriteLine($"{player.Name} rolled a {player.DiceRoll}");
        }

        players.Sort((x, y) => y.DiceRoll.CompareTo(x.DiceRoll));
        Console.WriteLine("Order of changing cards:");
        foreach (var player in players)
        {
            Console.WriteLine(player.Name);
        }
    }

    static void DealCards(List<Player> players, List<string> deck)
    {
        foreach (var player in players)
        {
            player.Hand = DrawCards(deck, 4);
        }
    }

    static char GetValidInput(char[] validInputs)
    {
        char input;
        do
        {
            input = Console.ReadKey(true).KeyChar;
        } while (Array.IndexOf(validInputs, input) == -1);
        return input;
    }

    static List<string> CreateDeck()
    {
        List<string> deck = new List<string>();
        string[] suits = { "Hearts", "Diamonds", "Clubs", "Spades" };
        string[] values = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };

        foreach (string suit in suits)
        {
            foreach (string value in values)
            {
                deck.Add($"{value} of {suit}");
            }
        }
        return deck;
    }

    static void Shuffle(List<string> deck)
    {
        int n = deck.Count;
        for (int i = n - 1; i > 0; i--)
        {
            int j = rng.Next(i + 1);
            string temp = deck[i];
            deck[i] = deck[j];
            deck[j] = temp;
        }
    }

    static List<string> DrawCards(List<string> deck, int count)
    {
        List<string> hand = new List<string>();
        for (int i = 0; i < count; i++)
        {
            hand.Add(DrawCard(deck));
        }
        return hand;
    }

    static string DrawCard(List<string> deck)
    {
        int randomIndex = rng.Next(deck.Count);
        string card = deck[randomIndex];
        deck.RemoveAt(randomIndex);
        return card;
    }

    static void DisplayHand(List<string> hand)
    {
        foreach (string card in hand)
        {
            Console.WriteLine(GetAsciiArt(card));
        }
    }

    static int CalculateSum(List<string> hand)
    {
        int sum = 0;
        foreach (string card in hand)
        {
            sum += CardValue(card);
        }
        return sum;
    }

    static int CardValue(string card)
    {
        string value = card.Split(' ')[0];
        return value switch
        {
            "2" => 2,
            "3" => 3,
            "4" => 4,
            "5" => 5,
            "6" => 6,
            "7" => 7,
            "8" => 8,
            "9" => 9,
            "10" => 10,
            "J" => 11,
            "Q" => 12,
            "K" => 13,
            "A" => 1,
            _ => 0
        };
    }

    static string GetAsciiArt(string card)
    {
        return card switch
        {
            "2 of Hearts" => "  _____\n |2    |\n |  ♥  |\n |     |\n |____Z|",
            "3 of Hearts" => "  _____\n |3    |\n | ♥ ♥ |\n |     |\n |____E|",
            "4 of Hearts" => "  _____\n |4    |\n | ♥ ♥ |\n | ♥ ♥ |\n |____h|",
            "5 of Hearts" => "  _____\n |5    |\n | ♥ ♥ |\n |  ♥  |\n | ♥ ♥ |\n |____S|",
            "6 of Hearts" => "  _____\n |6    |\n | ♥ ♥ |\n | ♥ ♥ |\n | ♥ ♥ |\n |____9|",
            "7 of Hearts" => "  _____\n |7    |\n | ♥ ♥ |\n |♥ ♥ ♥|\n | ♥ ♥ |\n |____L|",
            "8 of Hearts" => "  _____\n |8    |\n | ♥ ♥ |\n |♥ ♥ ♥|\n |♥ ♥ ♥|\n |____8|",
            "9 of Hearts" => "  _____\n |9    |\n |♥ ♥ ♥|\n |♥ ♥ ♥|\n |♥ ♥ ♥|\n |____6|",
            "10 of Hearts" => "  _____\n |10  ♥|\n |♥ ♥ ♥|\n |♥ ♥ ♥|\n |♥ ♥ ♥|\n |___0I|",
            "J of Hearts" => "  _____\n |J  ww|\n | ♥ {)|\n | ♥ {)|\n | ♥ ♥ |\n |____L|",
            "Q of Hearts" => "  _____\n |Q  ww|\n | ♥ {(|\n | ♥ {(|\n |♥♥♥♥|\n |____O|",
            "K of Hearts" => "  _____\n |K  WW|\n | ♥ {)|\n | ♥ {)|\n |♥♥♥♥|\n |____C|",
            "A of Hearts" => "  _____\n |A    |\n |  ♥  |\n |     |\n |____V|",
            "2 of Diamonds" => "  _____\n |2    |\n |  ♦  |\n |     |\n |____Z|",
            "3 of Diamonds" => "  _____\n |3    |\n | ♦ ♦ |\n |     |\n |____E|",
            "4 of Diamonds" => "  _____\n |4    |\n | ♦ ♦ |\n | ♦ ♦ |\n |____h|",
            "5 of Diamonds" => "  _____\n |5    |\n | ♦ ♦ |\n |  ♦  |\n | ♦ ♦ |\n |____S|",
            "6 of Diamonds" => "  _____\n |6    |\n | ♦ ♦ |\n | ♦ ♦ |\n | ♦ ♦ |\n |____9|",
            "7 of Diamonds" => "  _____\n |7    |\n | ♦ ♦ |\n |♦ ♦ ♦|\n | ♦ ♦ |\n |____L|",
            "8 of Diamonds" => "  _____\n |8    |\n | ♦ ♦ |\n |♦ ♦ ♦|\n |♦ ♦ ♦|\n |____8|",
            "9 of Diamonds" => "  _____\n |9    |\n |♦ ♦ ♦|\n |♦ ♦ ♦|\n |♦ ♦ ♦|\n |____6|",
            "10 of Diamonds" => "  _____\n |10  ♦|\n |♦ ♦ ♦|\n |♦ ♦ ♦|\n |♦ ♦ ♦|\n |___0I|",
            "J of Diamonds" => "  _____\n |J  ww|\n | ♦ {)|\n | ♦ {)|\n | ♦ ♦ |\n |____L|",
            "Q of Diamonds" => "  _____\n |Q  ww|\n | ♦ {(|\n | ♦ {(|\n |♦♦♦♦|\n |____O|",
            "K of Diamonds" => "  _____\n |K  WW|\n | ♦ {)|\n | ♦ {)|\n |♦♦♦♦|\n |____C|",
            "A of Diamonds" => "  _____\n |A    |\n |  ♦  |\n |     |\n |____V|",
            "2 of Clubs" => "  _____\n |2    |\n |  ♣  |\n |     |\n |____Z|",
            "3 of Clubs" => "  _____\n |3    |\n | ♣ ♣ |\n |     |\n |____E|",
            "4 of Clubs" => "  _____\n |4    |\n | ♣ ♣ |\n | ♣ ♣ |\n |____h|",
            "5 of Clubs" => "  _____\n |5    |\n | ♣ ♣ |\n |  ♣  |\n | ♣ ♣ |\n |____S|",
            "6 of Clubs" => "  _____\n |6    |\n | ♣ ♣ |\n | ♣ ♣ |\n | ♣ ♣ |\n |____9|",
            "7 of Clubs" => "  _____\n |7    |\n | ♣ ♣ |\n |♣ ♣ ♣|\n | ♣ ♣ |\n |____L|",
            "8 of Clubs" => "  _____\n |8    |\n | ♣ ♣ |\n |♣ ♣ ♣|\n |♣ ♣ ♣|\n |____8|",
            "9 of Clubs" => "  _____\n |9    |\n |♣ ♣ ♣|\n |♣ ♣ ♣|\n |♣ ♣ ♣|\n |____6|",
            "10 of Clubs" => "  _____\n |10  ♣|\n |♣ ♣ ♣|\n |♣ ♣ ♣|\n |♣ ♣ ♣|\n |___0I|",
            "J of Clubs" => "  _____\n |J  ww|\n | ♣ {)|\n | ♣ {)|\n | ♣ ♣ |\n |____L|",
            "Q of Clubs" => "  _____\n |Q  ww|\n | ♣ {(|\n | ♣ {(|\n |♣♣♣♣|\n |____O|",
            "K of Clubs" => "  _____\n |K  WW|\n | ♣ {)|\n | ♣ {)|\n |♣♣♣♣|\n |____C|",
            "A of Clubs" => "  _____\n |A    |\n |  ♣  |\n |     |\n |____V|",
            "2 of Spades" => "  _____\n |2    |\n |  ♠  |\n |     |\n |____Z|",
            "3 of Spades" => "  _____\n |3    |\n | ♠ ♠ |\n |     |\n |____E|",
            "4 of Spades" => "  _____\n |4    |\n | ♠ ♠ |\n | ♠ ♠ |\n |____h|",
            "5 of Spades" => "  _____\n |5    |\n | ♠ ♠ |\n |  ♠  |\n | ♠ ♠ |\n |____S|",
            "6 of Spades" => "  _____\n |6    |\n | ♠ ♠ |\n | ♠ ♠ |\n | ♠ ♠ |\n |____9|",
            "7 of Spades" => "  _____\n |7    |\n | ♠ ♠ |\n |♠ ♠ ♠|\n | ♠ ♠ |\n |____L|",
            "8 of Spades" => "  _____\n |8    |\n | ♠ ♠ |\n |♠ ♠ ♠|\n |♠ ♠ ♠|\n |____8|",
            "9 of Spades" => "  _____\n |9    |\n |♠ ♠ ♠|\n |♠ ♠ ♠|\n |♠ ♠ ♠|\n |____6|",
            "10 of Spades" => "  _____\n |10  ♠|\n |♠ ♠ ♠|\n |♠ ♠ ♠|\n |♠ ♠ ♠|\n |___0I|",
            "J of Spades" => "  _____\n |J  ww|\n | ♠ {)|\n | ♠ {)|\n | ♠ ♠ |\n |____L|",
            "Q of Spades" => "  _____\n |Q  ww|\n | ♠ {(|\n | ♠ {(|\n |♠♠♠♠|\n |____O|",
            "K of Spades" => "  _____\n |K  WW|\n | ♠ {)|\n | ♠ {)|\n |♠♠♠♠|\n |____C|",
            "A of Spades" => "  _____\n |A    |\n |  ♠  |\n |     |\n |____V|",
            "[Hidden]" => "  _____\n |     |\n |  ?  |\n |     |\n |_____|",
            _ => "  _____\n |     |\n |  ?  |\n |     |\n |_____|"
        };
    }
}

class Player
{
    public string Name { get; set; }
    public List<string> Hand { get; set; }
    public int RoundScore { get; set; }
    public int Wins { get; set; }
    public int DiceRoll { get; set; }

    public Player(string name)
    {
        Name = name;
        Hand = new List<string>();
        RoundScore = 0;
        Wins = 0;
        DiceRoll = 0;
    }
}
