from constants import Constants
from game import Game


def main():
    Constants.load_from_env()
    game = Game()
    game.start()
    pass


if __name__ == '__main__':
    main()