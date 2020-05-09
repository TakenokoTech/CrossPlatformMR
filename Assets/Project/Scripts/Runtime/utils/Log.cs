using System.Threading;
using UnityEngine;

namespace Project.Scripts.Runtime.utils
{
    public static class Log
    {
        public static void D(string tag, string msg)
        {
            Debug.LogFormat("({0})[{1}] {2}", tag, Thread.CurrentThread.ManagedThreadId, msg);
        }
    }
}