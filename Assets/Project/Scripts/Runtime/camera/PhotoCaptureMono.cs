using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Project.Scripts.Runtime.utils;
using UnityEditor;
using UnityEngine;
using UnityEngine.Windows.WebCam;

namespace Project.Scripts.Runtime.camera
{
    public class PhotoCaptureMono : MonoBehaviour
    {
        private const string Tag = "PhotoCaptureMono";

        [Space(16)]
        [SerializeField] private Material material;

        internal PhotoCaptureModel photoCaptureModel;
        private static readonly int MainTex = Shader.PropertyToID("_MainTex");

        async void Start()
        {
            Log.D(Tag, "Start");

            photoCaptureModel = new PhotoCaptureModel();
            var result = await photoCaptureModel.PhotoCaptureCreate();
            if (!result.success) return;
            
            // CreateQuad();
            material.SetTexture(MainTex, photoCaptureModel.texture);

            await photoCaptureModel.TakePhoto();
        }

        private async void OnDestroy()
        {
            Log.D(Tag, "OnDestroy");
            await photoCaptureModel.StopPhoto();
        }

        private void CreateQuad()
        {
            Log.D(Tag, "CreateQuad");
            GameObject.CreatePrimitive(PrimitiveType.Quad).Apply(quad =>
            {
                quad.transform.parent = transform;
                quad.transform.localPosition = new Vector3(0.0f, 0.0f, 1.0f);
                quad.transform.localScale = new Vector3(
                    (float) photoCaptureModel.resolution.width / photoCaptureModel.resolution.height, 1, 1);
                quad.GetComponent<Renderer>().Apply(render =>
                {
                    render.material = new Material(Shader.Find("Unlit/Texture"));
                    render.material.SetTexture(MainTex, photoCaptureModel.texture);
                });
            });
        }
    }

    [CustomEditor(typeof(PhotoCaptureMono))]
    [SuppressMessage("ReSharper", "Unity.NoNullPropagation")]
    public class ExampleScriptEditor : Editor {
        public override void OnInspectorGUI(){
            base.OnInspectorGUI ();
            var photoCaptureMono = target as PhotoCaptureMono;
            if (GUILayout.Button("Take Photo")){
                photoCaptureMono?.photoCaptureModel?.TakePhoto();
            }  
        }
    } 
}