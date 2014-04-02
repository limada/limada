using Xwt.Drawing;
using Limaki.Iconerias;

namespace Limaki.View.Properties {
    
    public class Iconery {

        public static T Create<T> () where T:Iconeria, new() {
            var fillColor = new Xwt.Drawing.Color (.3, .3, .3, .8);
            var strokeColor = new Xwt.Drawing.Color (.1, .1, .1, .3);
            fillColor = Xwt.Drawing.Colors.DarkSlateBlue.WithAlpha (.8);
            strokeColor = Xwt.Drawing.Colors.LemonChiffon.WithAlpha (.3);
            return new T {
                             Fill = true,
                             FillColor = fillColor,
                             StrokeColor = strokeColor,
                             Stroke = true,
                             StrokeFirst = false,
                             DefaultSize = DefaultSize,
                         };
        }

        public static Xwt.Size DefaultSize = new Xwt.Size (24, 24);

        static Iconery () { Compose (); }

        public static void Compose () {

            //strokeColor = fillColor.WithAlpha(.3);
            var awesome = Create<AwesomeIconeria>();
            var limadaIcons = Create<LimadaIconeria> ();
            var size = awesome.DefaultSize.Width;

            ArrangeLeft = awesome.AsImage(awesome.FaAlignLeft, size); //global::Limaki.View.Properties.Resources.ArrangeLeft;
            ArrangeCenter = awesome.AsImage (awesome.FaAlignCenter, size); // global::Limaki.View.Properties.Resources.ArrangeCenter;
            ArrangeRight = awesome.AsImage (awesome.FaAlignRight, size); //global::Limaki.View.Properties.Resources.ArrangeRight;

            ArrangeMiddle = limadaIcons.AsImage(limadaIcons.IconVerticalAlignCenter, size); //global::Limaki.View.Properties.Resources.ArrangeMiddle;
            ArrangeTop = limadaIcons.AsImage(limadaIcons.IconVerticalAlignTop, size); //global::Limaki.View.Properties.Resources.ArrangeTop;
            ArrangeBottom = limadaIcons.AsImage(limadaIcons.IconVerticalAlignBottom, size); //global::Limaki.View.Properties.Resources.ArrangeBottom;

            ArrageRows = awesome.AsImage (awesome.FaTh, size); //global::Limaki.View.Properties.Resources.ArrageRows;
            ArrangeOneRow = awesome.AsImage(awesome.FaBars, size); //global::Limaki.View.Properties.Resources.ArrangeOneRow;

            DimensionX = limadaIcons.AsImage(limadaIcons.IconDimensionX, size*1.05); //global::Limaki.View.Properties.Resources.DimensionX;
            DimensionY = limadaIcons.AsImage (limadaIcons.IconDimensionY, size * 1.05); //global::Limaki.View.Properties.Resources.DimensionY;

            LogicalLayout = limadaIcons.AsImage(limadaIcons.IconLogicalLayoutSelected, size); //global::Limaki.View.Properties.Resources.LogicalLayout;
            FullLayout = awesome.AsImage(limadaIcons.IconFullLayout, size); //global::Limaki.View.Properties.Resources.ModifyLayout24;

            LogicalLayoutLeaf = limadaIcons.AsImage(limadaIcons.IconLogicalLayoutSelectedLeaf, size);

            Undo = awesome.AsImage(awesome.FaUndo, size); //global::Limaki.View.Properties.Resources.Undo;

            Cancel = awesome.AsImage(awesome.FaTimes, size); //global::Limaki.View.Properties.Resources.cancel;
            OK = awesome.AsImage(awesome.FaCheck, size); //global::Limaki.View.Properties.Resources.ok;


            NewSheet = awesome.AsImage (limadaIcons.IconNewSheet, size * 1.15); //global::Limaki.View.Properties.Resources.document_new;
            NewNote = awesome.AsImage (awesome.FaPencilSquareO, size * 1.15); //global::Limaki.View.Properties.Resources.notes;
            SaveContent = awesome.AsImage (awesome.FaDownload, size * 1.05); //global::Limaki.View.Properties.Resources.stream_save;

            FontBoldIcon = awesome.AsImage (awesome.FaBold, size * .80); //global::Limaki.View.Properties.Resources.FontBoldIcon;
            FontItalicIcon = awesome.AsImage (awesome.FaItalic, size * .80); //global::Limaki.View.Properties.Resources.FontItalicIcon;
            FontUnderlineIcon = awesome.AsImage (awesome.FaUnderline, size * .80); //global::Limaki.View.Properties.Resources.FontUnderlineIcon;

            GoNext = awesome.AsImage(awesome.FaChevronRight, size); //global::Limaki.View.Properties.Resources.go_next;
            GoPrevious = awesome.AsImage(awesome.FaChevronLeft, size); //global::Limaki.View.Properties.Resources.go_previous;
            GoHome = awesome.AsImage (awesome.FaHome, size * 1.15); //global::Limaki.View.Properties.Resources.gohome;

            GraphGraphView = limadaIcons.AsImage (limadaIcons.IconGraphGraph, size * 1.15); //global::Limaki.View.Properties.Resources.GraphGraphView;
            GraphContentView = limadaIcons.AsImage (limadaIcons.IconGraphContent, size * 1.15); //global::Limaki.View.Properties.Resources.GraphDocView;
            ToggleView = limadaIcons.AsImage (limadaIcons.IconToggleView, size * 1.15); //global::Limaki.View.Properties.Resources.ToggleView;
            NewViewVisualNote = limadaIcons.AsImage (awesome.FaShare, size * 1.15); 

            Panning = awesome.AsImage(awesome.FaArrows, size); //global::Limaki.View.Properties.Resources.MoveShift;
            Zoom = awesome.AsImage(awesome.FaSearchPlus, size); //global::Limaki.View.Properties.Resources.ZoomToolIcon;
            FitToWidth = awesome.AsImage(awesome.FaArrowsH, size);
            FitToHeigth = awesome.AsImage (awesome.FaArrowsV, size);
            FitToScreen = awesome.AsImage (awesome.FaExpand, size);
            OriginalSize = awesome.AsImage (awesome.FaCompress, size);

            Select = awesome.AsImage(awesome.FaPencil, size); //global::Limaki.View.Properties.Resources.DrawSelection;

            awesome.FillColor = Xwt.Drawing.Colors.Red.WithAlpha(.7);

            StyleItem = awesome.AsImage(awesome.FaTint, size); //global::Limaki.View.Properties.Resources.ItemLayout;



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

        public static Image GraphContentView { get; set; }

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

        public static Image LimadaLogoL32 { get; set; }

        public static Image LogicalLayoutLeaf { get; set; }

        public static Image NewViewVisualNote { get; set; }

        public static Image FitToWidth { get; set; }

        public static Image FitToHeigth { get; set; }

        public static Image FitToScreen { get; set; }

        public static Image OriginalSize { get; set; }
    }
}