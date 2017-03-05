using System.Data.SQLite;
using GroceryCo.BusinessObjects;
using System;

namespace GroceryCo.Models
{
	public class GroceryCoInventory : IGroceryCoInventory
	{
		private SQLiteConnection databaseConnection;
		private string sqlQuery;
		private SQLiteCommand sqlCommand;
		private SQLiteDataReader sqlReader;
		private static GroceryCoInventory modelInstance;
		
		public static GroceryCoInventory GetInstance(string databaseDirectory)
		{
			if(modelInstance == null)
			{
				modelInstance = new GroceryCoInventory(databaseDirectory);
			}
			return modelInstance;
		}
		private GroceryCoInventory(string databaseFileName)
		{
			databaseConnection = new SQLiteConnection("Data Source=" + databaseFileName + "; Version=3;");
			InitializeDatabase();
		}

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

		public void IncrementStock(Product toIncrement)
		{
			sqlQuery = string.Format("UPDATE Inventory SET Stock = Stock + 1 WHERE UPC = {0}", toIncrement.UPC);
			sqlCommand = new SQLiteCommand(sqlQuery, databaseConnection);
			sqlCommand.ExecuteNonQuery();
		}

		public void DecrementStock(Product toDecrement)
		{
			sqlQuery = string.Format("UPDATE Inventory SET Stock = Stock - 1 WHERE UPC = {0}", toDecrement.UPC);
			sqlCommand = new SQLiteCommand(sqlQuery, databaseConnection);
			sqlCommand.ExecuteNonQuery();
		}

		public void AddToInventory(Product toAdd)
		{
			if (GetProductByUPC(toAdd.UPC) != null)
			{
				sqlQuery = string.Format("UPDATE Inventory SET Stock = Stock + 1 WHERE UPC = {0}", toAdd.UPC);
			}
			else
			{
				sqlQuery = string.Format("INSERT INTO Inventory (UPC, Name, Price, Stock) VALUES ({0}, '{1}', {2}, 1)", toAdd.UPC, toAdd.Name, toAdd.Price);
			}

			sqlCommand = new SQLiteCommand(sqlQuery, databaseConnection);
			sqlCommand.ExecuteNonQuery();
		}

		public void RemoveFromInventory(Product toRemove)
		{
			sqlQuery = "DELETE FROM Inventory WHERE UPC = " + toRemove.UPC;
			sqlCommand = new SQLiteCommand(sqlQuery, databaseConnection);
			sqlCommand.ExecuteNonQuery();
		}

		public void EditFromInventory(Product toEdit)
		{
			sqlQuery = string.Format("UPDATE Inventory SET Name='{0}', Price={1} WHERE UPC = {3}", toEdit.Name, toEdit.Price, toEdit.UPC);
			sqlCommand = new SQLiteCommand(sqlQuery, databaseConnection);
			sqlCommand.ExecuteNonQuery();
		}
		public void PopulateInventory()
		{
			string[] lines = System.IO.File.ReadAllLines(System.IO.Path.GetFullPath(@"Resources\Inventory.txt"));
			foreach (string line in lines)
			{
				string[] tokenizedLine = line.Split();
				Product toAdd = new Product(Convert.ToInt32(tokenizedLine[0]), tokenizedLine[1], Convert.ToInt32(tokenizedLine[2]));
				AddToInventory(toAdd);
			}
		}
	}
}