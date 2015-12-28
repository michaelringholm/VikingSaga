using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using System.Linq;
using VikingSaga.Code;
using VikingSagaWpfApp.Animations;
using VikingSagaWpfApp.Code.Battle;
using VikingSagaWpfApp.Code.Battle.Cards;
using System.Threading.Tasks;

namespace VikingSagaWpfApp.Controls
{
    /// <summary>
    /// Interaction logic for BattleBoardControl.xaml
    /// </summary>
    public partial class BattleBoardControl : UserControl, IBattleBoardUI
    {
        private readonly Color ColorHpInc = Colors.LightGreen;
        private readonly Color ColorHpDec = Colors.Red;
        private readonly Color ColorDmgInc = Colors.Azure;
        private readonly Color ColorDmgDec = Colors.Purple;
        private readonly Color ColorGainMana = Colors.LightBlue;
        private readonly Color ColorCardNotifications = Colors.Yellow;

        private CardPlaceholder[] _ply1Hand = new CardPlaceholder[5];
        private CardPlaceholder[] _ply2Hand = new CardPlaceholder[5];
        private CardPlaceholder[] _ply1Board = new CardPlaceholder[5];
        private CardPlaceholder[] _ply2Board = new CardPlaceholder[5];

        public BattleBoardControl()
        {
            InitializeComponent();

            BuildPlaceholderLut();
            //Background = WPFGUIUtil.GetImageBrush("\\backgrounds\\battle-form-background-1900x1200.jpg");
            borderInfo.Opacity = 0;
        }

        private void BuildPlaceholderLut()
        {
            _ply1Board[0] = Ply1Board1;
            _ply1Board[1] = Ply1Board2;
            _ply1Board[2] = Ply1Board3;
            _ply1Board[3] = Ply1Board4;
            _ply1Board[4] = Ply1Board5;

            _ply1Hand[0] = Ply1Hand1;
            _ply1Hand[1] = Ply1Hand2;
            _ply1Hand[2] = Ply1Hand3;
            _ply1Hand[3] = Ply1Hand4;
            _ply1Hand[4] = Ply1Hand5;

            _ply2Board[0] = Ply2Board1;
            _ply2Board[1] = Ply2Board2;
            _ply2Board[2] = Ply2Board3;
            _ply2Board[3] = Ply2Board4;
            _ply2Board[4] = Ply2Board5;

            _ply2Hand[0] = Ply2Hand1;
            _ply2Hand[1] = Ply2Hand2;
            _ply2Hand[2] = Ply2Hand3;
            _ply2Hand[3] = Ply2Hand4;
            _ply2Hand[4] = Ply2Hand5;
        }

        private void DumpPhInfo()
        {
            string P1H = string.Empty;
            string P1B = string.Empty;
            string P2H = string.Empty;
            string P2B = string.Empty;

            for (int i = 0; i < 5; ++i)
            {
                P1H += _ply1Hand[i].CardControl == null ? "-" : "X";
                P1B += _ply1Board[i].CardControl == null ? "-" : "X";
                P2H += _ply2Hand[i].CardControl == null ? "-" : "X";
                P2B += _ply2Board[i].CardControl == null ? "-" : "X";
            }

            Log.Line(P2H);
            Log.Line(P2B);
            Log.Line(P1B);
            Log.Line(P1H);
        }

        private CardPlaceholder GetHandPlaceholder(Player player, int position)
        {
            return IsPlayer1(player) ? _ply1Hand[position] : _ply2Hand[position];
        }

        private CardPlaceholder GetBoardPlaceholder(Player player, int position)
        {
            return IsPlayer1(player) ? _ply1Board[position] : _ply2Board[position];
        }

        private void btnViewProfile_Click(object sender, EventArgs e)
        {
            GameController.Current.ShowProfile();
        }

        private Battle GetBattle()
        {
            return GameEngine.Current.CurrentBattle;
        }

        private bool IsPlayer1(Player player)
        {
            return GetBattle().IsPlayer1(player);
        }

