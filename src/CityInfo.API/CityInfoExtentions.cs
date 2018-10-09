using CityInfo.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API
{
    public static class CityInfoExtentions
    {
        public static void EnsureSeedDataForContext(this CityInfoContext context)
        {
            if (context.Cities.Any())
            {
                return;
            }

            var cities = new List<City>
            {
                new City {                    
                    Name ="New York City",
                    Description ="The one with big park",
                    PointsOfInterest = new List<PointOfInterest>
                    {
                        new PointOfInterest
                        {                           
                            Name="Central Park",
                            Description=" Lankomiausia Vieta"
                        },
                        new PointOfInterest
                        {                           
                            Name="Central Park",
                            Description="Antra Lankomiausia Vieta"
                        }
                    }
                },
                new City {                   
                    Name ="Antwerp",
                    Description ="The one with unfinished church",
                    PointsOfInterest = new List<PointOfInterest>
                    {
                        new PointOfInterest
                        {                            
                            Name="Tall Bridge",
                            Description="Labai akstas tiltas salia kazkokios upes"
                        },
                        new PointOfInterest
                        {                           
                            Name="Unfinished Church",
                            Description="Nebaigta statyti baznycia suvienu bokstu"
                        }
                    }
                },
                new City {
                    Name ="Vilnius",
                    Description ="The one with TV Tower",
                    PointsOfInterest = new List<PointOfInterest>
                    {
                        new PointOfInterest
                        {                           
                            Name="Gedimino pilis",
                            Description="Vilniaus pilies komplekso bokstas"
                        },
                        new PointOfInterest
                        {                            
                            Name="TV tower",
                            Description="Auksciausias pastatas Vilniuje"
                        }
                    }
                }
            };

            context.Cities.AddRange(cities);
            context.SaveChanges();
        }
    }
}
