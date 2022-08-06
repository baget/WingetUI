using System.Diagnostics;

namespace WingetUI
{
    public partial class Winget
    {
        [DebuggerDisplay("Name: {Name}, Id: {Id}, Version: {Version}, AvailableVersion: {AvailableVersion}, Source: {Source}")]
        public struct Entity
        {
            private string name;
            private string id;
            private string version;
            private string availableVersion;
            private string source;

            public string Name { get => name; set => name = value; }
            public string Id { get => id; set => id = value; }
            public string Version { get => version; set => version = value; }
            public string AvailableVersion { get => availableVersion; set => availableVersion = value; }
            public string Source { get => source; set => source = value; }

            private string GetDebuggerDisplay() => ToString();
        }
    }
}
