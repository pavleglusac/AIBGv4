class BoardCell:
    def __init__(self, print_symbol):
        self.print_symbol = print_symbol

    def set_print_symbol(self, symbol):
        self.print_symbol = symbol

    def get_print_symbol(self):
        return self.print_symbol