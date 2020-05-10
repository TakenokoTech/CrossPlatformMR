using System;
using System.Reflection;
using Project.Scripts.Runtime.camera;

namespace Project.Tests.Runtime.utils
{
    public static class TestUtil
    {
        public static T StaticMethod<T>(this Type clazz, string methodName, params object[] args)
        {
            var methodInfo = clazz.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Static);
            var data = methodInfo?.Invoke(null, args);
            return data is T d ? d : default;
        }
        
        public static T GetValue<T>(this object obj, string fieldName)
        {
            return (T) obj.GetType().GetField(fieldName).GetValue(obj);
        }
    }
}