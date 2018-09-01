﻿using Discord.WebSocket;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace BotListAPI
{
    public class ListAPI
    {

        public readonly string Name;
        public readonly string Website;
        public string API;
        public readonly ListOwner Owner;
        public bool Enabled = true;
        private HttpClient Http;
        private ListClient Client;
        private ListType Type;
        public ListAPI(ListClient client, ListType type)
        {
            Client = client;
            Type = type;
            switch (Type)
            {
                case ListType.DiscordBots:
                    Name = "Discord Bots";
                    Website = "https://bots.discord.pw";
                    API = Website + "/api/bots/{0}/stats";
                    Owner = new ListOwner("meew0#9811", 66237334693085184);
                    break;
                case ListType.DiscordBotList:
                    Name = "Discord Bot List";
                    Website = "https://discordbots.org";
                    API = Website + "/api/bots/{0}/stats";
                    Owner = new ListOwner("Oliy#0330", 129908908096487424);
                    break;
                case ListType.DiscordBotListv2:
                    Name = "Discord Bot List v2";
                    Website = "https://discordbotlist.com";
                    API = Website + "/api/bots/{0}/stats";
                    Owner = new ListOwner("luke#0123", 149505704569339904);
                    break;
                case ListType.BotsForDiscord:
                    Name = "Bots For Discord";
                    Website = "https://botsfordiscord.com";
                    API = Website + "/api/v1/bots/{0}";
                    Owner = new ListOwner("Habchy#1665", 162780049869635584);
                    break;
                case ListType.Carbonitex:
                    Name = "Carbonitex";
                    Website = "https://www.carbonitex.net";
                    API = Website + "";
                    Owner = new ListOwner("jet#9999", 228290260239515649);
                    break;
                case ListType.BotListSpace:
                    Name = "Bot List Space";
                    Website = "https://botlist.space";
                    API = Website + "/api/bots/{0}";
                    Owner = new ListOwner("PassTheMayo#1281", 153173425844781056);
                    break;
                case ListType.BotsOnDiscord:
                    Name = "Bots On Discord";
                    Website = "https://bots.ondiscord.xyz";
                    API = Website + "/bot-api/bots/{0}/guilds";
                    Owner = new ListOwner("Brussell#0660", 95286900801146880);
                    break;
                case ListType.DiscordBotWorld:
                    Name = "Discord Bot World";
                    Website = "https://discordbot.world";
                    API = Website + "/api/bot/{0}/stats";
                    Owner = new ListOwner("Tetrabyte#0001", 156114103033790464);
                    break;
                case ListType.DiscordBotsGroup:
                    Name = "Discord Bots Group";
                    Website = "https://discordbots.group";
                    API = Website + "";
                    Owner = new ListOwner("DetectiveHuman#0767", 423220263161692161);
                    break;
                case ListType.DiscordListApp:
                    Name = "Discord List App";
                    Website = "https://bots.discordlist.app";
                    API = Website + "/api/bot/{0}/stats";
                    Owner = new ListOwner("Auxim#0001", 66166172835385344);
                    break;
                case ListType.DiscordServices:
                    Name = "Discord Services";
                    Website = "http://discord.services";
                    API = Website + "/api/bots/{0}";
                    Owner = new ListOwner("Tails#0420", 472573259108319237);
                    break;
                case ListType.DivineBotList:
                    Name = "Divine Bot List";
                    Website = "https://divinediscordbots.com";
                    API = Website + "/api/bots/{0}/stats ";
                    Owner = new ListOwner("Sworder#1234", 240508683455299584);
                    break;
            }
        }

        private string GetToken()
        {
            switch (Type)
            {
                case ListType.DiscordBots:
                    return Client.Config.DiscordBots;
                case ListType.DiscordBotList:
                    return Client.Config.DiscordBotList;
                case ListType.DiscordBotListv2:
                    return Client.Config.DiscordBotListv2;
                case ListType.BotsForDiscord:
                    return Client.Config.BotsForDiscord;
                case ListType.Carbonitex:
                    return Client.Config.Carbonitex;
                case ListType.BotListSpace:
                    return Client.Config.BotListSpace;
                case ListType.BotsOnDiscord:
                    return Client.Config.BotsOnDiscord;
                case ListType.DiscordBotWorld:
                    return Client.Config.DiscordBotWorld;
                case ListType.DiscordBotsGroup:
                    return Client.Config.DiscordBotsGroup;
                case ListType.DiscordListApp:
                    return Client.Config.DiscordListApp;
                case ListType.DiscordServices:
                    return Client.Config.DiscordServices;
                case ListType.DivineBotList:
                    return Client.Config.DivineBotList;
            }
            return "";
        }

        public bool Post(bool Debug = false)
        {
            if (Http == null)
            {
                Http = new HttpClient();
                Http.DefaultRequestHeaders.Add("Authorization", GetToken());
                API = API.Replace("{0}", Client.Discord.CurrentUser.Id.ToString());
                Http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }
            try
            {
                string Json;
                switch (Type)
                {
                    case ListType.BotsOnDiscord:
                        Json = "{ \"guildCount\": 0 }".Replace("0", Client.Discord.Guilds.Count.ToString());
                        break;
                    case ListType.DiscordBotListv2:
                        Json = "{ \"guilds\": 0 }".Replace("0", Client.Discord.Guilds.Count.ToString());
                        break;
                    case ListType.DiscordBotWorld:
                        Json = "{ \"guild_count\": 0 }".Replace("0", Client.Discord.Guilds.Count.ToString());
                        break;
                    default:
                        Json = "{ \"server_count\": 0 }".Replace("0", Client.Discord.Guilds.Count.ToString());
                        break;
                }
                StringContent Content = new StringContent(Json, Encoding.UTF8, "application/json");
                Http.PostAsync(API, Content);
                if (Debug)
                    Client.Log("Successfully posted server count to " + Name);
            }
            catch (Exception ex)
            {
                if (Debug)
                    Client.Log($"Error could not post server count to {Name}, {ex.Message}");
                return false;
            }
            return true;
        }
    }
    public class ListOwner
    {
        public ListOwner(string name, ulong id)
        {
            Name = name;
            Id = id;
        }
        public string Name;
        public ulong Id;
    }
}
