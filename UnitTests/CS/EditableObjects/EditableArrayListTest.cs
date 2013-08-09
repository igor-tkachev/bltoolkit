using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

using NUnit.Framework;

using BLToolkit.EditableObjects;
using BLToolkit.Reflection;

namespace EditableObjects
{
	[Serializable]
	public abstract class SerializableObject : EditableObject
	{
		public abstract int    ID   { get; set; }
		public abstract Guid   UUID { get; set; }
		public abstract string Name { get; set; }

		public abstract EditableList<string> Array { get; set; }
	}

	[TestFixture]
	public class EditableArrayListTest
	{
		public abstract class EditableTestObject : EditableObject
		{
			public abstract int    ID      { get; set; }
			public abstract string Name    { get; set; }
			public abstract int    Seconds { get; set; }

			public static EditableTestObject CreateInstance()
			{
				return (EditableTestObject)TypeAccessor.CreateInstance(typeof(EditableTestObject));
			}
			
			public static EditableTestObject CreateInstance(int id, string name, int seconds)
			{
				var instance = CreateInstance();

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
		
		private static readonly EditableTestObject[] _testObjects = new EditableTestObject[]
			{
				EditableTestObject.CreateInstance(0, "Smith",  24),
				EditableTestObject.CreateInstance(1, "John",   22),
				EditableTestObject.CreateInstance(2, "Anna",   48),
				EditableTestObject.CreateInstance(3, "Tim",    56),
				EditableTestObject.CreateInstance(4, "Xiniu",  39),
				EditableTestObject.CreateInstance(5, "Kirill", 30)
			};
		
		public EditableArrayListTest()
		{
			_testList = new EditableArrayList(typeof(EditableTestObject));
			_testList.ListChanged += TestList_ListChanged;
		}

		private readonly EditableArrayList _testList;
		
		private void TestList_ListChanged(object sender, ListChangedEventArgs e)
		{
			var array = sender as EditableArrayList;
			
			Assert.IsNotNull(array);
			if (e.ListChangedType != ListChangedType.Reset)
				Assert.That(array.IsDirty);

			if(e.ListChangedType == ListChangedType.ItemAdded)
			{
				var o = array[e.NewIndex];

				Assert.IsNotNull(o);
				Assert.That(array.NewItems.Contains(o));
			}

			if (e.ListChangedType != ListChangedType.ItemDeleted && e.ListChangedType != ListChangedType.Reset)
				Console.WriteLine("ListChanged (ID:{3}). Type: {0}, OldIndex: {1}, NewIndex: {2}", e.ListChangedType, e.OldIndex, e.NewIndex, (e.NewIndex >= 0 && e.NewIndex < _testList.Count) ? ((EditableTestObject)_testList[e.NewIndex]).ID : -1);
			else
				Console.WriteLine("ListChanged (ID:???). Type: {0}, OldIndex: {1}, NewIndex: {2}", e.ListChangedType, e.OldIndex, e.NewIndex);
		}
		
		[Test]
		public void TestAdd()
		{
			Console.WriteLine("--- TestAdd ---");
			_testList.Clear();
			_testList.RemoveSort();

			for (var i = 0; i < 3; i++)
				_testList.Add(_testObjects[i]);
			
			Assert.AreEqual(_testList.Count, 3);
			Assert.AreEqual(_testList[0], _testObjects[0]);
			Assert.AreEqual(_testList[1], _testObjects[1]);
			Assert.AreEqual(_testList[2], _testObjects[2]);
			
			var _subArray = new EditableTestObject[3];

			for (var i = 3; i < _testObjects.Length; i++)
				_subArray[i-3] = _testObjects[i];

			_testList.AddRange(_subArray);
			Assert.AreEqual(_testList.Count, 6);
			
			for (var i = 3; i < _testObjects.Length; i++)
				Assert.AreEqual(_subArray[i - 3], _testObjects[i]);

			PrintList();
			
			_testList.Clear();
		}

		[Test]
		public void TestAddNew()
		{
			var list = new EditableList<EditableTestObject>();
			var listChangedFired = false;

			list.ListChanged += (sender, args) => listChangedFired = true;

			list.AddNew();
			Assert.That(listChangedFired);
			Assert.AreEqual(1, list.Count);
			Assert.AreEqual(1, list.NewItems.Count);

			listChangedFired = false;
			list.CancelNew(0);
			Assert.That(listChangedFired);
			Assert.IsEmpty(list);
			Assert.IsEmpty(list.NewItems);
		}

		[Test]
		public void TestInsert()
		{
			Console.WriteLine("--- TestInsert ---");
			_testList.Clear();
			_testList.RemoveSort();

			for (var i = 0; i < 3; i++)
				_testList.Insert(0, _testObjects[i]);

			Assert.AreEqual(_testList.Count, 3);
			Assert.AreEqual(_testList[0], _testObjects[2]);
			Assert.AreEqual(_testList[1], _testObjects[1]);
			Assert.AreEqual(_testList[2], _testObjects[0]);

			var _subArray = new EditableTestObject[3];

			for (var i = 3; i < _testObjects.Length; i++)
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
			Assert.IsEmpty(_testList.DelItems);

			_testList.AcceptChanges();
			Assert.IsEmpty(_testList.NewItems);

			_testList.Remove(_testList[0]);
			Assert.AreEqual(_testList.Count, 4);
			Assert.IsNotEmpty(_testList.DelItems);

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

			for (var i = 0; i < 3; i++)
				_testList.Add(_testObjects[i]);

			Assert.AreEqual(_testList.Count, 3);
			Assert.AreEqual(_testList[1], _testObjects[0]);
			Assert.AreEqual(_testList[2], _testObjects[1]);
			Assert.AreEqual(_testList[0], _testObjects[2]);

			var _subArray = new EditableTestObject[3];

			for (var i = 3; i < _testObjects.Length; i++)
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

			for (var i = 0; i < 3; i++)
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

			for (var i = 0; i < 3; i++)
				_testList.Insert(0, _testObjects[i]);

			Assert.AreEqual(_testList.Count, 3);
			Assert.AreEqual(_testList[0], _testObjects[2]);
			Assert.AreEqual(_testList[1], _testObjects[0]);
			Assert.AreEqual(_testList[2], _testObjects[1]);

			var _subArray = new EditableTestObject[3];

			for (var i = 3; i < _testObjects.Length; i++)
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

			for (var i = 0; i < 3; i++)
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

			var eto = EditableTestObject.CreateInstance(6, "Dummy", 10);

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

			var test   = TypeAccessor<SerializableObject>.CreateInstance();
			var stream = new MemoryStream();
			var bf     = new BinaryFormatter();

			bf.Serialize(stream, test);

			//Configuration.NotifyOnChangesOnly = false;
		}

		//////[Test] Resharpe 8 issue
		public void SerializationTest2()
		{
			//Configuration.NotifyOnChangesOnly = true;

			var list = new EditableList<SerializableObject> { TypeAccessor<SerializableObject>.CreateInstance() };

			var formatter = new BinaryFormatter();

			using (var stream = new MemoryStream())
			{
				formatter.Serialize(stream, list);
				stream.Position = 0;

				var result = formatter.Deserialize(stream);

				Assert.IsNotNull(result);

				var eal = (EditableList<SerializableObject>)result;

				Console.WriteLine(eal.Count);

				eal.ListChanged += eal_ListChanged;

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

		private static int _notificationCount;

		static void eal_ListChanged(object sender, ListChangedEventArgs e)
		{
			Console.WriteLine(e.ListChangedType);
			_notificationCount++;
		}

		[Test]
		public void SortTest()
		{
			var dataList = new EditableList<EditableTestObject>
			{
				EditableTestObject.CreateInstance(1, "John",   60),
				EditableTestObject.CreateInstance(1, "John",   60),
				EditableTestObject.CreateInstance(1, "John",   60),
				EditableTestObject.CreateInstance(2, "Tester", 70),
				EditableTestObject.CreateInstance(2, "Tester", 70),
				EditableTestObject.CreateInstance(2, "Tester", 70),
				EditableTestObject.CreateInstance(3, "Tester", 70),
				EditableTestObject.CreateInstance(3, "Tester", 70),
				EditableTestObject.CreateInstance(3, "Tester", 70)
			};

			var bindingSource = new BindingSource(dataList, null) { Sort = "ID" };
			var prev          = 0;

			foreach (var o in dataList)
			{
				Assert.IsTrue(o.ID >= prev);
				prev = o.ID;
			}

			bindingSource[0] = EditableTestObject.CreateInstance(2, "John", 60);

			prev = 0;

			foreach (var o in dataList)
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

			protected override void OnListChanged(ListChangedEventArgs e)
			{
				OnOnListChangedCalled();
				base.OnListChanged(e);
			}
		}


		[Test]
		public void DerivedOnListChanged()
		{
			var called = false;
			var list   = new DerivedEditableList<int>();

			list.OnListChangedCalled += (sender, args) => called = true;

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

		[Test]
		public void CloneTest()
		{
			var src   = new EditableList<int> { 1, 2, 3 };
			var clone = (EditableList<int>)src.Clone();

			Assert.AreEqual(src.Count,          clone.Count);
			Assert.AreEqual(src.NewItems.Count, clone.NewItems.Count);
			Assert.AreEqual(src.DelItems.Count, clone.DelItems.Count);

			src.AcceptChanges();

			clone = (EditableList<int>)src.Clone();
			
			Assert.AreEqual(src.Count,          clone.Count);
			Assert.AreEqual(src.NewItems.Count, clone.NewItems.Count);
			Assert.AreEqual(src.DelItems.Count, clone.DelItems.Count);

			src.RemoveAt(1);

			clone = (EditableList<int>)src.Clone();
			
			Assert.AreEqual(src.Count,          clone.Count);
			Assert.AreEqual(src.NewItems.Count, clone.NewItems.Count);
			Assert.AreEqual(src.DelItems.Count, clone.DelItems.Count);
		}

		[Test]
		public void CreateCleanTest()
		{
			var list = new EditableList<int>(new[] { 1, 2, 3 });

			Assert.IsFalse(list.IsDirty);

			Assert.AreEqual(3, list.Count);
			Assert.AreEqual(0, list.NewItems.Count);
			Assert.AreEqual(0, list.DelItems.Count);
			
			list = new EditableList<int>(new int[] { });

			Assert.IsFalse(list.IsDirty);

			Assert.AreEqual(0, list.Count);
			Assert.AreEqual(0, list.NewItems.Count);
			Assert.AreEqual(0, list.DelItems.Count);		
		}

		[Test]
		public void IsDirtyTest()
		{
			var list = new EditableList<EditableTestObject>();

			list.AddNew();
			list.AddNew();

			Assert.IsTrue(list.IsDirty);

			list.AcceptChanges();

			Assert.IsFalse(list.IsDirty);

			list[1].ID = 101;

			Assert.IsTrue(list[1].IsDirty);
			Assert.IsTrue(list.IsDirty);
		}

		[Test]
		public void CreateWithDirtyTest()
		{
			var to = EditableTestObject.CreateInstance();

			to.ID = 10;

			Assert.IsTrue(to.IsDirty);

			var list = new EditableList<EditableTestObject>(new[] { to });

			Assert.IsTrue(to.IsDirty);
			Assert.IsTrue(list.IsDirty);
		}
	}
}