        IEnumerable<CardPlaceholder> AllPlaceholders()
        {
            return _ply1Board.Concat(_ply1Hand).Concat(_ply2Board).Concat(_ply2Hand);
        }

        IEnumerable<CardControl> AllCardControls()
        {
            return AllPlaceholders().Where(ph => ph.CardControl != null).Select(ph => ph.CardControl);
        }

        CardControl CardControlFromCard(BattleCard card)
        {
            return AllCardControls().Where(cc => cc.Card == card).FirstOrDefault();
        }

        private bool OverlapsHero(Rect rect, HeroCardControl hero, double minCoverage = 0.3)
        {
            Rect heroRect = new Rect(hero.Margin.Left, hero.Margin.Top, hero.ActualWidth, hero.ActualHeight);
            double phArea = heroRect.Width * heroRect.Height;
            if (!heroRect.IntersectsWith(rect))
                return false;

            heroRect.Intersect(rect);
            double intersectArea = heroRect.Width * heroRect.Height;
            if (intersectArea <= 0)
                return false;

            if (intersectArea / phArea > minCoverage)
                return true;

            return false;
        }

        public HeroCardControl GetOverlappedHero(Rect rect, double minCoverage = 0.3)
        {
            if (OverlapsHero(rect, yourHeroCardControl, minCoverage))
                return yourHeroCardControl;

            if (OverlapsHero(rect, enemyHeroCardControl, minCoverage))
                return enemyHeroCardControl;

            return null;
        }

        public CardPlaceholder GetOverlappedPlaceholder(Rect rect, double minCoverage = 0.2)
        {
            foreach (var ph in AllPlaceholders())
            {
                Rect phRect = new Rect(ph.Margin.Left, ph.Margin.Top, ph.ActualWidth, ph.ActualHeight);
                double phArea = phRect.Width * phRect.Height;
                if (!phRect.IntersectsWith(rect))
                    continue;

                phRect.Intersect(rect);
                double intersectArea = phRect.Width * phRect.Height;
                if (intersectArea <= 0)
                    continue;

                if (intersectArea / phArea > minCoverage)
                    return ph;
            }

            return null;
        }

        public void ShowNotifications()
        {
            Ui.Send(() => ShowNotificationsInternal());
        }

        public void ShowNotificationInternal(BattleCard card)
        {
            if (card == null)
                return;

            var notifications = card.UiOutput;
            int count = notifications.Count;
            for (int i = 0; i < count; ++i)
            {
                string text = notifications[i];
                var cardControl = CardControlFromCard(card);
                if (cardControl != null)
                {
                    var param = FloatingInfoParam.Create(cardControl, text, Colors.Yellow, FloatingInfoParam.CategoryType.SpellInfo, 2, 1500, false);
                    param.Offset = new Point(0, i * 10);
                    param.FontSize = 20;
                    ShowFloatingInfoMid(param);
                }
            }
            card.UiOutput.Clear();
        }

        public void ShowNotificationsInternal()
        {
            var battle = GetBattle();
            var all = battle.Board.RowPlayer1.Cards.Concat(battle.Board.RowPlayer2.Cards).Concat(battle.Player1.Hand.Cards).Concat(battle.Player2.Hand.Cards);
            foreach (var card in all)
            {
                ShowNotificationInternal(card);
            }

            foreach(var control in AllCardControls())
            {
                control.UpdateCardControl(control.Card);
            }
        }

        public void ShowFloatingInfoMid(Control control, string text, Color color, FloatingInfoParam.CategoryType category)
        {
            ShowFloatingInfoMid(FloatingInfoParam.Create(control, text, color, category));
        }

        public void ShowFloatingInfoMid(FloatingInfoParam param)
        {
            FloatingInfo fi = new FloatingInfo();
            MainGrid.Children.Add(fi);
            const int floatingInfoZIndex = 10000;
            Panel.SetZIndex(fi, floatingInfoZIndex);
            Point pos = GetControlMid(param.Control);
            fi.Show(pos.X, pos.Y, param);
        }

