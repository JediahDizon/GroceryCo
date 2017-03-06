using GroceryCo.Models;
using GroceryCo.BusinessObjects;
using GroceryCo.Views;

namespace GroceryCo.Controllers
{
	/// <summary>
	/// The <c>Controller</c> class that will be utilized by customers when checking out their items
	/// in the kiosks of GroceryCo. This will allow connectivity between the GroceryCo Inventory and the 
	/// display for the customers.
	/// </summary>
	public class GroceryCoController : IGroceryCoController
	{
		private static GroceryCoView viewInstance;
		private static GroceryCoController controllerInstance;
		private static GroceryCoInventory modelInstance;

		/// <summary>
		/// The <c>GetInstance</c> method is used to prevent multiple instances of this object to exist by 
		/// returning only a single instance of the object and returns the same instance everytime this method is called. 
		/// This will enforce the idea that there's only one instances of View/Controller/Model that exist in memory 
		/// for every single customer using the Kiosk.
		/// </summary>
		/// <returns>The instance of this <c>Controller</c> class.</returns>
		public static GroceryCoController GetInstance()
		{
			if (controllerInstance == null)
			{
				controllerInstance = new GroceryCoController();
			}
			return controllerInstance;
		}

		/// <summary>
		/// The <c>Constructor</c> for this controller class. This instantiates the View for the customer to view, as well
		/// as the Model used to communicate with the database to get product information.
		/// </summary>
		private GroceryCoController()
		{
			viewInstance = GroceryCoView.GetInstance();
			modelInstance = GroceryCoInventory.GetInstance("GroceryCo.Inventory.db");
		}

		/// <summary>
		/// The <c>ShowMainMenu</c> is the starting point for people who will extend from this application. It allows
		/// the user to make various selections on the available options, as well as check out items from a text file.
		/// </summary>
		public void ShowMainMenu()
		{
			int menuChoice;
			do
			{
				menuChoice = System.Convert.ToInt32(viewInstance.PrintMainMenu());
				switch (menuChoice)
				{
					case 1:
						ReadProductList(viewInstance.GetDirectoryInput());
						CheckOut();
						return;

					case 2:
						string toToggle;
						do
						{
							toToggle = viewInstance.PrintPromoMenu(modelInstance.GetAllPromotions());
							if (!toToggle.Equals("0"))
							{
								modelInstance.TogglePromotion(toToggle);
							}
						} while (!toToggle.Equals("0"));
						break;
				}
			} while (menuChoice != 0);
		}

		/// <summary>
		/// The <c>ReadProductList</c> method is used to get the text file that lists the products
		/// being "scanned" by the kiosk and for each of those items, we get the information and add
		/// it to the list of items that is going to be checked out. 
		/// 
		/// Normally, we only should be getting UPC/Bar codes from the
		/// products being scanned, but based on the requirements, the products must be scanned using
		/// the product name.
		/// </summary>
		public void ReadProductList(string productDirectory)
		{
			viewInstance.PrintWelcomeScreen();
			try
			{
				string[] lines = System.IO.File.ReadAllLines(System.IO.Path.GetFullPath(productDirectory));
				foreach (string line in lines)
				{
					Product toScan = GetProductByName(line);
					if (toScan == null)
					{
						viewInstance.PrintProductNotFound(line);
					}
					else
					{
						ScanProductUPC(toScan.UPC);
					}
				}
			}
			catch(System.IO.FileNotFoundException errorEvent)
			{
				//No products will be "scanned".
			}
			
		}

		/// <summary>
		/// The <c>ScanProductUPC</c> is the method that gets called whenever the customer "Scans" an item from
		/// the barcode scanner. Once scanned, the item is added to the list of items to checkout, after all the
		/// promotions has been applied for that specific product.
		/// </summary>
		/// <param name="productUPC">The UPC of the product that was "Scanned" from the kiosk.</param>
		public void ScanProductUPC(int productUPC)
		{
			Product toAdd = modelInstance.GetProductByUPC(productUPC);
			if(toAdd != null)
			{
				modelInstance.AddToCart(toAdd);
				viewInstance.PrintProduct(toAdd);
			}
			else
			{
				viewInstance.PrintProductNotFound(productUPC);
			}
		}

		/// <summary>
		/// The <c>VoidItem</c> method lets the customer to remove items from their checkout items at will. It
		/// also voids the promotion that was applied on the item when it was scanned.
		/// </summary>
		/// <param name="productUPC">The UPC of the product being "Scanned" from the kiosk.</param>
		public void VoidItem(int productUPC)
		{
			Product toRemove = modelInstance.RemoveFromCart(GetProductByUPC(productUPC));
			if(toRemove != null)
			{
				viewInstance.PrintVoidItem(toRemove);
			}
			else
			{
				viewInstance.PrintProductNotFound(productUPC);
			}
		}

		/// <summary>
		/// The <c>CheckOut</c> function retrieves the total of all the products from the <c>Model</c> instance and
		/// then clears out all necessary resources to prepare for the next customer.
		/// </summary>
		/// <returns>The Total of the products that were checked out as cents.</returns>
		public int CheckOut()
		{
			int checkoutTotal = modelInstance.CheckOut();
			viewInstance.PrintTotal(checkoutTotal);
			viewInstance.PrintFarewellScreen();
			return checkoutTotal;
		}

		/// <summary>
		/// The <c>GetProductByUPC</c> method is used to get a <c>Product</c> object from the Database using the
		/// product UPC.
		/// </summary>
		/// <param name="productUPC"></param>
		/// <returns>The Product Object from the Database if it exist. Otherwise, null is returned.</returns>
		public Product GetProductByUPC(int productUPC)
		{
			return modelInstance.GetProductByUPC(productUPC);
		}
		/// <summary>
		/// The <c>GetProductByName</c> method is used to get a <c>Product</c> object from the Database using the
		/// product Name.
		/// </summary>
		/// <param name="productName"></param>
		/// <returns>The Product Object from the Database if it exist. Otherwise, null is returned.</returns>
		public Product GetProductByName(string productName)
		{
			return modelInstance.GetProductByName(productName);
		}

		/// <summary>
		/// The <c>GetAllProducts</c> function gets all the products from the database by invoking a function in 
		/// the <c>Model</c> class. This is normally used when monitoring the stock movements from the company inventory.
		/// </summary>
		/// <returns>An Array of <c>Products</c> from the database.</returns>
		public Product[] GetAllProducts()
		{
			return modelInstance.GetAllProducts();
		}
	}
}