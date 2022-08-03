using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace NorthwindMvc.Models.EmployeeModels
{
    public class EmployeeModel
    {
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Type first name.")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Type last name.")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Type title.")]
        public string? Title { get; set; }

        public string? Address { get; set; }

        public string? HomePhone { get; set; }

        [Required(ErrorMessage = "Choose the Birth date")]
        public DateTime? BirthDate { get; set; }

        [Required(ErrorMessage = "Choose the Hire date")]
        public DateTime? HireDate { get; set; }

        public byte[]? Photo { get; set; }

        [DataType(DataType.Upload)]
        public IFormFile PictureForm { get; set; }
    }
}
