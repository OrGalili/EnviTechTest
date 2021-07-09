using Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EnviTechTest
{
    class ViewModel:INotifyPropertyChanged
    {
        private string value;
        public string Value
        {
            get { return value; }
            set
            {
                Regex regex = new Regex("^[+-]?([0-9]+([.][0-9]*)?|[.][0-9]+)$");
                if (regex.IsMatch(value) || value.Length == 0)
                    this.value = value;
                OnPropertyChanged();
            }
        }
        private int selectedOperator = -1;
        public int SelectedOperator
        {
            get { return selectedOperator; }
            set { selectedOperator = value;
                OnPropertyChanged();
            }
        }

        private int selectedValue = -1;
        public int SelectedValue
        {
            get { return selectedValue; }
            set { selectedValue = value;
                OnPropertyChanged();
            }
        }

        private DataModel data = new DataModel();

        public DataModel Data
        {
            get { return data; }
            set { data = value;
                OnPropertyChanged();
            }
        }

        private OperatorModel _operator = new OperatorModel();

        public OperatorModel Operator
        {
            get { return _operator; }
            set { _operator = value; }
        }

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
            ClearCommand = new DelegateCommand<string>(clearControls);
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public DelegateCommand<string> ShowCommand { get; set; }

        public DelegateCommand<string> ClearCommand { get; set; }

        public void ShowTable(string none)
        {
            if(Table.Table != null)
            {
                Table.Table.Rows.Clear();
            }
            
            if (String.IsNullOrEmpty(value) && selectedValue ==-1 && selectedOperator ==-1 && Data.FromDate==null && Data.TillDate ==null)
            {
                DataTable dt = DataAccess.GetDataTable("SELECT Date_Time"+values+" FROM DATA", "DATA");
                Table = dt.DefaultView;
            }
            else if (!String.IsNullOrEmpty(value) && selectedValue!=-1 && selectedOperator !=-1 && Data.FromDate!=null && Data.TillDate != null)
            {
                string d1 = new DateTime(Data.FromDate.Value.Year, Data.FromDate.Value.Month, Data.FromDate.Value.Day).ToString("yyyy-MM-dd");
                string d2 = new DateTime(Data.TillDate.Value.Year, Data.TillDate.Value.Month, Data.TillDate.Value.Day).ToString("yyyy-MM-dd");
                
                DataTable dt = DataAccess.GetDataTable("SELECT Date_Time"+values+" FROM DATA where Value"+(selectedValue+1)+Operator.Name[SelectedOperator]+float.Parse(Value)+" and Date_Time >'" +d1+"' and Date_Time < '"+d2+"'", "DATA");
                Table = dt.DefaultView;
            }
        }
        public void clearControls(string none)
        {
            Value = "";
            SelectedValue = -1;
            SelectedOperator = -1;
            Data.FromDate = null;
            Data.TillDate = null;
            OnPropertyChanged("TillDate");
        }

        private string values = "";
        private void FillValues()
        {
            object obj = DataAccess.ExecuteScalar("SELECT count(COLUMN_NAME) FROM INFORMATION_SCHEMA.COLUMNS WHERE COLUMN_NAME like 'value%' and TABLE_NAME = 'DATA'");
            int numberOfValues = 0;
            if (obj is int)
                numberOfValues = int.Parse(obj.ToString());

            for (int i = 1; i <= numberOfValues; i++)
            {
                data.Value.Add("Value" + (i));
                values += ", Value" + i;
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
