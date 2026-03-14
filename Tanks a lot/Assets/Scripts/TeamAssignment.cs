using UnityEngine;

/// <summary>
/// Defines which team a tank belongs to
/// Used for team-based damage, scoring, and visual identification
/// </summary>
public class TeamAssignment : MonoBehaviour
{
    public enum Team
    {
        Team1,
        Team2,
        Neutral
    }

    [SerializeField] private Team _team = Team.Neutral;
    public Team CurrentTeam => _team;

    public Color GetTeamColor()
    {
        GameParameters parameters = Resources.Load<GameParameters>("GameParameters");
        if (parameters == null)
        {
            Debug.LogWarning("[TeamAssignment] GameParameters not found in Resources folder!");
            return Color.white;
        }

        return _team switch
        {
            Team.Team1 => parameters.Team1Color,
            Team.Team2 => parameters.Team2Color,
            _ => Color.white
        };
    }

    public bool IsTeam(Team otherTeam) => _team == otherTeam;

    public bool IsAllyOf(TeamAssignment other) => other != null && _team == other._team;

    public bool IsEnemyOf(TeamAssignment other) => other != null && _team != other._team && _team != Team.Neutral;

    public void SetTeam(Team newTeam)
    {
        _team = newTeam;
    }
}
