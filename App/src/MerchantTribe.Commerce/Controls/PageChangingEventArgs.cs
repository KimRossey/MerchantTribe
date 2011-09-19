
namespace MerchantTribe.Commerce.Controls
{
	public class PageChangingEventArgs
	{

		private int _currentRow;
		private int _rowCount;
		private int _itemsPerPage;

		public int CurrentRow {
			get { return _currentRow; }
			set { _currentRow = value; }
		}

		public int RowCount {
			get { return _rowCount; }
			set { _rowCount = value; }
		}

		public int ItemsPerPage {
			get { return _itemsPerPage; }
			set { _itemsPerPage = value; }
		}

	}
}
