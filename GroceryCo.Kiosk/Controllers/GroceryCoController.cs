using System.Collections.Generic;
using GroceryCo.Models;
using GroceryCo.BusinessObjects;
using GroceryCo.Views;
using System;

namespace GroceryCo.Controllers
{
	public class GroceryCoController : IGroceryCoController
	{
		private static GroceryCoView viewInstance;
		private static GroceryCoController controllerInstance;
		private static GroceryCoInventory modelInstance;
		private List<Product> productList;
		private Promo productPromo;
		public static GroceryCoController GetInstance()
		{
			if (controllerInstance == null)
			{
				controllerInstance = new GroceryCoController();
			}
			return controllerInstance;
		}

		private GroceryCoController()
		{
			productList = new List<Product>();
			productPromo = new Promo();
			viewInstance = GroceryCoView.GetInstance();
			modelInstance = GroceryCoInventory.GetInstance("GroceryCo.Inventory.db");
		}

		public void ReadProductList()
		{
			string directoryInput = viewInstance.GetDirectoryInput();
			string[] lines = System.IO.File.ReadAllLines(System.IO.Path.GetFullPath(directoryInput));
			viewInstance.PrintWelcomeScreen();
			foreach (string line in lines)
			{
				ScanProductUPC(System.Convert.ToInt32(line));
			}
		}
		public void ScanProductUPC(int productUPC)
		{
			Product toAdd = modelInstance.GetProductByUPC(productUPC);
			if(toAdd != null)
			{
				productPromo.ApplyDiscount(toAdd);
				productList.Add(toAdd);
				viewInstance.PrintProduct(toAdd);
			}
			else
			{
				viewInstance.PrintProductNotFound(productUPC);
			}
		}
		public void VoidItem(int productUPC)
		{
			// Item removal is currently out of the project's scope.
			throw new NotImplementedException();
		}

		public void CheckOut()
		{
			int purchaseTotal = 0;
			foreach(Product toCheckout in productList)
			{
				modelInstance.DecrementStock(toCheckout);
				purchaseTotal += toCheckout.Price;
			}
			viewInstance.PrintTotal(purchaseTotal);
			viewInstance.PrintFarewellScreen();
			productList.Clear();
		}

		public Product GetProductByUPC(int productUPC)
		{
			return modelInstance.GetProductByUPC(productUPC);
		}
		
		public Product[] GetAllProducts()
		{
			return modelInstance.GetAllProducts();
		}

	}
}