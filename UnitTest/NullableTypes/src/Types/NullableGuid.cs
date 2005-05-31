//
// NullableTypes.?
// 
// Authors: Luca Minudel (lukadotnet@users.sourceforge.net)
//          ...
//
// Date         Author  Changes    Reasons
// 07-Apr-2003  Luca    Created    Declared public members
// ??-Apr-2003  ...     ...
//

namespace NullableTypes {
    using sys = System;

	public struct NullableGuid : INullable, sys.IComparable
	{
        #region Fields
        sys.Guid _value;
        private bool _notNull;

        public static readonly NullableGuid Null;
        #endregion // Fields

        #region Constructors
        public NullableGuid (byte[] value) {
            _value = new sys.Guid (value);
            _notNull = true;
        }

        public NullableGuid (sys.Guid g) {
            _value = g;
            _notNull = true;
        }

        public NullableGuid (string s) {
            _value = new sys.Guid (s);
            _notNull = true;
        }

        public NullableGuid (int a, short b, short c, byte d, byte e, byte f, byte g, byte h, byte i, byte j, byte k) {
            _value = new sys.Guid (a, b, c, d, e, f, g, h, i, j, k);
            _notNull = true;
        }
        #endregion // Constructors

        #region INullable

        public bool IsNull {
            get { return !_notNull; }
        }
        #endregion // INullable

        #region IComparable - Ordering
        public int CompareTo(object value) {
            if (value == null)
                return 1;
            else if (!(value is NullableGuid))
                throw new sys.ArgumentException(string.Format(Locale.GetText("Value is not a {0}."), "NullableTypes.NullableGuid"));
            else if (((NullableGuid)value).IsNull)
                return 1;
            else
                return _value.CompareTo(((NullableGuid)value).Value);
        }

        #endregion // IComparable - Ordering

        #region Equivalence
        public override bool Equals(object value) {
            if (!(value is NullableGuid))
                return false;
            else if (this.IsNull && ((NullableGuid)value).IsNull)
                return true;
            else if (((NullableGuid)value).IsNull)
                return false;
            else
                return (bool) (this == (NullableGuid)value);
        }

        public static NullableBoolean NotEquals(NullableGuid x, NullableGuid y) {
            return (x != y);
        }


        public static NullableBoolean operator == (NullableGuid x, NullableGuid y) {
            if (x.IsNull || y.IsNull) return NullableBoolean.Null;
            return new NullableBoolean (x.Value == y.Value);
        }

        public static NullableBoolean operator != (NullableGuid x, NullableGuid y) {
            if (x.IsNull || y.IsNull) return NullableBoolean.Null;
            return new NullableBoolean (!(x.Value == y.Value));
        }

        public static NullableBoolean Equals(NullableGuid x, NullableGuid y) {
            return (x == y);
        }

        public override int GetHashCode() {
            byte [] bytes  = this.ToByteArray ();
			
            int result = 10;
            foreach (byte b in  bytes) {
                result = 91 * result + b.GetHashCode ();
            }

            return result;
        }

        #endregion // Equivalence

        #region Properties
        public sys.Guid Value { 
            get { 
                if (this.IsNull) 
                    throw new NullableNullValueException();

                return _value; 
            }
        }
        #endregion // Properties

        #region Methods
        public static NullableBoolean GreaterThan(NullableGuid x, NullableGuid y) {
            return (x > y);
        }

        public static NullableBoolean GreaterThanOrEqual(NullableGuid x, NullableGuid y) {
            return (x >= y);
        }

        public static NullableBoolean LessThan(NullableGuid x, NullableGuid y) {
            return (x < y);
        }

        public static NullableBoolean LessThanOrEqual(NullableGuid x, NullableGuid y) {
            return (x <= y);
        }

        public static NullableGuid Parse(string s) {
            return new NullableGuid (s);
        }
        #endregion // Methods

        #region Operators
        public static NullableBoolean operator > (NullableGuid x, NullableGuid y) {
            if (x.IsNull || y.IsNull)
                return NullableBoolean.Null;

            if (x.Value.CompareTo (y.Value) > 0)
                return new NullableBoolean (true);
            else
                return new NullableBoolean (false);
        }

        public static NullableBoolean operator >= (NullableGuid x, NullableGuid y) {
            if (x.IsNull || y.IsNull)
                return NullableBoolean.Null;
			
            if (x.Value.CompareTo (y.Value) >= 0)
                return new NullableBoolean (true);
            else
                return new NullableBoolean (false);

        }

        public static NullableBoolean operator < (NullableGuid x, NullableGuid y) {
            if (x.IsNull || y.IsNull)
                return NullableBoolean.Null;

            if (x.Value.CompareTo (y.Value) < 0)
                return new NullableBoolean (true);
            else
                return new NullableBoolean (false);

        }

        public static NullableBoolean operator <= (NullableGuid x, NullableGuid y) {
            if (x.IsNull || y.IsNull)
                return NullableBoolean.Null;

            if (x.Value.CompareTo (y.Value) <= 0)
                return new NullableBoolean (true);
            else
                return new NullableBoolean (false);
        }
        #endregion // Operators

        #region Conversion Operators
        public static explicit operator NullableGuid(NullableBinary x) {
            return new NullableGuid (x.Value);
        }

        public static explicit operator sys.Guid(NullableGuid x) {
            return x.Value;
        }

        public static explicit operator NullableGuid(NullableString x) {
            return new NullableGuid (x.Value);
        }

        public static implicit operator NullableGuid(sys.Guid x) {
            return new NullableGuid (x);
        }

        #endregion // Conversion Operators

        #region Conversions

        public byte[] ToByteArray() {
            return _value.ToByteArray ();
        }

        public NullableBinary ToNullableBinary () {
            return ((NullableBinary)this);
        }

        public NullableString ToNullableString () {
            return ((NullableString)this);
        }

        public override string ToString () {
            if (this.IsNull)
                return string.Empty;
            else
                return _value.ToString ();
        }
        #endregion // Conversions

	}
}
			
