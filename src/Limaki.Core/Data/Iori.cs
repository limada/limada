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
using Limaki.Common;

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
        public static Iori FromFileName (string fileName) {
            var iori = new Iori ();
            IoriExtensions.FromFileName (iori, fileName);
            return iori;
        }

        public static Iori FromIori (Iori other) {
            return new Copier<Iori> ().Copy (other, new Iori ());
        }

        public override string ToString () {
            return this.ToFileName ();

        }
    }

    public static class IoriExtensions {

        public static string ConnectionString (this Iori iori) {

            var provider = iori.Provider.ToLower ();
            if (provider == "mysql") {
                return $"Server = {iori.Server}; Database = {iori.Name}; Uid = {iori.User}; Pwd = {iori.Password}; charset = utf8; pooling=false;";
            } else if (provider == "firebird") {
                return $"User={iori.User};Password={iori.Password};Database={iori.ToFileName ()};DataSource={iori.Server};";
            } else if (provider == "sqlite") {
                return $"Data Source = {iori.ToFileName ()}; Version = 3;";
            } else if (provider.StartsWith ("postgres")) {
                return $"User ID={iori.User};Password={iori.Password};Database={iori.ToFileName ()};Host={iori.Server};";
            } else if (provider.StartsWith ("sqlserver")) {
                return $"User Id={iori.User};Password={iori.Password};Initial Catalog={iori.Name};Data Source={iori.Server};";
            }
            return null;

        }

        public static string ToFileName (this Iori iori) {
            var extension = iori.Extension ?? "";
            if (!extension.StartsWith (".") && !string.IsNullOrEmpty (extension))
                extension = "." + extension;
            var sep = Path.DirectorySeparatorChar.ToString ();
            var path = iori.Path ?? "";
            if (!(path.EndsWith (sep) || string.IsNullOrEmpty (path)))
                path = path + sep;
            return path + iori.Name + extension;
        }

        public static void FromFileName (this Iori iori, string fileName) {
            var file = new System.IO.FileInfo (fileName);
            iori.Server = "localhost";
            iori.Path = file.DirectoryName + System.IO.Path.DirectorySeparatorChar;
            iori.Name = System.IO.Path.GetFileNameWithoutExtension (file.FullName);
            iori.Extension = System.IO.Path.GetExtension (file.FullName).ToLower ();
        }
    }
}