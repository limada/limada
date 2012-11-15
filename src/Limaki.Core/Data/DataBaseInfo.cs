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
    public class DataBaseInfo {
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
        public static DataBaseInfo FromFileName(string fileName) {
            var dataBaseInfo = new DataBaseInfo ();
            FromFileName (dataBaseInfo,fileName);
            return dataBaseInfo;
        }

        public static void FromFileName (DataBaseInfo dataBaseInfo, string fileName) {
            System.IO.FileInfo file = new System.IO.FileInfo (fileName);
            dataBaseInfo.Server = "localhost";
            dataBaseInfo.Path = file.DirectoryName + System.IO.Path.DirectorySeparatorChar;
            dataBaseInfo.Name = System.IO.Path.GetFileNameWithoutExtension (file.FullName);
            dataBaseInfo.Extension = System.IO.Path.GetExtension (file.FullName).ToLower ();
            if (dataBaseInfo.Extension == ".limfb") {
                dataBaseInfo.Provider = "FirebirdProvider";
                dataBaseInfo.User = "SYSDBA";
                dataBaseInfo.Password = "masterkey";
            } else if (dataBaseInfo.Extension == ".limo") {
                dataBaseInfo.Provider = "Db4oProvider";
            } else if (dataBaseInfo.Extension == ".pib") {
                dataBaseInfo.Provider = "PartsProvider";
                dataBaseInfo.User = "SYSDBA";
                dataBaseInfo.Password = "masterkey";
            }
        }

        public static string ToFileName(DataBaseInfo dataBaseInfo) {
            return dataBaseInfo.Path+dataBaseInfo.Name+dataBaseInfo.Extension;
        }
        public override string ToString() {
            return (this.Path ?? "") + (this.Name ?? "") + (this.Extension ?? "");
           
        }
    }
}
