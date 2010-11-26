/*
 * Limaki 
 * Version 0.08
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


namespace Limaki.Data {
	

	/// <summary>
    /// Zusammenfassung für GatewayBase.
	/// </summary>
	public abstract class GatewayBase : IGateway {
		#region IGateway Member

        DataBaseInfo _dataBaseInfo = null;
        public DataBaseInfo DataBaseInfo {
            get { return _dataBaseInfo; }
            set { _dataBaseInfo = value; }
        }

		public abstract void Open(DataBaseInfo dataBaseInfo);

		public abstract void Close();

		public abstract bool IsOpen();

		public abstract string FileExtension {
			get;
		}

		#endregion

		
	}
}