        AutoResetEvent _playRoundEvent = new AutoResetEvent(false);

        public void WaitForHumanEndTurn()
        {
            UpdateHandHighlighting();
            EnableUserInteraction(true);

            _playRoundEvent.WaitOne();

            EnableUserInteraction(false);
            UpdateHandHighlighting();
        }

        void UpdateHandHighlighting(bool disable = false)
        {
            foreach(var ph in _ply1Hand)
            {
                if (ph != null && ph.CardControl != null)
                {
                    bool canAffordCard = GetBattle().Player1.Mana >= ph.CardControl.Card.Mana;
                    bool isOnTheMove = ph.CardControl.IsAligning;

                    ph.CardControl.IsSelected = canAffordCard && !isOnTheMove && !disable;
                }
            }
        }

        public void SetStatusMessage(string msg, int ms = 1500)
        {
            SequentialActions.RunAsync(InternalStatusMessage(msg, ms));
        }

        private IEnumerable<int> InternalStatusMessage(string msg, int ms = 1500)
        {
            tbInfo.Text = msg;
            AnimHelper.ApplyFadeAnimation(borderInfo, 0, 1, 200);
            yield return ms;
            AnimHelper.ApplyFadeAnimation(borderInfo, 1, 0, 300);
        }

        private Point GetDealtCardStartPosition(Player player, CardControl cardControl)
        {
            var result = new Point();
            result.X = this.Width / 2 - cardControl.Width / 2;
            result.Y = IsPlayer1(player) ? this.Height + cardControl.Height : 0 - cardControl.Height * 2;
            return result;
        }

        private CardControl CreateAndPlaceCardControl(BattleCard card, CardPlaceholder placeHolder, int position, bool isDraggable)
        {
            var cardControl = new CardControl();
            cardControl.InitCardControl(card);
            cardControl.UpdateCardControl(card);

            cardControl.Position = position;
            cardControl.Visibility = Visibility.Visible;
            cardControl.OwningPlaceholder = placeHolder;
            cardControl.Width = placeHolder.ActualWidth;
            cardControl.Height = placeHolder.ActualHeight;

            cardControl.InitDragHelper();
            cardControl.IsDraggable = isDraggable;

            Grid.SetRowSpan(cardControl, 999);
            Grid.SetColumnSpan(cardControl, 999);

            MainGrid.Children.Add(cardControl);

            var p = GetDealtCardStartPosition(card.Owner, cardControl);
            cardControl.Margin = new Thickness(p.X, p.Y, 0, 0);
            placeHolder.MoveCardControl(cardControl);

            return cardControl;
        }

        public IEnumerable<int> AddCardToHand(Player player, BattleCard card, int position)
        {
            SoundUtil.PlaySound(@"sounds/draw-card-2.wav");

            var placeholder = GetHandPlaceholder(player, position);
            var cardControl = CreateAndPlaceCardControl(card, placeholder, position, false);

            UpdateHandHighlighting();

            const int alignMs = 300;
            cardControl.AlignToUserControl(placeholder, alignMs);

            yield return alignMs;
        }

        public IEnumerable<int> ShowCardPlacedOnEmptyPlaceHolder(Player player, int handPosition, int boardPosition, bool isAi)
        {
            SoundUtil.PlaySound(@"sounds/draw-card-2.wav");

            UpdateHandHighlighting();

            var cardControl = GetHandPlaceholder(player, handPosition).CardControl;
            var boardPlaceholder = GetBoardPlaceholder(player, boardPosition);

            boardPlaceholder.MoveCardControl(cardControl);

            const int alignMs = 300;

            // If placed by a human the card is already dragged and dropped at the correct position.
            // For AI it must be moved from hand to board.
            if (isAi)
            {
                cardControl.AlignToUserControl(boardPlaceholder, alignMs);
            }

            yield return alignMs;
        }

