# Battleship Protocol (BSP)

This is an implementation of a BSP Client and Server as described in the specification document. BSP runs over port 47374. The client can also auto-discover the server if they are on the same subnet. Briefly, it does this using UDP broadcast packets. This has only been tested on Windows.

## Requirements

 - Visual Studio 2019

 - .NET Core 3.1

## Building

1. Open Battleship.sln in Visual Studio 2019.

2. Go to Build > Build Solution to build.

3. Find the binaries for the server in `Battleship.Server/bin/Debug/netcoreapp3.1/`.

4. Find the binaries for the client in `Battleship.Client/bin/Debug/netcoreapp3.1/`.

## Run

(Note: These are not standalone .exe's.)

1. Run `Battleship.Server.exe 127.0.0.1` or with any other IP or hostname you wish to bind to. This will also default to localhost.

2. Run at least two instances of `Battleship.Client.exe`. The clients should auto-discover the server. As a fallback, you can always specify the server IP address as a command line argument.

3. Log in on each of the clients. Use one of these four usernames: `jason`, `sam`, `alice`, `bob`. Password is always `password`. Note the same user cannot be logged in more than once at the same time.

4. Follow the prompts to enter a match and play the game.

## Analysis

Both the server and client are incredibly conservative. Both will terminate immediately if they encounter an invalid message or an invalid message received for the current conversation state. Given this, I can't imagine this would be easily cracked through fuzzing. The only viable path I can see would be actually playing the game. During development, my client could send arbitrary messages to the server. My server would frequently disconnected due to this light form of fuzzing.