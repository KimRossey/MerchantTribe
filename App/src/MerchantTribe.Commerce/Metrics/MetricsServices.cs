using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Metrics
{
    public class MetricsServices
    {
        private RequestContext context = null;

        public SearchQueryRepository SearchQueries { get; private set; }

        public static MetricsServices InstantiateForMemory(RequestContext c)
        {
            return new MetricsServices(c,
                                      SearchQueryRepository.InstantiateForMemory(c)                                      
                                      );

        }
        public static MetricsServices InstantiateForDatabase(RequestContext c)
        {
            return new MetricsServices(c,
                                    SearchQueryRepository.InstantiateForDatabase(c)
                                    );
        }
        public MetricsServices(RequestContext c,
                            SearchQueryRepository queries
                            )
        {
            context = c;
            this.SearchQueries = queries;
        }
      
    }
}
