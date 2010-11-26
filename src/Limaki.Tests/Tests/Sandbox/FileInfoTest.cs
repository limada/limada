using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Limaki.UnitTest;
using NUnit.Framework;

namespace Limaki.Tests.Sandbox {
    public class FileInfoTest:DomainTest {
        [Test]
        public void Test () {
            string fileName = @"e:\test\testfile.info";
            System.IO.FileInfo file = new System.IO.FileInfo(fileName);

            string _path = file.DirectoryName + System.IO.Path.DirectorySeparatorChar;
            string _name = System.IO.Path.GetFileNameWithoutExtension(file.FullName);
            string _extension = System.IO.Path.GetExtension(file.FullName).ToLower();
            System.Console.Out.WriteLine(_path);
            System.Console.Out.WriteLine(_name);
            System.Console.Out.WriteLine(_extension);
        }
    }
}
