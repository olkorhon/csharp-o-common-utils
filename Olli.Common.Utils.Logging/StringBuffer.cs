using System.ComponentModel;

namespace Ponsse.Olli.CommieTester.Utility
{
    public class StringBuffer : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private object bufferLock = new object();

        private string[] buffer;
        private int bufferWritePos = 0;
        private int bufferReadPos = 0;

        private int bufferLoadMessageCount = 0;
        private int bufferLoadPercentile = 0;
        private int totalBytesBuffered = 0;

        public StringBuffer(int bufferSize)
        {
            buffer = new string[bufferSize];

            BufferSize = bufferSize;
        }

        /// <summary>
        /// Maximum amount of buffered items
        /// </summary>
        public int BufferSize { get; }

        /// <summary>
        /// Count of messages currently waiting to be read
        /// </summary>
        public int BufferLoadMessageCount
        {
            get { return bufferLoadMessageCount; }
            set
            {
                bufferLoadMessageCount = value;
                InvokePropertyChanged(nameof(bufferLoadMessageCount));
            }
        }

        /// <summary>
        /// Integer between 0 and 100, represents how full the buffer is
        /// </summary>
        public int BufferLoadPercentile
        {
            get { return bufferLoadPercentile; }
            set {
                bufferLoadPercentile = value;
                InvokePropertyChanged(nameof(BufferLoadPercentile));
            }
        }

        /// <summary>
        /// Cumulative sum of bytes that have gone through this buffer
        /// </summary>
        public int TotalBytesBuffered
        {
            get { return totalBytesBuffered; }
            set
            {
                totalBytesBuffered = value;
                InvokePropertyChanged(nameof(TotalBytesBuffered));
            }
        }

        /// <summary>
        /// Position in the buffer where the next value should be written to
        /// </summary>
        public int BufferWritePos
        {
            get { return bufferWritePos; }
            set
            {
                bufferWritePos = value;
                InvokePropertyChanged(nameof(BufferWritePos));
            }
        }

        /// <summary>
        /// Position in the buffer where the next value should be read from
        /// </summary>
        public int BufferReadPos
        {
            get { return bufferReadPos; }
            set
            {
                bufferReadPos = value;
                InvokePropertyChanged(nameof(BufferReadPos));
            }
        }

        /// <summary>
        /// Reads a single message from the buffer, moves read position
        /// </summary>
        /// <returns></returns>
        public string GetBufferedMsg()
        {
            string msgToSend = "";
            lock (bufferLock)
            {
                if (bufferReadPos != bufferWritePos)
                {
                    msgToSend = buffer[bufferReadPos];
                    BufferReadPos = (bufferReadPos + 1) % BufferSize;

                    UpdateBufferLoad();
                }
            }

            return msgToSend;
        }

        /// <summary>
        /// Appends a single message to the buffer, moves write position
        /// </summary>
        /// <param name="msg"></param>
        public void BufferMsg(string msg)
        {
            lock (bufferLock)
            {
                buffer[bufferWritePos] = msg;
                BufferWritePos = (bufferWritePos + 1) % BufferSize;

                // Buffer overflow, push read pointer forward
                if (bufferWritePos == bufferReadPos)
                    BufferReadPos = (bufferReadPos + 1) % BufferSize;

                TotalBytesBuffered += msg.Length;
                UpdateBufferLoad();
            }
        }
    
        /// <summary>
        /// Recalculates buffer load based on buffer write and read positions
        /// </summary>
        private void UpdateBufferLoad()
        {
            if (bufferWritePos == bufferReadPos)
            {
                BufferLoadMessageCount = 0;
                BufferLoadPercentile = 0;
            }
            else if (bufferWritePos > bufferReadPos)
            {
                BufferLoadMessageCount = bufferWritePos - bufferReadPos;
                float normalizeBufferLoad = (float)(BufferLoadMessageCount + 1) / BufferSize;
                BufferLoadPercentile = (int)(normalizeBufferLoad * 100.0f);
            }
            else
            {
                BufferLoadMessageCount = BufferSize - bufferReadPos + bufferWritePos;
                float normalizedBufferLoad = (float)(BufferLoadMessageCount + 1) / BufferSize;
                BufferLoadPercentile = (int)(normalizedBufferLoad * 100.0f);
            }
        }

        /// <summary>
        /// Inform property watchers that a property has changed
        /// </summary>
        /// <param name="propertyName"></param>
        private void InvokePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
