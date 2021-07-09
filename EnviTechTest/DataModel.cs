using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EnviTechTest
{
    class DataModel: INotifyPropertyChanged
    {
        private DateTime? fromDate;
        public DateTime? FromDate
        {
            get { return fromDate; }
            set { fromDate = value;
                OnPropertyChanged();
            }
        }
        
        private DateTime? tillDate;
        public DateTime? TillDate
        {
            get { return tillDate; }
            set { tillDate = value;
                OnPropertyChanged();
            }
        }

        //public List<float> Value { get; set; }
        private List<string> value = new List<string>();

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public List<string> Value { get {return value; } set { this.value = value; } }

        public List<int> Status { get; set; }
    }
}
