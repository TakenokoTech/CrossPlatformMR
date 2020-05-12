using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Project.Scripts.Runtime.api
{
    public static class JsonParser {
        public static Parser<T> Default<T>() => new DefaultParser<T>();
        public static Parser<List<T>> List<T>() where T : class => new ListParser<T>();
        public static Parser<Dictionary<TKey, TValue>> Dict<TKey, TValue>() where TKey : class where TValue : class => new DictParser<TKey, TValue>();
        
        public abstract class Parser<T>
        {
            public abstract T Parse(string jsonText);
        }

        private class DefaultParser<T> : Parser<T>
        { 
            public override T Parse(string jsonText) => JsonUtility.FromJson<T>(jsonText);
        }

        private class ListParser<T> : Parser<List<T>> where T : class
        { 
            public override List<T> Parse(string jsonText) => (MiniJson.Deserialize(jsonText)as List<object>)?.ConvertAll(x => x as T);
        }

        private class DictParser<TKey, TValue> : Parser<Dictionary<TKey, TValue>> where TKey : class where TValue : class
        { 
            public override Dictionary<TKey, TValue> Parse(string jsonText) => (MiniJson.Deserialize(jsonText) as Dictionary<string, object>)?.ToDictionary(
                k => k.Key as TKey,v=> v.Value as TValue);
        }
        
        // public static List<T> ListParse<T>(this string jsonText) where T : class
        // {
        //     return (MiniJson.Deserialize(jsonText)as List<object>)?.ConvertAll(x => x as T);
        // }
        //
        // public static Dictionary<TKey, TValue> DictParse<TKey, TValue>(this string jsonText) where TKey : class where TValue : class
        // {
        //     return (MiniJson.Deserialize(jsonText) as Dictionary<string, object>)?.ToDictionary(
        //         k => k.Key as TKey,v=> v.Value as TValue);
        // }
        //
        // public static T JsonParse<T>(this string jsonText) where T : class
        // {
        //     return JsonUtility.FromJson<T>(jsonText);
        // }
    }
}