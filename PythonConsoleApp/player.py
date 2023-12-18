from colorama import Fore, Style

from constants import Constants


class Player:
    def __init__(self, name, x, y):
        self.name = name
        self.energy = Constants.start_energy
        self.xp = Constants.start_xp
        self.coins = Constants.start_coins
        self.x = x
        self.y = y
        self.frozen_turns = 0
        self.daze_turns = 0
        self.ores_in_backpack = 0
        self.increased_backpack_turns = 0
        self.backpack_storage_capacity = Constants.backpack_default_storage_capacity
        self.crystals = []

    def decrease_energy(self, amount):
        self.energy -= amount
        if self.energy <= 0:
            self.energy = 0

    def increase_energy(self, amount):
        self.energy += amount
        if self.energy >= Constants.max_energy:
            self.energy = Constants.max_energy

    def add_coins(self, c):
        self.coins += c
        if self.coins > Constants.max_coins:
            self.coins = Constants.max_coins

    def remove_coins(self, c):
        self.coins -= c
        if self.coins < 0:
            self.coins = 0

    def is_frozen(self):
        return self.frozen_turns > 0

    def add_frozen_turns(self):
        self.frozen_turns += Constants.number_of_frozen_turns

    def decrease_frozen_turns(self):
        if self.frozen_turns > 0:
            self.frozen_turns -= - 1

    def is_dazed(self):
        return self.daze_turns > 0

    def decrease_daze_turns(self):
        if self.daze_turns > 0:
            self.daze_turns -= 1

    def add_daze_turns(self):
        self.daze_turns += Constants.number_of_daze_turns

    def add_increased_backpack_storage_turns(self):
        self.increased_backpack_turns += Constants.number_of_bigger_backpack_turns
        self.backpack_storage_capacity = (Constants.backpack_default_storage_capacity +
                                          Constants.increase_in_backpack_storage_capacity)

    def decrease_increased_backpack_storage_turns(self):
        if self.increased_backpack_turns > 0:
            self.increased_backpack_turns -= 1
        else:
            self.backpack_storage_capacity = Constants.backpack_default_storage_capacity
            if self.ores_in_backpack > Constants.backpack_default_storage_capacity:  # TODO ovde kad dodas listu proveru
                self.ores_in_backpack = Constants.backpack_default_storage_capacity

    def print_player_stats(self, color):
        print(
            f"{color}{self.name}{Style.RESET_ALL}\n"
            f"{Fore.GREEN}Energy: {self.energy}{Style.RESET_ALL} "
            f"{Fore.CYAN}XP: {self.xp}{Style.RESET_ALL} "
            f"{Fore.YELLOW}Coins: {self.coins}{Style.RESET_ALL} "
            f"{Fore.MAGENTA}Position: ({self.x},{self.y}){Style.RESET_ALL}\n"
            f"{Fore.LIGHTYELLOW_EX}Backpack capacity: {self.backpack_storage_capacity}{Style.RESET_ALL} "
            f"{Fore.LIGHTGREEN_EX}Increased backpack capacity duration: {self.increased_backpack_turns}{Style.RESET_ALL} "
            f"{Fore.LIGHTRED_EX}Daze turns: {self.daze_turns}{Style.RESET_ALL} "
            f"{Fore.LIGHTCYAN_EX}Frozen turns: {self.frozen_turns}{Style.RESET_ALL} "
        )