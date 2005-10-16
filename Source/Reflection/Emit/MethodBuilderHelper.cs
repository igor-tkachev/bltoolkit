using System;
using System.Reflection.Emit;
using System.Reflection;

namespace BLToolkit.Reflection.Emit
{
	/// <summary>
	/// A wrapper around the <see cref="MethodBuilder"/> class.
	/// </summary>
	/// <include file="Examples.CS.xml" path='examples/emit[@name="Emit"]/*' />
	/// <include file="Examples.VB.xml" path='examples/emit[@name="Emit"]/*' />
	/// <seealso cref="System.Reflection.Emit.MethodBuilder">MethodBuilder Class</seealso>
	public class MethodBuilderHelper : MethodBuilderBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="MethodBuilderHelper"/> class
		/// with the specified parameters.
		/// </summary>
		/// <param name="typeBuilder">Associated <see cref="TypeBuilderHelper"/>.</param>
		/// <param name="methodBuilder">A <see cref="MethodBuilder"/></param>
		public MethodBuilderHelper(TypeBuilderHelper typeBuilder, MethodBuilder methodBuilder)
			: base(typeBuilder)
		{
			if (methodBuilder == null) throw new ArgumentNullException("methodBuilder");

			_methodBuilder = methodBuilder;
			_methodBuilder.SetCustomAttribute(Type.Assembly.BLToolkitAttribute);
		}

		private MethodBuilder _methodBuilder;
		/// <summary>
		/// Gets MethodBuilder.
		/// </summary>
		public  MethodBuilder  MethodBuilder
		{
			get { return _methodBuilder; }
		}

		/// <summary>
		/// Converts the supplied <see cref="MethodBuilderHelper"/> to a <see cref="MethodBuilder"/>.
		/// </summary>
		/// <param name="methodBuilder">The MethodBuilderHelper.</param>
		/// <returns>A MethodBuilder.</returns>
		public static implicit operator MethodBuilder(MethodBuilderHelper methodBuilder)
		{
			return methodBuilder.MethodBuilder;
		}

		private EmitHelper _emitter;
		/// <summary>
		/// Gets EmitHelper.
		/// </summary>
		public override EmitHelper Emitter
		{
			get
			{
				if (_emitter == null)
					_emitter = new EmitHelper(this, _methodBuilder.GetILGenerator());

				return _emitter;
			}
		}
	}
}
