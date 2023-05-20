using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace WuuShan.AIBehaviour
{
    /// <summary>
    /// 行为树
    /// </summary>
    [CreateAssetMenu()]
    public class BehaviourTree : ScriptableObject
    {
        /// <summary>
        /// 根节点
        /// </summary>
        public Node rootNode;

        /// <summary>
        /// 当前状态
        /// </summary>
        public Node.State treeState = Node.State.Running;

        /// <summary>
        /// 子节点列表
        /// </summary>
        public List<Node> nodes = new();

        public Node.State Update()
        {
            if (rootNode.state == Node.State.Running)
            {
                treeState = rootNode.Update();
            }

            return treeState;
        }

        /// <summary>
        /// 创建节点
        /// </summary>
        /// <param name="type">节点类型</param>
        /// <returns></returns>
        public Node CreateNode(System.Type type)
        {
            Node node = CreateInstance(type) as Node;   // 创建一个实例化的脚本化对象节点
            node.name = type.Name;
            node.guid = GUID.Generate().ToString();
            nodes.Add(node);

            AssetDatabase.AddObjectToAsset(node, this); // 将对象添加到由资产对象标识的现有资产
            AssetDatabase.SaveAssets(); // 将所有未保存的资产更改写入磁盘

            return node;
        }

        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="node">要删除的节点</param>
        public void DeleteNode(Node node)
        {
            nodes.Remove(node);
            AssetDatabase.RemoveObjectFromAsset(node);  // 从资产中移除对象 (也可以看看: AssetDatabase.AddObjectToAsset).
            AssetDatabase.SaveAssets(); // 将所有未保存的资产更改写入磁盘
        }

        /// <summary>
        /// 为父节点添加子节点
        /// </summary>
        /// <param name="parent">父节点</param>
        /// <param name="child">子节点</param>
        public void AddChild(Node parent, Node child)
        {
            if (parent is DecoratorNode decorator)
            {
                decorator.child = child;
            }

            if (parent is RootNode root)
            {
                root.child = child;
            }

            if (parent is CompositeNode composite)
            {
                composite.children.Add(child);
            }
        }

        /// <summary>
        /// 为父节点移除子节点
        /// </summary>
        /// <param name="parent">父节点</param>
        /// <param name="child">子节点</param>
        public void RemoveChild(Node parent, Node child)
        {
            if (parent is DecoratorNode decorator)
            {
                decorator.child = null;
            }

            if (parent is RootNode root)
            {
                root.child = null;
            }

            if (parent is CompositeNode composite)
            {
                composite.children.Remove(child);
            }
        }

        /// <summary>
        /// 获得该父节点的子节点列表
        /// </summary>
        /// <param name="parent">父节点</param>
        /// <returns>子节点列表</returns>
        public List<Node> GetChildren(Node parent)
        {
            if (parent is DecoratorNode decorator && decorator.child != null)
            {
                return new List<Node> { decorator.child };
            }

            if (parent is RootNode root && root.child != null)
            {
                return new List<Node> { root.child };
            }

            if (parent is CompositeNode composite)
            {
                return composite.children;
            }

            return new();
        }

        public BehaviourTree Clone()
        {
            BehaviourTree tree = Instantiate(this);
            tree.rootNode = tree.rootNode.Clone();
            return tree;
        }
    }
}