using System.Collections.Generic;
using System.IO;
using Project.Scripts.Runtime.utils;
using UnityEngine;

namespace Project.Scripts.Runtime.camera
{
    public static class ImageExtensions
    {
        public static Texture2D ReadPng(string path)
        {
            var readBinary = ReadPngFile(path);
            var rect = GetRect(readBinary);
            return new Texture2D(rect.width, rect.height).Apply(it => { it.LoadImage(readBinary.bytes); });
        }
        
        // TODO Texture2D -> png

        public static Rect GetRect(PngData data)
        {
            var pos = 16;
            
            var width = 0;
            for (var i = 0; i < 4; i++)
                width = width * 256 + data.bytes[pos++];
            
            var height = 0;
            for (var i = 0; i < 4; i++)
                height = height * 256 + data.bytes[pos++];
            
            return new Rect(width, height);
        }

        public static PngData ReadPngFile(string path)
        {
            var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            using (var bin = new BinaryReader(fileStream))
                return new PngData(bin.ReadBytes((int) bin.BaseStream.Length));
        }

        public readonly struct Rect
        {
            public readonly int width;
            public readonly int height;
            
            public Rect(int width, int height)
            {
                this.width = width;
                this.height = height;
            }
        }

        public readonly struct PngData
        {
            internal readonly byte[] bytes;
            
            public PngData(byte[] bytes)
            {
                this.bytes = bytes;
            }
        }
    }
}