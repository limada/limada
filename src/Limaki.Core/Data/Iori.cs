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

using Limaki.Contents.IO;
using System.IO;
using System.Runtime.Serialization;

namespace Limaki.Data {

    ///<summary>
    /// Input Output Resource Identifier
    /// provides information needed to connect to a database
    /// </summary>
    ///  <stereotype>description</stereotype>
    [DataContract]
    public class Iori {

        [DataMember]
        public string Path { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Extension { get; set; }

        [DataMember]
        public string Server { get; set; }
        [DataMember]
        public int Port { get; set; }
        
        [DataMember]
        public string User { get; set; }
        [DataMember]
        public string Password { get; set; }
        
        [DataMember]
        public string Provider { get; set; }
        
        [DataMember]
        public IoMode AccessMode { get; set; }

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
            var file = new System.IO.FileInfo (fileName);
            iori.Server = "localhost";
            iori.Path = file.DirectoryName + System.IO.Path.DirectorySeparatorChar;
            iori.Name = System.IO.Path.GetFileNameWithoutExtension (file.FullName);
            iori.Extension = System.IO.Path.GetExtension (file.FullName).ToLower ();
            if (iori.Extension == ".limfb") {
                iori.Provider = "Firebird";
                iori.User = "SYSDBA";
                iori.Password = "masterkey";
            } else if (iori.Extension == ".limo") {
                iori.Provider = "Db4o";
            } else if (iori.Extension == ".pib") {
                iori.Provider = "Firebird";
                iori.User = "SYSDBA";
                iori.Password = "masterkey";
            } 
        }

        public static string ToFileName (Iori iori) {
            var extension = iori.Extension;
            if (extension == null)
                extension = "";
            if (!extension.StartsWith("."))
                extension = "." + extension;
            var sep = System.IO.Path.DirectorySeparatorChar.ToString();
            var path = iori.Path;
            if (!(path.EndsWith (sep) || string.IsNullOrEmpty (path)))
                path = path + sep;
            return path + iori.Name + extension;
        }

        public override string ToString() {
            return ToFileName (this);

        }
    }
}
