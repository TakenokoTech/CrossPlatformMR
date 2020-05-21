using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project.Scripts.Runtime.transition
{
    public class MainSceneManagerMono : MonoBehaviour
    {
        private const string SceneRoot = "TranstionSampleScene";
        private const string Scene1 = "SampleScene1";
        private const string Scene2 = "SampleScene2";
        private const string Scene3 = "SampleScene3";

        [SerializeField] private string thisSceneName;
        [SerializeField] private string nextSceneName;

        private void Awake()
        {
            if(!string.IsNullOrEmpty(nextSceneName)) SceneManager.LoadScene(nextSceneName, LoadSceneMode.Additive);
        }
    }
}
