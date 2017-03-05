using System.Collections.Generic;

namespace GroceryCo.BusinessObjects
{
	class Promo
	{
		Dictionary<int, int> PurchaseCount;

		public Promo()
		{
			PurchaseCount = new Dictionary<int, int>();
		}

		public void ApplyDiscount(Product toCheck)
		{
			if(!PurchaseCount.ContainsKey(toCheck.UPC))
			{
				PurchaseCount.Add(toCheck.UPC, 0);
			}

			PurchaseCount[toCheck.UPC]++;
			int productUPC = toCheck.UPC;
			switch(productUPC)
			{
				case 111:
					//2nd One is 50% off
					if (PurchaseCount[toCheck.UPC] % 2 == 0)
						toCheck.Discount = toCheck.Price / 2;
					break;

				case 222:
					//Buy 2 Get, Get One Free
					if (PurchaseCount[toCheck.UPC] % 3 == 0)
						toCheck.Discount = toCheck.Price;
					break;
			}
		}
	}
}
