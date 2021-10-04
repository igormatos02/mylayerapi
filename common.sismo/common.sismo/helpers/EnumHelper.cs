using common.sismo.enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace common.sismo.helpers
{
    public static class EnumHelper
    {
        public static string GetEnumDescription(Enum value)
        {
            var fi = value.GetType().GetField(value.ToString());

            var attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }

        public static string GetOperationalFrontEnumDescription(Enum value, OperationalFrontType operationalFrontType)
        {
            var fi = value.GetType().GetField(value.ToString());
            var classType = typeof(DescriptionAttribute);
            switch (operationalFrontType)
            {
                case OperationalFrontType.Permit:
                    classType = typeof(PermitStatusDescription);
                    break;
                case OperationalFrontType.Topography:
                    classType = typeof(TopographyStatus);
                    break;
                case OperationalFrontType.Drilling:
                    classType = typeof(DrillingStatus);
                    break;
                case OperationalFrontType.Charging:
                    classType = typeof(ChargingStatus);
                    break;
                case OperationalFrontType.SeismoA:
                    classType = typeof(SeismoAStatus);
                    break;
                case OperationalFrontType.Detonation:
                    classType = typeof(DetonationStatus);
                    break;
                case OperationalFrontType.SeismoB:
                    classType = typeof(SeismoBStatus);
                    break;
                case OperationalFrontType.Inspection:
                    classType = typeof(InspectionStatus);
                    break;
                //case OperationalFrontType.QualityControl:
                //    classType = typeof(QualityControlStatus);
                //    break;
                case OperationalFrontType.Gravimetry:
                    classType = typeof(GravimetryStatus);
                    break;
                case OperationalFrontType.Magnetometry:
                    classType = typeof(MagnetometryStatus);
                    break;
            }
            var attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(classType, false);

            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }

        public static IEnumerable<object> ListEnumDescriptions(Type enumType)
        {
            return from Enum n in Enum.GetValues(enumType)
                   select GetEnumDescription(n);
        }

        public static object ListEnumObjectsToScreen(Type enumType)
        {
            return from Enum n in Enum.GetValues(enumType)
                   select new
                   {
                       Value = n,
                       Description = GetEnumDescription(n)
                   };
        }

        public static object ListProductionStatusEnumObjectsToScreen(Type enumType, OperationalFrontType operationalFrontType)
        {
            return from Enum n in Enum.GetValues(enumType)
                   select new
                   {
                       Value = n,
                       Description = GetOperationalFrontEnumDescription(n, operationalFrontType)
                   };
        }
    }
}
