using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "MoveDatabaseSO", menuName = "Scriptable Objects/MoveDatabaseSO")]
public class MoveDatabaseSO : ScriptableObject
{
    public List<MoveData> adMoves = new List<MoveData>();
    public List<MoveData> supMoves = new List<MoveData>();
    public List<MoveData> conjurorMoves = new List<MoveData>();
}
