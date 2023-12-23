from constants import Constants
from game import Game


def main():
    Constants.load_from_env()
    game = Game()
    game.play_game()


if __name__ == '__main__':
    main()