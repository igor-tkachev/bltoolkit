using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using BLToolkit.EditableObjects;
using BLToolkit.Reflection;
using NUnit.Framework;

namespace EditableObjects
{
	[TestFixture]
	public class NotifyCollectionChangeTest
	{
		public abstract class EditableTestObject : EditableObject
		{
			public abstract int    ID      { get; set; }
			public abstract string Name    { get; set; }
			public abstract int    Seconds { get; set; }

			public static EditableTestObject CreateInstance()
			{
				return TypeAccessor<EditableTestObject>.CreateInstance();
			}
			
			public static EditableTestObject CreateInstance(int id, string name, int seconds)
			{
				EditableTestObject instance = CreateInstance();

				instance.ID = id;
				instance.Name = name;
				instance.Seconds = seconds;
				
				return instance;
			}

			public override string ToString()
			{
				return string.Format("EditableTestObject - ID:({0}) Name: ({1}) Seconds({2})", ID, Name, Seconds);
			}
		}
		
		private static readonly EditableTestObject[] _testObjects = new EditableTestObject[6]
		{
			EditableTestObject.CreateInstance(0, "Smith",  24),
			EditableTestObject.CreateInstance(1, "John",   22),
			EditableTestObject.CreateInstance(2, "Anna",   48),
			EditableTestObject.CreateInstance(3, "Tim",    56),
			EditableTestObject.CreateInstance(4, "Xiniu",  39),
			EditableTestObject.CreateInstance(5, "Kirill", 30)
		};
		
		public NotifyCollectionChangeTest()
		{
			_testList = new EditableList<EditableTestObject>();
			_testList.CollectionChanged += TestList_CollectionChanged;
		}

		private readonly EditableArrayList _testList;

		private void TestList_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			EditableArrayList array = sender as EditableArrayList;
			
			Assert.IsNotNull(array);
			if (e.Action != NotifyCollectionChangedAction.Reset)
				Assert.That(array.IsDirty);

			EditableTestObject o = (EditableTestObject)(e.NewItems != null? e.NewItems[0]:
									e.OldItems != null ? e.OldItems[0]: null);

			Console.WriteLine("CollectionChanged (ID:{3}). Type: {0}, OldIndex: {1}, NewIndex: {2}",
				e.Action, e.OldStartingIndex, e.NewStartingIndex, o != null? o.ID: -1);
		}
		
		[Test]
		public void TestAdd()
		{
			Console.WriteLine("--- TestAdd ---");
			_testList.Clear();
			_testList.RemoveSort();

			for (int i = 0; i < 3; i++)
				_testList.Add(_testObjects[i]);
			
			Assert.AreEqual(_testList.Count, 3);
			Assert.AreEqual(_testList[0], _testObjects[0]);
			Assert.AreEqual(_testList[1], _testObjects[1]);
			Assert.AreEqual(_testList[2], _testObjects[2]);
			
			EditableTestObject[] subArray = new EditableTestObject[3];

			for (int i = 3; i < _testObjects.Length; i++)
				subArray[i-3] = _testObjects[i];

			_testList.AddRange(subArray);
			Assert.AreEqual(_testList.Count, 6);
			
			for (int i = 3; i < _testObjects.Length; i++)
				Assert.AreEqual(subArray[i - 3], _testObjects[i]);

			PrintList();
			
			_testList.Clear();
		}

		[Test]
		public void TestAddNew()
		{
			EditableList<EditableTestObject> list = new EditableList<EditableTestObject>();
			bool collectionChangedFired = false;
			list.CollectionChanged += delegate { collectionChangedFired = true; };

			list.AddNew();
			Assert.That(collectionChangedFired);
			Assert.AreEqual(1, list.Count);
			Assert.AreEqual(1, list.NewItems.Count);
			Assert.AreEqual(0, list.DelItems.Count);

			collectionChangedFired = false;
			list.CancelNew(0);
			Assert.That(collectionChangedFired);
			Assert.IsEmpty(list);
			Assert.IsEmpty(list.NewItems);
			Assert.IsEmpty(list.DelItems);
		}

