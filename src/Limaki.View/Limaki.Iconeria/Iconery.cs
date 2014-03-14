using Xwt.Drawing;
using Limaki.Iconerias;

namespace Limaki.View.Properties {
    
    public class Iconery {

        static Iconery () {
            var fillColor = new Xwt.Drawing.Color(.3, .3, .3, .8);
            var strokeColor = new Xwt.Drawing.Color(.1, .1, .1, .3);
            fillColor = Xwt.Drawing.Colors.DarkSlateBlue.WithAlpha(.8);
            strokeColor = Xwt.Drawing.Colors.LemonChiffon.WithAlpha(.3);

            //strokeColor = fillColor.WithAlpha(.3);
            var awesome = new AwesomeIconeria {
                                                  Fill = true,
                                                  FillColor = fillColor,
                                                  StrokeColor = strokeColor,
                                                  Stroke = true,
                                                  StrokeFirst = false,
                                              };
            var limadaIcons = new LimadaIconeria {
                                                     Fill = awesome.Fill,
                                                     FillColor = awesome.FillColor,
                                                     StrokeColor = awesome.StrokeColor,
                                                     Stroke = awesome.Stroke,
                                                     StrokeFirst = awesome.StrokeFirst,
                                                 };
            var size = 24;

            ArrangeLeft = awesome.AsImage(awesome.IconAlignLeft, size); //global::Limaki.View.Properties.Resources.ArrangeLeft;
            ArrangeCenter = awesome.AsImage(awesome.IconAlignCenter, size); // global::Limaki.View.Properties.Resources.ArrangeCenter;
            ArrangeRight = awesome.AsImage(awesome.IconAlignRight, size); //global::Limaki.View.Properties.Resources.ArrangeRight;

            ArrangeMiddle = limadaIcons.AsImage(limadaIcons.IconVerticalAlignCenter, size); //global::Limaki.View.Properties.Resources.ArrangeMiddle;
            ArrangeTop = limadaIcons.AsImage(limadaIcons.IconVerticalAlignTop, size); //global::Limaki.View.Properties.Resources.ArrangeTop;
            ArrangeBottom = limadaIcons.AsImage(limadaIcons.IconVerticalAlignBottom, size); //global::Limaki.View.Properties.Resources.ArrangeBottom;

            ArrageRows = awesome.AsImage(awesome.IconTh, size); //global::Limaki.View.Properties.Resources.ArrageRows;
            ArrangeOneRow = awesome.AsImage(awesome.IconReorder, size); //global::Limaki.View.Properties.Resources.ArrangeOneRow;

            DimensionX = limadaIcons.AsImage(limadaIcons.IconDimensionX, 26); //global::Limaki.View.Properties.Resources.DimensionX;
            DimensionY = limadaIcons.AsImage(limadaIcons.IconDimensionY, 26); //global::Limaki.View.Properties.Resources.DimensionY;

            LogicalLayout = limadaIcons.AsImage(limadaIcons.IconLogicalLayoutSelected, size); //global::Limaki.View.Properties.Resources.LogicalLayout;
            FullLayout = awesome.AsImage(limadaIcons.IconFullLayout, size); //global::Limaki.View.Properties.Resources.ModifyLayout24;

            LogicalLayoutLeaf = limadaIcons.AsImage(limadaIcons.IconLogicalLayoutSelectedLeaf, size);

            Undo = awesome.AsImage(awesome.IconUndo, size); //global::Limaki.View.Properties.Resources.Undo;

            Cancel = awesome.AsImage(awesome.IconRemove, size); //global::Limaki.View.Properties.Resources.cancel;
            OK = awesome.AsImage(awesome.IconOk, size); //global::Limaki.View.Properties.Resources.ok;


            NewSheet = awesome.AsImage(limadaIcons.IconNewSheet, 28); //global::Limaki.View.Properties.Resources.document_new;
            NewNote = awesome.AsImage(awesome.IconEdit, 28); //global::Limaki.View.Properties.Resources.notes;
            SaveContent = awesome.AsImage(awesome.IconDownloadAlt, 26); //global::Limaki.View.Properties.Resources.stream_save;

            FontBoldIcon = awesome.AsImage(awesome.IconBold, 20); //global::Limaki.View.Properties.Resources.FontBoldIcon;
            FontItalicIcon = awesome.AsImage(awesome.IconItalic, 20); //global::Limaki.View.Properties.Resources.FontItalicIcon;
            FontUnderlineIcon = awesome.AsImage(awesome.IconUnderline, 20); //global::Limaki.View.Properties.Resources.FontUnderlineIcon;

            GoNext = awesome.AsImage(awesome.IconChevronRight, size); //global::Limaki.View.Properties.Resources.go_next;
            GoPrevious = awesome.AsImage(awesome.IconChevronLeft, size); //global::Limaki.View.Properties.Resources.go_previous;
            GoHome = awesome.AsImage(awesome.IconHome, 26); //global::Limaki.View.Properties.Resources.gohome;

            GraphGraphView = limadaIcons.AsImage(limadaIcons.IconGraphGraph, 28); //global::Limaki.View.Properties.Resources.GraphGraphView;
            GraphContentView = limadaIcons.AsImage(limadaIcons.IconGraphContent, 28); //global::Limaki.View.Properties.Resources.GraphDocView;
            ToggleView = limadaIcons.AsImage(limadaIcons.IconToggleView, 28); //global::Limaki.View.Properties.Resources.ToggleView;
            NewViewVisualNote = limadaIcons.AsImage (awesome.IconShare, 28); 

            Panning = awesome.AsImage(awesome.IconMove, size); //global::Limaki.View.Properties.Resources.MoveShift;
            Zoom = awesome.AsImage(awesome.IconZoomIn, size); //global::Limaki.View.Properties.Resources.ZoomToolIcon;
            Select = awesome.AsImage(awesome.IconPencil, size); //global::Limaki.View.Properties.Resources.DrawSelection;

            awesome.FillColor = Xwt.Drawing.Colors.Red.WithAlpha(.7);

            StyleItem = awesome.AsImage(awesome.IconTint, size); //global::Limaki.View.Properties.Resources.ItemLayout;



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
    }
}