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
            var limadaIcons = Create<LimadaIconeria> ();
            var size = awesome.DefaultSize.Width;

            Arrange = awesome.AsImage (awesome.FaSitemap, size);
            ArrangeLeft = awesome.AsImage (awesome.FaAlignLeft, size);
            ArrangeCenter = awesome.AsImage (awesome.FaAlignCenter, size);
            ArrangeRight = awesome.AsImage (awesome.FaAlignRight, size);

            ArrangeMiddle = limadaIcons.AsImage (limadaIcons.VerticalAlignCenter, size);
            ArrangeTop = limadaIcons.AsImage (limadaIcons.VerticalAlignTop, size);
            ArrangeBottom = limadaIcons.AsImage (limadaIcons.VerticalAlignBottom, size);

            ArrageRows = awesome.AsImage (awesome.FaTh, size);
            ArrangeOneRow = awesome.AsImage (awesome.FaBars, size);

            DimensionX = limadaIcons.AsImage (limadaIcons.DimensionX, size * 1.05);
            DimensionY = limadaIcons.AsImage (limadaIcons.DimensionY, size * 1.05);

            LogicalLayout = limadaIcons.AsImage (limadaIcons.LogicalLayoutSelected, size);
            FullLayout = awesome.AsImage (limadaIcons.FullLayout, size);

            LogicalLayoutLeaf = limadaIcons.AsImage (limadaIcons.LogicalLayoutSelectedLeaf, size);

            Undo = awesome.AsImage (awesome.FaUndo, size);

            Cancel = awesome.AsImage (awesome.FaTimes, size);
            OK = awesome.AsImage (awesome.FaCheck, size);

            NewSheet = awesome.AsImage (limadaIcons.NewSheet, size * 1.15);
            NewNote = awesome.AsImage (awesome.FaPencilSquareO, size * 1.15);

            FileMenu = awesome.AsImage (awesome.FaNewspaperO, size * 1.05);
            SaveContent = awesome.AsImage (awesome.FaDownload, size * 1.05);
            OpenFile = awesome.AsImage (awesome.FaFolderOpen, size * 1.05);
            CreateFile = awesome.AsImage (awesome.FaFileO, size * 1.05);


            FontBoldIcon = awesome.AsImage (awesome.FaBold, size * .80);
            FontItalicIcon = awesome.AsImage (awesome.FaItalic, size * .80);
            FontUnderlineIcon = awesome.AsImage (awesome.FaUnderline, size * .80);
            FontStrikeThroughIcon = awesome.AsImage (awesome.FaStrikethrough, size * .80);

            GoNext = awesome.AsImage (awesome.FaChevronRight, size);
            GoPrevious = awesome.AsImage (awesome.FaChevronLeft, size);
            GoHome = awesome.AsImage (awesome.FaHome, size * 1.15);

            Search = awesome.AsImage (awesome.FaSearch, size);

            GraphGraphView = limadaIcons.AsImage (limadaIcons.GraphGraph, size * 1.15);
            GraphContentView = limadaIcons.AsImage (limadaIcons.GraphContent, size * 1.15);
            ToggleView = limadaIcons.AsImage (limadaIcons.ToggleView, size * 1.15);
            NewViewVisualNote = limadaIcons.AsImage (awesome.FaShare, size * 1.15);

            Panning = awesome.AsImage (awesome.FaArrows, size);
            Zoom = awesome.AsImage (awesome.FaSearchPlus, size);
            FitToWidth = awesome.AsImage (awesome.FaArrowsH, size);
            FitToHeigth = awesome.AsImage (awesome.FaArrowsV, size);
            FitToScreen = awesome.AsImage (awesome.FaExpand, size);
            OriginalSize = awesome.AsImage (awesome.FaCompress, size);

            Select = awesome.AsImage (awesome.FaPencil, size);

            ToggleVisual = awesome.AsImage (awesome.FaExpand, size);
            HideVisual = awesome.AsImage (awesome.FaEyeSlash, size);
            AddVisual = awesome.AsImage (awesome.FaPlus, size);
            AddLink = awesome.AsImage (awesome.FaLink, size);
            CopyItemToAdjacent = awesome.AsImage (awesome.FaCaretSquareORight, size);

            Delete = awesome.AsImage (awesome.FaTrashO, size);

            ViewState = awesome.AsImage (awesome.FaNewspaperO, size); 

            awesome.FillColor = Xwt.Drawing.Colors.Red.WithAlpha (.7);

            StyleItem = awesome.AsImage (awesome.FaTint, size);

            LimadaLogo = ConvertedResources.LimadaLogo;
            SubWinIcon = ConvertedResources.SubWinIcon;

            awesome.FillColor = Xwt.Drawing.Colors.Aqua.WithAlpha (1);
            awesome.StrokeColor = Colors.Black;
            awesome.LineWidth = 1;

            ErpIcon = awesome.AsImage (awesome.FaPaintBrush,32);

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

        public static Image ErpIcon { get; set; }

    }
}