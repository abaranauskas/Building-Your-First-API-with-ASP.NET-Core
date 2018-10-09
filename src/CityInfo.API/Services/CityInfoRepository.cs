using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.Services
{
    public class CityInfoRepository : ICityInforepository
    {
        CityInfoContext _context;

        public CityInfoRepository(CityInfoContext context)
        {
            _context = context;
        }

        public void AddPointOfInterestForCity(int cityId, PointOfInterest pointOfInterest)
        {
            var city = GetCity(cityId, false);
            city.PointsOfInterest.Add(pointOfInterest);
            Save();
        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }

        public bool CityExist(int cityId)
        {
            return _context.Cities.Any(c => c.Id == cityId);
        }

        public IEnumerable<City> GetCities()
        {
            return _context.Cities.OrderBy(c=>c.Name).ToList();
        }

        public City GetCity(int cityId, bool includePointOfInterest)
        {
            if (includePointOfInterest)
            {
                return _context.Cities.Include(c => c.PointsOfInterest)
                    .Where(c => c.Id == cityId).FirstOrDefault();
            }

            return _context.Cities.FirstOrDefault(c => c.Id == cityId);
        }

        public PointOfInterest GetPointOfInterestForCity(int cityId, int pointOfInterestId)
        {
            return _context.PointOfInterest
                .Where(p => p.Id == pointOfInterestId && p.CityId == cityId)
                .FirstOrDefault();
                
        }

        public IEnumerable<PointOfInterest> GetPointsOfInterestForCity(int cityId)
        {
            return _context.PointOfInterest.Where(p => p.CityId == cityId)
                .ToList();
               
        }

        public void DeletePointOfInterest(PointOfInterest pointOfInterest)
        {
            _context.PointOfInterest.Remove(pointOfInterest);
        }
    }
}
