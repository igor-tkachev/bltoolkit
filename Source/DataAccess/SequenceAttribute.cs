using System;

namespace BLToolkit.DataAccess
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class SequenceAttribute : Attribute
    {
        private readonly string _sequence;

        public SequenceAttribute(string sequence)
        {
            _sequence = sequence;
        }

        public string Sequence
        {
            get { return _sequence; }
        }
    }
}