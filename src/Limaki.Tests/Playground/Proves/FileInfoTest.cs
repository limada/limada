using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Limaki.UnitTest;
using NUnit.Framework;
using System.Diagnostics;
using Limaki.Tests;

namespace Limaki.Playground.Proves {

    public class FileInfoTest:DomainTest {
        [Test]
        public void Test () {
            string fileName = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)+@"\testfile.info";
            System.IO.FileInfo file = new System.IO.FileInfo(fileName);

            string _path = file.DirectoryName + System.IO.Path.DirectorySeparatorChar;
            string _name = System.IO.Path.GetFileNameWithoutExtension(file.FullName);
            string _extension = System.IO.Path.GetExtension(file.FullName).ToLower();
            Trace.WriteLine(_path);
            Trace.WriteLine(_name);
            Trace.WriteLine(_extension);
        }
    }
}
