namespace Itb.DalCore.Domain
{
	/// <summary> Interface of objects with primary key
	/// </summary>
	/// <typeparam name="ID">primary key data type</typeparam>
	public interface IPrimaryKey<ID> {
		/// <summary> primary key value (get)
		/// </summary>
		ID __Pk { get; }
	}
}
