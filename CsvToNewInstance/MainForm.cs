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
            string folderPath = Application.ExecutablePath;
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = folderPath;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                this._converter.ClassName = ofd.SafeFileName;

                this._fileAbsolutePath = ofd.FileName;
                lblFilePath.Text = ofd.FileName;
            }
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this._fileAbsolutePath))
            {
                MessageBox.Show("請選擇檔案~!!");
                return;
            }

            var fileStream = new StreamReader(this._fileAbsolutePath);
            string eachLine = string.Empty;

            while ((eachLine = fileStream.ReadLine()) != null)
            {
                this._converter.InputEachLine(eachLine);
            }

            tbxResult.Text = this._converter.Result;
        }

        private ConvertCsvToNewInstance _converter;
        private string _fileAbsolutePath;
    }
}
