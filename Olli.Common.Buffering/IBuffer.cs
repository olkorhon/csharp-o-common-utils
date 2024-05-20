namespace Olli.Common.Buffering
{
    public interface IBuffer<T>
    {
        /// <summary>
        /// Maximum amount of buffered items
        /// </summary>
        int BufferSize { get; }

        /// <summary>
        /// Count of elements currently waiting to be read
        /// </summary>
        int BufferedElementCount { get; }

        /// <summary>
        /// Integer between 0 and 100, represents how full the buffer is
        /// </summary>
        int BufferLoadPercentile { get; }

        /// <summary>
        /// Cumulative sum of bytes that have gone through this buffer
        /// </summary>
        long TotalBufferedElementCount { get; }

        /// <summary>
        /// Current state of buffer in text format, should fit in a single line of text
        /// </summary>
        /// <returns></returns>
        string Status { get; }

        /// <summary>
        /// Reads a single element from the buffer, moves read position
        /// </summary>
        /// <returns></returns>
        T Read();

        /// <summary>
        /// Appends a single element to the buffer, moves write position
        /// </summary>
        /// <param name="elem"></param>
        void Buffer(T elem);
    }
}
