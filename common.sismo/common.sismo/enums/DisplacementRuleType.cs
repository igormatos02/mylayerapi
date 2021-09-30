using System.ComponentModel;

namespace common.sismo.enums
{
    public enum DisplacementRuleType
    {
        [Description("Todos")]
        All = 0,
        [Description("Sísmica")]
        Seismic = 1,
        [Description("Grav")]
        Grav = 2,
        [Description("Mag")]
        Mag = 3,
    }
}
