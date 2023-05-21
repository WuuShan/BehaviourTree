using System;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.UIElements;

namespace WuuShan.AIBehaviour
{
    /// <summary>
    /// 行为树编辑器的节点视图
    /// </summary>
    public class NodeView : UnityEditor.Experimental.GraphView.Node
    {
        /// <summary>
        /// 节点被选中事件
        /// </summary>
        public Action<NodeView> OnNodeSelected;

        /// <summary>
        /// 行为树节点
        /// </summary>
        public Node node;
        /// <summary>
        /// 输入端口
        /// </summary>
        public Port input;
        /// <summary>
        /// 输出端口
        /// </summary>
        public Port output;

        public NodeView(Node node) : base("Assets/Editor/AIBehaviour/NodeView.uxml")
        {
            this.node = node;
            this.title = node.name;
            this.viewDataKey = node.guid;

            style.left = node.position.x;
            style.top = node.position.y;

            CreateInputPorts();
            CreateOutputPorts();

            SetupClasses();
        }

        private void SetupClasses()
        {
            // 将一个类添加到元素的类列表中，以便从 USS 分配样式。
            if (node is ActionNode)
            {
                AddToClassList("action");
            }
            else if (node is CompositeNode)
            {
                AddToClassList("composite");
            }
            else if (node is DecoratorNode)
            {
                AddToClassList("decorator");
            }
            else if (node is RootNode)
            {
                AddToClassList("root");
            }
        }

        /// <summary>
        /// 创建输入端口
        /// </summary>
        private void CreateInputPorts()
        {
            if (node is ActionNode or CompositeNode or DecoratorNode)
            {
                input = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool));
            }

            if (input != null)
            {
                input.portName = "";
                input.style.flexDirection = FlexDirection.Column;
                inputContainer.Add(input);
            }
        }


        /// <summary>
        /// 创建输出端口
        /// </summary>
        private void CreateOutputPorts()
        {
            if (node is CompositeNode)
            {
                output = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Multi, typeof(bool));
            }
            else if (node is DecoratorNode or RootNode)
            {
                output = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(bool));
            }

            if (output != null)
            {
                output.portName = "";
                output.style.flexDirection = FlexDirection.ColumnReverse;
                outputContainer.Add(output);
            }
        }


        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);

            // 允许你在将要对其执行更改的特定对象上注册撤消操作。
            // 记录在 RecordObject 函数之后对对象所做的任何更改。
            Undo.RecordObject(node, "Behaviour Tree (Set Position)");

            node.position.x = newPos.xMin;
            node.position.y = newPos.yMin;

            EditorUtility.SetDirty(node);
        }

        public override void OnSelected()
        {
            base.OnSelected();

            OnNodeSelected?.Invoke(this);
        }
    }
}
