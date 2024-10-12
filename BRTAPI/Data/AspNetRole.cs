namespace BRTAPI.Data
{
    public class AspNetRole
    {
        public string Id { get; set; } // Assuming Id is of type uniqueidentifier in SQL Server
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public string ConcurrencyStamp { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class AspNetRoleWithUserRole
    {
        public Guid Id { get; set; } // Assuming Id is of type uniqueidentifier in SQL Server
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public string ConcurrencyStamp { get; set; }
        public bool IsDeleted { get; set; } // From AspNetRoles

        public string UserId { get; set; } // Assuming there is a UserId field in AspNetUserRoles
        public string RoleId { get; set; } // Assuming there is a RoleId field in AspNetUserRoles

    }
}
