using System.ComponentModel.DataAnnotations;

namespace SportsStore.Domain.Entities
{
    public class ShippingDetails
    {
        [Required(ErrorMessage = "Please enter name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter the first address line")]
        [Display(Name = "Address Line 1")]
        public string AddressLine1 { get; set; }
        [Display(Name = "Address Line 2")]
        public string AddressLine2 { get; set; }
        
        [Required(ErrorMessage = "Please enter city name")]
        public string City { get; set; }

        public string PostalCode { get; set; }

        [Required(ErrorMessage = "Please enter country name")]
        public string Country { get; set; }

        public bool GiftWrap { get; set; }
    }
}
