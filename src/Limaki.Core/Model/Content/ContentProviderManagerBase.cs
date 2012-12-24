using System;
using System.IO;
using System.Linq;
using Limaki.Common;
using Limaki.Model.Streams;

namespace Limaki.UseCases {
	public class ContentProviderManagerBase {

		public Content<Stream> Content { get;set;}
           
		public void Close() {
            if (Content != null && Content.Data !=null) {
                Content.Data.Close();
            }   
			Content = null;
        }
		
		protected virtual IContentProvider GetProvider(Uri uri) {
            var providers = Registry.Pool.TryGetCreate<ContentProviders>();
			if(uri.IsFile){
				var filename = IOUtils.UriToFileName(uri);
				var ext = Path.GetExtension(filename).Trim('.');
				return providers.Find(ext);	
			}
            return null;
        }

        protected virtual ContentInfo GetStreamTypeInfo(Content stream) {
            var providers = Registry.Pool.TryGetCreate<ContentProviders>();
            var provider = providers.Find(stream.StreamType);
            if (provider != null) {
                return provider.SupportedContents.Where(e => e.ContentType == stream.StreamType).First();
            }
            return null;
        }

        public Action<Content<Stream>> Import { get; set; }
        protected void OnImport(Content<Stream> content) {
            if (Import != null) {
                Import(content);
            }
        }
        public Func<Content<Stream>> Export { get; set; }
        protected Content<Stream> OnExport() {
            if (Export != null) {
                return Export();
            }
            return null;
        }

		public virtual bool OpenFile(Uri uri) {
            var provider = GetProvider(uri);
            bool result = false;
            if (provider != null && provider.Readable) {
                this.Close();
                try {
					this.Content = provider.Open(uri);
					if (this.Content!=null){
						OnImport(this.Content);
					}	
					Close();
	                result = true;
					
                } catch (Exception e){
					result = false;	
				}
            }
            return result;
        }

        public virtual bool SaveFile(Uri uri) {
            var provider = GetProvider(uri);
            bool result = false;
            if (provider != null && provider.Saveable) {
                try {
                    // get the content:
                    if (this.Content != null) {
                        provider.Save(this.Content, uri);
                        Close();
                        result = true;    
                    }
                } catch (Exception e) {
                    result = false;
                }
            }
            return result;
        }

	}
		
	
}
