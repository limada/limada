/*
 * Limaki 
 * Version 0.081
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2008 Lytico
 *
 * http://limada.sourceforge.net
 * 
 */


using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Limada.View;
using Limaki.Graphs;
using Limaki.Model.Streams;
using Limaki.Widgets;
using Limaki.Common;

namespace Limaki.Presenter.Winform.DragDrop {
    public class WidgetTextDataObjectHandler :
        IDataObjectHandler<IGraph<IWidget, IEdgeWidget>, IWidget>, IWidgetDataObjectHandler {

        public virtual string[] DataFormats {
            get {
                return new string[] {
                   System.Windows.Forms.DataFormats.Rtf,
                   System.Windows.Forms.DataFormats.Html
                };
            }
        }

        public Type HandledType {
            get { return typeof(IWidget); }
        }

        public void SetData(IDataObject data, IGraph<IWidget, IEdgeWidget> container, IWidget value) {

        }

        public IWidget GetData(IDataObject data, IGraph<IWidget, IEdgeWidget> container) {
            DataObject dataObject = data as DataObject;
            IWidget result = null;

            object description = null;
            StreamInfo<string> textInfo = new StreamInfo<string> ();
            textInfo.Compression = CompressionType.bZip2;

            if (dataObject != null) {
                // find out if this is a long string:

                if (dataObject.ContainsText(TextDataFormat.Text)) {
                    string plaintext = dataObject.GetText(TextDataFormat.Text);

                    // find lines
                    Regex rx = new Regex("\r\n|\n|\r|\n|\f");
                    MatchCollection matches = rx.Matches(plaintext);

                    // extract first line
                    if (matches.Count > 0) {
                        description = plaintext.Substring(0, matches[0].Index);
                    } else {
                        // if there is only one line, make a plain string-thing
                        IWidget widget = Registry.Pool.TryGetCreate<IWidgetFactory>()
                            .CreateItem(plaintext);
                        return widget;
                    }
                }

                if (dataObject.ContainsText(TextDataFormat.Rtf)) {
                    
                    textInfo.Data = dataObject.GetText(TextDataFormat.Rtf);
                    textInfo.StreamType = StreamTypes.RTF;

                } else if (dataObject.ContainsText(TextDataFormat.Html)) {

                    textInfo = HTMLPostProcess (dataObject.GetText (TextDataFormat.Html));
                    textInfo.StreamType = StreamTypes.HTML;

                }
            }

            if (textInfo.Data != null && textInfo.Data != string.Empty) {
                
                if (textInfo.Description == null)
                    textInfo.Description = description;

                StreamInfo<Stream> streamInfo = new StreamInfo<Stream> (textInfo);
                streamInfo.Data = new MemoryStream();
                StreamWriter writer = new StreamWriter(streamInfo.Data);
                writer.Write(textInfo.Data);
                writer.Flush();
                
                result = new WidgetThingStreamHelper().CreateFromStream(container, streamInfo);

                writer.Dispose();

            }
            return result;
        }

        StreamInfo<string> HTMLPostProcess(string text) {
            StreamInfo<string> result = new StreamInfo<string> ();
            result.Data = text;
            result.Description = null;
            result.Source = null;
            try {
                int startIndex = -1;
                int endIndex = -1;
                string subText = Between (text, "StartHTML:", "\r\n", 0);
                if (subText != null) int.TryParse (subText, out startIndex);
                subText = Between (text, "EndHTML:", "\r\n", 0);
                if (subText != null)
                    int.TryParse (subText, out endIndex);
                if (startIndex != -1 && endIndex != -1) {
                    result.Source = Between (text, "SourceURL:", "\r\n", 0);
                    result.Data = text.Substring (startIndex, endIndex - startIndex);
                }
            } catch (Exception e) {
                throw e;
            }
            return result;
        }

        string Between(string text, string start, string end, int startIndex) {
            int posStart = text.IndexOf (start, startIndex);
            if (posStart == -1) return null;
            posStart += start.Length;
            int posEnd = text.IndexOf (end,posStart);
            if (posEnd == -1) return null;
            return text.Substring (posStart,posEnd-posStart);
        }
    }
}


/*
function txbDragDropHandler.DropHTML(const dataObj: IDataObject;
  grfKeyState: Integer; pt: TPoint; var dwEffect: Integer): HRESULT;
var lItem:txbItem;
    PlainDone,RTFDone,PlainOnly:boolean;
    PlainText,RTFText,DescText,URLText,FileName:string;
    iStart,iEnd:integer;
    function PostProcess(const Source:string):string;
    var iLen,i:integer;
    begin
        result := Utf8ToAnsi(Source);
    end;
begin
     PlainDone := false;
     RTFDone := false;
     PlainOnly := false;
     URLText := emptyStr;
     with aFmtEtc do
     begin
       cfFormat := CF_HTML;
       ptd      := nil;
       dwAspect := DVASPECT_CONTENT;
       lindex   := -1;
       tymed    := TYMED_HGLOBAL;
     end;

     dwEffect := dropeffect_copy;
     result := dataObj.GetData(aFmtEtc, aStgMed);
     if result = S_OK then
     try
       pData := GlobalLock(aStgMed.hGlobal);
       PlainText := string(pData);
       PlainDone := true;
       iStart := 0; iEnd := 0;
       DescText := strBetween(PlainText,'StartHTML:',CrLf,1);
       if DescText <> emptyStr then
          iStart := strtoint(DescText)+1;

       DescText := strBetween(PlainText,'EndHTML:',CrLF,1);
       if DescText <> emptyStr then
          iEnd := strtoint(DescText);
       if (iStart <> 0) and (iEnd <> 0) then
       begin
          URLText := strBetween(PlainText,'SourceURL:',CrLF,1);
          PlainText := postProcess(copy (PlainText,iStart,iEnd));
       end
       else
         PlainText := emptyStr;
     finally
       GlobalUnlock(aStgMed.hGlobal);
       ReleaseStgMedium(aStgMed);
     end;
     result := S_FALSE;
     if PlainText <> emptyStr then
     begin
       lItem := txbStreamNode.createWithID;
       result := S_OK;
       with txbStreamNode(lItem) do
       begin
          Text := PlainText;
          StreamType := sTypHTML;
          compress;
          Area := self.Area;
          write;
          freeStream;
          DescText := emptyStr;
          if getDropText(DescText,dataObj,grfKeyState,pt,dwEffect)=S_OK then
             DescText := getDescText(DescText)
          else
             DescText := strBetween(PlainText,'<TITLE>','</TITLE>',1);

          if DescText <> emptyStr then
             Part[pidDescription] := DescText;

          if URLText <> emptyStr then
          begin
             FileName := strAfter('file://',URLText);
             if FileName <> emptyStr then
             begin
                FileName := URLFileName2FileName(FileName);
                // WorkAround dafür, daß vom internen Browser keine URL geliefert wird:
                if compareText(extractFileDir(FileName)+'\',getTempPath)=0 then
                begin
                   FileName := emptyStr;
                   URLText:=emptyStr;
                end;
             end;

             if FileName <> emptyStr then
                Part[pidSource] := FileName
             else if URLText <> emptyStr  then
                Part[pidSource] := URLText;
          end;

       end;
       if not isEmptyItem(lItem) then
       begin
          self.Item := lItem;
       end;
     end;



end;
*/