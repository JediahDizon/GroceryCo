using Microsoft.VisualStudio.TestTools.UnitTesting;
using GroceryCo.BusinessObjects;
using GroceryCo.Controllers;
using GroceryCo.Models;

namespace GroceryCo.Test
{
	[TestClass]
	public class UnitTest
	{
		private GroceryCoController controllerInstance;
		private Product toCompare1;
		private Product toCompare2;
		private Product toCompare3;

		[TestInitialize]
		public void TestInitialize()
		{
			controllerInstance = GroceryCoController.GetInstance();
			toCompare1 = new Product(111, "Apple", 75);
			toCompare2 = new Product(222, "Orange", 100);
			toCompare3 = new Product(333, "Banana", 100);
		}

		[TestCleanup]
		public void TestCleanup()
		{
			controllerInstance = null;
			toCompare1 = null;
			toCompare2 = null;
			toCompare3 = null;
		}

		[TestMethod]
		public void GetProductByUPC()
		{
			Assert.IsTrue(toCompare1.Equals(controllerInstance.GetProductByUPC(toCompare1.UPC)));
			Assert.IsTrue(toCompare2.Equals(controllerInstance.GetProductByUPC(toCompare2.UPC)));
			Assert.IsTrue(toCompare3.Equals(controllerInstance.GetProductByUPC(toCompare3.UPC)));
		}

		[TestMethod]
		public void GetAllProducts()
		{
			Product[] allProducts = controllerInstance.GetAllProducts();
			Assert.IsTrue(toCompare1.Equals(allProducts[0]));
			Assert.IsTrue(toCompare2.Equals(allProducts[1]));
			Assert.IsTrue(toCompare3.Equals(allProducts[2]));
		}

		[TestMethod]
		public void ProductNotFound()
		{
			Assert.IsNull(controllerInstance.GetProductByUPC(-1));
		}

		[TestMethod]
		public void RegularPrice()
		{
			Product fromInventory;
			fromInventory = controllerInstance.GetProductByUPC(111);
			Assert.AreEqual(toCompare1.Price, fromInventory.Price);
			fromInventory = controllerInstance.GetProductByUPC(222);
			Assert.AreEqual(toCompare2.Price, fromInventory.Price);
			fromInventory = controllerInstance.GetProductByUPC(333);
			Assert.AreEqual(toCompare3.Price, fromInventory.Price);
		}
	}
}