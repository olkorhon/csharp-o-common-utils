using System.ComponentModel;

namespace Olli.Common.Buffering
{
    public abstract class BaseBuffer<T> : IBuffer<T>, INotifyPropertyChanged
    {
        /// <summary>
        /// Represents change in buffer state, usually element read or buffer
        /// </summary>
        /// <returns></returns>
        public delegate void BufferStateChange(T elem);

        /// <inheritdoc />
        public event PropertyChangedEventHandler PropertyChanged;

        /// <inheritdoc />
        public abstract int BufferSize { get; }

        /// <inheritdoc />
        public abstract int BufferedElementCount { get; }

        /// <inheritdoc />
        public abstract int BufferLoadPercentile { get; }

        /// <inheritdoc />
        public abstract long TotalBufferedElementCount { get; }

        /// <inheritdoc />
        public abstract string Status { get; }

        /// <inheritdoc />
        public abstract void Buffer(T elem);

        /// <inheritdoc />
        public abstract T Read();

        /// <summary>
        /// Inform property watchers that a property has changed
        /// </summary>
        /// <param name="propertyName"></param>
        protected void InvokePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
