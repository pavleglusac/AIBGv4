import random

from colorama import Fore, Style

from boardcell import BoardCell
from constants import Constants


class Board:
    def __init__(self):
        self.board_cells = [[BoardCell(Constants.empty_pillar_symbol) for _ in range(Constants.board_size)] for _ in
                            range(Constants.board_size)]
        self.place_players()
        self.setup_ores()

    def place_players(self):
        self.board_cells[0][Constants.board_size - 1].set_print_symbol(Constants.player1_symbol)
        self.board_cells[Constants.board_size - 1][0].set_print_symbol(Constants.player2_symbol)

    def setup_ores(self):
        self.place_ores(Constants.number_of_cheap_ores, Constants.cheep_ore_symbol)
        self.place_ores(Constants.number_of_expensive_ores, Constants.expensive_ore_symbol)

    def place_ores(self, number_of_ores, ore_symbol):
        for i in range(number_of_ores):
            x1, y1 = random.randint(0, Constants.board_size - 1), random.randint(0, Constants.board_size - 1)
            while (
                    self.board_cells[x1][y1].get_print_symbol() != Constants.empty_pillar_symbol
                    or (x1 == 0 and y1 == Constants.board_size - 1)
                    or (x1 == Constants.board_size - 1 and y1 == 0)
            ):
                x1, y1 = random.randint(0, Constants.board_size - 1), random.randint(0, Constants.board_size - 1)
            self.board_cells[x1][y1].set_print_symbol(ore_symbol)
            self.board_cells[y1][x1].set_print_symbol(ore_symbol)

    def update_board(self):
        pass

    def draw_board(self):
        max_cols = len(self.board_cells[0])
        print("  " + "".join([f"{i:3}" for i in range(len(self.board_cells))]))

        for col in range(max_cols):
            colored_col = [print_with_color(self.board_cells[row][col].get_print_symbol()) for row in
                           range(len(self.board_cells))]
            print(f"{col:3} {'  '.join(colored_col)}")


def print_with_color(symbol):
    color_mapping = {
        Constants.player1_symbol: Fore.LIGHTBLUE_EX,
        Constants.player2_symbol: Fore.LIGHTRED_EX,
        Constants.player1_castle_symbol: Fore.BLUE,
        Constants.player2_castle_symbol: Fore.RED,
        Constants.empty_pillar_symbol: Fore.LIGHTBLACK_EX,
        Constants.cheep_ore_symbol: Fore.YELLOW,
        Constants.expensive_ore_symbol: Fore.LIGHTMAGENTA_EX,
        Constants.refinement_facility_symbol: Fore.LIGHTBLACK_EX,
    }

    return color_mapping.get(symbol, "") + symbol + Style.RESET_ALL