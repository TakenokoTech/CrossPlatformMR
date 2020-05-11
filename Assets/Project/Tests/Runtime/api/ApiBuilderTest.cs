using System.Collections.Generic;
using NUnit.Framework;
using Project.Scripts.Runtime.api;

namespace Project.Tests.Runtime.api
{
    public class ApiBuilderTest
    {
        [Test]
        public void ArrayString()
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
        public void Dictionary()
        {
            const string jsonText = "{\"key1\":\"value1\",\"key2\":\"value2\",\"key3\":\"value3\"}";
            var jsonObj = MiniJson.Deserialize(jsonText) as Dictionary<string,object>;
            var str = MiniJson.Serialize(jsonObj);
            Assert.AreEqual(str, jsonText);
        }

        [Test]
        public void JsonParseTest()
        {
            const string jsonText1 = "[\"value1\",\"value2\",\"value3\"]";
            var json1 = jsonText1.ListParse<string>();
            Assert.AreEqual(json1?[0], "value1");
            Assert.AreEqual(json1?[1], "value2");
            Assert.AreEqual(json1?[2], "value3");
            
            const string jsonText2 = "{\"key1\":\"value1\",\"key2\":\"value2\",\"key3\":\"value3\"}";
            var json2 = jsonText2.DictParse<string>();
            Assert.AreEqual(json2?["key1"], "value1");
            Assert.AreEqual(json2?["key2"], "value2");
            Assert.AreEqual(json2?["key3"], "value3");
        }
    }
}
