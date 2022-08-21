using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NorthwindMvc.Models.EmployeeModels
{
    public class EmployeeModel
    {
        [BindProperty]
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Type first name.")]
        [BindProperty, DataType(DataType.Text)]
        [DisplayName("Last name")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Type last name.")]
        [BindProperty, DataType(DataType.Text)]
        [DisplayName("First name")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Type title.")]
        [BindProperty, DataType(DataType.Text)]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Type Address.")]
        [BindProperty, DataType(DataType.Text)]
        public string? Address { get; set; }

        [Required(ErrorMessage = "Type phone number.")]
        [BindProperty, DataType(DataType.PhoneNumber)]
        [DisplayName("Home phone")]
        public string? HomePhone { get; set; }

        [Required(ErrorMessage = "Choose the Birth date")]
        [DisplayName("Birth date")]
        [BindProperty, DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }

        [Required(ErrorMessage = "Choose the Hire date")]
        [DisplayName("Hire date")]
        [BindProperty, DataType(DataType.Date)]
        public DateTime? HireDate { get; set; }

        public byte[] Photo { get; set; }

        [BindProperty]
        public string? PhotoStringValue { get; set; }

        [DataType(DataType.Upload)]
        [DisplayName("Picture")]
        public IFormFile PictureForm { get; set; }
    }
}
