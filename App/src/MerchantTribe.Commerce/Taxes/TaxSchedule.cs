using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Web;
using MerchantTribe.Web.Validation;
using MerchantTribe.Web.Geography;
using MerchantTribe.CommerceDTO.v1.Taxes;

namespace MerchantTribe.Commerce.Taxes
{
    public class TaxSchedule : IValidatable, ITaxSchedule
    {

        public long Id { get; set; }
        public long StoreId { get; set; }
        public string Name { get; set; }
        //public ITaxRateRepository RateRepository { get; set; }
        public TaxSchedule()
        {
            Id = 0;
            StoreId = 0;
            Name = string.Empty;
            //RateRepository = new TaxManager();
        }

        //public TaxSchedule(ITaxRateRepository repository)
        //{
        //    RateRepository = repository;
        //}

        #region IValidatable Members

        public bool IsValid()
        {
            if (GetRuleViolations().Count > 0)
            {
                return false;
            }
            return true;
        }

        public List<MerchantTribe.Web.Validation.RuleViolation> GetRuleViolations()
        {
            List<MerchantTribe.Web.Validation.RuleViolation> violations = new List<MerchantTribe.Web.Validation.RuleViolation>();

            MerchantTribe.Web.Validation.ValidationHelper.Required("Name", Name, violations, "schedulename");

            // Check name already exists
            /*MerchantTribe.Web.Validation.ValidationHelper.ValidateFalse(taxService.TaxSchedules.NameExists(this.Name, this.Id, StoreId),
                                                                            "A schedule with that name already exists",
                                                                            "Name", Name, violations, "schedulename");
            */
            return violations;
        }

        #endregion

        #region ITaxSchedule Members

        public long TaxScheduleId()
        {
            return this.Id;
        }

        public string TaxScheduleName()
        {
            return this.Name;
        }

        

        #endregion

        //DTO
        public TaxScheduleDTO ToDto()
        {
            TaxScheduleDTO dto = new TaxScheduleDTO();

            dto.Id = this.Id;
            dto.Name = this.Name;
            dto.StoreId = this.StoreId;

            return dto;
        }
        public void FromDto(TaxScheduleDTO dto)
        {
            if (dto == null) return;

            this.Id = dto.Id;
            this.Name = dto.Name;
            this.StoreId = dto.StoreId;
        }
    }
}
