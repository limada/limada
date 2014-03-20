using System.IO;
using Limada.Schemata;
using Limaki.Contents;
using Limaki.Common;

namespace Limada.Model {

    public class ThingContentFacade {
        public ThingContentFacade (IThingFactory factory) {
            this._factory = factory;
        }

        public ThingContentFacade() {}

        private IThingFactory _factory = null;
        public IThingFactory Factory {
            get {
                if (_factory == null) {
                    _factory = Registry.Factory.Create<IThingFactory>();
                }
                return _factory;
            }
            set { _factory = value; }
        }

        /// <summary>
        /// creates a StreamThing with content assigned
        /// and adds it to the graph
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public IStreamThing CreateAndAdd(IThingGraph graph, Content<Stream> content) {

            var thing = Factory.CreateItem(graph, content.Data) as IStreamThing;
            thing.Compression = content.Compression;
            thing.StreamType = content.ContentType;

            thing.Compress ();
            graph.Add(thing);

            thing.Flush();
            thing.ClearRealSubject();
            return thing;
        }

        /// <summary>
        /// assigns the the content to the thing
        /// </summary>
        /// <param name="thing"></param>
        /// <param name="content"></param>
        public void AssignContent(IStreamThing thing, Content<Stream> content) {
            if (thing != null && thing.ContentContainer != null) {
                thing.Data = content.Data;
                thing.Compression = content.Compression;
                thing.StreamType = content.ContentType;
                thing.Compress();
                thing.Flush ();
                thing.ClearRealSubject ();
            }
        }

        /// <summary>
        /// assigns the the content to the thing
        /// if thing is null, a new StreamThing is created
        /// </summary>
        /// <param name="thing"></param>
        /// <param name="content"></param>
        public IStreamThing AssignContent(IThingGraph graph, IStreamThing thing, Content<Stream> content) {
            if (thing != null) {
                thing.ContentContainer = graph.ContentContainer;
                AssignContent (thing, content);
                AssignContentDescription(thing, content, graph);
            } else {
                thing = CreateAndAdd(graph, content);
                AssignContentDescription(thing, content, graph);            
            }

            return thing;
        }

        /// <summary>
        /// assigns the content's descriptions to the thing
        /// assign = set content.Description to the leaf with CommonSchema.DescriptionMarker
        /// set content.Source to the leaf with CommonSchema.SourceMarker
        /// if the leaf doesn't exist, it will be created
        /// </summary>
        /// <param name="thing"></param>
        /// <param name="content"></param>
        /// <param name="thingGraph"></param>
        public virtual void AssignContentDescription(IThing thing, Content<Stream> content, IThingGraph thingGraph) {
            var streamThing = thing as IStreamThing;
            if (streamThing == null) return;
            var schema = new CommonSchema(thingGraph, streamThing);
            if (content.Description != null) {
                if (schema.Description != null) {
                    schema.Description.Data = content.Description;
                } else {
                    schema.Description = Factory.CreateItem(content.Description); ;
                }
            }
            if (content.Source != null) {
                var sourceThing = schema.GetTheLeaf(CommonSchema.SourceMarker);
                if (sourceThing == null) {
                    sourceThing = Factory.CreateItem (content.Source);
                    schema.SetTheLeaf (CommonSchema.SourceMarker, sourceThing);
                } else {
                    sourceThing.Data = content.Source;
                }
            }
        }

        /// <summary>
        /// gets a content of a thing
        /// and content.Description and content.Source
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="thing"></param>
        /// <returns></returns>
        public static Content<Stream> ContentOf(IThingGraph graph, IThing thing) {
            var result = ContentOf (thing);
            if (result !=null && graph is SchemaThingGraph) {
                result.Description = graph.Description(thing);
                result.Source = graph.Source(thing);
            }
            return result;
        }

        /// <summary>
        /// gets a content of a thing
        /// </summary>
        /// <param name="thing"></param>
        /// <returns></returns>
        public static Content<Stream> ContentOf(IThing thing) {
            var result = default( Content<Stream> );
            var streamThing = thing as IStreamThing;
            if (streamThing!=null) {
                result = new Content<Stream> ();
                
                streamThing.DeCompress ();
                result.Data = streamThing.Data as Stream;
                result.Compression = streamThing.Compression;
                result.ContentType = streamThing.StreamType;
                streamThing.ClearRealSubject (false);
            }
            return result;
        }
    }
}