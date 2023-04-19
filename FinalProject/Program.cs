using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading;

namespace FinalProject
{

    public static class Board
    {
        public static String [,] GameBoard = new String[6, 7];  // 2D array for 6*7 Connect Four Grid

        // Initialize all elements of GameBoard array with '#'
        public static void intializeValues()
        {
            for(int i = 0; i < GameBoard.GetLength(0); i++)
            {
                for(int j = 0; j < GameBoard.GetLength(1); j++)
                {
                    GameBoard[i, j] = "#";
                }
            }
        }

        // Displays the Connect Four Grid
        public static void DisplayBoard(int index, int turn)
        {
            Console.Clear();

            // Displays the Selector on top
            Console.Write("{0,2}", "");
            for (int i = 0; i < GameBoard.GetLength(1); i++)
            {
                if (i == index) Console.Write("{0,2}", "v");
                else Console.Write("{0,2}", "");
            }
            Console.WriteLine();

            // Displays the game grid
            for (int i = 0; i < GameBoard.GetLength(0); i++)
            {
                Console.Write("{0,2}", "|");
                for (int j = 0; j < GameBoard.GetLength(1); j++)
                {
                    Console.Write("{0,2}", GameBoard[i,j]);
                }
                Console.WriteLine("{0,2}", "|");
            }

            // Displays the dashes below
            Console.Write("{0,2}", "");
            for (int i = 0; i < GameBoard.GetLength(1); i++)
            {
                Console.Write("{0,2}", "-");
            }
            Console.WriteLine();

            // Displays player turn
            if (turn == 0) Console.WriteLine("{0,14}","X 's turn"); else Console.WriteLine("{0,14}", "O 's turn");
        }
    }



    
    public class Player
    {
        public String PlayerChar { get; set; }
        public int Index { get; set; }
        public int Pos { get; set; }


        public Player(String playerChar)
        {
            PlayerChar = playerChar;
        }

        // Checks if the Player has won the match or not
        public bool CheckWinner()
        {
            int count;
            int limit;

            // Vertical Check
            count = 1;
            for (int i = 1; i <= Pos; i++)
            {
                if (Board.GameBoard[Pos-i, Index] == PlayerChar) count++;
                else break;
            }
            for (int i = 1; i < Board.GameBoard.GetLength(0) - Pos; i++)
            {
                if (Board.GameBoard[Pos + i, Index] == PlayerChar) count++;
                else break;
            }
            if (count >= 4) return true;

            // Horizontal Check
            count = 1;
            for (int i = 1; i <= Index; i++)
            {
                if (Board.GameBoard[Pos, Index - i] == PlayerChar) count++;
                else break;
            }
            for (int i = 1; i < Board.GameBoard.GetLength(0) - Index; i++)
            {
                if (Board.GameBoard[Pos, Index + i] == PlayerChar) count++;
                else break;
            }
            if (count >= 4) return true;

            // Diagonal 1 Check
            count = 1;
            limit = Math.Min(Index, Pos);
            for (int i = 1; i <= limit; i++)
            {
                if (Board.GameBoard[Pos - i, Index - i] == PlayerChar) count++;
                else break;
            }
            limit = Board.GameBoard.GetLength(0) - Pos < Board.GameBoard.GetLength(1) - Index? Board.GameBoard.GetLength(0) - Pos : Board.GameBoard.GetLength(1) - Index;
            for (int i = 1; i < limit; i++)
            {
                if (Board.GameBoard[Pos + i, Index + i] == PlayerChar) count++;
                else break;
            }
            if (count >= 4) return true;

            // Diagonal 2 Check
            count = 1;
            limit = Pos < Board.GameBoard.GetLength(1) - Index ? Pos + 1 : Board.GameBoard.GetLength(1) - Index;
            for (int i = 1; i < limit; i++)
            {
                if (Board.GameBoard[Pos - i, Index + i] == PlayerChar) count++;
                else break;
            }
            limit = Index < Board.GameBoard.GetLength(0) - Pos ? Index + 1 : Board.GameBoard.GetLength(0) - Pos;
            for (int i = 1; i < limit; i++)
            {
                if (Board.GameBoard[Pos + i, Index - i] == PlayerChar) count++;
                else break;
            }
            if (count >= 4) return true;

            return false;
        }

        // Checks if all grid is filled hence draw
        public bool CheckDraw()
        {
            for (int i = 0; i < Board.GameBoard.GetLength(1); i++)
            {
                if(Board.GameBoard[0, i] == "#") return false;
            }
            return true;
        }

        // Runs when player has to make a move 
        // Is responsible for the movement of the cursor and selecting coloumn by player
        public void MakeMove(int turn)
        {
            Index = 0;
            bool inMenu = true;
            ConsoleKeyInfo keyinfo;

            Board.DisplayBoard(Index, turn);


            while (inMenu)
            {
                keyinfo = Console.ReadKey();

                if (keyinfo.Key == ConsoleKey.RightArrow)
                {
                    if (Index + 1 < 7)
                    {
                        Index++;
                        Board.DisplayBoard(Index, turn);
                    }
                }
                if (keyinfo.Key == ConsoleKey.LeftArrow)
                {
                    if (Index - 1 >= 0)
                    {
                        Index--;
                        Board.DisplayBoard(Index, turn);
                    }
                }

                if (keyinfo.Key == ConsoleKey.Enter)
                {
                    inMenu = PutPlayerChar();
                    if (!inMenu) Board.DisplayBoard(Index, turn);
                }

                if (keyinfo.Key == ConsoleKey.Escape)
                {
                    inMenu = false;
                    Program.DisplayMenu();
                }
            }
        }

