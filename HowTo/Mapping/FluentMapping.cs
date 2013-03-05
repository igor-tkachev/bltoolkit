using System;

using BLToolkit.Mapping;
using BLToolkit.Mapping.Fluent;
using BLToolkit.Mapping.MemberMappers;

namespace HowTo.Mapping
{
    public class Category
    {
        public int    CategoryID;
        public string CategoryName;
        public string Description;
        public Binary Picture;
        public TimeSpan RefreshTime;
        public AdditionalInfo AdditionalInfo;
        public List<Product> Products;
    }
    
    public class CategoryMap : FluentMap<Category>
    {
        public CategoryMap()
        {
            TableName("Categories");
            PrimaryKey(_ => _.CategoryID).Identity();
            Nullable(_ => _.Description);
            Nullable(_ => _.Picture);
            MapField(_ => _.RefreshTime).MemberMapper(typeof(TimeSpanBigIntMapper)).DbType(System.Data.DbType.Int64);
            MapField(_ => _.AdditionalInfo).MapIgnore(false).MemberMapper(typeof(BinarySerialisationMapper)).DbType(System.Data.DbType.Byte);
            Association(_ => _.Products,_ => _.CategoryID).ToMany((Product _) => _.CategoryID);
        }
    }
    
    public static void Main()
    {
        FluentConfig.Configure(Map.DefaultSchema).MapingFromAssemblyOf<Category>();
    }
}