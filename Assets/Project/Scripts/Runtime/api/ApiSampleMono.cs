using System;
using System.Collections.Generic;
using Project.Scripts.Runtime.utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace Project.Scripts.Runtime.api
{
    public class ApiSampleMono : MonoBehaviour
    {
        private const string Tag = "ApiSampleMono";
        
        // Start is called before the first frame update
        async void Start()
        {
            var response1 = await ApiBuilder.Get("http://127.0.0.1:8080/sample.json", JsonParser.Dict<string, string>());
            Log.D(Tag, $"{response1.statusCode}");
            Log.D(Tag, $"key1 = {response1.data?["key1"]}");
            Log.D(Tag, $"key2 = {response1.data?["key2"]}");
            Log.D(Tag, $"key3 = {response1.data?["key3"]}");
            
            var response2 = await ApiBuilder.Get<TestJson>("http://127.0.0.1:8080/sample.json");
            Log.D(Tag, $"{response2.statusCode}");
            Log.D(Tag, $"key1 = {response2.data?.key1}");
            Log.D(Tag, $"key2 = {response2.data?.key2}");
            Log.D(Tag, $"key3 = {response2.data?.key3}");
        }

        // Update is called once per frame
        void Update()
        {
        
        }
        
        [Serializable]
        public class TestJson
        {
            public string key1;
            public string key2;
            public string key3;
        }
    }
}
