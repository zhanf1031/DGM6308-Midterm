using System;

class Program
{
    static Random rng = new Random();

    static void Main(string[] args)
    {
        Console.WriteLine("Mutilplayer mode");

            List<string> team1 = new List<string>();
            List<string> team2 = new List<string>();

            Console.WriteLine("Enter the number of players (must be 4):");
            int playerCount = 4;

            for (int i = 1; i <= playerCount; i++)
            {
                Console.WriteLine($"Enter name for Player {i}:");
                string playerName = Console.ReadLine();

                bool assigned = false;
                while (!assigned)
                {
                    Console.WriteLine($"Which team would {playerName} like to join? (1 or 2):");
                    int teamChoice;
                    if (int.TryParse(Console.ReadLine(), out teamChoice))
                    {
                        if (teamChoice == 1 && team1.Count < 2)
                        {
                            team1.Add(playerName);
                            assigned = true;
                        }
                        else if (teamChoice == 2 && team2.Count < 2)
                        {
                            team2.Add(playerName);
                            assigned = true;
                        }
                        else
                        {
                            Console.WriteLine("Invalid choice or team is full. Please choose again.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter 1 or 2.");
                    }
                }
            }

            Console.WriteLine("Teams are set!");
            Console.WriteLine("Team 1:");
            foreach (var player in team1)
            {
                Console.WriteLine(player);
            }

            Console.WriteLine("Team 2:");
            foreach (var player in team2)
            {
                Console.WriteLine(player);
            }


            Console.WriteLine("This is a very simple card game. Players and computers randomly draw 4 cards from 52 cards excluding joker cards for comparison, and the one with the highest total will win one turn. Each round will draw from the remaining cards. The final game is won by the party with the most winning rounds. Now, please start the game.");

        bool playing = true;
        int playerScore = 0;
        int computerScore = 0;

        while (playing)
        {
            List<string> deck = CreateDeck();
            Shuffle(deck);

            while (deck.Count >= 12)
            {
                if (deck.Count == 12)
                {
                    Console.WriteLine("Attention! This is the final game!");
                }

                List<string> playerHand = DrawCards(deck, 4);
                List<string> computerHand = DrawCards(deck, 4);

                Console.WriteLine("Your Cards:");
                DisplayHand(playerHand);
                Console.WriteLine("Computer's Cards:");
                DisplayHiddenHand(computerHand);

                Console.WriteLine("Remaining cards in deck: " + deck.Count);

                Console.WriteLine("Would you like to switch one of your cards with the computer's? (y/n):");
                char switchChoice = GetValidInput(new char[] { 'y', 'Y', 'n', 'N' });
                if (switchChoice == 'y' || switchChoice == 'Y')
                {
                    ExchangeCard(playerHand, computerHand);
                }

                Console.WriteLine("Your Final Cards:");
                DisplayHand(playerHand);
                Console.WriteLine("Computer's Final Cards:");
                DisplayHand(computerHand);

                int playerSum = CalculateSum(playerHand);
                int computerSum = CalculateSum(computerHand);

                Console.WriteLine($"Your Sum: {playerSum}");
                Console.WriteLine($"Computer's Sum: {computerSum}");

                if (playerSum > computerSum)
                {
                    Console.WriteLine("You win this round!");
                    playerScore++;
                }
                else if (playerSum < computerSum)
                {
                    Console.WriteLine("Computer wins this round!");
                    computerScore++;
                }
                else
                {
                    Console.WriteLine("Draw! Restarting...");
                    continue;
                }

                Console.WriteLine($"Score: Player {playerScore} - Computer {computerScore}");
                Console.WriteLine("Remaining cards in deck: " + deck.Count);

                Console.WriteLine("Play again? (y/n):");
                char continueChoice = GetValidInput(new char[] { 'y', 'Y', 'n', 'N' });
                if (continueChoice != 'y' && continueChoice != 'Y')
                {
                    break;
                }
            }

            Console.WriteLine("Final Scores:");
            Console.WriteLine($"Player: {playerScore}");
            Console.WriteLine($"Computer: {computerScore}");

            if (playerScore > computerScore)
            {
                Console.WriteLine("You are the overall winner!");
            }
            else if (playerScore < computerScore)
            {
                Console.WriteLine("Computer is the overall winner!");
            }
            else
            {
                Console.WriteLine("It's a tie!");
            }

            Console.WriteLine("Play again? (y/n):");
            char playAgain = GetValidInput(new char[] { 'y', 'Y', 'n', 'N' });
            playing = playAgain == 'y' || playAgain == 'Y';

            if (playing)
            {
                playerScore = 0;
                computerScore = 0;
            }
        }

        Console.WriteLine("Game over!");
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
            int randomIndex = rng.Next(deck.Count);
            hand.Add(deck[randomIndex]);
            deck.RemoveAt(randomIndex);
        }
        return hand;
    }

