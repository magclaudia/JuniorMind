using Xunit;

namespace TeamsRanking
{
    public class SoccerTeamsRankingFacts
    {
        [Fact]
        public void AddTeamToRanking()
        {
            SoccerTeam team1 = new("Team1", 1);
            SoccerTeamRanking teamsRanking = new();
            teamsRanking.Add(team1);
            int number = teamsRanking.TeamsNumber();
            Assert.Equal(number, teamsRanking.TeamsNumber());
        }

        [Fact]
        public void AddMultipleTeamsToRanking()
        {
            SoccerTeam team1 = new("Team1", 1);
            SoccerTeam team2 = new("Team2", 4);
            SoccerTeam team3 = new("Team3", 2);
            SoccerTeam team4 = new("Team4", 3);
            SoccerTeamRanking teamsRanking = new();
            teamsRanking.Add(team1);
            teamsRanking.Add(team2);
            teamsRanking.Add(team3);
            teamsRanking.Add(team4);
            int number = teamsRanking.TeamsNumber();
            Assert.Equal(number, teamsRanking.TeamsNumber());
        }

        [Fact]
        public void CorrectlyReturnATeamByInputPosition()
        {
            SoccerTeam team1 = new("Team1", 1);
            SoccerTeam team2 = new("Team2", 4);
            SoccerTeam team3 = new("Team3", 2);
            SoccerTeam team4 = new("Team4", 3);
            SoccerTeamRanking teamsRanking = new();
            teamsRanking.Add(team1);
            teamsRanking.Add(team2);
            teamsRanking.Add(team3);
            teamsRanking.Add(team4);
            Assert.Equal(team3, teamsRanking.TeamAtPosition(2));
        }

        [Fact]
        public void CorrectlReturnATeamPosition()
        {
            SoccerTeam team1 = new("Team1", 1);
            SoccerTeam team2 = new("Team2", 4);
            SoccerTeam team3 = new("Team3", 2);
            SoccerTeam team4 = new("Team4", 3);
            SoccerTeamRanking teamsRanking = new();
            teamsRanking.Add(team1);
            teamsRanking.Add(team2);
            teamsRanking.Add(team3);
            teamsRanking.Add(team4);
            Assert.Equal(2, teamsRanking.PositionOf(team3));
        }

        [Fact]
        public void UpdatesPointsForWinningTeamAfterAMatchIfResultAreDifferent()
        {
            SoccerTeam team1 = new("Team1", 1);
            SoccerTeam team2 = new("Team2", 4);
            SoccerTeamRanking teamsRanking = new();
            teamsRanking.Add(team1);
            teamsRanking.Add(team2);
            teamsRanking.Update(team1, team2, 1, 2);
            Assert.Equal(0, teamsRanking.PositionOf(team2));
            Assert.Equal(1, teamsRanking.PositionOf(team1));
        }

        [Fact]
        public void UpdatesPointsForWinningTeamAfterAMatchIfResultAreEqual()
        {
            SoccerTeam team1 = new("Team1", 1);
            SoccerTeam team2 = new("Team2", 4);
            SoccerTeamRanking teamsRanking = new();
            teamsRanking.Add(team1);
            teamsRanking.Add(team2);
            teamsRanking.Update(team1, team2, 1, 1);
            Assert.Equal(0, teamsRanking.PositionOf(team2));
            Assert.Equal(1, teamsRanking.PositionOf(team1));
        }

        [Fact]
        public void ShouldCorrectlySortRankingAfterGames()
        {
            SoccerTeam team1 = new("Team1", 1);
            SoccerTeam team2 = new("Team2", 4);
            SoccerTeam team3 = new("Team3", 2);
            SoccerTeam team4 = new("Team4", 3);
            SoccerTeamRanking teamsRanking = new();
            teamsRanking.Add(team1);
            teamsRanking.Add(team2);
            teamsRanking.Add(team3);
            teamsRanking.Add(team4);
            teamsRanking.Update(team1, team2, 3, 1);
            teamsRanking.Update(team3, team4, 1, 3);
            Assert.Equal(0, teamsRanking.PositionOf(team4));
            Assert.Equal(1, teamsRanking.PositionOf(team1));
            Assert.Equal(2, teamsRanking.PositionOf(team2));
            Assert.Equal(3, teamsRanking.PositionOf(team3));
        }
    }
}