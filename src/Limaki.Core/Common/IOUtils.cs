
using System;
using System.IO;
using System.Linq;
using Limaki.Common;
using System.Text;

namespace Limaki
{


	public class IOUtils 
	{
		public static string FindSubDirInRootPath(string subdirToFind){
			return FindSubDirInRootPath(Directory.GetCurrentDirectory(),subdirToFind);
		}
		
		public static string FindSubDirInRootPath(string directory, string subdirToFind){
			
			var parent = Directory.GetParent(directory);
			
			
			while (parent!=null){
				
				foreach(var dir in Directory.GetDirectories(directory)){
					var subdir = dir.Split(Path.DirectorySeparatorChar).LastOrDefault();
					if (subdir == subdirToFind){
						return dir;
					}
				
				}
				directory = parent.FullName;
				parent = Directory.GetParent(directory);
				
			}
			return null;
		}
		
		public static string UriToFileName(Uri uri){
			if(uri.IsFile){
				//return uri.AbsoluteUri.Remove(0,6);
			    return Uri.UnescapeDataString(uri.AbsolutePath);
			}
			return null;
		}
		
		[TODO("handle relative filenames")]
		public static Uri UriFromFileName(string fileName){
			var uriKind = UriKind.Absolute;

			return new Uri(fileName,uriKind);
		}
        public static string NiceFileName(string fileName) {
            var b = new StringBuilder(fileName);
            foreach (var s in Path.GetInvalidFileNameChars())
                b.Replace(s, '_');
            return b.ToString();
        }
	}
}