		[Test]
		public void TestInsert()
		{
			Console.WriteLine("--- TestInsert ---");
			_testList.Clear();
			_testList.RemoveSort();

			for (int i = 0; i < 3; i++)
				_testList.Insert(0, _testObjects[i]);

			Assert.AreEqual(_testList.Count, 3);
			Assert.AreEqual(_testList[0], _testObjects[2]);
			Assert.AreEqual(_testList[1], _testObjects[1]);
			Assert.AreEqual(_testList[2], _testObjects[0]);

			EditableTestObject[] _subArray = new EditableTestObject[3];

			for (int i = 3; i < _testObjects.Length; i++)
				_subArray[i-3] = _testObjects[i];

			_testList.InsertRange(0, _subArray);
			Assert.AreEqual(_testList.Count, 6);

			Assert.AreEqual(_testList[0], _testObjects[3]);
			Assert.AreEqual(_testList[1], _testObjects[4]);
			Assert.AreEqual(_testList[2], _testObjects[5]);

			PrintList();

			_testList.Clear();
		}
		
		[Test]
		public void TestRemove()
		{
			Console.WriteLine("--- TestRemove ---");

			_testList.Clear();
			_testList.RemoveSort();
			_testList.AddRange(_testObjects);
			
			_testList.RemoveAt(2);
			Assert.AreEqual(_testList.Count, 5);
			_testList.Remove(_testList[0]);
			Assert.AreEqual(_testList.Count, 4);
			_testList.Clear();
			Assert.AreEqual(_testList.Count, 0);
		}

		[Test]
		public void TestRemoveRange()
		{
			Console.WriteLine("--- TestRemoveRange ---");

			_testList.Clear();
			_testList.RemoveSort();
			_testList.AcceptChanges();
			_testList.AddRange(_testObjects);

			Assert.AreEqual(6, _testList.Count);
			Assert.AreEqual(6, _testList.NewItems.Count);
			Assert.AreEqual(0, _testList.DelItems.Count);

			_testList.RemoveRange(1, 3);

			Assert.AreEqual(3, _testList.Count);
			Assert.AreEqual(3, _testList.NewItems.Count);
			Assert.AreEqual(0, _testList.DelItems.Count);
			Assert.AreEqual(0, ((EditableTestObject)_testList[0]).ID);
			Assert.AreEqual(4, ((EditableTestObject)_testList[1]).ID);

			_testList.AcceptChanges();

			Assert.AreEqual(3, _testList.Count);
			Assert.AreEqual(0, _testList.NewItems.Count);
			Assert.AreEqual(0, _testList.DelItems.Count);

			_testList.RemoveRange(0, 1);

			Assert.AreEqual(2, _testList.Count);
			Assert.AreEqual(0, _testList.NewItems.Count);
			Assert.AreEqual(1, _testList.DelItems.Count);
			
			_testList.Clear();

			Assert.AreEqual(0, _testList.Count);
			Assert.AreEqual(0, _testList.NewItems.Count);
			Assert.AreEqual(3, _testList.DelItems.Count);
			
			_testList.AcceptChanges();

			Assert.AreEqual(0, _testList.Count);
			Assert.AreEqual(0, _testList.NewItems.Count);
			Assert.AreEqual(0, _testList.DelItems.Count);
		}

		[Test]
		public void TestSortedAdd()
		{
			Console.WriteLine("--- TestSortedAdd ---");
			_testList.Clear();
			_testList.RemoveSort();
			_testList.ApplySort(_testList.GetItemProperties(null)["Seconds"], ListSortDirection.Descending);

			for (int i = 0; i < 3; i++)
				_testList.Add(_testObjects[i]);

			Assert.AreEqual(_testList.Count, 3);
			Assert.AreEqual(_testList[1], _testObjects[0]);
			Assert.AreEqual(_testList[2], _testObjects[1]);
			Assert.AreEqual(_testList[0], _testObjects[2]);

			EditableTestObject[] _subArray = new EditableTestObject[3];

			for (int i = 3; i < _testObjects.Length; i++)
				_subArray[i-3] = _testObjects[i];

			_testList.AddRange(_subArray);
			Assert.AreEqual(_testList.Count, 6);

			Assert.AreEqual(_testList[0], _testObjects[3]);
			Assert.AreEqual(_testList[2], _testObjects[4]);
			Assert.AreEqual(_testList[3], _testObjects[5]);

			PrintList();

			_testList.Clear();
			Assert.AreEqual(_testList.Count, 0);

			_testList.ApplySort(_testList.GetItemProperties(null)["Seconds"], ListSortDirection.Ascending);

			for (int i = 0; i < 3; i++)
				_testList.Add(_testObjects[i]);

			Assert.AreEqual(_testList.Count, 3);
			Assert.AreEqual(_testList[1], _testObjects[0]);
			Assert.AreEqual(_testList[0], _testObjects[1]);
			Assert.AreEqual(_testList[2], _testObjects[2]);

			_testList.AddRange(_subArray);
			Assert.AreEqual(_testList.Count, 6);

			Assert.AreEqual(_testList[5], _testObjects[3]);
			Assert.AreEqual(_testList[3], _testObjects[4]);
			Assert.AreEqual(_testList[2], _testObjects[5]);

			PrintList();

			_testList.Clear();
		}
		
