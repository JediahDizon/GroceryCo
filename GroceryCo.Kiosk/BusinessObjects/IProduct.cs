namespace GroceryCo.BusinessObjects
{
	interface IProduct
	{
		int UPC
		{
			get;
			set;
		}

		string Name
		{
			get;
			set;
		}
		int Price
		{
			get;
			set;
		}
		int Discount
		{
			get;
			set;
		}
	}
}
