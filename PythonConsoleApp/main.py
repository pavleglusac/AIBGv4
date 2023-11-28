import os
import random
import re
import time

import colorama
from colorama import Fore, Style
from dotenv import load_dotenv

colorama.init(autoreset=True)

# Global variables
board = []
game_finished = False
first_player_turn = True
number_of_moves = 0

input_history = []
board_size = 0
resting_energy = 0
time_to_move_duration = 0
response_message_show_duration = 0
max_number_of_moves = 0
invalid_move_energy_penalty = 0
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
    global board_size, time_to_move_duration, response_message_show_duration, max_number_of_moves
    global start_energy, max_energy, start_coins, max_coins, start_xp, winning_xp
    global player1_name, player2_name, refinement_facility_cost, movement_cost, resting_energy
    global unrefined_ore_weight, refined_ore_weight, unrefined_ore_cost, invalid_move_energy_penalty
    global refined_ore_cost, mining_energy_loss, player1_symbol, player2_symbol
    global player1_castle_symbol, player2_castle_symbol, empty_pillar_symbol, cheep_ore_symbol
    global expensive_ore_symbol, refinement_facility_symbol, number_of_cheap_ores, number_of_expensive_ores

    board_size = int(os.getenv("board_size"))
    resting_energy = int(os.getenv("resting_energy"))
    time_to_move_duration = int(os.getenv("time_to_move_duration"))
    response_message_show_duration = int(os.getenv("response_message_show_duration"))
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
    invalid_move_energy_penalty = int(os.getenv("invalid_move_energy_penalty"))
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
    global board, board_size, player1_symbol, player2_symbol, player1, player2
    global empty_pillar_symbol, expensive_ore_symbol, cheep_ore_symbol, number_of_cheap_ores, number_of_expensive_ores
    board = [[empty_pillar_symbol for _ in range(board_size)] for _ in range(board_size)]
    board[player1.x][player1.y] = player1_symbol
    board[player2.x][player2.y] = player2_symbol
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

    for i in range(number_of_expensive_ores):
        x1, y1 = random.randint(0, board_size - 1), random.randint(0, board_size - 1)
        while (
                board[x1][y1] != empty_pillar_symbol
                or (x1 == 0 and y1 == board_size - 1)
                or (x1 == board_size - 1 and y1 == 0)
        ):
            x1, y1 = random.randint(0, board_size - 1), random.randint(0, board_size - 1)
        board[x1][y1] = expensive_ore_symbol
        board[y1][x1] = expensive_ore_symbol


def setup_game():
    setup_env_variables()
    generate_board()
    setup_players()


def setup_players():
    global player1, player2
    player1 = Player(player2_name, start_energy, start_xp, start_coins, board_size - 1, 0)
    player2 = Player(player1_name, start_energy, start_xp, start_coins, 0, board_size - 1)


def print_with_color(symbol):
    color_mapping = {
        player1_symbol: Fore.LIGHTBLUE_EX,
        player2_symbol: Fore.LIGHTRED_EX,
        player1_castle_symbol: Fore.BLUE,
        player2_castle_symbol: Fore.RED,
        empty_pillar_symbol: Fore.LIGHTBLACK_EX,
        cheep_ore_symbol: Fore.YELLOW,
        expensive_ore_symbol: Fore.LIGHTMAGENTA_EX,
        refinement_facility_symbol: Fore.LIGHTBLACK_EX,
    }

    return color_mapping.get(symbol, "") + symbol + Style.RESET_ALL


def print_board():
    global board
    print("  " + "".join([f"{i:3}" for i in range(len(board))]))
    for i, row in enumerate(board):
        colored_row = [print_with_color(symbol) for symbol in row]
        print(f"{i:3} {'  '.join(colored_row)}")


class Player:
    def __init__(self, name, energy, xp, coins, x, y):
        self.name = name
        self.energy = energy
        self.xp = xp
        self.coins = coins
        self.x = x
        self.y = y

    def change_energy(self, amount):
        global max_energy
        self.energy += amount
        if self.energy >= max_energy:
            self.energy = max_energy


player1 = Player(player2_name, start_energy, start_xp, start_coins, board_size - 1, 0)
player2 = Player(player1_name, start_energy, start_xp, start_coins, 0, board_size - 1)


def get_current_player():
    if first_player_turn:
        return player1
    return player2


def print_player_stats(player, color):
    print(
        f"{color}{player.name}{Style.RESET_ALL}\n"
        f"{Fore.GREEN}Energy: {player.energy}{Style.RESET_ALL} "
        f"{Fore.CYAN}XP: {player.xp}{Style.RESET_ALL} "
        f"{Fore.LIGHTYELLOW_EX}Coins: {player.coins}{Style.RESET_ALL} "
        f"{Fore.MAGENTA}Position: ({player.x},{player.y}){Style.RESET_ALL}"
    )


