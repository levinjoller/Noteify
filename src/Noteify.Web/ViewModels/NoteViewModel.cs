using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Noteify.Web.ViewModels
{
    public class NoteViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public Guid Id { get; set; }

        [Display(Name = "Titel")]
        public string Designation { get; set; }

        [Display(Name = "Inhalt")]
        public string Message { get; set; }

        [Display(Name = "Erstellt am")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy hh\\:mm}")]
        public DateTime TimeStamp { get; set; }
    }
}