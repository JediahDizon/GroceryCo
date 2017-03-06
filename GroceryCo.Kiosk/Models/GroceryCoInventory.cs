using System.Data.SQLite;
using GroceryCo.BusinessObjects;
using System;
using System.Collections;
using System.IO;

namespace GroceryCo.Models
{
	/// <summary>
	/// The <c>GroceryCoInventory</c> class it the "Model" of the system that connects to the database
	/// and searches for and creates <c>Product</c> objects depending on the parameters of the invoker.
	/// </summary>
	class GroceryCoInventory : IGroceryCoInventory
	{
		private static GroceryCoInventory modelInstance;
		private ArrayList productList;
		private Promo productPromo;

		private SQLiteConnection databaseConnection;
		private string sqlQuery;
		private SQLiteCommand sqlCommand;
		private SQLiteDataReader sqlReader;

		/// <summary>
		/// The <c>GetInstane</c> method enforces a singleton pattern that prevents multiple connection
		/// to the database on a single Kiosk.
		/// </summary>
		/// <param name="databaseDirectory"></param>
		/// <returns>The instance of this <c>Model</c> class.</returns>
		public static GroceryCoInventory GetInstance(string databaseDirectory)
		{
			if(modelInstance == null)
			{
				modelInstance = new GroceryCoInventory(databaseDirectory);
			}
			return modelInstance;
		}

		/// <summary>
		/// The <c>Constructor</c> for this model class. This instantiates the list that will contain the products
		/// that were scanned by the customer, as well as the promotion that keeps track of which product qualifies
		/// for a sale.
		/// </summary>
		/// <param name="databaseFileName">The Database file name specified by the <c>Controller</c> class.</param>
		private GroceryCoInventory(string databaseFileName)
		{
			productList = new ArrayList();
			productPromo = new Promo();
			databaseConnection = new SQLiteConnection("Data Source=" + databaseFileName + "; Version=3;");
			InitializeDatabase();
		}

		/// <summary>
		/// The <c>InitializeDatabase</c> method will initialize the database by firstly checking if the inventory database exist.
		/// If it does exist, this class will determine if the inventory already contains data.
		/// If it doesn't contain data, it will call the <c>PopulateDatabase</c> method that will populate it with pre-determined data
		/// from a text file
		/// </summary>
		public void InitializeDatabase()
		{
			databaseConnection.Open();

			sqlQuery = "SELECT COUNT(name) FROM sqlite_master WHERE type='table' AND name='Inventory'";
			sqlCommand = new SQLiteCommand(sqlQuery, databaseConnection);
			sqlReader = sqlCommand.ExecuteReader();
			sqlReader.Read();
			if (sqlReader.GetInt16(0) != 0)
			{
				sqlQuery = "SELECT COUNT(*) FROM Inventory";
				sqlCommand = new SQLiteCommand(sqlQuery, databaseConnection);
				sqlReader = sqlCommand.ExecuteReader();
				sqlReader.Read();
				if (sqlReader.GetInt32(0) > 0)
				{
					//Table already contains data
					return;
				}
			}
			else
			{
				//No Table
				//Create New Table
				sqlQuery = "CREATE TABLE Inventory ( UPC INTEGER PRIMARY KEY, Name VARCHAR(25) NOT NULL, Price int NOT NULL, Stock int AUTO INCREMENT NOT NULL)";
				sqlCommand = new SQLiteCommand(sqlQuery, databaseConnection);
				sqlCommand.ExecuteNonQuery();
			}
			//Table doesn't have data
			//Add New Data
			PopulateInventory();
		}

		/// <summary>
		/// The <c>PopulateInventory</c> method will populate the inventory database based off of the text file located in the directory
		/// "Inventory.txt" from the root of this program. If it doesn't exist, then the Inventory is left empty. The Controller class however, 
		/// will be able to add products to the database by invoking the <c>AddToInventory</c> method.
		/// </summary>
		public void PopulateInventory()
		{
			try
			{
				string[] lines = System.IO.File.ReadAllLines(System.IO.Path.GetFullPath(@"Inventory.txt"));
				foreach (string line in lines)
				{
					string[] tokenizedLine = line.Split();
					Product toAdd = new Product(Convert.ToInt32(tokenizedLine[0]), tokenizedLine[1], Convert.ToInt32(tokenizedLine[2]));
					AddToInventory(toAdd);
				}
			}
			catch(FileNotFoundException errorEvent)
			{
				//File Not Found
				//Leave Database Empty
			}
		}

