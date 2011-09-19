using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

namespace MerchantTribe.Commerce.Utilities
{
	
	public class SortableCollection<T> : System.Collections.CollectionBase
	{

		public SortableCollection()
		{

		}

		public SortableCollection(IEnumerable<T> enumerator)
		{
			foreach (T item in enumerator) {
                T temp = item;
				this.Add(temp);
			}
		}

		public void Sort(string sortExpression, SortDirection direction)
		{
			InnerList.Sort(new SortableCollectionComparer(sortExpression, direction));
		}

		public void Sort(string sortExpression)
		{
			InnerList.Sort(new SortableCollectionComparer(sortExpression));
		}

        public T this[int index]
		{
			get { return (T)this.List[index]; }
			set { this.List[index] = value; }
		}

		public virtual int IndexOf(T item)
		{
			return this.List.IndexOf(item);
		}

		public virtual int Add(T item)
		{
			return this.List.Add(item);
		}

		public virtual void Remove(T item)
		{
			this.List.Remove(item);
		}

		public virtual void CopyTo(Array a, int index)
		{
			this.List.CopyTo(a, index);
		}

		public virtual void AddRange(SortableCollection<T> collection)
		{
			this.InnerList.AddRange(collection);
		}

		public virtual void AddRange(T[] collection)
		{
			this.InnerList.AddRange(collection);
		}

		public virtual bool Contains(T item)
		{
			return this.List.Contains(item);
		}

		public virtual void Insert(int index, T item)
		{
			this.List.Insert(index, item);
		}

		public string ToXml()
		{
			string result = string.Empty;

			try {
				StringWriter sw = new StringWriter();
				XmlSerializer xs = new XmlSerializer(this.GetType());
				xs.Serialize(sw, this);
				result = sw.ToString();
			}
			catch (Exception ex) {
				EventLog.LogEvent(ex);
				result = string.Empty;
			}

			return result;
		}

		public static SortableCollection<T> FromXml(string data)
		{
			SortableCollection<T> result = new SortableCollection<T>();

			if (!string.IsNullOrEmpty(data)) {
				try {
					StringReader tr = new StringReader(data);
					XmlSerializer xs = new XmlSerializer(result.GetType());
					result = (SortableCollection<T>)xs.Deserialize(tr);
					if (result == null) {
						result = new SortableCollection<T>();
					}
				}
				catch (Exception ex) {
					EventLog.LogEvent(ex);
					result = new SortableCollection<T>();
				}
			}

			return result;
		}

        public List<T> ToList()
        {
            List<T> result = new List<T>();

            foreach (T item in this.List)
            {
                result.Add(item);
            }
            return result;
        }
	}
}
