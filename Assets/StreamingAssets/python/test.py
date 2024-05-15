import sys
import time
import logging
import json
import signal
import random

logging.basicConfig(filename='./logs_raw.txt', level=logging.DEBUG)
logging.info("mrnjaaau")

# def handle_unhandled_exception(exc_type, exc_value, exc_traceback):
#     if issubclass(exc_type, KeyboardInterrupt):
#         sys.__excepthook__(exc_type, exc_value, exc_traceback)
#         return
#     logging.critical("Unhandled exception", exc_info=(exc_type, exc_value, exc_traceback))

# sys.excepthook = handle_unhandled_exception

# movePattern = @"^move (-?\d+) (-?\d+)$";

# minePattern = @"^mine (\d+) (\d+)$";

# buildPattern = @"^build (\d+) (\d+)$";

# conversionsPattern = @"^conv (\d+) diamond (\d+) mineral to coins, (\d+) diamond (\d+) mineral to energy, (\d+) diamond (\d+) mineral to xp$";

# restPattern = @"^rest$";

# shopPattern = @"^shop (freeze|backpack|daze)$";

# attackPattern = @"^attack (\d+) (\d+)$";

# putRefinement = @"^refinement-put (\d+) (\d+) mineral (\d+) diamond (\d+)$";

# takeRefinement = @"^refinement-take (\d+) (\d+) mineral (\d+) diamond (\d+)$";


class Player:
    def __init__(self, name, energy, xp, coins, position, increased_backpack_duration, daze_turns, frozen_turns, backpack_capacity, raw_minerals, processed_minerals, raw_diamonds, processed_diamonds):
        self.name = name
        self.energy = energy
        self.xp = xp
        self.coins = coins
        self.position = position
        self.increased_backpack_duration = increased_backpack_duration
        self.daze_turns = daze_turns
        self.frozen_turns = frozen_turns
        self.backpack_capacity = backpack_capacity
        self.raw_minerals = raw_minerals
        self.processed_minerals = processed_minerals
        self.raw_diamonds = raw_diamonds
        self.processed_diamonds = processed_diamonds
        self.daze = False

    def move(self, x, y):
        if self.daze:
            x, y = y, x
        print(f"move {x} {y}", flush=True)


    def freeze(self):
        print("shop freeze", flush=True)

    def calculate_capacity(self):
        return self.raw_minerals * 2 + self.processed_minerals * 1 + self.raw_diamonds * 5 + self.processed_diamonds * 3


    def handle_conversion(self):
        # if I am low on energy, convert ores to energy
        minerals_to_energy = 0
        diamonds_to_energy = 0
        resulting_energy = 0
        mineral_count = self.raw_minerals + self.processed_minerals
        diamond_count = self.raw_diamonds + self.processed_diamonds
        logging.info(f"energy when converting: {self.energy}")
        if self.energy < 120:
            # convert all minerals to energy
            if mineral_count:
                minerals_to_energy = min(mineral_count, 3)
                resulting_energy += minerals_to_energy * GameState.minerals_to_energy
                mineral_count -= minerals_to_energy
            
            # if resulting energy is lower than 500, convert diamonds to energy
            if resulting_energy < 500:
                if diamond_count:
                    diamonds_to_energy = min(diamond_count, 3)
                    resulting_energy += diamonds_to_energy * GameState.diamonds_to_energy
                    diamond_count -= diamonds_to_energy
            
        minerals_to_xp = 0
        diamonds_to_xp = 0
        resulting_xp = 0
        # if I have a lot of ores, convert one to coins, rest to xp
        # if I have few ores, convert all to xp
        if mineral_count:
            minerals_to_xp = min(mineral_count, 3)
            resulting_xp += minerals_to_xp * GameState.minerals_to_xp
            mineral_count -= minerals_to_xp

        if diamond_count:
            diamonds_to_xp = min(diamond_count, 3)
            resulting_xp += diamonds_to_xp * GameState.diamonds_to_xp
            diamond_count -= diamonds_to_xp

        # convert rest to coins
        minerals_to_coins = mineral_count
        diamonds_to_coins = diamond_count
        # if all are zero, return
        suma = minerals_to_coins + diamonds_to_coins + minerals_to_energy + diamonds_to_energy + minerals_to_xp + diamonds_to_xp
        if suma == 0:
            logging.info("no ores to convert")
            return False
        logging.info(f"converting {minerals_to_coins} mineral {diamonds_to_coins} diamond to coins, {minerals_to_energy} mineral {diamonds_to_energy} diamond to energy, {minerals_to_xp} mineral {diamonds_to_xp} diamond to xp")
        print(f"conv {diamonds_to_coins} diamond {minerals_to_coins} mineral to coins, {diamonds_to_energy} diamond {minerals_to_energy} mineral to energy, {diamonds_to_xp} diamond {minerals_to_xp} mineral to xp", flush=True)
        return True


