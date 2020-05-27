using System;
using UnityEditor;
using UnityEngine;

namespace Project.Scripts.Runtime.utils
{
    [Serializable]
    public class SceneObject
    {
        [SerializeField] private string sceneName;
        public static implicit operator string(SceneObject sceneObject) => sceneObject.sceneName;
        public static implicit operator SceneObject(string sceneName) => new SceneObject {sceneName = sceneName};
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(SceneObject))]
    public class SceneObjectEditor : PropertyDrawer
    {
        private const string SceneName = "sceneName";
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var sceneObj = GetSceneObject(property.FindPropertyRelative(SceneName).stringValue);

            var newSceneName = EditorGUI.ObjectField(position, label, sceneObj, typeof(SceneAsset), false)?.name;
            
            if (newSceneName == null)
            {
                property.FindPropertyRelative(SceneName).stringValue = "";
                return;
            }
            
            if (newSceneName == property.FindPropertyRelative(SceneName).stringValue)
            {
                return;
            }

            if (GetSceneObject(newSceneName) == null)
            {
                Debug.LogWarning($"The scene {newSceneName} cannot be used.");
            }

            property.FindPropertyRelative(SceneName).stringValue = newSceneName;
        }

        private static SceneAsset GetSceneObject(string sceneObjectName)
        {
            if (string.IsNullOrEmpty(sceneObjectName))
                return null;

            foreach (var scene in EditorBuildSettings.scenes)
            {
                if (scene.path.IndexOf(sceneObjectName, StringComparison.Ordinal) != -1)
                    return AssetDatabase.LoadAssetAtPath(scene.path, typeof(SceneAsset)) as SceneAsset;
            }

            Debug.Log(
                $"Scene [{sceneObjectName}] cannot be used. Add this scene to the 'Scenes in the Build' in the build settings.");
            return null;
        }
    }
    
#endif
}