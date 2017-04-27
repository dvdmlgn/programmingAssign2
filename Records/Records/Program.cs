//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Author: David Mulligan
// Date: 26 - 4 - 17
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

        public static int scoresSum = 0;
        public static int scoresAverage = 0;

        public static string filepath = @"C:\Users\David\Documents\College\Programming\scores.txt";

        public static string[,] scores = new string[3,5];
        public static int[] score = new int[3];

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

            FiniteStateMachine();


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
             
            PrintAverageScore();
            PrintStanardDeviation();
            PrintTopPlayer();
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


            int[] count = new int[5];
            int[] irish = new int[5];
            int[] nonIrish = new int[5];
             
            foreach(int number in score)
            {
                int counter;

                Console.WriteLine();

                if(number < 400)
                {
                    count[0]++;

                    counter = 0;
                }

                else if (number < 600)
                {
                    count[1]++;

                    counter = 1;
                }

                else if (number < 700)
                {
                    count[2]++;

                    counter = 2;
                }

                else if (number < 1000)
                {
                    count[3]++;

                    counter = 3;
                }

                else
                {
                    count[4]++;

                    counter = 4;
                }

                Console.WriteLine(counter);
                
            }

            foreach(int number in count)        // just for debug use
            {
                Console.WriteLine(number);
            }


            input = UserInput(back2Menu);

            if (input.Equals(@"\"))
            {
                newState = 0;
            }
        }

        static void Search()
        {
            string input;
            bool matchFound = false;
            int matchLocation = 0;

            do
            {
                Header();

                Console.WriteLine("Search");
                Spacer();

                input = UserInput(searchMessage + back2Menu);

                if (input.Equals(@"\"))
                {
                    newState = 0;
                }

                else
                {
                    for (int i = 0; i < 3; i++)
                    {
                        if (input.Equals(scores[i, 0]))
                        {
                            matchFound = true;

                            if (matchFound)
                            {
                                matchLocation = i;
                            }
                        }
                    }

                    // Console.WriteLine("your input was {0}", input);

                    if (matchFound)
                    {
                        Header();

                        Console.WriteLine(foundMatch, input);
                        Spacer();

                        Console.WriteLine("Player Name: {0}", scores[matchLocation,(int)Scores.Name]);
                        Console.WriteLine("Player Score: {0}", scores[matchLocation, (int)Scores.Score]);

                        Spacer();

                        Console.WriteLine(searchAgain);

                        Footer();
                    }

                    else
                    {
                        Header();

                        Console.WriteLine(noMatch, input);
                        Console.WriteLine(searchAgain);

                        Footer();
                    }

                    Header();
                }
            } while (newState != 0);
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

        #region Player Report Child Functions
        static void PrintPlayer()
        {
            string displayFormat = "{0,-15}{1,-10}{2}";

            for(int i = 0; i < 3; i++)
            {
                Console.WriteLine(displayFormat, scores[i,(int)Scores.Name], scores[i, (int)Scores.Score], scores[i, (int)Scores.Stars]);
            }
        }

        static void PrintAverageScore()
        {

            for(int i = 0; i < score.Length; i++)
            {
                scoresSum += score[i];
            }

            scoresAverage = scoresSum / score.Length;

            Console.WriteLine(scoresAverage);     
        }

        static void PrintStanardDeviation()
        {
            double standardDeviation = 0;
            double mean = 0;
            double squaredMean = 0;
            double squaredSum = 0;

            int[] scoresSquared = new int[score.Length];

            mean = scoresAverage;

            int number;
            int numberSquared;

            for(int i =0; i < score.Length; i++)        // Works out the squared values of the scores
            {
                number = score[i];

                numberSquared = (int)Math.Pow((double)(number - scoresAverage), 2.0);

                scoresSquared[i] = numberSquared;
            }

            for(int i = 0; i < scoresSquared.Length; i++)       // Works out the Sum of all the squared values
            {
                squaredSum += scoresSquared[i];
            }

            squaredMean = squaredSum / scoresSquared.Length;

            standardDeviation = Math.Sqrt(squaredMean);

            Console.WriteLine(standardDeviation);
        }

        static void PrintTopPlayer()
        {
            int highestScore = 0;
            int scoreplace = 0;

            for(int i = 0; i < score.Length; i++)
            {
                if(score[i] > highestScore)
                {
                    highestScore = score[i];
                    scoreplace = i;
                }
            }

            Console.WriteLine("high score belongs to {0} with {1}", scores[scoreplace, (int)Scores.Name], scores[scoreplace, (int)Scores.Score]);
        }
        #endregion

        #region Search Child Functions

        #endregion

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
        static string searchMessage = "Please enter in a player number to begin, or \n";
        static string noMatch = "I'm sorry but, No Matches have been found for {0}";
        static string searchAgain = "Please press any key to search again...";
        static string foundMatch = "A match for Player Number {0} was found";
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

    }
}