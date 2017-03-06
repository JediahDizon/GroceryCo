using GroceryCo.Controllers;

namespace GroceryCo
{
	class Program
	{
		static void Main(string[] args)
		{
			GroceryCoController controllerInstance = GroceryCoController.GetInstance();
			controllerInstance.ShowMainMenu();
			/*
			controllerInstance.ScanProductUPC(111);
			controllerInstance.ScanProductUPC(222);
			controllerInstance.ScanProductUPC(333);
			controllerInstance.ScanProductUPC(111);
			controllerInstance.ScanProductUPC(333); // Sale: Buy 2 Bananas Get 1 for %50 Off
			controllerInstance.ScanProductUPC(111); // Sale: Buy 3 Apples for $2.00
			controllerInstance.ScanProductUPC(222); // Sale: Buy 3 Oranges Get 1 Free - DISABLED
			controllerInstance.VoidItem(111); // Void Sale: Buy 3 Apples for $2.00
			controllerInstance.VoidItem(111);
			controllerInstance.VoidItem(000); // Item Not Found
			*/
			//int checkoutTotal = controllerInstance.CheckOut() / 100;
		}
	}
}
