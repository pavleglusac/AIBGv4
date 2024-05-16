#!/bin/bash

dir_to_look=${1:-.}
while true; do

    if [ -f "$dir_to_look/pid.log" ]; then
        pid=$(cat "$dir_to_look/pid.log")
    fi
    echo "PID: $pid"
    if ! ps -p $pid > /dev/null; then
        echo "PID $pid not found. Exiting."
        continue
    fi

    mem_usage=$(top -pid $pid -l 1 | awk '/^PID/{getline; print $8}')
    echo "$(date): Memory usage of PID $pid is $mem_usage"
    # if memory usage exceeds 2000MB, kill the process
    if [ $(echo "$mem_usage > 2000" | bc) -eq 1 ]; then
        echo "Memory usage exceeds 2000MB. Killing PID $pid"
        kill $pid
        break
    fi
    sleep 0.5
done