        public IEnumerable<int> DropCardOnPlayer(Player dst, BattleCard card, object data, bool isAi)
        {
            UpdateHandHighlighting();

            var hero = IsPlayer1(dst) ? yourHeroCardControl : enemyHeroCardControl;
            var cardControl = CardControlFromCard(card);

            if (card is CardInstantHpChange)
            {
                int amount = (int)data;
                ShowPlayerHpChange(dst, amount);

                MoveCardToTargetAndDie(cardControl, hero, isAi);

                if (amount < 0)
                {
                    AnimHelper.ApplyHeroDamageAnimation(hero);

                    foreach (var action in hero.HeroAttackedEffectActions())
                        yield return action;
                }
            }
            else if (card is CardInstantManaChange)
            {
                int amount = (int)data;
                ShowPlayerManaChange(dst, amount);
                MoveCardToTargetAndDie(cardControl, hero, isAi);
            }
            else if (card is CardInstantCustom)
            {
                MoveCardToTargetAndDie(cardControl, hero, isAi);
            }
            else throw new InvalidOperationException("Unsupported card type dropped on player");

            yield return 300;
        }

        private void MoveCardToTargetAndDie(CardControl cardControl, UserControl target, bool isAi)
        {
            cardControl.BringToFront();
            int moveMs = 300 * (isAi ? 2 : 1);
            cardControl.AlignToUserControl(target, moveMs);

            Task.Delay(isAi ? moveMs : 0).ContinueWith((t) =>
            {
                Ui.Send(() => { cardControl.FadeKill(); });
            });
        }

        public IEnumerable<int> DropCardOnCard(BattleCard src, BattleCard dst, object data, bool isAi)
        {
            UpdateHandHighlighting();

            if (src is CardInstantHpChange && dst is CardBasicMob)
            {
                int amount = (int)data;
                var dstControl = CardControlFromCard(dst);
                ShowCardHpChange((CardBasicMob)dstControl.Card, amount);

                var srcControl = CardControlFromCard(src);
                MoveCardToTargetAndDie(srcControl, dstControl, isAi);

                if (amount < 0)
                {
                    AnimHelper.ApplyCardDamageAnimation(dstControl);

                    foreach (var action in dstControl.CardAttackedEffectActions())
                        yield return action;

                }

                dstControl.UpdateCardControl(dst);
            }
            else if (src is CardInstantSpellProperty && dst is CardBasicMob)
            {
                var instant = (CardInstantSpellProperty)src;
                var mob = (CardBasicMob)dst;
                mob.AddUiSpellOutput(instant.OnApplyMsg);

                var srcControl = CardControlFromCard(src);
                var dstControl = CardControlFromCard(dst);
                MoveCardToTargetAndDie(srcControl, dstControl, isAi);
                dstControl.UpdateCardControl(dst);
            }
            else if (src is CardInstantDmgChange)
            {
                var instant = (CardInstantDmgChange)src;
                var mob = (CardBasicMob)dst;
                mob.AddUiSpellOutput(instant.OnApplyMsg);

                var dstControl = CardControlFromCard(dst);
                var srcControl = CardControlFromCard(src);
                MoveCardToTargetAndDie(srcControl, dstControl, isAi);
                dstControl.UpdateCardControl(dst);
            }
            else if (src is CardInstantCustom)
            {
                var dstControl = CardControlFromCard(dst);
                var srcControl = CardControlFromCard(src);
                MoveCardToTargetAndDie(srcControl, dstControl, isAi);
                ShowNotifications();
            }
            else throw new InvalidOperationException("Unsupported card on card types");

            ShowNotifications();
        }

        public IEnumerable<int> CardVsCard(CardBasicMob src, CardBasicMob dst, int amount)
        {
            var srcControl = CardControlFromCard(src);
            var dstControl = CardControlFromCard(dst);

            srcControl.BringToFront();

            int ms = 400;
            AnimHelper.ApplyCardVsCardAnimation(srcControl, dstControl, ms);

            yield return ms;

            ShowNotifications();
            ShowCardHpChange((CardBasicMob)dstControl.Card, amount);

            AnimHelper.ApplyCardDamageAnimation(dstControl);

            foreach (var action in dstControl.CardAttackedEffectActions())
                yield return action;

            dstControl.UpdateCardControl(dst);
        }