class Board:
    def __init__(self, grid):
        self.grid = grid
    
    def __str__(self):
        # pretty print rows and columns
        return "\n".join(["".join(row) for row in self.grid])


class GameState:
    diamonds_to_coins = 50
    minerals_to_coins = 15
    diamonds_to_energy = 100
    minerals_to_energy = 250
    diamonds_to_xp = 25
    minerals_to_xp = 10

    def __init__(self, text):
        text = text.replace("^", "\n")
        dic = json.loads(text)
        self.turns = int(dic['turn'])
        self.firstPlayerMove = bool(dic['firstPlayerTurn'])
        self.player1 = Player(**dic['player1'])
        self.player2 = Player(**dic['player2'])
        self.board = Board(dic['board'])


def get_valid_path(board, player, target_pos, step_on_target=False):
    my_pos = tuple(player.position)
    base_marker = player.base_marker
    queue = [(my_pos, [])]
    visited = set()
    
    while queue:
        pos = queue.pop(0)
        pos, path = pos
        if pos in visited:
            continue
        visited.add(pos)
        if not step_on_target:
            # check if we are next to the target, return path
            if abs(pos[0] - target_pos[0]) + abs(pos[1] - target_pos[1]) == 1:
                return path + [pos]
        # check if we are on the target
        if pos == target_pos:
            return path + [target_pos]
        
        # check if we can move in all directions
        for i, j in [(0, 1), (0, -1), (1, 0), (-1, 0)]:
            if abs(i) + abs(j) != 1:
                continue
            new_pos = (pos[0] + i, pos[1] + j)
            if new_pos[0] < 0 or new_pos[0] >= len(board.grid) or new_pos[1] < 0 or new_pos[1] >= len(board.grid[0]):
                continue
            pillar = board.grid[new_pos[0]][new_pos[1]]

            if any([pillar.startswith(x) for x in ['F', 'D', 'M', '1', '2', player.opp_base_marker]]):
                continue

            if board.grid[new_pos[0]][new_pos[1]].startswith(base_marker) and new_pos != tuple(player.base):
                continue

            if new_pos in visited:
                continue
            queue.append((new_pos, path + [pos]))
    return None



def closest_ore(board, player, ore_type='D'):
    # for all ores on the board (D_N_M, M_N_M), N needs to be > 0
    # find the closest one and give me the next move to get there
    target_pos = []
    for i in range(len(board.grid)):
        for j in range(len(board.grid[i])):
            # check if N > 0
            if board.grid[i][j].startswith(ore_type) and int(board.grid[i][j].split('_')[1]) > 0:
                target_pos.append((i, j))
    # try to find path for closest ore
    logging.info(f"trying to find path for target_pos: {target_pos}")
    # sort by distance path length
    target_paths = [get_valid_path(board, player, pos) for pos in target_pos]
    target_paths = [path for path in target_paths if path]
    # sort by sum of steps
    target_paths.sort(key=lambda x: len(x))
    if target_paths:
        return target_paths[0]
    return None


