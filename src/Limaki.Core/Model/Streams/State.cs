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

using System;

namespace Limaki.Model.Streams {
    public class State : ICloneable {

        bool _creating = true;
        /// <summary>
        /// just created 
        /// </summary>
        public bool Creating {
            get { return _creating; }
            set { _creating = value; }
        }

        bool _clean = false;
        /// <summary>
        /// is safed in dataStore and not changed since then
        /// sets creating/dirty/hollow false if true
        /// </summary>
        public bool Clean {
            get { return _clean; }
            set {
                if (value && Dirty) _dirty = false;
                if (value && Hollow) _hollow = false;
                if (value && Creating) _creating = false;
                _clean = value;
            }
        }

        bool _hollow = false;
        /// <summary>
        /// never saved in a dataStore until now
        /// </summary>
        public bool Hollow {
            get { return _hollow; }
            set {
                if (!value && _hollow) {
                    _dirty = false;
                    _clean = false;
                }
                if (value && Creating) _creating = false;
                _hollow = value;
            }
        }

        bool _dirty = false;
        /// <summary>
        /// changed; sets clean = false
        /// hollow remains unchanged!
        /// </summary>
        public bool Dirty {
            get { return _dirty; }
            set {
                if (!Creating) {
                    if (value && Clean) _clean = false;
                    _dirty = value;
                }
            }
        }

        #region ICloneable Member

        public object Clone() {
            State result = new State();
            result.Clean = this.Clean;
            result.Creating = this.Creating;
            result.Dirty = this.Dirty;
            result.Hollow = this.Hollow;
            return result;
        }

        #endregion
    }
}