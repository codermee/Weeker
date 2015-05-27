using System;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using ResourceLib;

namespace Weeker.Views
{
    public partial class AboutView
    {
        public AboutView()
        {
            InitializeComponent();
            SetupApplicationBar();
        }

        private void SetupApplicationBar()
        {
            var reviewIcon = (ApplicationBarIconButton)ApplicationBar.Buttons[0];
            reviewIcon.Text = Resource.Review;

            var mailIcon = (ApplicationBarIconButton)ApplicationBar.Buttons[1];
            mailIcon.Text = Resource.Mail;
        }

        private void OnApplicationBarEmailIconButtonClick(object sender, EventArgs e)
        {
            var emailComposeTask = new EmailComposeTask
                    {
                        Subject = Resource.MailSubject,
                        To = Resource.MailTo
                    };

            emailComposeTask.Show();
        }

        private void OnApplicationBarReviewIconButtonClick(object sender, EventArgs e)
        {
            var marketplaceReviewTask = new MarketplaceReviewTask();
            marketplaceReviewTask.Show();
        }
    }
}