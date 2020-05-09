using Project.Scripts.Runtime.utils;
using UnityEditor;
using UnityEngine.Windows.WebCam;

namespace Project.Scripts.Editor.camera
{
    public static class PhotoCaptureClass
    {
        private const string Tag = "PhotoCaptureClass";

        [MenuItem("Project/Check PhotoCapture.SupportedResolutions")]
        public static void CheckSupportedResolutions()
        {
            Log.D(Tag, "Check PhotoCapture.SupportedResolutions");
            foreach (var a in PhotoCapture.SupportedResolutions)
            {
                Log.D(Tag, a.ToString());
            }
        }
    }
}