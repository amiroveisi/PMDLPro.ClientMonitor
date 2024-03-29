﻿namespace PSYCOPMDLProCoreCommunications.Web.Proxy.Http2
{
    internal class Http2FrameHeader
    {
        public int Length;

        public Http2FrameType Type;

        public Http2FrameFlag Flags;

        public int StreamId;

        public byte[] Buffer;

        public byte[] CopyToBuffer()
        {
            int length = Length;
            var buf = /*new byte[9];*/Buffer;
            buf[0] = (byte)((length >> 16) & 0xff);
            buf[1] = (byte)((length >> 8) & 0xff);
            buf[2] = (byte)(length & 0xff);
            buf[3] = (byte)Type;
            buf[4] = (byte)Flags;
            int streamId = StreamId;
            //buf[5] = (byte)((streamId >> 24) & 0xff);
            //buf[6] = (byte)((streamId >> 16) & 0xff);
            //buf[7] = (byte)((streamId >> 8) & 0xff);
            //buf[8] = (byte)(streamId & 0xff);
            return buf;
        }
    }
}
