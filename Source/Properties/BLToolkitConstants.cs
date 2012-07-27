using System;

namespace BLToolkit
{
	/// <summary>
	/// Global library constants.
	/// </summary>
	public static partial class BLToolkitConstants
	{
		/// <summary>
		/// Major component of version.
		/// </summary>
		public const string MajorVersion = "4";

		/// <summary>
		/// Minor component of version.
		/// </summary>
		public const string MinorVersion = "1";

		/// <summary>
		/// Build component of version.
		/// </summary>
		public const string Build = "10";

		/// <summary>
		/// Full version string.
		/// </summary>
        public const string FullVersionString = MajorVersion + "." + MinorVersion + "." + Build + "." + AutoRevision;

		/// <summary>
		/// Full BLT version.
		/// </summary>
		public static readonly Version FullVersion = new Version(FullVersionString);

		public const string ProductName        = "Business Logic Toolkit";
		public const string ProductDescription = "Business Logic Toolkit for .NET";
		public const string Copyright          = "\xA9 2002-2012 www.bltoolkit.net";
	}

    partial class BLToolkitConstants
    {
        // <summary>
        // Revision component of version.
        // <summary>
        public const string AutoRevision = "6";
    }
}
