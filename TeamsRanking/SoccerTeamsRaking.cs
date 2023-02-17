namespace TeamsRanking
{
    public class SoccerTeamRanking
    {
        private SoccerTeam[] teams;

        public SoccerTeamRanking()
        {
            teams = new SoccerTeam[0];
        }

        public void Add(SoccerTeam team)
        {
            Array.Resize(ref teams, teams.Length + 1);
            teams[teams.Length - 1] = team;
        }

        public int TeamsNumber()
        {
            return teams.Length;
        }

        public SoccerTeam TeamAtPosition(int position)
        {
            return teams[position];
        }

        public int PositionOf(SoccerTeam team)
        {
            return Array.IndexOf(teams, team);
        }

        public void Update(SoccerTeam homeTeam, SoccerTeam awayTeam, int homeTeamScore, int awayTeamScore)
        {
            if (homeTeamScore > awayTeamScore)
            {
                homeTeam.AddPoints(3);
            }
            else if (awayTeamScore > homeTeamScore)
            {
                awayTeam.AddPoints(3);
            }
            else
            {
                homeTeam.AddPoints(1);
                awayTeam.AddPoints(1);
            }
               
            BubbleSort();
        }

        private void BubbleSort()
        {
            SoccerTeam temp;
            bool isNotSorted = true;
            while (isNotSorted)
            {
                isNotSorted = false;
                for (int i = 0; i < teams.Length - 1; i++)
                {
                    if (teams[i].ComparePoints(teams[i + 1]))
                    {
                        temp = teams[i + 1];
                        teams[i + 1] = teams[i];
                        teams[i] = temp;
                        isNotSorted = true;
                    }
                }
            }
        }

    }
}
