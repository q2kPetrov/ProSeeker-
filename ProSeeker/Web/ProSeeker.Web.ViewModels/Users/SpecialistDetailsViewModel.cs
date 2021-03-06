﻿namespace ProSeeker.Web.ViewModels.Users.Specialists
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using Ganss.XSS;
    using ProSeeker.Data.Models;
    using ProSeeker.Services.Mapping;
    using ProSeeker.Web.ViewModels.Categories;
    using ProSeeker.Web.ViewModels.Opinions;
    using ProSeeker.Web.ViewModels.Services;

    public class SpecialistDetailsViewModel : IMapFrom<Specialist_Details>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string AboutMe { get; set; }

        public string SanitezedAboutMe => new HtmlSanitizer().Sanitize(this.AboutMe);

        public string CompanyName { get; set; }

        public string Website { get; set; }

        public string Experience { get; set; }

        public string SanitizedExperience => new HtmlSanitizer().Sanitize(this.Experience);

        public string Qualification { get; set; }

        public string SanitizedQualification => new HtmlSanitizer().Sanitize(this.Qualification);

        public double AverageRating { get; set; }

        public int RatingsCount { get; set; }

        public ICollection<ServiceViewModel> Services { get; set; }

        public virtual ICollection<OpinionViewModel> Opinions { get; set; }

        public virtual CategorySimpleViewModel JobCategory { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Specialist_Details, SpecialistDetailsViewModel>()
                 .ForMember(x => x.AverageRating, opt =>
                   {
                       opt.MapFrom(m => m.Ratings.Average(v => v.Value));
                   })
                 .ForMember(y => y.RatingsCount, opt =>
                  {
                      opt.MapFrom(m => m.Ratings.Count());
                  });
        }
    }
}
