using UnityEngine;

namespace Project.Scripts.Runtime.transition
{
    [CreateAssetMenu(menuName = "Project/Create ModeData")] 
    public class ModeData: ScriptableObject
    {
        public string thisSceneName;
        public string nextSceneName;
    }
}
