# get the dir of current sh file
SCRIPT_DIR=$(cd "$(dirname "$0")"; pwd)
exec python3 $SCRIPT_DIR/test.py