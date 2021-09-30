using System.Collections.Generic;
using System.ComponentModel;

namespace common.sismo.enums
{

    public enum PreplotPointType
    {
        [Description("TODOS OS TIPOS ")]
        All = 0,
        [Description("Ponto de Tiro")]
        ShotPoint = 1,
        [Description("Estação Receptora")]
        ReceiverStation = 2,
        [Description("Ponto de Leitura Magnética")]
        MagnometricStation = 4,
        [Description("Ponto de Leitura Gravimétrica")]
        GravimetricStation = 5,
        [Description("Pontos do Programa sísmico")]
        AllSeismics = 6,
        [Description("Pontos do Programa não sísmico")]
        AllNotSeismics = 7,
    }
    public static class PreplotTypeResolver
    {
        public static List<PreplotPointType> Resolve(PreplotPointType pointType)
        {
            var pointTypes = new List<PreplotPointType>();
            //Strin
            if (pointType == PreplotPointType.AllNotSeismics)
            {
                pointTypes.Add(PreplotPointType.GravimetricStation);
                pointTypes.Add(PreplotPointType.MagnometricStation);
            }
            else if (pointType == PreplotPointType.AllSeismics)
            {
                pointTypes.Add(PreplotPointType.ReceiverStation);
                pointTypes.Add(PreplotPointType.ShotPoint);
            }
            else if (pointType != PreplotPointType.AllNotSeismics && pointType != PreplotPointType.AllSeismics && pointType != PreplotPointType.All)
            {
                pointTypes.Add(pointType);
            }
            else if (pointType == PreplotPointType.All)
            {
                pointTypes.Add(PreplotPointType.ReceiverStation);
                pointTypes.Add(PreplotPointType.ShotPoint);
                pointTypes.Add(PreplotPointType.GravimetricStation);
                pointTypes.Add(PreplotPointType.MagnometricStation);
            }
            return pointTypes;
        }
    }
}