		/// <summary>
		/// The <c>GetProductUPC</c> method is used to get a matching <c>Product</c> object based off of the UPC that was passed in.
		/// </summary>
		/// <param name="productUPC">The UPC of the <c>Product</c> being looked up.</param>
		/// <returns>The <c>Product</c> object that was found from the database. Otherwise, a null is returned.</returns>
		public Product GetProductByUPC(int productUPC)
		{
			sqlQuery = string.Format("SELECT UPC, Name, Price FROM Inventory WHERE UPC = {0}", productUPC);
			sqlCommand = new SQLiteCommand(sqlQuery, databaseConnection);
			sqlReader = sqlCommand.ExecuteReader();
			if (sqlReader.HasRows)
			{
				sqlReader.Read();
				return new Product(sqlReader.GetInt32(0), sqlReader.GetString(1), sqlReader.GetInt32(2));
			}
			return null;
		}

		/// <summary>
		/// The <c>GetProductName</c> method is used to get a matching <c>Product</c> object based off of the name that was passed in.
		/// </summary>
		/// <param name="productName">The String representation of the <c>Product Name</c> being looked up.</param>
		/// <returns></returns>
		public Product GetProductByName(string productName)
		{
			sqlQuery = string.Format("SELECT UPC, Name, Price FROM Inventory WHERE Name = '{0}'", productName);
			sqlCommand = new SQLiteCommand(sqlQuery, databaseConnection);
			sqlReader = sqlCommand.ExecuteReader();
			if (sqlReader.HasRows)
			{
				sqlReader.Read();
				return new Product(sqlReader.GetInt32(0), sqlReader.GetString(1), sqlReader.GetInt32(2));
			}
			return null;
		}
		
		/// <summary>
		/// The <c>GetAllProducts</c> function returns a list of all the products used in the Database.
		/// This is not normally used on a normal checkout process with customers, instead, it's for managers
		/// that will monitor the products. This was created for scalability in the future.
		/// </summary>
		/// <returns>An Array of <c>Products</c> from the database.</returns>
		public Product[] GetAllProducts()
		{
			Product[] toReturn;
			sqlQuery = string.Format("SELECT COUNT(*) FROM Inventory");
			sqlCommand = new SQLiteCommand(sqlQuery, databaseConnection);
			sqlReader = sqlCommand.ExecuteReader();
			if (sqlReader.HasRows)
			{
				sqlReader.Read();
				toReturn = new Product[sqlReader.GetInt32(0)];

				sqlQuery = string.Format("SELECT * FROM Inventory");
				sqlCommand = new SQLiteCommand(sqlQuery, databaseConnection);
				sqlReader = sqlCommand.ExecuteReader();

				int i = 0;
				while(sqlReader.Read())
				{
					toReturn[i] = new Product(sqlReader.GetInt32(0), sqlReader.GetString(1), sqlReader.GetInt32(2));
					i++;
				}
				return toReturn;
			}
			else
			{
				return toReturn = new Product[0];
			}
		}

		/// <summary>
		/// The <c>AddToCart</c> method will add a <c>Product</c> to the list of items the customer will checkout in their cart.
		/// </summary>
		/// <param name="toAdd">The <c>Product</c> to add to the list.</param>
		public void AddToCart(Product toAdd)
		{
			productList.Add(toAdd);
			productPromo.ApplyDiscount(toAdd);
		}

		/// <summary>
		/// The <c>ToRemove</c> function will remove the product from the cart and then return the Product that was removed. This way, customers
		/// can remove an item that they may have accidentally scanned, while preventing people from voiding an item that weren't in their cart
		/// in the first place.
		/// </summary>
		/// <param name="toRemove">The <c>Product</c> to remove.</param>
		/// <returns>The <c>Product</c> that was removed. Otherwise, it returns a null.</returns>
		public Product RemoveFromCart(Product toRemove)
		{
			int toRemoveIndex = productList.LastIndexOf(toRemove);
			if (toRemoveIndex >= 0)
			{
				Product toReturn = (Product) productList[toRemoveIndex];
				productPromo.RemoveDiscount(toRemove);
				productList.RemoveAt(toRemoveIndex);
				return toReturn;
			}
			return null;
		}

