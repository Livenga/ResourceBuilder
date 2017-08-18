using System;
using System.Drawing;
using System.Windows.Forms;

using Live.Data;
using Live.Drawing;


namespace Live.Forms
{
  public partial class ResourceForm
  {
    /// <summary></summary>
#region private void InitializeComponent()
    private void InitializeComponent()
    {
      this.mainMenu = new MenuStrip();
      this.mFile    = new ToolStripMenuItem();
      this.mfNew    = new ToolStripMenuItem();
      this.mfOpen   = new ToolStripMenuItem();
      this.mfSave   = new ToolStripMenuItem();
      this.mfExit   = new ToolStripMenuItem();

      this.lblResourceType = new Label();
      this.cbxResourceType = new ComboBox();

      this.dataString = new DataGridView();

      this.btnAdd     = new Button();
      this.btnDelete  = new Button();

      this.Controls.Add(this.mainMenu);
      this.Controls.Add(this.lblResourceType);
      this.Controls.Add(this.cbxResourceType);

      this.Controls.Add(this.btnAdd);
      this.Controls.Add(this.btnDelete);

      this.Controls.Add(this.dataString);


      this.mainMenu.SuspendLayout();
      this.SuspendLayout();

      // メニュー項目
      this.mainMenu.Items.Add(this.mFile);
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
      this.cbxResourceType.Items.AddRange(new string[] {
          "Strings", "Images", "Icons", "Audio", "Files"
          });
      this.cbxResourceType.SelectedIndex = 0;
      this.currentType = ResourceType.Strings;


      // 追加・削除
      this.btnAdd.Text     = "&Add Resource";
      this.btnAdd.Width    = 120;
      this.btnAdd.Location = new Point(350, 35);
      this.btnAdd.Anchor   = AnchorStyles.Top | AnchorStyles.Right;

      this.btnDelete.Text     = "&Delete Resource";
      this.btnDelete.Width    = 120;
      this.btnDelete.Location = new Point(480, 35);
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
      this.dataString.ColumnCount      = 3;
      this.dataString.Columns[0].Name  = "Name" ;
      this.dataString.Columns[0].Width = 150;
      this.dataString.Columns[1].Name = "Value" ;
      this.dataString.Columns[1].Width = 250;
      this.dataString.Columns[2].Name = "Comment" ;
      this.dataString.Columns[2].Width = 200;

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

      Console.WriteLine(this.dataString.Size);
      this.mainMenu.ResumeLayout(false);
      this.mainMenu.PerformLayout();
      this.ResumeLayout(false);
    }
#endregion

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
