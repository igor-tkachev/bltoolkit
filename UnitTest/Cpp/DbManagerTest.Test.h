// DbManagerTest.Test.h

#pragma once

using namespace System;
using namespace System::Data;

using namespace NUnit::Framework;

using namespace Rsdn::Framework::Data;

namespace Cpp
{
	namespace DbManagerTest
	{
		public __abstract __gc class Test
		{
		public:

			__property virtual String* get_ConfigurationString() = 0;

			virtual String *ParamText(String *param)
			{
				return S"@"->Concat(param);
			}

			// ExecuteNonQuery

			[NUnit::Framework::Test]
			virtual void SetCommand_CommandType_Text_ExecuteNonQuery()
			{
				DbManager *db = new DbManager(ConfigurationString);

				__try
				{
					db->SetCommand(CommandType::Text, "SELECT 1")->ExecuteNonQuery();
				}
				__finally
				{
					if (db != 0) db->Dispose();
				}
			}

			[NUnit::Framework::Test]
			virtual void SetCommand_CommandType_TableDirect_ExecuteNonQuery()
			{
				DbManager *db = new DbManager(ConfigurationString);

				__try
				{
					db->SetCommand(CommandType::TableDirect, "Customers")->ExecuteNonQuery();
				}
				__finally
				{
					if (db != 0) db->Dispose();
				}
			}

			[NUnit::Framework::Test]
			virtual void SetCommand_CommandType_StoredProcedure_ExecuteNonQuery()
			{
				DbManager *db = new DbManager(ConfigurationString);

				__try
				{
					db
						->SetCommand(CommandType::StoredProcedure, "[Ten Most Expensive Products]")
						->ExecuteNonQuery();
				}
				__finally
				{
					if (db != 0) db->Dispose();
				}
			}

			// ExecuteScalar

			[NUnit::Framework::Test]
			virtual void ExecuteScalar()
			{
				DbManager *db = new DbManager(ConfigurationString);

				__try
				{
					IDbDataParameter *params[] = 
					{ 
						db->Parameter(S"@country", S"USA")
					};

					int count = *static_cast<__box int*>(db
						->SetCommand(
							CommandType::Text,
							String::Format(
								S"SELECT Count(*) FROM Customers WHERE Country = {0}",
								ParamText(S"country")),
							params)
						->ExecuteScalar());

					Assert::IsFalse(count == 0);
				}
				__finally
				{
					if (db != 0) db->Dispose();
				}
			}
		};
	}
}