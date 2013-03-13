using System;
using System.Collections.Generic;
using System.ComponentModel;

using BLToolkit.Mapping;
using BLToolkit.Reflection;

using NUnit.Framework;

namespace Mapping
{
	[TestFixture]
	public class BltMapTests
	{
		#region Types

		public class TestStep
		{
			public string Step { get; set; }
		}

		internal class Test_Int
		{
			private BindingList<TestStep> _steps;

			public Test_Int()
			{
				InitializeStepsList();
			}

			public bool NotNullSetterCalled = false;

			private void InitializeStepsList(IList<TestStep> init = null)
			{
				_steps = new BindingList<TestStep>(init ?? new List<TestStep>());
				if (init != null) NotNullSetterCalled = true;
			}

			[MapField(Storage = "Steps")]
			public IList<TestStep> Steps
			{
				get { return _steps; }
				private set { InitializeStepsList(value); }
			}
		}

		public class Test_Pub
		{
			private BindingList<TestStep> _steps;

			public Test_Pub()
			{
				InitializeStepsList();
			}

			public bool NotNullSetterCalled = false;

			private void InitializeStepsList(IList<TestStep> init = null)
			{
				_steps = new BindingList<TestStep>(init ?? new List<TestStep>());
				if (init != null) NotNullSetterCalled = true;
			}

			[MapField(Storage = "Steps")]
			public IList<TestStep> Steps
			{
				get { return _steps; }
				private set { InitializeStepsList(value); }
			}
		}

		public class TestStepRecord
		{
			public string Step { get; set; }
		}

		public class TestRecord
		{
			//[MapField]
			public List<TestStepRecord> Steps;
		}

		#endregion

		[Test]
		public void TestExpressionMapper_Int()
		{
			//Arrange
			var em = new ExpressionMapper<TestRecord, Test_Int>();
			var tr = new TestRecord { Steps = new List<TestStepRecord> { new TestStepRecord {Step = "Test" } } };

			//Act
			var t = em.GetMapper()(tr);

			//Assert
			Assert.IsTrue(t.Steps.Count == 1);
			Assert.IsTrue(t.NotNullSetterCalled);
		}

		[Test]
		public void TestExpressionMapper_Pub()
		{
			//Arrange
			var em = new ExpressionMapper<TestRecord, Test_Pub>();
			var tr = new TestRecord { Steps = new List<TestStepRecord> { new TestStepRecord { Step = "Test" } } };

			//Act
			var t = em.GetMapper()(tr);

			//Assert
			Assert.IsTrue(t.Steps.Count == 1);
			Assert.IsTrue(t.NotNullSetterCalled);
		}
	}
}
