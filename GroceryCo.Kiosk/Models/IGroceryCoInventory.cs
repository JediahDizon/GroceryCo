﻿using GroceryCo.BusinessObjects;
using System.Collections.Generic;

namespace GroceryCo.Models
{
	interface IGroceryCoInventory
	{
		void InitializeDatabase();
		void PopulateInventory();
		Product GetProductByUPC(int productUPC);
		Product GetProductByName(string productName);
		Product[] GetAllProducts();
		Dictionary<string, bool> GetAllPromotions();
		void AddToCart(Product toAdd);
		Product RemoveFromCart(Product toRemove);
		int CheckOut();
		void IncrementStock(Product toIncrement);
		void DecrementStock(Product toDecrement);
		void AddToInventory(Product toAdd);
		void RemoveFromInventory(Product toRemove);
		void EditFromInventory(Product toEdit);
	}
}
