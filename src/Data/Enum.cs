using System;
using System.Data;

namespace Live.Data
{
  /// <summary>リソースの種類</summary>
  public enum ResourceType : uint {
    Strings = 0,
    Images  = 1,
    Icons   = 2,
    Audios  = 3,
    Files   = 4,
  }

  /// <summary>リソースのファイル編集モード</summary>
  public enum ResourceEditMode : uint {
    New  = 0,
    Edit = 1,
  }

  /// <summary>リソースの編集状態</summary>
  public enum ResourceStatus : uint {
    Saved   = 0,
    Updated = 1,
  }
}
