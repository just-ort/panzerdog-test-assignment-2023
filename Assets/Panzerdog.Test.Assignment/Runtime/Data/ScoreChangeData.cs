using System;
using Panzerdog.Test.Assignment.Types;

namespace Panzerdog.Test.Assignment.Data
{
    public struct ScoreChangeData
    {
        public ChangeScoreReason Reason { get; set; }
        public int Value { get; set; }

        public ScoreChangeData(ScoreChangeData other)
        {
            Reason = other.Reason;
            Value = other.Value;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Reason, Value);
        }

        public override bool Equals(object obj)
        {
            if (obj is ScoreChangeData other)
            {
                return Value == other.Value && Reason == other.Reason;
            }
            
            return false;
        }

        public override string ToString()
        {
            return $"{nameof(Reason)}={Reason}, {nameof(Value)}={Value}";
        }
    }
}