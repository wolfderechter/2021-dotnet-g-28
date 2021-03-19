using _2021_dotnet_g_28.Models.Domain;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace _2021_dotnet_g_28.Models.Viewmodels
{
    public class TicketEditViewModel
    {
        //attributes of ticket
        public DateTime DateCreated { get; set; }
        [Required(ErrorMessage = "The title is required")]
        
        public string Title { get; set; }
        // public string Remark { get; set; }
        [Required]
        public string Description { get; set; }
        
        [Required(ErrorMessage = "The type is required")]
        public TicketEnum.type Type { get; set; }
        public string PicturePath { get; set; }
        
        public IFormFile[] Attachments { get; set; }


        public TicketEditViewModel()
        {

        }

        public TicketEditViewModel(Ticket ticket)
        {
            //attributes of ticket from ticket
            Title = ticket.Title;
            //Remark = ticket.Remark;
            Description = ticket.Description;
            Type = ticket.Type;
        }
    }
}
