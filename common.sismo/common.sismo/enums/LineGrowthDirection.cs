using System.ComponentModel;

namespace common.sismo.enums
{
    public enum LineGrowthDirection
    {
        [Description("S-N")]
        SouthToNorth = 1,
        [Description("SO-NE")]
        SouthwestToNortheast = 2,
        [Description("O-E")]
        WestToEast = 3,
        [Description("NO-SE")]
        NorthwestToSoutheast = 4,
        [Description("N-S")]
        NorthToSouth = 5,
        [Description("NE-SO")]
        NortheastToSouthwest = 6,
        [Description("E-O")]
        EastToWest = 7,
        [Description("SE-NO")]
        SoutheastToNorthwest = 8
    }
}
