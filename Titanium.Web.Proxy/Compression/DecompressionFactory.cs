using System;
using System.IO;
using System.IO.Compression;
using PSYCOPMDLProCoreCommunications.Web.Proxy.Helpers;
using PSYCOPMDLProCoreCommunications.Web.Proxy.Http;

namespace PSYCOPMDLProCoreCommunications.Web.Proxy.Compression
{
    /// <summary>
    ///     A factory to generate the de-compression methods based on the type of compression
    /// </summary>
    internal class DecompressionFactory
    {
        internal static Stream Create(string type, Stream stream, bool leaveOpen = true)
        {
            switch (type)
            {
                case KnownHeaders.ContentEncodingGzip:
                    return new GZipStream(stream, CompressionMode.Decompress, leaveOpen);
                case KnownHeaders.ContentEncodingDeflate:
                    return new DeflateStream(stream, CompressionMode.Decompress, leaveOpen);
                case KnownHeaders.ContentEncodingBrotli:
                    return new BrotliSharpLib.BrotliStream(stream, CompressionMode.Decompress, leaveOpen);
                default:
                    //throw new Exception($"Unsupported decompression mode: {type}");
                    return stream;
            }
        }
    }
}
