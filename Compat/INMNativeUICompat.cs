using System.Collections.Generic;
using System.Drawing;
using GTA;
using LemonUI;
using LemonUI.Menus;
using LemonUI.Tools;
using System;

namespace BennysMotorworksRevamped.Compat
{
    public class MenuPool
    {
        private readonly ObjectPool _pool = new ObjectPool();
        private readonly List<UIMenu> _menus = new List<UIMenu>();

        public bool IsAnyMenuOpen => _pool.AreAnyVisible;

        public void Add(UIMenu menu)
        {
            if (menu == null)
            {
                return;
            }

            _menus.Add(menu);
            _pool.Add(menu.NativeMenu);
        }

        public void ProcessMenus() => _pool.Process();

        public void CloseAllMenus()
        {
            UIMenu.HideAll();
        }

        public void UpdateStats(float topSpeed, float acceleration, float braking, float traction)
        {
        }
    }

    public class UIMenu
    {
        private static readonly List<UIMenu> RegisteredMenus = new List<UIMenu>();
        private static bool IsVisibilityTransition;
        private static UIMenu ActiveMenu;

        public delegate void MenuCloseEvent(UIMenu sender);
        public delegate void ItemSelectEvent(UIMenu sender, UIMenuItem selectedItem, int index);
        public delegate void IndexChangedEvent(UIMenu sender, UIMenuItem selectedItem, int index);

        public event MenuCloseEvent OnMenuClose;
        public event ItemSelectEvent OnItemSelect;
        public event IndexChangedEvent OnIndexChange;

        public UIMenu(string title, string subtitle)
        {
            NativeMenu = new NativeMenu(title ?? string.Empty, subtitle ?? string.Empty);
            MenuItems = new List<UIMenuItem>();
            RegisteredMenus.Add(this);

            NativeMenu.Closed += (sender, args) =>
            {
                if (IsVisibilityTransition)
                {
                    return;
                }

                if (ReferenceEquals(ActiveMenu, this))
                {
                    ActiveMenu = null;
                }

                bool suppressCallback = SuppressCloseCallbackOnce;
                bool restoreParent = ParentMenu != null && !SuppressParentRestoreOnce;

                SuppressCloseCallbackOnce = false;
                SuppressParentRestoreOnce = false;

                if (restoreParent)
                {
                    ParentMenu.Visible = true;
                }

                if (!suppressCallback)
                {
                    OnMenuClose?.Invoke(this);
                }
            };

            NativeMenu.ItemActivated += (sender, args) =>
            {
                int index = NativeMenu.SelectedIndex;
                if (index >= 0 && index < MenuItems.Count)
                {
                    UIMenuItem selected = MenuItems[index];
                    if (selected?.Submenu != null)
                    {
                        SuppressParentRestoreOnce = true;
                        SuppressCloseCallbackOnce = true;
                        Visible = false;
                        selected.Submenu.Visible = true;
                        return;
                    }

                    OnItemSelect?.Invoke(this, selected, index);
                }
            };

            NativeMenu.SelectedIndexChanged += (sender, args) =>
            {
                int index = args.Index;
                if (index >= 0 && index < MenuItems.Count)
                {
                    OnIndexChange?.Invoke(this, MenuItems[index], index);
                }
            };
        }

        public UIMenu(string title, string subtitle, bool showStats) : this(title, subtitle)
        {
        }

        public NativeMenu NativeMenu { get; }
        public List<UIMenuItem> MenuItems { get; }
        public bool MouseEdgeEnabled { get; set; }
        public UIMenu ParentMenu { get; internal set; }
        private bool SuppressParentRestoreOnce { get; set; }
        private bool SuppressCloseCallbackOnce { get; set; }
        public int Size => MenuItems.Count;

        internal static void HideAll()
        {
            SetExclusiveVisibleMenu(null);
        }

        internal static void ShowOnly(UIMenu menu)
        {
            SetExclusiveVisibleMenu(menu);
        }

        internal static void EnsureSingleVisibleMenu()
        {
            UIMenu visibleMenu = null;
            int visibleCount = 0;

            foreach (UIMenu menu in RegisteredMenus)
            {
                if (!menu.NativeMenu.Visible)
                {
                    continue;
                }

                visibleCount++;
                if (ReferenceEquals(menu, ActiveMenu))
                {
                    visibleMenu = menu;
                }
                else if (visibleMenu == null)
                {
                    visibleMenu = menu;
                }
            }

            if (visibleCount == 0)
            {
                ActiveMenu = null;
            }
            else if (visibleCount == 1)
            {
                ActiveMenu = visibleMenu;
            }
            else
            {
                SetExclusiveVisibleMenu(visibleMenu);
            }
        }

