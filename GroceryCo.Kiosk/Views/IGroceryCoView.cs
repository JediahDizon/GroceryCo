using GroceryCo.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
namespace GroceryCo.Views
{
	interface IGroceryCoView
	{
		void PrintWelcomeScreen();
		string MainMenu();
		string PromoMenu();
		string GetDirectoryInput();
		void PrintProduct(Product toPrint);
		void PrintVoidItem(Product toPrint);
		void PrintProductNotFound(int productUPC);
		void PrintTotal(int purchaseTotal);
		void PrintFarewellScreen();
	}
}
