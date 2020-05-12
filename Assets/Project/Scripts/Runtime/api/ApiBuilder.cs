using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Project.Scripts.Runtime.exception;
using Project.Scripts.Runtime.utils;
using UnityEngine.Networking;

namespace Project.Scripts.Runtime.api
{
    public static class ApiBuilder
    {
        private const string Tag = "ApiBuilder";

        public static async Task<ApiResponse<T>> Get<T>(string path, JsonParser.Parser<T> parser = null) where T : class
        {
            var request = UnityWebRequest.Get(path);
            await request.SendWebRequest();

            if (request.isHttpError || request.isNetworkError)
            {
                Log.D(Tag, request.error);
                throw new ApiException(request.error);
            }

            Log.D(Tag, "===> " + Trim(request.downloadHandler.text));

            return new ApiResponse<T>
            {
                statusCode = request.responseCode,
                data = parser?.Parse(request.downloadHandler.text) ??
                       JsonParser.Default<T>().Parse(request.downloadHandler.text)
            };
        }

        public struct ApiResponse<T>
        {
            public long statusCode;
            [CanBeNull] public T data;
        }

        private static string Trim(this string str) => str
            .Replace(" ", "")
            .Replace("\t", "")
            .Replace("\r", "")
            .Replace("\n", " ");
    }
}