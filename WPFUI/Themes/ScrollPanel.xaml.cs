using AyoControlLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFUI.Themes
{
    /// <summary>
    /// Interaction logic for ScrollPanel.xaml
    /// </summary>
    public partial class ScrollPanel : UserControl
    {
        public event GuidSenderHandler OnItemSelected;

        private const int DEFAULT_GAP = 3;

        private int _gap = DEFAULT_GAP;

        private StackPanel _stack = new StackPanel();

        private Dictionary<Guid, Control> _controls = new Dictionary<Guid, Control>();



        public Control this[Guid g]
        {
            get { if (_controls.ContainsKey(g)) return _controls[g]; else return null; }
            set { if (_controls.ContainsKey(g)) _controls[g] = value; }
        }


        public int CornerRadius { get => Back.CornerRadius; set { Back.CornerRadius = value; InvalidateVisual(); } }
        public int BorderSize { get => Back.BorderSize; set { Back.BorderSize = value; InvalidateVisual(); } }


        public ERoundedType RoundedType { get => Back.RoundedType; set { Back.RoundedType = value; InvalidateVisual(); } }
        public ERoundedFlag RoundedFlag { get => Back.RoundedFlag; set { Back.RoundedFlag = value; InvalidateVisual(); } }


        public Color? ColorBackEnable { get => Back.ColorBackEnable; set { Back.ColorBackEnable = value; InvalidateVisual(); } }
        public Color? ColorBackDisable { get => Back.ColorBackDisable; set { Back.ColorBackDisable = value; InvalidateVisual(); } }
        public Color? ColorBorderEnable { get => Back.ColorBorderEnable; set { Back.ColorBorderEnable = value; InvalidateVisual(); } }
        public Color? ColorBorderDisable { get => Back.ColorBorderEnable; set { Back.ColorBorderEnable = value; InvalidateVisual(); } }

        public List<Control> Controls => _controls.Values.ToList();

        public ScrollPanel()
        {
            InitializeComponent();
            //(Back.Content as ScrollViewer).Content = _stack;
            //for (int i = 0; i < 10; i++)
            //{
            //    var temp = new JobChoiceUC();
            //    temp.Padding = new Thickness(_gap);
            //    temp.Width = _stack.Width;
            //    _stack.Children.Add(temp);
            //}
        }


        public void Remove(Control ctrl)
        {
            Guid g = Guid.Empty;
            foreach (var item in _controls)
            {
                if (item.Value == ctrl)
                {
                    g = item.Key;
                    break;
                }
            }

            if (g != Guid.Empty)
            {
                _stack.Children.Remove(_controls[g]);
                _controls.Remove(g);
            }
        }

        public void Remove(Guid guid)
        {
            if (_controls.ContainsKey(guid))
            {
                _stack.Children.Remove(_controls[guid]);
                _controls.Remove(guid);
            }
        }

        public void Add(UserControl ctrl,  Guid? g = null)
        {   
            Guid guid = g.HasValue? g.Value : Guid.NewGuid();
            _controls.Add(guid, ctrl);
            _stack.Children.Add(ctrl);
            ctrl.Tag = guid;

            if (ctrl is IClickable)
            {
                (ctrl as IClickable).OnClick -= Item_OnClick;
                (ctrl as IClickable).OnClick += Item_OnClick;
            }

        }

        private void Item_OnClick(object sender, EventArgs e)
        {
            if (!(sender is Control))
                return;

            if (!((sender as Control).Tag is Guid))
                return;

            Guid g = (Guid)(sender as Control).Tag;

            foreach (var item in _controls)
            {
                if (!(item.Value is IActivable))
                    continue;

                (item.Value as IActivable).IsActiv = item.Key == g;
            }

            OnItemSelected?.Invoke(this, new GuidSelecEventArg(g));
        }
    }


}
