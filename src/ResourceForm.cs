using System;
using System.Collections;
using System.Collections.Generic;
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
#if _DEBUG_
    private ToolStripMenuItem mTest;
#endif

    private Label    lblResourceType;
    private ComboBox cbxResourceType;

    private Button   btnAdd, btnDelete;


    private string resourceFilePath;
    
    // リソース編集
    private DataGridView dataString;
    private ListView     dataImage;
    private ListView     dataIcon;

    // イメージリスト
    private ImageList    dataImageList;
    private ImageList    dataIconList;

    //
    private List<Image>  listImage = new List<Image>();
    private List<Icon>   listIcon  = new List<Icon>();


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
      this.mTest.Click  += new EventHandler(onClickMenuTest);
#endif
      // フォームイベントを設定
      this.Closed += new EventHandler(this.onClosedForm);

      // メニューイベントを設定
      this.mfNew.Click  += new EventHandler(this.onClickNewMenu);
      this.mfOpen.Click += new EventHandler(this.onClickOpenMenu);
      this.mfExit.Click += new EventHandler(this.onClickExitMenu);
      this.mfSave.Click += new EventHandler(this.onClickSaveMenu);

      // ボタンイベントを設定
      this.btnAdd.Click    += new EventHandler(this.onClickAddButton);
      this.btnDelete.Click += new EventHandler(this.onClickDeleteButton);

      // コンボボックスイベントを設定
      this.cbxResourceType.SelectedIndexChanged +=
        new EventHandler(this.onChangedResourceType);


      //
      // リソース関係のイベントを設定
      //
      this.dataString.CellValueChanged += new DataGridViewCellEventHandler(this.onChangedStringResource);
      // NOTE: 不要の可能性があるため, コメントアウト
      //this.dataString.RowsAdded += new DataGridViewRowsAddedEventHandler(this.onAddedStringResource);


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
          // 追加済み文字列の取得
          if(this.dataString.Rows != null &&
              this.dataString.Rows.Count > 1) {
            foreach(DataGridViewRow row in this.dataString.Rows) {
              string _key, _value;

              if(!row.IsNewRow) {
                _key   = (string)row.Cells[0].Value;

                _value = (string)row.Cells[1].Value;
                if(_value == null) {
                  writer.AddResource(_key, "");
                }
                else {
                  writer.AddResource(_key, _value);
                }
                //Console.WriteLine("Row[{0}] -> Key:{1}, Value:{2}", row.Index, _key, _value);
              }
            }
          }

         // 追加済み通常画像の取得
          if(this.dataImage.Items.Count > 0) {
            for(int i = 0; i < this.dataImage.Items.Count; i++) {
              string _key;
              Image  _value;

              _key   = this.dataImage.Items[i].Text;
              _value = Image.FromFile((string)this.dataImage.Items[i].Tag);

              writer.AddResource(_key, _value);
              Console.WriteLine("Index[{0}] -> Key:{1}, Size:{2}x{3}",
                  i, _key, _value.Width, _value.Height);

              _value.Dispose();
              _value = null;
            }
          }


          // 追加済みアイコン画像の取得
          if(this.dataIcon.Items.Count > 0) {
            for(int i = 0; i < this.dataIcon.Items.Count; i++) {
              string _key;
              Icon   _value;

              _key   = this.dataIcon.Items[i].Text;
              _value = new Icon((string)this.dataIcon.Items[i].Tag);

              writer.AddResource(_key, _value);
              Console.WriteLine("Index[{0}] -> Key:{1}, Size:{2}x{3}",
                  i, _key, _value.Width, _value.Height);

              _value.Dispose();
              _value = null;
            }
          }
        } // using(ResXResourceWriter)

        this.Text          = "Resource Builder: " + this.resourceFilePath;
        this.currentMode   = ResourceEditMode.Edit;
        this.currentStatus = ResourceStatus.Saved;
      } // try
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

    /// <summary>New メニュー選択</summary>
