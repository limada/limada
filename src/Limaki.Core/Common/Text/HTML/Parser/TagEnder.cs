using System.Collections.Generic;

namespace Limaki.Common.Text.HTML.Parser{
	
	public class TagEnder{
		private Stack<string> _stack;
		public TagEnder(){
			_stack = new Stack<string>();
		}
		public void Set(string name, Status status){
			name = Name(name);
			if(status == Status.Endtag){
				Remove(name);
			}
			else if(status == Status.Name){
				Add(name);
			}
			else if(status == Status.Attribute){
				Add(name);
			}
			else if(status == Status.Value){
				Add(name);
			}
		}
		private string Name(string name){
			if(name.StartsWith("<")){
			    var pos = name.IndexOf(" ");
                if (pos != -1){
					return name.Substring(1, pos - 1);
				}else{
					return name.Substring(1, name.Length - 2);
				}
			}
			return name;
		}
		public void Add(string tag){
			_stack.Push(tag);
		}
		public void Remove(string tag){
			if (tag.Equals(_stack.Peek().ToString())){
				_stack.Pop();
			}
		}
    
		public string Get(){
			string result = "";
			while(_stack.Count > 0){
				result = result + "</" + _stack.Pop() + ">";
			}
			return result;
		}
	}
}