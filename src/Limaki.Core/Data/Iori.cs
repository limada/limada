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
    /// Input Output Resource Identifier
    /// provides information needed to connect to a database
    /// </summary>
    ///  <stereotype>description</stereotype>
    public class Iori {
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
        public static Iori FromFileName(string fileName) {
            var dataBaseInfo = new Iori ();
            FromFileName (dataBaseInfo,fileName);
            return dataBaseInfo;
        }

        public static void FromFileName (Iori iori, string fileName) {
            System.IO.FileInfo file = new System.IO.FileInfo (fileName);
            iori.Server = "localhost";
            iori.Path = file.DirectoryName + System.IO.Path.DirectorySeparatorChar;
            iori.Name = System.IO.Path.GetFileNameWithoutExtension (file.FullName);
            iori.Extension = System.IO.Path.GetExtension (file.FullName).ToLower ();
            if (iori.Extension == ".limfb") {
                iori.Provider = "FirebirdProvider";
                iori.User = "SYSDBA";
                iori.Password = "masterkey";
            } else if (iori.Extension == ".limo") {
                iori.Provider = "Db4oProvider";
            } else if (iori.Extension == ".pib") {
                iori.Provider = "PartsProvider";
                iori.User = "SYSDBA";
                iori.Password = "masterkey";
            }
        }

        public static string ToFileName(Iori iori) {
            return iori.Path+iori.Name+iori.Extension;
        }
        public override string ToString() {
            return (this.Path ?? "") + (this.Name ?? "") + (this.Extension ?? "");
           
        }
    }
}
