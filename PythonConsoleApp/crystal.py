from constants import Constants


class Crystal:
    def __init__(self, is_expensive):
        self.is_processed = False
        self.is_expensive = is_expensive

    def get_weight(self):
        if self.is_expensive and self.is_processed:
            return Constants.processed_expensive_crystal_weight
        if self.is_expensive and not self.is_processed:
            return Constants.raw_expensive_crystal_weight
        if not self.is_expensive and self.is_processed:
            return Constants.processed_cheap_crystal_weight
        if not self.is_expensive and not self.is_processed:
            return Constants.raw_cheap_crystal_weight