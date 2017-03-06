using GroceryCo.BusinessObjects;
using System.Collections.Generic;
namespace GroceryCo.Views
{
	interface IGroceryCoView
	{
		void PrintWelcomeScreen();
		string PrintMainMenu();
		string PrintPromoMenu(Dictionary<string, bool> toPrint);
		string GetDirectoryInput();
		void PrintProduct(Product toPrint);
		void PrintVoidItem(Product toPrint);
		void PrintProductNotFound(int productUPC);
		void PrintTotal(int purchaseTotal);
		void PrintFarewellScreen();
	}
}
