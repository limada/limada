/*
 * Limaki 
 * Version 0.071
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2008 Lytico
 *
 * http://limada.sourceforge.net
 * 
 */

using System;

namespace Limaki.Data {


    ///<summary>
    /// provides information needed to connect to a database
    /// </summary>
    ///  <stereotype>description</stereotype>
    public class DataBaseInfo {
        private string _server;
        private string _path;
        private string _name;
        private string _user;
        private string _password;
        private string _provider;
        private string _extension;

        public string Server {
            get { return _server; }
            set { _server = value; }
        }

        public string Path {
            get { return _path; }
            set { _path = value; }
        }

        public string Name {
            get { return _name; }
            set { _name = value; }
        }

        public string User {
            get { return _user; }
            set { _user = value; }
        }

        public string Password {
            get { return _password; }
            set { _password = value; }
        }

        public string Provider {
            get { return _provider; }
            set { _provider = value; }
        }

        public string Extension {
            get { return _extension; }
            set { _extension = value; }
        }

        /// <summary>
        /// extracts the information of filename and fills dataBaseInfo 
        /// with server, path, name and provider according to filename
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="dataBaseInfo"></param>
        public static DataBaseInfo FromFileName(string fileName) {
            DataBaseInfo dataBaseInfo = new DataBaseInfo ();
            System.IO.FileInfo file = new System.IO.FileInfo(fileName);
            dataBaseInfo._server = "localhost";
            dataBaseInfo._path = file.DirectoryName + System.IO.Path.DirectorySeparatorChar;
            dataBaseInfo._name = System.IO.Path.GetFileNameWithoutExtension(file.FullName);
            dataBaseInfo._extension = System.IO.Path.GetExtension(file.FullName).ToLower();
            if (dataBaseInfo._extension == ".limfb") {
                dataBaseInfo._provider = "FirebirdProvider";
                dataBaseInfo._user = "SYSDBA";
                dataBaseInfo._password = "masterkey";
            } else if (dataBaseInfo._extension == ".limo") {
                dataBaseInfo._provider = "Db4oProvider";
            } else if (dataBaseInfo._extension == ".pib") {
                dataBaseInfo._provider = "PartsProvider";
                dataBaseInfo._user = "SYSDBA";
                dataBaseInfo._password = "masterkey";
            }
            return dataBaseInfo;
        }
    }
}
