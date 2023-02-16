using Xunit;

namespace TeamsRanking
{
    public class SoccerTeamsFacts
    {

        [Fact]
        public void VerifyForNull()
        {
            SoccerTeams team1 = new SoccerTeams("team1", 2);
            SoccerTeams team2 = null;
            Assert.False(team1.CompareScore(team2));
        }

        [Fact]
        public void UpdateRakingAferAAMatch()
        {
            SoccerTeams team1 = new SoccerTeams("team1", 3);
            SoccerTeams team2 = new SoccerTeams("team2", 1);
            team2.UpdateScore(1);
            Assert.True(team2.CompareScore(team1));
        }
    }
}
