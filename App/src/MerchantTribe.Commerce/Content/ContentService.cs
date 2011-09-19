using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Content
{
    public class ContentService
    {
        private RequestContext context = null;

        public HtmlTemplateRepository HtmlTemplates { get; private set; }
        public PolicyRepository Policies { get; private set; }
        public CustomUrlRepository CustomUrls { get; private set; }
        public ContentColumnRepository Columns { get; private set; }

        public static ContentService InstantiateForMemory(RequestContext c)
        {
            return new ContentService(c,
                                      HtmlTemplateRepository.InstantiateForMemory(c),
                                      PolicyRepository.InstantiateForMemory(c),
                                      CustomUrlRepository.InstantiateForMemory(c),
                                      ContentColumnRepository.InstantiateForMemory(c)
                                      );

        }
        public static ContentService InstantiateForDatabase(RequestContext c)
        {
            return new ContentService(c, 
                                    HtmlTemplateRepository.InstantiateForDatabase(c),
                                    PolicyRepository.InstantiateForDatabase(c),
                                    CustomUrlRepository.InstantiateForDatabase(c),
                                    ContentColumnRepository.InstantiateForDatabase(c)
                                    );
        }
        public ContentService(RequestContext c, 
                            HtmlTemplateRepository templates,
                            PolicyRepository policies,
                            CustomUrlRepository customUrls,
                            ContentColumnRepository cols)
        {
            context = c;
            HtmlTemplates = templates;
            this.Policies = policies;
            this.CustomUrls = customUrls;
            this.Columns = cols;
        }

        public List<HtmlTemplate> GetAllTemplatesForStoreOrDefaults()
        {
            List<HtmlTemplate> templates = HtmlTemplates.FindAll();
            
            CheckForType(HtmlTemplateType.ContactFormToAdmin, templates);
            CheckForType(HtmlTemplateType.DropShippingNotice, templates);
            CheckForType(HtmlTemplateType.EmailFriend, templates);
            CheckForType(HtmlTemplateType.ForgotPassword, templates);
            CheckForType(HtmlTemplateType.NewOrder, templates);
            CheckForType(HtmlTemplateType.NewOrderForAdmin, templates);
            CheckForType(HtmlTemplateType.OrderShipment, templates);
            CheckForType(HtmlTemplateType.OrderUpdated, templates);

            return templates;
        }
        public HtmlTemplate GetHtmlTemplateOrDefault(HtmlTemplateType templateType)
        {
            HtmlTemplate existing = HtmlTemplates.FindByStoreAndType(context.CurrentStore.Id, templateType);

            if (existing == null)
            {
                HtmlTemplate standard = GetDefaultTemplate(templateType);

                if (standard != null)
                {
                    standard.Id = 0;
                    standard.StoreId = context.CurrentStore.Id;
                    HtmlTemplates.Create(standard);
                }

                return standard;
            }
            
            return existing;
        }
        private void CheckForType(HtmlTemplateType checkType, List<HtmlTemplate> existing)
        {
            var existCount = existing.Where(y => y.TemplateType == checkType).Count();
            if (existCount < 1)
            {
                HtmlTemplate standard = GetDefaultTemplate(checkType);
                if (standard != null)
                {
                    standard.Id = 0;
                    standard.StoreId = context.CurrentStore.Id;
                    HtmlTemplates.Create(standard);
                    existing.Add(standard);
                }
            }
        }
        private HtmlTemplate GetDefaultTemplate(HtmlTemplateType templateType)
        {
            return HtmlTemplates.FindByStoreAndType(0, templateType);
        }

    }
}
