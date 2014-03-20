/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2011 Lytico
 *
 * http://www.limada.org
 * 
 */


using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Limaki.Contents;
using Limaki.Graphs;
using Limada.View;
using Limada.VisualThings;
using Limaki.Common;
using Limaki.Visuals;

namespace Limaki.View.Swf.DragDrop {
    public class VisualsTextDataObjectHandler :
        IDataObjectHandler<IGraph<IVisual, IVisualEdge>, IVisual>, IVisualsDataObjectHandler {

        public virtual string[] DataFormats {
            get {
                return new string[] {
                   System.Windows.Forms.DataFormats.Rtf,
                   System.Windows.Forms.DataFormats.Html
                };
            }
        }

        public Type HandledType {
            get { return typeof(IVisual); }
        }

        public void SetData(IDataObject data, IGraph<IVisual, IVisualEdge> container, IVisual value) {

        }

        public IVisual GetData(IDataObject data, IGraph<IVisual, IVisualEdge> container) {
            DataObject dataObject = data as DataObject;
            IVisual result = null;

            object description = null;
            Content<string> textInfo = new Content<string> ();
            textInfo.Compression = CompressionType.bZip2;
            var encoding = System.Text.Encoding.Default;

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
                        IVisual visual = Registry.Pooled<IVisualFactory>()
                            .CreateItem(plaintext);
                        return visual;
                    }
                }

                if (dataObject.ContainsText(TextDataFormat.Rtf)) {
                    
                    textInfo.Data = dataObject.GetText(TextDataFormat.Rtf);
                    textInfo.ContentType = ContentTypes.RTF;

                } else if (dataObject.ContainsText(TextDataFormat.Html)) {
                    string s = null;
                    var r = DataObjectHelper.GetData((IDataObject)dataObject, System.Windows.Forms.DataFormats.Html, -1);//dataObject.GetText(TextDataFormat.Html);
                    if (r!=null)
                        s = System.Text.Encoding.UTF8.GetString(r); 
                    textInfo = HTMLPostProcess(s);
                    textInfo.ContentType = ContentTypes.HTML;
                    textInfo.Compression = CompressionType.bZip2;

                    if (false) {
                        var format = "HTML Format"; //"text/html";//
                        if (dataObject.GetDataPresent(format)) {
                            encoding = System.Text.Encoding.UTF8;
                            s = GetString(dataObject, format, encoding);
                            textInfo.Data = s;
                        }
                    }
                }
            }

            if (textInfo.Data != null && textInfo.Data != string.Empty) {
                
                if (textInfo.Description == null)
                    textInfo.Description = description;

                var content = new Content<Stream> (textInfo);
                content.Data = new MemoryStream();
                StreamWriter writer = new StreamWriter(content.Data, encoding);
                writer.Write(textInfo.Data);
                writer.Flush();
                
                result = new VisualThingsContentViz().VisualOfContent(container, content);

                writer.Dispose();

            }
            return result;
        }

        string GetString(IDataObject dataObject, string format, System.Text.Encoding encoding) {
            string s = null;
            var dataresult = dataObject.GetData(format);
            var stream = dataresult as Stream;
            if (stream != null)
                using (var reader = new StreamReader(stream, encoding))
                    s = reader.ReadToEnd();
            if (dataresult is string)
                s = dataresult as string;
            return s;

        }

        Content<string> HTMLPostProcess(string text) {
            Content<string> result = new Content<string> ();
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
                    endIndex = Math.Min(text.Length, endIndex);
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

namespace Limaki.View.Swf.DragDrop.Interop {
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
          ContentType := sTypHTML;
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



