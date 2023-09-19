function startGame() {
    let userFieldElement = document.getElementById('user_pool');
    let compFieldElement = document.getElementById('comp_pool');
    let userConsole = document.getElementsByClassName("user_console")[0];
    let compConsole = document.getElementsByClassName("comp_console")[0];
    let buttonPlace = document.getElementsByClassName('game_buttons')[0];
    let button = document.getElementsByClassName('buttons')[0];
    let reButton = document.createElement('button');
    button.remove()

    reButton.className = 'buttons';
    reButton.innerHTML = 'Заново'
    reButton.onclick = () => {
        userFieldElement.innerHTML = '';
        compFieldElement.innerHTML = '';
        userConsole.value = '';
        compConsole.value = '';
        startGame()
    }
    buttonPlace.appendChild(reButton);

    let userField = fillField();
    let compField = fillField();

    for (let i = 0; i < 10; i++) {
        for (let j = 0; j < 10; j++) {
            let seeDiv = document.createElement('div');
            seeDiv.className = 'sea_cell comp_cells';
            compFieldElement.appendChild(seeDiv)
            if (userField[i][j] === 1) {
                let shipDiv = document.createElement('div');
                shipDiv.className = 'ship_cell user_cells';
                userFieldElement.appendChild(shipDiv)
            } else {
                let seeDiv = document.createElement('div');
                seeDiv.className = 'sea_cell user_cells';
                userFieldElement.appendChild(seeDiv)
            }
        }
    }

    let activeCell = document.getElementsByClassName('comp_cells');
    let letters = ["А", "Б", "В", "Г", "Д", "Е", "Ж", "З", "И", "К"]
    for (let i = 0; i < activeCell.length; i++){
        activeCell[i].addEventListener('click', () => {
            let cellIndex = i;
            let x = cellIndex % 10 + 1;
            let y = Math.floor(cellIndex / 10);
            if (compField[y][x] === 3) {}
            else {
                userConsole.value += " Ваш ход: \n"
                if (compField[y][x] === 1) {
                    activeCell[i].style.background = 'red';
                    userConsole.value += "\t" + letters[x-1] + (y+1) + " - Попал! \n"
                    compField[y][x] = 3;
                } else {
                    activeCell[i].style.background = 'black';
                    userConsole.value += "\t" + letters[x-1] + (y+1) + " - Мимо \n"
                    compField[y][x] = 3;
                }
                setTimeout(() => {  compStep(userField, letters, compConsole); }, 1000);
            }
        })
    }
}

function compStep(userField, letters, compConsole) {
    let activeCell = document.getElementsByClassName('user_cells');
    let x = getRandomNumber();
    let y = getRandomNumber();
    let i = (y*10+x);
    if (userField[y][x] === 3) {
        compStep(userField, letters, compConsole);
        return;
    }
    compConsole.value += " Ход противника: \n"
    if (userField[y][x] === 1) {
        activeCell[i].style.background = 'red';
        userField[y][x] = 3;
        compConsole.value += "\t" + letters[x] + (y+1) + " - Попал! \n"
        setTimeout(() => {  compStep(userField, letters, compConsole); }, 1000);
    } else {
        activeCell[i].style.background = 'black';
        userField[y][x] = 3;
        compConsole.value += "\t" + letters[x] + (y+1) + " - Мимо \n"
    }
}

class Ship {
    constructor(size) {
        this.size = size;
    }
    getSize() {
        return this.size;
    }
}

function fillField() {
    let field = [];
    let ships = [new Ship(1), new Ship(1), new Ship(1), new Ship(1), new Ship(2),
        new Ship(2), new Ship(2), new Ship(3), new Ship(3), new Ship(4)]

    for (let i = 0; i < 10; i++) {
        field[i] = new Array(10).fill(0);
    }

    while (ships.length !== 0) {
        let ship = ships.pop();
        let x = getRandomNumber();
        let y = getRandomNumber();
        if (field[y][x] === 0) {
            let size = ship.getSize();
            if (x + size < 10 && field[y][x + size] === 0)
                for (let i = 0; i < size; i++){
                    field[y][x + i] = 1;
                    field = makeDeadZone(field, x + i, y)
                }
            else if (x - size >= 0 && field[y][x - size] === 0)
                for (let i = 0; i < size; i++) {
                    field[y][x - i] = 1;
                    field = makeDeadZone(field, x - i, y)
                }
            else if (y + size < 10 && field[y + size][x] === 0)
                for (let i = 0; i < size; i++) {
                    field[y + i][x] = 1;
                    field = makeDeadZone(field, x, y + i)
                }
            else if (y - size >= 0 && field[y - size][x] === 0)
                for (let i = 0; i < size; i++) {
                    field[y - i][x] = 1;
                    field = makeDeadZone(field, x, y - i)
                }
            else
                ships.push(ship);
        } else {
          ships.push(ship);
        }
    }
    return field;
}

function getRandomNumber() {
    return Math.floor(Math.random() * 10);
}

function makeDeadZone(field, x, y) {
    if (y + 1 < 10 && field[y+1][x] === 0) field[y+1][x] = 2;
    if (y - 1 >= 0 && field[y-1][x] === 0) field[y-1][x] = 2;
    if (x + 1 < 10 && field[y][x+1] === 0) field[y][x+1] = 2;
    if (x - 1 >= 0 && field[y][x-1] === 0) field[y][x-1] = 2;

    if (x + 1 < 10 && y + 1 < 10) field[y+1][x+1] = 2;
    if (x + 1 < 10 && y - 1 >= 0) field[y-1][x+1] = 2;
    if (x - 1 >= 0 && y + 1 < 10) field[y+1][x-1] = 2;
    if (x - 1 >= 0 && y - 1 >= 0) field[y-1][x-1] = 2;

    return field;
}