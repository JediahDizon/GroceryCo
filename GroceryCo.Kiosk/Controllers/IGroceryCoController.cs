using GroceryCo.BusinessObjects;

namespace GroceryCo.Controllers
{
	interface IGroceryCoController
	{
		void ScanProductUPC(int productUPC);
		void VoidItem(int productUPC);
		void CheckOut();
		Product GetProductByUPC(int productUPC);
		Product GetProductByName(string productName);
		Product[] GetAllProducts();
	}
}