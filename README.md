# Unity TicTacToe

A TicTacToe game made in Unity.

Unity Version: 2021.3.14f1

<img src="https://user-images.githubusercontent.com/49583943/227832958-4d60c1cf-5321-47e8-961d-727bfb642feb.png" alt="tictactoe_screenshot_1" width=375 /> <img src="https://user-images.githubusercontent.com/49583943/227832969-7c4fbbe5-d338-44bf-85e2-91125fe1e3c5.png" alt="tictactoe_screenshot_3" width=375 />

## How to play

Tap the screen to start game, player pick `O`, AI pick `X` as pawn.

There are three types: **easy**, **mid**, and **hard** of difficulty, increases when player win.

**Refresh:** tap the `O` in the upper left corner of the screen.

**Quit:** tap the `X` in the upper right corner of the screen or press `Esc`.

## Script Content

There are 4 folder in scripts content. 

![2023-03-27_114320](https://user-images.githubusercontent.com/49583943/227836372-cf563a71-5160-4021-b6d0-0303b07d0937.png)

### Definition

Some definitions of **enum** and **constant**.

### GameMain

TicTacToe: game procedure controller.

TicTacToeMiniMax: a partial class of `TicTacToe`, include minimax search algorithm.

TicTacToeWinnerChecker: a partial class of `TicTacToe`, include the functionality of board state check up.

### Grid

Object `Grid` and its data.

### UI

The view script for each user interface.

## TODO

- [ ] Implement minimax parallel for more efficient computing on *N x N* board
- [ ] Find a better ways to weaken search algorithms.
- [ ] Search in the coroutine.
- [ ] Lock/Hide the grid on a larger board to create more interesting game.

