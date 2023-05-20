using UnityEngine;

namespace WuuShan.AIBehaviour
{
    /// <summary>
    /// 行为树的节点
    /// </summary>
    public abstract class Node : ScriptableObject
    {
        /// <summary>
        /// 节点状态
        /// </summary>
        public enum State
        {
            /// <summary>
            /// 运行中
            /// </summary>
            Running,
            /// <summary>
            /// 失败
            /// </summary>
            Failure,
            /// <summary>
            /// 成功
            /// </summary>
            Success
        }

        /// <summary>
        /// 当前状态
        /// </summary>
        [HideInInspector] public State state = State.Running;
        /// <summary>
        /// 是否执行过
        /// </summary>
        [HideInInspector] public bool started = false;
        /// <summary>
        /// 唯一编号
        /// </summary>
        [HideInInspector] public string guid;
        /// <summary>
        /// 编辑器的位置
        /// </summary>
        [HideInInspector] public Vector2 position;

        public State Update()
        {
            if (!started)
            {
                OnStart();
                started = true;
            }

            state = OnUpdate();

            // if (state == State.Failure || state == State.Success)
            if (state != State.Running)
            {
                OnStop();
                started = false;
            }

            return state;
        }

        /// <summary>
        /// 克隆节点自身
        /// </summary>
        /// <returns></returns>
        public virtual Node Clone()
        {
            return Instantiate(this);
        }

        /// <summary>
        /// 开始执行节点的操作
        /// </summary>
        protected abstract void OnStart();

        /// <summary>
        /// 停止节点的操作
        /// </summary>
        protected abstract void OnStop();

        /// <summary>
        /// 更新节点状态
        /// </summary>
        /// <returns>节点状态</returns>
        protected abstract State OnUpdate();
    }
}