def print_both_players():
    global player1
    global player2
    print_player_stats(player1, Fore.BLUE)
    print_player_stats(player2, Fore.RED)


def print_stats_turns_and_board():
    print_both_players()
    print("\n")
    print("Turn number: ", number_of_moves)
    print("Now playing: ", get_current_player().name)
    print("\n")
    print_board()


def handle_resting():
    global resting_energy
    get_current_player().change_energy(resting_energy)
    print("Restin successful, energy increased for ", resting_energy, " points")


def update_board():
    board[board_size - 1][0] = player1_castle_symbol
    board[0][board_size - 1] = player2_castle_symbol
    for i in range(board_size):
        for j in range(board_size):
            if board[i][j] == player1_symbol or board[i][j] == player2_symbol:
                board[i][j] = empty_pillar_symbol

    board[player1.x][player1.y] = player1_symbol
    board[player2.x][player2.y] = player2_symbol


def handle_movement(user_input):
    global board, board_size

    match = re.match(r'^move (\d+) (\d+)$', user_input)
    if match:
        x, y = int(match.group(1)), int(match.group(2))
        current_player = get_current_player()

        if 0 <= x < board_size and 0 <= y < board_size:

            if (current_player.x == x) != (current_player.y == y):

                if not is_obstacle_on_path(current_player.x, current_player.y, x, y):
                    steps = (abs(current_player.x - x) + abs(current_player.y - y))
                    used_energy = steps  # When backpack is implemented
                    current_player.change_energy(-used_energy)
                    current_player.x = x
                    current_player.y = y
                    update_board()
                    print(f"Moving to position ({x}, {y}), took you ", used_energy, "energy points")
                else:
                    print("Error: There is an obstacle on the way")
                    invalid_move_handling()
            else:
                print("Error: Only moving horizontally or vertically is allowed")
                invalid_move_handling()
        else:
            print("Error: Move out of bounds!")
            invalid_move_handling()


def is_obstacle_on_path(start_x, start_y, end_x, end_y):
    for i in range(min(start_x, end_x), max(start_x, end_x) + 1):
        if i != start_x:
            if board[i][start_y] != empty_pillar_symbol:
                return True
    for j in range(min(start_y, end_y), max(start_y, end_y) + 1):
        if j != start_y:
            if board[start_x][j] != empty_pillar_symbol:
                return True
    return False


def get_user_input():
    print("\n")
    user_input = input("Your input:\n")
    global input_history

    input_history.append(user_input)  ## Za replay kasnije

    if re.match(r'^move \d+ \d+$', user_input):
        handle_movement(user_input)

    elif re.match(r'^rest', user_input):
        handle_resting()

    elif re.match(r'^shop (freeze|backpack|daze)$', user_input):
        print()

    else:
        invalid_move_handling()


def invalid_move_handling():
    print("Invalid move, penalty to energy: -", invalid_move_energy_penalty)
    get_current_player().energy = get_current_player().energy - invalid_move_energy_penalty


def check_if_game_is_finished():
    global game_finished, player1, player2

    if player1.energy <= 0:
        game_finished = True
        if player2.energy <= 0:
            return "TIE"
        return player2.name

    if player2.energy <= 0:
        game_finished = True
        if player1.energy <= 0:
            return "TIE"
        return player1.name

    if player1.xp >= winning_xp:
        game_finished = True
        if player1.xp <= player2.xp:
            return player2.name
        return player1.name

    if player2.xp >= winning_xp:
        game_finished = True
        if player2.xp <= player1.xp:
            return player1.name
        return player2.name

    if number_of_moves == max_number_of_moves:
        game_finished = True
        if player1.xp == player1.xp:
            return "TIE"
        if player1.xp > player2.xp:
            return player1.name
        if player1.xp > player2.xp:
            return player2.name

    return ""


def turn():
    global first_player_turn
    print_stats_turns_and_board()
    get_user_input()
    time.sleep(response_message_show_duration)
    os.system('cls' if os.name == 'nt' else 'clear')
    first_player_turn = not first_player_turn


def main():
    os.system('cls' if os.name == 'nt' else 'clear')
    global number_of_moves
    setup_game()
    winner = ""
    while not game_finished:
        turn()  ## player 1
        turn()  ## Player 2
        number_of_moves = number_of_moves + 1
        winner = check_if_game_is_finished()

    os.system('cls' if os.name == 'nt' else 'clear')
    print("Winner is :", winner)


if __name__ == '__main__':
    main()