        private static void SetExclusiveVisibleMenu(UIMenu menuToShow)
        {
            bool previousTransitionState = IsVisibilityTransition;
            IsVisibilityTransition = true;

            try
            {
                foreach (UIMenu menu in RegisteredMenus)
                {
                    menu.NativeMenu.Visible = ReferenceEquals(menu, menuToShow);
                }

                ActiveMenu = menuToShow;
            }
            finally
            {
                IsVisibilityTransition = previousTransitionState;
            }
        }

        public bool Visible
        {
            get => NativeMenu.Visible;
            set
            {
                if (value)
                {
                    ShowOnly(this);
                }
                else
                {
                    NativeMenu.Visible = false;
                }
            }
        }

        public Point GetUIMenuOffset => new Point(0, 0);

        public void AddItem(UIMenuItem item)
        {
            if (item == null)
            {
                return;
            }

            item.NativeItem.Tag = item;
            MenuItems.Add(item);
            NativeMenu.Add(item.NativeItem);
        }

        public void BindMenuToItem(UIMenu submenu, UIMenuItem item)
        {
            if (submenu == null || item == null)
            {
                return;
            }

            item.Submenu = submenu;
            submenu.ParentMenu = this;

            if (string.IsNullOrWhiteSpace(item.NativeItem.AltTitle))
            {
                item.NativeItem.AltTitle = ">";
            }
        }

        public void RefreshIndex()
        {
        }

        public void SetBannerType(object sprite)
        {
        }

        public void AddInstructionalButton(InstructionalButton button)
        {
            if (button != null)
            {
                NativeMenu.Buttons.Add(button.NativeButton);
            }
        }

        public static Point GetSafezoneBounds
        {
            get
            {
                PointF safe = SafeZone.GetSafePosition(new PointF(0.0f, 0.0f));
                return new Point((int)(safe.X * GTA.UI.Screen.Width), (int)(safe.Y * GTA.UI.Screen.Height));
            }
        }

        public static Size GetScreenResolutionMaintainRatio => GTA.UI.Screen.Resolution;

        public static bool IsMouseInBounds(Point position, Size size)
        {
            PointF topLeft = new PointF(
                position.X / (float)GTA.UI.Screen.Width,
                position.Y / (float)GTA.UI.Screen.Height);

            SizeF normalizedSize = new SizeF(
                size.Width / (float)GTA.UI.Screen.Width,
                size.Height / (float)GTA.UI.Screen.Height);

            return GameScreen.IsCursorInArea(topLeft, normalizedSize);
        }
    }

    public class UIMenuItem
    {
        private string _rightLabel = string.Empty;

        public enum BadgeStyle
        {
            None,
            Car,
        }

        public UIMenuItem(string text)
            : this(text, string.Empty)
        {
        }

        public UIMenuItem(string text, string description)
        {
            NativeItem = new NativeItem(text ?? string.Empty, description ?? string.Empty);
            NativeItem.Tag = this;
        }

        public NativeItem NativeItem { get; internal set; }
        public UIMenu Submenu { get; internal set; }
        public object Tag { get; set; }

        public string Text
        {
            get => NativeItem.Title;
            set => NativeItem.Title = value ?? string.Empty;
        }

        public string Description
        {
            get => NativeItem.Description;
            set => NativeItem.Description = value ?? string.Empty;
        }

        public BadgeStyle RightBadge { get; private set; }

        public void SetRightLabel(string value)
        {
            _rightLabel = value ?? string.Empty;
            RefreshRightText();
        }

        public void SetRightBadge(BadgeStyle value)
        {
            RightBadge = value;
            RefreshRightText();
        }

        private void RefreshRightText()
        {
            if (RightBadge == BadgeStyle.Car)
            {
                NativeItem.AltTitle = "Purchased";
            }
            else if (!string.IsNullOrEmpty(_rightLabel))
            {
                NativeItem.AltTitle = _rightLabel;
            }
            else
            {
                NativeItem.AltTitle = Submenu != null ? ">" : string.Empty;
            }
        }
    }

    public class InstructionalButton
    {
        private string _text;

        public InstructionalButton(Control control, string text)
        {
            _text = text ?? string.Empty;
            NativeButton = new LemonUI.Scaleform.InstructionalButton(_text, control);
        }

        public LemonUI.Scaleform.InstructionalButton NativeButton { get; }

        public string Text
        {
            get => _text;
            set => _text = value ?? string.Empty;
        }
    }
}
