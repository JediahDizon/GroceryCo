using GroceryCo.BusinessObjects;

namespace GroceryCo.Controllers
{
	interface IGroceryCoController
	{
		void ShowMainMenu();
		void ScanProductUPC(int productUPC);
		void VoidItem(int productUPC);
		int CheckOut();
		Product GetProductByUPC(int productUPC);
		Product GetProductByName(string productName);
		Product[] GetAllProducts();
	}
}