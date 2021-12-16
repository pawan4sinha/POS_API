using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace POS.Retail.Utilities
{
    #region ShapedPanel

    public class ShapedPanel : Panel
    {
        private Pen pen = new Pen(Color.White, penWidth);
        private Color _FillColor = ColorTranslator.FromHtml("#ffffff");
        private Color _ShadeColor = ColorTranslator.FromHtml("#ffffff");
        private static readonly float penWidth = 2f;
        private Color _borderColor = Color.White;
        private int _radius = 8;

        public ShapedPanel()
        {
            this.ResizeRedraw = true;
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
        }

        [Browsable(true)]
        public Color BorderColor
        {
            get { return _borderColor; }
            set
            {
                _borderColor = value;
                pen = new Pen(_borderColor, penWidth);
                Invalidate();
            }
        }
        public Color FillColor { get { return _FillColor; } set { _FillColor = value; } }
        public Color ShadeColor { get { return _ShadeColor; } set { _ShadeColor = value; } }
        public int Radius { get { return _radius; } set { _radius = value; } }

        protected override void OnPaint(PaintEventArgs e)
        {
            Color darkDarkColor = CustomGUIHelper.DarkenColor(this.BackColor, 12);

            Rectangle r = this.ClientRectangle;
            GraphicsPath path = CustomGUIHelper.RoundRectangle(r, Radius, Corners.All);
            LinearGradientBrush paintBrush = new LinearGradientBrush(r, FillColor, ShadeColor, LinearGradientMode.Vertical);
            Blend b = new Blend();
            b.Positions = new float[] { 0, 0.45F, 0.55F, 1 };
            b.Factors = new float[] { 0, 0, 1, 1 };
            paintBrush.Blend = b;

            //Draw the Background
            e.Graphics.FillPath(paintBrush, path);
            paintBrush.Dispose();

            //...and border
            Pen drawingPen = new Pen(darkDarkColor);
            e.Graphics.DrawPath(drawingPen, path);
            drawingPen.Dispose();
        }


        private int _edge = 50;
        [Browsable(true)]
        public int Edge
        {
            get { return _edge; }
            set
            {
                _edge = value;
                Invalidate();
            }
        }

        private Rectangle GetLeftUpper(int e)
        {
            return new Rectangle(0, 0, e, e);
        }

        private Rectangle GetRightUpper(int e)
        {
            return new Rectangle(Width - e, 0, e, e);
        }

        private Rectangle GetRightLower(int e)
        {
            return new Rectangle(Width - e, Height - e, e, e);
        }

        private Rectangle GetLeftLower(int e)
        {
            return new Rectangle(0, Height - e, e, e);
        }

        private void ExtendedDraw(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            GraphicsPath path = new GraphicsPath();
            path.StartFigure();
            path.StartFigure();
            path.AddArc(GetLeftUpper(Edge), 180, 90);
            path.AddLine(Edge, 0, Width - Edge, 0);
            path.AddArc(GetRightUpper(Edge), 270, 90);
            path.AddLine(Width, Edge, Width, Height - Edge);
            path.AddArc(GetRightLower(Edge), 0, 90);
            path.AddLine(Width - Edge, Height, Edge, Height);
            path.AddArc(GetLeftLower(Edge), 90, 90);
            path.AddLine(0, Height - Edge, 0, Edge);
            path.CloseFigure();
            Region = new Region(path);
        }

        private void DrawSingleBorder(Graphics graphics)
        {
            graphics.DrawArc(pen, new Rectangle(0, 0, Edge, Edge), 180, 90);
            graphics.DrawArc(pen, new Rectangle(Width - Edge - 1, -1, Edge, Edge), 270, 90);
            graphics.DrawArc(pen, new Rectangle(Width - Edge - 1, Height - Edge - 1, Edge, Edge), 0, 90);
            graphics.DrawArc(pen, new Rectangle(0, Height - Edge - 1, Edge, Edge), 90, 90);
            graphics.DrawRectangle(pen, 0f, 0f, Convert.ToSingle((Width - 1)), Convert.ToSingle((Height - 1)));
        }

        private void Draw3DBorder(Graphics graphics)
        {
            //TODO Implement 3D border
        }

        private void DrawBorder(Graphics graphics)
        {
            DrawSingleBorder(graphics);
        }


    }

    #endregion

    #region RoundedCornerTextbox

    public class RoundedCornerTextbox : TextBox
    {
        [DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        public static extern IntPtr GetWindowDC(IntPtr hwnd);
        [DllImport("user32.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        #region "Globals"
        private ArrayList mLeavingEventlist;
        private string _OriText = "";
        private string _NewText = "";
        private bool _Texthaschanged = false;
        private bool _Multiline = false;
        states state = states.normal;
        Pen BorderPen;
        Brush TextBrush;
        Rectangle MainRect;
        PointF[] pointArrow = new PointF[3];
        GraphicsPath ArrowPath = new GraphicsPath();

        PointF TxtLoc;
        public enum states
        {
            normal,
            focused,
            disabled
        }

        #endregion

        #region "Public properties"
        public bool Texthaschanged
        {
            get { return _Texthaschanged; }
            set { _Texthaschanged = value; }
        }

        public string OriText
        {
            get { return _OriText; }
            set { _OriText = value; }
        }

        public string NewText
        {
            get { return _NewText; }
            set { _NewText = value; }
        }

        public bool Multiline
        {
            get { return _Multiline; }
            set { _Multiline = value; }
        }

        #endregion

        #region "Events"

        public event EventHandler BijLeaving
        {
            add
            {
                if (mLeavingEventlist == null)
                {
                    mLeavingEventlist = new ArrayList();
                }
                mLeavingEventlist.Add(value);
            }
            remove { mLeavingEventlist.Remove(value); }

        }
        #endregion

        #region "Listeners"

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            bool result = false;
            if (msg.WParam.ToInt32() == Convert.ToInt32(Keys.Enter))
            {
                //SendKeys.Send("{Tab}");
                ////SendKeys.Send("{Enter}");
                //result = true;
            }
            else if (msg.WParam.ToInt32() == Convert.ToInt32(Keys.Decimal))
            {
                SendKeys.Send(",");
                result = true;
            }
            return result;
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {

                case 0xf:
                    //WM_PAINT
                    Rectangle rect = new Rectangle(0, 0, base.Width, base.Height);

                    IntPtr hDC = GetWindowDC(this.Handle);
                    Graphics g = Graphics.FromHdc(hDC);
                    //if (this.Enabled)
                    //{
                    //    g.Clear(Color.White);
                    //}
                    //else
                    //{
                    //    g.Clear(Color.FromName("control"));
                    //}
                    DrawBorder(g);
                    //DrawText(g);
                    ReleaseDC(this.Handle, hDC);
                    g.Dispose();

                    break;
                case 0x7:
                case 0x8:
                case 0x200:
                case 0x2a3:
                    //CMB_DROPDOWN, CMB_CLOSEUP, WM_SETFOCUS, 
                    //WM_KILLFOCUS, WM_MOUSEMOVE,  
                    //WM_MOUSELEAVE (if you move the mouse fast over
                    //the combobox, mouseleave doesn't always react)

                    UpdateState();
                    break;
            }
            base.WndProc(ref m);
        }

        private void Enabled_Changed(object sender, EventArgs e)
        {
            UpdateState();
        }

        private Timer withEventsField_Timer = new Timer();
        private Timer Timer
        {
            get { return withEventsField_Timer; }
            set
            {
                if (withEventsField_Timer != null)
                {
                    withEventsField_Timer.Tick -= Timer_Tick;
                }
                withEventsField_Timer = value;
                if (withEventsField_Timer != null)
                {
                    withEventsField_Timer.Tick += Timer_Tick;
                }
            }

        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            UpdateState();
        }

        protected override void Dispose(bool e)
        {
            this.Timer.Enabled = false;
            base.Dispose(e);
        }

        private void UpdateState()
        {
            //save the current state
            states temp = state;
            //
            if (this.Enabled)
            {
                if (ClientRectangle.Contains(PointToClient(Control.MousePosition)))
                {
                    this.state = states.focused;
                }
                else if (this.Focused)
                {
                    this.state = states.focused;
                }
                else
                {
                    this.state = states.normal;
                }
            }
            else
            {
                this.state = states.disabled;
            }

            if (state != temp)
            {
                this.Invalidate();
            }
        }

        #endregion

        #region "Initialisation"
        public RoundedCornerTextbox()
            : base()
        {
            this.ResizeRedraw = true;
            EnabledChanged += Enabled_Changed;
            base.Multiline = this.Multiline;
            base.Height = 23;
            Timer.Interval = 20;
            Timer.Enabled = true;
        }
        #endregion

        #region " Drawing functions "

        private void TekenRondeRechthoek(Graphics g, Pen pen, Rectangle rectangle, float radius)
        {
            float size = (radius * 2f);
            GraphicsPath gp = new GraphicsPath();
            gp.AddArc(rectangle.X, rectangle.Y, size, size, 180, 90);
            gp.AddArc((rectangle.X + (rectangle.Width - size)), rectangle.Y, size, size, 270, 90);
            gp.AddArc((rectangle.X + (rectangle.Width - size)), (rectangle.Y + (rectangle.Height - size)), size, size, 0, 90);
            gp.AddArc(rectangle.X, (rectangle.Y + (rectangle.Height - size)), size, size, 90, 90);
            gp.CloseFigure();
            g.DrawPath(pen, gp);
            gp.Dispose();
        }

        private void DrawBorder(Graphics g)
        {
            MainRect = new Rectangle(0, 0, this.Width - 1, this.Height - 1);
            switch (state)
            {
                case states.focused:
                    //BorderPen = new Pen(Color.Blue);
                    BorderPen = new Pen(ColorTranslator.FromHtml("#C0C0C0"));
                    break;
                case states.disabled:
                    BorderPen = new Pen(Color.DarkGray);
                    break;
                case states.normal:
                    //BorderPen = new Pen(Color.DimGray);
                    BorderPen = new Pen(ColorTranslator.FromHtml("#C0C0C0"));
                    break;
                default:
                    return;

                    break;
            }
            TekenRondeRechthoek(g, BorderPen, MainRect, 3f);

        }

        private void DrawText(Graphics g)
        {
            string text = "";
            switch (state)
            {
                case states.normal:
                case states.focused:
                    TextBrush = new SolidBrush(this.ForeColor);
                    break;
                case states.disabled:
                    TextBrush = new SolidBrush(Color.DarkGray);
                    break;
            }
            if (g.MeasureString(this.Text, this.Font).Width > this.Width - 30)
            {
                int i = -1;
                do
                {
                    i += 1;
                    if (g.MeasureString(text, this.Font).Width > this.Width - 30)
                        break; // TODO: might not be correct. Was : Exit Do
                    text += this.Text.Substring(i, 1);
                } while (true);
            }
            else
            {
                text = this.Text;
            }
            if (this.RightToLeft == System.Windows.Forms.RightToLeft.No)
            {
                TxtLoc = new PointF(1, 4);
            }
            else
            {
                float temp = this.Width - (g.MeasureString(text, this.Font).Width);
                TxtLoc = new PointF(temp, 4);
            }
            //g.DrawString(text, this.Font, TextBrush, TxtLoc);
            g.DrawString(text, this.Font, TextBrush, TxtLoc);
        }
        #endregion

        #region "Overrides"

        protected override void OnEnter(EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Text))
            {
                OriText = this.Text;
            }
        }

        protected override void OnLeave(EventArgs e)
        {
            if (this.Text != OriText)
            {
                Texthaschanged = true;
                if (!string.IsNullOrEmpty(this.Text))
                {
                    NewText = this.Text;
                }
                //if (BijLeaving != null)
                //{
                //    BijLeaving(this, EventArgs.Empty);
                //}
            }
        }

        #endregion

    }

    #endregion

    #region CustomButton



    public enum CustomButtonState
    {
        Normal = 1,
        Hot,
        Pressed,
        Disabled,
        Focused
    }
    public class CustomButton : Control, IButtonControl
    {
        public CustomButton()
            : base()
        {
            this.ResizeRedraw = true;
            this.SetStyle(ControlStyles.Selectable | ControlStyles.StandardClick | ControlStyles.ResizeRedraw
                | ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer | ControlStyles.UserPaint
                | ControlStyles.SupportsTransparentBackColor | ControlStyles.OptimizedDoubleBuffer, true);
        }


        #region Private Instance Variables

        private DialogResult m_DialogResult;
        private bool m_IsDefault;

        private int m_CornerRadius = 10;//8;
        private Corners m_RoundCorners;
        private CustomButtonState m_ButtonState = CustomButtonState.Normal;

        private ContentAlignment m_ImageAlign = ContentAlignment.MiddleCenter;
        private ContentAlignment m_TextAlign = ContentAlignment.MiddleCenter;
        private ImageList m_ImageList;
        private int m_ImageIndex = -1;

        private bool keyPressed;
        private Rectangle contentRect;

        private Color _FillColor = ColorTranslator.FromHtml("#a6d333");
        private Color _ShadeColor = ColorTranslator.FromHtml("#80af13");
        private Color _ButtonBoardColor = ColorTranslator.FromHtml("#96C927");

        private Color _DisableFillColor = ColorTranslator.FromHtml("#E4E9EF");
        private Color _DisableShadeColor = ColorTranslator.FromHtml("#80af13");
        private Color _DisableButtonBoardColor = ColorTranslator.FromHtml("#DCE3E8");
        private bool _EnableFocus = true;

        private bool _IsChangeColorOnClick = false;
        private Color _FillColorOnClick = ColorTranslator.FromHtml("#a6d333");
        private Color _ShadeColorOnClick = ColorTranslator.FromHtml("#80af13");
        private Color _ButtonBoardColorOnClick = ColorTranslator.FromHtml("#96C927");
        #endregion

        #region IButtonControl Implementation

        [Category("Behavior"), DefaultValue(typeof(DialogResult), "None")]
        [Description("The dialog result produced in a modal form by clicking the button.")]
        public DialogResult DialogResult
        {
            get { return m_DialogResult; }
            set
            {
                if (Enum.IsDefined(typeof(DialogResult), value))
                    m_DialogResult = value;
            }
        }


        public void NotifyDefault(bool value)
        {
            if (m_IsDefault != value)
                m_IsDefault = value;
            this.Invalidate();
        }


        public void PerformClick()
        {
            if (this.CanSelect)
                base.OnClick(EventArgs.Empty);
        }


        #endregion

        #region Properties

        //Button Backgound Color
        public Color FillColor
        {
            get
            {
                return _FillColor;
            }
            set
            {
                _FillColor = value;
            }
        }
        public Color ShadeColor
        {
            get
            {
                return _ShadeColor;
            }
            set
            {
                _ShadeColor = value;
            }
        }
        public Color ButtonBoardColor
        {
            get
            {
                return _ButtonBoardColor;
            }
            set
            {
                _ButtonBoardColor = value;
            }
        }

        public bool IsChangeColorOnClick
        {
            get
            {
                return _IsChangeColorOnClick;
            }
            set
            {
                _IsChangeColorOnClick = value;
            }
        }
        public Color FillColorOnClick
        {
            get
            {
                return _FillColorOnClick;
            }
            set
            {
                _FillColorOnClick = value;
            }
        }
        public Color ShadeColorOnClick
        {
            get
            {
                return _ShadeColorOnClick;
            }
            set
            {
                _ShadeColorOnClick = value;
            }
        }
        public Color ButtonBoardColorOnClick
        {
            get
            {
                return _ButtonBoardColorOnClick;
            }
            set
            {
                _ButtonBoardColorOnClick = value;
            }
        }

        public Color DisableFillColor
        {
            get
            {
                return _DisableFillColor;
            }
            set
            {
                _DisableFillColor = value;
            }
        }
        public Color DisableShadeColor
        {
            get
            {
                return _DisableShadeColor;
            }
            set
            {
                _DisableShadeColor = value;
            }
        }
        public Color DisableButtonBoardColor
        {
            get
            {
                return _DisableButtonBoardColor;
            }
            set
            {
                _DisableButtonBoardColor = value;
            }
        }

        public bool EnableFocus
        {
            get
            {
                return _EnableFocus;
            }
            set
            {
                _EnableFocus = value;
            }
        }

        //ButtonState
        [Browsable(false)]
        public CustomButtonState ButtonState
        {
            get { return m_ButtonState; }
        }


        //CornerRadius
        [Category("Appearance")]
        [DefaultValue(8)]
        [Description("Defines the radius of the controls RoundedCorners.")]
        public int CornerRadius
        {
            get { return m_CornerRadius; }
            set
            {
                if (m_CornerRadius == value)
                    return;
                m_CornerRadius = value;
                this.Invalidate();
            }
        }


        //DefaultSize
        protected override System.Drawing.Size DefaultSize
        {
            get { return new Size(75, 23); }
        }


        //IsDefault
        [Browsable(false)]
        public bool IsDefault
        {
            get { return m_IsDefault; }
        }


        //ImageList
        [Category("Appearance"), DefaultValue(typeof(ImageList), null)]
        [Description("The image list to get the image to display in the face of the control.")]
        public ImageList ImageList
        {
            get { return m_ImageList; }
            set
            {
                m_ImageList = value;
                this.Invalidate();
            }
        }


        //ImageIndex
        [Category("Appearance"), DefaultValue(-1)]
        [Description("The index of the image in the image list to display in the face of the control.")]
        [TypeConverter(typeof(ImageIndexConverter))]
        [Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design", typeof(System.Drawing.Design.UITypeEditor))]
        public int ImageIndex
        {
            get { return m_ImageIndex; }
            set
            {
                m_ImageIndex = value;
                this.Invalidate();
            }
        }


        //ImageAlign
        [Category("Appearance"), DefaultValue(typeof(ContentAlignment), "MiddleCenter")]
        [Description("The alignment of the image that will be displayed in the face of the control.")]
        public ContentAlignment ImageAlign
        {
            get { return m_ImageAlign; }
            set
            {
                if (!Enum.IsDefined(typeof(ContentAlignment), value))
                    throw new InvalidEnumArgumentException("value", (int)value, typeof(ContentAlignment));
                if (m_ImageAlign == value)
                    return;
                m_ImageAlign = value;
                this.Invalidate();
            }
        }


        //RoundCorners
        [Category("Appearance")]
        [DefaultValue(typeof(Corners), "None")]
        [Description("Gets/sets the corners of the control to round.")]
        [Editor(typeof(RoundCornersEditor), typeof(UITypeEditor))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Corners RoundCorners
        {
            get { return m_RoundCorners; }
            set
            {
                if (m_RoundCorners == value)
                    return;
                m_RoundCorners = value;
                this.Invalidate();
            }
        }


        //TextAlign
        [Category("Appearance"), DefaultValue(typeof(ContentAlignment), "MiddleCenter")]
        [Description("The alignment of the text that will be displayed in the face of the control.")]
        public ContentAlignment TextAlign
        {
            get { return m_TextAlign; }
            set
            {
                if (!Enum.IsDefined(typeof(ContentAlignment), value))
                    throw new InvalidEnumArgumentException("value", (int)value, typeof(ContentAlignment));
                if (m_TextAlign == value)
                    return;
                m_TextAlign = value;
                this.Invalidate();
            }
        }


        #endregion

        #region Overriden Methods

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.KeyCode == Keys.Space)
            {
                keyPressed = true;
                m_ButtonState = CustomButtonState.Pressed;
            }
            OnStateChange(EventArgs.Empty);
        }


        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            if (e.KeyCode == Keys.Space)
            {
                if (this.ButtonState == CustomButtonState.Pressed)
                    this.PerformClick();
                keyPressed = false;
                m_ButtonState = CustomButtonState.Focused;
            }
            OnStateChange(EventArgs.Empty);
        }


        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            if (!keyPressed)
                m_ButtonState = CustomButtonState.Hot;
            OnStateChange(EventArgs.Empty);
        }


        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (!keyPressed)
                if (this.IsDefault)
                    m_ButtonState = CustomButtonState.Focused;
                else
                    m_ButtonState = CustomButtonState.Normal;
            OnStateChange(EventArgs.Empty);
        }


        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                this.Focus();
                m_ButtonState = CustomButtonState.Pressed;
            }
            OnStateChange(EventArgs.Empty);
        }


        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            m_ButtonState = CustomButtonState.Focused;
            OnStateChange(EventArgs.Empty);
        }


        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (new Rectangle(Point.Empty, this.Size).Contains(e.X, e.Y) && e.Button == MouseButtons.Left)
                m_ButtonState = CustomButtonState.Pressed;
            else
            {
                if (keyPressed)
                    return;
                m_ButtonState = CustomButtonState.Hot;
            }
            OnStateChange(EventArgs.Empty);
        }


        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            m_ButtonState = CustomButtonState.Focused;
            this.NotifyDefault(true);
        }


        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            if (this.FindForm().Focused)
                this.NotifyDefault(false);
            m_ButtonState = CustomButtonState.Normal;
        }


        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            if (this.Enabled)
                m_ButtonState = CustomButtonState.Normal;
            else
                m_ButtonState = CustomButtonState.Disabled;
            OnStateChange(EventArgs.Empty);
        }


        protected override void OnClick(EventArgs e)
        {
            //Click gets fired before MouseUp which is handy
            if (this.ButtonState == CustomButtonState.Pressed)
            {
                this.Focus();
                this.PerformClick();
            }
        }


        protected override void OnDoubleClick(EventArgs e)
        {
            if (this.ButtonState == CustomButtonState.Pressed)
            {
                this.Focus();
                this.PerformClick();
            }
        }


        protected override bool ProcessMnemonic(char charCode)
        {
            if (IsMnemonic(charCode, this.Text))
            {
                base.OnClick(EventArgs.Empty);
                return true;
            }
            return base.ProcessMnemonic(charCode);
        }


        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            this.Invalidate();
        }


        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            //Simulate Transparency
            GraphicsContainer g = pevent.Graphics.BeginContainer();
            Rectangle translateRect = this.Bounds;
            pevent.Graphics.TranslateTransform(-this.Left, -this.Top);
            PaintEventArgs pe = new PaintEventArgs(pevent.Graphics, translateRect);
            this.InvokePaintBackground(this.Parent, pe);
            this.InvokePaint(this.Parent, pe);
            pevent.Graphics.ResetTransform();
            pevent.Graphics.EndContainer(g);

            pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            Color darkColor;
            Color lightColor;
            Color boardColor;
            if (!this.Enabled)
            {
                darkColor = DisableFillColor;
                lightColor = DisableShadeColor;
                boardColor = DisableButtonBoardColor;
            }
            else if (this.ButtonState == CustomButtonState.Hot)
            {
                darkColor = FillColor;
                lightColor = ShadeColor;
                boardColor = ButtonBoardColor;
            }
            else if (this.ButtonState == CustomButtonState.Pressed)
            {
                if (IsChangeColorOnClick == true)
                {
                    darkColor = FillColorOnClick;
                    lightColor = ShadeColorOnClick;
                    boardColor = ButtonBoardColorOnClick;
                }
                else
                {
                    darkColor = FillColor;
                    lightColor = ShadeColor;
                    boardColor = ButtonBoardColor;
                }
            }
            //else if (this.ButtonState == CustomButtonState.Disabled)
            //{
            //darkColor = DisableFillColor;
            //lightColor = DisableShadeColor;
            //boardColor = DisableButtonBoardColor;
            //}
            else
            {
                lightColor = FillColor;
                darkColor = ShadeColor;
                boardColor = ButtonBoardColor;
            }

            Rectangle r = this.ClientRectangle;
            //System.Drawing.Drawing2D.GraphicsPath path = RoundRectangle(r, this.CornerRadius, this.RoundCorners);
            GraphicsPath path = CustomGUIHelper.RoundRectangle(r, this.CornerRadius, Corners.All);

            LinearGradientBrush paintBrush = new LinearGradientBrush(r, lightColor, darkColor, LinearGradientMode.Vertical);

            //We want a sharp change in the colors so define a Blend for the brush
            Blend b = new Blend();
            b.Positions = new float[] { 0, 0, 0.55F, 1 };
            b.Factors = new float[] { 0, 0, 1, 1 };
            paintBrush.Blend = b;

            //Draw the Button Background
            pevent.Graphics.FillPath(paintBrush, path);
            paintBrush.Dispose();

            //...and border
            Pen drawingPen = new Pen(boardColor);
            pevent.Graphics.DrawPath(drawingPen, path);
            drawingPen.Dispose();

            //Get the Rectangle to be used for Content
            bool inBounds = false;
            //We could use some Math to get this from the radius but I'm 
            //not great at Math so for the example this hack will suffice.
            while (!inBounds && r.Width >= 1 && r.Height >= 1)
            {
                inBounds = path.IsVisible(r.Left, r.Top) &&
                            path.IsVisible(r.Right, r.Top) &&
                            path.IsVisible(r.Left, r.Bottom) &&
                            path.IsVisible(r.Right, r.Bottom);
                r.Inflate(-1, -1);

            }

            contentRect = r;

        }


        protected override void OnPaint(PaintEventArgs e)
        {
            DrawImage(e.Graphics);
            DrawText(e.Graphics);
            if (EnableFocus == true)
                DrawFocus(e.Graphics);
            base.OnPaint(e);
        }


        protected override void OnParentBackColorChanged(EventArgs e)
        {
            base.OnParentBackColorChanged(e);
            this.Invalidate();
        }


        protected override void OnParentBackgroundImageChanged(EventArgs e)
        {
            base.OnParentBackgroundImageChanged(e);
            this.Invalidate();
        }



        #endregion

        #region Internal Draw Methods

        private void DrawImage(Graphics g)
        {
            if (this.ImageList == null || this.ImageIndex == -1)
                return;
            if (this.ImageIndex < 0 || this.ImageIndex >= this.ImageList.Images.Count)
                return;

            Image _Image = this.ImageList.Images[this.ImageIndex];

            Point pt = Point.Empty;

            switch (this.ImageAlign)
            {
                case ContentAlignment.TopLeft:
                    pt.X = contentRect.Left;
                    pt.Y = contentRect.Top;
                    break;

                case ContentAlignment.TopCenter:
                    pt.X = (Width - _Image.Width) / 2;
                    pt.Y = contentRect.Top;
                    break;

                case ContentAlignment.TopRight:
                    pt.X = contentRect.Right - _Image.Width;
                    pt.Y = contentRect.Top;
                    break;

                case ContentAlignment.MiddleLeft:
                    pt.X = contentRect.Left;
                    pt.Y = (Height - _Image.Height) / 2;
                    break;

                case ContentAlignment.MiddleCenter:
                    pt.X = (Width - _Image.Width) / 2;
                    pt.Y = (Height - _Image.Height) / 2;
                    break;

                case ContentAlignment.MiddleRight:
                    pt.X = contentRect.Right - _Image.Width;
                    pt.Y = (Height - _Image.Height) / 2;
                    break;

                case ContentAlignment.BottomLeft:
                    pt.X = contentRect.Left;
                    pt.Y = contentRect.Bottom - _Image.Height;
                    break;

                case ContentAlignment.BottomCenter:
                    pt.X = (Width - _Image.Width) / 2;
                    pt.Y = contentRect.Bottom - _Image.Height;
                    break;

                case ContentAlignment.BottomRight:
                    pt.X = contentRect.Right - _Image.Width;
                    pt.Y = contentRect.Bottom - _Image.Height;
                    break;
            }

            if (this.ButtonState == CustomButtonState.Pressed)
                pt.Offset(1, 1);

            if (this.Enabled)
                this.ImageList.Draw(g, pt, this.ImageIndex);
            else
                ControlPaint.DrawImageDisabled(g, _Image, pt.X, pt.Y, this.BackColor);

        }


        private void DrawText(Graphics g)
        {
            SolidBrush TextBrush = new SolidBrush(this.ForeColor);

            RectangleF R = (RectangleF)contentRect;

            if (!this.Enabled)
                TextBrush.Color = SystemColors.GrayText;

            StringFormat sf = new StringFormat(StringFormatFlags.NoWrap | StringFormatFlags.NoClip);

            if (ShowKeyboardCues)
                sf.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.Show;
            else
                sf.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.Hide;

            switch (this.TextAlign)
            {
                case ContentAlignment.TopLeft:
                    sf.Alignment = StringAlignment.Near;
                    sf.LineAlignment = StringAlignment.Near;
                    break;

                case ContentAlignment.TopCenter:
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Near;
                    break;

                case ContentAlignment.TopRight:
                    sf.Alignment = StringAlignment.Far;
                    sf.LineAlignment = StringAlignment.Near;
                    break;

                case ContentAlignment.MiddleLeft:
                    sf.Alignment = StringAlignment.Near;
                    sf.LineAlignment = StringAlignment.Center;
                    break;

                case ContentAlignment.MiddleCenter:
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Center;
                    break;

                case ContentAlignment.MiddleRight:
                    sf.Alignment = StringAlignment.Far;
                    sf.LineAlignment = StringAlignment.Center;
                    break;

                case ContentAlignment.BottomLeft:
                    sf.Alignment = StringAlignment.Near;
                    sf.LineAlignment = StringAlignment.Far;
                    break;

                case ContentAlignment.BottomCenter:
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Far;
                    break;

                case ContentAlignment.BottomRight:
                    sf.Alignment = StringAlignment.Far;
                    sf.LineAlignment = StringAlignment.Far;
                    break;
            }

            if (this.ButtonState == CustomButtonState.Pressed)
                R.Offset(1, 1);

            if (this.Enabled)
                g.DrawString(this.Text, this.Font, TextBrush, R, sf);
            else
                ControlPaint.DrawStringDisabled(g, this.Text, this.Font, this.BackColor, R, sf);

        }


        private void DrawFocus(Graphics g)
        {
            Rectangle r = contentRect;
            r.Inflate(1, 1);
            if (this.Focused && this.ShowFocusCues && this.TabStop)
                ControlPaint.DrawFocusRectangle(g, r, this.ForeColor, this.BackColor);
        }


        #endregion


        private CustomButtonState currentState;
        private void OnStateChange(EventArgs e)
        {
            //Repaint the button only if the state has actually changed
            if (this.ButtonState == currentState)
                return;
            currentState = this.ButtonState;
            this.Invalidate();
        }


    }

    #region Custom TypeEditor for RoundCorners property

    [PermissionSetAttribute(SecurityAction.LinkDemand, Unrestricted = true)]
    [PermissionSetAttribute(SecurityAction.InheritanceDemand, Unrestricted = true)]
    public class RoundCornersEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        public override Object EditValue(ITypeDescriptorContext context, IServiceProvider provider, Object value)
        {
            if (value != typeof(Corners) || provider == null)
                return value;

            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (edSvc != null)
            {
                CheckedListBox lb = new CheckedListBox();
                lb.BorderStyle = BorderStyle.None;
                lb.CheckOnClick = true;

                lb.Items.Add("TopLeft", (((CustomButton)context.Instance).RoundCorners & Corners.TopLeft) == Corners.TopLeft);
                lb.Items.Add("TopRight", (((CustomButton)context.Instance).RoundCorners & Corners.TopRight) == Corners.TopRight);
                lb.Items.Add("BottomLeft", (((CustomButton)context.Instance).RoundCorners & Corners.BottomLeft) == Corners.BottomLeft);
                lb.Items.Add("BottomRight", (((CustomButton)context.Instance).RoundCorners & Corners.BottomRight) == Corners.BottomRight);

                edSvc.DropDownControl(lb);
                Corners cornerFlags = Corners.None;
                foreach (object o in lb.CheckedItems)
                {
                    cornerFlags = cornerFlags | (Corners)Enum.Parse(typeof(Corners), o.ToString());
                }
                lb.Dispose();
                edSvc.CloseDropDown();
                return cornerFlags;
            }
            return value;
        }

    }

    #endregion

    #endregion

    #region CustomCheckBox

    public class CustomCheckBox : CheckBox
    {
        public CustomCheckBox()
        {
            this.ResizeRedraw = true;
            this.TextAlign = ContentAlignment.MiddleRight;
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
        }
        public override bool AutoSize
        {
            get { return base.AutoSize; }
            set { base.AutoSize = false; }
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            int h = this.ClientSize.Height - 2;
            Rectangle rc = new Rectangle(new Point(0, 1), new Size(h, h));
            ControlPaint.DrawCheckBox(e.Graphics, rc, this.Checked ? ButtonState.Checked : ButtonState.Normal);
        }
    }

    #endregion

    #region MenuStrip

    public class TestColorTable : ProfessionalColorTable
    {
        public override Color MenuItemSelected
        {
            get
            {
                return ColorTranslator.FromHtml("#edf1f4");
            }
        }

        public override Color MenuBorder  //added for changing the menu border
        {
            get { return ColorTranslator.FromHtml("#7b868e"); }
        }

        public override Color MenuItemBorder
        {
            get
            {
                return ColorTranslator.FromHtml("#DBE1E5");
            }
        }

        #region Top Level Item
        public override Color MenuItemPressedGradientBegin
        {
            get
            {
                return ColorTranslator.FromHtml("#edf1f4");
            }
        }

        public override Color MenuItemPressedGradientMiddle
        {
            get
            {
                return ColorTranslator.FromHtml("#edf1f4");
            }
        }

        public override Color MenuItemPressedGradientEnd
        {
            get
            {
                return ColorTranslator.FromHtml("#edf1f4");
            }
        }
        #endregion

    }

    #endregion

    #region Custom Cobmo Box

    public delegate void BNDroppedDownEventHandler(object sender, EventArgs e);
    public delegate void BNDrawItemEventHandler(object sender, DrawItemEventArgs e);
    public delegate void BNMeasureItemEventHandler(object sender, MeasureItemEventArgs e);

    public class ComboboxItem
    {
        public string Text { get; set; }
        public Object Value { get; set; }

        public ComboboxItem(string text, Object value)
        {
            Text = text;
            Value = value;
        }

        public override string ToString()
        {
            return Text;
        }
    }

    public class BNRadius
    {
        private int _topLeft = 0;

        public int TopLeft
        {
            get { return _topLeft; }
            set { _topLeft = value; }
        }

        private int _topRight = 0;

        public int TopRight
        {
            get { return _topRight; }
            set { _topRight = value; }
        }

        private int _bottomLeft = 0;

        public int BottomLeft
        {
            get { return _bottomLeft; }
            set { _bottomLeft = value; }
        }

        private int _bottomRight = 0;

        public int BottomRight
        {
            get { return _bottomRight; }
            set { _bottomRight = value; }
        }
    }

    public class CustomComboBox : ListControl
    {
        #region Variables

        private bool hovered = false;
        private bool pressed = false;
        private bool resize = false;

        private Color _backColor = Color.White;
        private Color _color1 = ColorTranslator.FromHtml("#B5B5B5");
        private Color _color2 = ColorTranslator.FromHtml("#B5B5B5");
        private Color _color3 = ColorTranslator.FromHtml("#B5B5B5");
        private Color _color4 = ColorTranslator.FromHtml("#B5B5B5");
        private BNRadius _radius = new BNRadius();
        private Font _dropDownFont = new Font("Arial", 12, GraphicsUnit.Pixel);
        private int _dropDownHeight = 200;
        private int _dropDownWidth = 0;
        private int _maxDropDownItems = 8;

        private int _selectedIndex = -1;

        private bool _isDroppedDown = false;

        private ComboBoxStyle _dropDownStyle = ComboBoxStyle.DropDownList;

        private Rectangle rectBtn = new Rectangle(0, 0, 1, 1);
        private Rectangle rectContent = new Rectangle(0, 0, 1, 1);

        private ToolStripControlHost _controlHost;
        private ListBox _listBox;
        private ToolStripDropDown _popupControl;
        private TextBox _textBox;

        #endregion




        #region Delegates

        [Category("Behavior"), Description("Occurs when IsDroppedDown changed to True.")]
        public event BNDroppedDownEventHandler DroppedDown;

        [Category("Behavior"), Description("Occurs when the SelectedIndex property changes.")]
        public event EventHandler SelectedIndexChanged;

        [Category("Behavior"), Description("Occurs whenever a particular item/area needs to be painted.")]
        public event BNDrawItemEventHandler DrawItem;

        [Category("Behavior"), Description("Occurs whenever a particular item's height needs to be calculated.")]
        public event BNMeasureItemEventHandler MeasureItem;

        #endregion




        #region Properties

        public Color Color1
        {
            get { return _color1; }
            set { _color1 = value; Invalidate(true); }
        }

        public Color Color2
        {
            get { return _color2; }
            set { _color2 = value; Invalidate(true); }
        }

        public Color Color3
        {
            get { return _color3; }
            set { _color3 = value; Invalidate(true); }
        }

        public Color Color4
        {
            get { return _color4; }
            set { _color4 = value; Invalidate(true); }
        }

        public int DropDownHeight
        {
            get { return _dropDownHeight; }
            set { _dropDownHeight = value; }
        }

        public ListBox.ObjectCollection Items
        {
            get { return _listBox.Items; }
        }

        public Font DropDownFont
        {
            get { return _dropDownFont; }
            set { _dropDownFont = value; }
        }

        public int DropDownWidth
        {
            get { return _dropDownWidth; }
            set { _dropDownWidth = value; }
        }

        public int MaxDropDownItems
        {
            get { return _maxDropDownItems; }
            set { _maxDropDownItems = value; }
        }

        public new object DataSource
        {
            get { return base.DataSource; }
            set
            {
                _listBox.DataSource = value;
                base.DataSource = value;
                OnDataSourceChanged(System.EventArgs.Empty);
            }
        }

        public bool Soreted
        {
            get
            {
                return _listBox.Sorted;
            }
            set
            {
                _listBox.Sorted = value;
            }
        }

        [Category("Behavior"), Description("Indicates whether the code or the OS will handle the drawing of elements in the list.")]
        public DrawMode DrawMode
        {
            get { return _listBox.DrawMode; }
            set
            {
                _listBox.DrawMode = value;
            }
        }

        public ComboBoxStyle DropDownStyle
        {
            get { return _dropDownStyle; }
            set
            {
                _dropDownStyle = value;

                if (_dropDownStyle == ComboBoxStyle.DropDownList)
                {
                    _textBox.Visible = false;
                }
                else
                {
                    _textBox.Visible = true;
                }
                Invalidate(true);
            }
        }

        public new Color BackColor
        {
            get { return _backColor; }
            set
            {
                this._backColor = value;
                _textBox.BackColor = value;
                Invalidate(true);
            }
        }

        public bool IsDroppedDown
        {
            get { return _isDroppedDown; }
            set
            {
                if (_isDroppedDown == true && value == false)
                {
                    if (_popupControl.IsDropDown)
                    {
                        _popupControl.Close();
                    }
                }

                _isDroppedDown = value;

                if (_isDroppedDown)
                {
                    _controlHost.Control.Width = _dropDownWidth;

                    _listBox.Refresh();

                    if (_listBox.Items.Count > 0)
                    {
                        int h = 0;
                        int i = 0;
                        int maxItemHeight = 0;
                        int highestItemHeight = 0;
                        foreach (object item in _listBox.Items)
                        {
                            int itHeight = _listBox.GetItemHeight(i);
                            if (highestItemHeight < itHeight)
                            {
                                highestItemHeight = itHeight;
                            }
                            h = h + itHeight;
                            if (i <= (_maxDropDownItems - 1))
                            {
                                maxItemHeight = h;
                            }
                            i = i + 1;
                        }

                        if (maxItemHeight > _dropDownHeight)
                            _listBox.Height = _dropDownHeight + 13;
                        else
                        {
                            if (maxItemHeight > highestItemHeight)
                                _listBox.Height = maxItemHeight + 13;
                            else
                                _listBox.Height = highestItemHeight + 13;
                        }
                    }
                    else
                    {
                        _listBox.Height = 15;
                    }

                    _popupControl.Show(this, CalculateDropPosition(), ToolStripDropDownDirection.BelowRight);
                }

                Invalidate();
                if (_isDroppedDown)
                    OnDroppedDown(this, EventArgs.Empty);
            }
        }

        public BNRadius Radius
        {
            get { return _radius; }
        }

        #endregion




        #region Constructor
        public CustomComboBox()
        {
            this.ResizeRedraw = true;
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.ContainerControl, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.Selectable, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.UserMouse, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.Selectable, true);
            SetStyle(ControlStyles.DoubleBuffer, true);

            base.BackColor = Color.Transparent;
            _radius.BottomLeft = 2;
            _radius.BottomRight = 2;
            _radius.TopLeft = 2;
            _radius.TopRight = 6;

            this.Height = 30;
            this.Width = 95;

            this.SuspendLayout();
            _textBox = new TextBox();
            _textBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            _textBox.Location = new System.Drawing.Point(3, 4);
            _textBox.Size = new System.Drawing.Size(60, 13);
            _textBox.TabIndex = 0;
            _textBox.WordWrap = false;
            _textBox.Margin = new Padding(0);
            _textBox.Padding = new Padding(0);
            _textBox.TextAlign = HorizontalAlignment.Left;
            this.Controls.Add(_textBox);
            this.ResumeLayout(false);

            AdjustControls();

            _listBox = new ListBox();
            _listBox.Font = DropDownFont;
            _listBox.IntegralHeight = true;
            _listBox.BorderStyle = BorderStyle.FixedSingle;
            _listBox.SelectionMode = SelectionMode.One;
            _listBox.BindingContext = new BindingContext();
            _listBox.Height = DropDownHeight;

            _controlHost = new ToolStripControlHost(_listBox);
            _controlHost.Padding = new Padding(0);
            _controlHost.Margin = new Padding(0);
            _controlHost.AutoSize = false;

            _popupControl = new ToolStripDropDown();
            _popupControl.Padding = new Padding(0);
            _popupControl.Margin = new Padding(0);
            _popupControl.AutoSize = true;
            _popupControl.DropShadowEnabled = false;
            _popupControl.Items.Add(_controlHost);

            _dropDownWidth = this.Width;

            _listBox.MeasureItem += new MeasureItemEventHandler(_listBox_MeasureItem);
            _listBox.DrawItem += new DrawItemEventHandler(_listBox_DrawItem);
            _listBox.MouseClick += new MouseEventHandler(_listBox_MouseClick);
            _listBox.MouseMove += new MouseEventHandler(_listBox_MouseMove);

            _popupControl.Closed += new ToolStripDropDownClosedEventHandler(_popupControl_Closed);

            _textBox.Resize += new EventHandler(_textBox_Resize);
            _textBox.TextChanged += new EventHandler(_textBox_TextChanged);
        }



        #endregion




        #region Overrides

        protected override void OnDataSourceChanged(EventArgs e)
        {
            this.SelectedIndex = 0;
            base.OnDataSourceChanged(e);
        }

        protected override void OnDisplayMemberChanged(EventArgs e)
        {
            _listBox.DisplayMember = this.DisplayMember;
            this.SelectedIndex = this.SelectedIndex;
            base.OnDisplayMemberChanged(e);
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            Invalidate(true);
            base.OnEnabledChanged(e);
        }

        protected override void OnForeColorChanged(EventArgs e)
        {
            _textBox.ForeColor = this.ForeColor;
            base.OnForeColorChanged(e);
        }

        protected override void OnFormatInfoChanged(EventArgs e)
        {
            _listBox.FormatInfo = this.FormatInfo;
            base.OnFormatInfoChanged(e);
        }

        protected override void OnFormatStringChanged(EventArgs e)
        {
            _listBox.FormatString = this.FormatString;
            base.OnFormatStringChanged(e);
        }

        protected override void OnFormattingEnabledChanged(EventArgs e)
        {
            _listBox.FormattingEnabled = this.FormattingEnabled;
            base.OnFormattingEnabledChanged(e);
        }

        public override Font Font
        {
            get
            {
                return base.Font;
            }
            set
            {
                resize = true;
                _textBox.Font = value;
                base.Font = value;
                Invalidate(true);
            }
        }

        protected override void OnControlAdded(ControlEventArgs e)
        {
            e.Control.MouseDown += new MouseEventHandler(Control_MouseDown);
            e.Control.MouseEnter += new EventHandler(Control_MouseEnter);
            e.Control.MouseLeave += new EventHandler(Control_MouseLeave);
            e.Control.GotFocus += new EventHandler(Control_GotFocus);
            e.Control.LostFocus += new EventHandler(Control_LostFocus);
            base.OnControlAdded(e);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            hovered = true;
            this.Invalidate(true);
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            if (!this.RectangleToScreen(this.ClientRectangle).Contains(MousePosition))
            {
                hovered = false;
                Invalidate(true);
            }

            base.OnMouseLeave(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            _textBox.Focus();
            if ((this.RectangleToScreen(rectBtn).Contains(MousePosition) || (DropDownStyle == ComboBoxStyle.DropDownList)))
            {
                pressed = true;
                this.Invalidate(true);
                if (this.IsDroppedDown)
                {
                    this.IsDroppedDown = false;
                }
                this.IsDroppedDown = true;
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            pressed = false;

            if (!this.RectangleToScreen(this.ClientRectangle).Contains(MousePosition))
                hovered = false;
            else
                hovered = true;

            Invalidate();
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (e.Delta < 0)
                this.SelectedIndex = this.SelectedIndex + 1;
            else if (e.Delta > 0)
            {
                if (this.SelectedIndex > 0)
                    this.SelectedIndex = this.SelectedIndex - 1;
            }

            base.OnMouseWheel(e);
        }

        protected override void OnGotFocus(EventArgs e)
        {
            Invalidate(true);
            base.OnGotFocus(e);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            if (!this.ContainsFocus)
            {
                Invalidate();
            }

            base.OnLostFocus(e);
        }

        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            if (SelectedIndexChanged != null)
                SelectedIndexChanged(this, e);

            base.OnSelectedIndexChanged(e);
        }

        protected override void OnValueMemberChanged(EventArgs e)
        {
            _listBox.ValueMember = this.ValueMember;
            this.SelectedIndex = this.SelectedIndex;
            base.OnValueMemberChanged(e);
        }

        protected override void OnResize(EventArgs e)
        {
            if (resize)
            {

                resize = false;
                AdjustControls();

                Invalidate(true);
            }
            else
                Invalidate(true);

            if (DesignMode)
                _dropDownWidth = this.Width;
        }

        public override string Text
        {
            get
            {
                return _textBox.Text;
            }
            set
            {
                _textBox.Text = value;
                base.Text = _textBox.Text;
                OnTextChanged(EventArgs.Empty);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            //content border
            Rectangle rectCont = rectContent;
            rectCont.X += 1;
            rectCont.Y += 1;
            rectCont.Width -= 3;
            rectCont.Height -= 3;
            GraphicsPath pathContentBorder = CreateRoundRectangle(rectCont, Radius.TopLeft, Radius.TopRight, Radius.BottomRight,
                Radius.BottomLeft);

            //button border
            Rectangle rectButton = rectBtn;
            rectButton.X += 1;
            rectButton.Y += 1;
            rectButton.Width -= 3;
            rectButton.Height -= 3;
            GraphicsPath pathBtnBorder = CreateRoundRectangle(rectButton, 0, Radius.TopRight, Radius.BottomRight, 0);
            //GraphicsPath pathBtnBorder = CreateRoundRectangle(rectButton, 10, Radius.TopRight, Radius.BottomRight, 10);

            //outer border
            Rectangle rectOuter = rectContent;
            rectOuter.Width -= 1;
            rectOuter.Height -= 1;
            GraphicsPath pathOuterBorder = CreateRoundRectangle(rectOuter, Radius.TopLeft, Radius.TopRight, Radius.BottomRight,
                Radius.BottomLeft);

            //inner border
            Rectangle rectInner = rectContent;
            rectInner.X += 1;
            rectInner.Y += 1;
            rectInner.Width -= 3;
            rectInner.Height -= 3;
            GraphicsPath pathInnerBorder = CreateRoundRectangle(rectInner, Radius.TopLeft, Radius.TopRight, Radius.BottomRight,
                Radius.BottomLeft);

            //brushes and pens
            Brush brInnerBrush = new LinearGradientBrush(
                new Rectangle(rectInner.X, rectInner.Y, rectInner.Width, rectInner.Height + 1),
                (hovered || IsDroppedDown || ContainsFocus) ? Color4 : Color2, Color.Transparent,
                LinearGradientMode.Vertical);
            Brush brBackground;
            if (this.DropDownStyle == ComboBoxStyle.DropDownList)
            {
                brBackground = new LinearGradientBrush(pathInnerBorder.GetBounds(),
                    Color.FromArgb(IsDroppedDown ? 100 : 255, Color.White),
                    Color.FromArgb(IsDroppedDown ? 255 : 100, BackColor),
                    LinearGradientMode.Vertical);
            }
            else
            {
                brBackground = new SolidBrush(BackColor);
            }
            Pen penOuterBorder = new Pen(Color1, 0);
            Pen penInnerBorder = new Pen(brInnerBrush, 0);
            LinearGradientBrush brButtonLeft = new LinearGradientBrush(rectBtn, ColorTranslator.FromHtml("#BCBCBC"), ColorTranslator.FromHtml("#BCBCBC"), LinearGradientMode.Vertical);
            //ColorBlend blend = new ColorBlend();
            //blend.Colors = new Color[] { Color.Transparent, Color2, Color.Transparent };
            //blend.Positions = new float[] { 0.0f, 0.5f, 1.0f };
            //brButtonLeft.InterpolationColors = blend;
            Pen penLeftButton = new Pen(brButtonLeft, 2);
            Brush brButton = new LinearGradientBrush(pathBtnBorder.GetBounds(),
                Color.FromArgb(100, IsDroppedDown ? Color2 : Color.White),
                    Color.FromArgb(100, IsDroppedDown ? Color.White : Color2),
                    LinearGradientMode.Vertical);

            //draw
            e.Graphics.FillPath(brBackground, pathContentBorder);
            if (DropDownStyle != ComboBoxStyle.DropDownList)
            {
                e.Graphics.FillPath(brButton, pathBtnBorder);
            }
            e.Graphics.DrawPath(penOuterBorder, pathOuterBorder);
            e.Graphics.DrawPath(penInnerBorder, pathInnerBorder);

            //e.Graphics.DrawLine(penLeftButton, rectBtn.Left + 1, rectInner.Top + 1, rectBtn.Left + 1, rectInner.Bottom - 1);
            e.Graphics.DrawLine(penLeftButton, rectBtn.Left - 10, rectInner.Top - 2, rectBtn.Left - 10, rectInner.Bottom + 2);

            //Glimph
            Rectangle rectGlimph = rectButton;
            rectButton.Width -= 4;
            e.Graphics.TranslateTransform(rectGlimph.Left + rectGlimph.Width / 6f, rectGlimph.Top + rectGlimph.Height / 2f);
            GraphicsPath path = new GraphicsPath();
            PointF[] points = new PointF[3];
            points[0] = new PointF(-14 / 2.0f, -7 / 2.0f);
            points[1] = new PointF(14 / 2.0f, -7 / 2.0f);
            points[2] = new PointF(0, 14 / 2.0f);
            path.AddLine(points[0], points[1]);
            path.AddLine(points[1], points[2]);
            path.CloseFigure();
            e.Graphics.RotateTransform(0);

            SolidBrush br = new SolidBrush(Enabled ? Color.Gray : Color.Gainsboro);
            e.Graphics.FillPath(br, path);
            e.Graphics.ResetTransform();
            br.Dispose();
            path.Dispose();


            //text
            if (DropDownStyle == ComboBoxStyle.DropDownList)
            {
                StringFormat sf = new StringFormat(StringFormatFlags.NoWrap);
                sf.Alignment = StringAlignment.Near;

                Rectangle rectText = _textBox.Bounds;
                rectText.Offset(-3, 0);

                SolidBrush foreBrush = new SolidBrush(ForeColor);
                if (Enabled)
                {
                    //e.Graphics.DrawString(_textBox.Text, this.Font, foreBrush, rectText.Location);
                    e.Graphics.DrawString(_textBox.Text, this.Font, foreBrush, rectText.Location);
                }
                else
                {
                    ControlPaint.DrawStringDisabled(e.Graphics, _textBox.Text, Font, BackColor, rectText, sf);
                }
            }
            /*
            Dim foreBrush As SolidBrush = New SolidBrush(color)
            If (enabled) Then
                g.DrawString(text, font, foreBrush, rect, sf)
            Else
                ControlPaint.DrawStringDisabled(g, text, font, backColor, _
                     rect, sf)
            End If
            foreBrush.Dispose()*/


            pathContentBorder.Dispose();
            pathOuterBorder.Dispose();
            pathInnerBorder.Dispose();
            pathBtnBorder.Dispose();

            penOuterBorder.Dispose();
            penInnerBorder.Dispose();
            penLeftButton.Dispose();

            brBackground.Dispose();
            brInnerBrush.Dispose();
            brButtonLeft.Dispose();
            brButton.Dispose();
        }

        #endregion




        #region ListControlOverrides

        public override int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                if (_listBox != null)
                {
                    if (_listBox.Items.Count == 0)
                        return;

                    if ((this.DataSource != null) && value == -1)
                        return;

                    if (value <= (_listBox.Items.Count - 1) && value >= -1)
                    {
                        _listBox.SelectedIndex = value;
                        _selectedIndex = value;
                        string listBoxText = _listBox.GetItemText(_listBox.SelectedItem);

                        Font font = _textBox.Font;
                        Graphics g = _textBox.CreateGraphics();
                        int maxWidth = _textBox.Width - 40;

                        int newWidth = (int)g.MeasureString(listBoxText, font).Width;

                        if (newWidth > maxWidth)
                        {
                            int perCharWidth = newWidth / listBoxText.Length;
                            int stringLength = maxWidth / perCharWidth;
                            listBoxText = listBoxText.Substring(0,
                                (listBoxText.Length <= 5 ? listBoxText.Length :
                                    (listBoxText.Length > stringLength ? stringLength : listBoxText.Length))
                                );
                        }
                        _textBox.Text = listBoxText;

                        OnSelectedIndexChanged(EventArgs.Empty);
                    }
                }
            }
        }

        public object SelectedItem
        {
            get { return _listBox.SelectedItem; }
            set
            {
                _listBox.SelectedItem = value;
                this.SelectedIndex = _listBox.SelectedIndex;
            }
        }

        public new object SelectedValue
        {
            get { return base.SelectedValue; }
            set
            {
                base.SelectedValue = value;
            }
        }

        protected override void RefreshItem(int index)
        {
            //throw new Exception("The method or operation is not implemented.");
        }

        protected override void RefreshItems()
        {
            //base.RefreshItems();
        }

        protected override void SetItemCore(int index, object value)
        {
            //base.SetItemCore(index, value);
        }

        protected override void SetItemsCore(System.Collections.IList items)
        {
            //throw new Exception("The method or operation is not implemented.");
        }

        #endregion




        #region NestedControlsEvents

        void Control_LostFocus(object sender, EventArgs e)
        {
            OnLostFocus(e);
        }

        void Control_GotFocus(object sender, EventArgs e)
        {
            OnGotFocus(e);
        }

        void Control_MouseLeave(object sender, EventArgs e)
        {
            OnMouseLeave(e);
        }

        void Control_MouseEnter(object sender, EventArgs e)
        {
            OnMouseEnter(e);
        }

        void Control_MouseDown(object sender, MouseEventArgs e)
        {
            OnMouseDown(e);
        }


        void _listBox_MouseMove(object sender, MouseEventArgs e)
        {
            int i;
            for (i = 0; i < (_listBox.Items.Count); i++)
            {
                if (_listBox.GetItemRectangle(i).Contains(_listBox.PointToClient(MousePosition)))
                {
                    _listBox.SelectedIndex = i;
                    return;
                }
            }
        }

        void _listBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (_listBox.Items.Count == 0)
            {
                return;
            }

            if (_listBox.SelectedItems.Count != 1)
            {
                return;
            }

            this.SelectedIndex = _listBox.SelectedIndex;

            if (DropDownStyle == ComboBoxStyle.DropDownList)
            {
                this.Invalidate(true);
            }

            IsDroppedDown = false;
        }

        void _listBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index >= 0)
            {
                if (DrawItem != null)
                {
                    DrawItem(this, e);
                }
            }
        }

        void _listBox_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            if (MeasureItem != null)
            {
                MeasureItem(this, e);
            }
        }


        void _popupControl_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            _isDroppedDown = false;
            pressed = false;
            if (!this.RectangleToScreen(this.ClientRectangle).Contains(MousePosition))
            {
                hovered = false;
            }
            Invalidate(true);
        }



        void _textBox_Resize(object sender, EventArgs e)
        {
            this.AdjustControls();
        }

        void _textBox_TextChanged(object sender, EventArgs e)
        {
            OnTextChanged(e);
        }

        #endregion




        #region PrivateMethods

        private void AdjustControls()
        {
            this.SuspendLayout();

            resize = true;
            _textBox.Top = 4;
            _textBox.Left = 5;
            this.Height = _textBox.Top + _textBox.Height + _textBox.Top;

            rectBtn =
                    new System.Drawing.Rectangle(this.ClientRectangle.Width - 18,
                    this.ClientRectangle.Top, 18, _textBox.Height + 2 * _textBox.Top);


            _textBox.Width = rectBtn.Left - 1 - _textBox.Left;

            rectContent = new Rectangle(ClientRectangle.Left, ClientRectangle.Top,
                ClientRectangle.Width, _textBox.Height + 2 * _textBox.Top);

            this.ResumeLayout();

            Invalidate(true);
        }

        private Point CalculateDropPosition()
        {
            Point point = new Point(0, this.Height);
            if ((this.PointToScreen(new Point(0, 0)).Y + this.Height + _controlHost.Height) > Screen.PrimaryScreen.WorkingArea.Height)
            {
                point.Y = -this._controlHost.Height - 7;
            }
            return point;
        }

        private Point CalculateDropPosition(int myHeight, int controlHostHeight)
        {
            Point point = new Point(0, myHeight);
            if ((this.PointToScreen(new Point(0, 0)).Y + this.Height + controlHostHeight) > Screen.PrimaryScreen.WorkingArea.Height)
            {
                point.Y = -controlHostHeight - 7;
            }
            return point;
        }

        #endregion




        #region VirtualMethods

        public virtual void OnDroppedDown(object sender, EventArgs e)
        {
            if (DroppedDown != null)
            {
                DroppedDown(this, e);
            }
        }

        #endregion

        #region Render

        public static GraphicsPath CreateRoundRectangle(Rectangle rectangle, int topLeftRadius, int topRightRadius, int bottomRightRadius, int bottomLeftRadius)
        {
            GraphicsPath path = new GraphicsPath();
            int l = rectangle.Left;
            int t = rectangle.Top;
            int w = rectangle.Width;
            int h = rectangle.Height;

            if (topLeftRadius > 0)
            {
                path.AddArc(l, t, topLeftRadius * 2, topLeftRadius * 2, 180, 90);
            }
            path.AddLine(l + topLeftRadius, t, l + w - topRightRadius, t);
            if (topRightRadius > 0)
            {
                //path.AddArc(l + w - topRightRadius * 2, t, topRightRadius * 2, topRightRadius * 2, 270, 90);
                path.AddArc(l + w - topRightRadius * 1.5f, t, topRightRadius * 2, topRightRadius * 2, 270, 90);
            }
            //path.AddLine(l + w, t + topRightRadius, l + w, t + h - bottomRightRadius);
            path.AddLine(l + w, t + topRightRadius / 2.5f, l + w, t + h - bottomRightRadius);
            if (bottomRightRadius > 0)
            {
                path.AddArc(l + w - bottomRightRadius * 2, t + h - bottomRightRadius * 2, bottomRightRadius * 2, bottomRightRadius * 2, 0, 90);
            }
            path.AddLine(l + w - bottomRightRadius, t + h, l + bottomLeftRadius, t + h);
            if (bottomLeftRadius > 0)
            {
                path.AddArc(l, t + h - bottomLeftRadius * 2, bottomLeftRadius * 2, bottomLeftRadius * 2, 90, 90);
            }
            path.AddLine(l, t + h - bottomLeftRadius, l, t + topLeftRadius);
            path.CloseFigure();
            return path;
        }

        #endregion
    }

    #endregion
}
