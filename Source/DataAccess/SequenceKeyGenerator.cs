namespace BLToolkit.DataAccess
{
    /// <summary>
    /// Wrapper class used to get the SequenceName attribute value and add it to the ObjectMapper class.
    /// USeful for the SqlQuery InsertWithIdentity method
    /// </summary>
    public class SequenceKeyGenerator : KeyGenerator
    {
        private readonly string _sequence;

        public SequenceKeyGenerator(string sequence)
        {
            _sequence = sequence;
        }

        public string Sequence
        {
            get { return _sequence; }
        }
    }

    public class KeyGenerator
    {
    }
}

