const fs = require('fs');
const readline = require('readline');
const { EOL } = require('os');

const logger = fs.createWriteStream('./logs_raw_javascript.txt', { flags: 'a' });

class Player {
    constructor(name, energy, xp, coins, position, increased_backpack_duration, daze_turns, frozen_turns, backpack_capacity, raw_minerals, processed_minerals, raw_diamonds, processed_diamonds) {
        this.name = name;
        this.energy = energy;
        this.xp = xp;
        this.coins = coins;
        this.position = position;
        this.increased_backpack_duration = increased_backpack_duration;
        this.daze_turns = daze_turns;
        this.frozen_turns = frozen_turns;
        this.backpack_capacity = backpack_capacity;
        this.raw_minerals = raw_minerals;
        this.processed_minerals = processed_minerals;
        this.raw_diamonds = raw_diamonds;
        this.processed_diamonds = processed_diamonds;
    }
}

class Board {
    constructor(grid) {
        this.grid = grid;
    }

    toString() {
        return this.grid.map(row => row.join('')).join(EOL);
    }
}

class GameState {
    constructor(text) {
        text = text.replace("^", EOL);
        const dic = JSON.parse(text);
        this.player1 = new Player(...Object.values(dic['player1']));
        this.player2 = new Player(...Object.values(dic['player2']));
        this.board = new Board(dic['board']);
    }
}

function act(line) {
    console.log("rest");
}

const rl = readline.createInterface({
    input: process.stdin,
    output: process.stdout,
    terminal: false
});

rl.on('line', (line) => {
    logger.write(line + EOL);
    act(line);
});
