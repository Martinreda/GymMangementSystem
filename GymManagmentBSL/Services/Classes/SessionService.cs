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


        public UpdateSessionViewModel? GetSessionToUpdate(int SessionId)
        {
            var Session = _unitOfWork.SessionRepository.GetById( SessionId);
            if (!IsSessionAvailableForUpdating(session: Session!)) return null;

            return _mapper.Map<UpdateSessionViewModel>(source: Session);
        }

        public bool UpdateSession(UpdateSessionViewModel UpdatedSession, int SessionId)
        {
            try
            {
                // Retrieve original session entity from the database
                var Session = _unitOfWork.SessionRepository.GetById( SessionId);

                // 1. Check if the existing session is eligible for *any* updates
                if (!IsSessionAvailableForUpdating(session: Session!)) return false;

                // 2. Validate the incoming new data dependencies
                if (!IsTrainerExists(UpdatedSession.TrainerId)) return false;

                // 3. Validate the incoming new date/time integrity
                if (!IsDateTimeValid(UpdatedSession.StartDate, UpdatedSession.EndDate)) return false;

                // 4. Map the changes onto the existing entity
                _mapper.Map(source: UpdatedSession, destination: Session);

                // 5. Update metadata/audit fields
                Session!.UpdatedAt = DateTime.Now;

                // 6. Stage the entity as modified
                _unitOfWork.SessionRepository.Update(entity: Session);

                // 7. Commit changes to the database and return success status
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                // Handle unhandled exceptions during the process
                Console.WriteLine(value: $"Update Session Failed: {ex}");
                return false;
            }
        }

        public bool RemoveSession(int SessionId)
        {
            try
            {
                // Retrieve the existing session entity from the database
                var Session = _unitOfWork.SessionRepository.GetById(SessionId);

                // 1. Check if the session is eligible for removal based on business rules
                if (!IsSessionAvailableForRemoving(session: Session!)) return false;

                // 2. Stage the entity for deletion
                _unitOfWork.SessionRepository.Delete(entity: Session!);

                // 3. Commit the change to the database and return success status
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                // Handle unhandled exceptions during the process
                {
                    Console.WriteLine(value: $"Remove Session Failed :{ex}");
                    return false;
                }
            }
        }




        #region Helper Methods

        private bool IsSessionAvailableForUpdating(Session session)
        {
            if (session is null) return false;

            // If Session Completed - No Updated Allowed
            if (session.EndDate < DateTime.Now) return false;

            // If Session Started - No Updated Allowed
            if (session.StartDate <= DateTime.Now) return false;

            // If Session Has Active Bookings - No Updated Allowed
            var HasActiveBooking = _unitOfWork.SessionRepository.GetCountOfBooksSlots(sessionId: session.Id) > 0;
            if (HasActiveBooking) return false;

            return true;
        }

        private bool IsSessionAvailableForRemoving(Session session)
        {
            if (session is null) return false;

            // 1. Session In Progress - No Delete Allowed
            if (session.StartDate <= DateTime.Now && session.EndDate > DateTime.Now) return false;

            // 2. Session Is Upcoming - No Delete Allowed
            if (session.StartDate > DateTime.Now) return false;

            // 3. If Session Has Active Bookings - No Delete Allowed
            var HasActiveBooking = _unitOfWork.SessionRepository.GetCountOfBooksSlots(sessionId: session.Id) > 0;
            if (HasActiveBooking) return false;

            return true;
        }
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
