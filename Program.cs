using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows;

namespace Othello
{
    class Program
    {
        static void Main(string[] args)
        {
            int windowWidth = 70;
            int windowHeigth = 45;
            string player = "White";
            bool[] legalMove = new bool[9];
            int[] playerSelected = new int[2];
            Console.SetWindowSize(windowWidth, windowHeigth);

            GameBoard gameBoard = new GameBoard();
            gameBoard.initializeBoard();
            gameBoard.calculateScore();

            int counter = 0;
            bool gameRunning = true;

            Console.Clear();
            
            while (gameRunning)
            {
                WriteAt("White: " + gameBoard.getScore(0) + " - " + "Black: " + gameBoard.getScore(1), (windowWidth / 3) - 8, 0);
                gameBoard.drawGrid();
                do
                {
                    playerSelected = getMove(player);
                    legalMove = gameBoard.checkValidMove(playerSelected, player);
                } while (!legalMove[0]);
                //player = getPlayer();

                gameBoard.placePiece(playerSelected, player, legalMove);
                gameBoard.calculateScore();
                gameRunning = checkValidGame(gameBoard.getScore());
                counter++;
                player = changePlayer(player);
            }
        gameBoard.drawGrid();
        checkWinner(gameBoard.getScore(), counter);
        }

        /// <summary>
        /// Gets input from current player for where player wants to place the piece.
        /// </summary>
        /// <returns>string</returns>
        public static int[] getMove(string player)
        {
            int[] playerSelected = new int[2];
            WriteAt("                                                                    ", 0, 39);
            WriteAt("                                                                    ", 0, 40);
            WriteAt(player + " player. Where do you want to put your piece? x,y", 0, 39);
            Console.WriteLine("");
            try
            {
                playerSelected = inputPosition(Console.ReadLine());
            }
            catch (Exception)
            {
                playerSelected = getMove(player);
            }
            return playerSelected;
        }

        /// <summary>
        /// Changes from passed player to opposite
        /// </summary>
        /// <param name="player">String: White/Black</param>
        /// <returns>string: Black/White</returns>
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

        /// <summary>
        /// Converts inputstring on form [A-H],[0-7] to numeric int[0-7,0-7]
        /// </summary>
        /// <param name="coordString">String: [A-H],[0-7]</param>
        /// <returns>int[0-7,0-7]</returns>
        public static int[] inputPosition(string coordString)
        {
            int[] coordInt = new int[2];
            int[] returnInt = new int[2];
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

            coordInt[1] = coordInt[1] - 1;
            returnInt[0] = coordInt[1];
            returnInt[1] = coordInt[0];
            return returnInt;
        }

        /// <summary>
        /// Checks if the game is stil valid to continue.
        /// </summary>
        /// <param name="score">int[2]</param>
        /// <returns>true/false</returns>
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

        /// <summary>
        /// Checkes the passed score to see who won
        /// </summary>
        /// <param name="score">int[2]</param>
        /// <param name="numberOfTurns">int</param>
        public static void checkWinner(int[] score, int numberOfTurns) 
        {
            if (score[0] > score[1])
            {
                Console.WriteLine("White is the winner! Won in " + numberOfTurns + " turns.");
            }
            else if (score[0] < score[1])
            {
                Console.WriteLine("Black is the winner! Won in " + numberOfTurns + " turns.");
            }
            else
            {
                Console.WriteLine("It is a tie! It took " + numberOfTurns + " turns");
            }
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

/// <summary>
/// GameBoard Class contains gameBoard object definition and board-methods.
/// </summary>
class GameBoard
{
    int[] score = new int[2];
    public string[,] board = new string[8, 8];
    public GameBoard() { }

    /// <summary>
    /// Initializes the board, filling the array with default pieces, and blank for the rest of the board.
    /// </summary>
    public void initializeBoard()
    {
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                this.board[x, y] = " ";
            }
        }
        this.board[3, 3] = "W";
        this.board[3, 4] = "B";
        this.board[4, 3] = "B";
        this.board[4, 4] = "W";
    }

