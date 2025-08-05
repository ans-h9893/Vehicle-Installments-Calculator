using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace A2AnshChhabra
{
    public partial class ApplicationWindow : Window
    {
        private List<SavedRate> savedRates = new List<SavedRate>();
        public ApplicationWindow()
        {
            InitializeComponent();
        }
        
        private void Calculate_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateForm()) return;

            if (!double.TryParse(vehiclePriceBox.Text, out double price) || price <= 0)
            {
                MessageBox.Show("Invalid vehicle price.");
                vehiclePriceBox.Focus();
                return;
            }

            if (!double.TryParse(downPaymentBox.Text, out double downPayment) || downPayment < 0 || downPayment > price)
            {
                MessageBox.Show("Invalid down payment.");
                downPaymentBox.Focus();
                return;
            }

            double loanAmount = price - downPayment;

            double interestRate = GetInterestRate();
            if (interestRate < 0)
            {
                MessageBox.Show("Invalid interest rate.");
                return;
            }

            double monthlyRate = interestRate / 100 / 12;
            int durationMonths = (int)tenureSlider.Value;
            if (durationMonths <= 0)
            {
                MessageBox.Show("Loan duration must be greater than 0.");
                return;
            }

            double payment;

            if (monthlyRate == 0)
            {
                // Zero interest: simple division
                payment = loanAmount / durationMonths;
            }
            else
            {
                // Fixed-rate monthly payment formula
                payment = (loanAmount * monthlyRate) /
                          (1 - Math.Pow(1 + monthlyRate, -durationMonths));
            }

            estPaymentBox.Text = payment.ToString("C", CultureInfo.CurrentCulture);
        }


        private double GetInterestRate()
        {
            if (!string.IsNullOrWhiteSpace(customInterestBox.Text) &&
                double.TryParse(customInterestBox.Text, out double customRate))
                return customRate;

            if (interestBox.SelectedItem is ComboBoxItem selectedItem &&
                double.TryParse(selectedItem.Content.ToString(), out double selectedRate))
                return selectedRate;

            return 0;
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            custNameBox.Clear();
            custPhoneBox.Clear();
            custCityBox.Clear();
            custProvinceBox.SelectedIndex = -1;
            carRadio.IsChecked = truckRadio.IsChecked = familyVanRadio.IsChecked = false;
            newRadio.IsChecked = usedRadio.IsChecked = false;
            vehiclePriceBox.Clear();
            downPaymentBox.Clear();
            interestBox.SelectedIndex = -1;
            customInterestBox.Clear();
            tenureSlider.Value = 12;
            weeklyRadio.IsChecked = biWeeklyRadio.IsChecked = monthlyRadio.IsChecked = false;
            estPaymentBox.Text = "";
            SavedRatesList.SelectedItem = null;
        }

        private void SaveRates_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateForm()) return;

            var rate = new SavedRate
            {
                Name = custNameBox.Text,
                VehicleType = GetVehicleType(),
                InterestRate = GetInterestRate().ToString("F2") + "%"
            };
            savedRates.Add(rate);
            MessageBox.Show("Rate saved.");
        }

        private void ShowRates_Click(object sender, RoutedEventArgs e)
        {
            SavedRatesList.ItemsSource = null;
            SavedRatesList.ItemsSource = savedRates;
        }

        private void SavedRatesList_DoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (SavedRatesList.SelectedItem is SavedRate selected)
            {
                custNameBox.Text = selected.Name;
                MessageBox.Show($"Loading {selected.Name}'s rate: {selected.InterestRate}");
            }
        }

        private string GetVehicleType()
        {
            if (carRadio.IsChecked == true) return "Car";
            if (truckRadio.IsChecked == true) return "Truck";
            if (familyVanRadio.IsChecked == true) return "Family Van";
            return "";
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(custNameBox.Text)) { custNameBox.Focus(); return false; }
            if (string.IsNullOrWhiteSpace(custPhoneBox.Text)) { custPhoneBox.Focus(); return false; }
            if (string.IsNullOrWhiteSpace(custCityBox.Text)) { custCityBox.Focus(); return false; }
            if (custProvinceBox.SelectedIndex == -1) { custProvinceBox.Focus(); return false; }
            if (!(carRadio.IsChecked == true || truckRadio.IsChecked == true || familyVanRadio.IsChecked == true))
            { MessageBox.Show("Select a vehicle type"); return false; }
            if (!(newRadio.IsChecked == true || usedRadio.IsChecked == true))
            { MessageBox.Show("Select vehicle age"); return false; }
            if (!double.TryParse(vehiclePriceBox.Text, out _)) { vehiclePriceBox.Focus(); return false; }
            if (!double.TryParse(downPaymentBox.Text, out _)) { downPaymentBox.Focus(); return false; }
            return true;
        }
    }

    public class SavedRate
    {
        public string Name { get; set; }
        public string VehicleType { get; set; }
        public string InterestRate { get; set; }
    }
}
