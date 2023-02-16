using Xunit;

namespace TeamsRanking
{
    public class SoccerTeamFacts
    {

        [Fact]
        public void VerifyForNull()
        {
            SoccerTeam team1 = new("team1", 2);
            SoccerTeam team2 = null;
            Assert.False(team1.ComparePoints(team2));
        }

        [Fact]
        public void UpdateRakingAferAAMatch()
        {
            SoccerTeam team1 = new("team1", 3);
            SoccerTeam team2 = new("team2", 1);
            team2.AddPoints(1);
            Assert.True(team2.ComparePoints(team1));
        }
    }
}
