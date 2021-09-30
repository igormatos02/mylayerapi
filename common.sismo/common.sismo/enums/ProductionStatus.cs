using System.ComponentModel;

namespace common.sismo.enums
{
    public enum ProductionStatus
    {
        [PermitStatusDescription("Não Liberado"),
        TopographyStatus("Não Locado"),
        DrillingStatus("Não Perfurado"),
        ChargingStatus("Não Carregado"),
        SeismoAStatus("Não Espalhado"),
        DetonationStatus("Não Detonado"),
        SeismoBStatus("Não Recolhido"),
        InspectionStatus("Não Recuperado"),
        GravimetryStatus("Não Lido"),
        MagnetometryStatus("Não Lido"),
        Description("NÃO EXECUTADO")]
        Unaccomplished = -1,
        [PermitStatusDescription("Liberado"),
        TopographyStatus("Locado"),
        DrillingStatus("Perfurado"),
        ChargingStatus("Carregado"),
        SeismoAStatus("Espalhado"),
        DetonationStatus("Detonado"),
        SeismoBStatus("Recolhido"),
        InspectionStatus("Recuperado"),
        GravimetryStatus("Lido"),
        MagnetometryStatus("Lido"),
        Description("EXECUTADO")]
        Accomplished = 1,
        [PermitStatusDescription("Liberado Somente ER"),
        Description("Liberado Somente ER")]
        OnlyReceptorStations = 2,
        [DetonationStatus("Pendente"),
        Description("PENDENTE")]
        Pending = 0
    }

    public enum QualityControlStatus
    {
        [Description("Válido")]
        Valid = 1,
        [Description("Redetonar")]
        Redetonate = 2,
        [Description("Retirado pelo CQ")]
        WithdrawnByQc = 3
    }

    public class PermitStatusDescription : DescriptionAttribute
    {
        public PermitStatusDescription(string description)
        {
            DescriptionValue = description;
        }
    }
    public class TopographyStatus : DescriptionAttribute
    {
        public TopographyStatus(string description)
        {
            DescriptionValue = description;
        }
    }
    public class DrillingStatus : DescriptionAttribute
    {
        public DrillingStatus(string description)
        {
            DescriptionValue = description;
        }
    }
    public class ChargingStatus : DescriptionAttribute
    {
        public ChargingStatus(string description)
        {
            DescriptionValue = description;
        }
    }
    public class SeismoAStatus : DescriptionAttribute
    {
        public SeismoAStatus(string description)
        {
            DescriptionValue = description;
        }
    }
    public class DetonationStatus : DescriptionAttribute
    {
        public DetonationStatus(string description)
        {
            DescriptionValue = description;
        }
    }
    public class SeismoBStatus : DescriptionAttribute
    {
        public SeismoBStatus(string description)
        {
            DescriptionValue = description;
        }
    }
    public class InspectionStatus : DescriptionAttribute
    {
        public InspectionStatus(string description)
        {
            DescriptionValue = description;
        }
    }
    public class GravimetryStatus : DescriptionAttribute
    {
        public GravimetryStatus(string description)
        {
            DescriptionValue = description;
        }
    }
    public class MagnetometryStatus : DescriptionAttribute
    {
        public MagnetometryStatus(string description)
        {
            DescriptionValue = description;
        }
    }
}