    static void DisplayHand(List<string> hand)
    {
        foreach (string card in hand)
        {
            Console.WriteLine(GetAsciiArt(card));
        }
    }

    static void DisplayHiddenHand(List<string> hand)
    {
        for (int i = 0; i < hand.Count; i++)
        {
            Console.WriteLine(GetAsciiArt("[Hidden]"));
        }
    }

    static void ExchangeCard(List<string> playerHand, List<string> computerHand)
    {
        Console.WriteLine("Choose a card to give to the computer (1-4):");
        int playerCardIndex = int.Parse(Console.ReadLine()) - 1;

        Console.WriteLine("Choose a computer card to take (1-4):");
        DisplayHiddenHand(computerHand);
        int computerCardIndex = int.Parse(Console.ReadLine()) - 1;

        string playerCard = playerHand[playerCardIndex];
        string computerCard = computerHand[computerCardIndex];

        playerHand[playerCardIndex] = computerCard;
        computerHand[computerCardIndex] = playerCard;

        Console.WriteLine("Cards switched successfully!");
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
        switch (value)
        {
            case "2": return 2;
            case "3": return 3;
            case "4": return 4;
            case "5": return 5;
            case "6": return 6;
            case "7": return 7;
            case "8": return 8;
            case "9": return 9;
            case "10": return 10;
            case "J": return 11;
            case "Q": return 12;
            case "K": return 13;
            case "A": return 1;
            default: return 0;
        }
    }

