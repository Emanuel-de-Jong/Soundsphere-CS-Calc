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
                DataObject.AddPastingHandler(tb, TextBoxPasteValidateNumber);
                tb.PreviewTextInput += TextBoxValidateNumber;
                tb.TextChanged += TextBoxTextChanged;
            }
        }

        private void TextBoxValidateNumber(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.-]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void TextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            string text = tb.Text;
            if (text == "")
            {
                tb.Text = "0";
                return;
            }

            Calculate();
        }

        private void TextBoxPasteValidateNumber(object sender, DataObjectPastingEventArgs e)
        {
            bool isText = e.SourceDataObject.GetDataPresent(DataFormats.UnicodeText, true);
            if (!isText)
            {
                e.CancelCommand();
                return;
            }

            string text = e.SourceDataObject.GetData(DataFormats.UnicodeText) as string;
            Regex regex = new Regex("^[0-9.-]*$");
            if (!regex.IsMatch(text))
            {
                e.CancelCommand();
                return;
            }
        }

        private void Calculate()
        {
            float resX, resY, resW, resH;

            //Dictionary<string, int> numbs = new Dictionary<string, int>();
            //foreach (TextBox tb in numberTextBoxes)
            //{
            //    numbs.Add(tb.Name, );
            //}

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

            resX = (1 / winH) * (960 - objX1);
            resY = (1 / winW) * (objY1);

            ResX.Text = resX.ToString("0.0000");
            ResY.Text = resY.ToString("0.0000");
        }
    }
}
