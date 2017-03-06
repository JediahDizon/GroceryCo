using System;
using GroceryCo.BusinessObjects;

namespace GroceryCo.Views
{
	class GroceryCoView : IGroceryCoView
	{
		private string companyName = "GroceryCo";

		private int receiptWidth = 50;
		private static GroceryCoView viewInstance;

		public static GroceryCoView GetInstance()
		{
			if(viewInstance == null)
			{
				viewInstance = new GroceryCoView();
			}

			return viewInstance;
		}
		private GroceryCoView()
		{

		}

		public string MainMenu()
		{
			System.Console.WriteLine("1\tCheckout Products...");
			System.Console.WriteLine("2\tSales and Promo...");
			System.Console.WriteLine("3\tExit");
			return System.Console.ReadLine();
		}

		public string PromoMenu()
		{
			return null;
		}

		public string GetDirectoryInput()
		{
			System.Console.Write("Item List Directory: ");
			return System.Console.ReadLine();
		}

		public void PrintWelcomeScreen()
		{
			string headerText = "Welcome to";
			string centerText = companyName;
			string dividerText = "--------------------------------------------------";
			System.Console.WriteLine(string.Format("{0,-" + receiptWidth + "}", string.Format("{0," + ((receiptWidth + headerText.Length) / 2).ToString() + "}", dividerText)));
			System.Console.WriteLine(string.Format("{0,-" + receiptWidth + "}", string.Format("{0," + ((receiptWidth + headerText.Length) / 2).ToString() + "}", headerText)));
			System.Console.WriteLine(string.Format("{0,-" + receiptWidth + "}", string.Format("{0," + ((receiptWidth + centerText.Length) / 2).ToString() + "}", centerText)));
			System.Console.WriteLine("");
			System.Console.WriteLine(string.Format("{0,-" + receiptWidth + "}", string.Format("{0," + ((receiptWidth + DateTime.Today.ToString().Length) / 2).ToString() + "}", DateTime.Now.ToString())));
			System.Console.WriteLine(string.Format("{0,-" + receiptWidth + "}", string.Format("{0," + ((receiptWidth + dividerText.Length) / 2).ToString() + "}", dividerText)));
		}

		public void PrintProduct(Product toPrint)
		{
			string discountString = "";
			if (toPrint.Discount > 0)
			{
				discountString = string.Format("SALE: {0:###.00} Off", ((float)toPrint.Discount) / 100);
			}
			System.Console.WriteLine(string.Format("{0,-5}{1,-20}{2,10:###.00} {3,-10}", toPrint.UPC, toPrint.Name, ((float)toPrint.Price - toPrint.Discount) / 100, discountString));
		}

		public void PrintVoidItem(Product toPrint)
		{
			System.Console.WriteLine(string.Format("{0,-5}{1,-20}{2,10:###.00}", toPrint.UPC, "VOID " + toPrint.Name, -((float)toPrint.Price - toPrint.Discount) / 100));
		}

		public void PrintProductNotFound(string productName)
		{
			System.Console.WriteLine(string.Format("{0,-" + (receiptWidth / 2) + "} {1}", "Not Found: " + productName, "-"));
		}
		public void PrintProductNotFound(int productUPC)
		{
			System.Console.WriteLine(string.Format("{0,-" + (receiptWidth / 2) + "}", "Not Found: " + productUPC));
		}

		public void PrintTotal(int purchaseTotal)
		{
			System.Console.WriteLine("");
			System.Console.WriteLine(string.Format("{0,-25}{1,10:###.00}", "SUBTOTAL", ((float)purchaseTotal) / 100));
		}

		public void PrintFarewellScreen()
		{
			string headerText = "--------------------------------------------------";
			string centerText = "Thank you for Shopping at";
			string footerText = companyName;
			System.Console.WriteLine(string.Format("{0,-" + receiptWidth + "}", string.Format("{0," + ((receiptWidth + headerText.Length) / 2).ToString() + "}", headerText)));
			System.Console.WriteLine(string.Format("{0,-" + receiptWidth + "}", string.Format("{0," + ((receiptWidth + centerText.Length) / 2).ToString() + "}", centerText)));
			System.Console.WriteLine(string.Format("{0,-" + receiptWidth + "}", string.Format("{0," + ((receiptWidth + footerText.Length) / 2).ToString() + "}", footerText)));
		}
	}
}