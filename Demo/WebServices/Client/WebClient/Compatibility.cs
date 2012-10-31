namespace Demo.WebServices.Client.WebClient
{
#if !FW3

	/// <summary>
	/// Encapsulates a method that takes no parameters and does not return a value.
	/// </summary>
	public delegate void Action();

	/// <summary>
	/// Encapsulates a method that has two parameters and does not return a value.
	/// </summary>
	/// <param name="arg1">
	/// The first parameter of the method that this delegate encapsulates.
	/// </param>
	/// <param name="arg2">
	/// The second parameter of the method that this delegate encapsulates.
	/// </param>
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// </typeparam>
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// </typeparam>
	public delegate void Action<T1, T2>(T1 arg1, T2 arg2);

	/// <summary>
	/// Encapsulates a method that has three parameters and does not return a value.
	/// </summary>
	/// <param name="arg1">
	/// The first parameter of the method that this delegate encapsulates.
	/// </param>
	/// <param name="arg2">
	/// The second parameter of the method that this delegate encapsulates.
	/// </param>
	/// <param name="arg3">
	/// The third parameter of the method that this delegate encapsulates.
	/// </param>
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// </typeparam>
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// </typeparam>
	/// <typeparam name="T3">
	/// The type of the third parameter of the method that this delegate encapsulates.
	/// </typeparam>
	public delegate void Action<T1, T2, T3>(T1 arg1, T2 arg2, T3 arg3);

	/// <summary>
	/// Encapsulates a method that has four parameters and does not return a value.
	/// </summary>
	/// <param name="arg1">
	/// The first parameter of the method that this delegate encapsulates.
	/// </param>
	/// <param name="arg2">
	/// The second parameter of the method that this delegate encapsulates.
	/// </param>
	/// <param name="arg3">
	/// The third parameter of the method that this delegate encapsulates.
	/// </param>
	/// <param name="arg4">
	/// The fourth parameter of the method that this delegate encapsulates.
	/// </param>
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// </typeparam>
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// </typeparam>
	/// <typeparam name="T3">
	/// The type of the third parameter of the method that this delegate encapsulates.
	/// </typeparam>
	/// <typeparam name="T4">
	/// The type of the fourth parameter of the method that this delegate encapsulates.
	/// </typeparam>
	public delegate void Action<T1, T2, T3, T4>(T1 arg1, T2 arg2, T3 arg3, T4 arg4);

#endif
}
