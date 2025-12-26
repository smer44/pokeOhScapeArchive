using UnityEngine;
using System.Linq;
using System.Collections.Generic;


public class QueryBFFunctions
{

    public static int CountEnemyTeamMembersOld(BattleFieldMB field)
    {
        return CountTeamMembers(field, GetCurrentEnemyTeamNames(field));
    }

    public static int CountTeamMembers(BattleFieldMB field, List<string> teamNames)
    {
        int count = 0;
        foreach (string team in teamNames)
        {
            count += field.teams[team].Count;
        }
        return count;
    }

    public static List<string> GetCurrentEnemyTeamNames(BattleFieldMB field)
    {
        List<string> enemyTeams = new List<string>();
        foreach (var team in field.teams.Keys)
        {
            if (team != field.currentTeam)
            {
                enemyTeams.Add(team);
            }

        }
        return enemyTeams;
    }



    public static List<GameObject> GetEnemyUnitsOfNextTeam(BattleFieldMB field)
    {
        var enemyTeamName = field.teamNames[(field.teamNamePos + 1) % field.teamNames.Length];
        var enemyUnits = field.teams[enemyTeamName];
        return enemyUnits;
    }


    public static List<GameObject> GetCurrentTeamUnits(BattleFieldMB field)
    {
        return field.teams[field.currentTeam];
    }


    public static List<GameObject> GetPerformer(BattleFieldMB field)
    {
        return new List<GameObject>() { field.mems.CurrentFirstPerformer() };
    }

    /// <summary>
    /// Returns the “half of all cells” depending on field.teamNamePos:
    /// - teamNamePos == 0  → first half  (indices [0 .. half-1])
    /// - teamNamePos == 1  → second half (indices [half .. len-1])
    /// Other values are not supported and will throw.
    /// </summary>
    public static List<GameObject> GetCellsHalfForCurrentTeam(BattleFieldMB field)
    {

        return GetCellsHalfByTeamIndex(field, field.teamNamePos);
    }


    public static List<GameObject> GetCellsHalfByTeamIndex(BattleFieldMB field, int teamIndex)
    {
        int half = field.grid.Length / 2;
        if (teamIndex == 0)
        {
            return field.grid.Take(half).ToList();
        }
        else if (teamIndex == 1)
        {
            return field.grid.Skip(half).ToList();
        }
        else
        {
            throw new System.Exception($"QueryBFFunctions.GetCellsHalfByTeamIndex: teamIndex {teamIndex} not supported");
        }
    }

    public static List<GameObject> GetNeighborsCells(BattleFieldMB field, GameObject cell)
    {

        CellMB cellMB = cell.GetComponent<CellMB>();
        int x = cellMB.zone;
        int y = cellMB.lane;

        List<int> indexes = GridFunctions.Neighbors4Indices(x, y, field);
        List<GameObject> neighbors = new List<GameObject>(indexes.Count);
        foreach (int idx in indexes)
        {
            neighbors.Add(field.grid[idx]);
        }        
        return neighbors;
    }

}
