To use BLToolkit in Partial Trust Environment you should perform the following steps:

	For all assemblies containing classes for which BLToolkit generates
	new types such as Partial.Trust.Components.dll in this demo:

		- Sign the assembly.

		- Add the AllowPartiallyTrustedCallers attribute:

			[assembly: AllowPartiallyTrustedCallers]

		- Use BLTgen.exe to generate BLToolkit extensions at the post-build step.
		  For example:

			$(ProjectDir)..\..\..\Tools\BLTgen\bin\$(ConfigurationName)\BLTgen.exe $(TargetPath) /O:$(ProjectDir)..\Asp.Net\bin /K:$(ProjectDir)Partial.Trust.snk /D

		  Extension assembly must be signed as well (use /K flag).

	Turn the TypeFactory.LoadTypes flag on.

		Add the following section in the Web.config file:

			<configSections>
				<section name="bltoolkit" type="BLToolkit.Configuration.BLToolkitSection, BLToolkit.3" requirePermission="false"/>
			</configSections>
			<bltoolkit>
				<typeFactory loadTypes="true" />
			</bltoolkit>

			- or

			set 

				TypeFactory.LoadTypes = true;

			somewhere before the first use of BLToolkit (Global.asax for Web applications).
