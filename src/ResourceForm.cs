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
    // UI �֌W
    private MenuStrip         mainMenu;
    private ToolStripMenuItem mFile, mfNew, mfOpen, mfSave, mfExit;

    private Label    lblResourceType;
    private ComboBox cbxResourceType;

    private Button   btnAdd, btnDelete;


    private string resourceFilePath;
    
    // ���\�[�X�ҏW
    private DataGridView dataString;
    private ListView     dataImage;
    private ListView     dataIcon;


    //
    private ResourceEditMode currentMode   = ResourceEditMode.New;
    private ResourceType     currentType   = ResourceType.Strings;
    private ResourceStatus   currentStatus = ResourceStatus.Saved;


    //
    // �t�H�[���R���X�g���N�^
    //

    /// <summary>�R���X�g���N�^</summary>
#region public ResourceForm()
    public ResourceForm()
    {
      this.InitializeComponent();

      
      // �R�[���o�b�N�C�x���g�̐ݒ�
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

      // UI �n(ResourceForm.design.cs �ɋL�q)
      //this.lblResourceType.Click += new EventHandler(this.onClickResourceTypeLabel);
    }
#endregion


    //
    // ���p���\�b�h
    //

    /// <summary>���\�[�X�ҏW��ʂ̕\��</summary>
    /// <param name="type">�ҏW���郊�\�[�X�̎��</param>
#region private void DisplayResourceEditor(ResourceType)
    private void DisplayResourceEditor(ResourceType type)
    {
      this.dataString.Visible = (ResourceType.Strings == type) ? true : false;
      this.dataImage.Visible  = (ResourceType.Images  == type) ? true : false;
      this.dataIcon.Visible   = (ResourceType.Icons   == type) ? true : false;
    }
#endregion

    /// <summary>���\�[�X�t�@�C���̕ۑ�</summary>
#region private void SaveResourceFile()
    private void SaveResourceFile()
    {
      SaveFileDialog dialog;
      DialogResult   result;

      if(this.resourceFilePath != null &&
          this.currentMode == ResourceEditMode.Edit) { // �㏑���ۑ�
#if _DEBUG_
        Console.WriteLine("### [D] Overwrite {0}", this.resourceFilePath);
#endif
      }
      else { // �V�K�쐬
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


      // �t�@�C���p�X��p����, ���\�[�X�������ݏ���
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
    // �R�[���o�b�N���\�b�h: ���j���[
    //

    /// <summary>Open ���j���[�I��</summary>
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
        // TODO: ������, ���\�[�X�t�@�C����W�J����.
        try {
          using(ResXResourceSet set = new ResXResourceSet(this.resourceFilePath)) {
            IDictionaryEnumerator dict = set.GetEnumerator();

            while(dict.MoveNext()) {
              string key = (string)dict.Key;
              object v   = (object)dict.Value;


              // is �Ŏ���
              if(v is string) { // ������
                Console.WriteLine("key:{0} is string.", key);
              }

              else if(v is Bitmap) { // �摜
                Console.WriteLine("key:{0} is bitmap.", key);
              }

              else if(v is SoundPlayer) { // ����
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

    /// <summary>Save ���j���[�I�� ���\�[�X�t�@�C���̏�������</summary>
#region private void onClickSaveMenu(object, EventArgs)
    private void onClickSaveMenu(object sender, EventArgs e)
    {
      this.SaveResourceFile();
    }
#endregion

    /// <summary>Exit ���j���[�I��</summary>
#region private void onClickExitMenu(object, EventArgs)
    private void onClickExitMenu(object sender, EventArgs e)
    {
      this.Close();
    }
#endregion

    /// <summary>���\�[�X��ނ̕ύX</summary>
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

      // ���\�[�X�ҏW��ʂ�J��
      this.DisplayResourceEditor(this.currentType);
    }
#endregion

    //
    // �R�[���o�b�N���\�b�h: �{�^��
    //

    // �ǉ��E�폜�{�^������
    /// <summary></summary>
#region private void onClickEditButton(object, EventArgs)
    private void onClickEditButton(object sender, EventArgs e)
    {
      Button btn = (Button)sender;


      // �f�[�^�̒ǉ�
      if(btn == this.btnAdd) {
      }

      // �f�[�^�̍폜
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
    // �R�[���o�b�N���\�b�h: �t�H�[��
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
