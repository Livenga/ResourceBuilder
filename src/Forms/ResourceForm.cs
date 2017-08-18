using System;
using System.Drawing;
using System.Windows.Forms;

using Live.Data;


namespace Live.Forms
{
  public partial class ResourceForm : Form
  {
    // UI 関係
    private MenuStrip         mainMenu;
    private ToolStripMenuItem mFile, mfNew, mfOpen, mfSave, mfExit;

    private Label    lblResourceType;
    private ComboBox cbxResourceType;

    private Button   btnAdd, btnDelete;


    // リソース編集
    private DataGridView dataString;

    //
    private ResourceType currentType;


    /// <summary>コンストラクタ</summary>
#region public ResourceForm()
    public ResourceForm()
    {
      this.InitializeComponent();


#if _DEBUG_
      this.Resize += new EventHandler(
          (s, e) => { Console.WriteLine("### [D] Resize: {0}x{1}", this.Width, this.Height); });
#endif

      this.mfOpen.Click += new EventHandler(this.onClickOpenMenu);
      this.mfExit.Click += new EventHandler(this.onClickExitMenu);

      this.btnAdd.Click    += new EventHandler(this.onClickEditButton);
      this.btnDelete.Click += new EventHandler(this.onClickEditButton);

      this.cbxResourceType.SelectedIndexChanged +=
        new EventHandler(this.onChangedResourceType);
    }
#endregion


    /// <summary>リソース編集画面の表示</summary>
    /// <param name="type">編集するリソースの種類</param>
#region private void DisplayResourceEditor(ResourceType)
    private void DisplayResourceEditor(ResourceType type)
    {
      this.dataString.Visible = (ResourceType.Strings == type) ? true : false;
    }
#endregion


    //
    // コールバックメソッド
    //

    /// <summary>Open メニュー選択</summary>
#region private void onClickOpenMenu(object, EventArgs)
    private void onClickOpenMenu(object sender, EventArgs e)
    {
      OpenFileDialog dialog;
      DialogResult   result;

      dialog = new OpenFileDialog();
      dialog.Title            = "リソースファイルを開く";
      dialog.Multiselect      = false;
      dialog.Filter           = "XML Resouce(*.resx)|*.resx|All files (*.*)|*.*";
      dialog.FilterIndex      = 1;
      dialog.InitialDirectory = ".";
      dialog.RestoreDirectory = true;

      result = dialog.ShowDialog();
      if(result == DialogResult.OK) {
      }

      dialog.Dispose();
      dialog = null;
    }
#endregion

    /// <summary>Exit メニュー選択</summary>
#region private void onClickExitMenu(object, EventArgs)
    private void onClickExitMenu(object sender, EventArgs e)
    {
      this.Close();
    }
#endregion

    /// <summary>リソース種類の変更</summary>
#region private void onChangedResourceType(object, EventArgs)
    private void onChangedResourceType(object sender, EventArgs e)
    {
      switch(this.cbxResourceType.SelectedIndex) {
        case 0:
          this.currentType = ResourceType.Strings;
          break;

        case 1:
          this.currentType = ResourceType.Images;
          break;

        case 2:
          this.currentType = ResourceType.Icons;
          break;

        case 3:
          this.currentType = ResourceType.Audios;
          break;

        case 4:
          this.currentType = ResourceType.Files;
          break;

        case 5:
          this.currentType = ResourceType.Strings;
          break;
      }

      // リソース編集画面を遷移
      this.DisplayResourceEditor(this.currentType);
    }
#endregion
 
    // 追加・削除ボタン処理
    /// <summary></summary>
    private void onClickEditButton(object sender, EventArgs e)
    {
      Button btn = (Button)sender;


      // データの追加
      if(btn == this.btnAdd) {
      }

      // データの削除
      else if(btn == this.btnDelete) {

#if _DEBUG_
        Console.WriteLine("Type:{0}, {1}",
            this.dataString.CurrentCell.ValueType,
            this.dataString.CurrentCell.Value);

        Console.WriteLine("{0}, {1}, {2}",
            this.dataString.SelectedCells.Count,
            this.dataString.SelectedColumns.Count,
            this.dataString.SelectedRows.Count);
#endif
        //this.dataString.CurrentCell;
      }
    }
  }
}