        public Point GetPosition(Control control)
        {
            Point position = control.TransformToAncestor(MainGrid).Transform(new Point(0, 0));
            return position;
        }

        private Point GetControlMid(Control control)
        {
            var point = GetPosition(control);
            point.X += control.ActualWidth * 0.5;
            point.Y += control.ActualHeight * 0.5;
            return point;
        }

        public IEnumerable<int> RemoveDeadCardsOnBoard()
        {
            var battle = GetBattle();
            var allBoardPhs = _ply1Board.Concat(_ply2Board);
            bool deadCardsFound = false;

            const int ms = 1000;
            foreach (var ph in allBoardPhs)
            {
                if (ph.CardControl != null && ((CardBasicMob)ph.CardControl.Card).IsDead)
                {
                    deadCardsFound = true;
                    Action postAction = () =>
                    {
                        if (ph.CardControl != null)
                        {
                            ph.CardControl.Kill();
                            ph.ClearCardControl();
                        }
                    };

                    AnimHelper.ApplyKillCardAnimation(ph.CardControl, ms, postAction);
                }
            }

            if (deadCardsFound)
                yield return ms / 2;
        }

        public IEnumerable<int> CardVsPlayer(Player src, Player dst, int position, int amount)
        {
            var srcControl = GetBoardPlaceholder(src, position).CardControl;

            var isPlayer1 = IsPlayer1(src);
            srcControl.BringToFront();

            const int ms = 400;
            var heroControl = IsPlayer1(dst) ? yourHeroCardControl : enemyHeroCardControl;
            AnimHelper.ApplyCardVsCardAnimation(srcControl, heroControl, ms);

            yield return ms;

            var hero = isPlayer1 ? enemyHeroCardControl : yourHeroCardControl;
            AnimHelper.ApplyHeroDamageAnimation(hero);

            foreach (var action in hero.HeroAttackedEffectActions())
                yield return action;

            ShowPlayerHpChange(dst, amount);
            UpdateHeroes();
        }

        private string AmountString(int amount)
        {
            if (amount <= 0)
                return amount.ToString();
            else
                return "+" + amount.ToString();
        }

        public void ShowCardDmgChange(CardBasicMob card, int amount)
        {
            Ui.Send(() =>
            {
                var control = CardControlFromCard(card);
                //ShowFloatingInfoMid(control, AmountString(amount), amount > 0 ? ColorDmgInc : ColorDmgDec, FloatingInfoParam.CategoryType.DmgChange);
                control.UpdateCardControl(card);
            });
        }

        public void ShowCardHpChange(CardBasicMob card, int amount)
        {
            Ui.Send(() =>
                {
                    var control = CardControlFromCard(card);
                    ShowFloatingInfoMid(control, AmountString(amount), amount > 0 ? ColorHpInc : ColorHpDec, FloatingInfoParam.CategoryType.HpChange);
                    control.UpdateCardControl(card);
                });
        }

        public void ShowPlayerHpChange(Player player, int amount)
        {
            Ui.Send(() =>
            {
                var control = IsPlayer1(player) ? yourHeroCardControl : enemyHeroCardControl;
                ShowFloatingInfoMid(control, AmountString(amount), amount > 0 ? ColorHpInc : ColorHpDec, FloatingInfoParam.CategoryType.HpChange);
                UpdateHeroes();
            });
        }

        public void ShowPlayerManaChange(Player player, int amount)
        {
            Ui.Send(() =>
            {
                var control = IsPlayer1(player) ? yourHeroCardControl : enemyHeroCardControl;
                ShowFloatingInfoMid(control, AmountString(amount), ColorGainMana, FloatingInfoParam.CategoryType.ManaChange);
                UpdateHeroes();
            });
        }

