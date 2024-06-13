using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;

namespace LeTai.Asset.TranslucentImage.Editor
{
    [CustomEditor(typeof(ScalableBlurConfig))]
    [CanEditMultipleObjects]
    public class ScalableBlurConfigEditor : UnityEditor.Editor
    {
        readonly AnimBool _useAdvancedControl = new AnimBool(false);

        int _tab, _previousTab;

        EditorProperty _radius;
        EditorProperty _iteration;
        EditorProperty _strength;

        public void Awake()
        {
            LoadTabSelection();
            _useAdvancedControl.value = _tab > 0;
        }

        public void OnEnable()
        {
            _radius    = new EditorProperty(serializedObject, nameof(ScalableBlurConfig.Radius));
            _iteration = new EditorProperty(serializedObject, nameof(ScalableBlurConfig.Iteration));
            _strength  = new EditorProperty(serializedObject, nameof(ScalableBlurConfig.Strength));
            
            // Without this editor will not Repaint automatically when animating
            _useAdvancedControl.valueChanged.AddListener(Repaint);
        }

        public override void OnInspectorGUI()
        {
            Draw();
        }

        public void Draw()
        {
            using (new EditorGUILayout.VerticalScope())
            {
                DrawTabBar();

                using (var changes = new EditorGUI.ChangeCheckScope())
                {
                    serializedObject.Update();
                    DrawTabsContent();
                    if (changes.changed) serializedObject.ApplyModifiedProperties();
                }
            }
        }

        void DrawTabBar()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();

                _tab = GUILayout.Toolbar(
                    _tab,
                    new[] { "Simple", "Advanced" },
                    GUILayout.MinWidth(0),
                    GUILayout.MaxWidth(EditorGUIUtility.pixelsPerPoint * 192)
                );

                GUILayout.FlexibleSpace();
            }

            if (_tab != _previousTab)
            {
                GUI.FocusControl(""); // Defocus
                SaveTabSelection();
                _previousTab = _tab;
            }

            _useAdvancedControl.target = _tab == 1;
        }

        void DrawTabsContent()
        {
            if (EditorGUILayout.BeginFadeGroup(1 - _useAdvancedControl.faded))
            {
                // EditorProperty dooesn't invoke getter. Not needed anywhere else.
                _ = ((ScalableBlurConfig)target).Strength;
                _strength.Draw();
            }
            EditorGUILayout.EndFadeGroup();

            if (EditorGUILayout.BeginFadeGroup(_useAdvancedControl.faded))
            {
                _radius.Draw();
                _iteration.Draw();
            }
            EditorGUILayout.EndFadeGroup();
        }

        //Persist selected tab between sessions and instances
        void SaveTabSelection()
        {
            EditorPrefs.SetInt("LETAI_TRANSLUCENTIMAGE_TIS_TAB", _tab);
        }

        void LoadTabSelection()
        {
            if (EditorPrefs.HasKey("LETAI_TRANSLUCENTIMAGE_TIS_TAB"))
                _tab = EditorPrefs.GetInt("LETAI_TRANSLUCENTIMAGE_TIS_TAB");
        }
    }
}
