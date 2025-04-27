using System.Collections.Generic;
using System.Linq;
using DAL.Models;

namespace DAL.Repo
{
    public class TherapistRepository
    {
        private readonly SbsContext _context;

        public TherapistRepository()
        {
            _context = new SbsContext();
        }

        public List<Therapist> GetAll()
        {
            return _context.Therapists
                .Select(t => new Therapist
                {
                    Id = t.Id,
                    Description = t.Description,
                    Experience = t.Experience,
                    Image = t.Image,
                    IdNavigation = t.IdNavigation
                }).ToList();
        }

        public Therapist? GetById(int id)
        {
            return _context.Therapists.FirstOrDefault(t => t.Id == id);
        }

        public void Add(Therapist therapist)
        {
            _context.Therapists.Add(therapist);
            _context.SaveChanges();
        }

        public void Update(Therapist therapist)
        {
            _context.Therapists.Update(therapist);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var therapist = _context.Therapists.FirstOrDefault(t => t.Id == id);
            if (therapist != null)
            {
                _context.Therapists.Remove(therapist);
                _context.SaveChanges();
            }
        }
    }
}