using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading;

namespace FinalProject
{

    public static class Board
    {
        public static String [,] GameBoard = new String[6, 7];

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

        public static void DisplayBoard(int index)
        {
            Console.Clear();

            Console.Write("{0,2}", "");
            for (int i = 0; i < GameBoard.GetLength(1); i++)
            {
                if (i == index) Console.Write("{0,2}", "v");
                else Console.Write("{0,2}", "");
            }
            Console.WriteLine();


            for (int i = 0; i < GameBoard.GetLength(0); i++)
            {
                Console.Write("{0,2}", "|");
                for (int j = 0; j < GameBoard.GetLength(1); j++)
                {
                    Console.Write("{0,2}", GameBoard[i,j]);
                }
                Console.WriteLine("{0,2}", "|");
            }


            Console.Write("{0,2}", "");
            for (int i = 0; i < GameBoard.GetLength(1); i++)
            {
                Console.Write("{0,2}", "-");
            }
            Console.WriteLine();

        }
    }




    public class Player
    {
        public String PlayerChar { get; set; }
        public int Index { get; set; }
        public int Height { get; set; }


        public Player(String playerChar)
        {
            PlayerChar = playerChar;
        }

        public bool CheckWinner()
        {
            return false;
        }

        public void MakeMove()
        {
            Index = 0;
            bool inMenu = true;
            ConsoleKeyInfo keyinfo;

            Board.DisplayBoard(Index);


            while (inMenu)
            {
                keyinfo = Console.ReadKey();

                // Handle each key input (down arrow will write the menu again with a different selected item)
                if (keyinfo.Key == ConsoleKey.RightArrow)
                {
                    if (Index + 1 < 7)
                    {
                        Index++;
                        Board.DisplayBoard(Index);
                    }
                }
                if (keyinfo.Key == ConsoleKey.LeftArrow)
                {
                    if (Index - 1 >= 0)
                    {
                        Index--;
                        Board.DisplayBoard(Index);
                    }
                }

                // Handle different action for the option
                if (keyinfo.Key == ConsoleKey.Enter)
                {
                    inMenu = PutPlayerChar();
                    if (!inMenu) Board.DisplayBoard(Index);
                }

                if (keyinfo.Key == ConsoleKey.Escape)
                {
                    inMenu = false;
                    Program.DisplayMenu();
                }
            }
        }

        public bool PutPlayerChar()
        {
            Height = -1;

            for(int i = 5; i >= 0; i--)
            {
                if(Board.GameBoard[i, Index] == "#")
                {
                    Height = i;
                    break;
                }
            }
            if (Height == -1) return true;

            Board.GameBoard[Height, Index] = this.PlayerChar;
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
        public static Option InfoOption;
        public static List<Player> Players;

        static void Main(string[] args)
        {
            DisplayMenu();
        }

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

                // Handle each key input (down arrow will write the menu again with a different selected item)
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

                // Handle different action for the option
                if (keyinfo.Key == ConsoleKey.Enter)
                {
                    MenuOptions[index].Selected.Invoke();
                    index = 0;
                    inMenu = false;
                }
            }
        }

        public static void Game()
        {
            Board.intializeValues();

            Players = new List<Player>
            {
                new Player("X"),
                new Player("O")
            };

            bool Gameon = true;
            int turn = 0;

            while (Gameon)
            {
                turn %= 2;

                Players[turn].MakeMove();

                if(Players[turn].CheckWinner())
                {
                    Gameon = false;
                }
                turn++;
            }
        }

        public static void DisplayInfo()
        {
            InfoOption = new Option("Back", DisplayMenu);

            Console.Clear();

            Console.WriteLine("SODV1202: Introduction to Object Oriented Programming");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("");

            Console.Write("{0,-2}", ">");
            Console.WriteLine(InfoOption.Name);

            ConsoleKeyInfo keyinfo;
            bool inMenu = true;

            while (inMenu)
            {
                keyinfo = Console.ReadKey();

                if (keyinfo.Key == ConsoleKey.Enter)
                {
                    InfoOption.Selected.Invoke();
                    inMenu = false;
                }
            }

        }

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
