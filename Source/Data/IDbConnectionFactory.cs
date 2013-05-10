namespace BLToolkit.Data
{
    public interface IDbConnectionFactory
    {
        DbManager CreateDbManager();
    }
}