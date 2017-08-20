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
    /// <summary>セルの値が変更時のイベント</summary>
#region private void onChangedStringResource(object, EventArgs)
    private void onChangedStringResource(object sender, DataGridViewCellEventArgs e)
    {
      this.currentStatus = ResourceStatus.Updated;

#if _DEBUG_
      Console.WriteLine("### [D] onChangedStringResoruce");
#endif
    }
#endregion

    // NOTE: 行追加後に, onChangedStringResoruce が呼ばれるため, 不要である可能性がある.
    /// <summary>新規の行を追加した際に呼ばれる.</summary>
    private void onAddedStringResource(object sender, DataGridViewRowsAddedEventArgs e)
    {
#if _DEBUG_
      Console.WriteLine("### [D] onAddedStringResource");
#endif
    }
  }
}
