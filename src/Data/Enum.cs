using System;
using System.Data;

namespace Live.Data
{
  /// <summary>���\�[�X�̎��</summary>
  public enum ResourceType : uint {
    Strings = 0,
    Images  = 1,
    Icons   = 2,
    Audios  = 3,
    Files   = 4,
  }

  /// <summary>���\�[�X�̃t�@�C���ҏW���[�h</summary>
  public enum ResourceEditMode : uint {
    New  = 0,
    Edit = 1,
  }

  /// <summary>���\�[�X�̕ҏW���</summary>
  public enum ResourceStatus : uint {
    Saved   = 0,
    Updated = 1,
  }
}
