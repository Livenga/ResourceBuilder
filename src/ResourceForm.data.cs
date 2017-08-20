using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Media;
using System.Resources;
using System.Windows.Forms;

using Live.Data;


namespace Live
{
  public partial class ResourceForm
  {
    /// <summary>�Z���̒l���ύX���̃C�x���g</summary>
#region private void onChangedStringResource(object, EventArgs)
    private void onChangedStringResource(object sender, DataGridViewCellEventArgs e)
    {
      this.currentStatus = ResourceStatus.Updated;

#if _DEBUG_
      Console.WriteLine("### [D] onChangedStringResoruce");
#endif
    }
#endregion

    // NOTE: �s�ǉ����, onChangedStringResoruce ���Ă΂�邽��, �s�v�ł���\��������.
    /// <summary>�V�K�̍s��ǉ������ۂɌĂ΂��.</summary>
    private void onAddedStringResource(object sender, DataGridViewRowsAddedEventArgs e)
    {
#if _DEBUG_
      Console.WriteLine("### [D] onAddedStringResource");
#endif
    }
  }
}
