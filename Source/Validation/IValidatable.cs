namespace BLToolkit.Validation
{
	public interface IValidatable
	{
		void     Validate();

		bool     IsValid         (string fieldName);
		string[] GetErrorMessages(string fieldName);
	}
}
