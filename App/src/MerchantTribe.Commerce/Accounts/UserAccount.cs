using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Web;
using MerchantTribe.Web.Validation;
using MerchantTribe.Web.Cryptography;

namespace MerchantTribe.Commerce.Accounts
{
    public class UserAccount : IValidatable
    {
        public long Id { get; set; }
        public string Email { get; set; }

        private string _hashedPassword = string.Empty;
        public string HashedPassword { 
            get
            {
                return _hashedPassword;
            } 
            set
            {
                // Hash the password only if we have a salt value
                if (this.Salt.Trim() != string.Empty)
                {
                    this._hashedPassword = this.EncryptPassword(value);
                }
                else
                {
                    this._hashedPassword = value;
                }
            } 
        }
        public DateTime DateCreated { get; set; }
        public UserAccountStatus Status { get; set; }
        public string Salt { get; set; }
        public string ResetKey { get; set; }

        public UserAccount()
        {
            Id = -1;
            Email = string.Empty;
            _hashedPassword = string.Empty;
            DateCreated = DateTime.UtcNow;
            Status = UserAccountStatus.Active;
            Salt = string.Empty;
            ResetKey = string.Empty;
        }

        public bool ResetPassword(string resetKey, string newPassword)
        {
            if (resetKey != ResetKey) return false;
            this.HashedPassword = newPassword;
            this.HashPasswordIfNeeded();
            return true;
        }

        public bool DoesPasswordMatch(string trialPassword)
        {
            if (this.Salt == string.Empty)
            {
                return this.HashedPassword.Equals(trialPassword, StringComparison.InvariantCulture);
            }
            else
            {
                string hashedTrial = this.EncryptPassword(trialPassword);
                return this.HashedPassword.Equals(hashedTrial, StringComparison.InvariantCulture);
            }            
        }

        private string EncryptPassword(string password)
        {
            string result = string.Empty;
            result = Hashing.Md5Hash(password, this.Salt);
            return result;
        }

        internal void HashPasswordIfNeeded()
        {
            if (this.Salt.Trim() == string.Empty)
            {
                this.Salt = System.Guid.NewGuid().ToString();
                this._hashedPassword = EncryptPassword(this._hashedPassword);
            }
        }

        #region IValidatable Members

        public bool IsValid()
        {
            if (GetRuleViolations().Count > 0)
            {
                return false;
            }
            return true;
        }

        public List<RuleViolation> GetRuleViolations()
        {
            List<RuleViolation> violations = new List<RuleViolation>();

            ValidationHelper.ValidEmail("Email", Email, violations, "email");
            ValidationHelper.LengthCheck(6, 255, "Password", HashedPassword, violations, "password");

            return violations;
        }

        #endregion
    }
}