        // Puts the player charter in the GameBoard Array on the available spot in column if available
        public bool PutPlayerChar()
        {
            Pos = -1;

            for(int i = 5; i >= 0; i--)
            {
                if(Board.GameBoard[i, Index] == "#")
                {
                    Pos = i;
                    break;
                }
            }
            if (Pos == -1) return true;

            Board.GameBoard[Pos, Index] = this.PlayerChar;
            return false;
        }

    }


    public class Option
    {
        public string Name { get; }
        public Action Selected { get; }

        public Option(string name, Action selected)
        {
            Name = name;
            Selected = selected;
        }
    }



    internal static class Program
    {
        public static List<Option> MenuOptions;
        public static Option BackOption;
        public static List<Player> Players;

        static void Main(string[] args)
        {
            DisplayMenu();
        }

        // Displays the main Menu of the Game
        public static void DisplayMenu()
        {
            MenuOptions = new List<Option>
            {
                new Option("Play", Game),
                new Option("Info", DisplayInfo),
                new Option("Exit", () => Environment.Exit(0)),
            };

            bool inMenu = true;
            int index = 0;
            ConsoleKeyInfo keyinfo;

            WriteMenu(MenuOptions, MenuOptions[index]);

            while (inMenu)
            {
                keyinfo = Console.ReadKey();

                if (keyinfo.Key == ConsoleKey.DownArrow)
                {
                    if (index + 1 < MenuOptions.Count)
                    {
                        index++;
                        WriteMenu(MenuOptions, MenuOptions[index]);
                    }
                }

                if (keyinfo.Key == ConsoleKey.UpArrow)
                {
                    if (index - 1 >= 0)
                    {
                        index--;
                        WriteMenu(MenuOptions, MenuOptions[index]);
                    }
                }

                if (keyinfo.Key == ConsoleKey.Enter)
                {
                    MenuOptions[index].Selected.Invoke();
                    index = 0;
                    inMenu = false;
                }
            }
        }

        // Handles the running of the main game
        public static void Game()
        {
            // Initalizes the GameBoard
            Board.intializeValues();

            // Makes two player objects to play
            Players = new List<Player>
            {
                new Player("X"),
                new Player("O")
            };

            BackOption = new Option("Back", DisplayMenu);
            bool Gameon = true;
            int turn = 0;


            // Runs the Game
            while (Gameon)
            {
                turn %= 2;
                // Player turn
                Players[turn].MakeMove(turn);

                // Check if the turn made lead to win and if so, shows the winner
                if(Players[turn].CheckWinner())
                {
                    Gameon = false;
                    Console.SetCursorPosition(0, Console.CursorTop - 1);
                    Console.WriteLine("{0,3} {1}", Players[turn].PlayerChar, "is the Winner");
                }

                // Checks if the turn made lead to draw and if so, shows draw message
                if (Players[turn].CheckDraw())
                {
                    Gameon = false;
                    Console.WriteLine("It is a Draw");
                }

                turn++; // turn increment
            }


            // Shows Restart and back options after game is over
            Console.Write("{0,-2}", ">");
            Console.WriteLine(BackOption.Name);

            ConsoleKeyInfo keyinfo;
            bool inMenu = true;

            while (inMenu)
            {
                keyinfo = Console.ReadKey();

                if (keyinfo.Key == ConsoleKey.Enter)
                {
                    BackOption.Selected.Invoke();
                    inMenu = false;
                }
            }
        }


        // Displays the Info Page
        public static void DisplayInfo()
        {
            BackOption = new Option("Back", DisplayMenu);

            Console.Clear();

            // Displays the program Info
            Console.WriteLine("SODV1202: Introduction to Object Oriented Programming");
            Console.WriteLine("Final Project: Connect Four");
            Console.WriteLine("Group Name: 404 Found");
            Console.WriteLine("Members: Jagdeep Singh (j.singh2232@mybvc.ca)");

            // Shows the back option
            Console.Write("{0,-2}", ">");
            Console.WriteLine(BackOption.Name);

            ConsoleKeyInfo keyinfo;
            bool inMenu = true;
            while (inMenu)
            {
                keyinfo = Console.ReadKey();

                if (keyinfo.Key == ConsoleKey.Enter)
                {
                    BackOption.Selected.Invoke();
                    inMenu = false;
                }
            }

        }

        // Writes the Menu Options provided
        static void WriteMenu(List<Option> options, Option selectedOption)
        {
            Console.Clear();

            Console.WriteLine("Connect Four");

            foreach (Option option in options)
            {
                if (option == selectedOption)
                {
                    Console.Write("{0,-2}", ">");
                }
                else
                {
                    Console.Write("{0,-2}", "");
                }

                Console.WriteLine(option.Name);
            }
        }
    }
}
