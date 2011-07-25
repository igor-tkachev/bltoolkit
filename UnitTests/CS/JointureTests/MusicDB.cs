using BLToolkit.Data;
using BLToolkit.Data.Linq;

namespace UnitTests.CS.JointureTests
{
    public class MusicDB : DbManager
    {
        public Table<Title> Title { get { return GetTable<Title>(); } }
        public Table<Artist2> Artist2 { get { return GetTable<Artist2>(); } }
    }
}