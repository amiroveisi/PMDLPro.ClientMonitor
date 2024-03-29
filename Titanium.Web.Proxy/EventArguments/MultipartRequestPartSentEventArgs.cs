using System;
using PSYCOPMDLProCoreCommunications.Web.Proxy.Http;

namespace PSYCOPMDLProCoreCommunications.Web.Proxy.EventArguments
{
    /// <summary>
    ///     Class that wraps the multipart sent request arguments.
    /// </summary>
    public class MultipartRequestPartSentEventArgs : EventArgs
    {
        internal MultipartRequestPartSentEventArgs(string boundary, HeaderCollection headers)
        {
            Boundary = boundary;
            Headers = headers;
        }

        /// <summary>
        ///     Boundary.
        /// </summary>
        public string Boundary { get; }

        /// <summary>
        ///     The header collection.
        /// </summary>
        public HeaderCollection Headers { get; }
    }
}
