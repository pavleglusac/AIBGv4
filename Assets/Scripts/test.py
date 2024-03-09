import sys
import time
import logging

logging.basicConfig(filename='/Users/pavleglusac/Personal/AIBGv4/Assets/Scripts/logs_raw.txt', filemode='a', level=logging.DEBUG)
logging.info("mrnjaaau")


def act(line):
    print(f"move 1 1", flush=True)
    # logging.info(lines)


while True:
    line = sys.stdin.readline().strip()
    if line:
        logging.info(line)
        act(line)

    time.sleep(2)


print("Python Echo Script Ended.")
