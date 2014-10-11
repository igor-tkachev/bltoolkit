[BLToolkitGenerated]
public sealed class TestObject : EditableObjectTest.TestObject, IEditable, IMemberwiseEditable, IPrintDebugState
{
	// Note that the internal representation of the properties is EditableValue<string>.
	// The EditableValue class provides a mechanism to keep and control the field value state.
	//
	private /*[a]*/EditableValue<string>/*[/a]*/ _firstName;
	private /*[a]*/EditableValue<string>/*[/a]*/ _lastName;

	// PropertyInfo is used for internal purposes.
	//
	private static PropertyInfo _firstName_propertyInfo =
		TypeHelper.GetPropertyInfo(typeof(EditableObjectTest.TestObject), "FirstName", typeof(string), Type.EmptyTypes);
	private static PropertyInfo _lastName_propertyInfo  =
		TypeHelper.GetPropertyInfo(typeof(EditableObjectTest.TestObject), "LastName",  typeof(string), Type.EmptyTypes);

	// Constructors.
	//
	public TestObject()
	{
		this._firstName = new EditableValue<string>("");
		this._lastName  = new EditableValue<string>("");
	}

	public TestObject(InitContext ctx)
	{
		this._firstName = new EditableValue<string>("");
		this._lastName  = new EditableValue<string>("");
	}

	// Abstract property implementation.
	//
	public override string FirstName
	{
		get
		{
			return _firstName./*[a]*/Value/*[/a]*/;
		}

		set
		{
			_firstName./*[a]*/Value/*[/a]*/ = value;

			// The PropertyChanged event support.
			//
			((IPropertyChanged)this)./*[a]*/OnPropertyChanged/*[/a]*/(_firstName_propertyInfo);
		}
	}

	public override string LastName
	{
		get
		{
			return _lastName./*[a]*/Value/*[/a]*/;
		}

		set
		{
			_lastName./*[a]*/Value/*[/a]*/ = value;
			((IPropertyChanged)this)./*[a]*/OnPropertyChanged/*[/a]*/(_lastName_propertyInfo);
		}
	}

	// The IEditable interface implementation.
	//
	bool IEditable.IsDirty
	{
		get { return _firstName.IsDirty || _lastName.IsDirty; }
	}

	void IEditable.AcceptChanges()
	{
		this._firstName.AcceptChanges();
		this._lastName. AcceptChanges();
	}

	void IEditable.RejectChanges()
	{
		this._firstName.RejectChanges();
		this._lastName. RejectChanges();
	}

	// The IMemberwiseEditable interface implementation.
	//
	bool IMemberwiseEditable.AcceptMemberChanges(PropertyInfo propertyInfo, string memberName)
	{
		return
			_firstName.AcceptMemberChanges(_firstName_propertyInfo, memberName) ||
			_lastName. AcceptMemberChanges(_lastName_propertyInfo,  memberName);
	}

	void IMemberwiseEditable.GetDirtyMembers(PropertyInfo propertyInfo, ArrayList list)
	{
		_firstName.GetDirtyMembers(_firstName_propertyInfo, list);
		_lastName. GetDirtyMembers(_lastName_propertyInfo,  list);
	}

	bool IMemberwiseEditable.IsDirtyMember(PropertyInfo propertyInfo, string memberName, ref bool isDirty)
	{
		return
			_firstName.IsDirtyMember(_firstName_propertyInfo, memberName, ref isDirty) ||
			_lastName. IsDirtyMember(_lastName_propertyInfo,  memberName, ref isDirty);
	}

	bool IMemberwiseEditable.RejectMemberChanges(PropertyInfo propertyInfo, string memberName)
	{
		return
			_firstName.RejectMemberChanges(_firstName_propertyInfo, memberName) ||
			_lastName. RejectMemberChanges(_lastName_propertyInfo,  memberName);
	}

	// The IPrintDebugState interface implementation.
	//
	void IPrintDebugState.PrintDebugState(PropertyInfo propertyInfo, ref string str)
	{
		_firstName.PrintDebugState(_firstName_propertyInfo, ref str);
		_lastName. PrintDebugState(_lastName_propertyInfo,  ref str);
	}
}
