using System;
using UnityEditor;
using UnityEngine.UIElements;

namespace WuuShan.AIBehaviour
{
    /// <summary>
    /// 检查器视图
    /// </summary>
    public class InspectorView : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<InspectorView, VisualElement.UxmlTraits> { }

        private Editor editor;

        public InspectorView()
        {

        }

        /// <summary>
        /// 根据选择的 节点视图 更新 检查器视图内容
        /// </summary>
        /// <param name="node">节点视图</param>
        internal void UpdateSelection(NodeView nodeView)
        {
            Clear();

            UnityEngine.Object.DestroyImmediate(editor);
            editor = Editor.CreateEditor(nodeView.node);
            IMGUIContainer container = new(() => { editor.OnInspectorGUI(); });
            Add(container);
        }
    }
}