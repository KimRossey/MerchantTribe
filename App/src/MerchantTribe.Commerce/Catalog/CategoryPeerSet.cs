using System.Collections.Generic;
using System;
using System.Data;
using MerchantTribe.CommerceDTO.v1.Catalog;

namespace MerchantTribe.Commerce.Catalog
{
	public class CategoryPeerSet
	{

        private List<Catalog.CategorySnapshot> _Parents = new List<Catalog.CategorySnapshot>();
        private List<Catalog.CategorySnapshot> _Peers = new List<Catalog.CategorySnapshot>();
        private List<Catalog.CategorySnapshot> _Children = new List<Catalog.CategorySnapshot>();

        public List<Catalog.CategorySnapshot> Parents
        {
			get { return _Parents; }
			set { _Parents = value; }
		}
        public List<Catalog.CategorySnapshot> Peers
        {
			get { return _Peers; }
			set { _Peers = value; }
		}
        public List<Catalog.CategorySnapshot> Children
        {
			get { return _Children; }
			set { _Children = value; }
		}

		public CategoryPeerSet()
		{

		}

        public CategoryPeerSetDTO ToDto()
        {
            CategoryPeerSetDTO result = new CategoryPeerSetDTO();

            foreach (CategorySnapshot c in this._Children)
            {
                result.Children.Add(c.ToDto());
            }
            foreach (CategorySnapshot c2 in this._Parents)
            {
                result.Parents.Add(c2.ToDto());
            }
            foreach (CategorySnapshot c3 in this._Peers)
            {
                result.Peers.Add(c3.ToDto());
            }

            return result;
        }

	}
}
