using GameLib.Battles;
using GameLib.Battles.Cards;
using GameLib.Encounters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using Vik.Code.Controls.Cards;
using Vik.Code.Controls.Utility;
using Vik.Code.Utility;

namespace Vik.Code.Controls.Battle
{
    public partial class BattleWindow : FakeWindow, IBattleObserver
    {
        private enum BattleState { None, EnemyTurn, SelectPartyCard, SelectAbility, SelectAbilityTarget };

        private BattleState _battleState;
        private Encounter _encounter;
        private GameLib.Battles.Battle _battle;

        private readonly Color ColorHpInc = Colors.LightGreen;
        private readonly Color ColorHpDec = Colors.Red;
        private readonly Color ColorDmgInc = Colors.Azure;
        private readonly Color ColorDmgDec = Colors.Purple;
        private readonly Color ColorGainMana = Colors.LightBlue;
        private readonly Color ColorCardNotifications = Colors.Yellow;

        private CardPlaceholderControl[] _ply1Hand = new CardPlaceholderControl[5];
        private CardPlaceholderControl[] _ply2Hand = new CardPlaceholderControl[5];
        private CardPlaceholderControl[] _ply1Board = new CardPlaceholderControl[5];
        private CardPlaceholderControl[] _ply2Board = new CardPlaceholderControl[5];

        public BattleWindow(Encounter encounter)
        {
            InitializeComponent();

            tbYourTurn.Visibility = System.Windows.Visibility.Hidden;
            tbEnemyTurn.Visibility = System.Windows.Visibility.Hidden;

            RemoveAllAdornersOnClose = true;
            UiUtil.RemoveAllGlobalControlAdorners();

            _encounter = encounter;

            BuildPlaceholderLut();
        }

        public void SetBattle(GameLib.Battles.Battle battle)
        {
            _battle = battle;
            Loaded += delegate { _battle.StartAsync(); };
        }

        private void StateSelectPartyCard()
        {
            RemoveAllHighlighting();
            var humanPlayer = _battle.Player1;

            for (int i = 0; i < 5; ++i)
            {
                if (humanPlayer.TurnTakenFlags[i])
                    continue;

                var ph = GetBoardPlaceholder(humanPlayer, i);
                var card = ph.GetInnerCard();
                if (card == null || card.NeedRevive)
                    continue;

                ph.HighlightAccept(true);
            }
        }

        private void SetYourTurnText(BattleState state)
        {
            bool enemyTurn = state == BattleState.EnemyTurn;
            tbEnemyTurn.Visibility = enemyTurn ? Visibility.Visible : Visibility.Hidden;

            if (!enemyTurn)
            {
                string turnInfo = string.Empty;
                switch (state)
                {
                    case BattleState.SelectPartyCard: turnInfo = "select a party member"; break;
                    case BattleState.SelectAbility: turnInfo = "select an ability"; break;
                    case BattleState.SelectAbilityTarget: turnInfo = "select a target for the ability"; break;
                }

                tbYourTurn.Text = "Your turn, " + turnInfo;
            }
            tbYourTurn.Visibility = enemyTurn ? Visibility.Hidden : Visibility.Visible;

            AnimHelper.ApplyScaleAnimation(enemyTurn ? tbEnemyTurn : tbYourTurn, 0.1, 1.0, 1000, SimpleEase.CubicOut);
        }

        private void StateEnemyTurn()
        {
        }

        private void SetBattleState(BattleState battleState)
        {
            _battleState = battleState;
            UiUtil.ShowFloatingInfo("State change: " + battleState.ToString(), 0.5, 0.8, true, Colors.Turquoise, false, 0, 0, -100, 30, 500, 3000, 3000);

            switch (battleState)
            {
                case BattleState.EnemyTurn:
                case BattleState.SelectPartyCard: StateSelectPartyCard(); break;
            }

            SetYourTurnText(battleState);
        }

        private void RemoveAllHighlighting()
        {
            foreach (var ph in AllPlaceholders())
                ph.RemoveHighlight();
        }

        private void btnPlayRound_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _playRoundEvent.Set();
            //Close(Result.Yes);
        }

        private void btnSurrender_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Close(Result.No);
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

        private CardPlaceholderControl GetHandPlaceholder(GameLib.Battles.Player player, int position)
        {
            return IsPlayer1(player) ? _ply1Hand[position] : _ply2Hand[position];
        }

