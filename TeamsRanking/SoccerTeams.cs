using System;

namespace TeamsRanking
{
    public class SoccerTeams
    {
        private readonly string teamName;
        private int points;
        public SoccerTeams(string teamName, int points)
        {
            this.teamName = teamName;
            this.points = points;
        }

        public void UpdateScore(int newPoints)
        {
            points += newPoints;
        }

        public bool CompareScore(SoccerTeams team2)
        {
            if (team2 == null)
            {
                return false;
            }

            return points < team2.points;
        }
    }
}
