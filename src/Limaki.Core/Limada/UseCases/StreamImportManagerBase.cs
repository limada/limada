using System;
using System.IO;
using Limada.Data;
using Limada.Model;
using Limaki.Common;
using Limaki.Data;
using Limaki.Model.Streams;
using System.Linq;

namespace Limaki.UseCases {
	public class StreamImportManagerBase {

		public StreamInfo<Stream> Content { get;set;}
           
		public void Close() {
            if (Content != null && Content.Data !=null) {
                Content.Data.Close();
            }   
			Content = null;
        }
		
		protected virtual IStreamProvider GetProvider(Uri uri) {
            var providers = Registry.Pool.TryGetCreate<StreamProviders>();
			if(uri.IsFile){
				var filename = IOUtils.UriToFileName(uri);
				var ext = Path.GetExtension(filename).Trim('.');
				return providers.Find(ext);	
			}
            return null;
        }
        protected virtual StreamTypeInfo GetStreamTypeInfo(StreamInfo stream) {
            var providers = Registry.Pool.TryGetCreate<StreamProviders>();
            var provider = providers.Find(stream.StreamType);
            if (provider != null) {
                return provider.SupportedStreamTypes.Where(e => e.StreamType == stream.StreamType).First();
            }
            return null;
        }

        public Action<StreamInfo<Stream>> Import { get; set; }
        protected void OnImport(StreamInfo<Stream> content) {
            if (Import != null) {
                Import(content);
            }
        }
        public Func<StreamInfo<Stream>> Export { get; set; }
        protected StreamInfo<Stream> OnExport() {
            if (Export != null) {
                return Export();
            }
            return null;
        }

		public virtual bool OpenFile(Uri uri) {
            var provider = GetProvider(uri);
            bool result = false;
            if (provider != null) {
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
            if (provider != null) {
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
