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
        self.x_base = x
        self.y_base = y
        self.frozen_turns = 0
        self.daze_turns = 0
        self.increased_backpack_turns = 0
        self.backpack_storage_capacity = Constants.backpack_default_storage_capacity
        self.crystals = []

    def is_in_base(self):
        return self.x_base == self.x and self.y_base == self.y

    def remove_expensive_crystals(self, number_of_crystals_to_remove):
        expensive_crystals = [crystal for crystal in self.crystals if crystal.is_expensive]
        if len(expensive_crystals) >= number_of_crystals_to_remove:
            removed_crystals = expensive_crystals[:number_of_crystals_to_remove]
            self.crystals = [crystal for crystal in self.crystals if crystal not in removed_crystals]

    def remove_cheap_crystals(self, number_of_crystals_to_remove):
        non_expensive_crystals = [crystal for crystal in self.crystals if not crystal.is_expensive]
        if len(non_expensive_crystals) >= number_of_crystals_to_remove:
            removed_crystals = non_expensive_crystals[:number_of_crystals_to_remove]
            self.crystals = [crystal for crystal in self.crystals if crystal not in removed_crystals]

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
            if self.get_total_weight() > Constants.backpack_default_storage_capacity:
                while self.get_total_weight() > Constants.backpack_default_storage_capacity:
                    self.crystals.pop()

    def add_to_backpack_storage_(self, crystal) -> bool:
        if self.get_total_weight() + crystal.get_weight() <= self.backpack_storage_capacity:
            self.crystals.append(crystal)
            return True
        return False

    def get_total_weight(self):
        return sum(crystal.get_weight() for crystal in self.crystals)

    def get_total_weight_expensive_processed(self):
        return sum(crystal.get_weight() for crystal in self.crystals if crystal.is_expensive and crystal.is_processed)

    def get_total_weight_expensive_raw(self):
        return sum(
            crystal.get_weight() for crystal in self.crystals if crystal.is_expensive and not crystal.is_processed)

    def get_total_weight_cheap_processed(self):
        return sum(
            crystal.get_weight() for crystal in self.crystals if not crystal.is_expensive and crystal.is_processed)

    def get_total_weight_cheap_raw(self):
        return sum(
            crystal.get_weight() for crystal in self.crystals if not crystal.is_expensive and not crystal.is_processed)

    def get_count_expensive_processed(self):
        return sum(1 for crystal in self.crystals if crystal.is_expensive and crystal.is_processed)

    def get_count_expensive_raw(self):
        return sum(1 for crystal in self.crystals if crystal.is_expensive and not crystal.is_processed)

    def get_count_expensive(self):
        return sum(1 for crystal in self.crystals if crystal.is_expensive)

    def get_count_cheap(self):
        return sum(1 for crystal in self.crystals if not crystal.is_expensive)

    def get_count_cheap_processed(self):
        return sum(1 for crystal in self.crystals if not crystal.is_expensive and crystal.is_processed)

    def get_count_cheap_raw(self):
        return sum(1 for crystal in self.crystals if not crystal.is_expensive and not crystal.is_processed)

    def print_stats(self, color):
        print(
            f"{color}{self.name}{Style.RESET_ALL}\n"
            f"{Fore.GREEN}Energy: {self.energy}{Style.RESET_ALL} "
            f"{Fore.CYAN}XP: {self.xp}{Style.RESET_ALL} "
            f"{Fore.YELLOW}Coins: {self.coins}{Style.RESET_ALL} "
            f"{Fore.MAGENTA}Position: ({self.x},{self.y}){Style.RESET_ALL}\n"
            f"{Fore.LIGHTGREEN_EX}Increased backpack capacity duration: {self.increased_backpack_turns}{Style.RESET_ALL} "
            f"{Fore.LIGHTRED_EX}Daze turns: {self.daze_turns}{Style.RESET_ALL} "
            f"{Fore.LIGHTCYAN_EX}Frozen turns: {self.frozen_turns}{Style.RESET_ALL} "
            f"{Fore.LIGHTYELLOW_EX}Backpack capacity: {self.get_total_weight()}/{self.backpack_storage_capacity}{Style.RESET_ALL}\n"
            f"{Fore.YELLOW}Raw cheap crystal -> Weight: {self.get_total_weight_cheap_raw()}, Count: {self.get_count_cheap_raw()} || "
            f"{Fore.YELLOW}Processed cheap crystal -> Weight: {self.get_total_weight_cheap_processed()}, Count: {self.get_count_cheap_processed()}{Style.RESET_ALL}\n"
            f"{Fore.LIGHTMAGENTA_EX}Raw expensive crystal -> Weight: {self.get_total_weight_expensive_raw()}, Count: {self.get_count_expensive_raw()} || "
            f"{Fore.LIGHTMAGENTA_EX}Processed expensive crystal -> Weight: {self.get_total_weight_expensive_processed()}, Count: {self.get_count_expensive_processed()}{Style.RESET_ALL}\n"
        )

    def add_xp(self, xp):
        self.xp += xp