# ShipGeneratorDisocrdBot
This is a discord bot to generate ships between two OCs, made for some friends with a DnD one shots server. Feel free to download it, set it up, clear out the db and run it for your own server!



## Setup and Running
1) create a discord bot account, you'll need the bot token
https://discordpy.readthedocs.io/en/latest/discord.html

2) replace the text in bottoken.txt with your discord user's bot token.

3) Mess with the file path for the connection string so it points at the DB file. (hopefully this will just involve editing a text file rather than poking around the code before too long.)

4) compile and run the bot. (Hopefully this will also be simplified in the future to just running an exe)




## Useful links
https://docs.stillu.cc/index.html Discord.Net documentation


https://discord.com/developers/docs/intro Discord API docs


https://www.connectionstrings.com/sqlite/ SQLite connection string examples


https://sqlitebrowser.org/ SQLite Browser, the tool I used for modifying the database


## Main libraries used
Discord .NET
C# + .NET Core 3.1
Entity Framework Core
SQLite
