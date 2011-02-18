using System;

namespace Limaki.Common.Text.HTML.Parser{
    /// <summary>
    /// Extracting Styles out of a HTML
    ///	 *	Name		Starts with a letter or '-' or '_' followed by a letter
	///	 *				ends with ' '
	///	 *	Attributes	are between '{' and '}'
	///	 *				if the attribut is missing, the stylename is obsolet
	///	 *	Attribut	starts after '{' or ';'
    ///	 *	Value		starts after ':' and ends with ';'
	///	 *	No Attribut or Value
    ///	 *				comments, starting with  '/*' and ending with '*/'or ';'
    ///	 *				all cr lf, blanks (except in comments or with "") und tabs
    /// </summary>
	public class StyleParser : ParserBase{
		
		private Boolean _newStyle = false;
		private Boolean _inAttr = false;

		public StyleParser(string content) : base(content){
		}

		public StyleParser(Stuff stuff) : base(stuff){
		}

		
		public void Parse(){
		
			if (_stuff != null){
				Go();
			}
		}


		
		private void Go(){
			_stuff.Status = Status.None;
			_stuff.ActAt++;
			_stuff.StartTag = _stuff.ActAt;
			while (_stuff.ActAt < _stuff.Text.Length){
				if (_stop){
					_stuff.ActAt--;
					_stuff.ActAt--;
					_stuff.StartAt = _stuff.ActAt;
					OnElement();
                    State(Status.Text);
                    Stop();
					break;
				}
				_actual = _stuff.Text[_stuff.ActAt]; 
				Watch();
				_stuff.ActAt++; 
			}
		}

		private void Watch(){
            if(_actual.Equals('<') ||
                _actual.Equals('>')) {
                //Style ends - Attention:  a Style in a commenttag differs!!!
                _newStyle = false;
                State(Status.None);
                OnElement();
                base.Stop();
            }
            else if(_stuff.Status.Equals(Status.None)) {
                //start of tagname
                if (Letter(_actual) ||
                    (_actual.Equals('-')) ||
                    (_actual.Equals('#')) ||
                    (_actual.Equals('.')) ||
                    (_actual.Equals('_'))){
                    State(Status.Prename);
                    OnElement();
                    _stuff.StartAt = _stuff.ActAt;
                }
            }
            else if(_stuff.Status.Equals(Status.Prename)) {
                //end of tagname
                if(_actual.Equals('{') ||
                    _actual.Equals(' ') ||
                    _actual.Equals('\r') ||
                    _actual.Equals('\n') ||
                    _actual.Equals('\t')) {
                    State(Status.Name);
                    OnElement();
                    _stuff.StartAt = _stuff.ActAt;
                    if (_actual.Equals('{')){
                        State(Status.Attribute);
                    }
                }
            }
            else if(_stuff.Status.Equals(Status.Solotag)) {
                //start of next tagname
                if (Letter(_actual) ||
                    (_actual.Equals('-')) ||
                    (_actual.Equals('#')) ||
                    (_actual.Equals('.')) ||
                    (_actual.Equals('_'))){
                    State(Status.Name);
                    _stuff.StartAt = _stuff.ActAt;
                }
            }
            else if(_stuff.Status.Equals(Status.Name)) {
                //end of tagname; its already registered
                if (_actual.Equals('{')){
                    State(Status.Attribute);
                    _stuff.StartAt = _stuff.ActAt + 1;
                }
            }
            else if(_stuff.Status.Equals(Status.Attribute)) {
                //start of attribute-name
                if(Letter(_actual)) {
                    if(_inAttr == false) {
                        _stuff.StartTag = _stuff.ActAt;
                        //go on here !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    }
                }
                if(_actual.Equals(':')) {
                    OnElement();
                    State(Status.Value);
                    _stuff.StartAt = Stuff().ActAt + 1;
                }
                else if(_actual.Equals('}')) {
                    // something is wrong, the value of the attibute is missing
                    State(Status.Prename);
                }
            }
            else if(_stuff.Status.Equals(Status.Value)) {
                //end of a value
                if(_actual.Equals(';')) {
                    OnElement();
                    State(Status.Attribute);
                    _stuff.StartAt = _stuff.ActAt + 1;
                }
                else if(_actual.Equals('}')) {
                    //end of a tag
                    OnElement();
                    State(Status.Solotag);
                }
            }



//			if ((_actual.Equals('<')) || (_actual.Equals('>'))){
//				if((_stuff.Text.Length > _stuff.ActAt + 1) && _stuff.Text[_stuff.ActAt+1].Equals('!')){
					//the styles are in a Commenttag
//				}else{
					//end of stylse
//					_newStyle = false;
//					State(Status.None);
//					OnElement();
//					base.Stop();
//				}
//			}
//			else{
//				if(_stuff.Status == Status.None){
					//maybe a stylename begins
//					if(Letter(_actual) || 
//						(_actual.Equals('-')) ||
//						(_actual.Equals('#')) ||
//						(_actual.Equals('.')) || 
//						(_actual.Equals('_'))){
//						State(Status.Prename);
//					}
//				}
//				else if ((_stuff.Status == Status.Prename) || 
//							(_stuff.Status == Status.Solotag)){
//					if (Letter(_actual) ||
//							_actual.Equals('.') ||
//                            _actual.Equals('#')){
						//Stylename starts definitiv
//						if(_newStyle == false){
//							State(Status.Prename);
//							OnElement();
//						}		
//						_newStyle = true;
//						State(Status.Name);
//						_stuff.StartAt = _stuff.ActAt - 1;
//					}
//					else if (_actual.Equals('{')){
//                      // entrance in the zone of attributes and values
//						_stuff.StartAt = _stuff.ActAt;
//						_inAttr = true;
//						State(Status.Attribute);
//					}
//							}
//				else if(_stuff.Status == Status.Name){
					
//					if(Letter(_actual)==false && 
//								_actual.Equals('-')==false &&
//								_actual.Equals('_') == false &&
//								_actual.Equals('.') == false
//								){
//						OnElement(); 
//						State(Status.Solotag);
//					}
//				}
//				else if(_stuff.Status == Status.Attribute){
//					
//                    if((_actual.Equals('\r')) ||
//                        (_actual.Equals('\n')) ||
//                        (_actual.Equals('\t')) ||
//                        (_actual.Equals(' '))){
				        
//                        _stuff.StartAt = _stuff.ActAt;
//				    }
//					if(_actual.Equals(':')){
//						OnElement();
//						State(Status.Value);
//						_stuff.StartAt = _stuff.ActAt + 1;
//					}
//				}
//				else if(_stuff.Status.Equals(Status.Value)){
//					if (_actual.Equals(';')){
//						OnElement();
//						State(Status.Attribute);
//						_stuff.StartAt = _stuff.ActAt + 1;
//					}
//					else if (_actual.Equals('}')){
//						OnElement();//Die Attribut/Wert-Sequenz wird abgeschickt
//						State(Status.None);
//						_stuff.StartAt = _stuff.ActAt + 1;
//					}
//				}
//			}
		}

		
		public Action<Stuff> Element; //Ein Tag wird ausgeworfen

		private void OnElement(){
			if (Element != null){
				Element(_stuff);
			}
		}
	}
}