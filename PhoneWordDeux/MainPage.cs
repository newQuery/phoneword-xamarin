using System;
using Xamarin.Essentials;
using Xamarin.Forms;
public class MainPage : ContentPage
{
    private Entry phoneNumberText;
    private Button translateButton;
    private Button callButton;
    private string translatedNumber;

    public MainPage()
    {
        this.Padding = new Thickness(20, 80, 20, 20);

        StackLayout panel = new StackLayout
        {
            Spacing = 15
        };

        panel.Children.Add(new Label
        {
            Text = "Enter a Phoneword:",
            FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label))
        });

        panel.Children.Add(phoneNumberText = new Entry
        {
            Text = "1-855-XAMARIN",
        });

        panel.Children.Add(translateButton = new Button
        {
            Text = "Translate"
        });

        panel.Children.Add(callButton = new Button
        {
            Text = "Call",
            IsEnabled = false,
        });

        translateButton.Clicked += onTranslate;
        callButton.Clicked += onCall;

        this.Content = panel;
    }

    async void onCall(object sender, System.EventArgs e)
    {
        if (await this.DisplayAlert(
            "Dial a Number",
            "Would you like to call " + translatedNumber + "?",
            "Yes",
            "No"))
        {
            try
            {
                PhoneDialer.Open(translatedNumber);
            }
            catch (ArgumentNullException)
            {
                await DisplayAlert("Unable to dial", "Phone number was not valid.", "OK");
            }
            catch (FeatureNotSupportedException)
            {
                await DisplayAlert("Unable to dial", "Phone dialing not supported.", "OK");
            }
            catch (Exception)
            {
                // Other error has occurred.
                await DisplayAlert("Unable to dial", "Phone dialing failed.", "OK");
            }
        }
    }

    private void onTranslate(object sender, EventArgs e)
    {
        string input = phoneNumberText.Text;

        translatedNumber = Core.PhonewordTranslator.ToNumber(input);

        if (!string.IsNullOrEmpty(translatedNumber))
        {
            callButton.IsEnabled = true;
            callButton.Text = "Call " + translatedNumber;
        }
        else
        {
            callButton.IsEnabled = false;
            callButton.Text = "Call";
        }

    }
}