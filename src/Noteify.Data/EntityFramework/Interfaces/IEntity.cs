namespace Noteify.Data.EntityFramework.Interfaces
{
    /// <summary>
    ///     Help interface to specify the data type of the primary key for generic classes.
    /// </summary>
    /// <typeparam name="TKey">The data type of the primary key</typeparam>
    public interface IEntity<TKey>
    {
        TKey Id { get; set; }
    }
}