/*
 * Limaki 
 * Version 0.081
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Authors:
 * Dennis Hayes (dennish@raytek.com)
 * Ben Houston  (ben@exocortex.org)
 * Gonzalo Paniagua Javier (gonzalo@ximian.com)
 * Peter Dennis Bartok (pbartok@novell.com)
 * Sebastien Pouliot  <sebastien@ximian.com>
 *
 * (C) 2002 Dennis Hayes
 * (C) Copyright 2004,2006 Novell, Inc (http://www.novell.com)
 * 
 * http://limada.sourceforge.net
 * 
 */

/* 
 * this file is ported from
 * mono 2.4 - mcs\class\System.Drawing\System.Drawing\KnownColor.cs and
 * mono 2.4 - mcs\class\System.Drawing\System.Drawing\KnownColors.cs
 */




using Limaki.Common;
using System;

namespace Limaki.Drawing {
    public enum KnownColor {
        ActiveBorder = 1,
        ActiveCaption = 2,
        ActiveCaptionText = 3,
        AppWorkspace = 4,
        Control = 5,
        ControlDark = 6,
        ControlDarkDark = 7,
        ControlLight = 8,
        ControlLightLight = 9,
        ControlText = 10,
        Desktop = 11,
        GrayText = 12,
        Highlight = 13,
        HighlightText = 14,
        HotTrack = 15,
        InactiveBorder = 16,
        InactiveCaption = 17,
        InactiveCaptionText = 18,
        Info = 19,
        InfoText = 20,
        Menu = 21,
        MenuText = 22,
        ScrollBar = 23,
        Window = 24,
        WindowFrame = 25,
        WindowText = 26,
        Transparent = 27,
        AliceBlue = 28,
        AntiqueWhite = 29,
        Aqua = 30,
        Aquamarine = 31,
        Azure = 32,
        Beige = 33,
        Bisque = 34,
        Black = 35,
        BlanchedAlmond = 36,
        Blue = 37,
        BlueViolet = 38,
        Brown = 39,
        BurlyWood = 40,
        CadetBlue = 41,
        Chartreuse = 42,
        Chocolate = 43,
        Coral = 44,
        CornflowerBlue = 45,
        Cornsilk = 46,
        Crimson = 47,
        Cyan = 48,
        DarkBlue = 49,
        DarkCyan = 50,
        DarkGoldenrod = 51,
        DarkGray = 52,
        DarkGreen = 53,
        DarkKhaki = 54,
        DarkMagenta = 55,
        DarkOliveGreen = 56,
        DarkOrange = 57,
        DarkOrchid = 58,
        DarkRed = 59,
        DarkSalmon = 60,
        DarkSeaGreen = 61,
        DarkSlateBlue = 62,
        DarkSlateGray = 63,
        DarkTurquoise = 64,
        DarkViolet = 65,
        DeepPink = 66,
        DeepSkyBlue = 67,
        DimGray = 68,
        DodgerBlue = 69,
        Firebrick = 70,
        FloralWhite = 71,
        ForestGreen = 72,
        Fuchsia = 73,
        Gainsboro = 74,
        GhostWhite = 75,
        Gold = 76,
        Goldenrod = 77,
        Gray = 78,
        Green = 79,
        GreenYellow = 80,
        Honeydew = 81,
        HotPink = 82,
        IndianRed = 83,
        Indigo = 84,
        Ivory = 85,
        Khaki = 86,
        Lavender = 87,
        LavenderBlush = 88,
        LawnGreen = 89,
        LemonChiffon = 90,
        LightBlue = 91,
        LightCoral = 92,
        LightCyan = 93,
        LightGoldenrodYellow = 94,
        LightGray = 95,
        LightGreen = 96,
        LightPink = 97,
        LightSalmon = 98,
        LightSeaGreen = 99,
        LightSkyBlue = 100,
        LightSlateGray = 101,
        LightSteelBlue = 102,
        LightYellow = 103,
        Lime = 104,
        LimeGreen = 105,
        Linen = 106,
        Magenta = 107,
        Maroon = 108,
        MediumAquamarine = 109,
        MediumBlue = 110,
        MediumOrchid = 111,
        MediumPurple = 112,
        MediumSeaGreen = 113,
        MediumSlateBlue = 114,
        MediumSpringGreen = 115,
        MediumTurquoise = 116,
        MediumVioletRed = 117,
        MidnightBlue = 118,
        MintCream = 119,
        MistyRose = 120,
        Moccasin = 121,
        NavajoWhite = 122,
        Navy = 123,
        OldLace = 124,
        Olive = 125,
        OliveDrab = 126,
        Orange = 127,
        OrangeRed = 128,
        Orchid = 129,
        PaleGoldenrod = 130,
        PaleGreen = 131,
        PaleTurquoise = 132,
        PaleVioletRed = 133,
        PapayaWhip = 134,
        PeachPuff = 135,
        Peru = 136,
        Pink = 137,
        Plum = 138,
        PowderBlue = 139,
        Purple = 140,
        Red = 141,
        RosyBrown = 142,
        RoyalBlue = 143,
        SaddleBrown = 144,
        Salmon = 145,
        SandyBrown = 146,
        SeaGreen = 147,
        SeaShell = 148,
        Sienna = 149,
        Silver = 150,
        SkyBlue = 151,
        SlateBlue = 152,
        SlateGray = 153,
        Snow = 154,
        SpringGreen = 155,
        SteelBlue = 156,
        Tan = 157,
        Teal = 158,
        Thistle = 159,
        Tomato = 160,
        Turquoise = 161,
        Violet = 162,
        Wheat = 163,
        White = 164,
        WhiteSmoke = 165,
        Yellow = 166,
        YellowGreen = 167,

