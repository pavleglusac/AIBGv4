import random

from colorama import Fore, Style

from boardcell import BoardCell
from constants import Constants


def are_neighbours(x1, y1, x2, y2) -> bool:
    return (abs(x2 - x1) == 1 and y2 == y1) or (x2 == x1 and abs(y2 - y1) == 1)


class Board:
    def __init__(self):
        self.board_cells = [[BoardCell(Constants.empty_pillar_symbol) for _ in range(Constants.board_size)] for _ in
                            range(Constants.board_size)]
        self.place_players()
        self.setup_ores()  ## TODO sa bojanom uradi

    def place_players(self):
        self.board_cells[0][Constants.board_size - 1].print_symbol = Constants.player1_symbol
        self.board_cells[Constants.board_size - 1][0].print_symbol = Constants.player2_symbol

    def setup_ores(self):
        self.place_ores(Constants.number_of_cheap_crystals, Constants.cheep_crystal_symbol)
        self.place_ores(Constants.number_of_expensive_crystals, Constants.expensive_crystal_symbol)

    def place_ores(self, number_of_ores, ore_symbol):
        for i in range(number_of_ores):
            x1, y1 = random.randint(0, Constants.board_size - 1), random.randint(0, Constants.board_size - 1)
            while (
                    self.board_cells[x1][y1].print_symbol != Constants.empty_pillar_symbol
                    or (x1 == 0 and y1 == Constants.board_size - 1)
                    or (x1 == Constants.board_size - 1 and y1 == 0)
            ):
                x1, y1 = random.randint(0, Constants.board_size - 1), random.randint(0, Constants.board_size - 1)
            self.board_cells[x1][y1].print_symbol = ore_symbol
            self.board_cells[y1][x1].print_symbol = ore_symbol

    def update_board(self, player1, player2):
        self.board_cells[0][Constants.board_size - 1].print_symbol = Constants.player1_castle_symbol
        self.board_cells[Constants.board_size - 1][0].print_symbol = Constants.player2_castle_symbol
        for i in range(Constants.board_size):
            for j in range(Constants.board_size):
                if (self.board_cells[i][j].print_symbol == Constants.player1_symbol
                        or self.board_cells[i][j].print_symbol
                        == Constants.player2_symbol):
                    self.board_cells[i][j].print_symbol = Constants.empty_pillar_symbol

        self.board_cells[player1.x][player1.y].print_symbol = Constants.player1_symbol
        self.board_cells[player2.x][player2.y].print_symbol = Constants.player2_symbol

    def draw_board(self):
        max_cols = len(self.board_cells[0])
        print("  " + "".join([f"{i:3}" for i in range(len(self.board_cells))]))

        for col in range(max_cols):
            colored_col = [print_with_color(self.board_cells[row][col].print_symbol) for row in
                           range(len(self.board_cells))]
            print(f"{col:3} {'  '.join(colored_col)} {col:3} ")

        print("  " + "".join([f"{i:3}" for i in range(len(self.board_cells))]))

    def is_obstacle_on_path(self, start_x, start_y, end_x, end_y):
        for i in range(min(start_x, end_x), max(start_x, end_x) + 1):
            if i != start_x:
                if self.board_cells[i][start_y].print_symbol != Constants.empty_pillar_symbol:
                    return True
        for j in range(min(start_y, end_y), max(start_y, end_y) + 1):
            if j != start_y:
                if self.board_cells[start_x][j].print_symbol != Constants.empty_pillar_symbol:
                    return True
        return False


def print_with_color(symbol):
    color_mapping = {
        Constants.player1_symbol: Fore.LIGHTBLUE_EX,
        Constants.player2_symbol: Fore.LIGHTRED_EX,
        Constants.player1_castle_symbol: Fore.BLUE,
        Constants.player2_castle_symbol: Fore.RED,
        Constants.empty_pillar_symbol: Fore.LIGHTBLACK_EX,
        Constants.cheep_crystal_symbol: Fore.YELLOW,
        Constants.expensive_crystal_symbol: Fore.LIGHTMAGENTA_EX,
        Constants.refinement_facility_symbol: Fore.LIGHTBLACK_EX,
    }

    return color_mapping.get(symbol, "") + symbol + Style.RESET_ALL