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
				return this.productUPC;
			}
			set
			{
				this.productUPC = value;
			}
		}
		public string Name
		{
			get
			{
				return this.productName;
			}
			set
			{
				this.productName = value;
			}
		}
		public int Price
		{
			get
			{
				return this.productPrice;
			}
			set
			{
				this.productPrice = value;
			}
		}
		public int Discount
		{
			get
			{
				return this.productDiscount;
			}
			set
			{
				this.productDiscount = value;
			}
		}

		public bool Equals(Product toCompare)
		{
			if(this.UPC == toCompare.UPC && this.Name.Equals(toCompare.Name) && this.Price == toCompare.Price)
			{
				return true;
			}
			return false;
		}
	}
}
