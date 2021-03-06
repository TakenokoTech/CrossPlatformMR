﻿using System;
using System.Linq;
using System.Reflection;

namespace Project.Scripts.Runtime.utils
{
    public static class ObjectExtensions
    {
        public static string Dump<T>(this T obj)
        {
            const string separator = ",";
            const string format = "{0}={1}";
             
            var fields = string.Join(separator, obj
                .GetType()
                .GetFields(BindingFlags.Instance | BindingFlags.Public)
                .Select(c => string.Format(format, c.Name, c.GetValue(obj))));
             
            var properties = string.Join(separator, obj
                .GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(c => c.CanRead)
                .Select(c => string.Format(format, c.Name, c.GetValue(obj, null))));
             
            if ("".Equals(properties) && "".Equals(fields)) return "{}";
            if ("".Equals(fields)) return "{" + properties + "}";
            if ("".Equals(properties)) return "{" + fields + "}";
             
            return "{" + string.Join(separator, fields, properties) + "}";
        }
 
        public static T Apply<T>(this T obj, Action<T> block)
        {
            block(obj);
            return obj;
        }
    }
}