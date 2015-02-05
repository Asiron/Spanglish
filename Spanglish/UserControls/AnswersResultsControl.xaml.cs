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

namespace Spanglish.UserControls
{
    /// <summary>
    /// Interaction logic for AnswersResultsControl.xaml
    /// </summary>
    public partial class AnswersResultsControl : UserControl
    {
        public String CorrectAnswers
        {
            get { return (String)GetValue(CorrectAnswersProperty); }
            set { SetValue(CorrectAnswersProperty, value); }
        }
        public static readonly DependencyProperty CorrectAnswersProperty =
            DependencyProperty.Register("CorrectAnswers", typeof(string),
            typeof(AnswersResultsControl), new PropertyMetadata(""));

        public String WrongAnswers
        {
            get { return (String)GetValue(WrongAnswersProperty); }
            set { SetValue(WrongAnswersProperty, value); }
        }

        public static readonly DependencyProperty WrongAnswersProperty =
            DependencyProperty.Register("WrongAnswers", typeof(string),
            typeof(AnswersResultsControl), new PropertyMetadata(""));

        public String SkippedAnswers
        {
            get { return (String)GetValue(SkippedAnswersProperty); }
            set { SetValue(SkippedAnswersProperty, value); }
        }

        public static readonly DependencyProperty SkippedAnswersProperty =
            DependencyProperty.Register("SkippedAnswers", typeof(string),
            typeof(AnswersResultsControl), new PropertyMetadata(""));

        public String TotalAnswers
        {
            get { return (String)GetValue(TotalAnswersProperty); }
            set { SetValue(TotalAnswersProperty, value); }
        }

        public static readonly DependencyProperty TotalAnswersProperty =
            DependencyProperty.Register("TotalAnswers", typeof(string),
            typeof(AnswersResultsControl), new PropertyMetadata(""));

        public String LeftAnswers
        {
            get { return (String)GetValue(LeftAnswersProperty); }
            set { SetValue(LeftAnswersProperty, value); }
        }

        public static readonly DependencyProperty LeftAnswersProperty =
            DependencyProperty.Register("LeftAnswers", typeof(string),
            typeof(AnswersResultsControl), new PropertyMetadata(""));

        public String TimeElapsed
        {
            get { return (String)GetValue(TimeElapsedProperty); }
            set { SetValue(TimeElapsedProperty, value); }
        }

        public static readonly DependencyProperty TimeElapsedProperty =
            DependencyProperty.Register("TimeElapsed", typeof(string),
            typeof(AnswersResultsControl), new PropertyMetadata(""));

        public AnswersResultsControl()
        {
            InitializeComponent();
        }
    }
}
