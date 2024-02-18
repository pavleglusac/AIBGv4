import sys
import time
# print("Python Echo Script Started. Waiting for input...")

all_inputs = []

while True:
    line = sys.stdin.readline().strip()
    if not line:
        continue
    all_inputs.append(line)
    print(f"move 1 1", flush=True)
    time.sleep(2)

print("Python Echo Script Ended.")
