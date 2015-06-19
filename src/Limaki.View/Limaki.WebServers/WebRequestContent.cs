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

using System.Collections.Generic;
using System.IO;
using System.Net;
using Limaki.Common.Collections;
using System;

namespace Limaki.WebServers {

    /// <summary>
    /// gives back a respond using a System.Net.WebRequest 
    /// </summary>
    public class WebRequestContent : WebContent {
        
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
            System.Net.WebResponse webResponse = null;
            try {
                webResponse = webRequest.GetResponse();

                var contentLength = webResponse.ContentLength;
                var responseStream = webResponse.GetResponseStream();

                if (contentLength > 0) {

                    int pos = 0;

                    result.Data = new byte[contentLength];

                    int bytesRead = 1;
                    while (pos < contentLength && bytesRead != 0) {
                        bytesRead = responseStream.Read(result.Data, pos, (int)contentLength - pos);
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
                        bytesReceived = responseStream.Read(buff, 0, buff.Length);
                        totalReceived += bytesReceived;
                        stream.Write(buff, 0, bytesReceived);
                    }
                    result.Data = new byte[stream.Length];
                    // GetBuffer gives back the whole buffer, that is more than stream.Lenght
                    Array.Copy(stream.GetBuffer(), 0, result.Data, 0, stream.Length);
                    stream.Close ();
                    result.Success = true;
                }
                responseStream.Close();

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