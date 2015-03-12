using System;

namespace Limaki.Common {

    public class State : ICloneable {

        public State() {
            SetDirty = () => { this.Dirty = true; };
        }

        bool _creating = false;
        /// <summary>
        /// just created 
        /// </summary>
        public bool Creating {
            get { return _creating; }
            set {
                _creating = value;
            }
        }

        bool _clean = false;
        /// <summary>
        /// is safed in dataStore and not changed since then
        /// sets creating/dirty/hollow false if true
        /// </summary>
        public bool Clean {
            get { return _clean; }
            set {
                if (value && Dirty)
                    _dirty = false;
                if (value && Hollow)
                    _hollow = false;
                if (value && Creating)
                    _creating = false;
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
                if (value && Creating)
                    _creating = false;
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
                    if (value && Clean)
                        _clean = false;
                    _dirty = value;
                }
            }
        }

        public Action SetDirty { get; set; }
        public void Setter<T>(ref T target, T value) {
            if ((!object.Equals(value,target))) {
                target = value;
                SetDirty();
            }
        }

        #region ICloneable Member
        public void CopyTo(State target)
        {
            target.Clean = this.Clean;
            target.Creating = this.Creating;
            target.Dirty = this.Dirty;
            target.Hollow = this.Hollow;
        }

        public object Clone() {
            State result = new State();
            CopyTo(result);
            return result;
        }

        #endregion
    }
}