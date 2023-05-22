﻿using System.Collections;
using UnityEngine;

namespace WuuShan.AIBehaviour
{
    public class DebugLogNode : ActionNode
    {
        public string message;

        protected override void OnStart()
        {
            Debug.Log($"OnStart{message}");
        }

        protected override void OnStop()
        {
            Debug.Log($"OnStop{message}");
        }

        protected override State OnUpdate()
        {
            Debug.Log($"OnUpdate{message}");

            Debug.Log($"Blackboard:{blackboard.moveToPosition}");

            blackboard.moveToPosition.x += 1;

            return State.Success;
        }
    }
}
