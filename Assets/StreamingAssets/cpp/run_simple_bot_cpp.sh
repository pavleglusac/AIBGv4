#!/bin/bash
(sleep 900 && kill $$) & 
# Get the directory of this script
SCRIPT_DIR=$(cd "$(dirname "$0")" && pwd)

# Navigate to the directory containing the C++ file
cd "$SCRIPT_DIR"

# Compile the C++ file
g++ -o SimpleBot SimpleBot.cpp

# Check if compilation was successful
if [ $? -eq 0 ]; then
    # Run the compiled executable
    ./SimpleBot
else
    echo "Compilation failed. Please check your C++ code."
fi
