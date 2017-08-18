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
  public partial class ResourceForm : Form
  {
    // UI 関係
    private MenuStrip         mainMenu;
    private ToolStripMenuItem mFile, mfNew, mfOpen, mfSave, mfExit;

    private Label    lblResourceType;
    private ComboBox cbxResourceType;

    private Button   btnAdd, btnDelete;


    private string resourceFilePath;
    
    // リソース編集
    private DataGridView dataString;
    private ListView     dataImage;
    private ListView     dataIcon;


    //
    private ResourceEditMode currentMode   = ResourceEditMode.New;
    private ResourceType     currentType   = ResourceType.Strings;
    private ResourceStatus   currentStatus = ResourceStatus.Saved;


    //
    // フォームコンストラクタ
    //

    /// <summary>コンストラクタ</summary>
#region public ResourceForm()
    public ResourceForm()
    {
      this.InitializeComponent();

      
      // コールバックイベントの設定
#if _DEBUG_
      this.Resize += new EventHandler(
          (s, e) => { Console.WriteLine("### [D] Resize: {0}x{1}", this.Width, this.Height); });
#endif

      this.mfOpen.Click += new EventHandler(this.onClickOpenMenu);
      this.mfExit.Click += new EventHandler(this.onClickExitMenu);
      this.mfSave.Click += new EventHandler(this.onClickSaveMenu);

      this.btnAdd.Click    += new EventHandler(this.onClickEditButton);
      this.btnDelete.Click += new EventHandler(this.onClickEditButton);

      this.cbxResourceType.SelectedIndexChanged +=
        new EventHandler(this.onChangedResourceType);

      // UI 系(ResourceForm.design.cs に記述)
      //this.lblResourceType.Click += new EventHandler(this.onClickResourceTypeLabel);
    }
#endregion


    //
    // 共用メソッド
    //

    /// <summary>リソース編集画面の表示</summary>
    /// <param name="type">編集するリソースの種類</param>
#region private void DisplayResourceEditor(ResourceType)
    private void DisplayResourceEditor(ResourceType type)
    {
      this.dataString.Visible = (ResourceType.Strings == type) ? true : false;
      this.dataImage.Visible  = (ResourceType.Images  == type) ? true : false;
      this.dataIcon.Visible   = (ResourceType.Icons   == type) ? true : false;
    }
#endregion

    /// <summary>リソースファイルの保存</summary>
#region private void SaveResourceFile()
    private void SaveResourceFile()
    {
      SaveFileDialog dialog;
      DialogResult   result;

      if(this.resourceFilePath != null &&
          this.currentMode == ResourceEditMode.Edit) { // 上書き保存
#if _DEBUG_
        Console.WriteLine("### [D] Overwrite {0}", this.resourceFilePath);
#endif
      }
      else { // 新規作成
        dialog = new SaveFileDialog();
        dialog.Title            = "Save Resource File";
        dialog.Filter           = "XML Resouce(*.resx)|*.resx|All files (*.*)|*.*";
        dialog.FilterIndex      = 1;
        dialog.InitialDirectory = ".";
        dialog.RestoreDirectory = true;

        result = dialog.ShowDialog();
        if(result == DialogResult.OK) {
          this.resourceFilePath = dialog.FileName;
        }
        else {
          dialog.Dispose();
          dialog = null;
          return;
        }

        dialog.Dispose();
        dialog = null;
      }


      // ファイルパスを用いて, リソース書き込み処理
      try {
        using(ResXResourceWriter writer = new ResXResourceWriter(this.resourceFilePath)) {
          //Image img = Image.FromFile(@"samples\lena.png");

          writer.AddResource("Key1", "value1");
          //writer.AddResource("Image_Key1", (object)img);

          //img.Dispose();
        }
      }
      catch(Exception except) {
        MessageBox.Show(
            "faled to save resouce file.\n" + except.Message,
            "Resource File Save Error",
            MessageBoxButtons.OK,
            MessageBoxIcon.Error);
      }
    }
#endregion



    //
    // コールバックメソッド: メニュー
    //

    /// <summary>Open メニュー選択</summary>
#region private void onClickOpenMenu(object, EventArgs)
    private void onClickOpenMenu(object sender, EventArgs e)
    {
      OpenFileDialog dialog;
      DialogResult   result;

      dialog = new OpenFileDialog();
      dialog.Title            = "Open Resource File";
      dialog.Multiselect      = false;
      dialog.Filter           = "XML Resouce(*.resx)|*.resx|All files (*.*)|*.*";
      dialog.FilterIndex      = 1;
      dialog.InitialDirectory = ".";
      dialog.RestoreDirectory = true;

      result = dialog.ShowDialog();
      if(result == DialogResult.OK) {
        this.resourceFilePath = dialog.FileName;
        this.currentMode      = ResourceEditMode.Edit;

        //Console.WriteLine("Columns count: {0}", this.dataString.ColumnCount);
        //Console.WriteLine("Rows count: {0}", this.dataString.RowCount);
        // TODO: ここで, リソースファイルを展開する.
        try {
          using(ResXResourceSet set = new ResXResourceSet(this.resourceFilePath)) {
            IDictionaryEnumerator dict = set.GetEnumerator();

            while(dict.MoveNext()) {
              string key = (string)dict.Key;
              object v   = (object)dict.Value;


              // is で識別
              if(v is string) { // 文字列
                Console.WriteLine("key:{0} is string.", key);
              }

              else if(v is Bitmap) { // 画像
                Console.WriteLine("key:{0} is bitmap.", key);
              }

              else if(v is SoundPlayer) { // 音声
              }
            }
          }

          this.Text = "Resource Builder: " + this.resourceFilePath;
        }
        catch(Exception except) {
          MessageBox.Show(
              "failed to open resource file.\n" + except.Message,
              "Resource File Open Error",
              MessageBoxButtons.OK, MessageBoxIcon.Error);
          this.resourceFilePath = null;
        }
      }

      dialog.Dispose();
      dialog = null;
    }
#endregion

    /// <summary>Save メニュー選択 リソースファイルの書きこみ</summary>
#region private void onClickSaveMenu(object, EventArgs)
    private void onClickSaveMenu(object sender, EventArgs e)
    {
      this.SaveResourceFile();
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

    //
    // コールバックメソッド: ボタン
    //

    // 追加・削除ボタン処理
    /// <summary></summary>
#region private void onClickEditButton(object, EventArgs)
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
#endregion


    //
    // コールバックメソッド: フォーム
    //

    /// <summary></summary>
#region private void onClosedForm(object, EventArgs)
    private void onClosedForm(object sender, EventArgs e)
    {
      DialogResult result;

      if(this.currentStatus == ResourceStatus.Updated) {
        result = MessageBox.Show(
            "Overwrite?", "Overwrite confirmation",
            MessageBoxButtons.YesNo, MessageBoxIcon.Information);

        //
        if(result == DialogResult.Yes) {
          this.SaveResourceFile();
        }
      }
    }
#endregion
  }
}