def compress_path_as_turns(path):
    # if the axis is the same, just move in that direction
    # [(0, 0), (0, 1), (0, 2), (0, 3)] -> [(0, 3)], [(0, 0), (0, -1), (0, -2), (0, -3)] -> [(0, -3)
    # [(0, 0), (0, 1), (1, 1), (2, 1)] -> [(0, 1), (2, 1)]
    new_path = []
    i = 0
    compressing_x = False
    compressing_y = False
    if path is None:
        return []
    while i < len(path):
        if i == 0:
            new_path.append(path[i])
            i += 1
            continue
    
        if path[i][0] == path[i - 1][0]:
            compressing_x = True
            compressing_y = False
        elif path[i][1] == path[i - 1][1]:
            compressing_y = True
            compressing_x = False
        
        if compressing_x:
            while i < len(path) and path[i][0] == path[i - 1][0]:
                i += 1
            new_path.append(path[i - 1])
        elif compressing_y:
            while i < len(path) and path[i][1] == path[i - 1][1]:
                i += 1
            new_path.append(path[i - 1])
    return new_path
        

def energy_for_path(player, path):
    return (min(8, player.backpack_capacity) + 1) * len(path)


def energy_check(player, board, base_pos=(0, 9)):
    path_to_base = get_valid_path(board, player, base_pos, step_on_target=True)
    if not path_to_base:
        return None
    energy_needed = energy_for_path(player, path_to_base)
    if player.energy < energy_needed:
        # i can't make it to the base
        return None
    return path_to_base

def find_opponent_facilities(game_state, board, player):
    # find opponent's facility
    opp_marker = 'F_2' if game_state.firstPlayerMove else 'F_1'
    facs = []
    for i in range(len(board.grid)):
        for j in range(len(board.grid[i])):
            if board.grid[i][j].startswith(opp_marker):
                facs.append((i, j))
    return facs

