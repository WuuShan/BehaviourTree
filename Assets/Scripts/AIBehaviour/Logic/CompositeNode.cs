using System.Collections.Generic;
using UnityEngine;

namespace WuuShan.AIBehaviour
{
    /// <summary>
    /// 复合节点具有一个或多个子节点的节点
    /// </summary>
    public abstract class CompositeNode : Node
    {
        [HideInInspector] public List<Node> children = new();

        public override Node Clone()
        {
            CompositeNode node = Instantiate(this);
            node.children = children.ConvertAll(c => c.Clone());
            return node;
        }
    }
}