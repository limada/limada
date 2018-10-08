using System.IO;
using System.Text;
using Limada.IO;
using Limaki;
using Limaki.Common.IOC;
using Limaki.Contents;
using Limaki.Contents.IO;
using Limaki.Data;
using Limada.Usecases.Cms;
using Limaki.View;
using Limaki.View.Html5;
using Limaki.ImageLibs;
using Microsoft.Extensions.Configuration;

namespace Limada.Usecases.Cms {

    public class WebCmsContextResourceLoader : ContextResourceLoader, IBackendContextResourceLoader {

		protected static bool Applied { get; set; } 

        public override void ApplyResources (IApplicationContext context) {

			if (Applied)
				return;

	        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
	        
            new LimakiCoreContextResourceLoader ().ApplyResources (context);
            new Html5ContextResourceLoader().ApplyHtml5Resources (context);
            new ViewContextResourceLoader ().ApplyResources (context);

            var thingGraphContentPool = context.Pooled<ThingGraphIoPool>();
            thingGraphContentPool.Add(new Db4oThingGraphIo());
            thingGraphContentPool.Add(new XmlThingGraphIo());


			Applied = true;
        }
    }
}