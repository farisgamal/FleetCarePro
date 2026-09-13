using Microsoft.AspNetCore.Identity;

namespace MVCDemo.Models
{
    // Module 1: Identity Setup -> extending IdentityUser with FullName + EmployeeId
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
        public string EmployeeId { get; set; } = string.Empty;

        // Vehicle.AssignedDriverId -> ApplicationUser (a Driver can be assigned many vehicles)
        public List<Vehicle>? AssignedVehicles { get; set; }
    }
}