        ButtonFace = 168,
        ButtonHighlight = 169,
        ButtonShadow = 170,
        GradientActiveCaption = 171,
        GradientInactiveCaption = 172,
        MenuBar = 173,
        MenuHighlight = 174

    }

    public static class KnownColors {
        static internal uint[] ArgbValues = new uint[] {
                                                           0x00000000,	/* 000 - Empty */
                                                           0xFFD4D0C8,	/* 001 - ActiveBorder */
                                                           0xFF0054E3,	/* 002 - ActiveCaption */
                                                           0xFFFFFFFF,	/* 003 - ActiveCaptionText */
                                                           0xFF808080,	/* 004 - AppWorkspace */
                                                           0xFFECE9D8,	/* 005 - Control */
                                                           0xFFACA899,	/* 006 - ControlDark */
                                                           0xFF716F64,	/* 007 - ControlDarkDark */
                                                           0xFFF1EFE2,	/* 008 - ControlLight */
                                                           0xFFFFFFFF,	/* 009 - ControlLightLight */
                                                           0xFF000000,	/* 010 - ControlText */
                                                           0xFF004E98,	/* 011 - Desktop */
                                                           0xFFACA899,	/* 012 - GrayText */
                                                           0xFF316AC5,	/* 013 - Highlight */
                                                           0xFFFFFFFF,	/* 014 - HighlightText */
                                                           0xFF000080,	/* 015 - HotTrack */
                                                           0xFFD4D0C8,	/* 016 - InactiveBorder */
                                                           0xFF7A96DF,	/* 017 - InactiveCaption */
                                                           0xFFD8E4F8,	/* 018 - InactiveCaptionText */
                                                           0xFFFFFFE1,	/* 019 - Info */
                                                           0xFF000000,	/* 020 - InfoText */
                                                           0xFFFFFFFF,	/* 021 - Menu */
                                                           0xFF000000,	/* 022 - MenuText */
                                                           0xFFD4D0C8,	/* 023 - ScrollBar */
                                                           0xFFFFFFFF,	/* 024 - Window */
                                                           0xFF000000,	/* 025 - WindowFrame */
                                                           0xFF000000,	/* 026 - WindowText */
                                                           0x00FFFFFF,	/* 027 - Transparent */
                                                           0xFFF0F8FF,	/* 028 - AliceBlue */
                                                           0xFFFAEBD7,	/* 029 - AntiqueWhite */
                                                           0xFF00FFFF,	/* 030 - Aqua */
                                                           0xFF7FFFD4,	/* 031 - Aquamarine */
                                                           0xFFF0FFFF,	/* 032 - Azure */
                                                           0xFFF5F5DC,	/* 033 - Beige */
                                                           0xFFFFE4C4,	/* 034 - Bisque */
                                                           0xFF000000,	/* 035 - Black */
                                                           0xFFFFEBCD,	/* 036 - BlanchedAlmond */
                                                           0xFF0000FF,	/* 037 - Blue */
                                                           0xFF8A2BE2,	/* 038 - BlueViolet */
                                                           0xFFA52A2A,	/* 039 - Brown */
                                                           0xFFDEB887,	/* 040 - BurlyWood */
                                                           0xFF5F9EA0,	/* 041 - CadetBlue */
                                                           0xFF7FFF00,	/* 042 - Chartreuse */
                                                           0xFFD2691E,	/* 043 - Chocolate */
                                                           0xFFFF7F50,	/* 044 - Coral */
                                                           0xFF6495ED,	/* 045 - CornflowerBlue */
                                                           0xFFFFF8DC,	/* 046 - Cornsilk */
                                                           0xFFDC143C,	/* 047 - Crimson */
                                                           0xFF00FFFF,	/* 048 - Cyan */
                                                           0xFF00008B,	/* 049 - DarkBlue */
                                                           0xFF008B8B,	/* 050 - DarkCyan */
                                                           0xFFB8860B,	/* 051 - DarkGoldenrod */
                                                           0xFFA9A9A9,	/* 052 - DarkGray */
                                                           0xFF006400,	/* 053 - DarkGreen */
                                                           0xFFBDB76B,	/* 054 - DarkKhaki */
                                                           0xFF8B008B,	/* 055 - DarkMagenta */
                                                           0xFF556B2F,	/* 056 - DarkOliveGreen */
                                                           0xFFFF8C00,	/* 057 - DarkOrange */
                                                           0xFF9932CC,	/* 058 - DarkOrchid */
                                                           0xFF8B0000,	/* 059 - DarkRed */
                                                           0xFFE9967A,	/* 060 - DarkSalmon */
                                                           0xFF8FBC8B,	/* 061 - DarkSeaGreen */
                                                           0xFF483D8B,	/* 062 - DarkSlateBlue */
                                                           0xFF2F4F4F,	/* 063 - DarkSlateGray */
                                                           0xFF00CED1,	/* 064 - DarkTurquoise */
                                                           0xFF9400D3,	/* 065 - DarkViolet */
                                                           0xFFFF1493,	/* 066 - DeepPink */
                                                           0xFF00BFFF,	/* 067 - DeepSkyBlue */
                                                           0xFF696969,	/* 068 - DimGray */
                                                           0xFF1E90FF,	/* 069 - DodgerBlue */
                                                           0xFFB22222,	/* 070 - Firebrick */
                                                           0xFFFFFAF0,	/* 071 - FloralWhite */
                                                           0xFF228B22,	/* 072 - ForestGreen */
                                                           0xFFFF00FF,	/* 073 - Fuchsia */
                                                           0xFFDCDCDC,	/* 074 - Gainsboro */
                                                           0xFFF8F8FF,	/* 075 - GhostWhite */
                                                           0xFFFFD700,	/* 076 - Gold */
                                                           0xFFDAA520,	/* 077 - Goldenrod */
                                                           0xFF808080,	/* 078 - Gray */
                                                           0xFF008000,	/* 079 - Green */
                                                           0xFFADFF2F,	/* 080 - GreenYellow */
                                                           0xFFF0FFF0,	/* 081 - Honeydew */
                                                           0xFFFF69B4,	/* 082 - HotPink */
                                                           0xFFCD5C5C,	/* 083 - IndianRed */
                                                           0xFF4B0082,	/* 084 - Indigo */
                                                           0xFFFFFFF0,	/* 085 - Ivory */
                                                           0xFFF0E68C,	/* 086 - Khaki */
                                                           0xFFE6E6FA,	/* 087 - Lavender */
                                                           0xFFFFF0F5,	/* 088 - LavenderBlush */
                                                           0xFF7CFC00,	/* 089 - LawnGreen */
                                                           0xFFFFFACD,	/* 090 - LemonChiffon */
                                                           0xFFADD8E6,	/* 091 - LightBlue */
                                                           0xFFF08080,	/* 092 - LightCoral */
                                                           0xFFE0FFFF,	/* 093 - LightCyan */
                                                           0xFFFAFAD2,	/* 094 - LightGoldenrodYellow */
                                                           0xFFD3D3D3,	/* 095 - LightGray */
                                                           0xFF90EE90,	/* 096 - LightGreen */
                                                           0xFFFFB6C1,	/* 097 - LightPink */
                                                           0xFFFFA07A,	/* 098 - LightSalmon */
                                                           0xFF20B2AA,	/* 099 - LightSeaGreen */
                                                           0xFF87CEFA,	/* 100 - LightSkyBlue */
                                                           0xFF778899,	/* 101 - LightSlateGray */
                                                           0xFFB0C4DE,	/* 102 - LightSteelBlue */
                                                           0xFFFFFFE0,	/* 103 - LightYellow */
                                                           0xFF00FF00,	/* 104 - Lime */
                                                           0xFF32CD32,	/* 105 - LimeGreen */
                                                           0xFFFAF0E6,	/* 106 - Linen */
                                                           0xFFFF00FF,	/* 107 - Magenta */
                                                           0xFF800000,	/* 108 - Maroon */
                                                           0xFF66CDAA,	/* 109 - MediumAquamarine */
                                                           0xFF0000CD,	/* 110 - MediumBlue */
                                                           0xFFBA55D3,	/* 111 - MediumOrchid */
                                                           0xFF9370DB,	/* 112 - MediumPurple */
                                                           0xFF3CB371,	/* 113 - MediumSeaGreen */
                                                           0xFF7B68EE,	/* 114 - MediumSlateBlue */
                                                           0xFF00FA9A,	/* 115 - MediumSpringGreen */
                                                           0xFF48D1CC,	/* 116 - MediumTurquoise */
                                                           0xFFC71585,	/* 117 - MediumVioletRed */
                                                           0xFF191970,	/* 118 - MidnightBlue */
                                                           0xFFF5FFFA,	/* 119 - MintCream */
                                                           0xFFFFE4E1,	/* 120 - MistyRose */
                                                           0xFFFFE4B5,	/* 121 - Moccasin */
                                                           0xFFFFDEAD,	/* 122 - NavajoWhite */
                                                           0xFF000080,	/* 123 - Navy */
                                                           0xFFFDF5E6,	/* 124 - OldLace */
                                                           0xFF808000,	/* 125 - Olive */
                                                           0xFF6B8E23,	/* 126 - OliveDrab */
                                                           0xFFFFA500,	/* 127 - Orange */
                                                           0xFFFF4500,	/* 128 - OrangeRed */
                                                           0xFFDA70D6,	/* 129 - Orchid */
                                                           0xFFEEE8AA,	/* 130 - PaleGoldenrod */
                                                           0xFF98FB98,	/* 131 - PaleGreen */
                                                           0xFFAFEEEE,	/* 132 - PaleTurquoise */
                                                           0xFFDB7093,	/* 133 - PaleVioletRed */
                                                           0xFFFFEFD5,	/* 134 - PapayaWhip */
                                                           0xFFFFDAB9,	/* 135 - PeachPuff */
                                                           0xFFCD853F,	/* 136 - Peru */
                                                           0xFFFFC0CB,	/* 137 - Pink */
                                                           0xFFDDA0DD,	/* 138 - Plum */
                                                           0xFFB0E0E6,	/* 139 - PowderBlue */
                                                           0xFF800080,	/* 140 - Purple */
                                                           0xFFFF0000,	/* 141 - Red */
                                                           0xFFBC8F8F,	/* 142 - RosyBrown */
                                                           0xFF4169E1,	/* 143 - RoyalBlue */
                                                           0xFF8B4513,	/* 144 - SaddleBrown */
                                                           0xFFFA8072,	/* 145 - Salmon */
                                                           0xFFF4A460,	/* 146 - SandyBrown */
                                                           0xFF2E8B57,	/* 147 - SeaGreen */
                                                           0xFFFFF5EE,	/* 148 - SeaShell */
                                                           0xFFA0522D,	/* 149 - Sienna */
                                                           0xFFC0C0C0,	/* 150 - Silver */
                                                           0xFF87CEEB,	/* 151 - SkyBlue */
                                                           0xFF6A5ACD,	/* 152 - SlateBlue */
                                                           0xFF708090,	/* 153 - SlateGray */
                                                           0xFFFFFAFA,	/* 154 - Snow */
                                                           0xFF00FF7F,	/* 155 - SpringGreen */
                                                           0xFF4682B4,	/* 156 - SteelBlue */
                                                           0xFFD2B48C,	/* 157 - Tan */
                                                           0xFF008080,	/* 158 - Teal */
                                                           0xFFD8BFD8,	/* 159 - Thistle */
                                                           0xFFFF6347,	/* 160 - Tomato */
                                                           0xFF40E0D0,	/* 161 - Turquoise */
                                                           0xFFEE82EE,	/* 162 - Violet */
                                                           0xFFF5DEB3,	/* 163 - Wheat */
                                                           0xFFFFFFFF,	/* 164 - White */
                                                           0xFFF5F5F5,	/* 165 - WhiteSmoke */
                                                           0xFFFFFF00,	/* 166 - Yellow */
                                                           0xFF9ACD32,	/* 167 - YellowGreen */

                                                           0xFFECE9D8,	/* 168 - ButtonFace */
                                                           0xFFFFFFFF,	/* 169 - ButtonHighlight */
                                                           0xFFACA899,	/* 170 - ButtonShadow */
                                                           0xFF3D95FF,	/* 171 - GradientActiveCaption */
                                                           0xFF9DB9EB,	/* 172 - GradientInactiveCaption */
                                                           0xFFECE9D8,	/* 173 - MenuBar */
                                                           0xFF316AC5,	/* 174 - MenuHighlight */

                                                       };

