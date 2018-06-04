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
using System.Runtime.InteropServices;


namespace PerkFactory
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        AttributeWindow AttrWin; 


       
        public MainWindow()
        {
            InitializeComponent();
            AttrWin = new AttributeWindow();
            Export_Success.Visibility = Visibility.Hidden;
        }

      

        private void AddNewAttribute_Click(object sender, RoutedEventArgs e)
        {

            Export_Success.Visibility = Visibility.Hidden;
            AttrWin.Show();
            
        }
        [DllImport("../../../../PerkFactory_Exporter/Release/PerkFactory_Exporter.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        public static extern void Export2(byte[] name, int nameSize,int[] ValueArr, int[] TypeArr, int[]Checks ,int CheckSize, int TypeSize,int Condition);
       
      
        private void ExportPerk_Click(object sender, RoutedEventArgs e)
        {
            
             
           
            int TypeSize = AttrWin.AttributeTypes.Count;
            int Checksize = AttrWin.CheckValues.Count;

            int ConditionSwitch = AttrWin.condition;

            int[] Values = AttrWin.AttributeValues.ToArray();
            int[] Types = AttrWin.AttributeTypes.ToArray();
            int[] Checks = AttrWin.CheckValues.ToArray();

            string input = Name_Input.Text;

            byte[] send = Encoding.Default.GetBytes(input);


            Export2(send, send.Length, Values, Types, Checks, Checksize, TypeSize, ConditionSwitch);

            Export_Success.Visibility = Visibility.Visible;
        }

        private void AttributeListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Remove_attribute_Click(object sender, RoutedEventArgs e)
        {
            if (AttributeListBox.SelectedItems.Count !=0 )
            {
                while (AttributeListBox.SelectedItems.Count>0)
                {
                    AttrWin.RemoveItems(AttributeListBox.SelectedItem.ToString());
                    
                    AttributeListBox.Items.Remove(AttributeListBox.SelectedItem);
                }
            }
        }
    }
}
