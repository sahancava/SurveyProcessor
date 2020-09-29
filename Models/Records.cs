using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Anket.Models
{
    public partial class Records
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage= "İsim alanının dolu olması gerekmektedir.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage= "Soyisim alanının dolu olması gerekmektedir.")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Telefon numarası alanının dolu olması gerekmektedir.")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Müşteri firma alanının dolu olması gerekmektedir.")]
        public string CustomerCompany { get; set; }
        public string RecordedUser { get; set; }

    }
}
