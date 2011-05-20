
using System;
using Limaki.Model.Streams;
using Limaki.UseCases;
using Limaki.UseCases.Viewers;
using Limaki.Visuals;
using Limada.View;
using Limaki.Presenter.Visuals.UI;
using Limaki.Drawing;
using System.IO;
using Limaki.Common;

namespace Limaki {


    public class StreamImportManager : StreamImportManagerBase {
        public string DefaultExtension = null;
        string _fileProviderFilter = null;
        public string FileProviderFilter {
            get {
                if (_fileProviderFilter == null) {
                    _fileProviderFilter = string.Empty;
                    var providers = Registry.Pool.TryGetCreate<StreamProviders>();
                    string defaultFilter = null;
                    foreach (var provider in providers) {
                        foreach (var info in provider.SupportedStreamTypes) {
                            string filter = info.StreamTypeDescription + "|*" + info.Extension + "|";
                            if (info.Extension == DefaultExtension)
                                defaultFilter = filter;
                            else
                                _fileProviderFilter += filter;

                        }
                    }
                    if (defaultFilter != null) {
                        _fileProviderFilter = defaultFilter + _fileProviderFilter;
                    }
                }
                return _fileProviderFilter;
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
            DefaultDialogValues(SaveFileDialog);
            this.Content = OnExport();
            if (this.Content != null) {
                var info = GetStreamTypeInfo (this.Content);
                if (info != null) {
                    SaveFileDialog.DefaultExt = info.Extension;
                    SaveFileDialog.FileName = this.Content.Description.ToString ();
                    SaveFileDialog.Filter = info.StreamTypeDescription + "|*" + info.Extension + "|" + "All Files|*.*";
                    if (FileDialogShow (SaveFileDialog, true) == DialogResult.OK) {
                        this.SaveFile (IOUtils.UriFromFileName (SaveFileDialog.FileName));
                    }
                }
            }
        }

        public void DefaultDialogValues(FileDialogMemento dialog) {
            dialog.Filter = this.FileProviderFilter + "All Files|*.*";
            dialog.DefaultExt = "";
            dialog.AddExtension = true;
            dialog.CheckFileExists = false;
            dialog.CheckPathExists = true;
            dialog.AutoUpgradeEnabled = true;
            dialog.SupportMultiDottedExtensions = true;
            // Important! under windows this adds the fileextension!
            dialog.ValidateNames = true;

        }

        public void ImportContent(StreamInfo<Stream> content, Scene scene, IGraphLayout<IVisual, IVisualEdge> layout) {
            var graph = scene.Graph;
            var thing = new VisualThingStreamHelper().CreateFromStream(graph, content);
            if (scene.Focused != null) {
                SceneTools.PlaceVisual(scene.Focused, thing, scene, layout);
            } else {
                SceneTools.AddItem(scene, thing, layout, scene.NoHit);
            }
        }

        public StreamInfo<Stream> ExportContent(Scene scene) {
            var graph = scene.Graph;
            if (graph!=null && scene.Focused != null) {
                return new VisualThingStreamHelper ().GetStream (graph,scene.Focused);
            }
            return null;
        }
    }
}
