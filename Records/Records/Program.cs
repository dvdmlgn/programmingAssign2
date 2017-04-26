//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Author: David Mulligan
// Date: 11 - 4 - 17
// ------------------------------------------------------------------------------
// Program Description:
// ``````````````````````````````````````````
//
//
//
//
//
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Threading;
using System.IO;

namespace Records
{
    class Program
    {
        #region setup
        public static int currentState = 0;
        public static int newState = 0;

        public static string filepath = @"C:\Users\David\Documents\College\Programming\scores.txt";

        public static string[,] scores = new string[3,5];

        public enum Scores
        {
            Number,
            Name,
            Nationality,
            Score,
            Stars
        }

        public enum State
        {
            Menu,
            PlayerReport,
            ScoreAnalysis,
            Search,
            Exit
        }

        public enum Option
        {
            PlayerReport,
            ScoreAnalysis,
            Search,
            Exit
        }

        public enum Format
        {
            Text,
            Integer
        }
        #endregion

        static void Main(string[] args)
        {
            Header();

            PopulateArray(scores);

            CalculateStarRating(scores);

            //FiniteStateMachine();

            PlayerReport();

            Footer();
        }

        #region File Handeling
        static void FileExists(string filepath)
        {
            bool exists = false;

            exists = File.Exists(filepath);

            Console.WriteLine(exists);
        }

        static void PopulateArray(string[,] array)       // no need for 'ref' keyword as Arrays are 'pass-by-reference' by default
        {
            int lineNumber = 0;     // this is put outside of the foreach loop because it would stay at 0 when it was inside it

            foreach (string line in File.ReadLines(filepath))
            {
                string[] lineItems = line.Split(',');

                for(int i = 0; i < 4; i++)
                {
                   array[lineNumber,i] = lineItems[i];
                }

                lineNumber++;
            }

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Console.Write(array[i, j] + ", ");
                }

                Console.WriteLine();
            }
        }

        static void CalculateStarRating(string[,] array)
        {
            int[] score = new int[3];
            string[] stars = new string[3];

            for(int i = 0; i < 3; i++)      // turns scores column in integers
            {
                score[i] = int.Parse(array[i, (int)Scores.Score]);
            } 

            for(int i = 0; i < 3; i++)      // assigns the star rating based on the scores
            {
                if(score[i] < 400)
                {
                    stars[i] = "*";
                }

                else if (score[i] >= 400 && score[i] < 600)
                {
                    stars[i] = "**";
                }

                else if (score[i] >= 600 && score[i] < 700)
                {
                    stars[i] = "***";
                }

                else if (score[i] >= 700 && score[i] < 1000)
                {
                    stars[i] = "****";
                }

                else
                {
                    stars[i] = "*****";
                }
            }

            for(int i = 0; i < 3; i++)      // puts the star ratings into the "scores" array
            {
                scores[i, (int)Scores.Stars] = stars[i];
            }

        }
        #endregion

        static void FiniteStateMachine()
        {
            do
            {
                switch (currentState)
                {
                    case 0:
                        Menu();
                        break;

                    case 1:
                        PlayerReport();
                        break;

                    case 2:
                        ScoreAnalysis();
                        break;

                    case 3:
                        Search();
                        break;

                    case 4:
                        Exit();
                        break;

                    case 5:
                        break;

                }

                currentState = newState;

            } while (currentState != 5);
        }

        #region states
        static void Menu()
        {
            Header();

            DisplayMenu();
            Spacer();
            newState = int.Parse(UserInput(menuSelect));

        }

        static void PlayerReport()
        {
            string input;

            Header();

            Console.WriteLine("Player Report");
            Spacer();

            PrintPlayer();
            Spacer();

            input = UserInput(back2Menu);

            if (input.Equals(@"\"))
            {
                newState = 0;
            }
        }

        static void ScoreAnalysis()
        {
            string input;

            Header();

            Console.WriteLine("Score Analysis");
            Spacer();
            input = UserInput(back2Menu);

            if (input.Equals(@"\"))
            {
                newState = 0;
            }
        }

        static void Search()
        {
            string input;

            Header();

            Console.WriteLine("Search");
            Spacer();
            input = UserInput(back2Menu);

            if (input.Equals(@"\"))
            {
                newState = 0;
            }
        }

        static void Exit()
        {
            Header();

            Console.WriteLine("Goodbye");

            Thread.Sleep(300);

            newState = 5;

            Header();
        }
        #endregion

        #region child Functions

        #region Menu child functions
        static void DisplayMenu()
        {
            Console.WriteLine("1. Player Report");
            Console.WriteLine("2. Score Analysis Report");
            Console.WriteLine("3. Search for a Player");
            Console.WriteLine("4. Exit");
        }
        #endregion

        static void PrintPlayer()
        {
            string displayFormat = "{0,-15}{1,-10}{2}";

            for(int i = 0; i < 3; i++)
            {
                Console.WriteLine(displayFormat, scores[i,(int)Scores.Name], scores[i, (int)Scores.Score], scores[i, (int)Scores.Stars]);
            }
        }

        #endregion

        static string UserInput(string message)
        {
            string userInput;

            Console.WriteLine(message);
            userInput = Console.ReadLine();

            return userInput;
        }

        #region Text Strings
        static string menuSelect = "Please select an option from above (in format 1 - 4 or program will crash)"; // text will need to be changed back, additional text is only for debugging proposes
        static string back2Menu = @"Please press the backslash '\', to go back to the menu";
        #endregion

        #region housekeeping
        static void Header()
        {
            Console.Clear();
        }

        static void Spacer()
        {
            Console.WriteLine();
        }

        static void Footer()
        {
            Console.ReadKey();
        }
        #endregion

        #region debug functions

        #endregion

        #region old code
        //#region Menu Section
        /*      static int Menu()
              {
                  string selectionString;
                  bool correctInput = false;
                  MenuOption selectionOption;

                  Console.Clear();

                  Console.WriteLine("Please select an option");

                  Console.WriteLine();

                  Console.WriteLine("1. Player Report");
                  Console.WriteLine("2. Score Analysis Report");
                  Console.WriteLine("3. Search for a Player");
                  Console.WriteLine("4. Exit");

                  selectionString = Console.ReadLine();
                  correctInput = OptionCheck(selectionString);
                  if (correctInput)
                  {
                      selectionOption = (MenuOption)Enum.Parse(typeof(MenuOption), selectionString); // Converts the End User's String choice into an Enum('MenuOption') format

                      MenuSelection(selectionOption);
                  }

                  else
                  {
                      selectionOption = (MenuOption)Enum.Parse(typeof(MenuOption), Menu().ToString);
                  }

                  Console.ReadKey();
               */  // return selectionOption;
                   /*
                  }

               #region Menu Child Functions

                  static bool OptionCheck(string option)
                  {
                      bool isGood = false;

                      if(option.Equals("1") || option.Equals("2") || option.Equals("3") || option.Equals("4"))
                      {
                          isGood = true;
                      }

                      return isGood;
                  }

                      static void MenuSelection(MenuOption choice)
                      {
                          Console.WriteLine("You chose {0}", choice);
                      }
                  #endregion // Child functions

                  #endregion

              */
        #endregion

    }
}