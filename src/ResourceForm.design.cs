using System;
using System.Drawing;
using System.Windows.Forms;

using Live.Data;
using Live.Drawing;


namespace Live
{
  public partial class ResourceForm
  {
    /// <summary>コンポネントの初期化</summary>
#region private void InitializeComponent()
    private void InitializeComponent()
    {
      this.mainMenu = new MenuStrip();
      this.mFile    = new ToolStripMenuItem();
#if _DEBUG_
      this.mTest    = new ToolStripMenuItem();
#endif

      this.mfNew    = new ToolStripMenuItem();
      this.mfOpen   = new ToolStripMenuItem();
      this.mfSave   = new ToolStripMenuItem();
      this.mfExit   = new ToolStripMenuItem();

      this.lblResourceType = new Label();
      this.cbxResourceType = new ComboBox();

      this.btnAdd     = new Button();
      this.btnDelete  = new Button();
      
      this.dataString = new DataGridView();
      this.dataImage  = new ListView();
      this.dataIcon   = new ListView();

      this.dataImageList = new ImageList();
      this.dataIconList  = new ImageList();

      this.dataImage.LargeImageList = this.dataImageList;
      this.dataIcon.LargeImageList  = this.dataIconList;


      this.Controls.Add(this.mainMenu);
      this.Controls.Add(this.lblResourceType);
      this.Controls.Add(this.cbxResourceType);

      this.Controls.Add(this.btnAdd);
      this.Controls.Add(this.btnDelete);

      this.Controls.Add(this.dataString);
      this.Controls.Add(this.dataImage);
      this.Controls.Add(this.dataIcon);


      this.mainMenu.SuspendLayout();
      this.SuspendLayout();

      // メニュー項目
      this.mainMenu.Items.Add(this.mFile);
#if _DEBUG_
      // Test(T)
      this.mTest.Text = "&Test";
      this.mainMenu.Items.Add(this.mTest);
#endif

      this.mFile.DropDownItems.Add(this.mfNew);
      this.mFile.DropDownItems.Add(this.mfOpen);
      this.mFile.DropDownItems.Add(new ToolStripSeparator());
      this.mFile.DropDownItems.Add(this.mfSave);
      this.mFile.DropDownItems.Add(new ToolStripSeparator());
      this.mFile.DropDownItems.Add(this.mfExit);
      
      // File(F)
      this.mFile.Text = "&File";


      this.mfNew.Text          = "&New";
      this.mfNew.ShortcutKeys  = Keys.Control | Keys.N;

      this.mfOpen.Text         = "&Open";
      this.mfOpen.ShortcutKeys = Keys.Control | Keys.O;

      this.mfSave.Text         = "&Save";
      this.mfSave.ShortcutKeys = Keys.Control | Keys.S;

      this.mfExit.Text         = "E&xit";
      this.mfExit.ShortcutKeys = Keys.Alt | Keys.F4;


      // リソースタイプ
      this.lblResourceType.Text     = "Resource Type";
      this.lblResourceType.Location = new Point(10, 35);
      this.lblResourceType.Width    = 85;

      this.cbxResourceType.Location      = new Point(100, 32);
      this.cbxResourceType.DropDownStyle = ComboBoxStyle.DropDownList;

#if _MODEL_VISUAL_STUDIO_
      this.cbxResourceType.Items.AddRange(new string[] {
          "Strings", "Images", "Icons",
          "Audio", "Files"
          });
#else
      this.cbxResourceType.Items.AddRange(new string[] {
          "Strings", "Images", "Icons"
          });
#endif

      this.cbxResourceType.SelectedIndex = 0;
      this.currentType = ResourceType.Strings;


      // 追加・削除
      this.btnAdd.Text     = "&Add Resource";
      this.btnAdd.Width    = 120;
      this.btnAdd.Location = new Point(365, 35);
      this.btnAdd.Anchor   = AnchorStyles.Top | AnchorStyles.Right;

      this.btnDelete.Text     = "&Delete Resource";
      this.btnDelete.Width    = 120;
      this.btnDelete.Location = new Point(495, 35);
      this.btnDelete.Anchor   = AnchorStyles.Top | AnchorStyles.Right;


      //
      // リソース編集画面
      //

      // Strings: リソースデータ
      this.dataString.Size            = new Size(605, 360);
      this.dataString.Location        = new Point(10, 70);
      this.dataString.Anchor =
        AnchorStyles.Top    | AnchorStyles.Right |
        AnchorStyles.Bottom | AnchorStyles.Left;
      //this.dataString.ColumnCount      = 3;
      this.dataString.ColumnCount      = 2;
      this.dataString.Columns[0].Name  = "Name" ;
      this.dataString.Columns[0].Width = 150;
      this.dataString.Columns[1].Name = "Value" ;
      this.dataString.Columns[1].Width = 250;
      //this.dataString.Columns[2].Name = "Comment" ;
      //this.dataString.Columns[2].Width = 200;

      // Image: リソースデータ
      this.dataImage.Size     = new Size(605, 360);
      this.dataImage.Location = new Point(10, 70);
      this.dataImage.Visible  = false;
      this.dataImage.Anchor   =
        AnchorStyles.Top    | AnchorStyles.Right |
        AnchorStyles.Bottom | AnchorStyles.Left;

      // Icon: リソースデータ
      this.dataIcon.Size     = new Size(605, 360);
      this.dataIcon.Location = new Point(10, 70);
      this.dataIcon.Visible  = false;
      this.dataIcon.Anchor   =
        AnchorStyles.Top    | AnchorStyles.Right |
        AnchorStyles.Bottom | AnchorStyles.Left;

      // フォーム
      this.Text          = "Resource Builder";
      this.MainMenuStrip = this.mainMenu;
      try {
        this.Font = new Font("YU Gothic UI", 9);
      }
      catch(Exception e) {
        Console.Error.WriteLine(e.Message);
      }

      this.Size        = new Size(640, 480);
      this.MinimumSize = new Size(640, 480);

      //Console.WriteLine(this.dataString.Size);
      this.mainMenu.ResumeLayout(false);
      this.mainMenu.PerformLayout();
      this.ResumeLayout(false);
    }
#endregion


#if _DEBUG_
    //
    // テストメソッド
    //
    /// <summary>テスト</summary>
#region private void onClickMenuTest(object, EventArgs)
    private void onClickMenuTest(object sender, EventArgs e)
    {
    }
#endregion
#endif

    /// <summary>Dispose</summary>
#region protected override void Dispose(bool)
    private System.ComponentModel.IContainer components = null;
    protected override void Dispose(bool disposing)
    {
      if(disposing && (components != null)) {
        components.Dispose();
      }
      base.Dispose(disposing);
    }
#endregion
  }
}
