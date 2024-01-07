using System.Collections;
using System.Collections.Generic;

namespace MittClick.Models
{
    public class IndexViewModel : IEnumerable<Profile>
    {
        private readonly List<Profile> profiles = new List<Profile>();

        public List<Project> Projects { get; set; } // Behåller Projects här

        public IEnumerator<Profile> GetEnumerator()
        {
            return profiles.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        // Method to add profiles to the list
        public void Add(Profile profile)
        {
            profiles.Add(profile);
        }
    }
}
