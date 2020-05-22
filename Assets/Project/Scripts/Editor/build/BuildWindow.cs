using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Project.Scripts.Editor.build
{
    public class BuildWindow : EditorWindow
    {
        private const string Tag = "BuildWindow";

        private static BuildConfigData buildConfig;
        private static List<string> sceneOrder = new List<string>();
        private static Dictionary<string, bool> allScene = new Dictionary<string, bool>();
        
        private ReorderableList draggableList;

        [MenuItem("Project/Show build window")]
        public static void ShowWindow()
        {
            GetWindow<BuildWindow>("Build Window");
        }

        private void OnEnable()
        {
            var config = LoadConfig();
            if (config != null) UpdateConfig(config);
            
            draggableList = new ReorderableList(sceneOrder, typeof(string), true, false, false, false);
        }

        public void OnGUI()
        {
            EditorGUILayout.Separator();
            
            EditorGUILayout.LabelField("ビルド設定", EditorStyles.label);
            
            EditorGUILayout.BeginHorizontal();
            var newConfig = (BuildConfigData)EditorGUILayout.ObjectField(buildConfig, typeof(BuildConfigData), true);
            if (newConfig != buildConfig)
            {
                UpdateConfig(newConfig);
            }

            if (GUILayout.Button("上書き", GUILayout.Width(80)))
            {
                buildConfig.scene = new List<string>(sceneOrder);
                buildConfig.updateDate = DateTime.Now.Ticks;
                Debug.Log("update done.");
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();
            
            EditorGUILayout.LabelField("シーン選択", EditorStyles.label);
            
            foreach (var pair in new Dictionary<string, bool>(allScene))
            {
                allScene[pair.Key] = EditorGUILayout.ToggleLeft(pair.Key, pair.Value);
            }

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            EditorGUILayout.LabelField("シーン順序", EditorStyles.label);
            foreach (var pair in allScene)
            {
                if (pair.Value && !sceneOrder.Contains(pair.Key)) sceneOrder.Add(pair.Key);
                if (!pair.Value && sceneOrder.Contains(pair.Key)) sceneOrder.Remove(pair.Key);
            }

            draggableList.headerHeight = 0;
            draggableList.footerHeight = 0;
            draggableList.DoLayoutList();
            
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();
            
            EditorGUILayout.LabelField("ビルド", EditorStyles.label);
            EditorGUILayout.BeginHorizontal ();
            if (GUILayout.Button("UWP ビルド"))
            {
                BuildClass.Build(sceneOrder.ToArray(), "UWP", BuildTarget.WSAPlayer, "");
            }
            
            if (GUILayout.Button("Android ビルド"))
            {
                BuildClass.Build(sceneOrder.ToArray(),"Android", BuildTarget.Android, "CrossPlatformMR.apk");
            }
            EditorGUILayout.EndHorizontal ();
        }

        private static BuildConfigData LoadConfig()
        {
            return AssetDatabase
                .FindAssets("t:ScriptableObject")
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(path => AssetDatabase.LoadAssetAtPath(path, typeof(BuildConfigData)))
                .Select(obj => (BuildConfigData) obj)
                .OrderByDescending(obj => obj.updateDate)
                .FirstOrDefault(obj => obj != null);
        }

        private static IEnumerable<string> LoadAllScene()
        {
            return AssetDatabase
                .FindAssets("t:Scene", new[] {"Assets/Project"})
                .Select(AssetDatabase.GUIDToAssetPath)
                .ToList();
        }
        
        private static void UpdateConfig(BuildConfigData config)
        {
            buildConfig = config;
            sceneOrder = new List<string>(buildConfig.scene);
            allScene = new Dictionary<string, bool>();
                
            foreach (var path in LoadAllScene())
            {
                if (!allScene.ContainsKey(path)) allScene.Add(path, buildConfig.scene.Contains(path));
                else allScene[path] = buildConfig.scene.Contains(path);
            }
        }
    }

    [CreateAssetMenu(menuName = "Project/Create BuildConfig")]
    public class BuildConfigData : ScriptableObject
    {
        [SerializeField] public List<string> scene;
        [SerializeField] public long updateDate;
    }
}