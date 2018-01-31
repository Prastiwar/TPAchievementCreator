using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

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
                    DrawNotification();
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
                EditorGUILayout.HelpBox("There is no object loaded!", MessageType.Error);
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

        TPNotification notification = null;
        void DrawNotification()
        {
            if (Event.current.type == EventType.DragPerform)
            {
                if (DragAndDrop.objectReferences.Length > 1)
                {
                    return;
                }
                else
                {
                    if (!PrefabUtility.GetPrefabObject(DragAndDrop.objectReferences[0]))
                    {
                        return;
                    }
                }
            }

            EditorGUILayout.LabelField("Put there you prefab of TPNotification", TPAchievementDesigner.skin.GetStyle("TipLabel"));
            notification = EditorGUILayout.ObjectField(notification, typeof(TPNotification), false) as TPNotification;

            if (notification == null)
                return;

            Space(4);
            EditorGUILayout.LabelField("Notification's Image for Icon", TPAchievementDesigner.skin.GetStyle("TipLabel"));
            notification.iconImage = EditorGUILayout.ObjectField(notification.iconImage, typeof(Image), false) as Image;
            Space(2);
            EditorGUILayout.LabelField("Notification's TextMeshProUGUI for Title", TPAchievementDesigner.skin.GetStyle("TipLabel"));
            notification.titleText = EditorGUILayout.ObjectField(notification.titleText, typeof(TextMeshProUGUI), false) as TextMeshProUGUI;
            Space(2);
            EditorGUILayout.LabelField("Notification's TextMeshProUGUI for Description", TPAchievementDesigner.skin.GetStyle("TipLabel"));
            notification.descriptionText = EditorGUILayout.ObjectField(notification.descriptionText, typeof(TextMeshProUGUI), false) as TextMeshProUGUI;

            if (GUI.changed)
            {
                EditorUtility.SetDirty(notification);
            }
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

            newObj = ScriptableObject.CreateInstance<TPAchievement>();
            newAssetPath += "New Achievement.asset";

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