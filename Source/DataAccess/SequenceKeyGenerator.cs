namespace BLToolkit.DataAccess
{
    public class SequenceKeyGenerator : KeyGenerator
    {
        private readonly string _sequence;

        public SequenceKeyGenerator(string sequence, bool retrievePkValue)
            : base(retrievePkValue)
        {
            _sequence = sequence;
        }

        public string Sequence
        {
            get { return _sequence; }
        }
    }

    public abstract class KeyGenerator
    {
        private readonly bool _retrievePkValue;

        protected KeyGenerator(bool retrievePkValue)
        {
            _retrievePkValue = retrievePkValue;
        }

        public bool RetrievePkValue
        {
            get { return _retrievePkValue; }
        }
    }
}