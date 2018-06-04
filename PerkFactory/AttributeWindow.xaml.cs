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
using System.Windows.Shapes;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace PerkFactory
{
    /// <summary>
    /// Interaction logic for AttributeWindow.xaml
    /// </summary>
    public partial class AttributeWindow : Window
    {
        

        List<Grid> GridList = new List<Grid>();
        public  List<string> nameList = new List<string>();
        public List<int> AttributeValues = new List<int>();
        public List<int> AttributeTypes = new List<int>();
        public List<int> CheckValues = new List<int>();
        public List<string> TagListValues = new List<string>();
        public List<string> TagListChecks = new List<string>();

        public int condition = 0;

        public AttributeWindow()
        {
            InitializeComponent();

            ChoseAttributeListBox.Items.Add("Lifesteal");
            ChoseAttributeListBox.Items.Add("Resistance");
            ChoseAttributeListBox.Items.Add("Stats");
            ChoseAttributeListBox.Items.Add("Damage");
            ChoseAttributeListBox.Items.Add("Immunity");
            ChoseAttributeListBox.Items.Add("Disallow_Weapon");
            ChoseAttributeListBox.Items.Add("Skills");
            ChoseAttributeListBox.Items.Add("Conditions");

            FillGridList();
            fillNameList();
            HideGrids();


        }

        private void ConfirmAttributeBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ChoseAttributeListBox.SelectedItems.Count != 0)
            {
                foreach (Grid CurrentGrid in GridList)
                {  

                     if (ChoseAttributeListBox.SelectedItem.ToString() == CurrentGrid.Name)
                     {
                        ExportAttributes(CurrentGrid);
                     }
                
                }

            }
        }

        private void AttributeCancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }



        private void ChoseAttributeListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            foreach (Grid CurrentGrid in GridList)
            {
                if (ChoseAttributeListBox.SelectedItem.ToString() == CurrentGrid.Name)
                {
                    HideAllExceptOne(CurrentGrid);
                }
            }

        }


        private void HideGrids()
        {
            foreach (Grid myGrid in GridList)
            {
                myGrid.Visibility = Visibility.Hidden;
            }
        }

        private void HideAllExceptOne(Grid ThisGrid)
        {
            foreach (Grid aGrid in GridList)
            {
                if (aGrid != ThisGrid)
                {
                    aGrid.Visibility = Visibility.Hidden;
                }
                else
                {
                    aGrid.Visibility = Visibility.Visible;
                }
            }
        }

        private void FillGridList()
        {
            GridList.Add(Lifesteal);
            GridList.Add(Resistance);
            GridList.Add(Stats);
            GridList.Add(Damage);
            GridList.Add(Immunity);
            GridList.Add(Disallow_Weapon);
            GridList.Add(Skills);
            GridList.Add(Conditions);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void ExportAttributes(Grid MyGrid)
        {
            for (int i = 0; i < MyGrid.Children.Count; i++)
            {
                if (MyGrid.Children[i].GetType() == typeof(TextBox))
                {
                    TextBox MyBox = (TextBox)MyGrid.Children[i];
                    
                    string choice = "";

                    List<object> removeList = new List<object>();

                    string text = MyBox.Text.ToString();
                    decimal test = 0;

                    bool succesfull = Decimal.TryParse(text, out test);
                    bool print = false;
                    int type = IdentifyEnum(MyBox.Name.ToString());

                    if (test < 0)
                    {
                        print = true;
                        choice += "Decreasing ";
                    }
                    else if (test > 0 )
                    {
                        print = true;
                        choice += "Increasing ";
                    }
                    

                    string compare = MyBox.Tag.ToString();
                    int t = ((MainWindow)Application.Current.MainWindow).AttributeListBox.Items.Count;
                    for (int p = 0; p < t; p++)
                    {
                        string comparison = ((MainWindow)Application.Current.MainWindow).AttributeListBox.Items[p].ToString();
                        if (comparison.Contains(compare))
                        {
                            removeList.Add(((MainWindow)Application.Current.MainWindow).AttributeListBox.Items[p]);
                        }
                    }
                    

                    for (int j = 0; j < removeList.Count; j++)
                    {
                        ((MainWindow)Application.Current.MainWindow).AttributeListBox.Items.Remove(removeList[j]);

                        
                    }

                    for (int k = 0; k < AttributeTypes.Count; k++)
                    {
                        if (type == AttributeTypes[k])
                        {
                            TagListValues.RemoveAt(k);
                            AttributeTypes.RemoveAt(k);
                            AttributeValues.RemoveAt(k);
                        }
                    }

                    if (print)
                    {
                        choice += MyBox.Tag + " ";
                        
                        choice += MyBox.Text;
                        ((MainWindow)Application.Current.MainWindow).AttributeListBox.Items.Add(choice);


                        

                        if (type != -1)
                        {
                            TagListValues.Add(MyBox.Tag.ToString());
                            AttributeTypes.Add(type);
                            AttributeValues.Add((int)test);
                            // call for DLL function
                        }
                        
                    }





                }
                else if (MyGrid.Children[i].GetType() == typeof(CheckBox))
                {
                    CheckBox MyCheckbox = (CheckBox)MyGrid.Children[i];
                    
                    
                    if (MyCheckbox.IsChecked.Value == true)
                    {
                        if (!((MainWindow)Application.Current.MainWindow).AttributeListBox.Items.Contains(MyCheckbox.Tag))
                        {
                            ((MainWindow)Application.Current.MainWindow).AttributeListBox.Items.Add(MyCheckbox.Tag);

                            int type = IdentifyEnum(MyCheckbox.Name.ToString());

                            if (type != -1)
                            {
                                TagListChecks.Add(MyCheckbox.Tag.ToString());
                                CheckValues.Add(type);
                            }
                        }
                       
                    }
                    else
                    {
                        if (((MainWindow)Application.Current.MainWindow).AttributeListBox.Items.Contains(MyCheckbox.Tag))
                        {
                            ((MainWindow)Application.Current.MainWindow).AttributeListBox.Items.Remove(MyCheckbox.Tag);

                            int type = IdentifyEnum(MyCheckbox.Name.ToString());

                            if (type != -1)
                            {
                                for (int j = 0; j < CheckValues.Count; j++)
                                {
                                    if (type == CheckValues[j])
                                    {
                                        TagListChecks.RemoveAt(j);
                                        CheckValues.RemoveAt(j);
                                        break;
                                    }

                                }
                                
                            }
                        }
                    }
      
                    
                } 
            }
        }

        public void RemoveItems(string itemName)
        {
            for (int i = 0; i < TagListValues.Count; i++)
            {
                if (itemName.Contains(TagListValues[i]))
                {
                    TagListValues.RemoveAt(i);
                    AttributeValues.RemoveAt(i);
                    AttributeTypes.RemoveAt(i);
                }
            }
            for (int j = 0; j < TagListChecks.Count; j++)
            {
                if (itemName.Contains(TagListChecks[j]))
                {
                    TagListChecks.RemoveAt(j);
                    CheckValues.RemoveAt(j);
                }
            }

        }

        private void fillNameList()
        {
            nameList.Add("LifeStealTxtBox");                    // 0
            nameList.Add("PhysicalResistanceTxtBox");           // 1
            nameList.Add("WaterResistanceTxtBox");              // 2
            nameList.Add("FireResistanceTxtBox");               // 3
            nameList.Add("NatureResistanceTxtBox");             // 4
            nameList.Add("Magic_Resistance_Txtbox");            // 5
            nameList.Add("Ranged_Resistance_Txtbox");           // 6
            nameList.Add("StrFlat");                            // 7
            nameList.Add("AgiFlat");                            // 8
            nameList.Add("IntFlat");                            // 9
            nameList.Add("HpFlat");                             // 10
            nameList.Add("StrMlp");                             // 11
            nameList.Add("IntMlp");                             // 12
            nameList.Add("AgiMlp");                             // 13
            nameList.Add("HpMlp");                              // 14
            nameList.Add("Dmg_Txtbox");                         // 15
            nameList.Add("Melee_Range_Txtbox");                 // 16
            nameList.Add("Melee_Dmg_Txtbox");                   // 17
            nameList.Add("Range_Dmg_Txtbox");                   // 18
            nameList.Add("AtkSpd_Txtbox");                      // 19
            nameList.Add("Melee_AtkSpd_Txtbox");                // 20
            nameList.Add("Cons_Dmg_Txtbox");                    // 21
            nameList.Add("Cons_AtkSpd_Txtbox");                 // 22
            nameList.Add("Cons_MeleeDmg_Txtbox");               // 23
            nameList.Add("Cons_RangeDmg_Txtbox");               // 24
            nameList.Add("Cons_Melee_AtkSpd_Txtbox");           // 25
            nameList.Add("Cons_Range_AtkSpd_Txtbox");           // 26
            nameList.Add("Nr_Of_Skills_Txtbox");                // 27
            nameList.Add("Skill_Cooldown_Txtbox");              // 28
            nameList.Add("Skill_Adaptive_Cooldown_Txtbox");     // 29
            nameList.Add("Skill_Dmg_txtbox");                   // 30
            nameList.Add("Movement_Speed_txtbox");              // 31
            nameList.Add("Max_Hp_Healing_Txtbox");              // 32
            nameList.Add("Flat_Healing_TxtBox");                // 33
            nameList.Add("Heal_Immunity_Box");                  // 34
            nameList.Add("Physical_Immunity_Box");              // 35
            nameList.Add("Water_Immunity_Box");                 // 36
            nameList.Add("Fire_Immunity_Box");                  // 37
            nameList.Add("Nature_Immunity_Box");                // 38
            nameList.Add("Slow_Immunity_Box");                  // 39
            nameList.Add("Stun_Immunity_Box");                  // 40
            nameList.Add("Magic_Immunity_Box");                 // 41
            nameList.Add("Knockback_Immunity_Box");             // 42
            nameList.Add("Disallow_Melee");                     // 43
            nameList.Add("Disallow_Range");                     // 44
            nameList.Add("Disallow_magic");                     // 45
            nameList.Add("Disallow_Water");                     // 46
            nameList.Add("Disallow_Fire");                      // 47
            nameList.Add("Disallow_Nature");                    // 48
        }
        
        private int IdentifyEnum(string name)
        {
            for (int i = 0; i < nameList.Count; i++)
            {
                if (name == nameList[i])
                {
                    return i;
                }
            }
        
            return -1;
        }

        void untickBoxes()
        {
            Physical_Taken.IsChecked = false;
            Water_Taken.IsChecked = false;
            Nature_Taken.IsChecked = false;
            Magic_Taken.IsChecked = false;
            Fire_Taken.IsChecked = false;
            Physical_Given.IsChecked = false;
            Water_Given.IsChecked = false;
            Nature_Given.IsChecked = false;
            Magic_Given.IsChecked = false;
            Fire_Given.IsChecked = false;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (LifeStealTxtBox.Text.ToString() != "")
            {
                
                string text = LifeStealTxtBox.Text.ToString();
                decimal test = 0;

                bool succesfull = Decimal.TryParse(text, out test);
                
                LifeStealSlider.Value = (double)test;
            }
        }

        private void LifeStealSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            LifeStealTxtBox.Text = LifeStealSlider.Value.ToString();
        }

        private void LifeStealTxtBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
            {
                if (e.Text != "-")
                {

                    e.Handled = true;
                }
            }
        }
        
        private void Physical_resistance_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            PhysicalResistanceTxtBox.Text = Physical_resistanceSlider.Value.ToString();
        }

        private void PhysicalResistanceTxtBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (PhysicalResistanceTxtBox.Text.ToString() != "")
            {

                string text = PhysicalResistanceTxtBox.Text.ToString();
                decimal test = 0;

                bool succesfull = Decimal.TryParse(text, out test);
                Physical_resistanceSlider.Value = (double)test;
            }
        }

        private void Water_resistanceSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            WaterResistanceTxtBox.Text = Water_resistanceSlider.Value.ToString();
        }

        private void WaterResistanceTxtBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (WaterResistanceTxtBox.Text.ToString() != "")
            {

                string text = WaterResistanceTxtBox.Text.ToString();
                decimal test = 0;

                bool succesfull = Decimal.TryParse(text, out test);
                Water_resistanceSlider.Value = (double)test;
            }
        }

        private void Fire_resistanceSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            FireResistanceTxtBox.Text = Fire_resistanceSlider.Value.ToString();
        }

        private void FireResistanceTxtBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (FireResistanceTxtBox.Text.ToString() != "")
            {

                string text = FireResistanceTxtBox.Text.ToString();
                decimal test = 0;

                bool succesfull = Decimal.TryParse(text, out test);
                Fire_resistanceSlider.Value = (double)test;
            }
        }

        private void NatureResistanceTxtBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (NatureResistanceTxtBox.Text.ToString() != "")
            {

                string text = NatureResistanceTxtBox.Text.ToString();
                decimal test = 0;

                bool succesfull = Decimal.TryParse(text, out test);
                Nature_resistanceSlider.Value = (double)test;
            }
        }

        private void Dmg_Txtbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Dmg_Txtbox.Text.ToString() != "")
            {

                string text = Dmg_Txtbox.Text.ToString();
                decimal test = 0;

                bool succesfull = Decimal.TryParse(text, out test);
                Dmg_Slider.Value = (double)test;
            }
        }

        private void Melee_Range_Txtbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Melee_Range_Txtbox.Text.ToString() != "")
            {

                string text = Melee_Range_Txtbox.Text.ToString();
                decimal test = 0;

                bool succesfull = Decimal.TryParse(text, out test);
                Melee_Range_Slider.Value = (double)test;
            }
        }

        private void Melee_Dmg_Txtbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Melee_Dmg_Txtbox.Text.ToString() != "")
            {

                string text = Melee_Dmg_Txtbox.Text.ToString();
                decimal test = 0;

                bool succesfull = Decimal.TryParse(text, out test);
                Melee_Dmg_Slider.Value = (double)test;
            }
        }

        private void Range_Dmg_Txtbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Range_Dmg_Txtbox.Text.ToString() != "")
            {

                string text = Range_Dmg_Txtbox.Text.ToString();
                decimal test = 0;

                bool succesfull = Decimal.TryParse(text, out test);
                Range_Dmg_Slider.Value = (double)test;
            }
        }

        private void AtkSpd_Txtbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (AtkSpd_Txtbox.Text.ToString() != "")
            {

                string text = AtkSpd_Txtbox.Text.ToString();
                decimal test = 0;

                bool succesfull = Decimal.TryParse(text, out test);
                AtkSpd_Slider.Value = (double)test;
            }
        }

        private void Melee_AtkSpd_Txtbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Melee_AtkSpd_Txtbox.Text.ToString() != "")
            {

                string text = Melee_AtkSpd_Txtbox.Text.ToString();
                decimal test = 0;

                bool succesfull = Decimal.TryParse(text, out test);
                Melee_AtkSpd_Slider.Value = (double)test;
            }
        }

        private void Cons_Dmg_Txtbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Cons_Dmg_Txtbox.Text.ToString() != "")
            {

                string text = Cons_Dmg_Txtbox.Text.ToString();
                decimal test = 0;

                bool succesfull = Decimal.TryParse(text, out test);
                Cons_Dmg_Slider.Value = (double)test;
            }
        }

        private void Cons_AtkSpd_Txtbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Cons_AtkSpd_Txtbox.Text.ToString() != "")
            {

                string text = Cons_AtkSpd_Txtbox.Text.ToString();
                decimal test = 0;

                bool succesfull = Decimal.TryParse(text, out test);
                Cons_AtkSpd_Slider.Value = (double)test;
            }
        }

        private void Cons_MeleeDmg_Txtbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Cons_MeleeDmg_Txtbox.Text.ToString() != "")
            {

                string text = Cons_MeleeDmg_Txtbox.Text.ToString();
                decimal test = 0;

                bool succesfull = Decimal.TryParse(text, out test);
                Cons_MeleeDmg_Slider.Value = (double)test;
            }
        }

        private void Cons_RangeDmg_Txtbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Cons_RangeDmg_Txtbox.Text.ToString() != "")
            {

                string text = Cons_RangeDmg_Txtbox.Text.ToString();
                decimal test = 0;

                bool succesfull = Decimal.TryParse(text, out test);
                Cons_RangeDmg_Slider.Value = (double)test;
            }
        }

        private void Cons_Melee_AtkSpd_Txtbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Cons_Melee_AtkSpd_Txtbox.Text.ToString() != "")
            {

                string text = Cons_Melee_AtkSpd_Txtbox.Text.ToString();
                decimal test = 0;

                bool succesfull = Decimal.TryParse(text, out test);
                Cons_MeleeAtkSpd_Slider.Value = (double)test;
            }
        }

        private void Cons_Range_AtkSpd_Txtbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Cons_Range_AtkSpd_Txtbox.Text.ToString() != "")
            {

                string text = Cons_Range_AtkSpd_Txtbox.Text.ToString();
                decimal test = 0;

                bool succesfull = Decimal.TryParse(text, out test);
                Cons_Range_AtkSpd_Slider.Value = (double)test;
            }
        }

        private void Nr_Of_Skills_Txtbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Nr_Of_Skills_Txtbox.Text.ToString() != "")
            {

                string text = Nr_Of_Skills_Txtbox.Text.ToString();
                decimal test = 0;

                bool succesfull = Decimal.TryParse(text, out test);
                Nr_Of_Skills_Slider.Value = (double)test;
            }
        }

        private void Skill_Cooldown_Txtbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Skill_Cooldown_Txtbox.Text.ToString() != "")
            {

                string text = Skill_Cooldown_Txtbox.Text.ToString();
                decimal test = 0;

                bool succesfull = Decimal.TryParse(text, out test);
                Skill_Cooldown_Slider.Value = (double)test;
            }
        }

        private void Skill_Dmg_txtbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Skill_Dmg_txtbox.Text.ToString() != "")
            {

                string text = Skill_Dmg_txtbox.Text.ToString();
                decimal test = 0;

                bool succesfull = Decimal.TryParse(text, out test);
                Skill_Dmg_Slider.Value = (double)test;
            }
        }

        private void Dmg_Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Dmg_Txtbox.Text = Dmg_Slider.Value.ToString();
        }

        private void Melee_Range_Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Melee_Range_Txtbox.Text = Melee_Range_Slider.Value.ToString();
        }

        private void Melee_Dmg_Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Melee_Dmg_Txtbox.Text = Melee_Dmg_Slider.Value.ToString();
        }

        private void Range_Dmg_Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Range_Dmg_Txtbox.Text = Range_Dmg_Slider.Value.ToString();
        }

        private void AtkSpd_Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            AtkSpd_Txtbox.Text = AtkSpd_Slider.Value.ToString();
        }

        private void Melee_AtkSpd_Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Melee_AtkSpd_Txtbox.Text = Melee_AtkSpd_Slider.Value.ToString();
        }

        private void Cons_Dmg_Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Cons_Dmg_Txtbox.Text = Cons_Dmg_Slider.Value.ToString();
        }

        private void Cons_AtkSpd_Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Cons_AtkSpd_Txtbox.Text = Cons_AtkSpd_Slider.Value.ToString();
        }

        private void Cons_MeleeDmg_Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Cons_MeleeDmg_Txtbox.Text = Cons_MeleeDmg_Slider.Value.ToString();
        }

        private void Cons_RangeDmg_Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Cons_RangeDmg_Txtbox.Text = Cons_RangeDmg_Slider.Value.ToString();
        }

        private void Cons_MeleeAtkSpd_Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Cons_Melee_AtkSpd_Txtbox.Text = Cons_MeleeAtkSpd_Slider.Value.ToString();
        }

        private void Cons_Range_AtkSpd_Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Cons_Range_AtkSpd_Txtbox.Text = Cons_Range_AtkSpd_Slider.Value.ToString();
        }

        private void Nr_Of_Skills_Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Nr_Of_Skills_Txtbox.Text = Nr_Of_Skills_Slider.Value.ToString();
        }

        private void Skill_Cooldown_Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Skill_Cooldown_Txtbox.Text = Skill_Cooldown_Slider.Value.ToString();
        }

        private void Skill_Dmg_Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Skill_Dmg_txtbox.Text = Skill_Dmg_Slider.Value.ToString();
        }

        private void Nature_resistanceSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            NatureResistanceTxtBox.Text = Nature_resistanceSlider.Value.ToString();
        }

        private void Physical_Taken_Checked(object sender, RoutedEventArgs e)
        {
            Physical_Taken.Checked -= Physical_Taken_Checked;
            untickBoxes();
            Physical_Taken.IsChecked = true;
            Physical_Taken.Checked += Physical_Taken_Checked;
            condition = 1;
        }

        private void Water_Taken_Checked(object sender, RoutedEventArgs e)
        {
            Water_Taken.Checked -= Water_Taken_Checked;
            untickBoxes();
            Water_Taken.IsChecked = true;
            Water_Taken.Checked += Water_Taken_Checked;
            condition = 2;
        }

        private void Nature_Taken_Checked(object sender, RoutedEventArgs e)
        {
            Nature_Taken.Checked -= Nature_Taken_Checked;
            untickBoxes();
            Nature_Taken.IsChecked = true;
            Nature_Taken.Checked += Nature_Taken_Checked;
            condition = 3;
        }

        private void Magic_Taken_Checked(object sender, RoutedEventArgs e)
        {
            Magic_Taken.Checked -= Magic_Taken_Checked;
            untickBoxes();
            Magic_Taken.IsChecked = true;
            Magic_Taken.Checked += Magic_Taken_Checked;
            condition = 4;
        }

        private void Fire_Taken_Checked(object sender, RoutedEventArgs e)
        {
            Fire_Taken.Checked -= Fire_Taken_Checked;
            untickBoxes();
            Fire_Taken.IsChecked = true;
            Fire_Taken.Checked += Fire_Taken_Checked;
            condition = 5;
        }

        private void Physical_Given_Checked(object sender, RoutedEventArgs e)
        {
            Physical_Given.Checked -= Physical_Given_Checked;
            untickBoxes();
            Physical_Given.IsChecked = true;
            Physical_Given.Checked += Physical_Given_Checked;
            condition = 6;
        }

        private void Water_Given_Checked(object sender, RoutedEventArgs e)
        {
            Water_Given.Checked -= Water_Given_Checked;
            untickBoxes();
            Water_Given.IsChecked = true;
            Water_Given.Checked += Water_Given_Checked;
            condition = 7;
        }

        private void Nature_Given_Checked(object sender, RoutedEventArgs e)
        {
            Nature_Given.Checked -= Nature_Given_Checked;
            untickBoxes();
            Nature_Given.IsChecked = true;
            Nature_Given.Checked += Nature_Given_Checked;
            condition = 8;
        }

        private void Magic_Given_Checked(object sender, RoutedEventArgs e)
        {
            Magic_Given.Checked -= Magic_Given_Checked;
            untickBoxes();
            Magic_Given.IsChecked = true;
            Magic_Given.Checked += Magic_Given_Checked;
            condition = 9;
        }

        private void Fire_Given_Checked(object sender, RoutedEventArgs e)
        {
            Fire_Given.Checked -= Fire_Given_Checked;
            untickBoxes();
            Fire_Given.IsChecked = true;
            Fire_Given.Checked += Fire_Given_Checked;
            condition = 10;
        }

        private void Movement_Speed_Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Movement_Speed_txtbox.Text = Movement_Speed_Slider.Value.ToString();
        }

        private void Movement_Speed_txtbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Movement_Speed_txtbox.Text.ToString() != "")
            {

                string text = Movement_Speed_txtbox.Text.ToString();
                decimal test = 0;

                bool succesfull = Decimal.TryParse(text, out test);
                Movement_Speed_Slider.Value = (double)test;
            }
        }

        private void Max_Hp_Healing_Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Max_Hp_Healing_Txtbox.Text = Max_Hp_Healing_Slider.Value.ToString();
        }

        private void Max_Hp_Healing_Txtbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Max_Hp_Healing_Txtbox.Text.ToString() != "")
            {

                string text = Max_Hp_Healing_Txtbox.Text.ToString();
                decimal test = 0;

                bool succesfull = Decimal.TryParse(text, out test);
                Max_Hp_Healing_Slider.Value = (double)test;
            }
        }

        private void Flat_Healing_TxtBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Flat_Healing_TxtBox.Text.ToString() != "")
            {

                string text = Flat_Healing_TxtBox.Text.ToString();
                decimal test = 0;

                bool succesfull = Decimal.TryParse(text, out test);
                Flat_Healing_Slider.Value = (double)test;
            }
        }

        private void Flat_Healing_Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Flat_Healing_TxtBox.Text = Flat_Healing_Slider.Value.ToString();
        }

        private void Physical_Taken_Unchecked(object sender, RoutedEventArgs e)
        {
            condition = 0;
        }

        private void Magic_Resistance_Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
             Magic_Resistance_Txtbox.Text = Magic_Resistance_Slider.Value.ToString();
        }

        private void Ranged_Resistance_Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Ranged_Resistance_Txtbox.Text = Ranged_Resistance_Slider.Value.ToString();
        }

        private void Magic_Resistance_Txtbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Magic_Resistance_Txtbox.Text.ToString() != "")
            {

                string text = Magic_Resistance_Txtbox.Text.ToString();
                decimal test = 0;

                bool succesfull = Decimal.TryParse(text, out test);
                Magic_Resistance_Slider.Value = (double)test;
            }
        }

        private void Ranged_Resistance_Txtbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Ranged_Resistance_Txtbox.Text.ToString() != "")
            {

                string text = Ranged_Resistance_Txtbox.Text.ToString();
                decimal test = 0;

                bool succesfull = Decimal.TryParse(text, out test);
                Ranged_Resistance_Slider.Value = (double)test;
            }
        }

        private void Skill_Adaptive_Cooldown_Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Skill_Adaptive_Cooldown_Txtbox.Text = Skill_Adaptive_Cooldown_Slider.Value.ToString();
        }

        private void Skill_Adaptive_Cooldown_Txtbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Skill_Adaptive_Cooldown_Txtbox.Text.ToString() != "")
            {

                string text = Skill_Adaptive_Cooldown_Txtbox.Text.ToString();
                decimal test = 0;

                bool succesfull = Decimal.TryParse(text, out test);
                Skill_Adaptive_Cooldown_Slider.Value = (double)test;
            }
        }
    }
}
