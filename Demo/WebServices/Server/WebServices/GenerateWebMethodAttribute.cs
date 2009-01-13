using System;
using System.EnterpriseServices;
using System.Web.Services;
using BLToolkit.TypeBuilder;

namespace Demo.WebServices.Server.WebServices
{
	[AttributeUsage(AttributeTargets.Method)]
	public class GenerateWebMethodAttribute : GenerateAttributeAttribute
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="GenerateWebMethodAttribute"/> class.
		/// </summary>
		public GenerateWebMethodAttribute(): base(typeof(WebMethodAttribute))
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="GenerateWebMethodAttribute"/> class.
		/// </summary>
		/// <param name="enableSession">Initializes whether session state is enabled for the XML Web service method.</param>
		public GenerateWebMethodAttribute(bool enableSession): base(typeof(WebMethodAttribute), enableSession)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="GenerateWebMethodAttribute"/> class.
		/// </summary>
		/// <param name="enableSession">Initializes whether session state is enabled for the XML Web service method.</param>
		/// <param name="transactionOption">Initializes the transaction support of an XML Web service method.</param>
		public GenerateWebMethodAttribute(
			bool              enableSession,
			TransactionOption transactionOption)
			: base(typeof(WebMethodAttribute), enableSession, transactionOption)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="GenerateWebMethodAttribute"/> class.
		/// </summary>
		/// <param name="enableSession">Initializes whether session state is enabled for the XML Web service method.</param>
		/// <param name="transactionOption">Initializes the transaction support of an XML Web service method.</param>
		/// <param name="cacheDuration">Initializes the number of seconds the response is cached.</param>
		public GenerateWebMethodAttribute(
			bool              enableSession,
			TransactionOption transactionOption,
			int               cacheDuration)
			: base(typeof(WebMethodAttribute), enableSession, transactionOption, cacheDuration)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="GenerateWebMethodAttribute"/> class.
		/// </summary>
		/// <param name="enableSession">Initializes whether session state is enabled for the XML Web service method.</param>
		/// <param name="transactionOption">Initializes the transaction support of an XML Web service method.</param>
		/// <param name="cacheDuration">Initializes the number of seconds the response is cached.</param>
		/// <param name="bufferResponse">Initializes whether the response for this request is buffered.</param>
		public GenerateWebMethodAttribute(
			bool              enableSession,
			TransactionOption transactionOption,
			int               cacheDuration,
			bool              bufferResponse)
			: base(typeof(WebMethodAttribute), enableSession, transactionOption, cacheDuration, bufferResponse)
		{
		}
	}
}