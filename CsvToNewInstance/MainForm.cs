using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CsvToNewInstance
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            this._converter = new ConvertCsvToNewInstance();
        }

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            this._converter.InitialPrivateData();
            string folderPath = Application.ExecutablePath;
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = folderPath;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                SetFileName(ofd.FileName);
            }
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this._fileAbsolutePath))
            {
                MessageBox.Show("請選擇檔案~!!");
                return;
            }

            this._converter.InitialPrivateData();

            using (var fileStream = new StreamReader(this._fileAbsolutePath, System.Text.Encoding.Default))
            {
                string eachLine = string.Empty;

                try
                {
                    this._converter.LineCount = 0;
                    while ((eachLine = fileStream.ReadLine()) != null)
                    {
                        this._converter.InputEachLine(eachLine);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

                tbxResult.Text = this._converter.Result;
        }

        private void cbxDelimiter_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch ((sender as ComboBox).SelectedItem.ToString())
            {
                case "Tab":
                    this._converter.Delimiter = '\t';
                    return;
                default:
                    this._converter.Delimiter = ',';
                    return;
            }
        }

        private void MainForm_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            var files = e.Data.GetData(DataFormats.FileDrop, false) as string[];
            if (files.Count() > 1)
            {
                MessageBox.Show("目前只支援單檔處理");
                return;
            }

            SetFileName(files[0]);
        }

        private void SetFileName(string fileAbsolutePath)
        {
            this._fileAbsolutePath = fileAbsolutePath;
            var lastIndexOfSlash = fileAbsolutePath.LastIndexOf('\\');

            // +1 是為了不取到 /
            var fileName = fileAbsolutePath.Substring(lastIndexOfSlash + 1);

            this._converter.ClassName = fileName.Split('.')[0];

            lblFilePath.Text = fileAbsolutePath;

            tbxResult.Text = string.Empty;
        }

        private ConvertCsvToNewInstance _converter;
        private string _fileAbsolutePath;
    }
}
