namespace Lab05_JeremyVizcarra.Infrastructure.Persistence;

public class GenericRepository<T> : Repository<T> where T : class
{
    public GenericRepository(Lab05DbContext context) : base(context)
    {
    }
}
