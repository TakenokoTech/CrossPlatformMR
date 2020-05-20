#if UNITY_WSA
using System;
using System.Linq;
using System.Threading.Tasks;
using Project.Scripts.Runtime.utils;
using UnityEngine;
using UnityEngine.Windows.WebCam;

namespace Project.Scripts.Runtime.camera
{
    internal class PhotoCaptureModel
    {
        private const string Tag = "PhotoCaptureModel";

        internal readonly Texture2D texture;
        internal readonly Resolution resolution;
        
        private PhotoCapture photoCaptureObject;
        private const int MaxSize = 1920 * 1080; // or 1280 * 720

        /**
         * After calling Start().
         */
        internal PhotoCaptureModel()
        {
            resolution = PhotoCapture
                .SupportedResolutions
                .OrderByDescending(res => res.width * res.height)
                .First(res => res.width * res.height <= MaxSize);
            texture = new Texture2D(resolution.width, resolution.height);
        }

        internal async Task<PhotoCapture.PhotoCaptureResult> PhotoCaptureCreate()
        {
            var completed = new TaskCompletionSource<PhotoCapture.PhotoCaptureResult>();
            Log.D(Tag, $"Create a PhotoCapture object. {resolution}");
            PhotoCapture.CreateAsync(captureObject =>
            {
                photoCaptureObject = captureObject;
                photoCaptureObject.StartPhotoModeAsync(new CameraParameters
                {
                    hologramOpacity = 0.0f,
                    cameraResolutionWidth = resolution.width,
                    cameraResolutionHeight = resolution.height,
                    pixelFormat = CapturePixelFormat.BGRA32
                }, result =>
                {
                    Log.D(Tag, $"OnPhotoModeStartedCallback. success={result.success}");
                    completed.SetResult(result);
                });
            });
            return await completed.Task;
        }

        internal async Task<PhotoCapture.PhotoCaptureResult> TakePhoto()
        {
            Log.D(Tag, $"TakePhoto");
            var completed = new TaskCompletionSource<PhotoCapture.PhotoCaptureResult>();
            photoCaptureObject.TakePhotoAsync((result, photoCaptureFrame) =>
            {
                Log.D(Tag, $"OnCapturedToMemoryCallback. success={result.success}");
                photoCaptureFrame.UploadImageDataToTexture(texture);
                completed.SetResult(result);
            });
            return await completed.Task;
        }

        // ReSharper disable once AccessToDisposedClosure
        internal async Task<PhotoCapture.PhotoCaptureResult> StopPhoto()
        {
            Log.D(Tag, $"StopPhoto");
            var completed = new TaskCompletionSource<PhotoCapture.PhotoCaptureResult>();
            try
            {
                photoCaptureObject.StopPhotoModeAsync(result =>
                {
                    Log.D(Tag, $"OnPhotoModeStoppedCallback. success={result.success}");
                    photoCaptureObject?.Dispose();
                    photoCaptureObject = null;
                    completed.SetResult(result);
                });
            }
            catch (Exception e)
            {
                Log.D(Tag, e.ToString());
                photoCaptureObject?.Dispose();
                photoCaptureObject = null;
            }
            return await completed.Task;
        }
    }
}
#endif