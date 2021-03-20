using System.ComponentModel.DataAnnotations;
using _2021_dotnet_g_28
namespace _2021_dotnet_g_28.Models.viewmodels
{
    public class LoginViewModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "rememberMe", ResourceType = typeof(Resources.Models.Viewmodels.LoginViewModel))]
        public bool RememberMe { get; set; }


    }

}
