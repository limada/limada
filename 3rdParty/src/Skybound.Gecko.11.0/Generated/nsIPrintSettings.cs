// --------------------------------------------------------------------------------------------
// Version: MPL 1.1/GPL 2.0/LGPL 2.1
// 
// The contents of this file are subject to the Mozilla Public License Version
// 1.1 (the "License"); you may not use this file except in compliance with
// the License. You may obtain a copy of the License at
// http://www.mozilla.org/MPL/
// 
// Software distributed under the License is distributed on an "AS IS" basis,
// WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License
// for the specific language governing rights and limitations under the
// License.
// 
// <remarks>
// Generated by IDLImporter from file nsIPrintSettings.idl
// 
// You should use these interfaces when you access the COM objects defined in the mentioned
// IDL/IDH file.
// </remarks>
// --------------------------------------------------------------------------------------------
namespace Gecko
{
	using System;
	using System.Runtime.InteropServices;
	using System.Runtime.InteropServices.ComTypes;
	using System.Runtime.CompilerServices;
	using System.Windows.Forms;
	
	
	/// <summary>
    /// Simplified graphics interface for JS rendering.
    /// </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("4404c94b-0506-4255-9e3c-4582dba6cfbb")]
	public interface nsIPrintSettings
	{
		
		/// <summary>
        /// Set PrintOptions
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetPrintOptions(int aType, [MarshalAs(UnmanagedType.U1)] bool aTurnOnOff);
		
		/// <summary>
        /// Get PrintOptions
        /// </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool GetPrintOptions(int aType);
		
		/// <summary>
        /// Set PrintOptions Bit field
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		int GetPrintOptionsBits();
		
		/// <summary>
        /// Get the page size in twips, considering the
        /// orientation (portrait or landscape).
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetEffectivePageSize(ref double aWidth, ref double aHeight);
		
		/// <summary>
        /// Makes a new copy
        /// </summary>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIPrintSettings Clone();
		
		/// <summary>
        /// Assigns the internal values from the "in" arg to the current object
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void Assign([MarshalAs(UnmanagedType.Interface)] nsIPrintSettings aPS);
		
		/// <summary>
        /// Data Members
        /// </summary>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIPrintSession GetPrintSessionAttribute();
		
		/// <summary>
        /// Data Members
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetPrintSessionAttribute([MarshalAs(UnmanagedType.Interface)] nsIPrintSession aPrintSession);
		
		/// <summary>
        ///We hold a weak reference </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		int GetStartPageRangeAttribute();
		
		/// <summary>
        ///We hold a weak reference </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetStartPageRangeAttribute(int aStartPageRange);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		int GetEndPageRangeAttribute();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetEndPageRangeAttribute(int aEndPageRange);
		
		/// <summary>
        /// The edge measurements define the positioning of the headers
        /// and footers on the page. They're measured as an offset from
        /// the "unwriteable margin" (described below).
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		double GetEdgeTopAttribute();
		
		/// <summary>
        /// The edge measurements define the positioning of the headers
        /// and footers on the page. They're measured as an offset from
        /// the "unwriteable margin" (described below).
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetEdgeTopAttribute(double aEdgeTop);
		
		/// <summary>
        ///these are in inches </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		double GetEdgeLeftAttribute();
		
		/// <summary>
        ///these are in inches </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetEdgeLeftAttribute(double aEdgeLeft);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		double GetEdgeBottomAttribute();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetEdgeBottomAttribute(double aEdgeBottom);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		double GetEdgeRightAttribute();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetEdgeRightAttribute(double aEdgeRight);
		
		/// <summary>
        /// The margins define the positioning of the content on the page.
        /// They're treated as an offset from the "unwriteable margin"
        /// (described below).
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		double GetMarginTopAttribute();
		
		/// <summary>
        /// The margins define the positioning of the content on the page.
        /// They're treated as an offset from the "unwriteable margin"
        /// (described below).
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetMarginTopAttribute(double aMarginTop);
		
		/// <summary>
        ///these are in inches </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		double GetMarginLeftAttribute();
		
		/// <summary>
        ///these are in inches </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetMarginLeftAttribute(double aMarginLeft);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		double GetMarginBottomAttribute();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetMarginBottomAttribute(double aMarginBottom);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		double GetMarginRightAttribute();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetMarginRightAttribute(double aMarginRight);
		
