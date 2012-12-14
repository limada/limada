using System.Drawing;
using Limaki.Iconerias;

namespace Limaki.View.Properties {

    public class Iconery {

        static Iconery () {

            var awesome = new AwesomeIconeria {
                Fill = true,
                FillColor = Xwt.Drawing.Colors.Black,
                StrokeColor = Xwt.Drawing.Colors.DarkGray,
                Stroke = false,
            };
            var size = 24;

            ArrangeLeft = awesome.ToImage(awesome.IconAlignLeft, size); //global::Limaki.View.Properties.Resources.ArrangeLeft;
            ArrangeCenter = awesome.ToImage(awesome.IconAlignCenter, size); // global::Limaki.View.Properties.Resources.ArrangeCenter;
            ArrangeRight = awesome.ToImage(awesome.IconAlignRight, size); //global::Limaki.View.Properties.Resources.ArrangeRight;

            ArrangeMiddle = global::Limaki.View.Properties.Resources.ArrangeMiddle;
            ArrangeTop = global::Limaki.View.Properties.Resources.ArrangeTop;
            ArrangeBottom = global::Limaki.View.Properties.Resources.ArrangeBottom;

            ArrageRows = awesome.ToImage(awesome.IconTh, size); //global::Limaki.View.Properties.Resources.ArrageRows;
            ArrangeOneRow = awesome.ToImage(awesome.IconReorder, size); //global::Limaki.View.Properties.Resources.ArrangeOneRow;

            DimensionX = global::Limaki.View.Properties.Resources.DimensionX;
            DimensionY = global::Limaki.View.Properties.Resources.DimensionY;

            LogicalLayout = awesome.ToImage(awesome.IconRetweet, size); //global::Limaki.View.Properties.Resources.LogicalLayout;
            FullLayout = awesome.ToImage(awesome.IconSitemap, size); //global::Limaki.View.Properties.Resources.ModifyLayout24;

            StyleItem = global::Limaki.View.Properties.Resources.ItemLayout;

            Undo = awesome.ToImage(awesome.IconUndo, size); //global::Limaki.View.Properties.Resources.Undo;

            Cancel = awesome.ToImage(awesome.IconRemove, size); //global::Limaki.View.Properties.Resources.cancel;
            OK = awesome.ToImage(awesome.IconOk, size); //global::Limaki.View.Properties.Resources.ok;

            AddVisual = global::Limaki.View.Properties.Resources.CreateWidget24;

            NewSheet = global::Limaki.View.Properties.Resources.document_new;
            NewNote = awesome.ToImage(awesome.IconEdit, 28); //global::Limaki.View.Properties.Resources.notes;
            SaveContent = awesome.ToImage(awesome.IconDownloadAlt, 26); //global::Limaki.View.Properties.Resources.stream_save;

            FontBoldIcon = awesome.ToImage(awesome.IconBold, 20); //global::Limaki.View.Properties.Resources.FontBoldIcon;
            FontItalicIcon = awesome.ToImage(awesome.IconItalic, 20); //global::Limaki.View.Properties.Resources.FontItalicIcon;
            FontUnderlineIcon = awesome.ToImage(awesome.IconUnderline, 20); //global::Limaki.View.Properties.Resources.FontUnderlineIcon;

            GoNext = awesome.ToImage(awesome.IconChevronRight, size); //global::Limaki.View.Properties.Resources.go_next;
            GoPrevious = awesome.ToImage(awesome.IconChevronLeft, size); //global::Limaki.View.Properties.Resources.go_previous;
            GoHome = awesome.ToImage(awesome.IconHome, 26); //global::Limaki.View.Properties.Resources.gohome;

            GraphGraphView = global::Limaki.View.Properties.Resources.GraphGraphView;
            GraphDocView = global::Limaki.View.Properties.Resources.GraphDocView;
            ToggleView = global::Limaki.View.Properties.Resources.ToggleView;

            Panning = awesome.ToImage(awesome.IconMove, size); //global::Limaki.View.Properties.Resources.MoveShift;
            Zoom = awesome.ToImage(awesome.IconZoomIn, size); //global::Limaki.View.Properties.Resources.ZoomToolIcon;
            Select = awesome.ToImage(awesome.IconPencil, size); //global::Limaki.View.Properties.Resources.DrawSelection;

            StraightConnector = global::Limaki.View.Properties.Resources.StraightConnector;

            LimadaLogo = global::Limaki.View.Properties.Resources.LimadaLogo;
            LimadaLogoL32 = global::Limaki.View.Properties.Resources.LimadaLogoL32;

        }

        public static Image ArrageRows { get; set; }

        public static Image ArrangeBottom { get; set; }

        public static Image ArrangeCenter { get; set; }

        public static Image ArrangeLeft { get; set; }

        public static Image ArrangeMiddle { get; set; }

        public static Image ArrangeOneRow { get; set; }

        public static Image ArrangeRight { get; set; }

        public static Image ArrangeTop { get; set; }

        public static Image Cancel { get; set; }

        public static Image AddVisual { get; set; }

        public static Image DimensionX { get; set; }

        public static Image DimensionY { get; set; }

        public static Image NewSheet { get; set; }

        public static Image Select { get; set; }

        public static Image FontBoldIcon { get; set; }

        public static Image FontItalicIcon { get; set; }

        public static Image FontUnderlineIcon { get; set; }

        public static Image GoNext { get; set; }

        public static Image GoPrevious { get; set; }

        public static Image GoHome { get; set; }

        public static Image GraphDocView { get; set; }

        public static Image GraphGraphView { get; set; }

        public static Image StyleItem { get; set; }

        public static Image LogicalLayout { get; set; }

        public static Image FullLayout { get; set; }

        public static Image Panning { get; set; }

        public static Image NewNote { get; set; }

        public static Image OK { get; set; }

        public static Image StraightConnector { get; set; }

        public static Image SaveContent { get; set; }

        public static Image ToggleView { get; set; }

        public static Image Undo { get; set; }

        public static Image Zoom { get; set; }

        public static Icon LimadaLogo { get; set; }

        public static Image LimadaLogoL32 { get; set; }
    }
}