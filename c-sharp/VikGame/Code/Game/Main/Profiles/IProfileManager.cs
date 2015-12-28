using System.Collections.Generic;

namespace Vik
{
    public interface IProfileManager
    {
        IEnumerable<string> ExistingProfiles();
        void CreateProfile(string name);
        void DeleteProfile(string name);
        void MoveToTop(string name);
    }
}
