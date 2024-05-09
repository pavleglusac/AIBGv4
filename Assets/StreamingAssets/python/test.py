import sys
import time
import logging
import json

logging.basicConfig(filename='./logs_raw.txt', level=logging.DEBUG)
logging.info("mrnjaaau")

def handle_unhandled_exception(exc_type, exc_value, exc_traceback):
    if issubclass(exc_type, KeyboardInterrupt):
        sys.__excepthook__(exc_type, exc_value, exc_traceback)
        return
    logging.critical("Unhandled exception", exc_info=(exc_type, exc_value, exc_traceback))

sys.excepthook = handle_unhandled_exception

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


class Board:
    def __init__(self, grid):
        self.grid = grid
    
    def __str__(self):
        # pretty print rows and columns
        return "\n".join(["".join(row) for row in self.grid])


class GameState:
    def __init__(self, text):
        text = text.replace("^", "\n")
        dic = json.loads(text)
        self.player1 = Player(**dic['player1'])
        self.player2 = Player(**dic['player2'])
        self.board = Board(dic['board'])


def find_diamonds(board, player):
    # for all diamonds on the board, find the closest one and give me the next move to get there
    my_pos = player.position
    min_dist = 1000
    target_pos = []
    for i in range(len(board.grid)):
        for j in range(len(board.grid[i])):
            if board.grid[i][j] == 'D':
                target_pos.append((i, j))
    # try to find path for any diamond
    logging.info(f"trying to find path for target_pos: {target_pos}")
    for pos in target_pos:
        path = find_path_for_target(board, player, pos)
        logging.info(f"path: {path}")
        if path:
            return path
    return None
    


def find_path_for_target(board, player, target_pos):
    my_pos = tuple(player.position)
    queue = [(my_pos, [])]
    visited = set()
    while queue:
        pos, path = queue.pop(0)
        if pos == target_pos:
            path = path[:-1]
            return path
        if pos in visited:
            continue
        visited.add(pos)
        if board.grid[pos[0]][pos[1]] != 'E' and board.grid[pos[0]][pos[1]] != '2':
            continue
        for i, j in [(0, 1), (0, -1), (1, 0), (-1, 0)]:
            new_pos = (pos[0] + i, pos[1] + j)
            
            if not( 0 <= new_pos[0] < len(board.grid) and 0 <= new_pos[1] < len(board.grid[0])):
                continue
            queue.append((new_pos, path + [new_pos]))


def next_move(path, my_pos):
    if not path:
        return None

    direction_first_step = (path[0][0] - my_pos[0], path[0][1] - my_pos[1])
    if len(path) == 1:
        return path[0]
    
    for i in range(1, len(path)):
        direction_next_step = (path[i][0] - path[i-1][0], path[i][1] - path[i-1][1])
        if direction_next_step != direction_first_step:
            return path[i-1]
    
    return path[-1]

def diamonds_in_bag(player1):
    # check if player1 has diamonds in his bag
    if player1.raw_diamonds > 0:
        return True
    

def next_to_type(board, player, type='D'):
    # check if player is next to a diamond
    my_x, my_y = player.position
    for i in range(-1, 2):
        for j in range(-1, 2):
            # check if in bounds
            if i == 0 and j == 0:
                continue
            # one must be zero
            if not( i == 0 or j == 0):
                continue
            if not( 0 <= my_x + i < len(board.grid) and 0 <= my_y + j < len(board.grid[0])):
                continue

            if board.grid[my_x + i][my_y + j] == type:
                return [my_x + i, my_y + j]
    return None


def on_type(board, player, type='D'):
    my_x, my_y = player.position
    if board.grid[my_x][my_y] == type:
        return True
    return False


def act(line):
    logging.info("-"*100)
    game_state = GameState(line)
    logging.info(game_state.player1)
    logging.info(game_state.player2)
    # logging.info(lines)
    logging.info(game_state.board)
    # if i am next to a diamond, pick it up
    my_x, my_y = game_state.player2.position

    if not diamonds_in_bag(game_state.player2):
        logging.info("no diamonds in bag")
        next_to_diamonds_pos = next_to_type(game_state.board, game_state.player2)
        if next_to_diamonds_pos:
            print(f"mine {next_to_diamonds_pos[0]} {next_to_diamonds_pos[1]}", flush=True)
            return
        closest_diamond = find_diamonds(game_state.board, game_state.player2)
        logging.info(closest_diamond)
        next_move_to_diamond = next_move(closest_diamond, game_state.player2.position)
        logging.info(next_move_to_diamond)
        if next_move_to_diamond:
            print(f"move {next_move_to_diamond[0]} {next_move_to_diamond[1]}", flush=True)
        else:
            print("rest", flush=True)
    else:
        # go to base on 11, 0     
        logging.info("diamonds in bag")
        
        if my_x == 0 and my_y == 11:
            print(f"conv 0 diamond 0 mineral to coins, 0 diamond 0 mineral to energy, {game_state.player2.raw_diamonds} diamond {game_state.player2.raw_minerals} mineral to xp", flush=True)
            return
        
        next_to_base = next_to_type(game_state.board, game_state.player2, 'B')
        if next_to_base:
            print(f"move {next_to_base[0]} {next_to_base[1]}", flush=True)
            return

        closest_base = find_path_for_target(game_state.board, game_state.player2, (0, 11))
        logging.info(closest_base)
        next_move_to_base = next_move(closest_base, game_state.player2.position)
        logging.info(next_move_to_base)
        if next_move_to_base:
            print(f"move {next_move_to_base[0]} {next_move_to_base[1]}", flush=True)
        else:
            print("rest", flush=True)

while True:
    line = sys.stdin.readline().strip()
    if line:
        logging.info(line)
        act(line)
    
    time.sleep(2)


print("Python Echo Script Ended.")
