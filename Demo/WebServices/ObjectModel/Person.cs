using System;
using System.Xml.Serialization;

using BLToolkit.DataAccess;
using BLToolkit.EditableObjects;
using BLToolkit.Mapping;
using BLToolkit.Validation;

namespace Demo.WebServices.ObjectModel
{
	[XmlType(AnonymousType = true)]
	public abstract class Person: EditableObject<Person>
	{
		[PrimaryKey, MapField("PersonID")] public abstract int    Id         { get; set; }
		[MaxLength(50), Required]          public abstract string LastName   { get; set; }
		[MaxLength(50), Required]          public abstract string FirstName  { get; set; }
		[MaxLength(50)]                    public abstract string MiddleName { get; set; }
		                                   public abstract Gender Gender     { get; set; }
	}
}
