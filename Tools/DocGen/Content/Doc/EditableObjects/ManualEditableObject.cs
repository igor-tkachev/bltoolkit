public class TestObject : INotifyPropertyChanged
{
	// The FirstName editable property.
	//
	private string _originalFirstName;
	private string _currentFirstName;

	public override string FirstName
	{
		get { return _currentFirstName; }
		set
		{
			_currentFirstName = value;
			OnPropertyChanged("FirstName");
		}
	}

	bool IsFirstNameDirty
	{
		get { return _currentFirstName != _originalFirstName; }
	}

	void AcceptFirstNameChange()
	{
		if (IsFirstNameDirty)
		{
			_originalFirstName = _currentFirstName;
			OnPropertyChanged("FirstName");
		}
	}

	void RejectFirstNameChange()
	{
		if (IsFirstNameDirty)
		{
			_currentFirstName = _originalFirstName;
			OnPropertyChanged("FirstName");
		}
	}

	// The LastName editable property.
	//
	private string _originalLastName;
	private string _currentLastName;

	public override string LastName
	{
		get { return _currentLastName; }
		set
		{
			_currentLastName = value;
			OnPropertyChanged("LastName");
		}
	}

	bool IsLastNameDirty
	{
		get { return _currentLastName != _originalLastName; }
	}

	void AcceptLastNameChange()
	{
		if (IsLastNameDirty)
		{
			_originalLastName = _currentLastName;
			OnPropertyChanged("LastName");
		}
	}

	void RejectLastNameChange()
	{
		if (IsLastNameDirty)
		{
			_currentLastName = _originalLastName;
			OnPropertyChanged("LastName");
		}
	}

	// Common members.
	//
	public bool IsDirty
	{
		get
		{
			return IsFirstNameChange || IsLastNameChange;
		}
	}

	public void AcceptChanges()
	{
		AcceptFirstNameChange();
		AcceptLastNameChange();
	}

	public void RejectChanges()
	{
		RejectFirstNameChange();
		RejectLastNameChange();
	}

	public virtual event PropertyChangedEventHandler PropertyChanged;

	protected virtual void OnPropertyChanged(string propertyName)
	{
		if (PropertyChanged != null)
			PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
	}
}
