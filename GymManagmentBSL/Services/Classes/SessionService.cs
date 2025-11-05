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
    }
}
