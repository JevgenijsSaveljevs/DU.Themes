using System;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace DU.Themes.Data.Conventions
{
    public class CodeConvention : Convention
    {
        public CodeConvention()
        {
            this.Properties()
                .Where(x => x.Name.EndsWith("Code", StringComparison.OrdinalIgnoreCase))
                .Configure(p => p.HasMaxLength(20));
        }
    }
}