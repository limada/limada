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

using System.Collections.Generic;
using System.IO;
using System.Net;
using Limaki.Common.Collections;

namespace Limaki.UseCases.Viewers.StreamViewers.WebProxy {
    public class WebProxyContent : WebContent {
        
        public static ICollection<string> FailedConnections = new Set<string>();
        
        public override ResponseInfo Respond(RequestInfo requestInfo) {
            var result = new ResponseInfo();
            result.Success = false;
            if (requestInfo.Uri == null)
                return result;
            
            lock (FailedConnections) {
                if (FailedConnections.Contains(requestInfo.Uri.AbsoluteUri)) {
                    return result;
                }
            }

            var webRequest = WebRequest.CreateDefault(requestInfo.Uri);
            WebResponse webResponse = null;
            try {
                webResponse = webRequest.GetResponse();

                var contentLength = webResponse.ContentLength;
                Stream ReceiveStream = webResponse.GetResponseStream();

                if (contentLength > 0) {

                    int pos = 0;

                    result.Data = new byte[contentLength];

                    int bytesRead = 1;
                    while (pos < contentLength && bytesRead != 0) {
                        bytesRead = ReceiveStream.Read(result.Data, pos, (int)contentLength - pos);
                        pos += bytesRead;
                    }


                    if (pos == contentLength)
                        result.Success = true;
                    else
                        result.Success = false;

                } else {
                    var stream = new MemoryStream();
                    var bytesReceived = 1;
                    var totalReceived = 0;
                    var buff = new byte[1024];
                    while (bytesReceived != 0) {
                        bytesReceived = ReceiveStream.Read(buff, 0, buff.Length);
                        totalReceived += bytesReceived;
                        stream.Write(buff, 0, bytesReceived);
                    }
                    result.Data = stream.GetBuffer();
                    stream.Close ();
                    result.Success = true;
                }
                ReceiveStream.Close();

                result.MimeType = webResponse.ContentType;

            } catch (WebException we) {
                result.Success = false;
                lock (FailedConnections) {
                    FailedConnections.Add(requestInfo.Uri.AbsoluteUri);
                }
            } finally {
                if (webResponse != null) {
                    webResponse.Close();
                }
            }
            return result;
        }


    }
}