#if UNITY_WSA
using System.Diagnostics.CodeAnalysis;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project.Scripts.Runtime.transition
{
    public class SceneManagerMono : MonoBehaviour
    {
        private const string SceneRoot = "TranstionSampleScene";
        private const string Scene1 = "SampleScene1";
        private const string Scene2 = "SampleScene2";
        private const string Scene3 = "SampleScene3";

        [SerializeField] private string thisSceneName;
        [SerializeField] private string nextSceneName;

        public void Next()
        {
            for (var i = 0; i < SceneManager.sceneCount ; i++) {
                var sceneName = SceneManager.GetSceneAt(i).name;
                if (sceneName == nextSceneName) return;
            }
            // SceneManager.LoadScene(Scene1, LoadSceneMode.Additive);
            if(!string.IsNullOrEmpty(thisSceneName)) SceneManager.UnloadSceneAsync(thisSceneName);
            if(!string.IsNullOrEmpty(nextSceneName)) SceneManager.LoadScene(nextSceneName, LoadSceneMode.Additive);
        }

        public void Finish()
        {
            SceneManager.LoadScene(SceneRoot, LoadSceneMode.Single);
        }
    }
    
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
}
#endif