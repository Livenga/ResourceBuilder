using System;
using System.Data;

namespace Live.Data
{
  public enum ResourceType : uint {
    Strings = 0,
    Images  = 1,
    Icons   = 2,
    Audios  = 3,
    Files   = 4,
  }

  public enum ResourceEditMode : uint {
    New  = 0,
    Edit = 1,
  }

  public enum ResourceStatus : uint {
    Saved   = 0,
    Updated = 1,
  }
}
