using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using tictactoe;

namespace tictactoe
{
    /// <summary>
    /// A class representing a player.
    /// </summary>
    public class Player
    {
        private string token;
        private string name;

        /// <summary>
        /// Create a player.
        /// </summary>
        public Player(string token, string name)
        {
            this.token = token;
            this.name = name;
        }

        /// <summary>
        /// The readonly property for the player's name.
        /// </summary>
        public string Name
        {
            get { return name; }
        }

        /// <summary>
        /// The readonly property for the player's token.
        /// </summary>
        public string Token
        {
            get { return token; }
        }
    }

    /// <summary>
    /// A class representing a grid in the playing field.
    /// </summary>
    public class Slot
    {
        private string token;
        private Regex emptyRegex;

        /// <summary>
        /// Create a slot.
        /// </summary>
        public Slot(string token)
        {
            this.token = token;
            emptyRegex = new Regex(@"^\s*$");
        }

        /// <summary>
        /// The token in the grid.
        /// </summary>
        public string Token
        {
            get { return token; }
            set { this.token = value; }
        }

        /// <summary>
        /// Checks if the slot contains a token.
        /// </summary>
        /// <returns>
        /// Returns true if the slot is empty.
        /// If the token only contains spaces,
        /// it counts as empty.
        /// </returns>
        public bool IsEmpty()
        {
            return emptyRegex.Matches(token).Count > 0;
        }

        /// <summary>
        /// Reset the token as a string with a single space.
        /// </summary>
        public void Reset()
        {
            token = " ";
        }
    }


    /// <summary>
    /// A class representing the playing field for Tic-Tac-Toe.
    /// </summary>
    public class Board
    {
        private Slot[,] field;
        private const string initialToken = " ";
        private int height;
        private int width;


        /// <summary>
        /// Create a board.
        /// </summary>
        public Board(int height, int width)
        {
            field = new Slot[height, width];
            for(int i = 0; i < height; i++)
            {
                for(int j = 0; j < width; j++)
                {
                    field[i, j] = new Slot(initialToken);
                }
            }
            this.height = height;
            this.width = width;
        }

        /// <summary>
        /// Checks if the board is already full of tokens. Returns a boolean value.
        /// </summary>
        /// <returns>
        /// Returns true if all slots in the board are not empty.
        /// </returns>
        public bool IsFull()
        {
            int filled = 0;
            for(int i = 0; i < height; i++)
                for(int j = 0; j < width; j++)
                    filled += field[i, j].IsEmpty() ? 0 : 1;
            return filled == width * height;
        }

        /// <summary>
        /// Draws the board in the console.
        /// </summary>
        /// <param name="useTokens">
        /// If false, print the board in guide version by representing each slot
        /// with numbers corresponding to the input required to insert a token there.
        /// If true, print the board as is with the tokens currently in it.
        /// </param>
        /// <returns>
        /// Returns a string representing the current status of board. If usetokens
        /// us false, then the board will be printed in guide mode.
        /// </returns>
        public string Draw(bool useTokens)
        {
            List<string> printed = new List<string>();
            int ctr = 1;
            for(int i = 0; i < height; i++)
            {
                for(int j = 0; j < width; j++)
                {
                    string token = useTokens ? field[i, j].Token : ctr.ToString();
                    ctr += useTokens ? 0 : 1;
                    printed.Add(j == 0 ? " " + token + " | " : token);
                    if(0 < j && j < width - 1)
                        printed.Add(" | ");
                }
                printed.Add(i < height - 1 ? "\n-------------\n" : "\n");
            }
            return string.Join("", printed);
        }

        /// <summary>
        /// Writes player's move into the board.
        /// </summary>
        /// <param name="player">
        /// The player making the move.
        /// </param>
        /// <param name="move">
        /// The value of the player's input as the move.
        /// </param>
        /// <returns>
        /// Returns true if the turn is written successfully.
        /// </returns>
        public bool WriteTurn(Player player, string move)
        {
            int ctr = 1;
            for(int i = 0; i < width; i++)
            {
                for(int j = 0; j < height; j++)
                {
                    int moveNumber;
                    try
                    {
                        moveNumber = Int32.Parse(move);
                    }
                    catch(FormatException)
                    {
                        return false;
                    }
                    if(moveNumber == ctr)
                    {
                        bool empty = field[i, j].IsEmpty();
                        if(empty)
                            field[i, j].Token = player.Token;
                        return empty;
                    }
                    else
                    {
                        ctr++;
                    }
                }
            }
            return false;
        }


        /// <summary>
        /// Checks if all values from a collection of slots are equal.
        /// </summary>
        /// <param name="collection">
        /// A List of slots.
        /// </param>
        /// <returns>
        /// Returns true if all are equal and not empty.
        /// </returns>
        private bool CheckAllEqual(List<Slot> collection)
        {
            foreach(Slot current in collection)
                if(!(current.Token == collection[0].Token && !current.IsEmpty()))
                    return false;
            return true;
        }


        /// <summary>
        /// Checks if a win condition is already met.
        /// </summary>
        /// <returns>
        /// Returns true if a win condition is met.
        /// </returns>
        public bool CheckWin()
        {
            bool win = false;
            for(int i = 0; i < width; i++)
            {
                List<Slot> horizontalSlots = new List<Slot>();
                List<Slot> verticalSlots = new List<Slot>();
                List<Slot> diagonalTopToBottomSlots = new List<Slot>();
                List<Slot> diagonalBottomToTopSlots = new List<Slot>();

                /* Check horizontal and vertical. */
                for(int j = 0; j < height; j++)
                {
                    horizontalSlots.Add(field[i, j]);
                    verticalSlots.Add(field[j, i]);
                }

                int h = height - 1;
                /* Check diagonals. */
                for(int j = 0; j < height; j++)
                {
                    diagonalTopToBottomSlots.Add(field[j, j]);
                    diagonalBottomToTopSlots.Add(field[h, j]);
                    diagonalBottomToTopSlots.Add(field[j, h]);
                    h--;
                }

                win = (
                    CheckAllEqual(verticalSlots) ||
                    CheckAllEqual(horizontalSlots) ||
                    CheckAllEqual(diagonalTopToBottomSlots) ||
                    CheckAllEqual(diagonalBottomToTopSlots)
                );
                if(win)
                    break;
            }
            return win;
        }

        /// <summary>
        /// Resets the entire board for another game.
        /// </summary>
        public void Reset()
        {
            for(int i = 0; i < this.width; i++)
                for(int j = 0; j < this.height; j++)
                    field[i, j].Reset();
        }
    }
}
