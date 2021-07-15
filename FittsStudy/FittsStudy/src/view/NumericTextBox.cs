using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace FittsStudy
{
    /// <summary>
    /// Implements a double-precision floating-point numeric text box control. 
    /// </summary>
    [ToolboxBitmap(typeof(System.Windows.Forms.TextBox))]
    public partial class NumericTextBox : System.Windows.Forms.TextBox
    {
        #region Fields

        private uint _precision;
        private double _minimum;
        private double _maximum;

        #endregion

        /// <summary>
        /// Constructs a numeric text box control.
        /// </summary>
        public NumericTextBox()
        {
            InitializeComponent();
            MyInitialization();
        }

        /// <summary>
        /// Constructs a numeric text box control within the given container.
        /// </summary>
        /// <param name="container">The container in which to construct the numeric text box.</param>
        public NumericTextBox(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
            MyInitialization();
        }

        /// <summary>
        /// Common constructor initialization routine.
        /// </summary>
        private void MyInitialization()
        {
            // properties we set
            base.TextAlign = HorizontalAlignment.Right;
            base.WordWrap = false;

            // member variables
            _maximum = +10000d;
            _minimum = -10000d;
            _precision = 2;

            // set initial value
            base.Text = "0.00";
        }

        #region Event Overrides

        /// <summary>
        /// Catches invalid keypress characters in the numeric text box and refuses to display them.
        /// </summary>
        /// <param name="e">The arguments for this event.</param>
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
            bool validKey = (
                ('0' <= e.KeyChar && e.KeyChar <= '9') ||
                e.KeyChar == '-' ||
                e.KeyChar == '.' ||
                e.KeyChar == ',' ||
                e.KeyChar == 8   ||     // Backspace key
                e.KeyChar == 22         // Control key -- for Copy and Paste
                );
            e.Handled = !validKey;
        }

        /// <summary>
        /// Catches when the numeric text box loses focus and enforces a properly formatted numeric string.
        /// If the string is improperly formatted, then the string is reverted to zero.
        /// </summary>
        /// <param name="e">The arguments for this event.</param>
        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            double d;
            bool validParse = double.TryParse(base.Text, out d);
            if (validParse)
            {
                base.Text = UpdateWith(base.Text); // enforce min, max, precision
            }
            else
            {
                base.Text = UpdateWith("0.00"); // revert if not a valid parse
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the text in this numeric text box.
        /// </summary>
        [System.ComponentModel.Category("Numeric"),
        RefreshProperties(RefreshProperties.All)]
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = UpdateWith(value); // enforce min, max, precision
            }
        }

        /// <summary>
        /// Gets or sets the double-precision floating-point value of the text in this numeric text box.
        /// </summary>
        [System.ComponentModel.Category("Numeric"),
        RefreshProperties(RefreshProperties.All)]
        public double Value
        {
            get
            {
                double d;
                bool success = double.TryParse(base.Text, out d);
                return (success ? d : double.NaN);
            }
            set
            {
                base.Text = UpdateWith(value.ToString()); // enforce min, max, precision
            }
        }

        /// <summary>
        /// Defines the minimum acceptable value for this numeric text box. This value
        /// must be less than or equal to the current <b>Maximum</b>.
        /// </summary>
        [System.ComponentModel.Category("Numeric"),
        RefreshProperties(RefreshProperties.All)]
        public double Minimum
        {
            get
            {
                return _minimum;
            }
            set
            {
                if (value < _maximum)
                {
                    _minimum = value;
                    base.Text = UpdateWith(base.Text);
                }
            }
        }

        /// <summary>
        /// Defines the minimum acceptable value for this numeric text box. This value
        /// must be greater than or equal to the current <b>Minimum</b>.
        /// </summary>
        [System.ComponentModel.Category("Numeric"),
        RefreshProperties(RefreshProperties.All)]
        public double Maximum
        {
            get
            {
                return _maximum;
            }
            set
            {
                if (value > _minimum)
                {
                    _maximum = value;
                    base.Text = UpdateWith(base.Text);
                }
            }
        }

        /// <summary>
        /// Gets or sets the maximum allowed precision for this numeric text box.
        /// </summary>
        [System.ComponentModel.Category("Numeric"),
        RefreshProperties(RefreshProperties.All)]
        public uint Precision
        {
            get
            {
                return _precision;
            }
            set
            {
                _precision = value;
                base.Text = UpdateWith(base.Text);
            }
        }

        /// <summary>
        /// Enforces formatting, a minimum, a maximum, and precision requirements for the
        /// numeric text box.
        /// </summary>
        private string UpdateWith(string s)
        {
            double d;
            bool success = double.TryParse(s, out d);
            if (success)
            {
                if (d < _minimum)
                    d = _minimum;
                else if (d > _maximum)
                    d = _maximum;
                string format = String.Format("n{0}", _precision);
                return d.ToString(format);
            }
            else // not a parsable double -- return the old text
            {
                return base.Text;
            }
        }

        #endregion

    }
}