    /// <summary>
    /// Places the players piece on the selected location
    /// </summary>
    /// <param name="playerSelected">Coords for the piece. int[2]</param>
    /// <param name="player">string: White/Black</param>
    public void placePiece(int[] playerSelected, string player, bool[] toChange) 
    {
        this.board[playerSelected[0],playerSelected[1]] = Convert.ToString(player[0]);
        checkPosition(playerSelected, player, "Up", toChange[1]);
        checkPosition(playerSelected, player, "UpRight", toChange[2]);
        checkPosition(playerSelected, player, "Right", toChange[3]);
        checkPosition(playerSelected, player, "DownRight", toChange[4]);
        checkPosition(playerSelected, player, "Down", toChange[5]);
        checkPosition(playerSelected, player, "DownLeft", toChange[6]);
        checkPosition(playerSelected, player, "Left", toChange[7]);
        checkPosition(playerSelected, player, "UpLeft", toChange[8]);
    }

    /// <summary>
    /// Loops through the gameboard and counts number of white and black pieces.
    /// Score is saved in the objects int[] score.
    /// </summary>
    public void calculateScore()
    {
        //int[] score = new int[2];
        this.score[0] = 0;
        this.score[1] = 0;
        foreach (string piece in this.board)
        {
            if (piece.Equals("W"))
            {
                this.score[0]++;
            }
            else if (piece.Equals("B"))
            {
                this.score[1]++;
            }
        }
    }

    /// <summary>
    /// Grabs and returns the objects int[] score.
    /// </summary>
    /// <returns>int[2]</returns>
    public int[] getScore()
    {
        return this.score;
    }

    /// <summary>
    /// Grabs and returns the obcets int[player] score
    /// </summary>
    /// <param name="player">int player</param>
    /// <returns></returns>
    public int getScore(int player)
    {
        return this.score[player];
    }