		/// <summary>
		/// The <c>CheckOut</c> method is what finilizes the transaction with the customer. It calculates the
		/// total of every product that was scanned and prints it out using the <c>View</c> and neatly prints
		/// out the "Farewell" screen to politely thank the customer for shopping at GroceryCo.
		/// 
		/// The List of products that was checked out was negated from the Inventory database by decrementing the count
		/// field of the product in question. Afterwards, the product list is cleared out to prepare for the next customer.
		/// </summary>
		public int CheckOut()
		{
			int purchaseTotal = 0;
			foreach (Product toCheckout in productList)
			{
				DecrementStock(toCheckout);
				purchaseTotal += toCheckout.Price - toCheckout.Discount;
			}
			productList.Clear();
			productPromo.Clear();
			return purchaseTotal;
		}

		/// <summary>
		/// The <c>Increment</c> method will increment the count of the product in the inventory database. This is normally called whenever
		/// there's a stock-movement from the inventory. Possibly, a returned product or a re-stocking of products.
		/// </summary>
		/// <param name="toIncrement">The <c>Product</c> used to find the matching product from the inventory database.</param>
		public void IncrementStock(Product toIncrement)
		{
			sqlQuery = string.Format("UPDATE Inventory SET Stock = Stock + 1 WHERE UPC = {0}", toIncrement.UPC);
			sqlCommand = new SQLiteCommand(sqlQuery, databaseConnection);
			sqlCommand.ExecuteNonQuery();
		}

		/// <summary>
		/// The <c>Decrement</c> method will decrement the count of the product in the inventory database. Just like the
		/// <c>IncrementStock</c> method, this is normally called whenever there's a stock-movement in the inventory database, as well as
		/// whenever a customer checksout their items.
		/// </summary>
		/// <param name="toDecrement">The <c>Product</c> used to find the matching product from the inventory database.</param>
		public void DecrementStock(Product toDecrement)
		{
			sqlQuery = string.Format("UPDATE Inventory SET Stock = Stock - 1 WHERE UPC = {0}", toDecrement.UPC);
			sqlCommand = new SQLiteCommand(sqlQuery, databaseConnection);
			sqlCommand.ExecuteNonQuery();
		}

		/// <summary>
		/// The <c>AddToInventory</c> method will let store managers and their superiors to add a non-existing product from the database.
		/// </summary>
		/// <param name="toAdd">The <c>Product</c> to add to the database.</param>
		public void AddToInventory(Product toAdd)
		{
			if (GetProductByUPC(toAdd.UPC) == null)
			{
				sqlQuery = string.Format("INSERT INTO Inventory (UPC, Name, Price, Stock) VALUES ({0}, '{1}', {2}, 1)", toAdd.UPC, toAdd.Name, toAdd.Price);
				sqlCommand = new SQLiteCommand(sqlQuery, databaseConnection);
				sqlCommand.ExecuteNonQuery();
			}
		}

		/// <summary>
		/// The <c>RemoveFromInventory</c> will remove a product from the inventory. Used most often whenever there's a re-call of a product and
		/// needs to be pulled out of the inventory.
		/// </summary>
		/// <param name="toRemove">The <c>Product</c> to remove from the inventory database.</param>
		public void RemoveFromInventory(Product toRemove)
		{
			sqlQuery = "DELETE FROM Inventory WHERE UPC = " + toRemove.UPC;
			sqlCommand = new SQLiteCommand(sqlQuery, databaseConnection);
			sqlCommand.ExecuteNonQuery();
		}

		/// <summary>
		/// The <c>EditFromInventory</c> will overwrite the existing product information from the database with the new information
		/// that was passed in as a parameter to this method.
		/// </summary>
		/// <param name="toEdit">The <c>Product</c> used to look up the corresponding product from the inventory database.</param>
		public void EditFromInventory(Product toEdit)
		{
			sqlQuery = string.Format("UPDATE Inventory SET Name='{0}', Price={1} WHERE UPC = {3}", toEdit.Name, toEdit.Price, toEdit.UPC);
			sqlCommand = new SQLiteCommand(sqlQuery, databaseConnection);
			sqlCommand.ExecuteNonQuery();
		}
	}
}