using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts
{
    public sealed class LevelEditorWindow : EditorWindow
    {
        private int levelNumber = 1;
        private Vector2 scrollPos = Vector2.zero;
        private LevelData levelData = null;
        [MenuItem ("Custom/LevelEditorWindow")]
        public static void ShowWindow ()
        {
            var window = EditorWindow.GetWindow<LevelEditorWindow> ();
            window.Show ();
        }

        public void OnGUI ()
        {
            levelNumber = EditorGUILayout.IntField ("LevelNumber", levelNumber);

            if (GUILayout.Button ("Load"))
            {
                levelData = Load (levelNumber);
            }

            if (levelData != null)
            {
                var size = EditorGUILayout.Vector2IntField ("Size", levelData.stage.size);
                if (size != levelData.stage.size)
                {
                    levelData.stage.ChangeSize (size);
                }
                using (var scrollScope = new EditorGUILayout.ScrollViewScope (scrollPos))
                {
                    for (int y = 0; y < size.y; ++y)
                    {
                        using (new EditorGUILayout.HorizontalScope ())
                        {
                            for (int x = 0; x < size.x; ++x)
                            {
                                using (new EditorGUILayout.VerticalScope (GUI.skin.box, GUILayout.Width (50f)))
                                {
                                    var panel = levelData.stage[x, y];
                                    using (new GUIBackgroundColorScope (ContentColor (panel)))
                                    {
                                        panel.type = (Enums.EPanelType) EditorGUILayout.EnumPopup ((Enums.EPanelType) panel.type);
                                        panel.color = (Enums.EPanelColor) EditorGUILayout.EnumPopup ((Enums.EPanelColor) panel.color);
                                    }
                                    if (panel.type == Enums.EPanelType.WALL)
                                    {
                                        panel.adjacentWallPattern = levelData.stage.GetAdjacentWallPattern (x, y);
                                    }
                                    panel.color = AdjustmentColor (panel);
                                    levelData.stage[x, y] = panel;
                                }
                            }
                        }
                    }
                    scrollPos = scrollScope.scrollPosition;
                }
            }

            if (GUILayout.Button ("Initialize"))
            {
                levelData.stage.Initialize ();
            }

            if (GUILayout.Button ("Save"))
            {
                levelData.number = levelNumber;
                Save (levelNumber, levelData);
            }
        }

        private LevelData Load (int number)
        {
            var data = Resources.Load<LevelData> (LevelData.GetLevelDataPath (number)).Clone ();
            if (data == null)
            {
                data = new LevelData (number);
            }
            return data;
        }

        private void Save (int number, LevelData level)
        {
            var path = LevelData.GetLevelDataPath (number);
            var data = Resources.Load<LevelData> (path);
            if (data == null)
            {
                data = ScriptableObject.CreateInstance<LevelData> ();
                AssetDatabase.CreateAsset (data, path);
            }
            data.UpdateLevel (level);
            EditorUtility.SetDirty (data);
            AssetDatabase.SaveAssets ();
            Debug.Log ("Save!");
        }

        private Color ContentColor (PanelData panel)
        {
            if (panel.type == Enums.EPanelType.WALL) return Color.black;
            if (panel.type == Enums.EPanelType.NONE) return Color.white;
            switch (panel.color)
            {
            case Enums.EPanelColor.RED:
                return Color.red;
            case Enums.EPanelColor.GREEN:
                return Color.green;
            case Enums.EPanelColor.BLUE:
                return Color.blue;
            default:
                return Color.white;
            }
        }

        private Enums.EPanelColor AdjustmentColor (PanelData panel)
        {
            if (panel.type == Enums.EPanelType.WALL || panel.type == Enums.EPanelType.NONE) return Enums.EPanelColor.NONE;
            if (panel.color == Enums.EPanelColor.NONE) return Enums.EPanelColor.RED;
            return panel.color;
        }
    }
}