using System;
using System.Collections.Generic;
using System.Linq;

namespace q2_tdm_team_randomizer
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<int, string> players = new Dictionary<int, string>();
            players.Add(1, "chacal");
            players.Add(2, "lipe");
            players.Add(3, "hitman");
            players.Add(4, "ogrow");
            players.Add(5, "toxic");
            players.Add(6, "kolt");
            players.Add(7, "nhd");
            players.Add(8, "koda");
            players.Add(9, "hazael");
            players.Add(10, "vernom");
            players.Add(11, "kim");
            players.Add(12, "rivi");

            GenerateTeams(4, players);

        }

        static void GenerateTeams(int teamSize, Dictionary<int, string> playerList)
        {
            List<Player> players = new List<Player>();
            int currentTier = 1;
            int round = 0;
            bool isNewTier = false;

            foreach (var item in playerList)
            {
                isNewTier = (round != 0 && (round % teamSize) == 0);

                if (isNewTier)
                {
                    currentTier++;
                }

                players.Add(new Player() { Nick = item.Value, Rank = item.Key, Tier = currentTier });

                round++;
            }

            List<Team> teams = new List<Team>();
            
            foreach (var player in players.Where(a => a.Tier == 1).OrderBy(b => b.Rank))
            {
                Team team = new Team();
                team.Name = string.Format("Team{0}", player.Rank);
                team.Players.Add(player);
                currentTier = 2;

                for (int i = 0; (i < teamSize -1) && (teams.SelectMany(a => a.Players).Count() < players.Count); i++)
                {
                    var tier = players.Where(a => a.Tier == currentTier);
                    Player newPlayer = tier.OrderBy(a => Guid.NewGuid()).First();;

                    while (team.Players.Where(a => a.Nick == newPlayer.Nick).FirstOrDefault() != null
                        || teams.SelectMany(a => a.Players).Where(b => b.Nick == newPlayer.Nick).FirstOrDefault() != null)
                    {
                        newPlayer = tier.OrderBy(a => Guid.NewGuid()).First();
                    }

                    team.Players.Add(newPlayer);

                    if (currentTier < players.Max(a => a.Tier))
                        currentTier++;
                }

                teams.Add(team);
            }
        }
    }

    class Player
    {
        public string Nick { get; set; }
        public int Rank { get; set; }
        public int Tier { get; set; }
    }

    class Team
    {
        public Team()
        {
            Players = new List<Player>();
        }

        public string Name { get; set; }
        public List<Player> Players { get; set; }
    }
}