        private CardPlaceholderControl GetBoardPlaceholder(GameLib.Battles.Player player, int position)
        {
            return IsPlayer1(player) ? _ply1Board[position] : _ply2Board[position];
        }

        private GameLib.Battles.Battle GetBattle()
        {
            return _battle;
        }

        private bool IsPlayer1(GameLib.Battles.Player player)
        {
            return GetBattle().IsPlayer1(player);
        }

        IEnumerable<CardPlaceholderControl> AllPlaceholders()
        {
            return _ply1Board.Concat(_ply1Hand).Concat(_ply2Board).Concat(_ply2Hand);
        }

        IEnumerable<CardControl> AllCardControls()
        {
            return AllPlaceholders().Where(ph => ph.GetCardControl() != null).Select(ph => ph.GetCardControl());
        }

        CardControl CardControlFromCard(CardBattle card)
        {
            return AllCardControls().Where(cc => cc.Card == card).FirstOrDefault();
        }

        public CardPlaceholderControl GetOverlappedPlaceholder(Rect rect, double minCoverage = 0.2)
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

        private void ShowNotificationInternal(CardBattle card)
        {
            UiUtil.Post(() => { ShowNotificationAsync(card); });
        }

        private void ShowNotificationAsync(CardBattle card)
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
                    UiUtil.ShowFloatingInfo(text, 0.3, 0.3, true, Colors.Yellow);
                }
            }
            card.UiOutput.Clear();
        }

        private void ShowNotificationsInternal()
        {
            var battle = GetBattle();
            var all = battle.Board.RowPlayer1.Cards.Concat(battle.Board.RowPlayer2.Cards).Concat(battle.Player1.Hand.Cards).Concat(battle.Player2.Hand.Cards);
            foreach (var card in all)
            {
                ShowNotificationInternal(card);
            }

            foreach(var control in AllCardControls())
            {
                control.UpdateStats();
            }
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
                if (ph != null && ph.GetCardControl() != null)
                {
                    bool canAffordCard = true; // GetBattle().Player1.Mana >= ph.GetCardControl().Card.Mana; TODO: mana?
                    bool isOnTheMove = ph.GetCardControl().IsAligning;

                    ph.GetCardControl().IsSelected = canAffordCard && !isOnTheMove && !disable;
                }
            }
        }

        public void SetStatusMessage(string msg, int ms = 1500)
        {
            UiUtil.ShowLargeOverlayText(msg, 200, 300, ms);
        }

        private Point GetDealtCardStartPosition(GameLib.Battles.Player player, CardPlaceholderControl placeholder)
        {
            var result = new Point();
            result.X = this.ActualWidth / 2 - placeholder.ActualWidth / 2;
            result.Y = IsPlayer1(player) ? this.ActualHeight + placeholder.ActualHeight : 0 - placeholder.ActualHeight * 2;
            return result;
        }

        private CardControl CreateAndPlaceCardControl(CardBattle card, CardPlaceholderControl placeHolder, int position)
        {
            var cardControl = new CardControl(forBattle: true);
            CardControl.StatDisplayFlags displayFlags = card is CardBasicMob ? CardControl.StatDisplayFlags.All : CardControl.StatDisplayFlags.Level | CardControl.StatDisplayFlags.Power;
            cardControl.SetCard(card, displayFlags);

            cardControl.Position = position;
            cardControl.Visibility = Visibility.Visible;
            cardControl.IsDraggable = false;
            cardControl.IsZoomable = true;

            placeHolder.SetCardControl(cardControl);

            var p = GetDealtCardStartPosition(card.Owner, placeHolder);
            //cardControl.SetTranslateXY(-50, -25);
            //cardControl.SetTranslateXY(p.X, p.Y);
            //cardControl.AlignToElement(placeHolder, 500);

            return cardControl;
        }

        public IEnumerable<int> AddCardToHand(GameLib.Battles.Player player, CardBattle card, int position)
        {
            VikGame.Sound.Play("draw-card-2.wav");

            var placeholder = GetHandPlaceholder(player, position);
            var cardControl = CreateAndPlaceCardControl(card, placeholder, position);

            UpdateHandHighlighting();

            const int alignMs = 100;
            //cardControl.AlignToUserControl(placeholder, alignMs);

            yield return alignMs;
        }

        public IEnumerable<int> ShowCardPlacedOnEmptyPlaceHolder(GameLib.Battles.Player player, int handPosition, int boardPosition, bool isAi)
        {
            //SoundUtil.PlaySound(@"sounds/draw-card-2.wav");

            UpdateHandHighlighting();

            var phFrom = GetHandPlaceholder(player, handPosition);
            var phTo = GetBoardPlaceholder(player, boardPosition);

            var cardControl = GetHandPlaceholder(player, handPosition).GetCardControl();
            phFrom.ClearCardControl();
            phTo.SetCardControl(cardControl);

            const int alignMs = 300;

            // If placed by a human the card is already dragged and dropped at the correct position.
            // For AI it must be moved from hand to board.
            if (isAi)
            {
                //cardControl.AlignToUserControl(boardPlaceholder, alignMs);
            }

            yield return alignMs;
        }

        private void MoveCardToTargetAndDie(CardControl cardControl, UserControl target, bool isAi)
        {
            //cardControl.BringToFront();
            //int moveMs = 300 * (isAi ? 2 : 1);
            //cardControl.AlignToUserControl(target, moveMs);

            //Task.Delay(isAi ? moveMs : 0).ContinueWith((t) =>
            //{
            //    Ui.Send(() => { cardControl.FadeKill(); });
            //});
        }

        public IEnumerable<int> DropCardOnCard(CardBattle src, CardBattle dst, object data, bool isAi)
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
                    //AnimHelper.ApplyCardDamageAnimation(dstControl);

                    //foreach (var action in dstControl.CardAttackedEffectActions())
                    //    yield return action;

                }

                dstControl.UpdateStats();
            }
            else if (src is CardInstantSpellProperty && dst is CardBasicMob)
            {
                var instant = (CardInstantSpellProperty)src;
                var mob = (CardBasicMob)dst;
                mob.AddUiSpellOutput(instant.OnApplyMsg);

                var srcControl = CardControlFromCard(src);
                var dstControl = CardControlFromCard(dst);
                MoveCardToTargetAndDie(srcControl, dstControl, isAi);
                dstControl.UpdateStats();
            }
            else if (src is CardInstantDmgChange)
            {
                var instant = (CardInstantDmgChange)src;
                var mob = (CardBasicMob)dst;
                mob.AddUiSpellOutput(instant.OnApplyMsg);

                var dstControl = CardControlFromCard(dst);
                var srcControl = CardControlFromCard(src);
                MoveCardToTargetAndDie(srcControl, dstControl, isAi);
                dstControl.UpdateStats();
            }
            else if (src is CardInstantCustom)
            {
                var dstControl = CardControlFromCard(dst);
                var srcControl = CardControlFromCard(src);
                MoveCardToTargetAndDie(srcControl, dstControl, isAi);
                ShowNotificationsInternal();
            }
            else throw new InvalidOperationException("Unsupported card on card types");

            ShowNotificationsInternal();

            yield break;
        }

        public IEnumerable<int> CardVsCard(CardBasicMob src, CardBasicMob dst, int amount)
        {
            var srcControl = CardControlFromCard(src);
            var dstControl = CardControlFromCard(dst);

            srcControl.BringToFront();

            int ms = 400;
            //AnimHelper.ApplyCardVsCardAnimation(srcControl, dstControl, ms);

            yield return ms;

            ShowNotificationsInternal();
            ShowCardHpChange((CardBasicMob)dstControl.Card, amount);

            //AnimHelper.ApplyCardDamageAnimation(dstControl);

            //foreach (var action in dstControl.CardAttackedEffectActions())
            //    yield return action;

            dstControl.UpdateStats();
        }

        public Point GetPosition(Control control)
        {
            Point position = control.TransformToAncestor(cardGrid).Transform(new Point(0, 0));
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
                if (ph.GetCardControl() != null && ((CardBasicMob)ph.GetCardControl().Card).IsDead)
                {
                    deadCardsFound = true;
                    Action postAction = () =>
                    {
                        if (ph.GetCardControl() != null)
                        {
                            //ph.GetCardControl().Kill();
                            ph.ClearCardControl();
                        }
                    };

                    //AnimHelper.ApplyKillCardAnimation(ph.CardControl, ms, postAction);
                }
            }

            // Safety: Don't move on until dead cards are removed from the model
            if (deadCardsFound)
                yield return ms + 100;
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
            UiUtil.Send(() =>
            {
                var control = CardControlFromCard(card);
                //ShowFloatingInfoMid(control, AmountString(amount), amount > 0 ? ColorDmgInc : ColorDmgDec, FloatingInfoParam.CategoryType.DmgChange);
                control.UpdateStats();
            });
        }

        public void ShowCardHpChange(CardBasicMob card, int amount)
        {
            UiUtil.Send(() =>
                {
                    var control = CardControlFromCard(card);
                    //ShowFloatingInfoMid(control, AmountString(amount), amount > 0 ? ColorHpInc : ColorHpDec, FloatingInfoParam.CategoryType.HpChange);
                    control.UpdateStats();
                });
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
            //foreach (var cardControl in AllCardControls())
            //    cardControl.Kill();
            ResetPlaceholders();
        }

        //public void ShowBattleBoard()
        //{
        //    ResetAll();
        //    this.Visibility = Visibility.Visible;
        //    EnableUserInteraction(false);
        //}

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

                    foreach (var cardControl in AllCardControls())
                        cardControl.IsZoomable = enable;
                };

            return action;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ResetAll();
        }

        void IBattleObserver.BattleStart(GameLib.Battles.Player firstPlayer)
        {
            UiUtil.Send(() => { BattleStartUi(firstPlayer); });
        }

        void BattleStartUi(GameLib.Battles.Player firstPlayer)
        {
            //UiUtil.ShowLargeOverlayText(firstPlayer.Name + " begins", 2000);
        }

        void IBattleObserver.BeforePlayerTurn(GameLib.Battles.Player player)
        {
            UiUtil.Send(() => BeforePlayerTurnUi(player));
        }

        void BeforePlayerTurnUi(GameLib.Battles.Player player)
        {
            if (_battle.IsPlayer1(player))
            {
                SetStatusMessage("Your turn, " + player.Name, 1500);
                SetBattleState(BattleState.SelectPartyCard);
            }
            else
            {
                SetBattleState(BattleState.EnemyTurn);
            }
        }

        void IBattleObserver.ShowNotifications()
        {
            UiUtil.Send(() => ShowNotificationsInternal());
        }

        void IBattleObserver.RemoveDeadCardsOnBoard()
        {
            UiUtil.Send(() => { });
        }

        void IBattleObserver.CardPlacedOnBoard(CardBasicMob card, int handPosition, int boardPosition, bool isAi)
        {
            UiUtil.Send(() => { });
        }

        void IBattleObserver.PartyCardPlaced(GameLib.Battles.Player player, int position)
        {
            var card = player.Party[position];
            SequentialActions.RunAsync(PlacePartyCard(player, card, position));
        }

        public IEnumerable<int> PlacePartyCard(GameLib.Battles.Player player, CardBattle card, int position)
        {
            //VikGame.Sound.Play("SlimeDamageA.ogg");
            
            var placeholder = GetBoardPlaceholder(player, position);
            var cardControl = CreateAndPlaceCardControl(card, placeholder, position);

            //UiUtil.AddColorFlashingAdorner(cardControl, Colors.White, 1, 100, 5000, 10500);
            //const int alignMs = 300;
            //yield return alignMs;
            yield break;
        }

        void IBattleObserver.CardDrawn(GameLib.Battles.Player player, int position)
        {
            var card = player.Hand.Cards[position];
            SequentialActions.RunBlocking(AddCardToHand(player, card, position));
        }

        void IBattleObserver.DropCardOnCard(CardBattle src, CardBattle dst, object data, bool isAi)
        {
            UiUtil.Send(() => { });
        }

        void IBattleObserver.ShowPlayerInfo(GameLib.Battles.Player player, string info)
        {
            UiUtil.Post(() => { UiUtil.ShowFloatingInfo(info, 0.5, 0.5, true, Colors.Yellow); });
        }

        void IBattleObserver.BoardCardVsCard(CardBasicMob src, CardBasicMob dst, int amount)
        {
            UiUtil.Send(() => { });
        }

        void IBattleObserver.AiArtificialDelay()
        {
            UiUtil.Send(() => {  });
        }

        void IBattleObserver.BattleEnded(GameLib.Battles.Player winner, GameLib.Battles.Player loser)
        {
            UiUtil.Send(() => { BattleEndedUi(winner, loser); });
        }

        void BattleEndedUi(GameLib.Battles.Player winner, GameLib.Battles.Player loser)
        {
        }
    }
}
