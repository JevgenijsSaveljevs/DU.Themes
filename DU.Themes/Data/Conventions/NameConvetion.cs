using System.Data.Entity.ModelConfiguration.Conventions;

namespace DU.Themes.Data.Conventions
{
    public class NameConvetion : Convention
    {
        public NameConvetion()
        {
            this.Properties()
                .Where(x => x.Name.EndsWith("Name"))
                .Configure(p => p.HasMaxLength(150));
        }
    }
}