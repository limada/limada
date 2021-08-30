using Xwt.Drawing;
using Xwt;

namespace Limaki.Iconerias {

    public class Iconery {

        public static T Create<T> () where T : Iconeria, new () {
            return new T {
                Fill = true,
                FillColor = FillColor,
                StrokeColor = StrokeColor,
                Stroke = true,
                StrokeFirst = false,
                DefaultSize = DefaultSize,
            };
        }

        public static Size DefaultSize = new Size (24, 24);
        public static Color FillColor = Xwt.Drawing.Colors.DarkSlateBlue.WithAlpha (.8);
        public static Color StrokeColor = Xwt.Drawing.Colors.LemonChiffon.WithAlpha (.3);

        static Iconery () {
            Compose ();
        }

        public static void Compose () {

            //strokeColor = fillColor.WithAlpha(.3);
            var awesome = Create<AwesomeIconeria> ();
            awesome.Stroke = false;
            var limadaIcons = Create<LimadaIconeria> ();
            var size = awesome.DefaultSize.Width;

            Arrange = awesome.AsImage (a => a.FaSitemap);
            ArrangeLeft = awesome.AsImage (a => a.FaAlignLeft);
            ArrangeCenter = awesome.AsImage (a => a.FaAlignCenter);
            ArrangeRight = awesome.AsImage (a => a.FaAlignRight);

            ArrangeMiddle = limadaIcons.AsImage (a => a.VerticalAlignCenter);
            ArrangeTop = limadaIcons.AsImage (a => a.VerticalAlignTop);
            ArrangeBottom = limadaIcons.AsImage (a => a.VerticalAlignBottom);

            ArrageRows = awesome.AsImage (a => a.FaTh);
            ArrangeOneRow = awesome.AsImage (a => a.FaBars);

            DimensionX = limadaIcons.AsImage (a => a.DimensionX, size * 1.05);
            DimensionY = limadaIcons.AsImage (a => a.DimensionY, size * 1.05);

            LogicalLayout = limadaIcons.AsImage (a => a.LogicalLayoutSelected);
            FullLayout = limadaIcons.AsImage (a => a.FullLayout);

            LogicalLayoutLeaf = limadaIcons.AsImage (a => a.LogicalLayoutSelectedLeaf);

            Undo = awesome.AsImage (a => a.FaUndo);

            Cancel = awesome.AsImage (a => a.FaTimes);
            OK = awesome.AsImage (a => a.FaCheck);

            NewSheet = limadaIcons.AsImage (a => a.NewSheet, size * 1.15);
            NewNote = awesome.AsImage (a => a.FaPenSquare, size * 1.15);

            FileMenu = awesome.AsImage (a => a.FaNewspaper, size * 1.05);
            SaveContent = awesome.AsImage (a => a.FaDownload, size * 1.05);
            OpenFile = awesome.AsImage (a => a.FaFolderOpen, size * 1.05);
            CreateFile = awesome.AsImage (a => a.FaFileImport, size * 1.05);

            FontBoldIcon = awesome.AsImage (a => a.FaBold, size * .80);
            FontItalicIcon = awesome.AsImage (a => a.FaItalic, size * .80);
            FontUnderlineIcon = awesome.AsImage (a => a.FaUnderline, size * .80);
            FontStrikeThroughIcon = awesome.AsImage (a => a.FaStrikethrough, size * .80);

            GoNext = awesome.AsImage (a => a.FaChevronRight);
            GoPrevious = awesome.AsImage (a => a.FaChevronLeft);
            GoHome = awesome.AsImage (a => a.FaHome, size * 1.15);

            Search = awesome.AsImage (a => a.FaSearch);

            GraphGraphView = limadaIcons.AsImage (a => a.GraphGraph, size * 1.15);
            GraphContentView = limadaIcons.AsImage (a => a.GraphContent, size * 1.15);
            ToggleView = limadaIcons.AsImage (a => a.ToggleView, size * 1.15);
            NewViewVisualNote = awesome.AsImage (a => a.FaShare, size * 1.15);

            Panning = awesome.AsImage (a => a.FaArrowsAlt);
            Zoom = awesome.AsImage (a => a.FaSearchPlus);
            FitToWidth = awesome.AsImage (a => a.FaArrowsAltH);
            FitToHeigth = awesome.AsImage (a => a.FaArrowsAltV);
            FitToScreen = awesome.AsImage (a => a.FaExpand);
            OriginalSize = awesome.AsImage (a => a.FaCompress);

            Select = awesome.AsImage (a => a.FaPencilAlt);

            ToggleVisual = awesome.AsImage (a => a.FaExpand);
            HideVisual = awesome.AsImage (a => a.FaEyeSlash);
            AddVisual = awesome.AsImage (a => a.FaPlus);
            AddLink = awesome.AsImage (a => a.FaLink);
            CopyItemToAdjacent = awesome.AsImage (a => a.FaCaretLeft);

            Delete = awesome.AsImage (a => a.FaTrash);

            ViewState = awesome.AsImage (a => a.FaNewspaper); 

            awesome.FillColor = Xwt.Drawing.Colors.Red.WithAlpha (.7);

            StyleItem = awesome.AsImage (a => a.FaTint);

            LimadaLogo = ConvertedResources.LimadaLogo;
            SubWinIcon = ConvertedResources.SubWinIcon;

            awesome.FillColor = Xwt.Drawing.Colors.Aqua.WithAlpha (1);
            awesome.StrokeColor = Colors.Black;
            awesome.LineWidth = 1;

        }

        public static Image Arrange { get; set; }

        public static Image ArrageRows { get; set; }

        public static Image ArrangeBottom { get; set; }

        public static Image ArrangeCenter { get; set; }

        public static Image ArrangeLeft { get; set; }

        public static Image ArrangeMiddle { get; set; }

        public static Image ArrangeOneRow { get; set; }

        public static Image ArrangeRight { get; set; }

        public static Image ArrangeTop { get; set; }

        public static Image Cancel { get; set; }

        public static Image DimensionX { get; set; }

        public static Image DimensionY { get; set; }

        public static Image NewSheet { get; set; }

        public static Image Select { get; set; }

        public static Image FontBoldIcon { get; set; }

        public static Image FontItalicIcon { get; set; }

        public static Image FontUnderlineIcon { get; set; }

        public static Image FontStrikeThroughIcon { get; set; }

        public static Image GoNext { get; set; }

        public static Image GoPrevious { get; set; }

        public static Image GoHome { get; set; }

        public static Image Search { get; set; }

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

        public static Image LogicalLayoutLeaf { get; set; }

        public static Image NewViewVisualNote { get; set; }

        public static Image FitToWidth { get; set; }

        public static Image FitToHeigth { get; set; }

        public static Image FitToScreen { get; set; }

        public static Image OriginalSize { get; set; }

        public static Image LimadaLogo { get; set; }

        public static Image SubWinIcon { get; set; }

        public static Image OpenFile { get; set; }

        public static Image CreateFile { get; set; }

        public static Image ToggleVisual { get; set; }

        public static Image HideVisual { get; set; }

        public static Image CopyItemToAdjacent { get; set; }

        public static Image AddVisual { get; set; }

        public static Image AddLink { get; set; }

        public static Image Delete { get; set; }

        public static Image ViewState { get; set; }
        public static Image FileMenu { get; private set; }

	}
}