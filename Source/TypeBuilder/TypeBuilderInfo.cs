using System;

using BLToolkit.Reflection;

namespace BLToolkit.TypeBuilder
{
	public class TypeBuilderInfo
	{
		public TypeBuilderInfo(Type type)
		{
			_originalType = _type = type;
		}

		private bool _inProgress = true;
		public  bool  InProgress
		{
			get { return _inProgress; }
		}

		private TypeHelper _type;
		public  TypeHelper  Type
		{
			get { return _type; }
		}

		internal void SetType(Type type)
		{
			_type = type;
		}

		private TypeHelper _originalType;
		public  TypeHelper  OriginalType
		{
			get { return _originalType; }
		}

		internal void FinalizeType()
		{
			_inProgress = false;
		}

		private ITypeBuilder[] _typeBuilders;
		public  ITypeBuilder[]  TypeBuilders
		{
			get { return _typeBuilders; }
		}

		internal void SetTypeBuilders(ITypeBuilder[] typeBuilders)
		{
			_typeBuilders = typeBuilders;
		}
	}
}
