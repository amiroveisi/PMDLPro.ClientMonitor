﻿using StreamExtended.Network;
using System;
using System.Threading;
using System.Threading.Tasks;
using PSYCOPMDLProCoreCommunications.Web.Proxy.EventArguments;
using PSYCOPMDLProCoreCommunications.Web.Proxy.Exceptions;
using PSYCOPMDLProCoreCommunications.Web.Proxy.Helpers;
using PSYCOPMDLProCoreCommunications.Web.Proxy.Http;
using PSYCOPMDLProCoreCommunications.Web.Proxy.Network.Tcp;

namespace PSYCOPMDLProCoreCommunications.Web.Proxy
{
    public partial class ProxyServer
    {

        /// <summary>
        ///     Handle upgrade to websocket
        /// </summary>
        private async Task handleWebSocketUpgrade(string httpCmd,
            SessionEventArgs args, Request request, Response response,
            CustomBufferedStream clientStream, HttpResponseWriter clientStreamWriter,
            TcpServerConnection serverConnection,
            CancellationTokenSource cancellationTokenSource, CancellationToken cancellationToken)
        {
            // prepare the prefix content
            await serverConnection.StreamWriter.WriteLineAsync(httpCmd, cancellationToken);
            await serverConnection.StreamWriter.WriteHeadersAsync(request.Headers,
                cancellationToken: cancellationToken);

            string httpStatus;
            try
            {
                httpStatus = await serverConnection.Stream.ReadLineAsync(cancellationToken);
                if (httpStatus == null)
                {
                    throw new ServerConnectionException("Server connection was closed.");
                }
            }
            catch (Exception e) when (!(e is ServerConnectionException))
            {
                throw new ServerConnectionException("Server connection was closed.", e);
            }

            Response.ParseResponseLine(httpStatus, out var responseVersion,
                out int responseStatusCode,
                out string responseStatusDescription);
            response.HttpVersion = responseVersion;
            response.StatusCode = responseStatusCode;
            response.StatusDescription = responseStatusDescription;

            await HeaderParser.ReadHeaders(serverConnection.Stream, response.Headers,
                cancellationToken);

            if (!args.IsTransparent)
            {
                await clientStreamWriter.WriteResponseAsync(response,
                    cancellationToken: cancellationToken);
            }

            // If user requested call back then do it
            if (!args.HttpClient.Response.Locked)
            {
                await invokeBeforeResponse(args);
            }

            await TcpHelper.SendRaw(clientStream, serverConnection.Stream, BufferPool, BufferSize,
                (buffer, offset, count) => { args.OnDataSent(buffer, offset, count); },
                (buffer, offset, count) => { args.OnDataReceived(buffer, offset, count); },
                cancellationTokenSource, ExceptionFunc);
        }
    }
}
