﻿using System;
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
        [Required]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        public int ContractNr { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        public ContractEnum.status Status { get; set; }
        [Required]
        public ContractType Type { get; set; }
        [Required]
        public Company Company { get; set; }
        [NotMapped]
        public int Duration { get { return EndDate.Year - StartDate.Year; } }
        #endregion


        public Contract()
        {
            
        }

        public Contract(ContractType type,int Duration,Company company)
        {
            Type = type;
            Company = company;
            StartDate = DateTime.Now;
            EndDate = DateTime.Now.AddYears(Duration);
            Status = ContractEnum.status.InProgress;
            Company = company;
            company.AddContract(this);
        }
    }
}
