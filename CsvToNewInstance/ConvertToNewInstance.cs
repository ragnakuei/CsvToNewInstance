using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvToNewInstance
{
    public class ConvertCsvToNewInstance
    {
        public ConvertCsvToNewInstance()
        {
            InitialPrivateData();
        }

        public void InputEachLine(string line)
        {
            if (LineCount == 0)
            {
                var tmpFields = line.Split(Delimiter);
                Fields = tmpFields.ToList();
            }
            else
            {
                ResultByList.Add(CombineEachLine(line.Split(Delimiter).ToList()));
            }

            LineCount++;
        }

        private string CombineEachLine(List<string> values)
        {
            if (Fields.Count != values.Count) throw new Exception($"第 {LineCount + 1} 行的欄位數量不一致，請重新確認 csv 欄位數量");
            var result = string.Empty;
            result += $"new {ClassName} {{ ";
            var fieldValue = Fields.Zip(values, (f, v) => $"{f} = {v}, ");
            result += string.Concat(fieldValue);
            result += $"}},\n";

            return result;
        }

        public void InitialPrivateData()
        {
            LineCount = 0;
            // ClassName = string.Empty;
            // Fields = new List<string>();
            ResultByList = new List<string>();
        }

        public int LineCount
        {
            get { return this._lineCount; }
            set { this._lineCount = value; }
        }

        public string ClassName
        {
            get { return this._className; }
            set { this._className = value; }
        }

        public List<string> ResultByList { get; private set; }

        public string Result
        {
            get
            {
                return string.Concat(ResultByList);
            }
        }

        public char Delimiter
        {
            get { return this._delimiter; }
            set { this._delimiter = value; }
        }

        private List<string> Fields
        {
            get { return this._fields; }
            set { this._fields = value; }
        }

        private int _lineCount = 0 ;
        private string _className = string.Empty;
        private List<string> _fields = new List<string>();
        private char _delimiter = ',';
    }
}
