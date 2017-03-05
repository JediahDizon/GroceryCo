using GroceryCo.Controllers;

namespace GroceryCo
{
	class Program
	{
		static void Main(string[] args)
		{
			while(true)
			{
				GroceryCoController controllerInstance = GroceryCoController.GetInstance();
				controllerInstance.ScanProductUPC(222);
				controllerInstance.ScanProductUPC(111);
				controllerInstance.ScanProductUPC(222);
				controllerInstance.ScanProductUPC(222); //Free
				controllerInstance.ScanProductUPC(111); //50% Off
				controllerInstance.ScanProductUPC(333);
				controllerInstance.ScanProductUPC(222);
				controllerInstance.ScanProductUPC(333);
				controllerInstance.ScanProductUPC(222);
				controllerInstance.ScanProductUPC(999); //No Product Found
				controllerInstance.ScanProductUPC(222); //Free

				controllerInstance.CheckOut();
				System.Console.ReadLine();
			}
		}
	}
}
