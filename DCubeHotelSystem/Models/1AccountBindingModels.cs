// Decompiled with JetBrains decompiler
// Type: DCubeHotelSystem.Models.ChangePasswordBindingModel
// Assembly: DCubeHotelSystem, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D65FBD1C-8A72-4F10-8253-AF378855DBF4
// Assembly location: D:\DLL\DCubeHotelSystem.dll

using System.ComponentModel.DataAnnotations;

namespace DCubeHotelSystem.Models
{
  public class ChangePasswordBindingModel
  {
    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Current password")]
    public string OldPassword { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "New password")]
    public string NewPassword { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm new password")]
    [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; }
  }
}
