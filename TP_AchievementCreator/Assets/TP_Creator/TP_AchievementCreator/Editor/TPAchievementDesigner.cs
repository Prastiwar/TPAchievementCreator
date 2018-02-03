using UnityEngine;
using UnityEditor;
using TP_Achievement;
using UnityEditor.SceneManagement;

namespace TP_AchievementEditor
{
    [InitializeOnLoad]
    public class TPAchievementDesigner : EditorWindow
    {
        public static TPAchievementDesigner window;
        static string currentScene;

        [MenuItem("TP_Creator/TP_AchievementCreator")]
        public static void OpenWindow()
        {
            if (EditorApplication.isPlaying)
            {
                Debug.Log("You can't change Achievement Manager Designer runtime!");
                return;
            }
            window = (TPAchievementDesigner)GetWindow(typeof(TPAchievementDesigner));
            currentScene = EditorSceneManager.GetActiveScene().name;
            EditorApplication.hierarchyWindowChanged += hierarchyWindowChanged;
            window.minSize = new Vector2(615, 290);
            window.maxSize = new Vector2(615, 290);
            window.Show();
        }

        static void hierarchyWindowChanged()
        {
            if (currentScene != EditorSceneManager.GetActiveScene().name)
            {
                if (TPAchievementToolsWindow.window)
                    TPAchievementToolsWindow.window.Close();
                if (window)
                    window.Close();
            }
        }

        public static TPAchievementGUIData EditorData;
        public static TPAchievementCreator AchievementCreator;
        public static GUISkin skin;

        Texture2D headerTexture;
        Texture2D managerTexture;
        Texture2D toolTexture;

        Rect headerSection;
        Rect managerSection;
        Rect toolSection;

        bool existManager;
        bool toggleChange;

        public static SerializedObject creator;

        void OnEnable()
        {
            InitEditorData();
            InitTextures();
            InitCreator();

            if(AchievementCreator)
                creator = new SerializedObject(AchievementCreator);
        }

        void InitEditorData()
        {
            EditorData = AssetDatabase.LoadAssetAtPath(
                   "Assets/TP_Creator/TP_AchievementCreator/EditorResources/AchievementEditorGUIData.asset",
                   typeof(TPAchievementGUIData)) as TPAchievementGUIData;
            
            if (EditorData == null)
                CreateEditorData();
            else
                CheckGUIData();

            skin = EditorData.GUISkin;
        }

        void CheckGUIData()
        {
            if (EditorData.GUISkin == null)
                EditorData.GUISkin = AssetDatabase.LoadAssetAtPath(
                      "Assets/TP_Creator/TP_AchievementCreator/EditorResources/TPAchievementGUISkin.guiskin",
                      typeof(GUISkin)) as GUISkin;

            if (EditorData.AchievementsPath == null || EditorData.AchievementsPath.Length < 5)
                EditorData.AchievementsPath = "TP_Creator/TP_AchievementCreator/AchievementData/";

            EditorUtility.SetDirty(EditorData);
        }

