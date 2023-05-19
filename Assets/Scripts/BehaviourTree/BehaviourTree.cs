using System.Collections;
using UnityEngine;

[CreateAssetMenu()]
public class BehaviourTree : ScriptableObject
{
    /// <summary>
    /// 行为树根节点
    /// </summary>
    public Node rootNode;
    public Node.State treeState = Node.State.Running;

    public Node.State Update()
    {
        if (rootNode.state == Node.State.Running)
        {
            treeState = rootNode.Update();
        }

        return treeState;
    }
}
