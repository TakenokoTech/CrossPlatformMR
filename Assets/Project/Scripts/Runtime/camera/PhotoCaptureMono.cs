using System;
using System.Linq;
using System.Threading.Tasks;
using Project.Scripts.Runtime.utils;
using UnityEngine;
using UnityEngine.Windows.WebCam;

namespace Project.Scripts.Runtime.camera
{
    public class PhotoCaptureMono : MonoBehaviour
    {
        private const string Tag = "PhotoCaptureMono";

        private PhotoCaptureModel photoCaptureModel;
        private static readonly int MainTex = Shader.PropertyToID("_MainTex");

        async void Start()
        {
            Log.D(Tag, "Start");

            photoCaptureModel = new PhotoCaptureModel();
            var result = await photoCaptureModel.PhotoCaptureCreate();
            if (!result.success) return;
            
            CreateQuad();

            await photoCaptureModel.TakePhoto();
            await photoCaptureModel.StopPhoto();
        }

        private void OnDestroy()
        {
            Log.D(Tag, "OnDestroy");
            photoCaptureModel.Cancel();
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
}