		/// <summary>
        /// The unwriteable margin defines the printable region of the paper, creating
        /// an invisible border from which the edge and margin attributes are measured.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		double GetUnwriteableMarginTopAttribute();
		
		/// <summary>
        /// The unwriteable margin defines the printable region of the paper, creating
        /// an invisible border from which the edge and margin attributes are measured.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetUnwriteableMarginTopAttribute(double aUnwriteableMarginTop);
		
		/// <summary>
        ///these are in inches </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		double GetUnwriteableMarginLeftAttribute();
		
		/// <summary>
        ///these are in inches </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetUnwriteableMarginLeftAttribute(double aUnwriteableMarginLeft);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		double GetUnwriteableMarginBottomAttribute();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetUnwriteableMarginBottomAttribute(double aUnwriteableMarginBottom);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		double GetUnwriteableMarginRightAttribute();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetUnwriteableMarginRightAttribute(double aUnwriteableMarginRight);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		double GetScalingAttribute();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetScalingAttribute(double aScaling);
		
		/// <summary>
        ///values 0.0 - 1.0 </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool GetPrintBGColorsAttribute();
		
		/// <summary>
        ///values 0.0 - 1.0 </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetPrintBGColorsAttribute([MarshalAs(UnmanagedType.U1)] bool aPrintBGColors);
		
		/// <summary>
        ///Print Background Colors </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool GetPrintBGImagesAttribute();
		
		/// <summary>
        ///Print Background Colors </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetPrintBGImagesAttribute([MarshalAs(UnmanagedType.U1)] bool aPrintBGImages);
		
		/// <summary>
        ///Print Background Images </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		short GetPrintRangeAttribute();
		
		/// <summary>
        ///Print Background Images </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetPrintRangeAttribute(short aPrintRange);
		
