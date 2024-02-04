# Introduction
This is a discord bot that is designed to interact with the Palworld game server. It is designed to provide a way for players to interact with the game server without having to be in the game.

Currently, the bot is under development and is not yet ready for public use.

# Features
The bot is designed to provide the following features:
- You can show the server information using the discord command: `/palworld_online`
- You can show the server players using the discord command: `/palworld_info`

The following features can only be invoked by a discord user, that has been added to the bot's admin list:
- You can save the server state using the discord command: `/palworld_save`
- You can shutdown the server using the discord command: `/palworld_shutdown <seconds> <reason>`
- You can kick a player using the discord command: `/palworld_kick <player_name>`
- You can ban a player using the discord command: `/palworld_ban <player_name> <reason>`
- You can broadcast a message to the server using the discord command: `/palworld_broadcast <message>`

# Usage
If you want to run the bot, you can do so by following the steps below.

Initialize the app settings with the following values:

```json
{
  "Discord": {
    "Token": "YOUR BOT TOKEN",
    "Admins": [ <id_1>, <id_2>, <id_3>, ... ]
  },
  "Rcon": {
    "Host": "localhost",
    "Port": 25575,
    "Password": "<Password>"
  }
}
```

Then run the app.

## Using docker
You can run the bot using docker. Make sure to specify the correct host address, and that the bot has access via RCON to the palworld server.

**Use these commands**

Build the image:
```
docker build -t saenatwork/palworld-discord-bot-net:latest \
             -f Palworld.Discord.Bot.Net.App/Dockerfile .
```

Then run the container with the following command:
```
docker run --link <palworld-docker-container-name> \
           -it \
           -e Bot__Token=<YOUR_TOKEN> \
           -e Rcon__Host=<IP_ADDRESS_OF_PALWORLD_SERVER> \
           -e Rcon__Port=<YOUR_RCON_PORT> \
           -e Rcon__Password=<YOUR_RCON_PASSWORD> \
           saenatwork/palworld-discord-bot-net:latest
```

Note: You can leave out the `--link` parameter if you are using a different method to connect the bot to the palworld server.