namespace TransportFourthProject.Api.DTOs.Employee
{
    public class EmployeeMeDto
    {
        public int EmployeeId { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string LicenseNumber { get; set; }
        public string Status { get; set; }
        public DateTime HireDate { get; set; }
    }
}
