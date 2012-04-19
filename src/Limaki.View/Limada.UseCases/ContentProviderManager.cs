using System;
using System.IO;
using Limaki;
using Limaki.Common;
using Limaki.Drawing;
using Limada.View;
using Limada.VisualThings;
using Limaki.Model.Streams;
using Limaki.View.Visuals.UI;
using Limaki.UseCases;
using Limaki.Viewers;
using Limaki.Visuals;

namespace Limada.UseCases {
    public class ContentProviderManager : ContentProviderManagerBase {
        public string DefaultExtension = null;
        string _streamProviderDialogFilter = null;
        public string StreamProviderDialogFilter {
            get {
                if (_streamProviderDialogFilter == null) {
                    _streamProviderDialogFilter = string.Empty;
                    var providers = Registry.Pool.TryGetCreate<ContentProviders>();
                    string defaultFilter = null;
                    foreach (var provider in providers) {
                        foreach (var info in provider.SupportedStreamTypes) {
                            string filter = info.Description + "|*" + info.Extension + "|";
                            if (info.Extension == DefaultExtension)
                                defaultFilter = filter;
                            else
                                _streamProviderDialogFilter += filter;

                        }
                    }
                    if (defaultFilter != null) {
                        _streamProviderDialogFilter = defaultFilter + _streamProviderDialogFilter;
                    }
                }
                return _streamProviderDialogFilter;
            }
        }

        public FileDialogMemento OpenFileDialog { get; set; }
        public FileDialogMemento SaveFileDialog { get; set; }

        public Func<string, string, MessageBoxButtons, DialogResult> MessageBoxShow { get; set; }
        public Func<FileDialogMemento, bool, DialogResult> FileDialogShow { get; set; }

        public void OpenFile() {
            DefaultDialogValues(OpenFileDialog);
            if (FileDialogShow(OpenFileDialog, true) == DialogResult.OK) {
                this.OpenFile(IOUtils.UriFromFileName(OpenFileDialog.FileName));
            }
        }
        public void SaveFile() {
            try {
                DefaultDialogValues(SaveFileDialog);
                this.Content = OnExport();
                if (this.Content != null) {
                    var info = GetStreamTypeInfo(this.Content);
                    if (info != null) {
                        SaveFileDialog.DefaultExt = info.Extension;
                        SaveFileDialog.FileName = this.Content.Description.ToString();
                        SaveFileDialog.Filter = info.Description + "|*" + info.Extension + "|" + "All Files|*.*";
                        if (FileDialogShow(SaveFileDialog, true) == DialogResult.OK) {
                            this.SaveFile(IOUtils.UriFromFileName(SaveFileDialog.FileName));
                        }
                    }
                }
            } catch (Exception ex) {
                Registry.Pool.TryGetCreate<IExceptionHandler>().Catch(ex, MessageType.OK);
            }
        }

        public void DefaultDialogValues(FileDialogMemento dialog) {
            dialog.Filter = this.StreamProviderDialogFilter + "All Files|*.*";
            dialog.DefaultExt = "";
            dialog.AddExtension = true;
            dialog.CheckFileExists = false;
            dialog.CheckPathExists = true;
            dialog.AutoUpgradeEnabled = true;
            dialog.SupportMultiDottedExtensions = true;
            // Important! under windows this adds the fileextension!
            dialog.ValidateNames = true;

        }

        public void ImportContent(Content<Stream> content, IGraphScene<IVisual, IVisualEdge> scene, IGraphSceneLayout<IVisual, IVisualEdge> layout) {
            var graph = scene.Graph;
            var thing = new VisualThingStreamHelper().CreateFromStream(graph, content);
            if (scene.Focused != null) {
                SceneExtensions.PlaceVisual(scene, scene.Focused, thing, layout);
            } else {
                SceneExtensions.AddItem(scene, thing, layout, scene.NoHit);
            }
        }

        public Content<Stream> ExtractContent(IGraphScene<IVisual, IVisualEdge> scene) {
            var graph = scene.Graph;
            if (graph!=null && scene.Focused != null) {
                return new VisualThingStreamHelper ().GetStream (graph,scene.Focused);
            }
            return null;
        }

        public void ExportContents(IGraphScene<IVisual, IVisualEdge> scene) {
            
        }
    }
}