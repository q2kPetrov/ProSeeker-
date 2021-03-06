﻿namespace ProSeeker.Web.ViewModels.Users
{
    using System;

    using ProSeeker.Data.Models;
    using ProSeeker.Services.Mapping;
    using ProSeeker.Web.ViewModels.Categories;

    public class SimpleSpecialistDetailsViewModel : IMapFrom<Specialist_Details>
    {
        public string Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public virtual CategorySimpleViewModel JobCategory { get; set; }

        public virtual SimpleUserViewModel User { get; set; }
    }
}
