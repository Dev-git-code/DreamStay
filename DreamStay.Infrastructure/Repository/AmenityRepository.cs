using DreamStay.Application.Common.Interfaces;
using DreamStay.Domain.Entities;
using DreamStay.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DreamStay.Infrastructure.Repository
{
    public class AmenityRepository : Repository<Amenity>, IAmenityRepository
    {
        private readonly ApplicationDbContext _db;

        public AmenityRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
      
        public void Update(Amenity entity)
        {
            _db.Amenities.Update(entity);
        }
    }
}