		[return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.WStringMarshaler")]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		string GetTitleAttribute();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetTitleAttribute([MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.WStringMarshaler")] string aTitle);
		
		[return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.WStringMarshaler")]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		string GetDocURLAttribute();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetDocURLAttribute([MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.WStringMarshaler")] string aDocURL);
		
		[return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.WStringMarshaler")]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		string GetHeaderStrLeftAttribute();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetHeaderStrLeftAttribute([MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.WStringMarshaler")] string aHeaderStrLeft);
		
		[return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.WStringMarshaler")]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		string GetHeaderStrCenterAttribute();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetHeaderStrCenterAttribute([MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.WStringMarshaler")] string aHeaderStrCenter);
		
		[return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.WStringMarshaler")]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		string GetHeaderStrRightAttribute();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetHeaderStrRightAttribute([MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.WStringMarshaler")] string aHeaderStrRight);
		
		[return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.WStringMarshaler")]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		string GetFooterStrLeftAttribute();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetFooterStrLeftAttribute([MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.WStringMarshaler")] string aFooterStrLeft);
		
		[return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.WStringMarshaler")]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		string GetFooterStrCenterAttribute();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetFooterStrCenterAttribute([MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.WStringMarshaler")] string aFooterStrCenter);
		
		[return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.WStringMarshaler")]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		string GetFooterStrRightAttribute();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetFooterStrRightAttribute([MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.WStringMarshaler")] string aFooterStrRight);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		short GetHowToEnableFrameUIAttribute();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetHowToEnableFrameUIAttribute(short aHowToEnableFrameUI);
		
		/// <summary>
        ///indicates how to enable the frameset UI </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool GetIsCancelledAttribute();
		
		/// <summary>
        ///indicates how to enable the frameset UI </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetIsCancelledAttribute([MarshalAs(UnmanagedType.U1)] bool aIsCancelled);
		
		/// <summary>
        ///indicates whether the print job has been cancelled </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		short GetPrintFrameTypeUsageAttribute();
		
		/// <summary>
        ///indicates whether the print job has been cancelled </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetPrintFrameTypeUsageAttribute(short aPrintFrameTypeUsage);
		
		/// <summary>
        ///indicates whether to use the interal value or not </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		short GetPrintFrameTypeAttribute();
		
		/// <summary>
        ///indicates whether to use the interal value or not </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetPrintFrameTypeAttribute(short aPrintFrameType);
		
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool GetPrintSilentAttribute();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetPrintSilentAttribute([MarshalAs(UnmanagedType.U1)] bool aPrintSilent);
		
		/// <summary>
        ///print without putting up the dialog </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool GetShrinkToFitAttribute();
		
		/// <summary>
        ///print without putting up the dialog </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetShrinkToFitAttribute([MarshalAs(UnmanagedType.U1)] bool aShrinkToFit);
		
		/// <summary>
        ///shrinks content to fit on page </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool GetShowPrintProgressAttribute();
		
		/// <summary>
        ///shrinks content to fit on page </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetShowPrintProgressAttribute([MarshalAs(UnmanagedType.U1)] bool aShowPrintProgress);
		
		/// <summary>
        ///Additional XP Related </summary>
		[return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.WStringMarshaler")]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		string GetPaperNameAttribute();
		
		/// <summary>
        ///Additional XP Related </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetPaperNameAttribute([MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.WStringMarshaler")] string aPaperName);
		
		/// <summary>
        ///name of paper </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		short GetPaperSizeTypeAttribute();
		
		/// <summary>
        ///name of paper </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetPaperSizeTypeAttribute(short aPaperSizeType);
		
		/// <summary>
        ///use native data or is defined here </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		short GetPaperDataAttribute();
		
		/// <summary>
        ///use native data or is defined here </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetPaperDataAttribute(short aPaperData);
		
		/// <summary>
        ///native data value </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		double GetPaperWidthAttribute();
		
		/// <summary>
        ///native data value </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetPaperWidthAttribute(double aPaperWidth);
		
		/// <summary>
        ///width of the paper in inches or mm </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		double GetPaperHeightAttribute();
		
		/// <summary>
        ///width of the paper in inches or mm </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetPaperHeightAttribute(double aPaperHeight);
		
		/// <summary>
        ///height of the paper in inches or mm </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		short GetPaperSizeUnitAttribute();
		
		/// <summary>
        ///height of the paper in inches or mm </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetPaperSizeUnitAttribute(short aPaperSizeUnit);
		
		/// <summary>
        ///paper is in inches or mm </summary>
		[return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.WStringMarshaler")]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		string GetPlexNameAttribute();
		
		/// <summary>
        ///paper is in inches or mm </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetPlexNameAttribute([MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.WStringMarshaler")] string aPlexName);
		
		/// <summary>
        ///name of plex mode (like "simplex", "duplex",
        /// "tumble" and various custom values) </summary>
		[return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.WStringMarshaler")]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		string GetColorspaceAttribute();
		
		/// <summary>
        ///name of plex mode (like "simplex", "duplex",
        /// "tumble" and various custom values) </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetColorspaceAttribute([MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.WStringMarshaler")] string aColorspace);
		
		/// <summary>
        ///device-specific name of colorspace, overrides |printInColor| </summary>
		[return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.WStringMarshaler")]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		string GetResolutionNameAttribute();
		
		/// <summary>
        ///device-specific name of colorspace, overrides |printInColor| </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetResolutionNameAttribute([MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.WStringMarshaler")] string aResolutionName);
		
		/// <summary>
        ///device-specific identifer of resolution or quality
        /// (like "600", "600x300", "600x300x12", "high-res",
        /// "med-res". "low-res", etc.) </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool GetDownloadFontsAttribute();
		
		/// <summary>
        ///device-specific identifer of resolution or quality
        /// (like "600", "600x300", "600x300x12", "high-res",
        /// "med-res". "low-res", etc.) </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetDownloadFontsAttribute([MarshalAs(UnmanagedType.U1)] bool aDownloadFonts);
		
		/// <summary>
        ///enable font download to printer? </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool GetPrintReversedAttribute();
		
		/// <summary>
        ///enable font download to printer? </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetPrintReversedAttribute([MarshalAs(UnmanagedType.U1)] bool aPrintReversed);
		
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool GetPrintInColorAttribute();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetPrintInColorAttribute([MarshalAs(UnmanagedType.U1)] bool aPrintInColor);
		
		/// <summary>
        ///a false means grayscale </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		int GetOrientationAttribute();
		
		/// <summary>
        ///a false means grayscale </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetOrientationAttribute(int aOrientation);
		
		/// <summary>
        ///see orientation consts </summary>
		[return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.WStringMarshaler")]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		string GetPrintCommandAttribute();
		
		/// <summary>
        ///see orientation consts </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetPrintCommandAttribute([MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.WStringMarshaler")] string aPrintCommand);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		int GetNumCopiesAttribute();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetNumCopiesAttribute(int aNumCopies);
		
		[return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.WStringMarshaler")]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		string GetPrinterNameAttribute();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetPrinterNameAttribute([MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.WStringMarshaler")] string aPrinterName);
		
		/// <summary>
        ///name of destination printer </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool GetPrintToFileAttribute();
		
		/// <summary>
        ///name of destination printer </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetPrintToFileAttribute([MarshalAs(UnmanagedType.U1)] bool aPrintToFile);
		
		[return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.WStringMarshaler")]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		string GetToFileNameAttribute();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetToFileNameAttribute([MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.WStringMarshaler")] string aToFileName);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		short GetOutputFormatAttribute();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetOutputFormatAttribute(short aOutputFormat);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		int GetPrintPageDelayAttribute();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetPrintPageDelayAttribute(int aPrintPageDelay);
		
		/// <summary>
        /// This attribute tracks whether the PS has been initialized
        /// from a printer specified by the "printerName" attr.
        /// If a different name is set into the "printerName"
        /// attribute than the one it was initialized with the PS
        /// will then get intialized from that printer.
        /// </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool GetIsInitializedFromPrinterAttribute();
		
		/// <summary>
        /// This attribute tracks whether the PS has been initialized
        /// from a printer specified by the "printerName" attr.
        /// If a different name is set into the "printerName"
        /// attribute than the one it was initialized with the PS
        /// will then get intialized from that printer.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetIsInitializedFromPrinterAttribute([MarshalAs(UnmanagedType.U1)] bool aIsInitializedFromPrinter);
		
		/// <summary>
        /// This attribute tracks whether the PS has been initialized
        /// from prefs. If a different name is set into the "printerName"
        /// attribute than the one it was initialized with the PS
        /// will then get intialized from prefs again.
        /// </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool GetIsInitializedFromPrefsAttribute();
		
		/// <summary>
        /// This attribute tracks whether the PS has been initialized
        /// from prefs. If a different name is set into the "printerName"
        /// attribute than the one it was initialized with the PS
        /// will then get intialized from prefs again.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetIsInitializedFromPrefsAttribute([MarshalAs(UnmanagedType.U1)] bool aIsInitializedFromPrefs);
		
		/// <summary>
        ///C++ Helper Functions </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetMarginInTwips(System.IntPtr aMargin);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetEdgeInTwips(System.IntPtr aEdge);
		
		/// <summary>
        ///Purposely made this an "in" arg </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetMarginInTwips(System.IntPtr aMargin);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetEdgeInTwips(System.IntPtr aEdge);
		
		/// <summary>
        /// We call this function so that anything that requires a run of the event loop
        /// can do so safely. The print dialog runs the event loop but in silent printing
        /// that doesn't happen.
        ///
        /// Either this or ShowPrintDialog (but not both) MUST be called by the print engine
        /// before printing, otherwise printing can fail on some platforms.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetupSilentPrinting();
		
		/// <summary>
        /// Sets/Gets the "unwriteable margin" for the page format.  This defines
        /// the boundary from which we'll measure the EdgeInTwips and MarginInTwips
        /// attributes, to place the headers and content, respectively.
        ///
        /// Note: Implementations of SetUnwriteableMarginInTwips should handle
        /// negative margin values by falling back on the system default for
        /// that margin.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetUnwriteableMarginInTwips(System.IntPtr aEdge);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetUnwriteableMarginInTwips(System.IntPtr aEdge);
		
		/// <summary>
        /// Get more accurate print ranges from the superior interval
        /// (startPageRange, endPageRange). The aPages array is populated with a
        /// list of pairs (start, end), where the endpoints are included. The print
        /// ranges (start, end), must not overlap and must be in the
        /// (startPageRange, endPageRange) scope.
        ///
        /// If there are no print ranges the aPages array is cleared.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetPageRanges(System.IntPtr aPages);
	}
}
