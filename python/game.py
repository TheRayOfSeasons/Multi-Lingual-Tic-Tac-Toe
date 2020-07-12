from __future__ import absolute_import

from os import name
from os import system

from tictactoe.core import Player
from tictactoe.core import Slot
from tictactoe.core import Board


player_1 = Player('O', 'Player 1')
player_2 = Player('X', 'Player 2')


def clear():
    """
    Clear the screen.
    """
    clear_key = ['clear', 'cls'][name == 'nt']
    _ = system(clear_key)


def toggle_player(player):
    """
    Switch the current player instance.
    """
    return [player_1, player_2][player.name == player_1.name]


def main():
    """
    The main game logic.
    """
    game_board = Board(3, 3)
    current_player = player_1
    alert_message = ''
    win = False
    draw = False
    again = True

    while (not win) and (not draw) and again:
        clear()

        print('Guide\n')
        print(game_board.draw(use_tokens=False))
        print('\n\n')

        print('Playing Field\n')
        print(game_board.draw(use_tokens=True))
        print(alert_message)
        print(f'Turn of {current_player.name} [{current_player.token}]\n')

        move = input('Input: ')

        valid_move = game_board.write_turn(current_player, move)
        win = game_board.check_win()
        if valid_move:
            if not win:
                current_player = toggle_player(current_player)
            alert_message = ''
        else:
            alert_message = (
                f'Slot {str(move)} is either taken or invalid.\n\n')

        if (not win) and game_board.full:
            draw = True

        if win or draw:
            clear()

            print(game_board.draw(use_tokens=True))
            if win:
                print(f'{current_player.name} has won!\n\n')
            if draw:
                print('Draw!\n\n')

            play_again = input('Play again? (Y/N) : ')

            if any([play_again == answer for answer in ['Y', 'y']]):
                win = False
                again = True
                draw = False
                game_board.reset()
            else:
                win = False
                again = False
                draw = False


if __name__ == '__main__':
    main()
