using System;

namespace BLToolkit.DataAccess
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class KeyGeneratorAttribute : Attribute
    {
        private readonly KeyGeneratorInfo _generatorInfo;

        public KeyGeneratorAttribute(PrimaryKeyGeneratorType generator, bool retrievePkValue)
        {
            _generatorInfo = new KeyGeneratorInfo(generator, retrievePkValue);
        }

        public KeyGeneratorAttribute(PrimaryKeyGeneratorType generator) : this(generator, true)
        {
        }

        public KeyGeneratorInfo GeneratorInfo
        {
            get { return _generatorInfo; }
        }
    }

    public class KeyGeneratorInfo
    {
        private readonly PrimaryKeyGeneratorType _generator;
        private readonly bool _retrievePkValue;

        public KeyGeneratorInfo(PrimaryKeyGeneratorType generator, bool retrievePkValue)
        {
            _generator = generator;
            _retrievePkValue = retrievePkValue;
        }

        public bool RetrievePkValue
        {
            get { return _retrievePkValue; }
        }

        public PrimaryKeyGeneratorType GeneratorType
        {
            get { return _generator; }
        }
    }
}