		[Test]
		public void TestSortedInsert()
		{
			Console.WriteLine("--- TestSortedInsert ---");
			_testList.Clear();
			_testList.RemoveSort();
			_testList.ApplySort(_testList.GetItemProperties(null)["Seconds"], ListSortDirection.Descending);

			for (int i = 0; i < 3; i++)
				_testList.Insert(0, _testObjects[i]);

			Assert.AreEqual(_testList.Count, 3);
			Assert.AreEqual(_testList[0], _testObjects[2]);
			Assert.AreEqual(_testList[1], _testObjects[0]);
			Assert.AreEqual(_testList[2], _testObjects[1]);

			EditableTestObject[] _subArray = new EditableTestObject[3];

			for (int i = 3; i < _testObjects.Length; i++)
				_subArray[i-3] = _testObjects[i];

			_testList.InsertRange(0, _subArray);
			Assert.AreEqual(_testList.Count, 6);

			Assert.AreEqual(_testList[0], _testObjects[3]);
			Assert.AreEqual(_testList[2], _testObjects[4]);
			Assert.AreEqual(_testList[3], _testObjects[5]);

			PrintList();

			_testList.Clear();
			Assert.AreEqual(_testList.Count, 0);

			_testList.ApplySort(_testList.GetItemProperties(null)["Seconds"], ListSortDirection.Ascending);

			for (int i = 0; i < 3; i++)
				_testList.Insert(0, _testObjects[i]);

			Assert.AreEqual(_testList.Count, 3);
			Assert.AreEqual(_testList[0], _testObjects[1]);
			Assert.AreEqual(_testList[1], _testObjects[0]);
			Assert.AreEqual(_testList[2], _testObjects[2]);

			_testList.InsertRange(0, _subArray);
			Assert.AreEqual(_testList.Count, 6);

			Assert.AreEqual(_testList[2], _testObjects[5]);
			Assert.AreEqual(_testList[3], _testObjects[4]);
			Assert.AreEqual(_testList[5], _testObjects[3]);

			PrintList();

			_testList.Clear();
		}
		
		[Test]
		public void TestSortedPropertyChange()
		{
			Console.WriteLine("--- TestSortedPropertyChange ---");
			_testList.Clear();
			_testList.RemoveSort();
			_testList.ApplySort(_testList.GetItemProperties(null)["Seconds"], ListSortDirection.Descending);
			_testList.AddRange(_testObjects);

			EditableTestObject eto = EditableTestObject.CreateInstance(6, "Dummy", 10);

			_testList.Add(eto);
			Assert.AreEqual(_testList.Count, 7);
			Assert.AreEqual(_testList[6], eto);

			eto.Seconds = 20;
			Assert.AreEqual(_testList[6], eto);

			eto.Seconds = 23;
			Assert.AreEqual(_testList[5], eto);

			eto.Seconds = 30;
			Assert.AreEqual(_testList[4], eto);

			eto.Seconds = 40;
			Assert.AreEqual(_testList[2], eto);

			eto.Seconds = 50;
			Assert.AreEqual(_testList[1], eto);

			eto.Seconds = 60;
			Assert.AreEqual(_testList[0], eto);
		}
		
		private void PrintList()
		{
			Console.WriteLine("--- Print List ---");
			foreach (EditableTestObject o in _testList)
				Console.WriteLine(o);
		}

