using UnityEngine;
using WuuShan.AIBehaviour;

namespace WuuShan
{
    public class BehaviourTreeRunner : MonoBehaviour
    {
        public BehaviourTree tree;

        private void Start()
        {
            tree = tree.Clone();
            tree.Bind();
        }

        private void Update()
        {
            tree.Update();
        }
    }
}
