using System;
using GroceryCo.BusinessObjects;

namespace GroceryCo.Views
{
	class GroceryCoView : IGroceryCoView
	{
		private string companyName = "GroceryCo";

		private int receiptWidth = 25;
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
		public string GetDirectoryInput()
		{
			System.Console.Write("Item List Directory: ");
			return System.Console.ReadLine();
		}
		public void PrintWelcomeScreen()
		{
			string headerText = "Welcome to";
			string centerText = companyName;
			string footerText = "-------------------------";
			System.Console.WriteLine(String.Format("{0,-" + receiptWidth + "}", String.Format("{0," + ((receiptWidth + headerText.Length) / 2).ToString() + "}", headerText)));
			System.Console.WriteLine(String.Format("{0,-" + receiptWidth + "}", String.Format("{0," + ((receiptWidth + centerText.Length) / 2).ToString() + "}", centerText)));
			System.Console.WriteLine(String.Format("{0,-" + receiptWidth + "}", String.Format("{0," + ((receiptWidth + footerText.Length) / 2).ToString() + "}", footerText)));
			System.Console.WriteLine(String.Format("{0,-" + receiptWidth + "}", String.Format("{0," + ((receiptWidth + DateTime.Today.ToString().Length) / 2).ToString() + "}", DateTime.Now.ToString())));
			System.Console.WriteLine("");
		}

		public void PrintProduct(Product toPrint)
		{
			string discountString = "";
			if (toPrint.Discount > 0)
			{
				discountString = string.Format("SALE: {0}", toPrint.Discount/100);
			}
			System.Console.WriteLine(string.Format("{0,-" + (receiptWidth - receiptWidth / 3) + "} {1:0.00} {2}", toPrint.Name, (float)(toPrint.Price - toPrint.Discount) / 100, discountString));
		}

		public void PrintVoidItem(Product toPrint)
		{
			// Currently out of project's scope.
		}

		public void PrintProductNotFound(int productUPC)
		{
			System.Console.WriteLine(string.Format("{0,-" + (receiptWidth - receiptWidth / 3) + "} {1}", "N/A", "-"));
		}

		public void PrintTotal(int purchaseTotal)
		{
			string footerText = "-------------------------";
			System.Console.WriteLine("");
			System.Console.WriteLine(string.Format("{0,-" + (receiptWidth - receiptWidth / 3) + "} {1:0.00}", "SUBTOTAL", (float)purchaseTotal / 100));
			System.Console.WriteLine(String.Format("{0,-" + (receiptWidth - receiptWidth / 3) + "}", String.Format("{0," + ((receiptWidth + footerText.Length) / 2).ToString() + "}", footerText)));
		}
		public void PrintFarewellScreen()
		{
			string headerText = "Thank you for Shopping at";
			string centerText = companyName;
			System.Console.WriteLine(String.Format("{0,-" + receiptWidth + "}", String.Format("{0," + ((receiptWidth + headerText.Length) / 2).ToString() + "}", headerText)));
			System.Console.WriteLine(String.Format("{0,-" + receiptWidth + "}", String.Format("{0," + ((receiptWidth + centerText.Length) / 2).ToString() + "}", centerText)));
		}
	}
}