/** Class representing a player. */
class Player
{
  /**
   * Create a player.
   * @param {string} token - The player's token in the board. 
   * @param {string} name - The player's name.
   */
  constructor(token, name, ...args)
  {
    this.token = token;
    this.name = name;
  }
}

/** Class representing a grid in the playing field. */
class Slot
{
  /**
   * Create a slot.
   * @param {string} token - The default starting token in the slot.
   */
  constructor(token, ...args)
  {
    this._token = token;
  }

  /**
   * Returns the token in the slot, else a string with a single space.
   */
  get token()
  {
    return this._token || ' ';
  }

  /**
   * Updates the token in the slot.
   */
  set token(token)
  {
    this._token = token;
  }

  /**
   * Returns if the slot is empty. If the token only contains spaces, it counts as empty.
   */
  isEmpty()
  {
    return /^\s*$/g.test(this._token);
  }

  /**
   * Reset the token as a string with a single space.
   */
  reset()
  {
    this.token = ' ';
  }
}

/** Class representing the playing field for Tic-Tac-Toe. */
class Board
{
  /**
   * Create a board.
   * @param {number} height - The amount of grids per column.
   * @param {number} width - The amount of grids per row.
   */
  constructor(height, width, ...args)
  {
    let field = [];
    for(let i = 0; i < width; i++)
    {
      field.push([]);
      for(let j = 0; j < width; j++)
      {
        field[i].push(new Slot(' '));
      }
    }
    this.field = field;
    this.height = height;
    this.width = width;
  }

  /**
   * Checks if the board is already full of tokens. Returns a boolean value.
   */
  isFull()
  {
    let filled = 0;
    for(let i = 0 ; i < this.width; i++)
      for(let j = 0; j < this.height; j++)
        filled += this.field[i][j].isEmpty() ? 0 : 1;
    return filled === this.width * this.height;
  }

  /**
   * Draws the board in the console.
   * @param {boolean} useTokens
   * - Defaults as true. If false, print the board in guide version by representing each slot with
   * numbers corresponding to the input required to insert a token there. If true, print the board
   * as is with the tokens currently in it.
   */
  draw(useTokens=true)
  {
    let printed = [];
    let ctr = 1;
    for(let i = 0; i < this.height; i++)
    {
      for(let j = 0; j < this.width; j++)
      {
        let token = useTokens ? this.field[i][j].token : ctr.toString();
        ctr += useTokens ? 0 : 1;
        printed.push(j == 0 ? ` ${token} | ` : token);

        if(0 < j && j < this.width - 1)
          printed.push(' | ');
      }
      printed.push(i < this.height - 1 ? '\n-------------\n' : '\n');
    }
    return printed.join('');
  }

  /**
   * Writes player's move into the board.
   * @param {Player} player - The player making the move.
   * @param {string} move - The player's input as the move.
   */
  writeTurn(player, move)
  {
    let ctr = 1;
    for(let i = 0; i < this.width; i++)
    {
      for(let j = 0; j < this.height; j++)
      {
        if(parseInt(move) == ctr)
        {
          const empty = this.field[i][j].isEmpty();
          if(empty)
            this.field[i][j].token = player.token;
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

  /**
   * Checks if a win condition is already met. Returns a boolean value.
   */
  checkWin()
  {
    let win = false;
    const isConsistent = (item, i, arr) =>
    {
      return item.token == arr[0].token && !item.isEmpty();
    };

    for(let i = 0; i < this.width; i++)
    {
      const horizontalSlots = [];
      const verticalSlots = [];
      const diagonalTopToBottomSlots = [];
      const diagonalBottomToTopSlots = [];

      /* Check horizontal and vertical. */
      for(let j = 0; j < this.height; j++)
      {
        horizontalSlots.push(this.field[i][j]);
        verticalSlots.push(this.field[j][i]);
      }

      let h = this.height - 1;
      for(let j = 0; j < this.height; j++)
      {
        diagonalTopToBottomSlots.push(this.field[j][j]);
        diagonalBottomToTopSlots.push(this.field[h][j]);
        diagonalBottomToTopSlots.push(this.field[j][h]);
        h--;
      }

      win = [
        verticalSlots.every(isConsistent),
        horizontalSlots.every(isConsistent),
        diagonalTopToBottomSlots.every(isConsistent),
        diagonalBottomToTopSlots.every(isConsistent)
      ].some(i => i);

      if(win)
        break;
    }

    return win;
  }

  /**
   * Resets the board for another game.
   */
  reset()
  {
    for(let i = 0; i < this.width; i++)
      for(let j = 0; j < this.height; j++)
        this.field[i][j].reset()
  }
}


exports.Player = Player;
exports.Slot = Slot;
exports.Board = Board;
