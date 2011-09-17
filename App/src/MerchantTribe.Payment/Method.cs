using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Payment
{
    public abstract class Method
    {

        public abstract string Name { get; }

        public abstract string Id { get; }        

        public abstract void ProcessTransaction(Transaction t);
        
        public abstract MethodSettings BaseSettings { get; }
        
    }
}
