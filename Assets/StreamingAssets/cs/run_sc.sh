(sleep 900 && kill $$) & SCRIPT_DIR=$(cd "$(dirname "$0")"; pwd) && cd $SCRIPT_DIR && dotnet run