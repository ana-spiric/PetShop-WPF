using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using FontAwesome.Sharp;

namespace Pet_Shop_HCI.classes
{
    public class MainViewModel : ViewModelBase
    {
        private ViewModelBase currentChildView;
        private string caption;
        private IconChar icon;

        public ViewModelBase CurrentChildView
        {
            get
            {
                return currentChildView;
            }
            set
            {
                currentChildView = value;
                onPropertyChanged(nameof(CurrentChildView));
            }
        }

        public string Caption
        {
            get
            {
                return caption;
            }
            set
            {
                caption = value;
                onPropertyChanged(nameof(Caption));
            }
        }

        public IconChar Icon
        {
            get
            {
                return icon;
            }
            set
            {
                icon = value;
                onPropertyChanged(nameof(Icon));
            }
        }

        public ICommand ShowEmployeeViewCommand { get; }

        public MainViewModel()
        {
            ShowEmployeeViewCommand = new ViewModelCommand(ExecuteShowEmployeeCommand);
            ExecuteShowEmployeeCommand(null);
        }

        private void ExecuteShowEmployeeCommand(object ob)
        {
            CurrentChildView = new PregledRadnika();
            Caption = "Pregled radnika";
            Icon = IconChar.ListOl;
        }
    }
}
