using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Steam.Models.SteamCommunity;
using SteamWebAPI2.Interfaces;
using SteamWebAPI2.Utilities;

namespace GameLauncherCloud_Client
{
    public class SteamAPIHandler
    {
        private const string PathToTheKey = @"./Resources/steamAPI.txt";
        private const string PathToTheID = @"./Resources/steamID.txt";
        private PlayerService steamUser;
        private SteamUserStats steamStats;
        private string apiKey;
        private ulong userId;

        public SteamAPIHandler()
        {
        }

        public bool Start()
        {
            // Load the api key from the file.
            if (!File.Exists(PathToTheKey))
            {
                return false;
            }
            apiKey = File.ReadAllText(PathToTheKey).Trim();
            if (!File.Exists(PathToTheID))
            {
                return false;
            }
            string playerIdText = File.ReadAllText(PathToTheID).Trim();
            // todo return if failed
            if (!ulong.TryParse(playerIdText, out userId))
            {
                return false;
            }

            steamUser = new PlayerService(apiKey);
            steamStats = new SteamUserStats(apiKey);
            return true;
        }

        public async Task<List<Game>> GetAllGames()
        {
            // Fetch all the games of the user
            List<Game> games = new List<Game>();
            var allGamesTask = steamUser.GetOwnedGamesAsync(userId, true, true);

            // Fetch all the recents games for the shared libraries
            var allRecentGamesTask = steamUser.GetRecentlyPlayedGamesAsync(userId);
            await allRecentGamesTask;
            await allGamesTask;

            var allGames = allGamesTask.Result;
            var allRecentGames = allRecentGamesTask.Result;
            var recentGamesList = allRecentGames.Data.RecentlyPlayedGames.ToList();

            foreach (var ownedGame in allGames.Data.OwnedGames)
            {
                games.Add(ConstructGame(ownedGame));
                recentGamesList.RemoveAll(model => model.AppId == ownedGame.AppId);
            }

            foreach (var notOwnedGamePlayed in recentGamesList)
            {
                games.Add(ConstructGame(notOwnedGamePlayed));
            }

            return games;
        }

        private static Game ConstructGame(OwnedGameModel gameToConstruct)
        {
            return new Game(gameToConstruct.Name, @"steam://rungameid/" + gameToConstruct.AppId, new GameTime(Convert.ToInt32(gameToConstruct.PlaytimeForever.TotalMinutes)), true);
        }

        private static Game ConstructGame(RecentlyPlayedGameModel gameToConstruct)
        {
            return new Game(gameToConstruct.Name, @"steam://rungameid/" + gameToConstruct.AppId, new GameTime(Convert.ToInt32(gameToConstruct.PlaytimeForever)), true);
        }
    }
}