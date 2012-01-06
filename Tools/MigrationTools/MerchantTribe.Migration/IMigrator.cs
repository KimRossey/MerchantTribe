using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Migration
{
    public interface IMigrator
    {
        event MigrationService.ProgressReportDelegate ProgressReport;
        
        void Migrate(MigrationSettings settings);
    }
}