    /// <summary>
    /// Draws out the board and fills in the pieces.
    /// </summary>
    public void drawGrid()
    {
        int gridSize = 32;
        int boxSize = gridSize / 8;
        int pieceX_Offset = boxSize * 2;
        int pieceY_Offset = 4;
        int pieceOffset = 4;

        int offset = 2;
        int rowCounter = 0;
        string[] rowName = new string[8] { "A", "B", "C", "D", "E", "F", "G", "H" };
        string[] columnName = new string[8] { "1", "2", "3", "4", "5", "6", "7", "8" };

        // printing the grid.
        for (int lineCount = offset; lineCount <= (8 * boxSize) + offset; lineCount += boxSize)
        {
            for (int horizontalLine = boxSize + offset; horizontalLine <= (9 * boxSize) + offset; horizontalLine++)
            {
                Othello.Program.WriteAt("-", horizontalLine, lineCount);
            }

            for (int verticalCoord = offset; verticalCoord <= (8 * boxSize) + offset; verticalCoord++)
            {
                Othello.Program.WriteAt("|", lineCount + boxSize, verticalCoord);
            }

            if (lineCount < 8 * boxSize)
            {
                Othello.Program.WriteAt(rowName[rowCounter], offset, lineCount + 2);
                Othello.Program.WriteAt(columnName[rowCounter], lineCount + boxSize + 2, (8 * boxSize) + offset + 1);
                rowCounter++;
            }
        }
        Othello.Program.WriteAt("", 1, 9 * boxSize);

        //print the pieces
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                Othello.Program.WriteAt(this.board[x, y], pieceX_Offset + (x * pieceOffset), pieceY_Offset + (y * pieceOffset));
            }
        }
        Othello.Program.WriteAt("", 0, 39);
    }

    /// <summary>
    /// Checks if the int[] move is a valid place to put a piece.
    /// </summary>
    /// <param name="move">int[2]</param>
    /// <param name="player">player placing the piece</param>
    /// <returns></returns>
    public bool[] checkValidMove(int[] move, string player)
    {
        bool[] isValid = new bool[9];
        
        bool isValidUp, isValidUpLeft, isValidLeft, isValidDownLeft, isValidDown, isValidDownRight, isValidRight, isValidUpRight;

        isValid[1] = isValidUp = checkPosition(move, player, "Up", false);
        isValid[2] = isValidUpRight = checkPosition(move, player, "UpRight", false);
        isValid[3] = isValidRight = checkPosition(move, player, "Right", false);
        isValid[4] = isValidDownRight = checkPosition(move, player, "DownRight", false);
        isValid[5] = isValidDown = checkPosition(move, player, "Down", false);
        isValid[6] = isValidDownLeft = checkPosition(move, player, "DownLeft", false);
        isValid[7] = isValidLeft = checkPosition(move, player, "Left", false);
        isValid[8] = isValidUpLeft = checkPosition(move, player, "UpLeft", false);

        if (move[0] < 8 && move[1] < 8 && this.board[move[0], move[1]].Equals(" "))
        {
            if (isValidUp || isValidUpLeft || isValidLeft || isValidDownLeft || isValidDown || isValidDownRight || isValidRight || isValidUpRight)
            {
                isValid[0] = true;
            }
            else
            {
                isValid[0] = false;
            }
        }
        else
        {
            isValid[0] = false;
        }
        return isValid;
    }

    /// <summary>
    /// Checks from the given position to the end of the board in the direction given.
    /// </summary>
    /// <param name="position">Position to check from int[2]</param>
    /// <param name="player">Player placing the piece. string: White or Black</param>
    /// <param name="direction">Direction to check. String: Up, UpRight, Right, DownRight, Down, DownLeft, Left, UpLeft</param>
    /// <returns>Returns: true/false</returns>
    public bool checkPosition(int[] position, string player, string direction, bool toChange)
    {
        bool isValid;
        int offsetHorizontal;
        int offsetVertical;
        string lastChecked = player;
        string playerChar = Convert.ToString(player[0]);

        switch (direction)
        {
            case "Up":
                offsetHorizontal = 0;
                offsetVertical = -1;
                break;
            case "UpRight":
                offsetHorizontal = 1;
                offsetVertical = -1;
                break;
            case "Right":
                offsetHorizontal = 1;
                offsetVertical = 0;
                break;
            case "DownRight":
                offsetHorizontal = 1;
                offsetVertical = 1;
                break;
            case "Down":
                offsetHorizontal = 0;
                offsetVertical = 1;
                break;
            case "DownLeft":
                offsetHorizontal = -1;
                offsetVertical = 1;
                break;
            case "Left":
                offsetHorizontal = -1;
                offsetVertical = 0;
                break;
            case "UpLeft":
                offsetHorizontal = -1;
                offsetVertical = -1;
                break;
            default:
                offsetHorizontal = 0;
                offsetVertical = 0;
                break;
        }

        if (offsetVertical == 0 && offsetHorizontal == 0)
        {
            return false;
        }

        int oppositeCount = 0;
        int[] nextPosition = new int[2];
        nextPosition[0] = position[0] + offsetHorizontal;
        nextPosition[1] = position[1] + offsetVertical;

        //while the position is inside the board
        while ((nextPosition[0] >= 0 && nextPosition[0] < 8) && (nextPosition[1] >= 0 && nextPosition[1] < 8))
        {
            // If the piece checked equals the players color, set the lastChecked to the same and exit the loop.
            if (this.board[nextPosition[0], nextPosition[1]].Equals(playerChar))
            {
                lastChecked = player;
                break;
            }
            
            //If the position checked is blank exit the While-loop
            if (this.board[nextPosition[0], nextPosition[1]].Equals(" ")) 
            {
                break;
            }

            oppositeCount++;
            if (toChange)
            {
                if (player.Equals("White")) {
                    this.board[nextPosition[0], nextPosition[1]] = "W";
                }
                else
                {
                    this.board[nextPosition[0], nextPosition[1]] = "B";
                }
                
            }
            
            //If none of the above stops the loop, set lastChecked to opposite color of player.
            if (player.Equals("White"))
            {
                lastChecked = "Black";
            }
            else
            {
                lastChecked = "White";
            }

            //change position to next piece to check.
            nextPosition[0] = nextPosition[0] + offsetHorizontal;
            nextPosition[1] = nextPosition[1] + offsetVertical;
        }

        if (oppositeCount > 0 && Convert.ToString(lastChecked[0]).Equals(playerChar))
        {
            isValid = true;
        }
        else
        {
            isValid = false;
        }

        return isValid;
    }
}
