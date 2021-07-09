using Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EnviTechTest
{
    class ViewModel:INotifyPropertyChanged
    {

        public string SelectedOperator { get; set; }
        public string SelectedValue { get; set; }

        private DataModel data = new DataModel();

        public DataModel Data { get { return data; } set { data = value; } }

        private OperatorModel _operator = new OperatorModel();

        public OperatorModel Operator { get {return _operator; } set {_operator = value; } }

        private StatusModel status;

        public StatusModel Status { get; set; }

        private DataView table = new DataView();

        public event PropertyChangedEventHandler PropertyChanged;

        public DataView Table
        {
            get { return table; }
            set
            {
                table = value;
                OnPropertyChanged();
            }
        }


        public ViewModel() 
        {
            data = new DataModel();
            FillValues();
            FillOperators();
            ShowCommand = new DelegateCommand<string>(ShowTable);
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public DelegateCommand<string> ShowCommand { get; set; }

        public void ShowTable(string none)
        {
            DataTable dt = DataAccess.GetDataTable("SELECT * FROM DATA", "DATA");
            Table = dt.DefaultView;
        }

        private void FillValues()
        {
            object obj = DataAccess.ExecuteScalar("SELECT count(COLUMN_NAME) FROM INFORMATION_SCHEMA.COLUMNS WHERE COLUMN_NAME like 'value%' and TABLE_NAME = 'DATA'");
            int numberOfValues = 0;
            if (obj is int)
                numberOfValues = int.Parse(obj.ToString());

            for (int i = 0; i < numberOfValues; i++)
            {
                data.Value.Add("Value" + (i + 1));

            }
        }

        private void FillOperators()
        {
            DataTable operatorTable = DataAccess.GetDataTable("select Name from OPERATOR", "Operator");
            foreach(DataRow row in operatorTable.Rows)
            {
                _operator.Name.Add(row[0].ToString());
            }
        }
    }
}
