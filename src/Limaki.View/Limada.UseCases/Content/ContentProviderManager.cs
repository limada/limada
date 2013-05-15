using System;
using System.IO;
using Limada.VisualThings;
using Limaki;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Model.Content;
using Limaki.Model.Content.Providers;
using Limaki.View.Visuals.UI;
using Limaki.Viewers;
using Limaki.Visuals;

namespace Limada.Usecases {

    public class ContentProviderManager : ContentProviderManagerBase {

        public string DefaultExtension = null;

        string _contentProviderDialogFilter = null;
        public string ContentProviderDialogFilter {
            get {
                if (_contentProviderDialogFilter == null) {
                    _contentProviderDialogFilter = string.Empty;
                    var providers = Registry.Pool.TryGetCreate<ContentProviders>();
                    string defaultFilter = null;
                    foreach (var provider in providers) {
                        foreach (var info in provider.SupportedContents) {
                            string filter = info.Description + "|*" + info.Extension + "|";
                            if (info.Extension == DefaultExtension)
                                defaultFilter = filter;
                            else
                                _contentProviderDialogFilter += filter;

                        }
                    }
                    if (defaultFilter != null) {
                        _contentProviderDialogFilter = defaultFilter + _contentProviderDialogFilter;
                    }
                }
                return _contentProviderDialogFilter;
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
            dialog.Filter = this.ContentProviderDialogFilter + "All Files|*.*";
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