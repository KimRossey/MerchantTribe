using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BVSoftware.AcumaticaTools.Acumatica;

namespace BVSoftware.AcumaticaTools
{
    public class Accounts
    {
        public static List<AccountDescriptor> ListAllTaxClasses(ServiceContext context)
        {
            List<AccountDescriptor> result = new List<AccountDescriptor>();

			TX205500Content schema = context.Gate.TX205500GetSchema();
            try
            {
				TX205500Content[] content = context.Gate.TX205500Submit(new Command[]
				{
					schema.TaxCategory.ServiceCommands.EveryTaxCategoryID,
					schema.TaxCategory.TaxCategoryID,
					schema.TaxCategory.Description,
				});


				foreach (TX205500Content row in content)
                {
                    result.Add(new AccountDescriptor() { Id = row.TaxCategory.TaxCategoryID.Value, Description = row.TaxCategory.Description.Value });
                }
            }
            catch (Exception ex)
            {
                context.Errors.Add(new ServiceError() { Description = ex.Message, ErrorCode = "Account List error" });
            }
            return result;
        }
    }
}
