import copy
import re


class Player:
    """
    A class to represent the player.

    - Constructor Parameters

        :param token: :type str:
            - The player's token in the board.
        :param name: :type str:
            - The player's name.
    """

    def __init__(self, token, name, *args, **kwargs):
        self.token = token
        self.name = name


class Slot:
    """
    A slot will contain a space from the field.

    - Constructor Parameters

        :param token: :type str:
            - The initial token for the slot.
    """

    def __init__(self, token, *args, **kwargs):
        self.token = token

    def get_token(self):
        """
        Returns the token if available, else, a single space.
        """
        return self.token or ' '

    def update_token(self, token):
        """
        Updates the token in the slot if the slot is empty. Returns a boolean
        value stating if the update was a success or not.

        - Parameters:

            :param token: :type str:
                - The new token.
        """
        empty = self.is_empty()
        if empty:
            self.token = token
            return empty
        return empty

    def is_empty(self):
        """
        Returns False if the token is empty or only contains spaces.
        """
        return [False, True][re.search('^\s*$', self.token) is not None]

    def reset(self):
        """
        Resets the slot as empty.
        """
        self.token = ' '


class Board:
    """
    The playing field.

    - Constructor Parameters

        :param height: :type int:
            - The board's height.
        :param width: :type int:
            - The board's width.
    """

    def __init__(self, height, width, *args, **kwargs):
        field = []
        for i in range(width):
            field.append([])
            for j in range(height):
                field[i].append(Slot(' '))
        self.field = field
        self.height = height
        self.width = width

    @property
    def full(self):
        """
        Returns if the board's slots are full.
        """
        filled = 0
        for i in range(self.width):
            for j in range(self.height):
                if not self.field[i][j].is_empty():
                    filled += 1
        return filled == self.width * self.height

    def draw(self, use_tokens=True):
        """
        Prints the playing field in the command line.

        - Parameters

            :param use_tokens: :type bool:
                - Defaults as True. If set as false,
                  the board is drawn in guide mode.
        """
        printed = ''
        ctr = 1
        for i in range(self.height):
            for j in range(self.width):
                if use_tokens:
                    token = self.field[i][j].token
                else:
                    token = str(ctr)
                    ctr += 1

                if j == 0:
                    printed += f' {token} | '
                else:
                    printed += token

                if 0 < j < self.width - 1:
                    printed += ' | '

            printed += '\n'
            if i < self.height - 1:
                printed += "-------------"
                printed += "\n"
        return printed

    def write_turn(self, player, move):
        """
        Writes a players turn into the board then returns a boolean value
        representing if the move is valid.

        - Parameters

            :param player: :type Player:
                The instance of the player making the turn.
            :param input: :type str:
                The player's move.
        """
        ctr = 1
        for i in range(self.width):
            for j in range(self.height):
                if int(move) == ctr:
                    return self.field[i][j].update_token(player.token)
                else:
                    ctr += 1
        return False

    def equal_all_slots(self, slots):
        """
        Checks if all tokens from a collection of slots are equal.
        Returns True if they are.

        - Parameters

            :param slots: :type list:
                - The collection of slots.
        """
        for slot in slots:
            if slot.is_empty():
                return False
        return len(set([slot.token for slot in slots])) == 1

    def check_win(self):
        """
        Returns a boolean value representing the
        if a win condition is already met.
        """
        win = False
        for i in range(self.width):
            horizontal_slots = []
            vertical_slots = []
            diagonal_top_to_bottom_slots = []
            diagonal_bottom_to_top_slots = []
            # Check horizontal and vertical.
            for j in range(self.height):
                h_slot = self.field[i][j]
                v_slot = self.field[j][i]
                horizontal_slots.append(h_slot)
                vertical_slots.append(v_slot)

            for j in range(self.height):
                d_slot = self.field[j][j]
                diagonal_top_to_bottom_slots.append(d_slot)

            _height = copy.deepcopy(self.height - 1)
            for j in range(self.height):
                d_slot_1 = self.field[_height][j]
                d_slot_2 = self.field[j][_height]
                diagonal_bottom_to_top_slots.extend([d_slot_1, d_slot_2])
                _height -= 1

            h_win = self.equal_all_slots(horizontal_slots)
            v_win = self.equal_all_slots(vertical_slots)
            d1_win = self.equal_all_slots(diagonal_top_to_bottom_slots)
            d2_win = self.equal_all_slots(diagonal_bottom_to_top_slots)
            if h_win or v_win or d1_win or d2_win:
                win = True
                break

        return win

    def reset(self):
        """
        Resets the fields of the board.
        """
        for i in range(self.width):
            for j in range(self.height):
                self.field[i][j].reset()
