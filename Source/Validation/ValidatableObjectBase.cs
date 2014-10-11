namespace BLToolkit.Validation
{
	public class ValidatableObjectBase
	{
		public virtual void Validate()
		{
			Validator.Validate(this);
		}
	}
}
