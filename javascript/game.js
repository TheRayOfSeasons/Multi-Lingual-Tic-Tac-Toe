const { Player, Board } = require('./core/tictactoe');
const readline = require('readline');

const player_1 = new Player('O', 'Player 1');
const player_2 = new Player('X', 'Player 2');
const gameBoard = new Board(3, 3);
let currentPlayer = player_1;
let win = false;
let draw = false;
let again = true;
let validMove = false;
let alertMessage = '';

/**
 * Toggles player 1 and 2.
 * @param {Player} player - The current player.
 */
const togglePlayer = player => player.name == player_2.name ? player_1 : player_2;

/**
 * Returns a promise used for accepting inputs.
 * @param {string} text - The prompt message.
 * @param {Function} action
 * - The callback function used for processing the resolving value as an answer.
 * @param {boolean} returnInput
 * - Defaults as false. If true, the input is resolved back with the resolving value as an object.
 */
const input = (text, action, returnInput = false) => new Promise(resolve =>
{
  const interface = readline.createInterface({
    input: process.stdin,
    output: process.stdout
  });
  interface.question(text, _input =>
  {
    const answer = action(_input);
    interface.close();
    resolve(returnInput ? {answer, _input} : answer);
  });
});

/** The main game logic. */
async function main()
{
  while(!win && !draw && again)
  {
    console.clear();
    console.log('Guide\n');
    console.log(gameBoard.draw(false));
    console.log('\n\n');

    console.log('Playing Field\n');
    console.log(gameBoard.draw(true));
    console.log(alertMessage);
    console.log(`Turn of ${currentPlayer.name} [${currentPlayer.token}]\n`);

    const {answer: valid, _input: move} = await input(
      'Input: ',
      input => gameBoard.writeTurn(currentPlayer, input),
      true
    );
    validMove = valid;
    win = gameBoard.checkWin();

    if(validMove)
    {
      if(!win)
      {
        currentPlayer = togglePlayer(currentPlayer);
      }
      alertMessage = '';
    }
    else
    {
      alertMessage = `Slot ${move} is either taken or invalid.\n\n`;
    }

    if(!win && gameBoard.isFull())
    {
      draw = true;
    }

    if(win || draw)
    {
      console.clear();
      console.log(gameBoard.draw(true));
      console.log(win ? `${currentPlayer.name} has won!` : 'Draw!');
      console.log('\n\n');

      await input('Play again? (Y/N): ', input => {
        if(['Y', 'y'].includes(input))
        {
          win = false;
          again = true;
          draw = false;
          gameBoard.reset();
        }
        else
        {
          win = false;
          again = false;
          draw = false;
        }
      });
    }
  }
}

/* Export the main function for potential import. */
module.exports = main;

main();
