using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using OpenCvSharp;
using OpenCvSharp.Dnn;
using Project.Scripts.Runtime.utils;
using UnityEngine;
using UnityEngine.UI;
using Rect = UnityEngine.Rect;

namespace Project.Scripts.Runtime.camera
{
    [SuppressMessage("ReSharper", "Unity.PerformanceCriticalCodeInvocation")]
    public class WebCamMono : MonoBehaviour
    {
        private const string Tag = "WebCamMono";

        [SerializeField] private RawImage rawImage;
        [SerializeField] private Image image;
        [SerializeField] private Text text; 

        private WebCamTexture webCamTexture;
        private Texture2D takenPhoto;
        private Net model;

        private const int MaxWidth = 0;
        private const int MaxHeight = 0;

        private void Start()
        {
            LoadModel("vgg19-7");
            SetupWebCam();
        }
        private void Update()
        {
            UpdateWebCam();
            image.sprite = Sprite.Create(takenPhoto, new Rect(0, 0, takenPhoto.width, takenPhoto.height), Vector2.zero);
            Input(takenPhoto);
        }

        private void SetupWebCam()
        {
            webCamTexture = new WebCamTexture();
            rawImage.texture = webCamTexture;
            webCamTexture.requestedHeight = MaxHeight;
            webCamTexture.requestedWidth = MaxWidth;
            webCamTexture.Play();
        }
        private void UpdateWebCam()
        {
            takenPhoto = new Texture2D(webCamTexture.width, webCamTexture.height, TextureFormat.ARGB32, false).Apply(
                it =>
                {
                    it.SetPixels(webCamTexture.GetPixels());
                    it.Apply();
                });
        }
        
        private void LoadModel(string modelName)
        {
            try
            {
                var modelPath = Application.dataPath + @"/onnx/" + modelName + ".onnx";
                Log.D(Tag, $"modelPath = {modelPath}");
                model = Net.ReadNetFromONNX(modelPath);
                Log.D(Tag, $"model={model}");
            }
            catch (Exception e)
            {
                Log.D(Tag, $"{e}");
            }
        }
        private bool isFinished = true;
        private void Input(Texture2D input)
        {
            if (model == null || isFinished == false) return;
            Log.D(Tag, $"Input");
            isFinished = false;
            try
            {
                var mat = input.ToMat();
                var mean = new Scalar(0.485f, 0.456f, 0.406f);
                var size = new Size(224, 224);
                const double scaleFactor = 1.0 / 255.0 / 0.225f;
                var blob = CvDnn.BlobFromImage(mat, scaleFactor, size, mean, true, false);
                model.SetInput(blob);
                var scores = model.Forward();
                // Log.D(Tag, $"score: {scores}");
                text.text = GetBestScorePos(scores);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                isFinished = true;
            }
        }
        private string GetBestScorePos(Mat score)
        {
            var key = 0;
            var max = 0f;
            for (var i = 0; i < score.Width; i++)
            {
                var value = score.Get<float>(0, i);
                if(value > 3.0) 
                    Log.D(Tag,  $"{i}: {InitializeClassification()[i][0]}, {value}");
                if (!(value > max)) continue;
                key = i;
                max = value;
            }
            Log.D(Tag, InitializeClassification()[key][0]);

            return InitializeClassification()[key][0];
        }
        private List<string[]> InitializeClassification()
        {
            var data = new List<string[]>();
            
            var csvFile = Resources.Load("classification") as TextAsset;
            if (csvFile == null) return data;
            var reader = new StringReader(csvFile.text);
            while (reader.Peek() > -1)
                data.Add(reader.ReadLine()?.Split(','));
            
            return data;
        }
    }

    internal static class OpenCvImage
    {
        public static Mat ToMat(this RenderTexture renderTexture)
        {
            // var renderTexture = new RenderTexture(256, 256, 0, RenderTextureFormat.ARGB32);
            var tex = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false, false);
            RenderTexture.active = renderTexture;
            tex.ReadPixels(new Rect(0, 0, tex.width, tex.height), 0, 0);
            tex.Apply();
            RenderTexture.active = null;
            return new Mat(tex.height, tex.width, MatType.CV_8UC3, tex.GetRawTextureData());
        }
        public static Mat ToMat(this Texture2D texture2D)
        {
            // return new Mat(texture2D.height, texture2D.width, MatType.CV_8UC3, texture2D.GetRawTextureData());
            var c = texture2D.GetPixels32();
            var mat = new Mat(texture2D.width, texture2D.height, MatType.CV_8UC3);
            var sourceImageData = new Vec3b[texture2D.width * texture2D.height];
            for (var i = 0; i < texture2D.width; i++)
            for (var j = 0; j < texture2D.height; j++)
            {
                var col = c[i + j * 224];
                var vec3 = new Vec3b
                {
                    Item0 = col.b,
                    Item1 = col.g,
                    Item2 = col.r
                };
                sourceImageData[i + j * texture2D.height] = vec3;
            }
            mat.SetArray(sourceImageData);
            return mat.Flip(FlipMode.X);
        }
    }
}