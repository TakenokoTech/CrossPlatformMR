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

        /**
         * After calling Start().
         */
        internal PhotoCaptureModel()
        {
            resolution = PhotoCapture
                .SupportedResolutions
                .OrderByDescending(res => res.width * res.height)
                .First(res => res.width * res.height <= 1280 * 720);
            texture = new Texture2D(resolution.width, resolution.height);
        }

        internal async Task<PhotoCapture.PhotoCaptureResult> PhotoCaptureCreate()
        {
            var completed = new TaskCompletionSource<PhotoCapture.PhotoCaptureResult>();
            Log.D(Tag, $"Create a PhotoCapture object. {resolution.ToString()}");
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

        internal async Task<PhotoCapture.PhotoCaptureResult> StopPhoto()
        {
            Log.D(Tag, $"StopPhoto");
            var completed = new TaskCompletionSource<PhotoCapture.PhotoCaptureResult>();
            photoCaptureObject.StopPhotoModeAsync(result =>
            {
                Log.D(Tag, $"OnPhotoModeStoppedCallback. success={result.success}");
                photoCaptureObject?.Dispose();
                photoCaptureObject = null;
                completed.SetResult(result);
            });
            return await completed.Task;
        }

        internal void Cancel()
        {
            Log.D(Tag, "Cancel");
            photoCaptureObject?.Dispose();
            photoCaptureObject = null;
        }
    }
}