using System;

namespace GroceryCo.BusinessObjects
{
	public class Product : IProduct, IEquatable<Product>
	{
		private int productUPC;
		private string productName;
		private int productPrice;
		private int productDiscount;

		public Product(int productUPC = -1, string productName = "", int productPrice = 0, int productDiscount = 0)
		{
			this.productName = productName;
			this.productUPC = productUPC;
			this.productPrice = productPrice;
			this.productDiscount = productDiscount;
		}
		public int UPC
		{
			get
			{
				return productUPC;
			}
			set
			{
				productUPC = value;
			}
		}
		public string Name
		{
			get
			{
				return productName;
			}
			set
			{
				productName = value;
			}
		}
		public int Price
		{
			get
			{
				return productPrice;
			}
			set
			{
				productPrice = value;
			}
		}
		public int Discount
		{
			get
			{
				return productDiscount;
			}
			set
			{
				productDiscount = value;
			}
		}
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			Product toCompare = (Product)obj;
			return UPC == toCompare.UPC && Name.Equals(toCompare.Name) && Price == toCompare.Price;
		}
		public bool Equals(Product toCompare)
		{
			if (toCompare == null)
			{
				return false;
			}
			return UPC == toCompare.UPC && Name.Equals(toCompare.Name) && Price == toCompare.Price;
		}
	}
}
