using System;

namespace Olli.Common.Buffering
{
    public class MockBuffer : BaseBuffer<string>
    {
        public override int BufferSize => 1024;

        public override int BufferedElementCount => 512;

        public override int BufferLoadPercentile => 50;

        public override long TotalBufferedElementCount => 8315;

        public override string Status => "Buffer Size 1024 | Processed messages 8315";

        public override void Buffer(string elem)
        {
            throw new NotImplementedException();
        }

        public override string Read()
        {
            throw new NotImplementedException();
        }
    }
}
