using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Project.Scripts.Runtime.exception;
using Project.Scripts.Runtime.utils;
using UnityEngine;
using UnityEngine.Networking;

namespace Project.Scripts.Runtime.api
{
    public static class ApiBuilder
    {
        private const string Tag = "ApiBuilder";
        
        public static async Task<ApiResponse<T>> Get<T>(string path) where T : class
        {
            var request = UnityWebRequest.Get(path);
            await request.SendWebRequest();

            if (request.isHttpError || request.isNetworkError)
            {
                Log.D(Tag, request.error);
                throw new ApiException(request.error);
            }
            
            return new ApiResponse<T>
            {
                statusCode = request.responseCode,
                data = request.downloadHandler.text.JsonParse<T>()
            };
        }

        public static List<T> ListParse<T>(this string jsonText) where T : class
        {
            return (MiniJson.Deserialize(jsonText)as List<object>)?.ConvertAll(x => x as T);
        }
        
        public static Dictionary<string, T> DictParse<T>(this string jsonText) where T : class
        {
            return (MiniJson.Deserialize(jsonText) as Dictionary<string, object>)?.ToDictionary(
                k => k.Key,v=> v.Value as T);
        }
        
        public static T JsonParse<T>(this string jsonText) where T : class
        {
            return JsonUtility.FromJson<T>(jsonText);
        }

        public struct ApiResponse<T>
        {
            internal long statusCode;
            internal T data;
        }
    }
}
