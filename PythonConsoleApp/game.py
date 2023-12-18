from colorama import Fore

from board import Board
from constants import Constants
from player import Player


def print_shop_info():
    print("Daze cost (Duration:", Constants.number_of_daze_turns, "turns):", Constants.daze_cost)
    print("Freeze cost (Duration:", Constants.number_of_frozen_turns, "turns):", Constants.freeze_cost)
    print("Backpack increase cost (Duration:", Constants.number_of_bigger_backpack_turns,
          "turns,", Constants.increase_in_backpack_storage_capacity, "added to capacity):",
          Constants.bigger_backpack_cost)
    print()


class Game:
    def __init__(self):
        self.player1 = Player(Constants.player1_name, 0, Constants.board_size - 1)
        self.player2 = Player(Constants.player2_name, Constants.board_size - 1, 0)
        self.board = Board()
        self.game_finished = False
        self.first_player_turn = True
        self.number_of_turns = 0
        self.input_history = []

    def start(self):
        self.print_stats_shop_turns_and_board()

    def print_stats_shop_turns_and_board(self):
        self.print_both_players()
        print_shop_info()
        self.board.draw_board()
        self.print_turn_status()

    def print_both_players(self):
        self.player1.print_player_stats(Fore.BLUE)
        self.player2.print_player_stats(Fore.RED)

    def print_turn_status(self):
        print()
        print("Turn number: ", self.number_of_turns)
        if self.first_player_turn:
            color = Fore.BLUE
        else:
            color = Fore.RED
        print(color + "Now playing: ", color + self.get_current_player().name)

    def get_current_player(self):
        if self.first_player_turn:
            return self.player1
        return self.player2