        void CreateEditorData()
        {
            TPAchievementGUIData newEditorData = ScriptableObject.CreateInstance<TPAchievementGUIData>();
            AssetDatabase.CreateAsset(newEditorData, "Assets/TP_Creator/TP_AchievementCreator/EditorResources/AchievementEditorGUIData.asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorData = newEditorData;
            CheckGUIData();
        }

        void InitTextures()
        {
            Color colorHeader = new Color(0.19f, 0.19f, 0.19f);
            Color color = new Color(0.15f, 0.15f, 0.15f);

            headerTexture = new Texture2D(1, 1);
            headerTexture.SetPixel(0, 0, colorHeader);
            headerTexture.Apply();

            managerTexture = new Texture2D(1, 1);
            managerTexture.SetPixel(0, 0, color);
            managerTexture.Apply();

            toolTexture = new Texture2D(1, 1);
            toolTexture.SetPixel(0, 0, color);
            toolTexture.Apply();
        }

        static void InitCreator()
        {
            if (AchievementCreator == null)
            {
                AchievementCreator = FindObjectOfType<TPAchievementCreator>();

                if (AchievementCreator != null)
                    UpdateManager();
            }
        }

        void OnGUI()
        {
            if (EditorApplication.isPlaying)
            {
                if (TPAchievementToolsWindow.window)
                    TPAchievementToolsWindow.window.Close();
                this.Close();
            }
            DrawLayouts();
            DrawHeader();
            DrawManager();
            DrawTools();
        }

        void DrawLayouts()
        {
            headerSection = new Rect(0, 0, Screen.width, 50);
            managerSection = new Rect(0, 50, Screen.width / 2, Screen.height);
            toolSection = new Rect(Screen.width / 2, 50, Screen.width / 2, Screen.height);

            GUI.DrawTexture(headerSection, headerTexture);
            GUI.DrawTexture(managerSection, managerTexture);
            GUI.DrawTexture(toolSection, toolTexture);
        }

        void DrawHeader()
        {
            GUILayout.BeginArea(headerSection);
            GUILayout.Label("TP Achievement Creator - Manage your achievements!", skin.GetStyle("HeaderLabel"));
            GUILayout.EndArea();
        }

        void DrawManager()
        {
            GUILayout.BeginArea(managerSection);
            GUILayout.Label("Acheivement Manager - Core", skin.box);

            if (AchievementCreator == null)
            {
                InitializeManager();
            }
            else
            {
                ToggleDebugMode();
                ResetManager();

                if (GUILayout.Button("Refresh and update", skin.button, GUILayout.Height(70)))
                {
                    UpdateManager();
                }
            }

            GUILayout.EndArea();
        }

        void InitializeManager()
        {
            if (GUILayout.Button("Initialize New Manager", skin.button, GUILayout.Height(60)))
            {
                GameObject go = (new GameObject("TP_AchievementManager", typeof(TPAchievementCreator)));
                AchievementCreator = go.GetComponent<TPAchievementCreator>();
                UpdateManager();
                Debug.Log("Achievement Manager created!");
            }

            if (GUILayout.Button("Initialize Exist Manager", skin.button, GUILayout.Height(60)))
                existManager = !existManager;

            if (existManager)
                AchievementCreator = EditorGUILayout.ObjectField(AchievementCreator, typeof(TPAchievementCreator), true,
                    GUILayout.Height(30)) as TPAchievementCreator;
        }

        void ResetManager()
        {
            if (GUILayout.Button("Reset Manager", skin.button, GUILayout.Height(45)))
                AchievementCreator = null;
        }

        void ToggleDebugMode()
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Toggle Debug Mode", skin.button, GUILayout.Height(40)))
            {
                TPAchievementCreator.DebugMode = !TPAchievementCreator.DebugMode;
                if (TPAchievementToolsWindow.window)
                {
                    UpdateManager();
                    TPAchievementToolsWindow.window.Close();
                }
            }
            GUILayout.Toggle(TPAchievementCreator.DebugMode, GUIContent.none, GUILayout.Width(15));
            GUILayout.EndHorizontal();
        }

        public static void UpdateManager()
        {
            if (AchievementCreator)
            {
                AchievementCreator.Refresh();
                EditorUtility.SetDirty(AchievementCreator);
            }

            if (AchievementCreator)
                creator = new SerializedObject(AchievementCreator);

            if (creator != null)
            if (creator.targetObject != null)
            {
                creator.UpdateIfRequiredOrScript();
                creator.ApplyModifiedProperties();
            }
        }

        void DrawTools()
        {

            GUILayout.BeginArea(toolSection);
            GUILayout.Label("Achievement Manager - Tools", skin.box);

            if (AchievementCreator == null)
            {
                GUILayout.EndArea();
                return;
            }

            if (GUILayout.Button("Achievements", skin.button, GUILayout.Height(60)))
            {
                TPAchievementToolsWindow.OpenToolWindow(TPAchievementToolsWindow.Tool.Achievements);
            }
            if (GUILayout.Button("Notification", skin.button, GUILayout.Height(60)))
            {
                TPAchievementToolsWindow.OpenToolWindow(TPAchievementToolsWindow.Tool.Notification);
            }
            //if (GUILayout.Button("Layout", skin.button, GUILayout.Height(60)))
            //{
            //    TPAchievementToolsWindow.OpenToolWindow(TPAchievementToolsWindow.Tool.Layout);
            //}
            GUILayout.EndArea();
        }

    }
}