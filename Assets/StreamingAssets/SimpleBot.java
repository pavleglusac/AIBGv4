import java.util.*;
import java.io.*;

class Player {
    String name;
    int energy, xp, coins, positionX, positionY, increasedBackpackDuration, dazeTurns, frozenTurns, backpackCapacity, rawMinerals, processedMinerals, rawDiamonds, processedDiamonds;

    public Player(String name, int energy, int xp, int coins, int positionX, int positionY, int increasedBackpackDuration, int dazeTurns, int frozenTurns, int backpackCapacity, int rawMinerals, int processedMinerals, int rawDiamonds, int processedDiamonds) {
        this.name = name;
        this.energy = energy;
        this.xp = xp;
        this.coins = coins;
        this.positionX = positionX;
        this.positionY = positionY;
        this.increasedBackpackDuration = increasedBackpackDuration;
        this.dazeTurns = dazeTurns;
        this.frozenTurns = frozenTurns;
        this.backpackCapacity = backpackCapacity;
        this.rawMinerals = rawMinerals;
        this.processedMinerals = processedMinerals;
        this.rawDiamonds = rawDiamonds;
        this.processedDiamonds = processedDiamonds;
    }
}

class Board {
    char[][] grid;

    public Board(char[][] grid) {
        this.grid = grid;
    }

    @Override
    public String toString() {
        StringBuilder sb = new StringBuilder();
        for (char[] row : grid) {
            sb.append(row);
            sb.append("\n");
        }
        return sb.toString();
    }
}

class GameState {
    Player player1, player2;
    Board board;

    public GameState(String text) {
        text = text.replace("^", "\n");
        Map<String, Object> map = new HashMap<>();
        try {
            map = new ObjectMapper().readValue(text, new TypeReference<Map<String, Object>>() {});
        } catch (IOException e) {
            e.printStackTrace();
        }
        Map<String, Object> player1Map = (Map<String, Object>) map.get("player1");
        Map<String, Object> player2Map = (Map<String, Object>) map.get("player2");
        this.player1 = new Player(
            (String) player1Map.get("name"),
            (int) player1Map.get("energy"),
            (int) player1Map.get("xp"),
            (int) player1Map.get("coins"),
            (int) player1Map.get("positionX"),
            (int) player1Map.get("positionY"),
            (int) player1Map.get("increasedBackpackDuration"),
            (int) player1Map.get("dazeTurns"),
            (int) player1Map.get("frozenTurns"),
            (int) player1Map.get("backpackCapacity"),
            (int) player1Map.get("rawMinerals"),
            (int) player1Map.get("processedMinerals"),
            (int) player1Map.get("rawDiamonds"),
            (int) player1Map.get("processedDiamonds")
        );
        this.player2 = new Player(
            (String) player2Map.get("name"),
            (int) player2Map.get("energy"),
            (int) player2Map.get("xp"),
            (int) player2Map.get("coins"),
            (int) player2Map.get("positionX"),
            (int) player2Map.get("positionY"),
            (int) player2Map.get("increasedBackpackDuration"),
            (int) player2Map.get("dazeTurns"),
            (int) player2Map.get("frozenTurns"),
            (int) player2Map.get("backpackCapacity"),
            (int) player2Map.get("rawMinerals"),
            (int) player2Map.get("processedMinerals"),
            (int) player2Map.get("rawDiamonds"),
            (int) player2Map.get("processedDiamonds")
        );
        char[][] grid = new char[20][20];
        String boardString = (String) map.get("board");
        String[] rows = boardString.split("\\^");
        for (int i = 0; i < rows.length; i++) {
            grid[i] = rows[i].toCharArray();
        }
        this.board = new Board(grid);
    }
}

public class Main {
    static GameState gameState;

    public static void main(String[] args) throws Exception {
        BufferedReader br = new BufferedReader(new InputStreamReader(System.in));
        while (true) {
            String line = br.readLine().trim();
            if (!line.isEmpty()) {
                gameState = new GameState(line);
                act();
            }
            Thread.sleep(2000);
        }
    }

    static void act() {
        Player player = gameState.player2;
        Board board = gameState.board;

        int myX = player.positionX;
        int myY = player.positionY;

        if (!diamondsInBag(player)) {
            int[] nextToDiamondsPos = nextToType(board, player);
            if (nextToDiamondsPos != null) {
                System.out.println("mine " + nextToDiamondsPos[0] + " " + nextToDiamondsPos[1]);
                return;
            }
            List<int[]> closestDiamond = findDiamonds(board, player);
            if (closestDiamond != null) {
                int[] nextMoveToDiamond = nextMove(closestDiamond, player.positionX, player.positionY);
                if (nextMoveToDiamond != null) {
                    System.out.println("move " + nextMoveToDiamond[0] + " " + nextMoveToDiamond[1]);
                } else {
                    System.out.println("rest");
                }
            }
        } else {
            if (myX == 0 && myY == 11) {
                System.out.println("conv 0 diamond 0 mineral to coins, 0 diamond 0 mineral to energy, " + player.rawDiamonds + " diamond " + player.rawMinerals + " mineral to xp");
                return;
            }
            int[] nextToBase = nextToType(board, player, 'B');
            if (nextToBase != null) {
                System.out.println("move " + nextToBase[0] + " " + nextToBase[1]);
                return;
            }
            List<int[]> closestBase = findPathForTarget(board, player, new int[]{0, 11});
            if (closestBase != null) {
                int[] nextMoveToBase = nextMove(closestBase, player.positionX, player.positionY);
                if (nextMoveToBase != null) {
                    System.out.println("move " + nextMoveToBase[0] + " " + nextMoveToBase[1]);
                } else {
                    System.out.println("rest");
                }
            }
        }
    }

