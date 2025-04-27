using System.Collections.Generic;
using DAL.Models;
using DAL.Repo;

namespace BLL.Service
{
    public class TherapistService
    {
        private readonly TherapistRepository _repo;

        public TherapistService()
        {
            _repo = new TherapistRepository();
        }

        public List<Therapist> GetAllTherapists()
        {
            return _repo.GetAll();
        }

        public Therapist? GetTherapistById(int id)
        {
            return _repo.GetById(id);
        }

        public void AddTherapist(Therapist therapist)
        {
            _repo.Add(therapist);
        }

        public void UpdateTherapist(Therapist therapist)
        {
            _repo.Update(therapist);
        }

        public void DeleteTherapist(int id)
        {
            _repo.Delete(id);
        }
    }
}