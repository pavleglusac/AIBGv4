from constants import Constants


class BoardCell:
    def __init__(self, print_symbol):
        self.print_symbol = print_symbol
        self.MAX_MINE_HITS = 0
        self.REPLENISH_TURNS = 0
        self.hits = 0
        self.turns_to_wait = 0
        if self.print_symbol == Constants.cheep_crystal_symbol:
            self.MAX_MINE_HITS = Constants.cheap_crystal_mine_hits
            self.REPLENISH_TURNS = Constants.cheap_crystal_replenish_turns

        if self.print_symbol == Constants.expensive_crystal_symbol:
            self.MAX_MINE_HITS = Constants.expensive_crystal_mine_hits
            self.REPLENISH_TURNS = Constants.expensive_crystal_replenish_turns

        self.hits = self.MAX_MINE_HITS

    def mine(self) -> bool:
        if (self.print_symbol == Constants.cheep_crystal_symbol
                or self.print_symbol == Constants.expensive_crystal_symbol):
            if self.hits > 0:
                self.hits -= 1
                if self.hits == 0:
                    self.turns_to_wait = self.REPLENISH_TURNS
                return True
            return False
        return False

    def replenish(self):
        if self.print_symbol != Constants.cheep_crystal_symbol and self.print_symbol != Constants.expensive_crystal_symbol:
            return
        if self.turns_to_wait > 0:
            self.turns_to_wait -= 1
            if self.turns_to_wait == 0:
                self.hits = self.MAX_MINE_HITS