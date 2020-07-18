using System;
using System.Collections.Generic;
using tictactoe;

namespace tictactoe
{
    /// <summary>
    /// The main game logic.
    /// </summary>
    class Game
    {

        static void Main()
        {
            Player player1 = new Player("O", "Player 1");
            Player player2 = new Player("X", "Player 2");
            Board gameBoard = new Board(3, 3);
            Player currentPlayer = player1;
            bool win = false;
            bool draw = false;
            bool again = true;
            bool validMove = false;
            string alertMessage = "";

            while(!win && !draw && again)
            {
                Console.Clear();
                Console.WriteLine("Guide\n");
                Console.WriteLine(gameBoard.Draw(false));
                Console.WriteLine("\n\n");

                Console.WriteLine("Playing Field\n");
                Console.WriteLine(gameBoard.Draw(true));
                Console.WriteLine(alertMessage);
                Console.WriteLine("Turn of " + currentPlayer + " [" + currentPlayer.Token + "]\n");

                Console.Write("Input: ");
                string move = Console.ReadLine();
                validMove = gameBoard.WriteTurn(currentPlayer, move);
                win = gameBoard.CheckWin();
                if(validMove)
                {
                    if(!win)
                        TogglePlayer(ref currentPlayer, player1, player2);
                    alertMessage = "";
                }
                else
                {
                    alertMessage = "Slot " + move + " is either taken or invalid.";
                }

                if(!win && gameBoard.IsFull())
                    draw = true;

                if(win || draw)
                {
                    Console.Clear();
                    Console.WriteLine(gameBoard.Draw(true));
                    Console.WriteLine(win ? currentPlayer.Name + " has won!" : "Draw!");
                    Console.WriteLine("\n\n");

                    Console.Write("Play Again? (Y/N): ");
                    string playAgain = Console.ReadLine();
                    if(playAgain == "Y" || playAgain == "y")
                    {
                        win = false;
                        again = true;
                        draw = false;
                        gameBoard.Reset();
                    }
                    else
                    {
                        win = false;
                        again = false;
                        draw = false;
                    }
                }
            }
        }

        /// <summary>
        /// Toggles the current player making the turn.
        /// </summary>
        /// <param name="currentPlayer">
        /// A ref variable that will store the reference to the current player.
        /// </param>
        /// <param name="player1">
        /// The player 1.
        /// </param>
        /// <param name="player2">
        /// The player 2.
        /// </param>
        static void TogglePlayer(ref Player currentPlayer, Player player1, Player player2)
        {
            currentPlayer = currentPlayer.Name == player2.Name ? player1 : player2;
        }
    }
}
