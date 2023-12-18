import os

from dotenv import load_dotenv


class Constants:
    board_size = 0
    resting_energy = 0
    time_to_do_action_in_turn_duration = 0
    response_message_show_duration = 0
    max_number_of_turns = 0
    invalid_turn_energy_penalty = 0
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
    raw_cheap_crystal_weight = 0
    processed_cheap_crystal_weight = 0
    raw_cheap_crystal_cost = 0
    processed_cheap_crystal_cost = 0
    raw_expensive_crystal_weight = 0
    processed_expensive_crystal_weight = 0
    raw_expensive_crystal_cost = 0
    processed_expensive_crystal_cost = 0
    mining_energy_cheap_crystal_loss = 0
    mining_energy_expensive_crystal_loss = 0
    player1_symbol = ""
    player2_symbol = ""
    player1_castle_symbol = ""
    player2_castle_symbol = ""
    empty_pillar_symbol = ""
    cheep_crystal_symbol = ""
    expensive_crystal_symbol = ""
    refinement_facility_symbol = ""
    number_of_cheap_crystals = 0
    number_of_expensive_crystals = 0
    number_of_frozen_turns = 0
    freeze_cost = 0
    number_of_daze_turns = 0
    daze_cost = 0
    number_of_bigger_backpack_turns = 0
    bigger_backpack_cost = 0
    backpack_default_storage_capacity = 0
    increase_in_backpack_storage_capacity = 0

    @staticmethod
    def load_from_env():
        load_dotenv(dotenv_path='../env.txt')
        Constants.resting_energy = int(os.getenv("resting_energy"))
        Constants.board_size = int(os.getenv("board_size"))
        Constants.time_to_do_action_in_turn_duration = int(os.getenv("time_to_do_action_in_turn_duration"))
        Constants.response_message_show_duration = int(os.getenv("response_message_show_duration"))
        Constants.max_number_of_turns = int(os.getenv("max_number_of_turns"))
        Constants.start_energy = int(os.getenv("start_energy"))
        Constants.start_coins = int(os.getenv("start_coins"))
        Constants.max_energy = int(os.getenv("max_energy"))
        Constants.max_coins = int(os.getenv("max_coins"))
        Constants.start_xp = int(os.getenv("start_xp"))
        Constants.winning_xp = int(os.getenv("winning_xp"))
        Constants.player1_name = os.getenv("player1_name")
        Constants.player2_name = os.getenv("player2_name")
        Constants.refinement_facility_cost = int(os.getenv("refinement_facility_cost"))
        Constants.movement_cost = int(os.getenv("movement_cost"))
        Constants.raw_cheap_crystal_weight = int(os.getenv("raw_cheap_crystal_weight"))
        Constants.processed_cheap_crystal_weight = int(os.getenv("processed_cheap_crystal_weight"))
        Constants.raw_cheap_crystal_cost = int(os.getenv("raw_cheap_crystal_cost"))
        Constants.processed_cheap_crystal_cost = int(os.getenv("processed_cheap_crystal_cost"))
        Constants.raw_expensive_crystal_weight = int(os.getenv("raw_expensive_crystal_weight"))
        Constants.processed_expensive_crystal_weight = int(os.getenv("processed_expensive_crystal_weight"))
        Constants.raw_expensive_crystal_cost = int(os.getenv("raw_expensive_crystal_cost"))
        Constants.processed_expensive_crystal_cost = int(os.getenv("processed_expensive_crystal_cost"))
        Constants.invalid_turn_energy_penalty = int(os.getenv("invalid_turn_energy_penalty"))
        Constants.mining_energy_cheap_crystal_loss = int(os.getenv("mining_energy_cheap_crystal_loss"))
        Constants.mining_energy_expensive_crystal_loss = int(os.getenv("mining_energy_expensive_crystal_loss"))
        Constants.player1_symbol = os.getenv("player1_symbol")
        Constants.player2_symbol = os.getenv("player2_symbol")
        Constants.player1_castle_symbol = os.getenv("player1_castle_symbol")
        Constants.player2_castle_symbol = os.getenv("player2_castle_symbol")
        Constants.empty_pillar_symbol = os.getenv("empty_pillar_symbol")
        Constants.cheep_crystal_symbol = os.getenv("cheep_crystal_symbol")
        Constants.expensive_crystal_symbol = os.getenv("expensive_crystal_symbol")
        Constants.refinement_facility_symbol = os.getenv("refinement_facility_symbol")
        Constants.number_of_cheap_crystals = int(os.getenv("number_of_cheap_crystals"))
        Constants.number_of_expensive_crystals = int(os.getenv("number_of_expensive_crystals"))
        Constants.number_of_frozen_turns = int(os.getenv("number_of_frozen_turns"))
        Constants.freeze_cost = int(os.getenv("freeze_cost"))
        Constants.number_of_daze_turns = int(os.getenv("number_of_daze_turns"))
        Constants.daze_cost = int(os.getenv("daze_cost"))
        Constants.number_of_bigger_backpack_turns = int(os.getenv("number_of_bigger_backpack_turns"))
        Constants.bigger_backpack_cost = int(os.getenv("bigger_backpack_cost"))
        Constants.backpack_default_storage_capacity = int(os.getenv("backpack_default_storage_capacity"))
        Constants.increase_in_backpack_storage_capacity = int(os.getenv("increase_in_backpack_storage_capacity"))