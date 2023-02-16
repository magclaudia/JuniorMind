using System;

namespace TeamsRanking
{
    public class SoccerTeamsRanking
    {
        private SoccerTeams[] teams;
        public SoccerTeamsRanking()
        {
            teams = new SoccerTeams[0];
        }

        public void AddNewTeams(SoccerTeams team)
        {
            Array.Resize(ref teams, teams.Length + 1);
            teams[teams.Length - 1] = team;
        }

        public int GetTotalNumberOfTeams()
        {
            return teams.Length;
        }

        public SoccerTeams GetTeamByInputPosition(int position)
        {
            return teams[position];
        }

        public int GetPositionOfRequiredTeam(SoccerTeams team)
        {
            return Array.IndexOf(teams, team);
        }

        public void UpdateScoreAfterMatch(SoccerTeams firstTeam, SoccerTeams secondTeam, int firstTeamScore, int secondTeamScore)
        {
            if (firstTeamScore > secondTeamScore)
            {
                firstTeam.UpdateScore(1);
            }
            else if (secondTeamScore > firstTeamScore)
            {
                secondTeam.UpdateScore(1);
            }
            else
            {
                firstTeam.UpdateScore(1);
                secondTeam.UpdateScore(1);
            }
               
            BubbleSort();
        }

        private void BubbleSort()
        {
            SoccerTeams temp;
            bool isSorted = true;
            while (isSorted)
            {
                isSorted = false;
                for (int i = 0; i < teams.Length - 1; i++)
                {
                    if (teams[i].CompareScore(teams[i + 1]))
                    {
                        temp = teams[i + 1];
                        teams[i + 1] = teams[i];
                        teams[i] = temp;
                        isSorted = true;
                    }
                }
            }
        }

    }
}
