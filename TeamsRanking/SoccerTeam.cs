using System;

namespace TeamsRanking
{
    public class SoccerTeam
    {
        private readonly string teamName;
        private int points;

        public SoccerTeam(string teamName, int points)
        {
            this.teamName = teamName;
            this.points = points;
        }

        public void AddPoints(int newPoints)
        {
            points += newPoints;
        }

        public bool ComparePoints(SoccerTeam otherTeam)
        {
            if (otherTeam == null)
            {
                return false;
            }

            return points < otherTeam.points;
        }
    }
}
