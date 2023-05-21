using System;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UIElements;

namespace WuuShan.AIBehaviour
{
    /// <summary>
    /// 行为树编辑器
    /// </summary>
    public class BehaviourTreeEditor : EditorWindow
    {
        /// <summary>
        /// 行为树视图
        /// </summary>
        private BehaviourTreeView treeView;
        /// <summary>
        /// 检查器视图
        /// </summary>
        private InspectorView inspectorView;

        /// <summary>
        /// 可扩展标记语言文件路径
        /// </summary>
        private readonly string uxmlPath = "Assets/Editor/AIBehaviour/BehaviourTreeEditor.uxml";
        /// <summary>
        /// 样式表路径
        /// </summary>
        private readonly string ussPath = "Assets/Editor/AIBehaviour/BehaviourTreeEditor.uss";

        [MenuItem("WuuShan/BehaviourTreeEditor")]
        public static void OpenWindow()
        {
            BehaviourTreeEditor wnd = GetWindow<BehaviourTreeEditor>();
            wnd.titleContent = new GUIContent(nameof(BehaviourTreeEditor));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instanceId"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        [OnOpenAsset]
        public static bool OnOpenAsset(int instanceId, int line)
        {
            if (Selection.activeObject is BehaviourTree)
            {
                OpenWindow();
                return true;
            }
            return false;
        }

        public void CreateGUI()
        {
            // 每个编辑器窗口都包含一个根 VisualElement 对象
            VisualElement root = rootVisualElement;

            // 导入UXML
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(uxmlPath);
            visualTree.CloneTree(root);

            // 样式表可以添加到VisualElement.
            // 该样式将应用于VisualElement及其所有子元素。
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(ussPath);
            root.styleSheets.Add(styleSheet);

            treeView = root.Q<BehaviourTreeView>();
            inspectorView = root.Q<InspectorView>();

            treeView.OnNodeSelected = OnNodeSelectionChanged;

            OnSelectionChange();
        }

        private void OnNodeSelectionChanged(NodeView node)
        {
            inspectorView.UpdateSelection(node);
        }

        private void OnSelectionChange()
        {
            BehaviourTree tree = Selection.activeObject as BehaviourTree;

            // 检查 Unity 是否可以在编辑器中打开资产
            if (tree && AssetDatabase.CanOpenAssetInEditor(tree.GetInstanceID()))
            {
                treeView.PopulateView(tree);
            }
        }
    }
}