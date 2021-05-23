using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace _2021_dotnet_g_28.Models.Domain
{
    public class Contract
    {
        #region properties
        public int ContractNr { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "End date")]
        public DateTime EndDate { get; set; }
        
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Start date")]
        public DateTime StartDate { get; set; }
        public ContractEnum.status Status { get; set; }
        [Required]
        [Display(Name ="Contract Type")]
        public ContractType Type { get; set; }
        
        [Required]
        [Display(Name = "Company Info")]
        [JsonIgnore]
        public Company Company { get; set; }

        [NotMapped]
        public int Duration { get { return EndDate.Year - StartDate.Year; } }
        #endregion


        public Contract()
        {
            
        }

        public Contract(ContractType type,int duration,Company company)
        {
            
            if (duration < type.MinDuration) throw new ArgumentException("The duration has to be atleast equal to the min duration of their contractype");
            Type = type;
            Company = company;
            StartDate = DateTime.Now;
            EndDate = DateTime.Now.AddYears(duration);
            Status = ContractEnum.status.InProgress;
            Company = company;
            company.AddContract(this);
        }
    }
}
