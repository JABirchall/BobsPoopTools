namespace GhostTool
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public class GhostStats
    {
        [Category("Player K/D Stats")]
        public int Assists { get; set; }

        [Category("Objective Stats")]
        public int Captures { get; set; }

        [Category("Objective Stats")]
        public int Confirmed { get; set; }

        [Category("Player K/D Stats")]
        public int Deaths { get; set; }

        [Category("Objective Stats")]
        public int Defends { get; set; }

        [Category("Objective Stats")]
        public int Defuses { get; set; }

        [Category("Objective Stats")]
        public int Denied { get; set; }

        [Category("Objective Stats")]
        public int Destructions { get; set; }

        [Category("Customer Information")]
        public string Gamertag { get; set; }

        [Category("Played Game Stats")]
        public int GamesPlayed { get; set; }

        [Category("Weapon Shots Stats")]
        public int Headshots { get; set; }

        [Category("Weapon Shots Stats")]
        public int Hits { get; set; }

        [Category("Player Stats")]
        public int HoursPlayed { get; set; }

        [RefreshProperties(RefreshProperties.All), Category("Player K/D Stats")]
        public int Kills { get; set; }

        [Category("Player K/D Stats")]
        public int KillStreak { get; set; }

        [Category("Played Game Stats")]
        public int Losses { get; set; }

        [Category("Player Stats")]
        public int MinutesPlayed { get; set; }

        [Category("Weapon Shots Stats")]
        public int Misses { get; set; }

        [Category("Objective Stats")]
        public int Plants { get; set; }

        [Category("Objective Stats")]
        public int Returns { get; set; }

        [Category("Player Stats")]
        public int Score { get; set; }

        [Category("Customer Information")]
        public int SquadPoints { get; set; }

        [Category("Player K/D Stats")]
        public int Suicides { get; set; }

        [Category("Played Game Stats")]
        public int Ties { get; set; }

        [Category("Weapon Shots Stats")]
        public int TotalShots { get; set; }

        [Category("Played Game Stats")]
        public int Wins { get; set; }

        [Category("Played Game Stats")]
        public int WinStreak { get; set; }
    }
}

