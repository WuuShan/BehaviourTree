using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

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

        public NodeView(Node node)
        {
            this.node = node;
            this.title = node.name;
            this.viewDataKey = node.guid;

            style.left = node.position.x;
            style.top = node.position.y;

            CreateInputPorts();
            CreateOutputPorts();
        }

        /// <summary>
        /// 创建输入端口
        /// </summary>
        private void CreateInputPorts()
        {
            if (node is ActionNode or CompositeNode or DecoratorNode)
            {
                input = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool));
            }

            if (input != null)
            {
                input.portName = "";
                inputContainer.Add(input);
            }
        }


        /// <summary>
        /// 创建输出端口
        /// </summary>
        private void CreateOutputPorts()
        {
            if (node is DecoratorNode or RootNode)
            {
                output = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
            }
            else if (node is CompositeNode)
            {
                output = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(bool));
            }

            if (output != null)
            {
                output.portName = "";
                outputContainer.Add(output);
            }
        }


        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);

            node.position.x = newPos.xMin;
            node.position.y = newPos.yMin;
        }

        public override void OnSelected()
        {
            base.OnSelected();

            OnNodeSelected?.Invoke(this);
        }
    }
}
