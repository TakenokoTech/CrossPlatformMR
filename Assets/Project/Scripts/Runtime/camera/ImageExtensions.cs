using System;
using System.Collections.Generic;
using System.IO;
using Project.Scripts.Runtime.utils;
using UnityEngine;

namespace Project.Scripts.Runtime.camera
{
    public static class ImageExtensions
    {
        private const string Tag = "ImageExtensions";

        public static Texture2D ToTexture(this string path)
        {
            var readBinary = ReadPngFile(path);
            var rect = GetRect(readBinary);
            return new Texture2D(rect.width, rect.height).Apply(it => { it.LoadImage(readBinary.bytes); });
        }

        public static bool ToPng(this Texture2D texture2D, string path)
        {
            try
            {
                var png = texture2D.EncodeToPNG();
                File.WriteAllBytes(path, png);
                return true;
            }
            catch (Exception e)
            {
                Log.D(Tag, e.ToString());
                return false;
            }
        }

        private static Rect GetRect(PngData data)
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

        private static PngData ReadPngFile(string path)
        {
            var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            using (var bin = new BinaryReader(fileStream))
                return new PngData(bin.ReadBytes((int) bin.BaseStream.Length));
        }

        private readonly struct Rect
        {
            public readonly int width;
            public readonly int height;

            public Rect(int width, int height)
            {
                this.width = width;
                this.height = height;
            }
        }

        private readonly struct PngData
        {
            internal readonly byte[] bytes;

            public PngData(byte[] bytes)
            {
                this.bytes = bytes;
            }
        }
    }
}