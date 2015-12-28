using System;
using System.Collections.Generic;
using System.IO;
using Vik.Code.Utility;

namespace Vik
{
    // This profile manager stores data locally
    class ProfileManagerLocalData : IProfileManager
    {
        private const string FileName = "Profiles.txt";

        private List<string> _profiles = new List<string>();

        public ProfileManagerLocalData()
        {
            Load();
        }

        private string GetFileName()
        {
            string path = Util.GetFolderForLocalData();
            string fileName = Path.Combine(path, FileName);
            return fileName;
        }

        private void Save()
        {
            string fileName = GetFileName();
            File.WriteAllLines(fileName, _profiles);
        }

        private void Load()
        {
            string fileName = GetFileName();
            if (File.Exists(fileName))
            {
                var lines = File.ReadAllLines(fileName);
                _profiles = new List<string>(lines);
            }
        }

        IEnumerable<string> IProfileManager.ExistingProfiles()
        {
            return _profiles;
        }

        void IProfileManager.CreateProfile(string name)
        {
            if (_profiles.Contains(name))
                throw new ArgumentException("Profile already exists: " + name);

            _profiles.Add(name);
            Save();
        }

        void IProfileManager.DeleteProfile(string name)
        {
            if (!_profiles.Contains(name))
                throw new ArgumentException("Profile does not exist: " + name);

            _profiles.Remove(name);
            Save();
        }


        void IProfileManager.MoveToTop(string name)
        {
            if (!_profiles.Contains(name))
                throw new ArgumentException("Profile does not exist: " + name);

            _profiles.Remove(name);
            _profiles.Insert(0, name);
            Save();
        }
    }
}
