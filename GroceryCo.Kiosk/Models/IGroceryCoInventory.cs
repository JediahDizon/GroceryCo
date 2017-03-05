using GroceryCo.BusinessObjects;

namespace GroceryCo.Models
{
	interface IGroceryCoInventory
	{
		void InitializeDatabase();
		void PopulateInventory();
		void IncrementStock(Product toIncrement);
		void DecrementStock(Product toDecrement);
		void AddToInventory(Product toAdd);
		void RemoveFromInventory(Product toRemove);
		void EditFromInventory(Product toEdit);
		Product GetProductByUPC(int productUPC);
	}
}
