using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
namespace Editor
{
    [CustomEditor(typeof(ShapeManager))]
    public class ShapeManagerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            ShapeManager shapeManager = (ShapeManager)target;

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Shape Creation Tools", EditorStyles.boldLabel);
        
            if (GUILayout.Button("Capture Current Shape as Default"))
            {
                shapeManager.CaptureCurrentShapeAsDefault();
            }

            if (GUILayout.Button("Create Triangle Shape"))
            {
                shapeManager.CreateTriangleShape();
            }

            if (GUILayout.Button("Create Square Shape"))
            {
                shapeManager.CreateSquareShape();
            }

            if (GUILayout.Button("Reset to Default Shape"))
            {
                shapeManager.ResetToDefault();
            }
        }
    }
}
#endif