using System;
using System.Windows;
using GameLib.Battles.Cards;
using Vik.Code.Controls.Cards;

namespace Vik.Code.Utility
{
    public class CardDragEventArgs
    {
        public CardDragEventType DragEvent { get; set; }
        public Card Card { get; set; }
        public Rect DragRect { get; set; }
        public bool Accept { get; set; }
        public FrameworkElement AcceptingElement { get; set; } // Used for visual feedback when card is dropped
        public CardPlaceholderControl OwningPlaceholder { get; set; }
    }

    public enum CardDragEventType { DragBegin, DragMove, DropQueryAccept, Dropped };

    public class DragDropManager
    {
        public event EventHandler<CardDragEventArgs> CardDrag = delegate { };

        public void OnCardDrag(object sender, CardDragEventArgs e)
        {
            CardDrag(sender, e);
        }
    }
}
