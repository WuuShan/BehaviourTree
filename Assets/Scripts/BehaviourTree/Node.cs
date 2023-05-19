using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public State state = State.Running;
    /// <summary>
    /// 是否执行过
    /// </summary>
    public bool started = false;

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

    protected abstract void OnStart();
    protected abstract void OnStop();
    protected abstract State OnUpdate();
}
