using System.IO;
using Limada.IO;
using Limaki;
using Limaki.Common.IOC;
using Limaki.Contents;
using Limaki.Contents.IO;
using Limaki.Data;
using Limada.UseCases.Cms;
using Limaki.View;
using Limaki.View.Html5;
using Limaki.ImageLibs;

namespace Limada.Usecases.Cms {

    public class WebCmsContextResourceLoader : ContextResourceLoader, IBackendContextResourceLoader {

		protected static bool Applied { get; set; } 

        public override void ApplyResources (IApplicationContext context) {

			if (Applied)
				return;

            new LimakiCoreContextResourceLoader ().ApplyResources (context);
            new Html5ContextResourceLoader().ApplyHtml5Resources (context);
            new ViewContextResourceLoader ().ApplyResources (context);

            var thingGraphContentPool = context.Pooled<ThingGraphIoPool>();
            thingGraphContentPool.Add(new Db4oThingGraphIo());
            thingGraphContentPool.Add(new XmlThingGraphIo());

            context.Pooled<AppController>()
                .AppSettingsGetter = () => System.Web.Configuration.WebConfigurationManager.AppSettings;

            var converterPool = context.Pooled<ConverterPool<Stream>>();
            converterPool.Add(new HtmlRtfConverter());
            converterPool.Add (new ImageConverter ());

			Applied = true;
        }
    }
}