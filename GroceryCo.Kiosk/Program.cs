using GroceryCo.Controllers;

namespace GroceryCo
{
	class Program
	{
		static void Main(string[] args)
		{
			GroceryCoController controllerInstance = GroceryCoController.GetInstance();
			controllerInstance.ReadProductList();
			controllerInstance.CheckOut();
		}
	}
}
