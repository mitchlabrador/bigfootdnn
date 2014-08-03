using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BigfootDNN
{
    public class ClientLibrary
    {

        public enum LocationEnum { Head = 0, Footer = 1 }
        public enum LibraryTypeEnum { CSS = 0, Javascript = 1 }

        public string Key { get; set; }
        public LibraryTypeEnum LibraryType { get; set; }
        public string ReleaseSource { get; set; }
        public string DebugSource { get; set; }
        public int Order { get; set; }
        public LocationEnum Location { get; set; }

        public ClientLibrary(string key, LibraryTypeEnum libraryType, string releaseSource, string debugSource = "", int order = 10, LocationEnum location = LocationEnum.Head)
        {
            Key = key;
            LibraryType = libraryType;
            ReleaseSource = releaseSource;
            if (string.IsNullOrEmpty(debugSource)) debugSource = releaseSource;
            DebugSource = debugSource;
            Order = order;
            Location = location;
        }

        public string GetSource(bool isDebug)
        {
            return isDebug ? DebugSource : ReleaseSource;
        }
    }
}
