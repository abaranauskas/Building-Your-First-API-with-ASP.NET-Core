using AutoMapper;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Controllers
{
    [Route("api/cities")]
    public class CitiesController: Controller
    {
        ICityInforepository _cityInfoRepository;

        public CitiesController(ICityInforepository cityInfoRepository)
        {
            _cityInfoRepository = cityInfoRepository;
        }

        [HttpGet()]
        public IActionResult GetCities()
        {
            //return Ok(CitiesDataStore.Current.Cities);
            var cityEntities = _cityInfoRepository.GetCities();

            var results = Mapper.Map<IEnumerable<CityWithoutPointsOfInterestDto>>(cityEntities);

            //var results = new List<CityWithoutPointsOfInterestDto>();
            //foreach (var cityEntity in cityEntities)
            //{
            //    results.Add(new CityWithoutPointsOfInterestDto
            //    {
            //        Id = cityEntity.Id,
            //        Name = cityEntity.Name,
            //        Description = cityEntity.Description
            //    });
            //}
            return Ok(results);
        }

        [HttpGet("{id}")]
        public IActionResult GetCity(int id, bool includePointsOfInterest=false)
        {
            var city = _cityInfoRepository.GetCity(id, includePointsOfInterest);
            if (city == null)
            {
                return NotFound();
            }

            if (includePointsOfInterest)
            {
                var cityResult = Mapper.Map<CityDto>(city);
                //var cityResult = new CityDto
                //{
                //    Id = city.Id,
                //    Name = city.Name,
                //    Description = city.Description
                //};

                //foreach (var poi in city.PointsOfInterest)
                //{
                //    cityResult.PointsOfInterests.Add(
                //        new PointsOfInterestsDto()
                //        {
                //            Id = poi.Id,
                //            Name = poi.Name,
                //            Description = poi.Description
                //        });
                //}
                return Ok(cityResult);
            }


            var cityWithouPOIResult  = Mapper.Map<CityWithoutPointsOfInterestDto>(city);
            //var cityWithouPOIResult = new CityWithoutPointsOfInterestDto
            //{
            //    Id = city.Id,
            //    Name = city.Name,
            //    Description = city.Description
            //};
            return Ok(cityWithouPOIResult);


            //var city = CitiesDataStore.Current.Cities.FirstOrDefault(c=>c.Id==id);
            //if (city==null)
            //{
            //    return NotFound();
            //}
            //return Ok(city);
        }
    }
}
