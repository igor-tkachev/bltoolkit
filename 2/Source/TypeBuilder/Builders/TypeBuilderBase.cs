using System;

namespace BLToolkit.TypeBuilder.Builders
{
	public abstract class TypeBuilderBase : ITypeBuilder
	{
		private BuildContext _context;
		public  BuildContext  Context
		{
			get { return _context;  }
			set { _context = value; }
		}

		public virtual bool IsCompatible(BuildContext context, ITypeBuilder typeBuilder)
		{
			return true;
		}

		protected bool IsRelative(ITypeBuilder typeBuilder)
		{
			return GetType().IsInstanceOfType(typeBuilder) || typeBuilder.GetType().IsInstanceOfType(this);
		}
	}
}
