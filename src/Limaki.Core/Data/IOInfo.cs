/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2011 Lytico
 *
 * http://www.limada.org
 * 
 */

namespace Limaki.Data {
    ///<summary>
    /// provides information needed to connect to a database
    /// </summary>
    ///  <stereotype>description</stereotype>
    public class IoInfo {
        public string Server { get; set; }

        public string Path { get; set; }

        public string Name { get; set; }

        public string User { get; set; }

        public string Password { get; set; }

        public string Provider { get; set; }

        public string Extension { get; set; }

        public int Port { get; set; }

        /// <summary>
        /// extracts the information of filename and fills dataBaseInfo 
        /// with server, path, name and provider according to filename
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="dataBaseInfo"></param>
        public static IoInfo FromFileName(string fileName) {
            var dataBaseInfo = new IoInfo ();
            FromFileName (dataBaseInfo,fileName);
            return dataBaseInfo;
        }

        public static void FromFileName (IoInfo ioInfo, string fileName) {
            System.IO.FileInfo file = new System.IO.FileInfo (fileName);
            ioInfo.Server = "localhost";
            ioInfo.Path = file.DirectoryName + System.IO.Path.DirectorySeparatorChar;
            ioInfo.Name = System.IO.Path.GetFileNameWithoutExtension (file.FullName);
            ioInfo.Extension = System.IO.Path.GetExtension (file.FullName).ToLower ();
            if (ioInfo.Extension == ".limfb") {
                ioInfo.Provider = "FirebirdProvider";
                ioInfo.User = "SYSDBA";
                ioInfo.Password = "masterkey";
            } else if (ioInfo.Extension == ".limo") {
                ioInfo.Provider = "Db4oProvider";
            } else if (ioInfo.Extension == ".pib") {
                ioInfo.Provider = "PartsProvider";
                ioInfo.User = "SYSDBA";
                ioInfo.Password = "masterkey";
            }
        }

        public static string ToFileName(IoInfo ioInfo) {
            return ioInfo.Path+ioInfo.Name+ioInfo.Extension;
        }
        public override string ToString() {
            return (this.Path ?? "") + (this.Name ?? "") + (this.Extension ?? "");
           
        }
    }
}
