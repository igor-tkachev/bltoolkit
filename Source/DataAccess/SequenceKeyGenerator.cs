using System;

namespace BLToolkit.DataAccess
{
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
