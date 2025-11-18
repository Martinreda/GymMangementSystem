using GymManagmentBSL.ViewModels.SessionViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentBSL.Services.Interfaces
{
    public interface ISessionService
    {
        IEnumerable<SessionViewModel> GetAllSessions();

        public SessionViewModel? GetSessionById(int SessionId);

        bool CreateSession(CreateSessionViewModel CreatedSession);

        UpdateSessionViewModel? GetSessionToUpdate(int SessionId);

        bool UpdateSession(UpdateSessionViewModel UpdatedSession, int SessionId);

        bool RemoveSession(int SessionId);
        bool CanSessionBeDeleted(int sessionId);
        IEnumerable<SelectTrainerViewModel> GetTrainerDropDown();
        IEnumerable<SelectCategoryViewModel> GetCategoryDropDown();
    }
}