        static KnownColors() {

            RetrieveWindowsSystemColors();

        }

        static IDrawingUtils _drawingUtils = null;
        static IDrawingUtils drawingUtils {
            get {
                if (_drawingUtils == null) {
                    _drawingUtils = Registry.Factory.Create<IDrawingUtils>();
                }
                return _drawingUtils;
            }
        }

        static uint GetSysColor(SysColorIndex index) {
            return drawingUtils.GetSysColor(index);
        }

        static void RetrieveWindowsSystemColors() {
            ArgbValues[(int)KnownColor.ActiveBorder] = GetSysColor(SysColorIndex.COLOR_ACTIVEBORDER);
            ArgbValues[(int)KnownColor.ActiveCaption] = GetSysColor(SysColorIndex.COLOR_ACTIVECAPTION);
            ArgbValues[(int)KnownColor.ActiveCaptionText] = GetSysColor(SysColorIndex.COLOR_CAPTIONTEXT);
            ArgbValues[(int)KnownColor.AppWorkspace] = GetSysColor(SysColorIndex.COLOR_APPWORKSPACE);
            ArgbValues[(int)KnownColor.Control] = GetSysColor(SysColorIndex.COLOR_BTNFACE);
            ArgbValues[(int)KnownColor.ControlDark] = GetSysColor(SysColorIndex.COLOR_BTNSHADOW);
            ArgbValues[(int)KnownColor.ControlDarkDark] = GetSysColor(SysColorIndex.COLOR_3DDKSHADOW);
            ArgbValues[(int)KnownColor.ControlLight] = GetSysColor(SysColorIndex.COLOR_3DLIGHT);
            ArgbValues[(int)KnownColor.ControlLightLight] = GetSysColor(SysColorIndex.COLOR_BTNHIGHLIGHT);
            ArgbValues[(int)KnownColor.ControlText] = GetSysColor(SysColorIndex.COLOR_BTNTEXT);
            ArgbValues[(int)KnownColor.Desktop] = GetSysColor(SysColorIndex.COLOR_DESKTOP);
            ArgbValues[(int)KnownColor.GrayText] = GetSysColor(SysColorIndex.COLOR_GRAYTEXT);
            ArgbValues[(int)KnownColor.Highlight] = GetSysColor(SysColorIndex.COLOR_HIGHLIGHT);
            ArgbValues[(int)KnownColor.HighlightText] = GetSysColor(SysColorIndex.COLOR_HIGHLIGHTTEXT);
            ArgbValues[(int)KnownColor.HotTrack] = GetSysColor(SysColorIndex.COLOR_HOTLIGHT);
            ArgbValues[(int)KnownColor.InactiveBorder] = GetSysColor(SysColorIndex.COLOR_INACTIVEBORDER);
            ArgbValues[(int)KnownColor.InactiveCaption] = GetSysColor(SysColorIndex.COLOR_INACTIVECAPTION);
            ArgbValues[(int)KnownColor.InactiveCaptionText] = GetSysColor(SysColorIndex.COLOR_INACTIVECAPTIONTEXT);
            ArgbValues[(int)KnownColor.Info] = GetSysColor(SysColorIndex.COLOR_INFOBK);
            ArgbValues[(int)KnownColor.InfoText] = GetSysColor(SysColorIndex.COLOR_INFOTEXT);
            ArgbValues[(int)KnownColor.Menu] = GetSysColor(SysColorIndex.COLOR_MENU);
            ArgbValues[(int)KnownColor.MenuText] = GetSysColor(SysColorIndex.COLOR_MENUTEXT);
            ArgbValues[(int)KnownColor.ScrollBar] = GetSysColor(SysColorIndex.COLOR_SCROLLBAR);
            ArgbValues[(int)KnownColor.Window] = GetSysColor(SysColorIndex.COLOR_WINDOW);
            ArgbValues[(int)KnownColor.WindowFrame] = GetSysColor(SysColorIndex.COLOR_WINDOWFRAME);
            ArgbValues[(int)KnownColor.WindowText] = GetSysColor(SysColorIndex.COLOR_WINDOWTEXT);

            ArgbValues[(int)KnownColor.ButtonFace] = GetSysColor(SysColorIndex.COLOR_BTNFACE);
            ArgbValues[(int)KnownColor.ButtonHighlight] = GetSysColor(SysColorIndex.COLOR_BTNHIGHLIGHT);
            ArgbValues[(int)KnownColor.ButtonShadow] = GetSysColor(SysColorIndex.COLOR_BTNSHADOW);
            ArgbValues[(int)KnownColor.GradientActiveCaption] = GetSysColor(SysColorIndex.COLOR_GRADIENTACTIVECAPTION);
            ArgbValues[(int)KnownColor.GradientInactiveCaption] = GetSysColor(SysColorIndex.COLOR_GRADIENTINACTIVECAPTION);
            ArgbValues[(int)KnownColor.MenuBar] = GetSysColor(SysColorIndex.COLOR_MENUBAR);
            ArgbValues[(int)KnownColor.MenuHighlight] = GetSysColor(SysColorIndex.COLOR_MENUHIGHLIGHT);

        }

