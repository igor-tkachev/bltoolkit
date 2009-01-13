using BLToolkit.TypeBuilder;

namespace Demo.WebServices.Client.WebClient
{
	[System.Diagnostics.DebuggerStepThrough]
	[System.ComponentModel.DesignerCategory("Code")]
	public abstract class WebClientBase<T> : WebClientBase where T : WebClientBase /*<T> commented due to csc.exe bug */
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="WebClientBase"/> class
		/// using the namespace from WebServiceBinding attribute as url.
		/// </summary>
		protected WebClientBase()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="WebClientBase"/> class.
		/// </summary>
		/// <param name="relativeUrl">Path to web service, relative to <see cref="WebClientBase.BaseUrl"/>.</param>
		protected WebClientBase(string relativeUrl) : base(relativeUrl)
		{
		}

		/// <summary>
		/// Cached client instance.
		/// </summary>
		private static T _instance;
		public  static T  Instance
		{
			get { return _instance ?? (_instance = CreateInstance()); }
		}

		protected static T CreateInstance()
		{
			return TypeFactory.CreateInstance<T>();
		}
	}
}