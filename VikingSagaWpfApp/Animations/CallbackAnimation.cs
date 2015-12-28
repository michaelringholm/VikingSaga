using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace VikingSagaWpfApp.Animations
{
    public class CallbackAnimation : UIElement
    {
        public static readonly DependencyProperty Prop = DependencyProperty.Register("Value", typeof(double), typeof(CallbackAnimation), new PropertyMetadata(ValueChangeCallback));

        private DoubleAnimation _anim;
        public double Value { get; set; }

        public event EventHandler ValueChanged = delegate { };

        public CallbackAnimation(DoubleAnimation anim)
        {
            _anim = anim;
        }

        public CallbackAnimation(int ms)
        {
            var anim = new DoubleAnimation(0.0, 1.0, new Duration(new TimeSpan(0, 0, 0, 0, ms)));
            _anim = anim;
        }

        public void Begin()
        {
            this.BeginAnimation(Prop, _anim);
        }

        private static void ValueChangeCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = (CallbackAnimation)d;
            obj.Value = (double)e.NewValue;
            obj.ValueChanged(obj, EventArgs.Empty);
        }
    }
}