        public static Color FromKnownColor(KnownColor kc) {
            Color c;
            short n = (short)kc;
            if ((n <= 0) || (n >= ArgbValues.Length)) {
                // This is what it returns!
                c = Color.FromArgb(0, 0, 0, 0);
            } else {
                c = Color.FromArgb(ArgbValues[n]);
            }
            return c;
        }

        public static string GetName(short kc) {
            switch (kc) {
                case 1:
                    return "ActiveBorder";
                case 2:
                    return "ActiveCaption";
                case 3:
                    return "ActiveCaptionText";
                case 4:
                    return "AppWorkspace";
                case 5:
                    return "Control";
                case 6:
                    return "ControlDark";
                case 7:
                    return "ControlDarkDark";
                case 8:
                    return "ControlLight";
                case 9:
                    return "ControlLightLight";
                case 10:
                    return "ControlText";
                case 11:
                    return "Desktop";
                case 12:
                    return "GrayText";
                case 13:
                    return "Highlight";
                case 14:
                    return "HighlightText";
                case 15:
                    return "HotTrack";
                case 16:
                    return "InactiveBorder";
                case 17:
                    return "InactiveCaption";
                case 18:
                    return "InactiveCaptionText";
                case 19:
                    return "Info";
                case 20:
                    return "InfoText";
                case 21:
                    return "Menu";
                case 22:
                    return "MenuText";
                case 23:
                    return "ScrollBar";
                case 24:
                    return "Window";
                case 25:
                    return "WindowFrame";
                case 26:
                    return "WindowText";
                case 27:
                    return "Transparent";
                case 28:
                    return "AliceBlue";
                case 29:
                    return "AntiqueWhite";
                case 30:
                    return "Aqua";
                case 31:
                    return "Aquamarine";
                case 32:
                    return "Azure";
                case 33:
                    return "Beige";
                case 34:
                    return "Bisque";
                case 35:
                    return "Black";
                case 36:
                    return "BlanchedAlmond";
                case 37:
                    return "Blue";
                case 38:
                    return "BlueViolet";
                case 39:
                    return "Brown";
                case 40:
                    return "BurlyWood";
                case 41:
                    return "CadetBlue";
                case 42:
                    return "Chartreuse";
                case 43:
                    return "Chocolate";
                case 44:
                    return "Coral";
                case 45:
                    return "CornflowerBlue";
                case 46:
                    return "Cornsilk";
                case 47:
                    return "Crimson";
                case 48:
                    return "Cyan";
                case 49:
                    return "DarkBlue";
                case 50:
                    return "DarkCyan";
                case 51:
                    return "DarkGoldenrod";
                case 52:
                    return "DarkGray";
                case 53:
                    return "DarkGreen";
                case 54:
                    return "DarkKhaki";
                case 55:
                    return "DarkMagenta";
                case 56:
                    return "DarkOliveGreen";
                case 57:
                    return "DarkOrange";
                case 58:
                    return "DarkOrchid";
                case 59:
                    return "DarkRed";
                case 60:
                    return "DarkSalmon";
                case 61:
                    return "DarkSeaGreen";
                case 62:
                    return "DarkSlateBlue";
                case 63:
                    return "DarkSlateGray";
                case 64:
                    return "DarkTurquoise";
                case 65:
                    return "DarkViolet";
                case 66:
                    return "DeepPink";
                case 67:
                    return "DeepSkyBlue";
                case 68:
                    return "DimGray";
                case 69:
                    return "DodgerBlue";
                case 70:
                    return "Firebrick";
                case 71:
                    return "FloralWhite";
                case 72:
                    return "ForestGreen";
                case 73:
                    return "Fuchsia";
                case 74:
                    return "Gainsboro";
                case 75:
                    return "GhostWhite";
                case 76:
                    return "Gold";
                case 77:
                    return "Goldenrod";
                case 78:
                    return "Gray";
                case 79:
                    return "Green";
                case 80:
                    return "GreenYellow";
                case 81:
                    return "Honeydew";
                case 82:
                    return "HotPink";
                case 83:
                    return "IndianRed";
                case 84:
                    return "Indigo";
                case 85:
                    return "Ivory";
                case 86:
                    return "Khaki";
                case 87:
                    return "Lavender";
                case 88:
                    return "LavenderBlush";
                case 89:
                    return "LawnGreen";
                case 90:
                    return "LemonChiffon";
                case 91:
                    return "LightBlue";
                case 92:
                    return "LightCoral";
                case 93:
                    return "LightCyan";
                case 94:
                    return "LightGoldenrodYellow";
                case 95:
                    return "LightGray";
                case 96:
                    return "LightGreen";
                case 97:
                    return "LightPink";
                case 98:
                    return "LightSalmon";
                case 99:
                    return "LightSeaGreen";
                case 100:
                    return "LightSkyBlue";
                case 101:
                    return "LightSlateGray";
                case 102:
                    return "LightSteelBlue";
                case 103:
                    return "LightYellow";
                case 104:
                    return "Lime";
                case 105:
                    return "LimeGreen";
                case 106:
                    return "Linen";
                case 107:
                    return "Magenta";
                case 108:
                    return "Maroon";
                case 109:
                    return "MediumAquamarine";
                case 110:
                    return "MediumBlue";
                case 111:
                    return "MediumOrchid";
                case 112:
                    return "MediumPurple";
                case 113:
                    return "MediumSeaGreen";
                case 114:
                    return "MediumSlateBlue";
                case 115:
                    return "MediumSpringGreen";
                case 116:
                    return "MediumTurquoise";
                case 117:
                    return "MediumVioletRed";
                case 118:
                    return "MidnightBlue";
                case 119:
                    return "MintCream";
                case 120:
                    return "MistyRose";
                case 121:
                    return "Moccasin";
                case 122:
                    return "NavajoWhite";
                case 123:
                    return "Navy";
                case 124:
                    return "OldLace";
                case 125:
                    return "Olive";
                case 126:
                    return "OliveDrab";
                case 127:
                    return "Orange";
                case 128:
                    return "OrangeRed";
                case 129:
                    return "Orchid";
                case 130:
                    return "PaleGoldenrod";
                case 131:
                    return "PaleGreen";
                case 132:
                    return "PaleTurquoise";
                case 133:
                    return "PaleVioletRed";
                case 134:
                    return "PapayaWhip";
                case 135:
                    return "PeachPuff";
                case 136:
                    return "Peru";
                case 137:
                    return "Pink";
                case 138:
                    return "Plum";
                case 139:
                    return "PowderBlue";
                case 140:
                    return "Purple";
                case 141:
                    return "Red";
                case 142:
                    return "RosyBrown";
                case 143:
                    return "RoyalBlue";
                case 144:
                    return "SaddleBrown";
                case 145:
                    return "Salmon";
                case 146:
                    return "SandyBrown";
                case 147:
                    return "SeaGreen";
                case 148:
                    return "SeaShell";
                case 149:
                    return "Sienna";
                case 150:
                    return "Silver";
                case 151:
                    return "SkyBlue";
                case 152:
                    return "SlateBlue";
                case 153:
                    return "SlateGray";
                case 154:
                    return "Snow";
                case 155:
                    return "SpringGreen";
                case 156:
                    return "SteelBlue";
                case 157:
                    return "Tan";
                case 158:
                    return "Teal";
                case 159:
                    return "Thistle";
                case 160:
                    return "Tomato";
                case 161:
                    return "Turquoise";
                case 162:
                    return "Violet";
                case 163:
                    return "Wheat";
                case 164:
                    return "White";
                case 165:
                    return "WhiteSmoke";
                case 166:
                    return "Yellow";
                case 167:
                    return "YellowGreen";

                case 168:
                    return "ButtonFace";
                case 169:
                    return "ButtonHighlight";
                case 170:
                    return "ButtonShadow";
                case 171:
                    return "GradientActiveCaption";
                case 172:
                    return "GradientInactiveCaption";
                case 173:
                    return "MenuBar";
                case 174:
                    return "MenuHighlight";

                default:
                    return String.Empty;
            }
        }

        public static string GetName(KnownColor kc) {
            return GetName((short)kc);
        }

        public static Color FindColorMatch(Color c) {
            uint argb = (uint)c.ToArgb();
            for (int i = 0; i < KnownColors.ArgbValues.Length; i++) {
                if (argb == KnownColors.ArgbValues[i])
                    return KnownColors.FromKnownColor((KnownColor)i);
            }
            return Color.Empty;
        }

        // When this method is called, we teach any new color(s) to the Color class
        // NOTE: This is called (reflection) by System.Windows.Forms.Theme (this isn't dead code)
        public static void Update(int knownColor, int color) {
            ArgbValues[knownColor] = (uint)color;
        }
    }
}

// Authors:
//  Dennis Hayes (dennish@raytek.com)
//  Ben Houston  (ben@exocortex.org)
//	Gonzalo Paniagua Javier (gonzalo@ximian.com)
//	Peter Dennis Bartok (pbartok@novell.com)
//	Sebastien Pouliot  <sebastien@ximian.com>
//
// (C) 2002 Dennis Hayes
// Copyright (C) 2004,2006 Novell, Inc (http://www.novell.com)
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//