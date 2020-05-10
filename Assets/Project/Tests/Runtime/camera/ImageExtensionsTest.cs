using System.Collections;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using Project.Scripts.Runtime.camera;
using Project.Tests.Runtime.utils;
using UnityEngine;
using UnityEngine.TestTools;

namespace Project.Tests.Runtime.camera
{
    public class ImageExtensionsTest
    {
        private static readonly string Dir = $"{Directory.GetCurrentDirectory()}/Temp/test";
        private static readonly string ImgPath = $"{Dir}/a.png";
        
        [Test]
        public void ImageExtensionsTestSimplePasses()
        {
            Directory.CreateDirectory(Dir);
            
            var texture2D = new Texture2D(128, 128);
            var isSuccess = texture2D.ToPng(ImgPath);
            Assert.IsTrue(isSuccess);

            var data = typeof(ImageExtensions).StaticMethod<object>("ReadPngFile", ImgPath);
            var rect = typeof(ImageExtensions).StaticMethod<object>("GetRect", data);
            Assert.AreEqual(rect.GetValue<int>("width"), 128);
            Assert.AreEqual(rect.GetValue<int>("height"), 128);

            var texture = ImgPath.ToTexture();
            Assert.IsTrue(texture.isReadable);
        }
    }
}
