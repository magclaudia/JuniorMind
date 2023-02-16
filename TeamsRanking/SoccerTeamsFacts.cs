using Xunit;

namespace TeamsRanking
{
    public class SoccerTeamsFacts
    {
        [Fact]
        public void UpdateRaking()
        {
            SoccerTeams team1 = new SoccerTeams("team1", 2);
            SoccerTeams team2 = new SoccerTeams("team2", 0);
            team2.UpdateScore(1);
            Assert.True(team2.CompareScoreTo(team1));
        }
    }
}
