#include <iostream>
#include <vector>
#include <string>
#include <map>
#include <queue>
#include <unordered_set>
#include <algorithm>
#include <thread>
#include <chrono>

using namespace std;
// class Player {
// public:
//     std::string name;
//     int energy, xp, coins, positionX, positionY, increasedBackpackDuration, dazeTurns, frozenTurns, backpackCapacity, rawMinerals, processedMinerals, rawDiamonds, processedDiamonds;

//     Player(std::string name, int energy, int xp, int coins, int positionX, int positionY, int increasedBackpackDuration, int dazeTurns, int frozenTurns, int backpackCapacity, int rawMinerals, int processedMinerals, int rawDiamonds, int processedDiamonds) {
//         this->name = name;
//         this->energy = energy;
//         this->xp = xp;
//         this->coins = coins;
//         this->positionX = positionX;
//         this->positionY = positionY;
//         this->increasedBackpackDuration = increasedBackpackDuration;
//         this->dazeTurns = dazeTurns;
//         this->frozenTurns = frozenTurns;
//         this->backpackCapacity = backpackCapacity;
//         this->rawMinerals = rawMinerals;
//         this->processedMinerals = processedMinerals;
//         this->rawDiamonds = rawDiamonds;
//         this->processedDiamonds = processedDiamonds;
//     }
// };

// class Board {
// public:
//     std::vector<std::vector<char>> grid;

//     Board(std::vector<std::vector<char>> grid) {
//         this->grid = grid;
//     }

//     std::string toString() {
//         std::stringstream ss;
//         for (auto& row : grid) {
//             for (char c : row) {
//                 ss << c;
//             }
//             ss << "\n";
//         }
//         return ss.str();
//     }
// };

// class GameState {
// public:
//     Player player1, player2;
//     Board board;

//     GameState(std::string text) {
//         text = text.replace("^", "\n");
//         std::istringstream iss(text);
//         std::string token;
//         std::map<std::string, std::map<std::string, int>> playerData;
//         std::getline(iss, token, ';');
//         while (std::getline(iss, token, ';')) {
//             std::istringstream iss2(token);
//             std::string key;
//             std::map<std::string, int> data;
//             std::getline(iss2, key, ':');
//             while (std::getline(iss2, key, ',')) {
//                 std::istringstream iss3(key);
//                 std::string subKey;
//                 int value;
//                 std::getline(iss3, subKey, '=');
//                 iss3 >> value;
//                 data[subKey] = value;
//             }
//             playerData[key] = data;
//         }
//         this->player1 = Player(
//             playerData["player1"]["name"],
//             playerData["player1"]["energy"],
//             playerData["player1"]["xp"],
//             playerData["player1"]["coins"],
//             playerData["player1"]["positionX"],
//             playerData["player1"]["positionY"],
//             playerData["player1"]["increasedBackpackDuration"],
//             playerData["player1"]["dazeTurns"],
//             playerData["player1"]["frozenTurns"],
//             playerData["player1"]["backpackCapacity"],
//             playerData["player1"]["rawMinerals"],
//             playerData["player1"]["processedMinerals"],
//             playerData["player1"]["rawDiamonds"],
//             playerData["player1"]["processedDiamonds"]
//         );
//         this->player2 = Player(
//             playerData["player2"]["name"],
//             playerData["player2"]["energy"],
//             playerData["player2"]["xp"],
//             playerData["player2"]["coins"],
//             playerData["player2"]["positionX"],
//             playerData["player2"]["positionY"],
//             playerData["player2"]["increasedBackpackDuration"],
//             playerData["player2"]["dazeTurns"],
//             playerData["player2"]["frozenTurns"],
//             playerData["player2"]["backpackCapacity"],
//             playerData["player2"]["rawMinerals"],
//             playerData["player2"]["processedMinerals"],
//             playerData["player2"]["rawDiamonds"],
//             playerData["player2"]["processedDiamonds"]
//         );
//         std::string boardString = playerData["board"]["grid"];
//         std::istringstream iss3(boardString);
//         std::string row;
//         std::vector<std::vector<char>> grid;
//         while (std::getline(iss3, row, '^')) {
//             std::vector<char> newRow(row.begin(), row.end());
//             grid.push_back(newRow);
//         }
//         this->board = Board(grid);
//     }
// };


// bool diamondsInBag(Player player) {
//     return player.rawDiamonds > 0;
// }

// vector<int> nextToType(Board board, Player player) {
//     int myX = player.positionX;
//     int myY = player.positionY;
//     for (int i = -1; i <= 1; i++) {
//         for (int j = -1; j <= 1; j++) {
//             if (i == 0 && j == 0) continue;
//             if (!(i == 0 || j == 0)) continue;
//             if (!(myX + i >= 0 && myX + i < board.grid.size() && myY + j >= 0 && myY + j < board.grid[0].size())) continue;
//             if (board.grid[myX + i][myY + j] == 'D') return {myX + i, myY + j};
//         }
//     }
//     return {};
// }

// bool onType(Board board, Player player, char type) {
//     int myX = player.positionX;
//     int myY = player.positionY;
//     return board.grid[myX][myY] == type;
// }

