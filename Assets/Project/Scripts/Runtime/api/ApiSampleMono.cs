using System.Collections.Generic;
using Project.Scripts.Runtime.utils;
using UnityEngine;

namespace Project.Scripts.Runtime.api
{
    public class ApiSampleMono : MonoBehaviour
    {
        private const string Tag = "ApiSampleMono";
        
        // Start is called before the first frame update
        async void Start()
        {
            var response = await ApiBuilder.Get<Dictionary<string,string>>("http://127.0.0.1:8887/sample.json");
            Log.D(Tag, response.statusCode.ToString());
            Log.D(Tag, response.data.Dump());
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
