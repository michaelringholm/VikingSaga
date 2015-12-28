using GameLib.Battles.Cards;
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
using Vik.Code.Game.Main.Profiles;

namespace Vik.Code.Controls.Player
{
    public partial class PlayerEditDeckControl : UserControl
    {
        private List<CardFilterControl> cardFilters = new List<CardFilterControl>();

        public PlayerEditDeckControl()
        {
            InitializeComponent();

            if (!DesignerProperties.GetIsInDesignMode(this))
                this.Background = new SolidColorBrush(Colors.Transparent);

            Loaded += PlayerEditDeckControl_Loaded;
        }

        void PlayerEditDeckControl_Loaded(object sender, RoutedEventArgs e)
        {
            InitToggleButtons();
        }

        public void SetCardsFromProfile(ProfileDTO profile)
        {
            remainingCardsScrollList.SetCardsIds(profile.RemainingCards, refresh: true);
            deckCardsScrollList.SetCardsIds(profile.DeckCards, refresh: true);
        }

        private void UpdateScrollControlDisplayFlags(bool refresh)
        {
            var flags = CardFilter.BuildFlags(cardFilters);
            remainingCardsScrollList.SetDisplayFlags(flags, refresh);
            deckCardsScrollList.SetDisplayFlags(flags, refresh);
        }

        public void ApplyCardsToProfile(ProfileDTO profile)
        {
            profile.RemainingCards = new List<string>(this.remainingCardsScrollList.GetCardIds());
            profile.DeckCards = new List<string>(this.deckCardsScrollList.GetCardIds());
        }

        private void InitToggleButtons()
        {
            foreach (UIElement child in FiltersPanel.Children)
                if (child is CardFilterControl)
                    cardFilters.Add((CardFilterControl)child);

            UpdateScrollControlDisplayFlags(refresh: true);
        }

        private void CardFilterControl_FilterToggled(object sender, EventArgs e)
        {
            UpdateScrollControlDisplayFlags(refresh: true);
        }      
    }
}
