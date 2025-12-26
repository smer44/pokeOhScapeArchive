using UnityEngine;
using System.Collections.Generic;

public class MenuPathFunctions
{
    public static void AddBattleMenuPath(AbstractBattleActionSO pathData, PathTreeContainer root)
    {
        if ( pathData == null )
        {
            Debug.LogError($"AddBattleMenuPath: pathData is null: |{pathData}|");
            return;
        }
        if ( pathData.name == null )
        {
            Debug.LogError($"AddBattleMenuPath: Path name is null for: |{pathData}|");
            return;
        }
        if (pathData.path == null  )
        {
            Debug.LogError($"AddBattleMenuPath: Path is null for pathdata: |{pathData.name}|");
            return;
        }


        PathTreeContainer current = root;
        for (int i = 0; i < pathData.path.Length; i++)
        {
            string part = pathData.path[i];
            if (!current.children.ContainsKey(part))
            {
                current.children[part] = new PathTreeContainer
                {
                    name = part,
                    children = new Dictionary<string, PathTreeNode>()
                };
            }

            current = current.children[part] as PathTreeContainer;
            if (current == null)
            {
                Debug.LogError($"AddBattleMenuPath: Trying to create a submenu under a leaf node at: {part}");
                return;
            }
        }

        // Final node represents the action itself
        if (!current.children.ContainsKey(pathData.name))
        {
            current.children[pathData.name] = new PathTreeLeaf
            {
                name = pathData.name,
                action = pathData
            };
        }
        else
        {
            Debug.LogError($"AddBattleMenuPath: name of action is already present in this menu ui folder: {pathData.name}");
        }
    }
}
