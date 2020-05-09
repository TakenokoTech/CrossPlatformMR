using System.Collections;
using System.IO;
using NUnit.Framework;
using Project.Scripts.Runtime.camera;
using UnityEngine.TestTools;

namespace Project.Tests.Runtime.camera
{
    public class ImageExtensionsTest
    {
        private readonly string dir = Directory.GetCurrentDirectory() + $"/Temp/test";
        
        [Test]
        public void ImageExtensionsTestSimplePasses()
        {
            var bytes = ImageExtensions.ReadPngFile($"{dir}/a.png");
            var rect = ImageExtensions.GetRect(bytes);
            Assert.AreEqual(rect.width, 400);
            Assert.AreEqual(rect.height, 400);
            
            var texture = ImageExtensions.ReadPng($"{dir}/a.png");
            Assert.IsTrue(texture.isReadable);
        }
    }
}