    static string GetAsciiArt(string card)
    {
        switch (card)
        {
            case "2 of Hearts": return "  _____\n |2    |\n |  ♥  |\n |     |\n |____Z|";
            case "3 of Hearts": return "  _____\n |3    |\n | ♥ ♥ |\n |     |\n |____E|";
            case "4 of Hearts": return "  _____\n |4    |\n | ♥ ♥ |\n | ♥ ♥ |\n |____h|";
            case "5 of Hearts": return "  _____\n |5    |\n | ♥ ♥ |\n |  ♥  |\n | ♥ ♥ |\n |____S|";
            case "6 of Hearts": return "  _____\n |6    |\n | ♥ ♥ |\n | ♥ ♥ |\n | ♥ ♥ |\n |____9|";
            case "7 of Hearts": return "  _____\n |7    |\n | ♥ ♥ |\n |♥ ♥ ♥|\n | ♥ ♥ |\n |____L|";
            case "8 of Hearts": return "  _____\n |8    |\n | ♥ ♥ |\n |♥ ♥ ♥|\n |♥ ♥ ♥|\n |____8|";
            case "9 of Hearts": return "  _____\n |9    |\n |♥ ♥ ♥|\n |♥ ♥ ♥|\n |♥ ♥ ♥|\n |____6|";
            case "10 of Hearts": return "  _____\n |10  ♥|\n |♥ ♥ ♥|\n |♥ ♥ ♥|\n |♥ ♥ ♥|\n |___0I|";
            case "J of Hearts": return "  _____\n |J  ww|\n | ♥ {)|\n | ♥ {)|\n | ♥ ♥ |\n |____L|";
            case "Q of Hearts": return "  _____\n |Q  ww|\n | ♥ {(|\n | ♥ {(|\n |♥♥♥♥|\n |____O|";
            case "K of Hearts": return "  _____\n |K  WW|\n | ♥ {)|\n | ♥ {)|\n |♥♥♥♥|\n |____C|";
            case "A of Hearts": return "  _____\n |A    |\n |  ♥  |\n |     |\n |____V|";
            case "2 of Diamonds": return "  _____\n |2    |\n |  ♦  |\n |     |\n |____Z|";
            case "3 of Diamonds": return "  _____\n |3    |\n | ♦ ♦ |\n |     |\n |____E|";
            case "4 of Diamonds": return "  _____\n |4    |\n | ♦ ♦ |\n | ♦ ♦ |\n |____h|";
            case "5 of Diamonds": return "  _____\n |5    |\n | ♦ ♦ |\n |  ♦  |\n | ♦ ♦ |\n |____S|";
            case "6 of Diamonds": return "  _____\n |6    |\n | ♦ ♦ |\n | ♦ ♦ |\n | ♦ ♦ |\n |____9|";
            case "7 of Diamonds": return "  _____\n |7    |\n | ♦ ♦ |\n |♦ ♦ ♦|\n | ♦ ♦ |\n |____L|";
            case "8 of Diamonds": return "  _____\n |8    |\n | ♦ ♦ |\n |♦ ♦ ♦|\n |♦ ♦ ♦|\n |____8|";
            case "9 of Diamonds": return "  _____\n |9    |\n |♦ ♦ ♦|\n |♦ ♦ ♦|\n |♦ ♦ ♦|\n |____6|";
            case "10 of Diamonds": return "  _____\n |10  ♦|\n |♦ ♦ ♦|\n |♦ ♦ ♦|\n |♦ ♦ ♦|\n |___0I|";
            case "J of Diamonds": return "  _____\n |J  ww|\n | ♦ {)|\n | ♦ {)|\n | ♦ ♦ |\n |____L|";
            case "Q of Diamonds": return "  _____\n |Q  ww|\n | ♦ {(|\n | ♦ {(|\n |♦♦♦♦|\n |____O|";
            case "K of Diamonds": return "  _____\n |K  WW|\n | ♦ {)|\n | ♦ {)|\n |♦♦♦♦|\n |____C|";
            case "A of Diamonds": return "  _____\n |A    |\n |  ♦  |\n |     |\n |____V|";
            case "2 of Clubs": return "  _____\n |2    |\n |  ♣  |\n |     |\n |____Z|";
            case "3 of Clubs": return "  _____\n |3    |\n | ♣ ♣ |\n |     |\n |____E|";
            case "4 of Clubs": return "  _____\n |4    |\n | ♣ ♣ |\n | ♣ ♣ |\n |____h|";
            case "5 of Clubs": return "  _____\n |5    |\n | ♣ ♣ |\n |  ♣  |\n | ♣ ♣ |\n |____S|";
            case "6 of Clubs": return "  _____\n |6    |\n | ♣ ♣ |\n | ♣ ♣ |\n | ♣ ♣ |\n |____9|";
            case "7 of Clubs": return "  _____\n |7    |\n | ♣ ♣ |\n |♣ ♣ ♣|\n | ♣ ♣ |\n |____L|";
            case "8 of Clubs": return "  _____\n |8    |\n | ♣ ♣ |\n |♣ ♣ ♣|\n |♣ ♣ ♣|\n |____8|";
            case "9 of Clubs": return "  _____\n |9    |\n |♣ ♣ ♣|\n |♣ ♣ ♣|\n |♣ ♣ ♣|\n |____6|";
            case "10 of Clubs": return "  _____\n |10  ♣|\n |♣ ♣ ♣|\n |♣ ♣ ♣|\n |♣ ♣ ♣|\n |___0I|";
            case "J of Clubs": return "  _____\n |J  ww|\n | ♣ {)|\n | ♣ {)|\n | ♣ ♣ |\n |____L|";
            case "Q of Clubs": return "  _____\n |Q  ww|\n | ♣ {(|\n | ♣ {(|\n |♣♣♣♣|\n |____O|";
            case "K of Clubs": return "  _____\n |K  WW|\n | ♣ {)|\n | ♣ {)|\n |♣♣♣♣|\n |____C|";
            case "A of Clubs": return "  _____\n |A    |\n |  ♣  |\n |     |\n |____V|";
            case "2 of Spades": return "  _____\n |2    |\n |  ♠  |\n |     |\n |____Z|";
            case "3 of Spades": return "  _____\n |3    |\n | ♠ ♠ |\n |     |\n |____E|";
            case "4 of Spades": return "  _____\n |4    |\n | ♠ ♠ |\n | ♠ ♠ |\n |____h|";
            case "5 of Spades": return "  _____\n |5    |\n | ♠ ♠ |\n |  ♠  |\n | ♠ ♠ |\n |____S|";
            case "6 of Spades": return "  _____\n |6    |\n | ♠ ♠ |\n | ♠ ♠ |\n | ♠ ♠ |\n |____9|";
            case "7 of Spades": return "  _____\n |7    |\n | ♠ ♠ |\n |♠ ♠ ♠|\n | ♠ ♠ |\n |____L|";
            case "8 of Spades": return "  _____\n |8    |\n | ♠ ♠ |\n |♠ ♠ ♠|\n |♠ ♠ ♠|\n |____8|";
            case "9 of Spades": return "  _____\n |9    |\n |♠ ♠ ♠|\n |♠ ♠ ♠|\n |♠ ♠ ♠|\n |____6|";
            case "10 of Spades": return "  _____\n |10  ♠|\n |♠ ♠ ♠|\n |♠ ♠ ♠|\n |♠ ♠ ♠|\n |___0I|";
            case "J of Spades": return "  _____\n |J  ww|\n | ♠ {)|\n | ♠ {)|\n | ♠ ♠ |\n |____L|";
            case "Q of Spades": return "  _____\n |Q  ww|\n | ♠ {(|\n | ♠ {(|\n |♠♠♠♠|\n |____O|";
            case "K of Spades": return "  _____\n |K  WW|\n | ♠ {)|\n | ♠ {)|\n |♠♠♠♠|\n |____C|";
            case "A of Spades": return "  _____\n |A    |\n |  ♠  |\n |     |\n |____V|";
            case "[Hidden]": return "  _____\n |     |\n |  ?  |\n |     |\n |_____|";
            default: return "  _____\n |     |\n |  ?  |\n |     |\n |_____|";
        }
    }
}