def act(line):
    game_state = GameState(line)
    player = game_state.player1 if game_state.firstPlayerMove else game_state.player2
    player.base_marker = 'A' if game_state.firstPlayerMove else 'B'
    player.opp_base_marker = 'B' if game_state.firstPlayerMove else 'A'
    board = game_state.board
    game_state.base = (9, 0) if game_state.firstPlayerMove else (0, 9)
    player.base = game_state.base
    player_pos = tuple(player.position)

    # if I have less than 120 energy, check if I should make it to the base/rest
    if player_pos == game_state.base:
        logging.info(f"on base")
        ret = player.handle_conversion()
        if ret:
            return
    
    if game_state.turns < 2:
        if game_state.firstPlayerMove:
            print("build 8 0", flush=True)
        else:
            print("build 0 8", flush=True)
        return

    
    logging.info(f"energy on move: {player.energy}")
    if player.energy < 120:
        logging.info(f"low on energy")
        path_to_base = None
        if player.raw_minerals or player.processed_minerals or player.raw_diamonds or player.processed_diamonds:
            path_to_base = energy_check(player, board, game_state.base)
        if not path_to_base:
            logging.info(f"resting")
            print("rest", flush=True)
            return
        path_to_base = compress_path_as_turns(path_to_base)
        next_step = path_to_base[0]
        if next_step == player_pos and len(path_to_base) > 1:
            next_step = path_to_base[1]
        elif next_step == player_pos and len(path_to_base) == 1:
            print("rest", flush=True)
        logging.info(f"moving to {next_step}")
        player.move(next_step[0], next_step[1])
        return
    
    if player.coins >= 175 and random.random() > 0.5:
        logging.info(f"buying freeze")
        player.freeze()
        return

    facs = find_opponent_facilities(game_state, board, player)

    if facs and player.energy >= 300:
        logging.info(f"found opponent's facilities")
        valid_facs_paths = [get_valid_path(board, player, fac) for fac in facs]
        valid_facs_paths = [path for path in valid_facs_paths if path]
        len_to_fac = [len(path) for path in valid_facs_paths]
        if not len_to_fac:
            logging.info(f"resting")
            print("rest", flush=True)
            return
        closest_fac = facs[len_to_fac.index(min(len_to_fac))]
        path_to_fac = get_valid_path(board, player, closest_fac)
        if not path_to_fac:
            logging.info(f"resting")
            print("rest", flush=True)
            return
        path_to_fac = compress_path_as_turns(path_to_fac)
        next_step = path_to_fac[0]
        if next_step == player_pos and len(path_to_fac) > 1:
            next_step = path_to_fac[1]
        elif next_step == player_pos and len(path_to_fac) == 1:
            # if I am next to the facility, attack it
            print(f"attack {closest_fac[0]} {closest_fac[1]}", flush=True)
            return
        elif next_step == player_pos:
            print("rest", flush=True)
            return
        logging.info(f"moving to {next_step}, my_pos: {player_pos}")
        player.move(next_step[0], next_step[1])
        return
    
    # if my bag is at max capacity, go to base
    logging.info(f"backpack_capacity: {player.backpack_capacity}")
    if player.calculate_capacity() >= 5:
        logging.info(f"bag is full")
        path_to_base = get_valid_path(board, player, game_state.base, step_on_target=True)
        if not path_to_base:
            logging.info(f"resting")
            print("rest", flush=True)
            return
        path_to_base = compress_path_as_turns(path_to_base)
        logging.info(f"my_pos: {player_pos}")
        logging.info(f"path_to_base: {path_to_base}")
        next_step = path_to_base[0]
        logging.info(f"next_step: {next_step}")
        if next_step == player_pos and len(path_to_base) > 1:
            next_step = path_to_base[1]
        elif next_step == player_pos and len(path_to_base) == 1:
            print("rest", flush=True)
            return
        logging.info(f"moving to {next_step}")
        player.move(next_step[0], next_step[1])
        return
    
    # if I have more than 500 energy, mine diamonds, else mine minerals
    if player.energy >= 500 and player.backpack_capacity <= 3:
        logging.info(f"mining diamonds")
        path_to_ore = closest_ore(board, player, 'D')
    elif player.backpack_capacity <= 5:
        logging.info(f"mining minerals")
        path_to_ore = closest_ore(board, player, 'M')
    if not path_to_ore:
        logging.info(f"resting")
        print("rest", flush=True)
        return

    logging.info(f"my_pos: {player.position}")
    logging.info(f"path_to_ore: {path_to_ore}")
    if not path_to_ore:
        logging.info(f"resting")
        print("rest", flush=True)
        return
    path_to_ore = compress_path_as_turns(path_to_ore)
    logging.info(f"steps: {path_to_ore}")
    next_step = tuple(path_to_ore[0])
    # if next step is current position
    logging.info(f"next_step: {next_step}, player_pos: {player_pos}")
    if next_step == player_pos and len(path_to_ore) == 1:
        # mine the ore by sending mine x y
        # check where the ore is, x, y in [(0, 1),(1, 0),(-1, 0),(0, -1)]
        for i, j in [(0, 1), (1, 0), (-1, 0), (0, -1)]:
            # if in bounds and starts with either D_ or M_
            in_bounds = 0 <= player_pos[0] + i < len(board.grid) and 0 <= player_pos[1] + j < len(board.grid[0])
            if in_bounds:
                starts_with_ore = board.grid[player_pos[0] + i][player_pos[1] + j].startswith('D_') or board.grid[player_pos[0] + i][player_pos[1] + j].startswith('M_')
                if starts_with_ore:
                    ore_is_fresh = int(board.grid[player_pos[0] + i][player_pos[1] + j].split('_')[1]) > 0
                    if ore_is_fresh:
                        ore_x, ore_y = player_pos[0] + i, player_pos[1] + j
                        break
        logging.info(f"mining ore at {ore_x}, {ore_y}")
        print(f"mine {ore_x} {ore_y}", flush=True)
        return
    elif next_step == player_pos:
        next_step = path_to_ore[1]

    logging.info(f"moving to {next_step}")
    player.move(next_step[0], next_step[1])
    return


def timeout_handler(signum, frame):
    raise TimeoutError


signal.signal(signal.SIGALRM, timeout_handler)

while True:
    try:
        signal.alarm(20)
        line = sys.stdin.readline().strip()
        signal.alarm(0)
        if line:
            # logging.info(line)
            act(line)
    except TimeoutError:
        print("Readline timed out after 20 seconds.")
        break
