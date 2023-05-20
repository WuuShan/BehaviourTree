namespace WuuShan.AIBehaviour
{
    /// <summary>
    /// 顺序节点按顺序运行每个子节点，任意子节点失败则返回失败，若子节点都成功则返回成功。
    /// </summary>
    public class SequencerNode : CompositeNode
    {
        private int current;

        protected override void OnStart()
        {
            current = 0;
        }

        protected override void OnStop()
        {

        }

        protected override State OnUpdate()
        {
            var child = children[current];

            switch (child.Update())
            {
                case State.Running:
                    return State.Running;
                case State.Failure:
                    return State.Failure;
                case State.Success:
                    current++;
                    break;
            }

            return current == children.Count ? State.Success : State.Running;
        }
    }
}
