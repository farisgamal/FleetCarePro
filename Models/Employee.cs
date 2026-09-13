using System.ComponentModel.DataAnnotations;

namespace MVCDemo.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string Name { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        public int Age { get; set; }

        public int DeptId { get; set; }
        public Department? Department { get; set; }
    }
}
