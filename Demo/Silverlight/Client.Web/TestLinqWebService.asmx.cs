using System;
using System.Web.Services;

using BLToolkit.ServiceModel;

namespace Client.Web
{
	/// <summary>
	/// Summary description for TestLinqWebService
	/// </summary>
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	[System.ComponentModel.ToolboxItem(false)]
	// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
	// [System.Web.Script.Services.ScriptService]
	public class TestLinqWebService : LinqService
	{
	}
}