    static boolean diamondsInBag(Player player) {
        return player.rawDiamonds > 0;
    }

    static int[] nextToType(Board board, Player player) {
        int myX = player.positionX;
        int myY = player.positionY;
        for (int i = -1; i <= 1; i++) {
            for (int j = -1; j <= 1; j++) {
                if (i == 0 && j == 0) continue;
                if (!(i == 0 || j == 0)) continue;
                if (!(myX + i >= 0 && myX + i < board.grid.length && myY + j >= 0 && myY + j < board.grid[0].length)) continue;
                if (board.grid[myX + i][myY + j] == 'D') return new int[]{myX + i, myY + j};
            }
        }
        return null;
    }

    static boolean onType(Board board, Player player, char type) {
        int myX = player.positionX;
        int myY = player.positionY;
        return board.grid[myX][myY] == type;
    }

    static List<int[]> findDiamonds(Board board, Player player) {
        int myX = player.positionX;
        int myY = player.positionY;
        List<int[]> targetPos = new ArrayList<>();
        for (int i = 0; i < board.grid.length; i++) {
            for (int j = 0; j < board.grid[i].length; j++) {
                if (board.grid[i][j] == 'D') targetPos.add(new int[]{i, j});
            }
        }
        List<List<int[]>> paths = new ArrayList<>();
        for (int[] pos : targetPos) {
            List<int[]> path = findPathForTarget(board, player, pos);
            if (path != null) paths.add(path);
        }
        if (!paths.isEmpty()) return paths.get(0);
        return null;
    }

    static List<int[]> findPathForTarget(Board board, Player player, int[] targetPos) {
        int myX = player.positionX;
        int myY = player.positionY;
        Queue<int[]> queue = new LinkedList<>();
        Set<String> visited = new HashSet<>();
        queue.add(new int[]{myX, myY});
        while (!queue.isEmpty()) {
            int[] pos = queue.poll();
            if (pos[0] == targetPos[0] && pos[1] == targetPos[1]) return reconstructPath(visited, targetPos);
            if (visited.contains(Arrays.toString(pos))) continue;
            visited.add(Arrays.toString(pos));
            if (board.grid[pos[0]][pos[1]] != 'E' && board.grid[pos[0]][pos[1]] != '2') continue;
            int[][] directions = {{0, 1}, {0, -1}, {1, 0}, {-1, 0}};
            for (int[] direction : directions) {
                int newX = pos[0] + direction[0];
                int newY = pos[1] + direction[1];
                if (!(newX >= 0 && newX < board.grid.length && newY >= 0 && newY < board.grid[0].length)) continue;
                queue.add(new int[]{newX, newY});
            }
        }
        return null;
    }

    static int[] nextMove(List<int[]> path, int myX, int myY) {
        if (path == null || path.isEmpty()) return null;
        int[] firstStep = path.get(0);
        if (path.size() == 1) return firstStep;
        int[] directionFirstStep = {firstStep[0] - myX, firstStep[1] - myY};
        for (int i = 1; i < path.size(); i++) {
            int[] nextStep = path.get(i);
            int[] directionNextStep = {nextStep[0] - path.get(i - 1)[0], nextStep[1] - path.get(i - 1)[1]};
            if (!Arrays.equals(directionNextStep, directionFirstStep)) return path.get(i - 1);
        }
        return path.get(path.size() - 1);
    }

    static List<int[]> reconstructPath(Set<String> visited, int[] targetPos) {
        List<int[]> path = new ArrayList<>();
        path.add(targetPos);
        while (true) {
            int[] currentPos = path.get(path.size() - 1);
            int currentX = currentPos[0];
            int currentY = currentPos[1];
            if (visited.contains(Arrays.toString(new int[]{currentX, currentY}))) {
                visited.remove(Arrays.toString(new int[]{currentX, currentY}));
                int[][] directions = {{0, 1}, {0, -1}, {1, 0}, {-1, 0}};
                for (int[] direction : directions) {
                    int newX = currentX + direction[0];
                    int newY = currentY + direction[1];
                    if (visited.contains(Arrays.toString(new int[]{newX, newY}))) {
                        path.add(new int[]{newX, newY});
                        break;
                    }
                }
            } else break;
        }
        Collections.reverse(path);
        return path;
    }
}
