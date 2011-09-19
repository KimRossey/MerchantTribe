using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Web.Data;

namespace MerchantTribe.Commerce.Security
{
    public class FraudScorer
    {
        private FraudRuleRepository _repository = null;
        private RequestContext _context = null;

        public FraudScorer(RequestContext context, FraudRuleRepository repository)
        {
            _repository = repository;
            _context = context;
        }
        public FraudScorer(RequestContext context)
        {
            _repository = new FraudRuleRepository(context);
            _context = context;
        }

        public decimal ScoreData(FraudCheckData data)
        {
            decimal result = 0;

            List<FraudRule> storeRules = _repository.FindForStore(_context.CurrentStore.Id);
            if (storeRules == null) return result;

            foreach (FraudRule rule in storeRules)
            {
                result += ScoreSingleRule(data, rule);
            }

            if (result > 10) result = 10;

            return result;
        }

        private decimal ScoreSingleRule(FraudCheckData data, FraudRule rule)
        {
            decimal result = 0;

            switch (rule.RuleType)
            {
                case FraudRuleType.CreditCardNumber:
                    if (rule.RuleValue == data.CreditCard)
                    {
                        result += 7;
                        data.Messages.Add("Credit Card Matched Fraud Rules");
                    }
                    break;
                case FraudRuleType.DomainName:
                    if (rule.RuleValue == data.DomainName)
                    {
                        result += 3;
                        data.Messages.Add("Domain Matched Fraud Rules");
                    }
                    break;
                case FraudRuleType.EmailAddress:
                    if (rule.RuleValue == data.EmailAddress)
                    {
                        result += 5;
                        data.Messages.Add("Email Address Matched Fraud Rules");
                    }
                    break;
                case FraudRuleType.IPAddress:
                    if (rule.RuleValue == data.IpAddress)
                    {
                        result += 1;
                        data.Messages.Add("IP Address Fraud Rules");
                    }
                    break;
                case FraudRuleType.PhoneNumber:
                    if (rule.RuleValue == data.PhoneNumber)
                    {
                        result += 3;
                        data.Messages.Add("Phone Number Matched Fraud Rules");
                    }
                    break;
            }
            return result;
        }
    }
}
