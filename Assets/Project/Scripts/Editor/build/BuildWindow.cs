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
        private static readonly Dictionary<string, bool> AllScene = new Dictionary<string, bool>();
        private ReorderableList draggableList;

        [MenuItem("Project/Show build window")]
        public static void ShowWindow()
        {
            GetWindow<BuildWindow>("Build Window");
        }

        private void OnEnable()
        {
            buildConfig = LoadConfig();
            if (buildConfig == null) return;
            
            sceneOrder = buildConfig.scene;
            
            foreach (var path in LoadALLScene())
            {
                if (!AllScene.ContainsKey(path))
                    AllScene.Add(path, buildConfig.scene.Contains(path));
                else
                    AllScene[path] = buildConfig.scene.Contains(path);
            }
            
            draggableList = new ReorderableList(sceneOrder, typeof(string), true, false, false, false);
        }

        public void OnGUI()
        {
            GUILayout.Space(4);
            
            EditorGUILayout.LabelField("ビルド設定", EditorStyles.label);
            foreach (var pair in new Dictionary<string, bool>(AllScene))
            {
                AllScene[pair.Key] = EditorGUILayout.ToggleLeft(pair.Key, pair.Value);
            }
            
            GUILayout.Space(16);
            
            EditorGUILayout.LabelField("シーン順序", EditorStyles.label);
            {
                foreach (var pair in AllScene)
                {
                    if(pair.Value && !sceneOrder.Contains(pair.Key)) sceneOrder.Add(pair.Key);
                    if(!pair.Value && sceneOrder.Contains(pair.Key)) sceneOrder.Remove(pair.Key);
                }
                draggableList.DoLayoutList();
            }
            
            if(GUILayout.Button( "設定ファイル更新" )) {
                buildConfig.scene = sceneOrder; // (from pair in allScene where pair.Value select pair.Key).ToList();
                Debug.Log( "更新しました。" );
            }
        }
        
        private static BuildConfigData LoadConfig()
        {
            return (BuildConfigData)AssetDatabase
                .FindAssets("t:ScriptableObject")
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(path => AssetDatabase.LoadAssetAtPath(path, typeof(BuildConfigData)))
                .FirstOrDefault(obj => obj != null);
        }

        private static List<string> LoadALLScene()
        {
            return AssetDatabase.FindAssets("t:Scene", new[] {"Assets/Project"}).Select(AssetDatabase.GUIDToAssetPath).ToList();
        }
    }
    
    [CreateAssetMenu(menuName = "Project/Create BuildConfig")]
    public class BuildConfigData : ScriptableObject
    {
        [SerializeField] public List<string> scene;
    }
}