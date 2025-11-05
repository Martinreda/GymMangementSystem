using AutoMapper;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositiories.Interfaces;
using GymManagmentBSL.Services.Interfaces;
using GymManagmentBSL.ViewModels.SessionViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentBSL.Services.Classes
{
    public class SessionService : ISessionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SessionService(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public bool CreateSession(CreateSessionViewModel CreatedSession)
        {
            try
            {
                // Check if Trainer exists
                if (!IsTrainerExists(CreatedSession.TrainerId)) return false;

                // Check if Category exists
                if (!IsCategoryExists(CreatedSession.CategoryId)) return false;

                // Check if StartDate is before EndDate
                if (!IsDateTimeValid(CreatedSession.StartDate, CreatedSession.EndDate)) return false;

                // Check Capacity constraints
                if (CreatedSession.Capacity > 25 || CreatedSession.Capacity < 0) return false;

                // Map DTO to Entity
                var SessionEntity = _mapper.Map<Session>(source: CreatedSession);

                // Add Entity to Context
                _unitOfWork.GetRepository<Session>().Add(entity: SessionEntity);

                // Save Changes and return success status
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                // Log the error
                Console.WriteLine(value: $"Create Session Failed: {ex}");

                // Indicate failure
                return false;
            }

        }

        public IEnumerable<SessionViewModel> GetAllSessions()
        {
            var Sessions = _unitOfWork.SessionRepository.GetAllSessionsWithTrianerAndCategory();
            if (!Sessions.Any()) return [];

         

            var MappedSessions = _mapper.Map<IEnumerable<Session>, IEnumerable<SessionViewModel>>(Sessions);
            foreach (var session in MappedSessions)
            {
                session.AvailableSlots = session.Capacity - _unitOfWork.SessionRepository.GetCountOfBooksSlots(sessionId: session.Id);
            }
            return MappedSessions;
        }

        public SessionViewModel? GetSessionById(int SessionId)
        {
            var Sessions = _unitOfWork.SessionRepository.GeTSessionWithTrainerAndCategory(SessionId);
            if (Sessions is null) return null;
            var MappedSession = _mapper.Map<Session, SessionViewModel>(source: Sessions);
            MappedSession.AvailableSlots = MappedSession.Capacity - _unitOfWork.SessionRepository.GetCountOfBooksSlots(sessionId: MappedSession.Id);
            return MappedSession;
        }



        #region Helper Methods

        private bool IsTrainerExists(int TrainerId)
        {
            return _unitOfWork.GetRepository<Trainer>().GetById( TrainerId) is not null;
        }

        private bool IsCategoryExists(int CategoryId)
        {
            return _unitOfWork.GetRepository<Category>().GetById(CategoryId) is not null;
        }

        private bool IsDateTimeValid(DateTime StartDate, DateTime EndDate)
        {
            return StartDate < EndDate;
        }

        #endregion
    }
}
