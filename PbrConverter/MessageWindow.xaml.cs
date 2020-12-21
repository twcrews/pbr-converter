using System.Windows;

namespace Crews.Utility.PbrConverter
{
    /// <summary>
    /// Interaction logic for MessageWindow.xaml
    /// </summary>
    public partial class MessageWindow : Window
    {
        /// <summary>
        /// The message caption.
        /// </summary>
        public string Caption
        {
            get => CaptionTextBlock.Text;
            set => CaptionTextBlock.Text = value;
        }

        /// <summary>
        /// The message text.
        /// </summary>
        public string Message
        {
            get => MessageTextBlock.Text;
            set => MessageTextBlock.Text = value;
        }

        public MessageWindow() => InitializeComponent();
        private void DismissWindow(object sender, RoutedEventArgs e) => Close();
    }
}
