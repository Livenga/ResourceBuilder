//
// DataGridView �֌W�̃C�x���g
//

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
#region private void onChangedStringResource(object, DataGridViewCellEventArgs)
    private void onChangedStringResource(object sender, DataGridViewCellEventArgs e)
    {
      string _key;

      Console.WriteLine("changed row:{0}, cell:{1}", e.RowIndex, e.ColumnIndex);
      if(this.dataString.Rows[e.RowIndex] == null ||
          this.dataString.Rows[e.RowIndex].Cells[e.ColumnIndex] == null) return;


      if(e.ColumnIndex == 0) {
        _key = (string)this.dataString.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;

        if(_key == null) {
          this.dataString.Rows.RemoveAt(e.RowIndex);
          return;
        }


        foreach(DataGridViewRow row in this.dataString.Rows) {
          DataGridViewCell cell;

          cell = row.Cells[0];

          // �ύX�����Z���̔ԍ��Ƒ��쒆�Z���ԍ�����v�͏��O
          if(cell.RowIndex == e.RowIndex) continue;

          if(_key.Equals((string)cell.Value)) {
            MessageBox.Show(
                _key + " is already exists.", "Resource key error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);

            //this.dataString.CurrentCell =
              //this.dataString.Rows[e.RowIndex].Cells[0];
            return;
          }
        }
      }


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
