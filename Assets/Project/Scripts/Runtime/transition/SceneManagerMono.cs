using System.Diagnostics.CodeAnalysis;
using Project.Scripts.Runtime.utils;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project.Scripts.Runtime.transition
{
    public class SceneManagerMono : MonoBehaviour
    {
        private const string Tag = "SceneManagerMono";
        
        [SerializeField] private SceneObject rootSceneName;
        [SerializeField] private SceneObject thisSceneName;
        [SerializeField] private SceneObject nextSceneName;
        [SerializeField] private bool onAwake;
        
        private void Awake()
        {
            if (onAwake) Next();
        }

        public void Next()
        {
            for (var i = 0; i < SceneManager.sceneCount ; i++) {
                var sceneName = SceneManager.GetSceneAt(i).name;
                if (sceneName == nextSceneName) return;
            }
            
            SceneManager.sceneLoaded += GameSceneLoaded;
            // SceneManager.LoadScene(Scene1, LoadSceneMode.Additive);
            if(!string.IsNullOrEmpty(thisSceneName)) SceneManager.UnloadSceneAsync(thisSceneName);
            if(!string.IsNullOrEmpty(nextSceneName)) SceneManager.LoadScene(nextSceneName, LoadSceneMode.Additive);
        }

        public void Finish()
        {
            SceneManager.LoadScene(rootSceneName, LoadSceneMode.Single);
        }
        
        private static void GameSceneLoaded(Scene next, LoadSceneMode mode)
        {
            Log.D(Tag, $"GameSceneLoaded. {next.name}-{mode}");
            SceneManager.sceneLoaded -= GameSceneLoaded;
        }
    }
    
#if UNITY_EDITOR
    [CustomEditor(typeof(SceneManagerMono))]
    [SuppressMessage("ReSharper", "Unity.NoNullPropagation")]
    public class SceneManagerMonoEditor : Editor {
        public override void OnInspectorGUI(){
            base.OnInspectorGUI ();
            var sceneManagerMono = target as SceneManagerMono;
            if (GUILayout.Button("Next"))
            {
                sceneManagerMono?.Next();
            }
            if (GUILayout.Button("Finish")){
                sceneManagerMono?.Finish();
            } 
        }
    } 
#endif

}
