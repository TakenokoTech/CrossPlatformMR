using System;
using System.Runtime.CompilerServices;
using Project.Scripts.Runtime.utils;
using UnityEngine.Networking;

namespace Project.Scripts.Runtime.api
{
    // ReSharper disable once MemberCanBeMadeStatic.Global
    public class UnityWebRequestAwaiter : INotifyCompletion
    {
        private const string Tag = "UnityWebRequestAwaiter";
        private readonly UnityWebRequestAsyncOperation asyncOp;
        private Action continuation;

        public UnityWebRequestAwaiter(UnityWebRequestAsyncOperation asyncOp)
        {
            this.asyncOp = asyncOp;
            asyncOp.completed += obj => continuation();
        }

        public void OnCompleted(Action action)
        {
            this.continuation = action;
        }

        public bool IsCompleted => asyncOp.isDone;

        public void GetResult() => Log.D(Tag, "GetResult");
    }

    public static class ExtensionMethods
    {
        public static UnityWebRequestAwaiter GetAwaiter(this UnityWebRequestAsyncOperation asyncOp) =>
            new UnityWebRequestAwaiter(asyncOp);
    }
}