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
#if _DEBUG_
    private ToolStripMenuItem mTest;
#endif

    private Label    lblResourceType;
    private ComboBox cbxResourceType;

    private Button   btnAdd, btnDelete;


    private string resourceFilePath;
    
    // ���\�[�X�ҏW
    private DataGridView dataString;
    private ListView     dataImage;
    private ListView     dataIcon;

    // �C���[�W���X�g
    private ImageList    dataImageList;
    private ImageList    dataIconList;


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
      this.mTest.Click  += new EventHandler(onClickMenuTest);
#endif
      // �t�H�[���C�x���g��ݒ�
      this.Closed += new EventHandler(this.onClosedForm);

      // ���j���[�C�x���g��ݒ�
      this.mfOpen.Click += new EventHandler(this.onClickOpenMenu);
      this.mfExit.Click += new EventHandler(this.onClickExitMenu);
      this.mfSave.Click += new EventHandler(this.onClickSaveMenu);

      // �{�^���C�x���g��ݒ�
      this.btnAdd.Click    += new EventHandler(this.onClickAddButton);
      this.btnDelete.Click += new EventHandler(this.onClickDeleteButton);

      // �R���{�{�b�N�X�C�x���g��ݒ�
      this.cbxResourceType.SelectedIndexChanged +=
        new EventHandler(this.onChangedResourceType);


      //
      // ���\�[�X�֌W�̃C�x���g��ݒ�
      //
      this.dataString.CellValueChanged += new DataGridViewCellEventHandler(this.onChangedStringResource);
      // NOTE: �s�v�̉\�������邽��, �R�����g�A�E�g
      //this.dataString.RowsAdded += new DataGridViewRowsAddedEventHandler(this.onAddedStringResource);


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
          // �ǉ��ςݕ�����̎擾
          if(this.dataString.Rows != null &&
              this.dataString.Rows.Count > 1) {
            foreach(DataGridViewRow row in this.dataString.Rows) {
              string _key, _value;

              if(!row.IsNewRow) {
                _key   = (string)row.Cells[0].Value;
                _value = (string)row.Cells[1].Value;

                writer.AddResource(_key, _value);
                //Console.WriteLine("Row[{0}] -> Key:{1}, Value:{2}", row.Index, _key, _value);
              }
            }
          }

          // NOTE: ListViewItem ����� ImageList ���Q�Ƃ��邽��, for �����g�p����.
          
          // TODO: ����, ListViewItem�̃v���p�e�B ``Tag'' �ɐݒ肳�ꂽ�I�u�W�F�N�g��
          // �摜�̃p�X���i�[���Ă���. ���̂���, �X�V(������)���s���ۂɕs���������.

          // �ǉ��ςݒʏ�摜�̎擾
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


          // �ǉ��ς݃A�C�R���摜�̎擾
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
    // �R�[���o�b�N���\�b�h: ���j���[
    //

    /// <summary>Open ���j���[�I��</summary>
#region private void onClickOpenMenu(object, EventArgs)
    private void onClickOpenMenu(object sender, EventArgs e)
    {
      OpenFileDialog dialog;
      DialogResult   result;


      // TODO: ���݂̃��\�[�X��j��.


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

        // TODO: ������, ���\�[�X�t�@�C����W�J����.
        try {
          using(ResXResourceSet set = new ResXResourceSet(this.resourceFilePath)) {
            IDictionaryEnumerator dict = set.GetEnumerator();

            while(dict.MoveNext()) {
              string key = (string)dict.Key;
              object v   = (object)dict.Value;


              // is �Ŏ���
              if(v is string) { // ������
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

              // FIXME: Image, Icon �̃��\�[�X�����i�[���� ListView �̃v���p�e�B ``Tag'' �ɐ�΃p�X��
              // �i�[���Ă��邽��, ���݂̂܂܂ł͕s���������.
              else if(v is Image) { // �摜
                Image _v = (Image)v;

#if _DEBUG_
                Console.WriteLine("key:{0} is Image.", key);
                Console.WriteLine("{0} size {1}x{2}", key, _v.Width, _v.Height);
#endif
              }

              else if(v is Icon) { // �A�C�R���摜
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

    // �ǉ��{�^������
    /// <summary>���\�[�X�ǉ�����</summary>
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
          // TODO: �����񃊃\�[�X�̒ǉ�
          break;

        case ResourceType.Images:
        case ResourceType.Icons:
          if(this.currentType == ResourceType.Images)
            filter = "Image Files(*.bmp;*.jpg;*.jpeg;*.png)|*.bmp;*.jpg;*.jpeg;*.png|All files(*.*)|*.*";
          else if(this.currentType == ResourceType.Icons)
            filter = "Icon files(*.ico)|*.ico|All files(*.*)|*.*";
          else
            return;

          // Icon, Image ���J���_�C�A���O
          dialog = new OpenFileDialog();
          dialog.Title            = "Open";
          dialog.Multiselect      = true;
          dialog.Filter           = filter;
          dialog.FilterIndex      = 1;
          dialog.InitialDirectory = ".";
          dialog.RestoreDirectory = true;

          result = dialog.ShowDialog();
          if(result == DialogResult.OK) {
            // �����I�����̏���
            foreach(string path in dialog.FileNames) {
#if _DEBUG_
              Console.WriteLine("### [D] Selected File Name: {0}", path);
#endif

              lsvItem     = new ListViewItem(Path.GetFileName(path));
              lsvItem.Tag = (string)path;

              // �ʏ�摜
              if(this.currentType == ResourceType.Images) {
                try {
                  image = Image.FromFile(path);
                  lsvItem.ImageIndex = this.dataImage.Items.Count;

                  this.dataImage.Items.Add(lsvItem);
                  this.dataImageList.Images.Add(image);
                }
                catch(Exception except) {
                  MessageBox.Show(
                      except.Message, "Image File Error",
                      MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
              } // image

              // �A�C�R���摜
              else if(this.currentType == ResourceType.Icons) {
                try {
                  icon = new Icon(path);
                  lsvItem.ImageIndex = this.dataIcon.Items.Count;

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
    
    // �폜�{�^������
    /// <summary>���\�[�X�폜����</summary>
#region private void onClickDeleteButton(object, EventArgs)
    private void onClickDeleteButton(object sender, EventArgs e)
    {
      switch(this.currentType) {
        case ResourceType.Strings:
          break;

        case ResourceType.Images: // �ʏ�摜�̃��\�[�X���폜
          if(this.dataImage.SelectedItems.Count > 0) {
            foreach(ListViewItem _item in this.dataImage.SelectedItems) {
              int _image_index = _item.ImageIndex;

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


        case ResourceType.Icons: // �A�C�R���摜�̃��\�[�X���폜
          if(this.dataIcon.SelectedItems.Count > 0) {
            foreach(ListViewItem _item in this.dataIcon.SelectedItems) {
              int _icon_index = _item.ImageIndex;

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
