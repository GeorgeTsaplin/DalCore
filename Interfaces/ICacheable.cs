namespace Itb.DalCore
{
    /// <summary> Cacheable objects interface
    /// </summary>
    public interface ICacheable {
		/// <summary> Clear object's cache
		/// </summary>
		/// <param name="cacheLevels">cache levels to clear</param>
		void ClearCache(CacheLevels cacheLevels);
	}
}