#region private void onClickNewMenu(object, EventArgs)
    private void onClickNewMenu(object sender, EventArgs e)
    {
      DialogResult result;

      // TODO: 保存確認.
      if(this.currentStatus == ResourceStatus.Updated) {
        result = MessageBox.Show(
            "", "",
            MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);

        if(result == DialogResult.Yes) {
          this.SaveResourceFile();
        }
        else if(result == DialogResult.Cancel) {
          return;
        }
      }


      this.currentMode   = ResourceEditMode.New;
      this.currentStatus = ResourceStatus.Saved;

      this.dataString.Rows.Clear();

      this.dataImage.Items.Clear();
      this.dataImageList.Images.Clear();
      this.listImage.Clear();

      this.dataIcon.Items.Clear();
      this.dataIconList.Images.Clear();
      this.listIcon.Clear();

      System.GC.Collect();

      this.Text = "Resource Builder";
    }
#endregion

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
        // 変更状態を保存
        if(this.currentStatus == ResourceStatus.Updated) {
          result = MessageBox.Show("", "",
              MessageBoxButtons.YesNo, MessageBoxIcon.Information);

          if(result == DialogResult.Yes) {
            this.SaveResourceFile();
          }
        }

        // TODO: 現在のリソースを破棄.
        // 文字列リソースの破棄
        if(this.dataString.Rows.Count > 1)
          this.dataString.Rows.Clear();

        // 通常画像リソースの破棄
        if(this.dataImage.Items.Count > 0)
          this.dataImage.Items.Clear();
        if(this.dataImageList.Images.Count > 0)
          this.dataImageList.Images.Clear();
        this.listImage.Clear();

        // アイコン画像リソースの破棄
        if(this.dataIcon.Items.Count > 0)
          this.dataIcon.Items.Clear();
        if(this.dataIconList.Images.Count > 0)
          this.dataIconList.Images.Clear();
        this.listIcon.Clear();

        System.GC.Collect();


        this.resourceFilePath = dialog.FileName;
        this.currentMode      = ResourceEditMode.Edit;

        // TODO: ここで, リソースファイルを展開する.
        try {
          using(ResXResourceSet set = new ResXResourceSet(this.resourceFilePath)) {
            IDictionaryEnumerator dict = set.GetEnumerator();

            while(dict.MoveNext()) {
              string key = (string)dict.Key;
              object v   = (object)dict.Value;


              // is で識別
              if(v is string) { // 文字列
                int             idx = this.dataString.Rows.Count - 1;
                DataGridViewRow row = new DataGridViewRow();

                row.Cells.Add(new DataGridViewTextBoxCell() { Value = key });
                row.Cells.Add(new DataGridViewTextBoxCell() { Value = (string)v });

                //row.Cells[0].Value = key;
                //row.Cells[1].Value = (string)v;

                this.dataString.Rows.Add(row);
#if _DEBUG_
                Console.WriteLine("key:{0} is string.(value:{1})", key, (string)v);
#endif
              }

              // FIXME: Image, Icon のリソース名を格納する ListView のプロパティ ``Tag'' に絶対パスを
              // 格納しているため, 現在のままでは不具合が生じる.
              else if(v is Image) { // 画像
                Image _v = (Image)v;

                this.listImage.Add(_v);
                this.dataImage.Items.Add(
                    new ListViewItem(key) { ImageIndex = this.listImage.Count - 1 });
                this.dataImageList.Images.Add(_v);
#if _DEBUG_
                Console.WriteLine("key:{0} is Image.", key);
                Console.WriteLine("{0} size {1}x{2}", key, _v.Width, _v.Height);
#endif
              }

              else if(v is Icon) { // アイコン画像
                Icon _v = (Icon)v;

                this.listIcon.Add(_v);
                this.dataIcon.Items.Add(
                    new ListViewItem(key) { ImageIndex = this.listIcon.Count - 1 });
                this.dataIconList.Images.Add(_v);
#if _DEBUG_
                Console.WriteLine("key:{0} is Icon.", key);
#endif
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

    // 追加ボタン処理
    /// <summary>リソース追加処理</summary>
#region private void onClickAddButton(object, EventArgs)
    private void onClickAddButton(object sender, EventArgs e)
    {
      string         filter = null;

      DialogResult   result;
      OpenFileDialog dialog;

      Image image;
      Icon  icon;

      ListViewItem lsvItem;


#if _DEBUG_
      Console.WriteLine("### [D] Add Resource Type:{0}", this.currentType);
#endif

      switch(this.currentType) {
        case ResourceType.Strings:
          // TODO: 文字列リソースの追加
          break;

        case ResourceType.Images:
        case ResourceType.Icons:
          if(this.currentType == ResourceType.Images)
            filter = "Image Files(*.bmp;*.jpg;*.jpeg;*.png)|*.bmp;*.jpg;*.jpeg;*.png|All files(*.*)|*.*";
          else if(this.currentType == ResourceType.Icons)
            filter = "Icon files(*.ico)|*.ico|All files(*.*)|*.*";
          else
            return;

          // Icon, Image を開くダイアログ
          dialog = new OpenFileDialog();
          dialog.Title            = "Open";
          dialog.Multiselect      = true;
          dialog.Filter           = filter;
          dialog.FilterIndex      = 1;
          dialog.InitialDirectory = ".";
          dialog.RestoreDirectory = true;

          result = dialog.ShowDialog();
          if(result == DialogResult.OK) {
            // 複数選択時の処理
            foreach(string path in dialog.FileNames) {
#if _DEBUG_
              Console.WriteLine("### [D] Selected File Name: {0}", path);
#endif

              lsvItem     = new ListViewItem(Path.GetFileName(path));
              lsvItem.Tag = (string)path;

              // 通常画像
              if(this.currentType == ResourceType.Images) {
                try {
                  image              = Image.FromFile(path);
                  image.Tag          = (object)Path.GetFileName(path);
                  lsvItem.ImageIndex = this.dataImage.Items.Count;

                  this.dataImage.Items.Add(lsvItem);
                  
                  this.listImage.Add(image);
                  this.dataImageList.Images.Add(image);
                }
                catch(Exception except) {
                  MessageBox.Show(
                      except.Message, "Image File Error",
                      MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
              } // image

              // アイコン画像
              else if(this.currentType == ResourceType.Icons) {
                try {
                  icon               = new Icon(path);
                  //icon.Tag           = (object)Path.GetFileName(path);
                  lsvItem.ImageIndex = this.dataIcon.Items.Count;

                  this.listIcon.Add(icon);
                  this.dataIcon.Items.Add(lsvItem);
                  this.dataIconList.Images.Add(icon);
                }
                catch(Exception except) {
                  MessageBox.Show(
                      except.Message, "Icon File Error",
                      MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
              } // icon
            } // foreach(string)

          } // if(result)

          dialog.Dispose();
          dialog = null;
          break;
      }
    }
#endregion
    
    // 削除ボタン処理
    /// <summary>リソース削除処理</summary>
#region private void onClickDeleteButton(object, EventArgs)
    private void onClickDeleteButton(object sender, EventArgs e)
    {
      switch(this.currentType) {
        case ResourceType.Strings:
          break;

        case ResourceType.Images: // 通常画像のリソースを削除
          if(this.dataImage.SelectedItems.Count > 0) {
            foreach(ListViewItem _item in this.dataImage.SelectedItems) {
              int _image_index = _item.ImageIndex;

              this.listImage.RemoveAt(_image_index);
              this.dataImage.Items.Remove(_item);
              if(_image_index > 0) {
                this.dataImageList.Images.RemoveAt(_image_index);
              }
            }
          }
          else {
            MessageBox.Show(
                "Image has not been selected.", "Selected image error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
          }

          break;


        case ResourceType.Icons: // アイコン画像のリソースを削除
          if(this.dataIcon.SelectedItems.Count > 0) {
            foreach(ListViewItem _item in this.dataIcon.SelectedItems) {
              int _icon_index = _item.ImageIndex;

              this.listIcon.RemoveAt(_icon_index);
              this.dataIcon.Items.Remove(_item);
              if(_icon_index > 0) {
                this.dataIconList.Images.RemoveAt(_icon_index);
              }
            }
          }
          else {
            MessageBox.Show("Icon has not been selected.", "Selected icon error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
          }

          break;
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
