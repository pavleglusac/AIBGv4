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
        self.base_area_length = Constants.board_size // 3
        self.place_players()
        self.setup_ores()  ## TODO sa bojanom uradi

    def place_players(self):
        self.board_cells[0][Constants.board_size - 1].print_symbol = Constants.player1_symbol
        self.board_cells[Constants.board_size - 1][0].print_symbol = Constants.player2_symbol

    def setup_ores(self):
        self.place_crystals(Constants.cheep_crystal_symbol)
        self.place_crystals(Constants.expensive_crystal_symbol)

    def place_crystals(self, ore_symbol):
        is_expensive = ore_symbol == Constants.expensive_crystal_symbol
        number_of_groups = Constants.number_of_expensive_crystal_groups \
            if is_expensive else Constants.number_of_cheap_crystal_groups
        number_of_crystals_in_group = Constants.number_of_expensive_crystals_in_group \
            if is_expensive else Constants.number_of_cheap_crystals_in_group
        group_coordinates = []
        crystals_coordinates = []
        up = True
        for _ in range(number_of_groups):
            can_not_be_center = True
            coordinates = None
            try_number = 0
            can_not_find = False
            while can_not_be_center:
                try_number += 1
                if try_number > 225:
                    if number_of_crystals_in_group > 0:
                        number_of_crystals_in_group -= 1
                        try_number = 0
                    else:
                        can_not_find = True
                        break
                coordinates = self.generate_coordinates(up)
                can_not_be_center = self.check_center_coordinates(coordinates, number_of_crystals_in_group)

            if can_not_find:
                break
            x, y = coordinates
            up = not up
            group_coordinates.append((x, y))
            for existing_group in group_coordinates[:-1]:
                if (x, y) == existing_group or (y, x) == existing_group:
                    group_coordinates.pop()
                    break

            generated_crystal_group = self.generate_crystal_group(x, y, crystals_coordinates, is_expensive,
                                                                  number_of_crystals_in_group)
            crystals_coordinates.extend(generated_crystal_group)
            for x, y in generated_crystal_group:
                if (
                        self.check_if_coordinates_are_valid(x, y)
                        and self.board_cells[x][y].print_symbol == Constants.empty_pillar_symbol
                        and self.board_cells[y][x].print_symbol == Constants.empty_pillar_symbol
                ):
                    self.board_cells[x][y].print_symbol = ore_symbol
                    self.board_cells[y][x].print_symbol = ore_symbol

    def generate_crystal_group(self, x, y, existing_crystals, is_expensive, number_of_crystals_in_group):
        new_crystals_coordinates = []

        i = -1
        while i < number_of_crystals_in_group - 1:

            i += 1
            x_coordinate, y_coordinate = x, y
            directions = [0, 1, 2, 3]

            if i != 0:
                direction = random.choice(directions)
                directions.remove(direction)
                if direction == 0:
                    x_coordinate += 1
                elif direction == 1:
                    x_coordinate -= 1
                elif direction == 2:
                    y_coordinate += 1
                elif direction == 3:
                    y_coordinate -= 1

            if (
                    not self.check_if_coordinates_are_valid(x_coordinate, y_coordinate)
                    or (x_coordinate, y_coordinate) in existing_crystals
                    or (x_coordinate, y_coordinate) in new_crystals_coordinates
            ):
                i -= 1
                continue
            if self.board_cells[x_coordinate][y_coordinate].print_symbol == Constants.empty_pillar_symbol:
                new_crystals_coordinates.append((x_coordinate, y_coordinate))
            else:
                i -= 1
        return new_crystals_coordinates

    def check_center_coordinates(self, coordinates, group_size) -> bool:
        # print(coordinates)
        if self.board_cells[coordinates[0]][coordinates[1]].print_symbol != Constants.empty_pillar_symbol:
            return True

        count = 1
        for direction in [(0, 1), (0, -1), (-1, 0), (1, 0)]:
            new_x = coordinates[0] + direction[0]
            new_y = coordinates[1] + direction[1]
            if not self.check_if_coordinates_are_valid(new_x, new_y):
                continue
            if self.board_cells[new_x][new_y].print_symbol == Constants.empty_pillar_symbol:
                count += 1
        return count < group_size

    def generate_coordinates(self, up):
        ## TODO dobavljaj samo random SLOBODNE pozicije
        x, y = 0, 0
        if up:
            while not (x > y):
                x = random.randint(1, Constants.board_size - self.base_area_length - 2)
                y = random.randint(1, self.base_area_length)
        else:
            while not (x > y):
                x = random.randint(Constants.board_size - self.base_area_length, Constants.board_size - 1)
                y = random.randint(self.base_area_length + 1, Constants.board_size - 2)

        return x, y

    def check_if_coordinates_are_valid(self, x, y):
        if Constants.board_size - self.base_area_length <= x <= Constants.board_size - 1 and 0 <= y <= self.base_area_length - 1:
            return False
        if x < 0 or x >= Constants.board_size or y < 0 or y >= Constants.board_size or x == y:
            return False
        return True

    def update_board(self, player1, player2):
        self.board_cells[0][Constants.board_size - 1].print_symbol = Constants.player1_castle_symbol
        self.board_cells[Constants.board_size - 1][0].print_symbol = Constants.player2_castle_symbol
        for i in range(Constants.board_size):
            for j in range(Constants.board_size):
                self.board_cells[i][j].replenish()
                if (self.board_cells[i][j].print_symbol == Constants.player1_symbol
                        or self.board_cells[i][j].print_symbol
                        == Constants.player2_symbol):
                    self.board_cells[i][j].print_symbol = Constants.empty_pillar_symbol

        self.board_cells[player1.x][player1.y].print_symbol = Constants.player1_symbol
        self.board_cells[player2.x][player2.y].print_symbol = Constants.player2_symbol

    def draw_board(self):
        print("  " + "".join([f"{i:3}" for i in range(len(self.board_cells))]))
        for col in range(len(self.board_cells[0])):
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