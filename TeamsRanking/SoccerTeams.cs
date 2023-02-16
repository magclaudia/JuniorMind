using System;

namespace TeamsRanking
{
    public class SoccerTeams
    {
        private readonly string teamName;
        private int score;
        public SoccerTeams(string teamName, int score)
        {
            this.teamName = teamName;
            this.score = score;
        }

        public void UpdateScore(int newScore)
        {
            score += newScore;
        }

        public bool CompareScoreTo(SoccerTeams team2)
        {
            if (team2 == null)
            {
                return false;
            }

            return score < team2.score;
        }
    }
}
