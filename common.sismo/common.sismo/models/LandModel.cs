using NetTopologySuite.Geometries;

namespace common.sismo.models
{
    public class LandModel
    {
        public int LandId { get; set; }
        public string Name { get; set; }
        public Geometry HouseCoordinate { get; set; }
        public Geometry Polygon { get; set; }
        public string Cep { get; set; }
        public string AddressState { get; set; }
        public string AddressCity { get; set; }
        public string AddressNeighborhood { get; set; }
        public string AddressNumber { get; set; }
        public string AddressStreet { get; set; }
        public string LandOwnerCpfCnpj { get; set; }
        public string Observation { get; set; }
        public bool IsActive { get; set; }
    }
}