// vector<vector<int>> findDiamonds(Board board, Player player) {
//     int myX = player.positionX;
//     int myY = player.positionY;
//     vector<vector<int>> targetPos;
//     for (int i = 0; i < board.grid.size(); i++) {
//         for (int j = 0; j < board.grid[i].size(); j++) {
//             if (board.grid[i][j] == 'D') targetPos.push_back({i, j});
//         }
//     }
//     vector<vector<int>> paths;
//     for (auto pos : targetPos) {
//         auto path = findPathForTarget(board, player, pos);
//         if (!path.empty()) paths.push_back(path);
//     }
//     if (!paths.empty()) return paths[0];
//     return {};
// }

// vector<int> nextMove(vector<vector<int>>& path, int myX, int myY) {
//     if (path.empty()) return {};
//     vector<int> firstStep = path[0];
//     if (path.size() == 1) return firstStep;
//     vector<int> directionFirstStep = {firstStep[0] - myX, firstStep[1] - myY};
//     for (int i = 1; i < path.size(); i++) {
//         vector<int> nextStep = path[i];
//         vector<int> directionNextStep = {nextStep[0] - path[i - 1][0], nextStep[1] - path[i - 1][1]};
//         if (directionNextStep != directionFirstStep) return path[i - 1];
//     }
//     return path[path.size() - 1];
// }

// vector<int> reconstructPath(unordered_set<string>& visited, vector<int>& targetPos) {
//     vector<int> path;
//     path.push_back(targetPos[0]);
//     path.push_back(targetPos[1]);
//     while (true) {
//         vector<int> currentPos = {path[path.size() - 2], path[path.size() - 1]};
//         int currentX = currentPos[0];
//         int currentY = currentPos[1];
//         if (visited.count(to_string(currentX) + "," + to_string(currentY))) {
//             visited.erase(to_string(currentX) + "," + to_string(currentY));
//             vector<vector<int>> directions = {{0, 1}, {0, -1}, {1, 0}, {-1, 0}};
//             for (auto direction : directions) {
//                 int newX = currentX + direction[0];
//                 int newY = currentY + direction[1];
//                 if (visited.count(to_string(newX) + "," + to_string(newY))) {
//                     path.push_back(newX);
//                     path.push_back(newY);
//                     break;
//                 }
//             }
//         } else break;
//     }
//     reverse(path.begin(), path.end());
//     return path;
// }

// vector<int> findPathForTarget(Board board, Player player, vector<int>& targetPos) {
//     int myX = player.positionX;
//     int myY = player.positionY;
//     queue<vector<int>> q;
//     unordered_set<string> visited;
//     q.push({myX, myY});
//     while (!q.empty()) {
//         vector<int> pos = q.front();
//         q.pop();
//         if (pos[0] == targetPos[0] && pos[1] == targetPos[1]) return reconstructPath(visited, targetPos);
//         if (visited.count(to_string(pos[0]) + "," + to_string(pos[1]))) continue;
//         visited.insert(to_string(pos[0]) + "," + to_string(pos[1]));
//         if (board.grid[pos[0]][pos[1]] != 'E' && board.grid[pos[0]][pos[1]] != '2') continue;
//         vector<vector<int>> directions = {{0, 1}, {0, -1}, {1, 0}, {-1, 0}};
//         for (auto direction : directions) {
//             int newX = pos[0] + direction[0];
//             int newY = pos[1] + direction[1];
//             if (newX >= 0 && newX < board.grid.size() && newY >= 0 && newY < board.grid[0].size()) {
//                 q.push({newX, newY});
//             }
//         }
//     }
//     return {};
// }

// void act() {
//     Player player = gameState.player2;
//     Board board = gameState.board;

//     int myX = player.positionX;
//     int myY = player.positionY;

//     if (!diamondsInBag(player)) {
//         vector<int> nextToDiamondsPos = nextToType(board, player);
//         if (!nextToDiamondsPos.empty()) {
//             cout << "mine " << nextToDiamondsPos[0] << " " << nextToDiamondsPos[1] << endl;
//             return;
//         }
//         vector<int> closestDiamond = findDiamonds(board, player);
//         if (!closestDiamond.empty()) {
//             vector<int> nextMoveToDiamond = nextMove(closestDiamond, player.positionX, player.positionY);
//             if (!nextMoveToDiamond.empty()) {
//                 cout << "move " << nextMoveToDiamond[0] << " " << nextMoveToDiamond[1] << endl;
//             } else {
//                 cout << "rest" << endl;
//             }
//         }
//     } else {
//         if (myX == 0 && myY == 11) {
//             cout << "conv 0 diamond 0 mineral to coins, 0 diamond 0 mineral to energy, " << player.rawDiamonds << " diamond " << player.rawMinerals << " mineral to xp" << endl;
//             return;
//         }
//         vector<int> nextToBase = nextToType(board, player);
//         if (!nextToBase.empty()) {
//             cout << "move " << nextToBase[0] << " " << nextToBase[1] << endl;
//             return;
//         }
//         vector<int> closestBase = findPathForTarget(board, player, {0, 11});
//         if (!closestBase.empty()) {
//             vector<int> nextMoveToBase = nextMove(closestBase, player.positionX, player.positionY);
//             if (!nextMoveToBase.empty()) {
//                 cout << "move " << nextMoveToBase[0] << " " << nextMoveToBase[1] << endl;
//             } else {
//                 cout << "rest" << endl;
//             }
//         }
//     }
// }

int main() {
    while (true) {
        // string line;
        // getline(cin, line);
        // if (!line.empty()) {
        //     gameState = GameState(line);
        //     act();
        // }
        cout << "rest" << endl;
        std::this_thread::sleep_for(chrono::milliseconds(2000));
    }
    return 0;
}
