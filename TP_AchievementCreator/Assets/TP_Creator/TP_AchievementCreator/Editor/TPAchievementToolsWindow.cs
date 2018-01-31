using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace TP_AchievementEditor
{
    [InitializeOnLoad]
    internal class TPAchievementToolsWindow : EditorWindow
    {
        public static TPAchievementToolsWindow window;
        public enum Tool
        {
            Achievements,
            Notification,
            Layout
        }
        public static Tool tool;

        SerializedProperty _Achievements;
        SerializedProperty _Notifications;
        
        Texture2D mainTexture;
        Vector2 scrollPos = Vector2.zero;
        Vector2 textureVec;

        Rect mainRect;

        static float windowSize = 515;
        static string currentScene;

        public static void OpenToolWindow(Tool _tool)
        {
            if (window != null)
                window.Close();

            window = (TPAchievementToolsWindow)GetWindow(typeof(TPAchievementToolsWindow));

            currentScene = EditorSceneManager.GetActiveScene().name;
            EditorApplication.hierarchyWindowChanged += hierarchyWindowChanged;

            window.minSize = new Vector2(windowSize, windowSize);
            window.maxSize = new Vector2(windowSize, windowSize);
            window.Show();
            tool = _tool;
            AssetDatabase.OpenAsset(TPAchievementDesigner.AchievementCreator);
        }

        static void hierarchyWindowChanged()
        {
            if (currentScene != EditorSceneManager.GetActiveScene().name)
            {
                if (TPAchievementDesigner.window)
                    TPAchievementDesigner.window.Close();
                if (window)
                    window.Close();
            }
        }

        void OnEnable()
        {
            InitTextures();

            FindLayoutProperties();
        }

        void FindLayoutProperties()
        {
            _Achievements = TPAchievementDesigner.creator.FindProperty("Achievements");
            _Notifications = TPAchievementDesigner.creator.FindProperty("Notifications");
        }

        void InitTextures()
        {
            Color color = new Color(0.19f, 0.19f, 0.19f);
            mainTexture = new Texture2D(1, 1);
            mainTexture.SetPixel(0, 0, color);
            mainTexture.Apply();
        }

        void OnGUI()
        {
            mainRect = new Rect(0, 0, Screen.width, Screen.height);
            GUI.DrawTexture(mainRect, mainTexture);
            scrollPos = GUILayout.BeginScrollView(scrollPos, false, false, GUIStyle.none, GUI.skin.verticalScrollbar);
            DrawTool();
            GUILayout.EndScrollView();
        }

        public void DrawTool()
        {
            switch (tool)
            {
                case Tool.Achievements:
                    DrawObjects(_Achievements);
                    break;
                case Tool.Notification:
                    DrawObjects(_Notifications);
                    break;
                case Tool.Layout:
                    DrawLayout();
                    break;
            }
        }

        
        void DrawObjects(SerializedProperty property)
        {
            property.serializedObject.UpdateIfRequiredOrScript();

            EditorGUILayout.BeginVertical();

            if (GUILayout.Button("Create new", TPAchievementDesigner.EditorData.GUISkin.button))
            {
                CreateScriptable();
            }

            Space(2);

            if (property.arraySize <= 0)
            {
                EditorGUILayout.HelpBox("There is no Object Loaded!", MessageType.Error);
                EditorGUILayout.EndVertical();
                return;
            }

            foreach (SerializedProperty item in property)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(item, GUIContent.none);
                DeleteAsset(item.objectReferenceValue as UnityEngine.Object);
                EditAsset(item.objectReferenceValue as UnityEngine.Object);
                EditorGUILayout.EndHorizontal();
            }

            if (GUI.changed)
                property.serializedObject.ApplyModifiedProperties();

            EditorGUILayout.EndVertical();
        }

        void DrawNotification()
        {
            
        }

        void DrawLayout()
        {

        }

        void DeleteAsset(UnityEngine.Object obj)
        {
            if (GUILayout.Button("Del", GUILayout.Width(30)))
            {
                string assetPath = AssetDatabase.GetAssetPath(obj);
                AssetDatabase.MoveAssetToTrash(assetPath);

                TPAchievementDesigner.UpdateManager();
                DrawTool();
            }
        }

        void EditAsset(UnityEngine.Object obj)
        {
            if (GUILayout.Button("Edit", GUILayout.Width(40)))
            {
                AssetDatabase.OpenAsset(obj);
            }
        }

        void CreateScriptable()
        {
            string assetPath = "Assets/" + TPAchievementDesigner.EditorData.AchievementsPath;
            string newAssetPath = assetPath;
            UnityEngine.Object newObj = null;

            switch (tool)
            {
                case Tool.Achievements:
                    newObj = ScriptableObject.CreateInstance<TPAchievement>();
                    newAssetPath += "New Achievement.asset";
                    break;
                case Tool.Notification:
                    newObj = ScriptableObject.CreateInstance<TPNotification>();
                    newAssetPath += "New Notification.asset";
                    break;
                case Tool.Layout:
                    break;
                default:
                    break;
            }

            if (!AssetDatabase.IsValidFolder(assetPath))
                System.IO.Directory.CreateDirectory(assetPath);

            AssetDatabase.CreateAsset(newObj, AssetDatabase.GenerateUniqueAssetPath(newAssetPath));
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            AssetDatabase.OpenAsset(newObj);

            Debug.Log(newObj.name + " created in " + assetPath);
            TPAchievementDesigner.UpdateManager();
            DrawTool();
        }

        void Space(int length)
        {
            for (int i = 0; i < length; i++)
                EditorGUILayout.Space();
        }

        void Update()
        {
            if (EditorApplication.isCompiling)
                this.Close();
        }
    }
}