        public IEnumerable<int> HandCardVsPlayer(CardInstant card, Player targetPlayer, int amount)
        {
            // TODO : Differentiate between positive and negative effects
            var hero = IsPlayer1(targetPlayer) ? enemyHeroCardControl : yourHeroCardControl;
            AnimHelper.ApplyHeroDamageAnimation(hero);
            yield break;
        }

        void UpdateHeroes()
        {
            var battle = GetBattle();
            UpdateYourHeroControl(battle.Player1, GameController.Current.Profile.SelectedHero);
            if (GameEngine.Current.CurrentEncounter != null)
            {
                UpdateEnemyHeroControl(battle.Player2, GameEngine.Current.CurrentEncounter.Hero);
            }
        }

        public void UpdateEnemyHeroControl(Player player, Hero hero)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                enemyHeroCardControl.Player = player;
                enemyHeroCardControl.IsPlayer1 = false;
                enemyHeroCardControl.Mana.Text = Convert.ToString(player.Mana);
                enemyHeroCardControl.HP.Text = Convert.ToString(player.Hp);
                enemyHeroCardControl.HeroName.Text = Convert.ToString(player.Name);
                enemyHeroCardControl.Level.Text = hero.Level.ToString();
                enemyHeroCardControl.BackgroundCardBrush.ImageSource = WPFGUIUtil.GetImage(hero.CardImageURL).Source;
            }));
        }

        public void UpdateYourHeroControl(Player player, Hero hero)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                yourHeroCardControl.Player = player;
                yourHeroCardControl.IsPlayer1 = true;
                yourHeroCardControl.Mana.Text = Convert.ToString(player.Mana);
                yourHeroCardControl.HP.Text = Convert.ToString(player.Hp);
                yourHeroCardControl.HeroName.Text = Convert.ToString(player.Name);
                yourHeroCardControl.Level.Text = hero.Level.ToString();
                yourHeroCardControl.BackgroundCardBrush.ImageSource = WPFGUIUtil.GetImage(hero.CardImageURL).Source;
            }));
        }

        public void CloseBattleBoard()
        {
            Dispatcher.Invoke(new Action(() =>
            {
                this.Visibility = Visibility.Visible;
            }));
        }

        private void ResetAll()
        {
            foreach (var cardControl in AllCardControls())
                cardControl.Kill();
            ResetPlaceholders();
        }

        public void ShowBattleBoard()
        {
            ResetAll();
            this.Visibility = Visibility.Visible;
            EnableUserInteraction(false);
        }

        private void ResetPlaceholders()
        {
            foreach (var ph in AllPlaceholders())
            {
                ph.ClearCardControl();
            }
        }

        private void EnableUserInteraction(bool enable)
        {
            Dispatcher.Invoke(EnableUserInteractionAction(enable));
        }

        // Enable/disable controls used by the user when taking his turn
        private Action EnableUserInteractionAction(bool enable)
        {
            Action action = () =>
                {
                    btnPlayRound.IsEnabled = enable;
                    btnSurrender.IsEnabled = enable;

                    // Disable dragging hand cards when not users turn
                    for (int i = 0; i < 5; ++i)
                    {
                        if (_ply1Hand[i].CardControl != null)
                            _ply1Hand[i].CardControl.IsDraggable = enable;
                    }

                    foreach (var cardControl in AllCardControls())
                        cardControl.IsZoomable = enable;
                };

            return action;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ResetAll();

            if (GameEngine.Current.BattleInProgress)
                GameEngine.Current.Surrender();
        }


        public ImageSource GetMainWindowBackgroundImage()
        {
            return WPFGUIUtil.GetImage(@"\backgrounds\battle-form-background-1900x1200.jpg").Source;
        }

        private void btnPlayRound_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _playRoundEvent.Set();
        }

        private void btnSurrender_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            GameController.Current.PrepareSurrender();
        }

    }
}
