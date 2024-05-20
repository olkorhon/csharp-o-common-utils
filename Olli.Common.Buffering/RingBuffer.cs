namespace Olli.Common.Buffering
{
    public class RingBuffer<T> : BaseBuffer<T>
    {
        /// <summary>
        /// Triggers when a new element has been buffered
        /// </summary>
        public event BufferStateChange ElemBuffered;

        /// <summary>
        /// Triggers when an element has been read from the buffer
        /// </summary>
        public event BufferStateChange ElemRead;

        private object bufferLock = new object();

        private T[] buffer;
        private int writePos = 0;
        private int readPos = 0;

        private int bufferedElementCount = 0;
        private int bufferLoadPercentile = 0;

        private long totalElemsBuffered = 0;

        public RingBuffer(int bufferSize)
        {
            buffer = new T[bufferSize];
            writePos = 0;
            readPos = 0;

            bufferedElementCount = 0;
            bufferLoadPercentile = 0;

            totalElemsBuffered = 0;

            BufferSize = bufferSize;
            InvokePropertyChanged(nameof(Status));
        }

        /// <inheritdoc />
        public override int BufferSize { get; }

        /// <inheritdoc />
        public override int BufferedElementCount
        {
            get { return bufferedElementCount; }
        }

        /// <inheritdoc />
        public override int BufferLoadPercentile
        {
            get { return bufferLoadPercentile; }
        }

        /// <inheritdoc />
        public override long TotalBufferedElementCount
        {
            get { return totalElemsBuffered; }
        }

        public override string Status {
            get { return $"Buffer Size {BufferSize} | Read:Write - {ReadPos}:{WritePos} | Processed messages {TotalBufferedElementCount}"; }
        }

        /// <summary>
        /// Position in the buffer where the next value should be written to
        /// </summary>
        public int WritePos
        {
            get { return writePos; }
            private set
            {
                writePos = value;
                InvokePropertyChanged(nameof(WritePos));
            }
        }

        /// <summary>
        /// Position in the buffer where the next value should be read from
        /// </summary>
        public int ReadPos
        {
            get { return readPos; }
            private set
            {
                readPos = value;
                InvokePropertyChanged(nameof(ReadPos));
            }
        }

        /// <inheritdoc />
        public override T Read()
        {
            T bufferedElem = default;
            lock (bufferLock)
            {
                if (readPos != writePos)
                {
                    bufferedElem = buffer[readPos];
                    ReadPos = (readPos + 1) % BufferSize;

                    UpdateBufferLoad();
                }
            }

            if (bufferedElem != null)
                ElemRead?.Invoke(bufferedElem);

            return bufferedElem;
        }

        /// <inheritdoc />
        public override void Buffer(T elem)
        {
            lock (bufferLock)
            {
                buffer[writePos] = elem;
                WritePos = (writePos + 1) % BufferSize;

                // Buffer overflow, push read pointer forward
                if (writePos == readPos)
                    ReadPos = (readPos + 1) % BufferSize;

                totalElemsBuffered++;
                InvokePropertyChanged(nameof(TotalBufferedElementCount));
                InvokePropertyChanged(nameof(Status));
                UpdateBufferLoad();
                ElemBuffered?.Invoke(elem);
            }
        }

        /// <summary>
        /// Recalculates buffer load based on buffer write and read positions
        /// </summary>
        private void UpdateBufferLoad()
        {
            if (writePos == readPos)
            {
                bufferedElementCount = 0;
                bufferLoadPercentile = 0;

                InvokePropertyChanged(nameof(BufferedElementCount));
                InvokePropertyChanged(nameof(BufferLoadPercentile));
                return;
            }

            bufferedElementCount = writePos > readPos
                ?              writePos - readPos  // Write pointer ahead of read pointer, normal case
                : BufferSize + writePos - readPos; // Write pointer before read pointer, wrap around the end of buffer

            float normalizedBufferLoad = (float)(BufferedElementCount + 1) / BufferSize;
            bufferLoadPercentile = (int)(normalizedBufferLoad * 100.0f);

            InvokePropertyChanged(nameof(BufferedElementCount));
            InvokePropertyChanged(nameof(BufferLoadPercentile));
        }
    }
}
