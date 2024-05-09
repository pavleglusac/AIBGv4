#!/bin/bash

# Determine the directory where this script is located
SCRIPT_DIR=$(cd "$(dirname "$0")"; pwd)

# Compile the Java program
javac "$SCRIPT_DIR/Main.java"

# Check if compilation was successful
if [ $? -eq 0 ]; then
    # Run the Java program
    java -cp "$SCRIPT_DIR" Main
else
    echo "Compilation failed"
fi
