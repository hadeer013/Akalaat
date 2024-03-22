using System.ComponentModel.DataAnnotations;
using Akalaat.DAL.Models;


namespace Akalaat.ViewModels;

public class ForgetPasswordVM
{
    [EmailAddress,Display(Name = "Registered Email Address")]
    public string Email { get; set; }
    
    public bool EmailSent { get; set; }
}