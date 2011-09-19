using System;
using System.Collections;

namespace MerchantTribe.Commerce.Utilities
{
	
	public class SortableCollectionComparer : IComparer
	{

		private string _SortPropertyName = string.Empty;
		private SortDirection _SortDirection = SortDirection.Ascending;

		public SortableCollectionComparer(string propertyName)
		{
			_SortPropertyName = propertyName;
			_SortDirection = SortDirection.Ascending;
		}

		public SortableCollectionComparer(string propertyName, SortDirection direction)
		{
			_SortPropertyName = propertyName;
			_SortDirection = direction;
		}

		int IComparer.Compare(object x, object y)
		{
			object valueOfX = x.GetType().GetProperty(_SortPropertyName).GetValue(x, null);
			object valueOfY = y.GetType().GetProperty(_SortPropertyName).GetValue(y, null);

			IComparable comp = (IComparable)valueOfY;

			return Flip(comp.CompareTo(valueOfX));
		}

		private int Flip(int i)
		{
			return (i * (int)_SortDirection);
		}

	}
}
