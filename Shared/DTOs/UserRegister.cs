﻿using System.ComponentModel.DataAnnotations;

namespace BlazorEcommerceApp.Shared.DTOs
{
    public class UserRegister
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, StringLength(100), MinLength(6)]
        public string Password { get; set; } = string.Empty;

        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
