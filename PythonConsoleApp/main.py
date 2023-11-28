import os
import random

import colorama
from colorama import Fore, Style
from dotenv import load_dotenv

colorama.init(autoreset=True)

# Global variables
board = []
game_finished = False
first_player_turn = True
board_size = 0
time_to_move_duration = 0
invalid_move_message_duration = 0
max_number_of_moves = 0
start_energy = 0
max_energy = 0
start_coins = 0
max_coins = 0
start_xp = 0
winning_xp = 0
player1_name = ""
player2_name = ""
refinement_facility_cost = 0
movement_cost = 0
unrefined_ore_weight = 0
refined_ore_weight = 0
unrefined_ore_cost = 0
refined_ore_cost = 0
mining_energy_loss = 0
player1_symbol = ""
player2_symbol = ""
player1_castle_symbol = ""
player2_castle_symbol = ""
empty_pillar_symbol = ""
cheep_ore_symbol = ""
expensive_ore_symbol = ""
refinement_facility_symbol = ""
number_of_cheap_ores = 0
number_of_expensive_ores = 0


def setup_env_variables():
    load_dotenv()
    global board_size, time_to_move_duration, invalid_move_message_duration, max_number_of_moves
    global start_energy, max_energy, start_coins, max_coins, start_xp, winning_xp
    global player1_name, player2_name, refinement_facility_cost, movement_cost
    global unrefined_ore_weight, refined_ore_weight, unrefined_ore_cost
    global refined_ore_cost, mining_energy_loss, player1_symbol, player2_symbol
    global player1_castle_symbol, player2_castle_symbol, empty_pillar_symbol, cheep_ore_symbol
    global expensive_ore_symbol, refinement_facility_symbol, number_of_cheap_ores, number_of_expensive_ores

    board_size = int(os.getenv("board_size"))
    time_to_move_duration = int(os.getenv("time_to_move_duration"))
    invalid_move_message_duration = int(os.getenv("invalid_move_message_duration"))
    max_number_of_moves = int(os.getenv("max_number_of_moves"))
    start_energy = int(os.getenv("start_energy"))
    max_energy = int(os.getenv("max_energy"))
    start_coins = int(os.getenv("start_coins"))
    max_coins = int(os.getenv("max_coins"))
    start_xp = int(os.getenv("start_xp"))
    winning_xp = int(os.getenv("winning_xp"))
    player1_name = os.getenv("player1_name")
    player2_name = os.getenv("player2_name")
    refinement_facility_cost = int(os.getenv("refinement_facility_cost"))
    movement_cost = int(os.getenv("movement_cost"))
    unrefined_ore_weight = int(os.getenv("unrefined_ore_weight"))
    refined_ore_weight = int(os.getenv("refined_ore_weight"))
    unrefined_ore_cost = int(os.getenv("unrefined_ore_cost"))
    refined_ore_cost = int(os.getenv("refined_ore_cost"))
    mining_energy_loss = int(os.getenv("mining_energy_loss"))
    player1_symbol = os.getenv("player1_symbol")
    player2_symbol = os.getenv("player2_symbol")
    player1_castle_symbol = os.getenv("player1_castle_symbol")
    player2_castle_symbol = os.getenv("player2_castle_symbol")
    empty_pillar_symbol = os.getenv("empty_pillar_symbol")
    cheep_ore_symbol = os.getenv("cheep_ore_symbol")
    expensive_ore_symbol = os.getenv("expensive_ore_symbol")
    refinement_facility_symbol = os.getenv("refinement_facility_symbol")
    number_of_cheap_ores = int(os.getenv("number_of_cheap_ores"))
    number_of_expensive_ores = int(os.getenv("number_of_expensive_ores"))


def generate_board():
    global board, board_size, player1_symbol, player2_symbol
    global empty_pillar_symbol, expensive_ore_symbol, cheep_ore_symbol, number_of_cheap_ores, number_of_expensive_ores
    board = [[empty_pillar_symbol for _ in range(board_size)] for _ in range(board_size)]
    board[0][board_size - 1] = player2_symbol
    board[board_size - 1][0] = player1_symbol
    for i in range(number_of_cheap_ores):
        x1, y1 = random.randint(0, board_size - 1), random.randint(0, board_size - 1)
        while (
                board[x1][y1] != empty_pillar_symbol
                or (x1 == 0 and y1 == board_size - 1)
                or (x1 == board_size - 1 and y1 == 0)
        ):
            x1, y1 = random.randint(0, board_size - 1), random.randint(0, board_size - 1)
        board[x1][y1] = cheep_ore_symbol
        board[y1][x1] = cheep_ore_symbol

    # Place expensive ore symbols randomly on each diagonal half (diagonally symmetrical)
    for i in range(number_of_expensive_ores):
        # Diagonal half 1
        x1, y1 = random.randint(0, board_size - 1), random.randint(0, board_size - 1)
        while (
                board[x1][y1] != empty_pillar_symbol
                or (x1 == 0 and y1 == board_size - 1)
                or (x1 == board_size - 1 and y1 == 0)
        ):
            x1, y1 = random.randint(0, board_size - 1), random.randint(0, board_size - 1)
        board[x1][y1] = expensive_ore_symbol
        board[y1][x1] = expensive_ore_symbol


def print_with_color(symbol):
    color_mapping = {
        player1_symbol: Fore.LIGHTBLUE_EX,
        player2_symbol: Fore.LIGHTRED_EX,
        player1_castle_symbol: Fore.BLUE,
        player2_castle_symbol: Fore.RED,
        empty_pillar_symbol: Fore.BLACK,
        cheep_ore_symbol: Fore.YELLOW,
        expensive_ore_symbol: Fore.LIGHTMAGENTA_EX,
        refinement_facility_symbol: Fore.LIGHTBLACK_EX,
    }

    return color_mapping.get(symbol, "") + symbol + Style.RESET_ALL


def print_board():
    global board
    for row in board:
        colored_row = [print_with_color(symbol) for symbol in row]
        print(" ".join(colored_row))


def print_player_stats():
    pass


def print_whole_game():
    print_player_stats()
    print_board()


def get_user_input():
    pass


def main():
    setup_env_variables()
    generate_board()
    print_board()


if __name__ == '__main__':
    main()
