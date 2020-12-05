using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;

namespace Soundsphere_CS_Calc
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TextBox[] numberTextBoxes;

        public MainWindow()
        {
            InitializeComponent();

            numberTextBoxes = new TextBox[] { WinW, WinH, CSX, CSY, CSA, CSB, ObjX1, ObjX2, ObjY1, ObjY2 };

            foreach (TextBox tb in numberTextBoxes)
            {
                DataObject.AddPastingHandler(tb, TextBox_PasteValidate);
                tb.PreviewTextInput += TextBox_Validate;
                tb.TextChanged += TextBox_TextChanged;
            }

            CSBind.SelectionChanged += ComboBox_SelectionChanged;
        }

        private void TextBox_PasteValidate(object sender, DataObjectPastingEventArgs e)
        {
            bool isText = e.SourceDataObject.GetDataPresent(DataFormats.UnicodeText, true);
            if (!isText)
            {
                e.CancelCommand();
                return;
            }

            string text = e.SourceDataObject.GetData(DataFormats.UnicodeText) as string;
            Regex regex = new Regex("^-?([0-9][0-9]*)(\\.[0-9]+)?$");
            if (!regex.IsMatch(text))
            {
                e.CancelCommand();
                return;
            }
        }

        private void TextBox_Validate(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.-]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!validate())
            {
                return;
            }

            Calculate();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!validate())
            {
                return;
            }

            Calculate();
        }

        private bool validate()
        {
            Regex regex = new Regex("^-?([0-9][0-9]*)(\\.[0-9]+)?$");

            foreach (TextBox tb in numberTextBoxes)
            {
                if (tb.Text == "" || !regex.IsMatch(tb.Text))
                {
                    return false;
                }
            }

            return true;
        }

        private void Calculate()
        {
            float winW = float.Parse(WinW.Text);
            float winH = float.Parse(WinH.Text);
            float csX = float.Parse(CSX.Text);
            float csY = float.Parse(CSY.Text);
            float csA = float.Parse(CSA.Text);
            float csB = float.Parse(CSB.Text);
            float objX1 = float.Parse(ObjX1.Text);
            float objX2 = float.Parse(ObjX2.Text);
            float objY1 = float.Parse(ObjY1.Text);
            float objY2 = float.Parse(ObjY2.Text);
            string csBind = CSBind.SelectedValue.ToString();

            float baseV;
            switch (csBind)
            {
                case "w":
                    baseV = 1 / winW;
                    break;
                case "h":
                    baseV = 1 / winH;
                    break;
                default:
                    baseV = 1 / 100;
                    break;
            }

            float winWCS = winW * csX;
            float winHCS = winH * csY;

            float resX, resY, resW, resH;
            resX = baseV * (objX1 - winWCS);
            resY = baseV * (objY1 - winHCS);
            resW = (baseV * (objX2 - winWCS)) - resX;
            resH = (baseV * (objY2 - winHCS)) - resY;

            string format = "0.0000";
            ResX.Text = resX.ToString(format);
            ResY.Text = resY.ToString(format);
            ResW.Text = resW.ToString(format);
            ResH.Text = resH.ToString(format);
        }
    }
}
