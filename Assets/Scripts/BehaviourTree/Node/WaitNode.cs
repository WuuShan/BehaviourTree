using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitNode : ActionNode
{
    /// <summary>
    /// 等待时长
    /// </summary>
    public float duration = 1;
    /// <summary>
    /// 进入该节点的时间
    /// </summary>
    private float startTime;

    protected override void OnStart()
    {
        startTime = Time.time;
    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        if (Time.time - startTime > duration)
        {
            return State.Success;
        }

        return State.Running;
    }
}
