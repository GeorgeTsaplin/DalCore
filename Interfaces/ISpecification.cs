namespace Itb.DalCore
{
    /// <summary> Specification interface
    /// </summary>
    /// <typeparam name="TEntity">entity data type</typeparam>
    /// <typeparam name="ID">entity ID data type</typeparam>
    //TODO: maybe IQueryable<TEntity> would be right desicion
    public interface ISpecification<TEntity, ID> where TEntity : Domain.IPrimaryKey<ID> {
		/// <summary> Check what specified entity satisfy specification
		/// </summary>
		/// <param name="entity">entity</param>
		/// <returns>entity satisfy specification?</returns>
		bool IsSatisfiedBy(TEntity entity);
	}
}
