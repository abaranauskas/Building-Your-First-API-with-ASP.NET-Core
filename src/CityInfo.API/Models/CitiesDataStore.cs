using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Models
{
    public class CitiesDataStore
    {
        public static CitiesDataStore Current { get; } = new CitiesDataStore();

        public CitiesDataStore()
        {
            Cities = new List<CityDto>
            {
                new CityDto {
                    Id =1, Name="New York City",
                    Description ="The one with big park",
                    PointsOfInterests = new List<PointsOfInterestsDto>
                    {
                        new PointsOfInterestsDto
                        {
                            Id = 1,
                            Name="Central Park",
                            Description=" Lankomiausia Vieta"
                        },
                        new PointsOfInterestsDto
                        {
                            Id = 2,
                            Name="Central Park",
                            Description="Antra Lankomiausia Vieta"
                        }
                    }
                },
                new CityDto {
                    Id =2,
                    Name ="Antwerp",
                    Description ="The one with unfinished church",
                    PointsOfInterests = new List<PointsOfInterestsDto>
                    {
                        new PointsOfInterestsDto
                        {
                            Id = 1,
                            Name="Tall Bridge",
                            Description="Labai akstas tiltas salia kazkokios upes"
                        },
                        new PointsOfInterestsDto
                        {
                            Id = 2,
                            Name="Unfinished Church",
                            Description="Nebaigta statyti baznycia suvienu bokstu"
                        }
                    }
                },
                new CityDto {Id=3,
                    Name ="Vilnius",
                    Description ="The one with TV Tower",
                    PointsOfInterests = new List<PointsOfInterestsDto>
                    {
                        new PointsOfInterestsDto
                        {
                            Id = 1,
                            Name="Gedimino pilis",
                            Description="Vilniaus pilies komplekso bokstas"
                        },
                        new PointsOfInterestsDto
                        {
                            Id = 2,
                            Name="TV tower",
                            Description="Auksciausias pastatas Vilniuje"
                        }
                    }
                }
            };            
        }

        public List<CityDto> Cities { get; set; }        
    }
}