		[Test]
		public void SerializationTest()
		{
			//Configuration.NotifyOnEqualSet = true;

			SerializableObject test = TypeAccessor<SerializableObject>.CreateInstance();

			MemoryStream    stream = new MemoryStream();
			BinaryFormatter bf     = new BinaryFormatter();

			bf.Serialize(stream, test);

			//Configuration.NotifyOnChangesOnly = false;
		}

		//////[Test] Resharpe 8 issue
		public void SerializationTest2()
		{
			//Configuration.NotifyOnChangesOnly = true;

			EditableList<SerializableObject> list = new EditableList<SerializableObject>();

			list.Add(TypeAccessor<SerializableObject>.CreateInstance());

			BinaryFormatter formatter = new BinaryFormatter();

			using (MemoryStream stream = new MemoryStream())
			{
				formatter.Serialize(stream, list);
				stream.Position = 0;

				object result = formatter.Deserialize(stream);

				Assert.IsNotNull(result);

				EditableList<SerializableObject> eal = (EditableList<SerializableObject>)result;

				Console.WriteLine(eal.Count);

				eal.CollectionChanged += eal_CollectionChanged;

				eal[0].ID = 0;
				_notificationCount = 0;
				eal[0].ID = -100;
				eal[0].ID = -100;
				eal[0].ID = -100;
				eal[0].ID = -100;
				eal[0].ID = -100;
				eal[0].ID = -100;

				Console.WriteLine(eal.Count);

				//Assert.AreEqual(_notificationCount, 1);
			}

			//Configuration.NotifyOnChangesOnly = false;
		}

		private static int _notificationCount = 0;

		static void eal_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			Console.WriteLine(e.Action);
			_notificationCount++;
		}

		[Test]
		public void SortTest()
		{
			EditableList<EditableTestObject> dataList = new EditableList<EditableTestObject>();

			dataList.Add(EditableTestObject.CreateInstance(1, "John", 60));
			dataList.Add(EditableTestObject.CreateInstance(1, "John", 60));
			dataList.Add(EditableTestObject.CreateInstance(1, "John", 60));
			dataList.Add(EditableTestObject.CreateInstance(2, "Tester", 70));
			dataList.Add(EditableTestObject.CreateInstance(2, "Tester", 70));
			dataList.Add(EditableTestObject.CreateInstance(2, "Tester", 70));
			dataList.Add(EditableTestObject.CreateInstance(3, "Tester", 70));
			dataList.Add(EditableTestObject.CreateInstance(3, "Tester", 70));
			dataList.Add(EditableTestObject.CreateInstance(3, "Tester", 70));

			BindingSource bindingSource = new BindingSource(dataList, null);
			bindingSource.Sort = "ID";

			int prev = 0;
			foreach (EditableTestObject o in dataList)
			{
				Assert.IsTrue(o.ID >= prev);
				prev = o.ID;
			}

			bindingSource[0] = EditableTestObject.CreateInstance(2, "John", 60);

			prev = 0;
			foreach (EditableTestObject o in dataList)
			{
				Assert.IsTrue(o.ID >= prev);
				prev = o.ID;
			}
		}

		public class DerivedEditableList<T> : EditableList<T>
		{
			public event EventHandler OnListChangedCalled;

			protected void OnOnListChangedCalled()
			{
				if (OnListChangedCalled != null)
					OnListChangedCalled(this, EventArgs.Empty);
			}

			protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
			{
				OnOnListChangedCalled();
				base.OnCollectionChanged(e);
			}
		}


		[Test]
		public void DerivedOnListChanged()
		{
			bool called = false;

			DerivedEditableList<int> list = new DerivedEditableList<int>();
			list.OnListChangedCalled += delegate
			{
				called = true;
			};

			list.Add(1);

			Assert.IsTrue(called);
			Assert.AreEqual(1, list.NewItems.Count);
			Assert.AreEqual(1, list.Count);
			Assert.AreEqual(0, list.DelItems.Count);

			called = false;
			list.RemoveAt(0);

			Assert.IsTrue(called);
			Assert.AreEqual(0, list.NewItems.Count);
			Assert.AreEqual(0, list.Count);
			Assert.AreEqual(0, list.DelItems.Count);
		}

	}
}