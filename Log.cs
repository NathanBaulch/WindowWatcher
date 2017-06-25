using System;

namespace WindowWatcher
{
    public class Log
    {
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public string Process { get; set; }
        public string Caption { get; set; }
        public int? Idle { get; set; }
        public int? Active { get; set; }
        public string Category { get; set; }
        public double Certainty { get; set; }

        public int Total => (Idle ?? 0) + (Active ?? 0);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == typeof(Log) && Equals((Log) obj);
        }

        public bool Equals(Log obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.Start.Equals(Start) && obj.End.Equals(End) && Equals(obj.Process, Process) && Equals(obj.Caption, Caption);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var result = (Start.HasValue ? Start.Value.GetHashCode() : 0);
                result = (result * 397) ^ (End.HasValue ? End.Value.GetHashCode() : 0);
                result = (result * 397) ^ (Process != null ? Process.GetHashCode() : 0);
                result = (result * 397) ^ (Caption != null ? Caption.GetHashCode() : 0);
                return result;
            }
        }
    }
}