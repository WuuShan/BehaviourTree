using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourTreeRunner : MonoBehaviour
{
    private BehaviourTree tree;

    private void Start()
    {
        tree = ScriptableObject.CreateInstance<BehaviourTree>();

        var log1 = ScriptableObject.CreateInstance<DebugLogNode>();
        log1.message = "111 !!!";

        var pause1 = ScriptableObject.CreateInstance<WaitNode>();
        pause1.duration = 1;

        var log2 = ScriptableObject.CreateInstance<DebugLogNode>();
        log2.message = "222 !!!";

        var pause2 = ScriptableObject.CreateInstance<WaitNode>();
        pause2.duration = 1;

        var log3 = ScriptableObject.CreateInstance<DebugLogNode>();
        log3.message = "333 !!!";

        var pause3 = ScriptableObject.CreateInstance<WaitNode>();
        pause3.duration = 1;

        var sequence = ScriptableObject.CreateInstance<SequencerNode>();
        sequence.children.Add(log1);
        sequence.children.Add(pause1);
        sequence.children.Add(log2);
        sequence.children.Add(pause2);
        sequence.children.Add(log3);
        sequence.children.Add(pause3);

        var loop = ScriptableObject.CreateInstance<RepeatNode>();
        loop.child = sequence;

        tree.rootNode = loop;
    }

    private void Update()
    {
        tree.Update();
    }
}
