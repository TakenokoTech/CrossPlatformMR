using System.Linq;
using UnityEngine;
// using UnityEngine.XR.WSA.WebCam;
/*
namespace Project.Scripts.Runtime.camera
{
    public class PhotoCaptureMono : MonoBehaviour
    {
        private UnityEngine.Windows.WebCam.PhotoCapture _photoCaptureObject;
        private Texture2D _targetTexture;
        private static readonly int MainTex = Shader.PropertyToID("_MainTex");

        // Use this for initialization
        void Start()
        {
            var cameraResolution = UnityEngine.Windows.WebCam.PhotoCapture.SupportedResolutions
                .OrderByDescending((res) => res.width * res.height).First();
            _targetTexture = new Texture2D(cameraResolution.width, cameraResolution.height);

            // Create a PhotoCapture object
            UnityEngine.Windows.WebCam.PhotoCapture.CreateAsync(false,
                delegate(UnityEngine.Windows.WebCam.PhotoCapture captureObject)
                {
                    _photoCaptureObject = captureObject;
                    var cameraParameters =
                        new UnityEngine.Windows.WebCam.CameraParameters
                        {
                            hologramOpacity = 0.0f,
                            cameraResolutionWidth = cameraResolution.width,
                            cameraResolutionHeight = cameraResolution.height,
                            pixelFormat = UnityEngine.Windows.WebCam.CapturePixelFormat.BGRA32
                        };

                    // Activate the camera
                    _photoCaptureObject.StartPhotoModeAsync(cameraParameters,
                        delegate
                        {
                            // Take a picture
                            _photoCaptureObject.TakePhotoAsync(OnCapturedPhotoToMemory);
                        });
                });
        }

        void OnCapturedPhotoToMemory(UnityEngine.Windows.WebCam.PhotoCapture.PhotoCaptureResult result,
            UnityEngine.Windows.WebCam.PhotoCaptureFrame photoCaptureFrame)
        {
            // Copy the raw image data into our target texture
            photoCaptureFrame.UploadImageDataToTexture(_targetTexture);

            // Create a game object that we can apply our texture to
            var quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
            var quadRenderer = quad.GetComponent<Renderer>();
            quadRenderer.material = new Material(Shader.Find("Unlit/Texture"));

            quad.transform.parent = this.transform;
            quad.transform.localPosition = new Vector3(0.0f, 0.0f, 3.0f);

            quadRenderer.material.SetTexture(MainTex, _targetTexture);

            // Deactivate our camera
            _photoCaptureObject.StopPhotoModeAsync(OnStoppedPhotoMode);
        }

        void OnStoppedPhotoMode(UnityEngine.Windows.WebCam.PhotoCapture.PhotoCaptureResult result)
        {
            // Shutdown our photo capture resource
            _photoCaptureObject.Dispose();
            _photoCaptureObject = null;
        }
    }
}
*/