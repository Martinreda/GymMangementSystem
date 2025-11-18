using AutoMapper;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositiories.Interfaces;
using GymManagmentBSL.Services.Interfaces;
using GymManagmentBSL.ViewModels.TrianerViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentBSL.Services.Classes
{
    public class TrainerService : ITrainerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TrainerService(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public bool CreateTrainer(CreateTrainerViewModel createdTrainer)
        {
            try
            {
                var repo = _unitOfWork.GetRepository<Trainer>();

                if (IsEmailExists(createdTrainer.Email) || IsPhoneExists(createdTrainer.Phone))
                    return false;

                var trainer = _mapper.Map<Trainer>(createdTrainer);

                repo.Add(trainer);

                return _unitOfWork.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }

        public IEnumerable<TrainerViewModel> GetAllTrainers()
        {
            var trainers = _unitOfWork.GetRepository<Trainer>().GetALl();
            if (trainers == null || !trainers.Any())
                return Enumerable.Empty<TrainerViewModel>();

            return _mapper.Map<IEnumerable<TrainerViewModel>>(trainers);
        }

        public TrainerViewModel? GetTrainerDetails(int trainerId)
        {
            var trainer = _unitOfWork.GetRepository<Trainer>().GetById(trainerId);
            if (trainer == null) return null;

            return _mapper.Map<TrainerViewModel>(trainer);
        }

        public TrainerToUpdateViewModel? GetTrainerToUpdate(int trainerId)
        {
            var trainer = _unitOfWork.GetRepository<Trainer>().GetById(trainerId);
            if (trainer == null) return null;

            return _mapper.Map<TrainerToUpdateViewModel>(trainer);
        }

        public bool RemoveTrainer(int trainerId)
        {
            var repo = _unitOfWork.GetRepository<Trainer>();
            var trainerToRemove = repo.GetById(trainerId);

            if (trainerToRemove == null || HasActiveSessions(trainerId)) return false;

            repo.Delete(trainerToRemove);
            return _unitOfWork.SaveChanges() > 0;
        }

        public bool UpdateTrainerDetails(TrainerToUpdateViewModel updatedTrainer, int trainerId)
        {
            var repo = _unitOfWork.GetRepository<Trainer>();
            var trainerToUpdate = repo.GetById(trainerId);

            if (trainerToUpdate == null || IsEmailExists(updatedTrainer.Email) || IsPhoneExists(updatedTrainer.Phone))
                return false;

            _mapper.Map(updatedTrainer, trainerToUpdate);

            repo.Update(trainerToUpdate);
            return _unitOfWork.SaveChanges() > 0;
        }

        public bool HasActiveSessions(int trainerId)
        {
            return _unitOfWork.GetRepository<Session>()
                .GetALl(s => s.TrainerId == trainerId && s.Description != "Cancelled" && s.StartDate >= DateTime.Today)
                .Any();
        }

        public IEnumerable<Session> GetActiveSessions(int trainerId)
        {
            return _unitOfWork.GetRepository<Session>()
                .GetALl(s => s.TrainerId == trainerId && s.Description != "Cancelled" && s.StartDate >= DateTime.Today)
                .ToList();
        }


        #region Helper Methods

        private bool IsEmailExists(string email)
        {
            var existing = _unitOfWork.GetRepository<Member>().GetALl(
                m => m.Email == email).Any();
            return existing;
        }

        private bool IsPhoneExists(string phone)
        {
            var existing = _unitOfWork.GetRepository<Member>().GetALl(
                m => m.Phone == phone).Any();
            return existing;
        }

      
        #endregion
    }
}
