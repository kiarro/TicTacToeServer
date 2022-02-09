# Tic tac toe Server app

Simple HTTP server that allow to play tic tac toe against computer.

## Server

HTTP Server created on low level with TcpClient class.

Server parse requests and compile responses without using special libraries.

CServer class do all connection things. Class CClient processes and responds to incoming connections.

## Game

Game logic is in the Game_TTT class. 

Computer use sum-base algorithm to choose best cell to place token.

Several games might be played parallel.
