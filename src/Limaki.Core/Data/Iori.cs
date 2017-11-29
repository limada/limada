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
using System.Xml;
using System.Collections.Specialized;

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

        public static Iori Clone (Iori other) {
            return new Copier<Iori> ().Copy (other, new Iori ());
        }

        public override string ToString () {
            return $"{this.ToFileName()} | {Provider}";

        }
    }

    public static class IoriExtensions {

        public static string ConnectionString (this Iori iori) {

            var provider = iori.Provider.ToLower ();
            if (provider == "mysql") {
                return $"Server = {iori.Server}; Database = {iori.Name}; Uid = {iori.User}; Pwd = {iori.Password}; charset = utf8; pooling=false;SslMode=none";
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
            iori.Path = file.DirectoryName + Path.DirectorySeparatorChar;
            iori.Name = Path.GetFileNameWithoutExtension (file.FullName);
            iori.Extension = Path.GetExtension (file.FullName).ToLower ();
        }

        public static Iori FromXmlStream (this Iori iori, string rootElement, Stream source) {

            var reader = XmlDictionaryReader.CreateTextReader (source, XmlDictionaryReaderQuotas.Max);
            var serializer = new DataContractSerializer (typeof (Iori));
            reader.ReadStartElement (rootElement);
            while (serializer.IsStartObject (reader)) {
                iori.FromIori (serializer.ReadObject (reader) as Iori);
            }
            return iori;
        }

        public static void ToXmlStream (this Iori iori, string rootElement, Stream sink) {

            var writer = XmlDictionaryWriter.CreateDictionaryWriter (XmlWriter.Create (sink, new XmlWriterSettings {
                OmitXmlDeclaration = true,
                ConformanceLevel = ConformanceLevel.Fragment,
                CloseOutput = false,
            }));
            var serializer = new DataContractSerializer (typeof (Iori));
            writer.WriteStartElement (rootElement);
            serializer.WriteObject (writer, iori);
            writer.WriteEndElement ();
            writer.Flush ();
        }


        public static Iori FromAppSettings (this Iori iori, NameValueCollection appSettings) {

            for (int i = 0; i < appSettings.Count; i++) {
                string key = appSettings.GetKey (i);
                string data = appSettings.Get (i);

                if (key == "DataBaseFileName") {
                    FromFileName (iori, data);
                }

                if (key == "DataBaseServer") {
                    iori.Server = data;
                }
                if (key == "DataBaseName") {
                    iori.Name = data;
                }
                if (key == "DataBasePath") {
                    iori.Path = data;
                }
                if (key == "DataBaseUser") {
                    iori.User = data;
                }
                if (key == "DataBasePassword") {
                    iori.Password = data;
                }
                if (key == "DataBaseProvider") {
                    iori.Provider = data;
                }

            }
            return iori;

        }

		public static Iori FromIori (this Iori iori, Iori other) {
            return new Copier<Iori> ().Copy (other, iori);
		}
    }
}