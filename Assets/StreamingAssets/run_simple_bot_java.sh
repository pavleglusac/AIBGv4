#!/bin/bash

# Determine the directory where this script is located
SCRIPT_DIR=$(cd "$(dirname "$0")"; pwd)

# Compile the Java program
javac "$SCRIPT_DIR/SimpleBot.java"

# Check if compilation was successful
if [ $? -eq 0 ]; then
    echo "Compilation successful"
    # Run the Java program
    java -cp "$SCRIPT_DIR" SimpleBot
else
    echo "Compilation failed"
fi
