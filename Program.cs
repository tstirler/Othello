using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Othello
{
    class Program
    {
        static void Main(string[] args)
        {
            int windowWidth = 60;
            int windowHeigth = 50;
            string player = "White";
            bool legalMove = false;
            int[] playerSelected = new int[2];
            Console.SetWindowSize(windowWidth, windowHeigth);

            GameBoard gameBoard = new GameBoard();
            gameBoard.initializeBoard();

            int[] score = new int[2];
            score = calculateScore(gameBoard.board);

            int counter = 0;
            bool gameRunning = true;

            Console.Clear();
            
            while (gameRunning)
            {
                WriteAt("White: " + score[0] + " - " + "Black: " + score[1], (windowWidth / 3) - 8, 0);
                drawGrid(gameBoard.board);
                do
                {
                    WriteAt("                                                                    ", 0, 39);
                    WriteAt("                                                                    ", 0, 40);
                    WriteAt(player + " player. Where do you want to put your piece? x,y ", 0, 39);
                    Console.WriteLine("");
                    playerSelected = inputPosition(Console.ReadLine());
                    legalMove = checkValidMove(playerSelected, gameBoard.board, player);
                } while (!legalMove);
                
                //placePiece();
                //changeHorizontal();
                //changekDiagonal();
                //chhangeVertical();
                calculateScore(gameBoard.board);
                checkValidGame(score);
                counter++;
                gameRunning = false;
                changePlayer(player);
            }
            checkWinner(score);
        }

        public static bool checkValidMove(int[] move, string[,] board, string player)
        {

            bool isValid = false;
            return isValid;
        }

        public static string changePlayer(string player)
        {
            if (player.Equals("White"))
            {
                player = "Black";
            }
            else
            {
                player = "White";
            }
            return player;
        }

        public static int[] inputPosition(string coordString)
        {
            int[] coordInt = new int[2];
            string[] rowName = new string[8] { "A", "B", "C", "D", "E", "F", "G", "H" };
            int rowCounter = 0;
            int counter = 0;
            string[] separator = new string[] {","};
            string[] coords = coordString.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            foreach (string c in rowName)
            {
                if (coords[0].ToUpper().Equals(c))
                {
                    coords[0] = Convert.ToString(rowCounter);
                }
                rowCounter++;
            }

            foreach (string c in coords)
            {

                coordInt[counter] = Convert.ToInt32(c);
                counter++;
            }
            return coordInt;
        }

        public static bool checkValidGame(int[] score) 
        {
            bool gameRunning = true;
            if (score[0] + score[1] == 64)
            {
                gameRunning = false;
            }
            else if (score[0] == 0 || score[1] == 0)
            {
                gameRunning = false;
            }
            return gameRunning;
        }

        public static void checkWinner(int[] score) 
        {
            if (score[0] > score[1])
            {
                Console.WriteLine("White is the winner!");
            }
            else if (score[0] < score[1])
            {
                Console.WriteLine("Black is the winner!");
            }
            else
            {
                Console.WriteLine("It is a tie!");
            }
        }

        public static int[] calculateScore(string[,] board)
        {
            int[] score = new int[2];
            foreach (string piece in board)
            {
                if (piece.Equals("W")) {
                    score[0]++;
                } else if (piece.Equals("B")) {
                    score[1]++;
                }
            }
            return score;
        }

        /// <summary>
        /// Draws a 8x8 board on the screen.
        /// </summary>
        static void drawGrid(string[,] board)
        {
            int gridSize = 32;
            int boxSize = gridSize / 8;
            int pieceX_Offset = boxSize * 2;
            int pieceY_Offset = 4;
            int pieceOffset = 4;

            int offset = 2;
            int rowCounter = 0;
            string[] rowName = new string[8] {"A", "B", "C", "D", "E", "F", "G", "H"};
            string[] columnName = new string[8] {"1", "2", "3", "4", "5", "6", "7", "8"};

            // printing the grid.
            for (int lineCount = offset; lineCount <= (8 * boxSize) + offset; lineCount += boxSize)
            {   
                for (int horizontalLine = boxSize + offset; horizontalLine <= (9 * boxSize) + offset; horizontalLine++)
                {
                    WriteAt("-", horizontalLine, lineCount);
                }

                for (int verticalCoord = offset; verticalCoord <= (8 * boxSize) + offset; verticalCoord++)
                {
                    WriteAt("|", lineCount + boxSize, verticalCoord);
                }

                if (lineCount < 8 * boxSize)
                {
                    WriteAt(rowName[rowCounter], offset, lineCount + 2);
                    WriteAt(columnName[rowCounter], lineCount + boxSize + 2, (8 * boxSize) + offset + 1);
                    rowCounter++;
                }
            }
            WriteAt("", 1, 9 * boxSize);

            //print the pieces
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    WriteAt(board[x, y], pieceX_Offset + (x * pieceOffset), pieceY_Offset + (y * pieceOffset));
                }
            }
            WriteAt("", 0, 39);
        }
        
        /// <summary>
        /// WriteAt positions the cursor and writes out a string.
        /// </summary>
        /// <param name="s">"s" is the string to output to console.</param>
        /// <param name="x">"x" is the x-coordinates in the console.</param>
        /// <param name="y">"y" is the y-coordinates in the console.</param>
        public static void WriteAt(string s, int x, int y)
        {
            try
            {
                Console.SetCursorPosition(x, y);
                Console.Write(s);
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.Clear();
                Console.WriteLine(e.Message);
            }
        }
    }
}
class GameBoard
{
    public string[,] board = new string[8, 8];
    public GameBoard() { }

    public void initializeBoard()
    {
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                this.board[x, y] = "";
            }
        }
        this.board[3, 3] = "W";
        this.board[3, 4] = "B";
        this.board[4, 3] = "B";
        this.board[4, 4] = "W";
    }
}

