import os
import re
import time

from colorama import Fore
from inputimeout import inputimeout

from board import Board, are_neighbours
from constants import Constants
from crystal import Crystal
from player import Player


def print_shop_info():
    print("Daze cost (Duration:", Constants.number_of_daze_turns, "turns):", Constants.daze_cost)
    print("Freeze cost (Duration:", Constants.number_of_frozen_turns, "turns):", Constants.freeze_cost)
    print("Backpack increase cost (Duration:", Constants.number_of_bigger_backpack_turns,
          "turns,", Constants.increase_in_backpack_storage_capacity, "added to capacity):",
          Constants.bigger_backpack_cost)
    print()


def clear_screen():
    os.system('cls' if os.name == 'nt' else 'clear')


def pause_for_response_message():
    time.sleep(Constants.response_message_show_duration)


class Game:
    def __init__(self):
        self.player1 = Player(Constants.player1_name, 0, Constants.board_size - 1)
        self.player2 = Player(Constants.player2_name, Constants.board_size - 1, 0)
        self.board = Board()
        self.game_finished = False
        self.first_player_turn = True
        self.number_of_turns = 1
        self.input_history = []

    def play_game(self):
        clear_screen()
        winner = ""
        while winner == "":
            self.board.update_board(self.player1, self.player2)
            self.turn()
            self.number_of_turns += 1
            winner = self.check_if_game_is_finished()
        print(f"{Fore.GREEN}The winner is: {winner} ")
        _ = input("Game ended!\nPress enter to exit")
        clear_screen()

    def print_stats_shop_turns_and_board(self):
        self.print_both_players()
        print_shop_info()
        self.board.draw_board()
        self.print_turn_status()

    def print_both_players(self):
        self.player1.print_stats(Fore.BLUE)
        self.player2.print_stats(Fore.RED)

    def print_turn_status(self):
        print()
        print("Turn number: ", self.number_of_turns, "/", Constants.max_number_of_turns)
        if self.first_player_turn:
            color = Fore.BLUE
        else:
            color = Fore.RED
        print(color + "Now playing: ", color + self.get_current_player().name)

    def get_current_player(self):
        if self.first_player_turn:
            return self.player1
        return self.player2

    def get_alternate_player(self):
        if self.first_player_turn:
            return self.player2
        return self.player1

    def turn(self):
        if self.get_current_player().is_frozen():
            self.switch_current_player()
        self.decrease_player_statuses()
        self.print_stats_shop_turns_and_board()
        self.get_user_input()
        pause_for_response_message()
        clear_screen()
        self.switch_current_player()

    def switch_current_player(self):
        self.first_player_turn = not self.first_player_turn

    def decrease_player_statuses(self):
        self.get_alternate_player().decrease_daze_turns()
        self.get_alternate_player().decrease_frozen_turns()
        self.get_current_player().decrease_increased_backpack_storage_turns()

    def check_if_game_is_finished(self):

        if self.player1.energy <= 0:
            return self.player2.name

        if self.player2.energy <= 0:
            return self.player1.name

        if self.player1.xp >= Constants.winning_xp:
            return self.player1.name

        if self.player2.xp >= Constants.winning_xp:
            return self.player2.name

        if self.number_of_turns > Constants.max_number_of_turns:
            if self.player1.xp == self.player2.xp:
                return "TIE!"
            if self.player1.xp > self.player2.xp:
                return self.player1.name
            if self.player1.xp < self.player2.xp:
                return self.player2.name

        return ""

    def timeout_handling(self):
        print("Sorry, time is up, you have ", Constants.time_to_do_action_in_turn_duration, "seconds to do an action!")
        self.invalid_turn_handling()
        return

    def get_user_input(self):
        try:
            user_input = inputimeout(prompt='Your input: ', timeout=Constants.time_to_do_action_in_turn_duration)
            user_input = user_input.strip()
            self.input_history.append(user_input)  # For replay later

            if re.match(r'^move (-?\d+) (-?\d+)$', user_input):
                self.handle_movement(user_input)

            elif re.match(r'^mine (-?\d+) (-?\d+)$', user_input):
                self.handle_mine(user_input)

            elif re.match(r'^rest', user_input):
                self.handle_resting()

            elif re.match(r'^shop (freeze|backpack|daze)$', user_input):
                choice = re.match(r'^shop (freeze|backpack|daze)$', user_input).group(1)
                if choice == 'freeze':
                    self.handle_freeze()
                elif choice == 'backpack':
                    self.handle_backpack()
                elif choice == 'daze':
                    self.handle_daze()
            else:
                print('Invalid input,command not recognised!')
                self.invalid_turn_handling()
        except (Exception,):
            self.timeout_handling()

    def invalid_turn_handling(self):
        print("Invalid turn, penalty to energy: -", Constants.invalid_turn_energy_penalty, "!")
        self.get_current_player().decrease_energy(Constants.invalid_turn_energy_penalty)

    def handle_resting(self):
        self.get_current_player().increase_energy(Constants.resting_energy)
        print("Restin successful, energy increased for ", Constants.resting_energy, " points!")

    def handle_freeze(self):
        if self.get_current_player().coins < Constants.freeze_cost:
            print("You do not have enough coins to buy this power!")
            self.invalid_turn_handling()
            return
        self.get_alternate_player().add_frozen_turns()
        self.get_current_player().remove_coins(Constants.freeze_cost)
        print("Freeze purchased successfully!")

    def handle_backpack(self):
        if self.get_current_player().coins < Constants.bigger_backpack_cost:
            print("You do not have enough coins to buy this power!")
            self.invalid_turn_handling()
            return
        self.get_current_player().add_increased_backpack_storage_turns()
        self.get_current_player().add_coins(Constants.bigger_backpack_cost)
        print("Increased backpack storage purchased successfully!")

    def handle_daze(self):
        if self.get_current_player().coins < Constants.daze_cost:
            print("You do not have enough coins to buy this power!")
            self.invalid_turn_handling()
            return
        self.get_alternate_player().add_daze_turns()
        self.get_current_player().remove_coins(Constants.daze_cost)
        print("Daze purchased successfully!")

    def handle_movement(self, user_input):
        match = re.match(r'^move (-?\d+) (-?\d+)$', user_input)
        if not match:
            return
        x, y = int(match.group(1)), int(match.group(2))
        current_player = self.get_current_player()
        if current_player.is_dazed():
            step_x, step_y = abs(x - current_player.x), abs(y - current_player.y)
            x = current_player.x - step_x if x > current_player.x else current_player.x + step_x
            y = current_player.y - step_y if y > current_player.y else current_player.y + step_y
        if (0 > x or x >= Constants.board_size) and (0 > y or y >= Constants.board_size):
            print("Error: Move out of bounds!")
            self.invalid_turn_handling()
        else:
            if (current_player.x == x) == (current_player.y == y):
                print("Error: Only moving horizontally or vertically is allowed!")
                self.invalid_turn_handling()
            else:
                if self.board.is_obstacle_on_path(current_player.x, current_player.y, x, y):
                    print("Error: There is an obstacle on the way!")
                    self.invalid_turn_handling()
                else:
                    steps = (abs(current_player.x - x) + abs(current_player.y - y))
                    used_energy = steps * (current_player.get_total_weight() + 1)
                    current_player.decrease_energy(used_energy)
                    if current_player.energy > 0:
                        current_player.x = x
                        current_player.y = y
                        print(f"Moving to position ({x}, {y}), took you ", used_energy, "energy points!")
                    else:
                        print("Error: Not enough energy for this move!")
                        self.invalid_turn_handling()

    def handle_mine(self, user_input):
        match = re.match(r'^mine (-?\d+) (-?\d+)$', user_input)
        if not match:
            return
        x, y = int(match.group(1)), int(match.group(2))
        current_player = self.get_current_player()
        if (0 > x or x >= Constants.board_size) and (0 > y or y >= Constants.board_size):
            print("Error: Mine out of bounds!")
            self.invalid_turn_handling()
        else:
            if not are_neighbours(current_player.x, current_player.y, x, y):
                print("Error: Can not mine something that is not your neighbor!")
                self.invalid_turn_handling()
            else:
                if self.board.board_cells[x][y].print_symbol == Constants.cheep_crystal_symbol:
                    if self.board.board_cells[x][y].mine():
                        if current_player.add_to_backpack_storage_(Crystal(False)):
                            print("Cheep crystal added to your backpack!")
                            current_player.decrease_energy(Constants.mining_energy_cheap_crystal_loss)
                        else:
                            print("Backpack full!")
                            self.invalid_turn_handling()
                    else:
                        print("You can not mine a mine that is not replenished!")
                        self.invalid_turn_handling()
                elif self.board.board_cells[x][y].print_symbol == Constants.expensive_crystal_symbol:
                    if self.board.board_cells[x][y].mine():
                        if current_player.add_to_backpack_storage_(Crystal(True)):
                            print("Expensive crystal added to your backpack!")
                            current_player.decrease_energy(Constants.mining_energy_expensive_crystal_loss)
                        else:
                            print("Backpack full!")
                            self.invalid_turn_handling()
                    else:
                        print("You can not mine a mine that is not replenished!")
                        self.invalid_turn_handling()
                else:
                    print("Error: Can not mine something that is not crystal!")
                    self.invalid_turn_handling()