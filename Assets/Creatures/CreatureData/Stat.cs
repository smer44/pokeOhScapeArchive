using UnityEngine;
using System;


[System.Serializable]
public class Stat 
{
    public Action<StatType, int> onValueChange;
    public StatType type;
    public int value;
    public int minValue;
    public int maxValue;



}