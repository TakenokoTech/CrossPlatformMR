using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using Project.Scripts.Runtime.api;
using UnityEngine;
using UnityEngine.TestTools;

namespace Project.Tests.Runtime.api
{
    public class ApiBuilderTest
    {
        [Test]
        public void ArrayStringTest()
        {
            const string jsonText = "[\"value1\",\"value2\",\"value3\"]";
            var jsonObj = MiniJson.Deserialize(jsonText) as List<object>;
            Assert.AreEqual(jsonObj?[0], "value1");
            Assert.AreEqual(jsonObj?[1], "value2");
            Assert.AreEqual(jsonObj?[2], "value3");
            var str = MiniJson.Serialize(jsonObj);
            Assert.AreEqual(str, jsonText);
        }

        [Test]
        public void DictionaryTest()
        {
            const string jsonText = "{\"key1\":\"value1\",\"key2\":\"value2\",\"key3\":\"value3\"}";
            var jsonObj = MiniJson.Deserialize(jsonText) as Dictionary<string, object>;
            var str = MiniJson.Serialize(jsonObj);
            Assert.AreEqual(str, jsonText);
        }

        [Test]
        public void JsonParseTest()
        {
            const string jsonText1 = "[\"value1\",\"value2\",\"value3\"]";
            var json1 = JsonParser.List<string>().Parse(jsonText1);
            Assert.AreEqual(json1?[0], "value1");
            Assert.AreEqual(json1?[1], "value2");
            Assert.AreEqual(json1?[2], "value3");

            const string jsonText2 = "{\"key1\":\"value1\",\"key2\":\"value2\",\"key3\":\"value3\"}";
            var json2 = JsonParser.Dict<string, string>().Parse(jsonText2);
            Assert.AreEqual(json2?["key1"], "value1");
            Assert.AreEqual(json2?["key2"], "value2");
            Assert.AreEqual(json2?["key3"], "value3");

            const string jsonText3 = "{\"key1\":\"value1\",\"key2\":\"value2\",\"key3\":\"value3\"}";
            var json3 = JsonParser.Default<TestJson>().Parse(jsonText3);
            Assert.AreEqual(json3?.key1, "value1");
            Assert.AreEqual(json3?.key2, "value2");
            Assert.AreEqual(json3?.key3, "value3");
        }
        
        [Serializable]
        private class TestJson
        {
            public string key1;
            public string key2;
            public string key3;
        }
    }
}