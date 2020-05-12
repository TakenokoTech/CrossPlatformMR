using System.Net;
using System.Text;
using Project.Scripts.Runtime.utils;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Scripts.Runtime.api
{
    public class HostSampleMono : MonoBehaviour
    {
        [SerializeField] private bool startOnAwake = true;
        [SerializeField] private string assetSrc;
        
        private readonly HttpListener httpListener = new HttpListener();
        private TextAsset textAsset;

        void Start()
        {
            textAsset = Resources.Load(assetSrc) as TextAsset;
            httpListener.Prefixes.Add($"http://*:{8080}/");
            if (startOnAwake) StartServer();
        }
        
        private async void StartServer()
        {
            httpListener.Start();
            while (true)
            {
                var context = await httpListener.GetContextAsync();
                using (var res = context.Response)
                {
                    res.StatusCode = 200;
                    res.ContentType = "application/json";
                    var content = Encoding.UTF8.GetBytes(textAsset?.text ?? "");
                    await res.OutputStream.WriteAsync(content, 0, content.Length);
                }
            }
        }
    
        // 破棄時にサーバーを止める
        void OnDestroy()
        {
            httpListener.Stop();
        }
    }
}
