namespace InCinema.Repositories;

public interface IRepository<TItem, in TId> where TItem : class
{
    public IEnumerable<TItem> GetAll();
    public TItem GetById(TId id);
    public void Add(TItem item);
    public void Update(TItem item);
    public void Delete(TId id);
}