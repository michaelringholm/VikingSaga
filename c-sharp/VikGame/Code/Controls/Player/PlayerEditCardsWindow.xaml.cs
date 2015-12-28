using GameLib.DataStore.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using Vik.Code.Utility;

namespace Vik.Code.Controls.Player
{
    public partial class PlayerEditCardsWindow : FakeWindow
    {
        private Vik.Code.Controls.Base.TabControl _selectedTab;
        private static bool StartShowingParty = true;

        public PlayerEditCardsWindow()
        {
            InitializeComponent();

            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                var brush = new ImageBrush();
                brush.ImageSource = VikGame.ResourceLocator.GetImageResource("Data/Gfx/Backgrounds/edit-cards.jpg");
                OuterBorder.Background = brush;
            }

            Loaded += delegate { SelectTab(StartShowingParty ? PartyTab : DeckTab); };
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            ApplyTabCardsToProfile(_selectedTab);

            Close(Result.OK);
        }

        private void ApplyTabCardsToProfile(Vik.Code.Controls.Base.TabControl tab)
        {
            if (tab == null)
                return;

            if (tab == DeckTab)
            {
                EditDeckControl.ApplyCardsToProfile(VikGame.World.PlayerProfile.Data);
            }
            else
            {
                EditPartyControl.ApplyCardsToProfile(VikGame.World.PlayerProfile.Data);
            }
        }

        private void SelectTab(Vik.Code.Controls.Base.TabControl tab)
        {
            foreach (var tabControl in TabPanel.Children)
                if (tabControl is Vik.Code.Controls.Base.TabControl && tabControl != tab)
                    ((Vik.Code.Controls.Base.TabControl)tabControl).Selected = false;

            StartShowingParty = tab == PartyTab;

            ApplyTabCardsToProfile(_selectedTab);

            if (tab == DeckTab)
            {
                EditDeckControl.SetCardsFromProfile(VikGame.World.PlayerProfile.Data);
            }
            else
            {
                EditPartyControl.SetCardsFromProfile(VikGame.World.PlayerProfile.Data);
            }

            _selectedTab = tab;

            EditDeckControl.Visibility = _selectedTab == DeckTab ? Visibility.Visible : Visibility.Hidden;
            EditPartyControl.Visibility = _selectedTab == PartyTab ? Visibility.Visible : Visibility.Hidden;
        }

        private void TabSelected(object sender, EventArgs e)
        {
            SelectTab((Vik.Code.Controls.Base.TabControl)sender);
        }
    }
}
