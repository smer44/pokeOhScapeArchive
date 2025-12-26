using UnityEngine;
using System.Collections.Generic;

public class PathTreeNode
{
    public string name;

}


public class PathTreeContainer : PathTreeNode
{    
    public Dictionary<string,PathTreeNode> children;

}

public class PathTreeLeaf : PathTreeNode
{
    public AbstractBattleActionSO action;

}
