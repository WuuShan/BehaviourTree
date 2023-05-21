using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace WuuShan.AIBehaviour
{
    public class BehaviourTreeView : GraphView
    {
        /// <summary>
        /// 节点被选中事件
        /// </summary>
        public Action<NodeView> OnNodeSelected;

        /// <summary>
        /// 样式表路径
        /// </summary>
        private readonly string ussPath = "Assets/Editor/AIBehaviour/BehaviourTreeEditor.uss";

        public new class UxmlFactory : UxmlFactory<BehaviourTreeView, GraphView.UxmlTraits>
        { }

        private BehaviourTree tree;

        public BehaviourTreeView()
        {
            Insert(0, new GridBackground());    // 实例化网格背景

            // 添加操作 鼠标中键 内容缩放、拖拽 鼠标左键 拉取选框
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(ussPath);
            styleSheets.Add(styleSheet);

            Undo.undoRedoPerformed += OnUndoRedo;
        }

        /// <summary>
        /// 撤销重做
        /// </summary>
        private void OnUndoRedo()
        {
            PopulateView(tree);
            AssetDatabase.SaveAssets();
        }

        /// <summary>
        /// 根据节点GUID查找对应的节点视图并返回
        /// </summary>
        /// <param name="node"></param>
        /// <returns>对应GUID的节点视图</returns>
        private NodeView FindNodeView(Node node)
        {
            // 获取具有给定 GUID 的第一个节点。如果没有找到则为空。
            return GetNodeByGuid(node.guid) as NodeView;
        }

        /// <summary>
        /// 填充行为树视图
        /// </summary>
        /// <param name="tree"></param>
        internal void PopulateView(BehaviourTree tree)
        {
            this.tree = tree;

            graphViewChanged -= OnGraphViewChanged;
            DeleteElements(graphElements);
            graphViewChanged += OnGraphViewChanged;

            if (tree.rootNode == null)
            {
                tree.rootNode = tree.CreateNode(typeof(RootNode)) as RootNode;
                EditorUtility.SetDirty(tree);   // 将目标对象标记为脏。
                AssetDatabase.SaveAssets();
            }

            // 创建节点视图
            tree.nodes.ForEach(n => CreateNodeView(n));

            // 创建边线
            tree.nodes.ForEach(n =>
            {
                var children = tree.GetChildren(n);

                children.ForEach(c =>
                {
                    NodeView parentView = FindNodeView(n);
                    NodeView childView = FindNodeView(c);

                    Edge edge = parentView.output.ConnectTo(childView.input);
                    AddElement(edge);
                });
            });
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            return ports.ToList().Where(endPort =>
            endPort.direction != startPort.direction &&
            endPort.node != startPort.node).ToList();
        }

        private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
        {
            graphViewChange.elementsToRemove?.ForEach(elem =>
            {
                if (elem is NodeView nodeView)
                {
                    tree.DeleteNode(nodeView.node);
                }

                if (elem is Edge edge)
                {
                    if (edge.output.node is NodeView parentView
                      && edge.input.node is NodeView childView)
                    {
                        tree.RemoveChild(parentView.node, childView.node);
                    }
                }
            });

            graphViewChange.edgesToCreate?.ForEach(edge =>
            {
                if (edge.output.node is NodeView parentView
                  && edge.input.node is NodeView childView)
                {
                    tree.AddChild(parentView.node, childView.node);
                }
            });

            if (graphViewChange.movedElements != null)
            {
                nodes.ForEach((n) =>
                {
                    if (n is NodeView view)
                    {
                        view.SortChildren();
                    }
                });
            }

            return graphViewChange;
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            // base.BuildContextualMenu(evt);

            {
                var types = TypeCache.GetTypesDerivedFrom<ActionNode>();

                foreach (var type in types)
                {
                    evt.menu.AppendAction($"[{type.BaseType.Name}] {type.Name}", (a) => CreateNode(type));
                }
            }

            {
                var types = TypeCache.GetTypesDerivedFrom<CompositeNode>();

                foreach (var type in types)
                {
                    evt.menu.AppendAction($"[{type.BaseType.Name}] {type.Name}", (a) => CreateNode(type));
                }
            }

            {
                var types = TypeCache.GetTypesDerivedFrom<DecoratorNode>();

                foreach (var type in types)
                {
                    evt.menu.AppendAction($"[{type.BaseType.Name}] {type.Name}", (a) => CreateNode(type));
                }
            }
        }

        private void CreateNode(Type type)
        {
            Node node = tree.CreateNode(type);
            CreateNodeView(node);
        }

        /// <summary>
        /// 创建节点视图
        /// </summary>
        /// <param name="node">节点</param>
        private void CreateNodeView(Node node)
        {
            NodeView nodeView = new(node)
            {
                OnNodeSelected = OnNodeSelected
            };

            AddElement(nodeView);
        }
    }
}