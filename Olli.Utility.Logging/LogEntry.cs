namespace Olli.Utility.Logging
{
    public class LogEntry
    {
        readonly LType type;
        readonly string msg;

        public LogEntry(LType type, string msg)
        {
            this.type = type;
            this.msg = msg ?? "";
        }

        public LType LogType { get { return type; } }
        public string Msg    { get { return msg; } }
    }
}
