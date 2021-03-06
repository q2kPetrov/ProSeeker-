﻿namespace ProSeeker.Web.ViewModels.Cities
{
    using System.ComponentModel.DataAnnotations;

    using ProSeeker.Data.Models;
    using ProSeeker.Services.Mapping;

    public class CitySimpleViewModel : IMapFrom<City>
    {
        [Required(ErrorMessage = "Моля, изберете град от падащото меню!")]
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
