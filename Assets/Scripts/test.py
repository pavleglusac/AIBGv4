import sys
import time
import logging

logging.basicConfig(filename='./logs_raw.txt', filemode='a', level=logging.DEBUG)
logging.info("mrnjaaau")


def act(line):
    print(f"move 1 1", flush=True)
    # logging.info(lines)


while True:
    line = sys.stdin.readline().strip()
    if line:
        print("rest", flush=True)



print("Python Echo Script Ended.")
