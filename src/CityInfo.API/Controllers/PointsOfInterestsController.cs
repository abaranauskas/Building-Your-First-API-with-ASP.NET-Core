using AutoMapper;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Controllers
{
    [Route("api/cities")]
    public class PointsOfInterestsController : Controller
    {
        private ILogger<PointsOfInterestsController> _logger;
        private IMailService _mailService;
        private ICityInforepository _cityInfoRepository;

        public PointsOfInterestsController(ILogger<PointsOfInterestsController> logger,
            IMailService mailService, ICityInforepository cityInfoRepository)
        {
            _logger = logger;
            _mailService = mailService;
            _cityInfoRepository = cityInfoRepository;
        }

        [HttpGet("{cityId}/pointsofinterests")]
        public IActionResult GetPointsOfInterest(int cityId)
        {
            try
            {
                if (!_cityInfoRepository.CityExist(cityId))
                {
                    _logger.LogInformation($"City with Id {cityId} was not found when accessing points of interests");
                    return NotFound();
                }
                //throw new Exception("sample exception");
                // var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
                var pointsOfInterestForCity = _cityInfoRepository.GetPointsOfInterestForCity(cityId);

                var pointsOfInterestForCityResults = new List<PointsOfInterestsDto>();
                
                foreach (var poi in pointsOfInterestForCity)
                {
                    pointsOfInterestForCityResults.Add(
                        new PointsOfInterestsDto
                        {
                            Id = poi.Id,
                            Name = poi.Name,
                            Description = poi.Description
                        });
                }
                return Ok(pointsOfInterestForCityResults);

                //if (city == null)
                //{
                //    _logger.LogInformation($"City with Id {cityId} was not found when accessing points of interests");
                //    return NotFound();
                //}
                //return Ok(city.PointsOfInterests);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exeption while getting points of interest for city woth id {cityId}", ex);
                return StatusCode(500, "A problem happened while handling your request.");
            }


        }


        [HttpGet("{cityId}/pointsofinterests/{id}", Name = "GetPointOfInterest")]
        public IActionResult GetPointOfInterest(int cityId, int id)
        {
            if (!_cityInfoRepository.CityExist(cityId))
            {
               return NotFound();
            }

            var pointofInterest = _cityInfoRepository.GetPointOfInterestForCity(cityId, id);
            
            if (pointofInterest==null)
            {
                return NotFound();
            }

            var pointofInterestResult = new PointsOfInterestsDto()
            {
                Id = pointofInterest.Id,
                Name = pointofInterest.Name,
                Description = pointofInterest.Description
            };

            return Ok(pointofInterestResult);

            //var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            //if (city == null)
            //{
            //    return NotFound();
            //}

            //var interest = city.PointsOfInterests.FirstOrDefault(i => i.Id == id);
            //if (interest == null)
            //{
            //    return NotFound();
            //}
            //return Ok(interest);
        }

        [HttpPost("{cityId}/pointsofinterests")]
        public IActionResult CreatePointOfInterest(int cityId,
            [FromBody] PointOfInterestsCreationDto pointOfInterest)
        {
            if (pointOfInterest == null)
            {
                return BadRequest();
            }

            if (pointOfInterest.Description == pointOfInterest.Name)
            {
                ModelState.AddModelError("Decription", "Description must be different from name!");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            //if (city == null)
            if (!_cityInfoRepository.CityExist(cityId))
            {
                return NotFound();
            }

            //var maxPointOfInterestId = CitiesDataStore.Current.Cities
            //                            .SelectMany(c => c.PointsOfInterests)
            //                            .Max(i => i.Id);

            //var finalPointOfInterest = new PointsOfInterestsDto
            //{
            //    Id = ++maxPointOfInterestId,
            //    Name = pointOfInterest.Name,
            //    Description = pointOfInterest.Description
            //};

            //city.PointsOfInterests.Add(finalPointOfInterest);

            var finalPointOfInterest = Mapper.Map<Entities.PointOfInterest>(pointOfInterest);

            _cityInfoRepository.AddPointOfInterestForCity(cityId, finalPointOfInterest);

            if (!_cityInfoRepository.Save())
            {
                return StatusCode(500, "Something went wrog");
            }

            var createdPointOfInterestToReturn = Mapper.Map<Models.PointsOfInterestsDto>(finalPointOfInterest);


            return CreatedAtRoute("GetPointOfInterest",
                new { cityId = cityId, id = createdPointOfInterestToReturn.Id }, createdPointOfInterestToReturn);
            //return CreatedAtRoute("GetPointOfInterest",
            //    new { cityId = city.Id, id = finalPointOfInterest.Id }, finalPointOfInterest);
        }

        [HttpPut("{cityId}/pointsofinterests/{id}")]
        public IActionResult UpdatePointOfInterest(int cityId, int id,
            [FromBody] PointOfInterestUpdateDto pointOfInterest)
        {
            if (pointOfInterest == null)
            {
                return BadRequest();
            }

            if (pointOfInterest.Description == pointOfInterest.Name)
            {
                ModelState.AddModelError("Decription", "Description must be different from name!");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            //var interest = city.PointsOfInterests.FirstOrDefault(i => i.Id == id);
            var interest = _cityInfoRepository.GetPointOfInterestForCity(cityId, id);
            if (!_cityInfoRepository.CityExist(cityId) || interest == null)
            {
                return NotFound();
            }

            Mapper.Map(pointOfInterest, interest);

            if (!_cityInfoRepository.Save())
            {
                return StatusCode(500, "Something went wrog");
            }

            //interest.Name = pointOfInterest.Name;
            //interest.Description = pointOfInterest.Description;

            return NoContent();
        }

        [HttpPatch("{cityId}/pointsofinterests/{id}")]
        public IActionResult PartillyUpdatePointOfInterest(int cityId, int id,
            [FromBody] JsonPatchDocument<PointOfInterestUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            //var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            var interest = _cityInfoRepository.GetPointOfInterestForCity(cityId, id);
            if (_cityInfoRepository.CityExist(cityId) || interest == null)
            {
                return NotFound();
            }

            var pointofInterestToPatch =
                new PointOfInterestUpdateDto()
                {
                    Name = interest.Name,
                    Description = interest.Description
                };

            patchDoc.ApplyTo(pointofInterestToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (pointofInterestToPatch.Description == pointofInterestToPatch.Name)
            {
                ModelState.AddModelError("Decription", "Description must be different from name!");
            }

            TryValidateModel(pointofInterestToPatch);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            interest.Name = pointofInterestToPatch.Name;
            interest.Description = pointofInterestToPatch.Description;


            return NoContent();
        }

        [HttpDelete("{cityId}/pointsofinterests/{id}")]
        public IActionResult DeletePointOfInterest(int cityId, int id)
        {
            //var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            var interest = _cityInfoRepository.GetPointOfInterestForCity(cityId, id);
            if (_cityInfoRepository.CityExist(cityId) || interest == null)
            {
                return NotFound();
            }

            //city.PointsOfInterests.Remove(interest);
            _cityInfoRepository.DeletePointOfInterest(interest);

            _mailService.Send("Point of interest deleted!",
                $"Point of interest {interest.Name} with id {interest.Id} was detleted!");

            if (!_cityInfoRepository.Save())
            {
                return StatusCode(500, "Something went wrog");
            }

            return NoContent();
        }
    }
}
