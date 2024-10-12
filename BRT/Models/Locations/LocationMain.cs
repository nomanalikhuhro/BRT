namespace BRT.Models.Locations
{
    public class LocationMain
    {
    }
    public class BRTMAINLOCATIONS
    {
        public int Id { get; set; }
        public string name { get; set; }
        
        public ICollection<BRTSUBLOCATIONS> SubLocations { get; set; }
    }

    public class BRTSUBLOCATIONS
    {
        public int id { get; set; }
        public string name { get; set; }
        public int Status { get; set; }
        public int MainLocationId { get; set; }
        public BRTMAINLOCATIONS MainLocation { get; set; }
        public virtual ICollection<ContainerLocation> ContainerLocations { get; set; }

    }

    public class ContainerLocation
    {
        public int Id { get; set; }
        public string ContainerNo { get; set; }
      
        public int Prev_locationId { get; set; }
        public int locationId { get; set; }
        public string createdby { get; set; }
        public DateTime createdon { get; set; }
        public string updatedby { get; set; }
        public DateTime? updatedon { get; set; }
        public int? Status { get; set; } = 0;

        public virtual BRTSUBLOCATIONS Location { get; set; }
    }

}
