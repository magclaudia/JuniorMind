using Xunit;

namespace TeamsRanking
{
    public class SoccerTeamsRankingFacts
    {
        [Fact]
        public void AddTeamToRanking()
        {
            SoccerTeams team1 = new SoccerTeams("Team1", 1);
            SoccerTeamsRanking teamsRanking = new SoccerTeamsRanking();
            teamsRanking.AddNewTeams(team1);
            Assert.Equal(team1, teamsRanking.GetTeams()[0]);
        }

        [Fact]
        public void AddMultipleTeamsToRanking()
        {
            SoccerTeams team1 = new SoccerTeams("Team1", 1);
            SoccerTeams team2 = new SoccerTeams("Team2", 4);
            SoccerTeams team3 = new SoccerTeams("Team3", 2);
            SoccerTeams team4 = new SoccerTeams("Team4", 3);
            SoccerTeams[] soccerTeams = new SoccerTeams[] { team1, team2, team3, team4};
            SoccerTeamsRanking teamsRanking = new SoccerTeamsRanking();
            teamsRanking.AddNewTeams(team1);
            teamsRanking.AddNewTeams(team2);
            teamsRanking.AddNewTeams(team3);
            teamsRanking.AddNewTeams(team4);
            Assert.Equal(soccerTeams, teamsRanking.GetTeams());
        }

        [Fact]
        public void CorrectlyReturnATeamByInputPosition()
        {
            SoccerTeams team1 = new SoccerTeams("Team1", 1);
            SoccerTeams team2 = new SoccerTeams("Team2", 4);
            SoccerTeams team3 = new SoccerTeams("Team3", 2);
            SoccerTeams team4 = new SoccerTeams("Team4", 3);
            SoccerTeamsRanking teamsRanking = new SoccerTeamsRanking();
            teamsRanking.AddNewTeams(team1);
            teamsRanking.AddNewTeams(team2);
            teamsRanking.AddNewTeams(team3);
            teamsRanking.AddNewTeams(team4);
            Assert.Equal(team3, teamsRanking.GetTeamByInputPosition(2));
        }

        [Fact]
        public void CorrectlReturnATeamPosition()
        {
            SoccerTeams team1 = new SoccerTeams("Team1", 1);
            SoccerTeams team2 = new SoccerTeams("Team2", 4);
            SoccerTeams team3 = new SoccerTeams("Team3", 2);
            SoccerTeams team4 = new SoccerTeams("Team4", 3);
            SoccerTeamsRanking teamsRanking = new SoccerTeamsRanking();
            teamsRanking.AddNewTeams(team1);
            teamsRanking.AddNewTeams(team2);
            teamsRanking.AddNewTeams(team3);
            teamsRanking.AddNewTeams(team4);
            Assert.Equal(2, teamsRanking.GetPositionOfRequiredTeam(team3));
        }

        [Fact]
        public void UpdatesPointsForWinningTeamAfterAMatchIfResultAreDifferent()
        {
            SoccerTeams team1 = new SoccerTeams("Team1", 1);
            SoccerTeams team2 = new SoccerTeams("Team2", 4);
            SoccerTeamsRanking teamsRanking = new SoccerTeamsRanking();
            teamsRanking.AddNewTeams(team1);
            teamsRanking.AddNewTeams(team2);
            teamsRanking.UpdateScoreAfterMatch(team1, team2, 1, 2);
            Assert.Equal(1, teamsRanking.GetPositionOfRequiredTeam(team2));
            Assert.Equal(0, teamsRanking.GetPositionOfRequiredTeam(team1));
        }

        [Fact]
        public void UpdatesPointsForWinningTeamAfterAMatchIfResultAreEqual()
        {
            SoccerTeams team1 = new SoccerTeams("Team1", 1);
            SoccerTeams team2 = new SoccerTeams("Team2", 4);
            SoccerTeamsRanking teamsRanking = new SoccerTeamsRanking();
            teamsRanking.AddNewTeams(team1);
            teamsRanking.AddNewTeams(team2);
            teamsRanking.UpdateScoreAfterMatch(team1, team2, 1, 1);
            Assert.Equal(0, teamsRanking.GetPositionOfRequiredTeam(team1));
            Assert.Equal(1, teamsRanking.GetPositionOfRequiredTeam(team2));
        }

        [Fact]
        public void ShouldCorrectlySortRankingAfterGames()
        {
            SoccerTeams team1 = new SoccerTeams("Team1", 1);
            SoccerTeams team2 = new SoccerTeams("Team2", 4);
            SoccerTeams team3 = new SoccerTeams("Team3", 2);
            SoccerTeams team4 = new SoccerTeams("Team4", 3);
            SoccerTeamsRanking teamsRanking = new SoccerTeamsRanking();
            teamsRanking.AddNewTeams(team1);
            teamsRanking.AddNewTeams(team2);
            teamsRanking.AddNewTeams(team3);
            teamsRanking.AddNewTeams(team4);
            teamsRanking.UpdateScoreAfterMatch(team1, team2, 3, 1);
            teamsRanking.UpdateScoreAfterMatch(team3, team4, 1, 3);
            Assert.Equal(0, teamsRanking.GetPositionOfRequiredTeam(team2));
            Assert.Equal(1, teamsRanking.GetPositionOfRequiredTeam(team4));
            Assert.Equal(2, teamsRanking.GetPositionOfRequiredTeam(team1));
            Assert.Equal(3, teamsRanking.GetPositionOfRequiredTeam(team3));
        }

    }
}